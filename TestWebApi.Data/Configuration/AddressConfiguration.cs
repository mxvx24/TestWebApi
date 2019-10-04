namespace TestWebApi.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The address configuration.
    /// </summary>
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasData(DataGenerator.GetAddress(500));
        }
    }
}
