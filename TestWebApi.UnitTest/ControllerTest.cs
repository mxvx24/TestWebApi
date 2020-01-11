namespace TestWebApi.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

    using TestWebAPI.DTOs;

    using Xunit;

    using Employee = TestWebApi.Domain.Entities.Employee;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

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
            IActionResult result = await controller.GetEmployees();

            // ASSERT
            OkObjectResult viewResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IActionResult>(viewResult);

            List<Employee> model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Value);
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
            Employee employee = await this.context.Employees.FirstOrDefaultAsync();

            // ACT
            IActionResult result = await controller.GetEmployee(employee.Id);

            // ASSERT
            OkObjectResult viewResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IActionResult>(viewResult);

            Employee model = Assert.IsAssignableFrom<Employee>(viewResult.Value);
            Assert.Equal(employee.Id, model.Id);
        }

        /// <summary>
        /// The test update loan status.
        /// </summary>
        [Fact]
        public void TestUpdateLoanStatus()
        {
            // ARRANGE
            LoansController controller = new LoansController();
            var validationResults = new List<ValidationResult>();

            var request = new UpdateStatusRequest()
            {
                RunDate = DateTime.Now,
                Statuses = new List<string>() { "status 1", "status 2" }
            };

            // ACT
            ActionResult result = controller.UpdateStatus(request);
            var valid = Validator.TryValidateObject(request, new ValidationContext(request, null, null), validationResults);

            // ASSERT
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ActionResult>(result);
            Assert.True(valid);
        }

        /// <summary>
        /// The test add loan status.
        /// </summary>
        [Fact]
        public void TestAddLoanStatus()
        {
            // ARRANGE
            LoansController controller = new LoansController();
            var validationResults = new List<ValidationResult>();

            var request = new AddStatusRequest()
                              {
                                  RunDate = DateTime.Now,
                                  Statuses = new List<string>() { }
                              };

            // ACT
            ActionResult result = controller.AddStatus(request);
            var valid = Validator.TryValidateObject(request, new ValidationContext(request, null, null), validationResults);

            // ASSERT
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ActionResult>(result);
            Assert.True(valid);
        }
    }
}
