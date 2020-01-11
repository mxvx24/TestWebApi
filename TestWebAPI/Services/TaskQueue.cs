namespace TestWebAPI.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The background task queue.
    /// </summary>
    public class TaskQueue : ITaskQueue
    {
        /// <summary>
        /// The _work items.
        /// </summary>
        private readonly ConcurrentQueue<Func<CancellationToken, DbContext, Task>> workItems =
            new ConcurrentQueue<Func<CancellationToken, DbContext, Task>>();

        /// <summary>
        /// SemaphoreSlim - Represents a lightweight alternative to Semaphore that limits the
        /// number of threads that can access a resource or pool of resources concurrently.
        /// </summary>
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);

        /// <summary>
        /// The queue background work item.
        /// </summary>
        /// <param name="workItem">
        /// The work item.
        /// </param>
        public void QueueWorkItem(Func<CancellationToken, DbContext, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            this.workItems.Enqueue(workItem);

            // Releases the SemaphoreSlim object once.
            this.signal.Release();
        }

        /// <summary>
        /// The dequeue async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Func<CancellationToken, DbContext, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            // Blocks the current thread until it can enter the SemaphoreSlim.
            await this.signal.WaitAsync(cancellationToken);
            this.workItems.TryDequeue(out Func<CancellationToken, DbContext, Task> workItem);

            return workItem;
        }
    }
}
