namespace TestWebAPI.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// The add status request.
    /// </summary>
    public class AddStatusRequest
    {
        /// <summary>
        /// Gets or sets the run date.
        /// </summary>
        [Required, DataType(DataType.DateTime)]
        public DateTime RunDate { get; set; }

        /// <summary>
        /// Gets or sets the statuses.
        /// </summary>
        [Required, JsonProperty(Required = Required.DisallowNull)]
        public IList<string> Statuses { get; set; }

        /// <summary>
        /// The validate.
        /// </summary>
        /// <param name="validationContext">
        /// The validation context.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.RunDate == default)
            {
                // yield return new ValidationResult($"Provide a valid value for {nameof(this.RunDate)} field.");
                results.Add(new ValidationResult($"Provide a valid value for {nameof(this.RunDate)} field."));
            }

            if (this.Statuses is null)
            {
                // yield return new ValidationResult($"Provide a valid value for {nameof(this.Statuses)} field.");
                results.Add(new ValidationResult($"Provide a valid value for {nameof(this.Statuses)} field."));
            }

            if (this.Statuses != null && this.Statuses.Count == 0)
            {
                results.Add(new ValidationResult($"Provide at least one value for {nameof(this.Statuses)} field."));
            }

            if (this.Statuses != null && this.Statuses.Contains(null))
            {
                results.Add(new ValidationResult($"{nameof(this.Statuses)} field cannot contain null value."));
            }

            return results;
        }*/
    }
}
