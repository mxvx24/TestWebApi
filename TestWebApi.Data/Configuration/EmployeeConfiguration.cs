namespace TestWebApi.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The employee configuration.
    /// </summary>
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
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
            builder.HasData(DataGenerator.GetEmployee(20000));
        }
    }
}
