namespace TestWebAPI.DTOs
{
    using TestWebAPI.DTOs.RandomUserApi;

    /// <summary>
    /// The location.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        public Street Street { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        public string PostCode { get; set; }
    }
}
