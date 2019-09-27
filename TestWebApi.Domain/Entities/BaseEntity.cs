namespace TestWebApi.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The base entity.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
