namespace TestWebAPI.Library
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// The database connection check.
    /// Source: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
    /// </summary>
    public class DatabaseConnectionCheck : IHealthCheck
    {
        /// <summary>
        /// The default test query.
        /// </summary>
        private static readonly string DefaultTestQuery = "SELECT 1";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionCheck"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public DatabaseConnectionCheck(string connectionString) : this(connectionString, testQuery: DefaultTestQuery)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionCheck"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="testQuery">
        /// The test query.
        /// </param>
        public DatabaseConnectionCheck(string connectionString, string testQuery)
        {
            this.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.TestQuery = testQuery;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Gets the test query.
        /// </summary>
        public string TestQuery { get; }

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
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    if (this.TestQuery != null)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = this.TestQuery;

                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}
