namespace TestWebAPI.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using TestWebAPI.Entities;

    /// <summary>
    /// The employee configuration.
    /// </summary>
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // HasData method to seed initial records
            builder.HasData(
                new Employee
                    {
                        Id = 1,
                        FirstName = "Mohammed",
                        LastName = "Hoque",
                        Email = "mohammed.hoque@email.com",
                        Title = "Big Title"
                    },
                new Employee
                    {
                        Id = 2,
                        FirstName = "Test",
                        LastName = "User",
                        Email = "test.user@email.com",
                        Title = "Cool Title"
                    });
        }
    }
}
