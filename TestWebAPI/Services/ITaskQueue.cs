namespace TestWebAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The background task queue.
    /// Source Article: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2&tabs=visual-studio
    /// </summary>
    public interface ITaskQueue
    {
        /// <summary>
        /// The queue background work item.
        /// </summary>
        /// <param name="workItem">
        /// The work item.
        /// </param>
        void AddWorkItem(Func<CancellationToken, DbContext, Task> workItem);

        /// <summary>
        /// The dequeue async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Func<CancellationToken, DbContext, Task>> DequeueWorkItemAsync(
            CancellationToken cancellationToken);      
    }
}