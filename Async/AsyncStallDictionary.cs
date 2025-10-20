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
    /// <see cref="AsyncStallDictionary{K, T}"/> provides a thread-safe mechanism to serialize
    /// asynchronous operations of type <typeparamref name="T"/> on a per-key basis, where the key
    /// is of type <typeparamref name="K"/>. It ensures that only one instance of the provided async
    /// work runs at a time for each unique key. Subsequent calls to <see cref="StallAsync"/> for
    /// the same key await the completion of the currently running <see cref="Task{T}"/>. Operations
    /// for different keys run concurrently. Once a task for a key completes (success, fault, or
    /// cancellation), the entry for that key is removed from the internal dictionary to free
    /// memory. This is useful for throttling concurrent async operations per category, such as
    /// rate-limiting API calls grouped by endpoint or user, without blocking threads or retaining
    /// memory for idle keys.
    /// </summary>
    /// <typeparam name="K">The type of the key used to identify and serialize operations.</typeparam>
    /// <typeparam name="T">The return type of the async work.</typeparam>
    public class AsyncStallDictionary<K, T> where K : notnull
    {
        /// <summary>
        /// The dictionary of currently active tasks, keyed by <typeparamref name="K"/>. Each entry
        /// holds the running <see cref="Task{T}"/> for that key. Completed tasks are removed
        /// promptly. This field is shared across calls and protected by the <see cref="mutex"/>.
        /// </summary>
        private readonly Dictionary<K, Task<T>> currents = new Dictionary<K, Task<T>>();

        /// <summary>
        /// A private mutex object used for locking to ensure thread-safe access to the <see
        /// cref="currents"/> dictionary. This prevents concurrent modifications when multiple
        /// threads call <see cref="StallAsync"/> simultaneously.
        /// </summary>
        private readonly object mutex = new object();

        /// <summary>
        /// Executes the provided async <paramref name="work"/> function for the given <paramref
        /// name="key"/>, but serializes it per key so only one runs at a time for that key. If no
        /// task is running for the key, it starts a new one using the key as input to the work. If
        /// a task is already running, it returns that task, causing the caller to await its
        /// completion (stalling). If a completed task is found, it is discarded, and a new one is
        /// started. The method is fully async-friendly and propagates faults or cancellations to
        /// all awaiters for that key. After completion, the key's entry is removed to free memory.
        /// </summary>
        /// <param name="key">The key identifying the serialized operation.</param>
        /// <param name="work">
        /// The asynchronous function to execute, taking the <paramref name="key"/> and returning a
        /// <see cref="Task{T}"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task{T}"/> representing the work for the key, either new or the existing
        /// shared one.
        /// </returns>
        public Task<T> StallAsync(K key, Func<K, Task<T>> work)
        {
            // acquire the lock on mutex to protect the critical section: checking, updating, and potentially removing from 'tasks'.
            // this ensures atomicity, preventing races where multiple threads might both see no active task and start duplicates.
            lock (mutex)
            {
                // check if there is an active (incomplete) task for the key.
                // if present and not completed, share the existing one to serialize.
                if (currents.TryGetValue(key, out var current) && !current.IsCompleted)
                {
                    // a task is already running, so return the existing 'current' task.
                    // this causes the caller to implicitly await it, serializing the operations for this key.
                    // faults or cancellations in the shared task propagate to all awaiters.
                    return current;
                }
                else
                {
                    // no active task: either absent or completed.
                    // remove the entry if present to ensure cleanup.
                    currents.Remove(key);

                    // create a new task by scheduling the user-provided 'work' with the key on the thread pool.
                    // task.run unwraps the func<k, task<t>> and returns the inner task<t>.
                    var task = Task.Run(() => work(key));

                    // assign the new task to the dictionary under the lock, making it visible to other threads.
                    // this marks that a task is now active for this key, so future calls will await it.
                    currents[key] = task;

                    // attach a continuation to the new task that runs after it completes in any state.
                    // the continuation handles cleanup: removing the key's entry from the dictionary to free memory.
                    // it uses execute_synchronously to run inline on the completing thread, minimizing overhead.
                    task.ContinueWith(t =>
                    {
                        // re-acquire the lock in the continuation to safely remove from the dictionary.
                        // this protects against concurrent completions or other calls modifying the dictionary.
                        lock (mutex)
                        {
                            // verify that the 'key' still points to this exact task 't' in 'currents'.
                            // this idempotent check avoids resetting if another continuation already cleared it.
                            if (currents.TryGetValue(key, out var current) && current == t)
                            {
                                // remove the entry for the key unconditionally, as this continuation confirms completion.
                                // this happens regardless of completion type: success, fault, or cancellation.
                                currents.Remove(key);
                            }
                        }
                    }, TaskContinuationOptions.ExecuteSynchronously);

                    // return the new task to the caller, who can await it for the result.
                    // under the lock, this ensures the caller gets the freshly assigned task.
                    return task;
                }
            }
        }
    }
}