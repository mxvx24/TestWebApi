namespace TestWebAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using TestWebApi.Data.Contexts;

    /// <summary>
    /// The queued hosted service.
    /// Source Article: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2&tabs=visual-studio
    /// </summary>
    public class QueuedHostedService : IHostedService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The shutdown.
        /// </summary>
        private readonly CancellationTokenSource shutdown = new CancellationTokenSource();

        /// <summary>
        /// The services.
        /// </summary>
        private readonly IServiceProvider services;

        /// <summary>
        /// The background task.
        /// </summary>
        private Task backgroundTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueuedHostedService"/> class.
        /// </summary>
        /// <param name="taskQueue">
        /// The task queue.
        /// </param>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        public QueuedHostedService(ITaskQueue taskQueue, IServiceProvider services, ILoggerFactory loggerFactory)
        {
            this.TaskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.logger = loggerFactory.CreateLogger<QueuedHostedService>();
        }

        /// <summary>
        /// Gets the task queue.
        /// </summary>
        public ITaskQueue TaskQueue { get; }

        /// <summary>
        /// The start async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Queued Hosted Service is starting.");
            this.backgroundTask = Task.Run(this.BackgroundProcessingAsync);
            return Task.CompletedTask;
        }

        /// <summary>
        /// The stop async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Queued Hosted Service is stopping.");
            this.shutdown.Cancel();

            // WhenAny - A task that represents the completion of one of the supplied tasks.
            return Task.WhenAny(this.backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task BackgroundProcessingAsync()
        {
            this.logger.LogInformation("Queued Hosted Service is starting.");

            while (!this.shutdown.IsCancellationRequested)
            {
                Func<CancellationToken, DbContext, Task> workItem = await this.TaskQueue.DequeueAsync(this.shutdown.Token);

                try
                {
                    /*using (IServiceScope scope = this.services.CreateScope())
                    {
                        Type workType = workItem
                            .GetType()
                            .GetInterfaces()
                            .First(t => t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(ITaskQueue))
                            .GetGenericArguments()
                            .Last();

                        var worker = scope.ServiceProvider
                            .GetRequiredService(workType);

                        var task = (Task)workType.GetMethod("DoWork")
                            .Invoke(worker, new object[] { workOrder, this._shutdown.Token });
                        await task;
                    }*/

                    using (IServiceScope scope = this.services.CreateScope())
                    {
                        ProductDbContext context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                        await workItem(this.shutdown.Token, context);
                        
                        TestWebApi.Domain.Entities.Product p = context.Products.FirstOrDefault();

                        this.logger.LogInformation($"UpdatedBy: {p?.UpdatedBy}");
                        this.logger.LogInformation("Completed work-item.");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            this.logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
}
