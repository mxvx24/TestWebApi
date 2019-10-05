namespace TestWebAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;

    using TestWebApi.Domain.Entities;
    
    /// <summary>
    /// The value.
    /// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "FirstName length must be between 3 and 50.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        public string Title { get; set; }
    }
}
