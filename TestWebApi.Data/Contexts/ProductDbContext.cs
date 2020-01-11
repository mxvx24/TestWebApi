namespace TestWebApi.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The product db context.
    /// </summary>
    public class ProductDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDbContext"/> class.
        /// </summary>
        public ProductDbContext()
        {
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

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
                    "Server=(localdb)\\mssqllocaldb;Database=Test-WebApi-local;Trusted_Connection=True;MultipleActiveResultSets=true",
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

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product One" },
                new Product { Id = 2, Name = "Product Two" });
        }
    }
}
