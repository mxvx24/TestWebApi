namespace TestWebAPI.Library.HealthChecks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The memory health check.
    /// Source: https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/host-and-deploy/health-checks/samples/2.x/HealthChecksSample/MemoryHealthCheck.cs
    /// </summary>
    public class MemoryHealthCheck : IHealthCheck
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly IOptionsMonitor<MemoryCheckOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryHealthCheck"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name => "memory_check";

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
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var localOptions = this.options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
                           {
                               { "AllocatedBytes", allocated },
                               { "Gen0Collections", GC.CollectionCount(0) },
                               { "Gen1Collections", GC.CollectionCount(1) },
                               { "Gen2Collections", GC.CollectionCount(2) },
                           };

            var status = (allocated < localOptions.Threshold) ?
                             HealthStatus.Healthy : HealthStatus.Unhealthy;

            return Task.FromResult(new HealthCheckResult(
                status: status,
                description: "Reports degraded status if allocated bytes " + $">= {localOptions.Threshold} bytes.",
                exception: null,
                data: data));
        }
    }
}
