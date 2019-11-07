namespace TestWebAPI.DTOs
{
    /// <summary>
    /// The user API response.
    /// </summary>
    public class UserApiResponse
    {
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        public User[] Results { get; set; }

        /// <summary>
        /// Gets or sets the info.
        /// </summary>
        public Information Info { get; set; }

        /// <summary>
        /// The information.
        /// </summary>
        public class Information
        {
            /// <summary>
            /// Gets or sets the seed.
            /// </summary>
            public string Seed { get; set; }

            /// <summary>
            /// Gets or sets the results.
            /// </summary>
            public int Results { get; set; }

            /// <summary>
            /// Gets or sets the page.
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// Gets or sets the version.
            /// </summary>
            public string Version { get; set; }
        }
    }
}
