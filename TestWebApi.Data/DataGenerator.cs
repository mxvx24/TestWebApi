namespace TestWebApi.Data
{
    using System;
    using System.Collections.Generic;

    using TestWebApi.Domain.Entities;
    using TestWebApi.Domain.Enums;

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
                    Title = $"Title_{i}",
                    UserAccountControl = GetRandomUserAccountControlValue(),
                    CreatedBy = "System",
                    CreatedOn = DateTime.UtcNow
                });
            }

            return employees;
        }

        /// <summary>
        /// The get skill.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<Skill> GetSkill()
        {
            var skills = new List<Skill>
                             {
                                 new Skill { Id = 1, Name = "C#" }, 
                                 new Skill { Id = 2, Name = "ASP.NET" },
                                 new Skill { Id = 3, Name = ".NET Core" },
                                 new Skill { Id = 4, Name = "SQL" },
                                 new Skill { Id = 5, Name = "Angular 6+" },
                                 new Skill { Id = 6, Name = "HTML" },
                                 new Skill { Id = 7, Name = "CSS" },
                                 new Skill { Id = 8, Name = "JavaScript" },
                             };


            return skills;
        }

        /// <summary>
        /// The get address.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<Address> GetAddress(int count)
        {
            var employees = new List<Address>();

            for (var i = 1; i <= count; i++)
            {
                employees.Add(new Address()
                                  {
                                      Id = i,
                                      LocationName = $"Location_Name_{i}",
                                      StreetName = $"Road_{i}",
                                      StreetNo = $"{i}",
                                      City = $"City_{i}",
                                      State = GetRandomState(),
                                      CreatedBy = "System",
                                      CreatedOn = DateTime.UtcNow
                });
            }

            return employees;
        }

        /// <summary>
        /// The get random state.
        /// </summary>
        /// <returns>
        /// The <see cref="States"/>.
        /// </returns>
        private static States GetRandomState()
        {
            var values = Enum.GetValues(typeof(States));
            return (States)values.GetValue(new Random().Next(values.Length));
        }

        /// <summary>
        /// The get random user account control value.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int GetRandomUserAccountControlValue()
        {
            // 512 Enabled Account, 514 Disabled Account
            var values = new[] { 512, 514 };
            var randomIndex = new Random().Next(values.Length);
            return values[randomIndex];
        }
    }
}
