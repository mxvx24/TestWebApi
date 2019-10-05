namespace TestWebApi.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The value.
    /// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
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
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the address id.
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the user account control.
        /// </summary>
        public int UserAccountControl { get; set; }
        
        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        public virtual ICollection<EmployeeSkill> Skills { get; set; } = new List<EmployeeSkill>();

        /// <summary>
        /// The is action.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsActive()
        {
            if (this.UserAccountControl != default)
            {
                return !Convert.ToBoolean(this.UserAccountControl & 0x0002);
            }

            return false;
        }
    }
}
