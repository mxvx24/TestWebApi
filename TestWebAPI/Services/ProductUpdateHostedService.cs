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
    /// The product update hosted service.
    /// </summary>
    public class ProductUpdateHostedService : IHostedService, IDisposable
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILogger log;

        /// <summary>
        /// The provider.
        /// </summary>
        private readonly IServiceProvider provider;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUpdateHostedService"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public ProductUpdateHostedService(ILogger<ProductUpdateHostedService> logger, IServiceProvider provider)
        {
            this.log = logger;
            this.provider = provider;
        }

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
            this.log.LogInformation($"{nameof(ProductUpdateHostedService)} is Starting");

            // this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            this.DoWork();

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
            this.log.LogInformation($"{nameof(ProductUpdateHostedService)} is Stopping");

            // this.timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            // this.timer?.Dispose();
        }

        /// <summary>
        /// The do work.
        /// </summary>
        private void DoWork()
        {
            this.log.LogInformation("Timed Background Service is working.");

            using (IServiceScope scope = this.provider.CreateScope())
            {
                ProductDbContext context = scope.ServiceProvider.GetService<ProductDbContext>();
                var products = context.Products.FromSql("SpUpdateAllProducts @p0, @p1", new object[] { "1,2,3", "TestWebApiLoad" }).ToList();
                this.log.LogInformation($"Product Count: {products.Count()}");
            }
        }
    }
}
