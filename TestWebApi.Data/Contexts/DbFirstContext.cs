namespace TestWebApi.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The database first context.
    /// </summary>
    public class DbFirstContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbFirstContext"/> class.
        /// </summary>
        public DbFirstContext()
        {
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public DbSet<TestProduct> TestProducts { get; set; }

        /// <summary>
        /// The on configuring.
        /// </summary>
        /// <param name="optionsBuilder">
        /// The options builder.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=Test-WebApi-local;Trusted_Connection=True;MultipleActiveResultSets=true;Application Name=TestWebApi",
                    option => { option.EnableRetryOnFailure(); });
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseLoggerFactory(new LoggerFactory(
                    new[]
                        {
                            new ConsoleLoggerProvider(
                                (category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information,
                                true)
                        }));
            }
        }
    }
}
