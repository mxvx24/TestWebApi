namespace TestWebApi.Domain.Entities
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using TestWebApi.Domain.Enums;

    /// <summary>
    /// The address.
    /// </summary>
    public class Address : BaseEntity
    {
        /// <summary>
        /// Gets or sets the location name.
        /// </summary>
        [Description("Location Name is an options field")]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        [Required]
        public string StreetName { get; set; }

        /// <summary>
        /// Gets or sets the street no.
        /// </summary>
        [Required]
        public string StreetNo { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public States State { get; set; }
    }
}
