namespace TestWebAPI.Library.HealthChecks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// The gc info health check builder extensions.
    /// Source: https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/host-and-deploy/health-checks/samples/2.x/HealthChecksSample/MemoryHealthCheck.cs
    /// </summary>
    public static class GCInfoHealthCheckBuilderExtensions
    {
        /// <summary>
        /// The add memory health check.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="failureStatus">
        /// The failure status.
        /// </param>
        /// <param name="tags">
        /// The tags.
        /// </param>
        /// <param name="thresholdInBytes">
        /// The threshold in bytes.
        /// </param>
        /// <returns>
        /// The <see cref="IHealthChecksBuilder"/>.
        /// </returns>
        public static IHealthChecksBuilder AddMemoryHealthCheck(
            this IHealthChecksBuilder builder,
            string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            long? thresholdInBytes = null)
        {
            // Register a check of type GCInfo.
            builder.AddCheck<MemoryHealthCheck>(
                name, failureStatus ?? HealthStatus.Degraded, tags);

            // Configure named options to pass the threshold into the check.
            if (thresholdInBytes.HasValue)
            {
                builder.Services.Configure<MemoryCheckOptions>(
                    name,
                    options =>
                        {
                            options.Threshold = thresholdInBytes.Value;
                    });
            }

            return builder;
        }
    }
}
