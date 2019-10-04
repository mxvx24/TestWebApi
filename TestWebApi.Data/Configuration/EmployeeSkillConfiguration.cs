namespace TestWebApi.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The employee skill configuration.
    /// </summary>
    internal class EmployeeSkillConfiguration : IEntityTypeConfiguration<EmployeeSkill>
    {
        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Configure(EntityTypeBuilder<EmployeeSkill> builder)
        {
            builder.HasKey(
                es => new { es.EmployeeId, es.SkillId });
        }
    }
}
