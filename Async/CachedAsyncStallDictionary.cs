///////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2025 Henry de Jongh
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
////////////////////// https://github.com/Henry00IS/CSharp ////////// http://00laboratories.com/ //

namespace OOLaboratories.Async
{
    /// <summary>
    /// <see cref="CachedAsyncStallDictionary{T}"/> provides a thread-safe mechanism to serialize
    /// asynchronous operations of type <typeparamref name="T"/> on a per-key basis, where the key
    /// is of type <typeparamref name="K"/>. It ensures that only one instance of the provided async
    /// work runs at a time for each unique key. Subsequent calls to <see cref="StallAsync"/> for
    /// the same key await the completion of the currently running <see cref="Task{T}"/>. Operations
    /// for different keys run concurrently. Once a task completes successfully for a key, it caches
    /// the result and returns it immediately for future calls to that key. On failure or
    /// cancellation, the entry for that key is removed from the internal dictionary to free memory.
    /// This is useful for throttling concurrent async operations per category, such as
    /// rate-limiting API calls grouped by endpoint or user, with per-key memoization on success,
    /// such as rate-limiting API calls, without blocking threads or retaining memory for
    /// unsuccessful idle keys.
    /// </summary>
    /// <typeparam name="K">The type of the key used to identify and serialize operations.</typeparam>
    /// <typeparam name="T">The return type of the async work.</typeparam>
    public class CachedAsyncStallDictionary<K, T> where K : notnull
    {
        /// <summary>
        /// The dictionary of currently active tasks, keyed by <typeparamref name="K"/>. Each entry
        /// holds the running <see cref="Task{T}"/> for that key. Completed tasks are removed
        /// promptly. This field is shared across calls and protected by the <see cref="mutex"/>.
        /// </summary>
        private readonly Dictionary<K, Task<T>> currents = new Dictionary<K, Task<T>>();

        /// <summary>
        /// The dictionary of cached successful tasks, keyed by <typeparamref name="K"/>. Each entry
        /// holds the completed successful <see cref="Task{T}"/> for that key, returned immediately
        /// for future calls. This field is shared across calls and protected by the <see cref="mutex"/>.
        /// </summary>
        private readonly Dictionary<K, Task<T>> caches = new Dictionary<K, Task<T>>();

        /// <summary>
        /// A private mutex object used for locking to ensure thread-safe access to the <see
        /// cref="currents"/> and <see cref="caches"/> dictionaries. This prevents concurrent
        /// modifications when multiple threads call <see cref="StallAsync"/> simultaneously.
        /// </summary>
        private readonly object mutex = new object();

        /// <summary>
        /// Executes the provided async <paramref name="work"/> function for the given <paramref
        /// name="key"/>, but serializes it per key so only one runs at a time for that key. If a
        /// successful result is cached for the key, it returns that immediately. If no task is
        /// running for the key, it starts a new one using the key as input to the work. If a task
        /// is already running, it returns that task, causing the caller to await its completion
        /// (stalling). The method is fully async-friendly and propagates faults or cancellations to
        /// all awaiters for that key. After success, the key's cache is set and persists; after
        /// failure or cancellation, the key's entry is removed to free memory and allow retries.
        /// </summary>
        /// <param name="key">The key identifying the serialized operation.</param>
        /// <param name="work">
        /// The asynchronous function to execute, taking the <paramref name="key"/> and returning a
        /// <see cref="Task{T}"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task{T}"/> representing the work for the key: the cached success if
        /// available, or the new or existing shared one.
        /// </returns>
        public Task<T> StallAsync(K key, Func<K, Task<T>> work)
        {
            // acquire the lock on mutex to protect the critical section: checking and updating 'current' and 'caches'.
            // this ensures atomicity, preventing races where two threads might both see null and start tasks.
            lock (mutex)
            {
                // check if a successful result is cached for the key; if so, return it immediately without running anything.
                // this memoizes the success state for all future calls to this key.
                if (caches.TryGetValue(key, out var cached))
                {
                    return cached;
                }

                // check if there is an active task for the key.
                if (currents.TryGetValue(key, out var current))
                {
                    // if present and not completed, share the existing one to serialize.
                    if (!current.IsCompleted)
                    {
                        // a task is already running, so return the existing 'current' task.
                        // this causes the caller to implicitly await it, serializing the operations for this key.
                        // faults or cancellations in the shared task propagate to all awaiters.
                        return current;
                    }
                    else
                    {
                        // we are executing before the continuation handler was able to handle the cache.
                        // on successful completion, cache the task for immediate future returns to this key.
                        // this remembers the successful state and prevents further executions for this key.
                        if (current.IsCompletedSuccessfully)
                        {
                            caches[key] = current;

                            // remove from 'currents', as we confirmed completion, to free memory.
                            currents.Remove(key);

                            // return the success state.
                            return current;
                        }
                    }
                }

                // no active task: either absent or completed (unsuccessfully).
                // remove the entry if present to ensure cleanup (completed tasks shouldn't linger in 'currents').
                currents.Remove(key);

                // create a new task by scheduling the user-provided 'work' with the key on the thread pool.
                // task.run unwraps the func<k, task<t>> and returns the inner task<t>.
                var task = Task.Run(() => work(key));

                // assign the new task to the dictionary under the lock, making it visible to other threads.
                // this marks that a task is now active for this key, so future calls will await it.
                currents[key] = task;

                // attach a continuation to the new task that runs after it completes in any state.
                // the continuation handles post-completion logic: caching on success, always removing from 'currents'.
                // it uses execute_synchronously to run inline on the completing thread, minimizing overhead.
                task.ContinueWith(t =>
                {
                    // re-acquire the lock in the continuation to safely update 'currents' and 'caches'.
                    // this protects against concurrent completions or other calls modifying the dictionaries.
                    lock (mutex)
                    {
                        // verify that the 'key' still points to this exact task 't' in 'currents'.
                        // this idempotent check avoids interfering if another continuation already handled it.
                        if (currents.TryGetValue(key, out var current) && current == t)
                        {
                            // on successful completion, cache the task for immediate future returns to this key.
                            // this remembers the successful state and prevents further executions for this key.
                            if (t.IsCompletedSuccessfully)
                            {
                                caches[key] = t;
                            }

                            // remove from 'currents' unconditionally, as this continuation confirms completion.
                            // on success, it's now in 'caches'; but on fault or cancel, the key is fully freed for retry.
                            // this happens regardless of completion type.
                            currents.Remove(key);
                        }
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);

                // return the new task to the caller, who can await it for the result.
                // under the lock, this ensures the caller gets the freshly assigned task.
                return task;
            }
        }

        /// <summary>
        /// Asynchronously clears the cached successful result for the given <paramref name="key"/>,
        /// if present. If a task is currently running for the key, awaits its completion before
        /// final cleanup to avoid duplicate executions or incomplete state. This does not cancel or
        /// interrupt running tasks. If the task that was awaited is no longer in cache then the
        /// cache is not cleared (i.e. older calls do not affect newer calls).
        /// <para>
        /// Warning: Do not call this inside of the <see cref="StallAsync"/> callback function, as
        /// that will cause a deadlock (awaiting the current task inside of the current task; it can
        /// never complete).
        /// </para>
        /// </summary>
        /// <param name="key">The key whose cache to clear.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous clear operation.</returns>
        public async Task ClearCacheAsync(K key)
        {
            // find the current task associated with this key (it cannot be in cache when found).
            Task<T>? task = null;
            lock (mutex)
            {
                currents.TryGetValue(key, out task);

                // clear the cache if there is no current task.
                if (task is null)
                {
                    caches.Remove(key);
                    return; // we are done.
                }
            }

            // now that we exited the lock statement, other threads could have changed everything.

            // we await the task that was current at the time of this function call.
            try
            {
                await task;
            }
            catch
            {
                // swallow faults from the awaited task as we are just synchronizing.
            }

            // acquire another lock on the dictionaries.
            lock (mutex)
            {
                // ensure the cached task is still the task we awaited:
                if (caches.TryGetValue(key, out var cache) && cache == task)
                    caches.Remove(key);

                // so that we do not clear the cache of future tasks.
            }
        }
    }
}