namespace TestWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.Extensions.Logging;

    using TestWebApi.Data;
    using TestWebApi.Data.Contexts;
    using TestWebApi.Data.Repositories;

    using TestWebAPI.DTOs;
    using TestWebAPI.Library;

    using Employee = TestWebApi.Domain.Entities.Employee;

    /// <summary>
    /// The employees controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The _context. Only needed for compiled query.
        /// </summary>
        private readonly EmployeeDataContext context;

        /// <summary>
        /// The employee repository.
        /// </summary>
        private readonly IRepository<Employee> employeeRepository;

        /// <summary>
        /// Gets the employee name search.
        /// MSDN: Although in general EF Core can automatically compile and cache queries based on a hashed representation of the query expressions,
        /// this mechanism can be used to obtain a small performance gain by bypassing the computation of the hash and the cache lookup, allowing
        /// the application to use an already compiled query through the invocation of a delegate.
        /// </summary>
        private readonly Func<EmployeeDataContext, string, AsyncEnumerable<Employee>> searchEmployeesByName =
            EF.CompileAsyncQuery((EmployeeDataContext context, string nameLike) =>
                context.Employees.Where(e => e.FirstName.Contains(nameLike) || e.LastName.Contains(nameLike)));

        /*EF.Functions.Like(e.FirstName, nameLike) || EF.Functions.Like(e.LastName, nameLike)*/

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public EmployeesController(ILogger<EmployeesController> logger, IRepository<Employee> repository, EmployeeDataContext context)
        {
            this.employeeRepository = repository ?? throw new ArgumentNullException(nameof(repository));

            // ToDo: Remove context. Only needed for the use of compiled query.
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger;

            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            this.logger.LogTrace($"{nameof(EmployeesController)} class has been instantiated.");
        }

        /*[HttpGet("/test")]
        public IActionResult<Employee> Test()
        {
            return this.Ok(new Employee());
        }*/

        /// <summary>
        /// The get employee.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            var employee = await this.employeeRepository.GetAsync(id);

            if (employee == null)
            {
                return this.NotFound($"Invalid employee id: {id}");
            }

            return this.Ok(employee);
        }

        /// <summary>
        /// The get employees.
        /// </summary>
        /// <param name="employeeRequestFilter">
        /// The employee request filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("")]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeRequestFilter employeeRequestFilter)
        {
            IEnumerable<DTOs.Employee> employees = null;

            // Create default predicate (returns all tasks)
            var predicate = PredicateBuilder.True<Employee>();

            // ToDo: Too many "if" statements
            if (employeeRequestFilter.FirstName != null)
            {
                predicate = predicate.And(e => employeeRequestFilter.FirstName.Contains(e.FirstName));
            }

            if (employeeRequestFilter.LastName != null)
            {
                predicate = predicate.And(e => employeeRequestFilter.LastName.Contains(e.LastName));
            }

            employees = await this.employeeRepository.FindAsync<DTOs.Employee>(predicate);
            
            if (employees is null || !employees.Any())
            {
                return this.NotFound(employees);
            }

            return this.Ok(employees);
        }

        /// <summary>
        /// The get employees.
        /// </summary>
        /// <param name="nameLike">
        /// The name Like.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        /*[HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] string nameLike = default)
        {
            var employees = string.IsNullOrWhiteSpace(nameLike)
                                ? await this.employeeRepository.GetAllAsync()
                                : await this.searchEmployeesByName(this.context, nameLike).ToListAsync();
            // await this.employeeRepository.FindAsync(e => e.FirstName.Contains(nameLike) || e.LastName.Contains(nameLike));

            if (!employees.Any())
            {
                return this.NotFound(employees);
            }

            return this.Ok(employees);
        } */

        /// <summary>
        /// The post employee.
        /// </summary>
        /// <param name="employee">
        /// The employee.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] DTOs.Employee employee)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            /* this.context.Employees.Add(employee.ToEntity());
            await this.context.SaveChangesAsync(); */

            await this.employeeRepository.Add(employee.ToEntity());
            await this.employeeRepository.SaveChangesAsync();

            return this.CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        /// <summary>
        /// The put employee.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="employee">
        /// The employee.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] DTOs.Employee employee)
        {
            /*{"Id":1,"FirstName":"Mohammed","LastName":"Hoque","Email":"mohammed.hoque@email.com","Title":"something"}*/

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != employee.Id)
            {
                return this.BadRequest();
            }

            // this.context.Entry(employee.ToEntity()).State = EntityState.Modified;
            var existingEmployee = await this.context.Employees.FindAsync(id);
            Util.CopyValues(employee.ToEntity(), existingEmployee, true);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.EmployeeExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        /// <summary>
        /// The patch employee.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="patch">
        /// The patch.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee([FromRoute]int id, [FromBody]JsonPatchDocument<DTOs.Employee> patch)
        {
            if (id == 0 || patch is null)
            {
                return this.BadRequest("Required parameters cannot be zero or null");
            }

            var employee = await this.context.Employees.FindAsync(id);
            var employeeDto = employee.ToDto();

            patch.ApplyTo(employeeDto, this.ModelState);

            this.TryValidateModel(employeeDto);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok();
        }

        /// <summary>
        /// The delete employee.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var employee = await this.context.Employees.FindAsync(id);

            if (employee == null)
            {
                return this.NotFound();
            }

            this.context.Employees.Remove(employee);
            await this.context.SaveChangesAsync();

            return this.Ok(employee);
        }

        /// <summary>
        /// The get database date time.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("DbDateTime")]
        public async Task<IActionResult> GetDbDateTime()
        {
            string result;

            using (var connection = this.context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT GETDATE()";
                    result = (string)await command.ExecuteScalarAsync();
                }
            }

            return this.Ok(result);
        }

        [HttpGet("exception")]
        public async Task<IActionResult> GetException()
        {
            throw new Exception("Test Exception. Please ignore.");
            return Ok();
        }

        /// <summary>
        /// The employee exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool EmployeeExists(int id)
        {
            return this.context.Employees.Any(e => e.Id == id);
        }
    }
}