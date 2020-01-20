namespace TestWebAPI.Library.HealthChecks
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    using TestWebApi.Data.Contexts;

    /// <summary>
    /// The db context health check.
    /// There's built-in EF COre check: services.AddHealthChecks().AddDbContextCheck<ProductDbContext>("ProductDbContextHealthCheck");
    /// </summary>
    public class ProductDbContextHealthCheck : IHealthCheck
    {
        /// <summary>
        /// The db context.
        /// </summary>
        private readonly ProductDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDbContextHealthCheck"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The product database context.
        /// </param>
        public ProductDbContextHealthCheck(ProductDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// The check health async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return await this.dbContext.Database.CanConnectAsync(cancellationToken) 
                       ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
    }
}
