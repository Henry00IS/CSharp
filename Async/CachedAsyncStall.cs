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
    /// <see cref="CachedAsyncStall{T}"/> provides a thread-safe mechanism to serialize asynchronous
    /// operations of type <typeparamref name="T"/>. It ensures that only one instance of the
    /// provided async work runs at a time. Subsequent calls to <see cref="StallAsync"/> await the
    /// completion of the currently running <see cref="Task"/> instead of starting a new one. Once
    /// the task completes successfully, it caches the result and returns it immediately for future
    /// calls. On failure or cancellation, the slot is freed for the next call to retry. This is
    /// useful for throttling concurrent async operations with memoization on success, such as
    /// rate-limiting API calls, without blocking threads.
    /// </summary>
    public class CachedAsyncStall<T>
    {
        /// <summary>
        /// The current running task, or null if none is active. This field is shared across calls
        /// and protected by the <see cref="mutex"/>.
        /// </summary>
        private Task<T>? current;

        /// <summary>
        /// The cached successful task, or null if no success yet. Once set, it is returned immediately.
        /// </summary>
        private Task<T>? successCache;

        /// <summary>
        /// A private mutex object used for locking to ensure thread-safe access to the <see
        /// cref="current"/> field. This prevents concurrent modifications when multiple threads
        /// call <see cref="StallAsync"/> simultaneously.
        /// </summary>
        private readonly object mutex = new object();

        /// <summary>
        /// Executes the provided async <paramref name="work"/> function, but serializes it so only
        /// one runs at a time. If a successful result is cached, it returns that immediately. If no
        /// task is running, it starts a new one. If a task is already running, it returns that
        /// task, causing the caller to await its completion (stalling). The method is fully
        /// async-friendly and propagates faults or cancellations to all awaiters.
        /// </summary>
        /// <param name="work">The asynchronous function to execute, returning a <see cref="Task{T}"/>.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> representing the work: the cached success if available, or the
        /// new or existing shared one.
        /// </returns>
        public Task<T> StallAsync(Func<Task<T>> work)
        {
            // acquire the lock on mutex to protect the critical section: checking and updating 'current' and 'successcache'.
            // this ensures atomicity, preventing races where two threads might both see null and start tasks.
            lock (mutex)
            {
                // check if a successful result is cached; if so, return it immediately without running anything.
                // this memoizes the success state for all future calls.
                if (successCache != null)
                {
                    return successCache;
                }

                // check if no task is currently running by testing if 'current' is null.
                // this is the condition to start fresh work; otherwise, share the existing one.
                if (current == null)
                {
                    // create a new task by scheduling the user-provided 'work' on the thread pool.
                    // task.run unwraps the func<task<t>> and returns the inner task<t>.
                    var task = Task.Run(work);

                    // assign the new task to 'current' under the lock, making it visible to other threads.
                    // this marks that a task is now active, so future calls will await it.
                    current = task;

                    // attach a continuation to the new task that runs after it completes in any state.
                    // the continuation handles cleanup: resetting 'current' to null to free the slot.
                    // it uses execute_synchronously to run inline on the completing thread, minimizing overhead.
                    task.ContinueWith(t =>
                    {
                        // re-acquire the lock in the continuation to safely check and update 'current' and 'successcache'.
                        // this protects against concurrent completions or other calls modifying the field.
                        lock (mutex)
                        {
                            // verify that 'current' still points to this exact task 't'.
                            // this idempotent check avoids resetting if another continuation already cleared it.
                            // it's a safeguard for rare races, though unlikely with the synchronous option.
                            if (current == t)
                            {
                                // on successful completion, cache the task for immediate future returns.
                                // this remembers the successful state and prevents further executions.
                                if (t.IsCompletedSuccessfully)
                                {
                                    successCache = t;
                                }

                                // reset 'current' to null, allowing the next call to start a new task (only if not cached).
                                // this happens regardless of completion type: success (after caching), fault, or cancellation.
                                current = null;
                            }
                        }
                    }, TaskContinuationOptions.ExecuteSynchronously);

                    // return the new task to the caller, who can await it for the result.
                    // under the lock, this ensures the caller gets the freshly assigned task.
                    return task;
                }
                else
                {
                    // a task is already running, so return the existing 'current' task.
                    // this causes the caller to implicitly await it, serializing the operations.
                    // faults or cancellations in the shared task propagate to all awaiters.
                    return current;
                }
            }
        }
    }
}