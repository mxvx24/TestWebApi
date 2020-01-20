namespace TestWebAPI.Library.HealthChecks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// The ping health check.
    /// </summary>
    public class PingHealthCheck : IHealthCheck
    {
        /// <summary>
        /// The host.
        /// </summary>
        private string host;

        /// <summary>
        /// The timeout.
        /// </summary>
        private int timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="PingHealthCheck"/> class.
        /// </summary>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        public PingHealthCheck(string host, int timeout)
        {
            this.host = host;
            this.timeout = timeout;
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
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(this.host, this.timeout);

                    if (reply.Status != IPStatus.Success)
                    {
                        return HealthCheckResult.Unhealthy();
                    }

                    if (reply.RoundtripTime > this.timeout)
                    {
                        return HealthCheckResult.Degraded();
                    }

                    return HealthCheckResult.Healthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
