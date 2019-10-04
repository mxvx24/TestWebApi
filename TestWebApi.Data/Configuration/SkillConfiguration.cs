namespace TestWebApi.Data.Configuration
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The skill configuration.
    /// </summary>
    internal class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasData(DataGenerator.GetSkill());
        }
    }
}
