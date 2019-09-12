namespace TestWebAPI.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using TestWebAPI.Entities;
    using TestWebAPI.Library;

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
            // HasData method to seed initial records - not working
            /*var employees = DataGenerator.GetEmployee(20000);
            builder.HasData(employees);*/
        }
    }
}
