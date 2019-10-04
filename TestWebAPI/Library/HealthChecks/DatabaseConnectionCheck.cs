namespace TestWebAPI.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// The database connection check.
    /// </summary>
    public class DatabaseConnectionCheck : IHealthCheck
    {
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
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = new CancellationToken())
        {
            const bool HealthCheckResultHealthy = true;

            if (HealthCheckResultHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The check indicates a healthy result."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("The check indicates an unhealthy result."));
        }
    }
}
