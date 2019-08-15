namespace TestWebAPI.Data
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using TestWebAPI.Configuration;
    using TestWebAPI.Entities;

    /// <summary>
    ///     The employee data context.
    /// </summary>
    public class EmployeeDataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDataContext" /> class.
        /// </summary>
        public EmployeeDataContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDataContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public EmployeeDataContext(DbContextOptions<EmployeeDataContext> options) : base(options)
        {
            // this.nameChangeEventHandler = employeeEventHandler;
        }

        /// <summary>
        /// The on save event handler.
        /// </summary>
        /// <param name="entries">
        /// The entries.
        /// </param>
        public delegate void OnSaveEventHandler(IEnumerable<EntityEntry> entries);

        /// <summary>
        /// Gets or sets the on save event handlers.
        /// </summary>
        public OnSaveEventHandler OnSaveEventHandlers { get; set; }

        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.OnSaveEventHandlers?.Invoke(this.ChangeTracker.Entries());

            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// The on configuring.
        /// </summary>
        /// <param name="optionsBuilder">
        /// The options builder.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=Test-WebApi-local;Trusted_Connection=True;MultipleActiveResultSets=true",
                option =>
                    {
                        option.EnableRetryOnFailure();
                    });

            optionsBuilder.EnableSensitiveDataLogging();
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
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}