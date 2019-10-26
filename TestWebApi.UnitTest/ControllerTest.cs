namespace TestWebApi.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Moq;

    using TestWebAPI;
    using TestWebAPI.Controllers;
    using TestWebApi.Data;
    using TestWebApi.Data.Contexts;
    using TestWebApi.Data.Repositories;
    using TestWebApi.Domain.Entities;

    using Xunit;

    /// <summary>
    /// The controller test.
    /// </summary>
    public class ControllerTest
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly EmployeeDataContext context;

        /// <summary>
        /// The provider.
        /// </summary>
        private readonly ServiceProvider provider;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTest"/> class.
        /// </summary>
        public ControllerTest()
        {
            var services = new ServiceCollection();
            this.provider = services
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<EmployeeDataContext>(
                    (provider, options) =>
                        {
                            options.UseInMemoryDatabase(Guid.NewGuid().ToString()).EnableSensitiveDataLogging()
                                .UseInternalServiceProvider(provider);
                        })
                .AddAutoMapper(typeof(Startup))
                .BuildServiceProvider();

            this.provider = services.BuildServiceProvider();
            this.mapper = this.provider.GetService<IMapper>();
            this.context = this.provider.GetService<EmployeeDataContext>();

            this.context.AddRange(DataGenerator.GetEmployee(100));
            this.context.SaveChanges();
        }

        /// <summary>
        /// The test get employees.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task TestGetEmployees()
        {
            // ARRANGE
            var repo = new GenericRepository<Employee, EmployeeDataContext>(this.context, this.mapper);
            var controller = new EmployeesController(new Mock<ILogger<EmployeesController>>().Object, repo, this.context);
            
            // ACT
            var result = await controller.GetEmployees();

            // ASSERT
            var viewResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IActionResult>(viewResult);

            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Value);
            Assert.NotEmpty(model);
        }

        /// <summary>
        /// The test get employee by id.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task TestGetEmployeeById()
        {
            // ARRANGE
            var repo = new GenericRepository<Employee, EmployeeDataContext>(this.context, this.mapper);
            var controller = new EmployeesController(new Mock<ILogger<EmployeesController>>().Object, repo, this.context);
            var employee = await this.context.Employees.FirstOrDefaultAsync();

            // ACT
            var result = await controller.GetEmployee(employee.Id);

            // ASSERT
            var viewResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IActionResult>(viewResult);

            var model = Assert.IsAssignableFrom<Employee>(viewResult.Value);
            Assert.Equal(employee.Id, model.Id);
        }
    }
}
