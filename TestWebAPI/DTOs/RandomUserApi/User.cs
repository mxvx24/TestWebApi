namespace TestWebAPI.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public Location Location { get; set; }
    }
}
