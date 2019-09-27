namespace TestWebAPI
{
    using TestWebAPI.DTOs;

    /// <summary>
    /// The conversion.
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        /// The to DTO.
        /// </summary>
        /// <param name="employee">
        /// The employee.
        /// </param>
        /// <returns>
        /// The <see cref="Employee"/>.
        /// </returns>
        public static Employee ToDto(this TestWebApi.Domain.Entities.Employee employee)
        {
            return new DTOs.Employee()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Title = employee.Title
            };
        }

        /// <summary>
        /// The to entity.
        /// </summary>
        /// <param name="employee">
        /// The employee.
        /// </param>
        /// <returns>
        /// The <see cref="Employee"/>.
        /// </returns>
        public static TestWebApi.Domain.Entities.Employee ToEntity(this Employee employee)
        {
            return new TestWebApi.Domain.Entities.Employee
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Title = employee.Title
            };
        }
    }
}