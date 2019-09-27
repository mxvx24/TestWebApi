namespace TestWebApi.Data
{
    using System.Collections.Generic;

    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The data generator.
    /// </summary>
    public class DataGenerator
    {
        /// <summary>
        /// The get employee.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<Employee> GetEmployee(int count)
        {
            var employees = new List<Employee>();

            for (var i = 1; i <= count; i++)
            {
                employees.Add(new Employee()
                {
                    Id = i,
                    FirstName = $"First_{i}",
                    LastName = $"Last_{i}",
                    Email = $"email_{i}@email.com",
                    Title = $"Title_{i}"
                });
            }

            return employees;
        }
    }
}
