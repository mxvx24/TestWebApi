namespace TestWebApi.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The product.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
