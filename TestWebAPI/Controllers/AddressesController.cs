namespace TestWebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Internal;

    using TestWebApi.Data.Repositories;
    using TestWebApi.Domain.Entities;

    /// <summary>
    /// The values controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        /// <summary>
        /// The employee repository.
        /// </summary>
        private readonly IRepository<Address> employeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressesController"/> class.
        /// </summary>
        /// <param name="employeeRepository">
        /// The employee repository.
        /// </param>
        public AddressesController(IRepository<Address> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="searchKey">
        /// The search keyword
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get([FromQuery]string searchKey = "")
        {
            List<Address> addresses;

            if (string.IsNullOrWhiteSpace(searchKey))
            {
                addresses = await this.employeeRepository.GetAllAsync();
            }
            else
            {
                addresses = await this.employeeRepository.FindAsync(
                               a => a.LocationName.Contains(searchKey) 
                                    || a.City.Contains(searchKey)
                                    || a.StreetName.Contains(searchKey));
            }

            if (addresses != null && addresses.Any())
            {
                return this.Ok(addresses);
            }

            return this.NotFound();
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> Get(int id)
        {
            var address = await this.employeeRepository.GetAsync(id);

            if (address is null)
            {
                return this.NotFound($"No record found for id {id}");
            }

            return this.Ok(address);
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// The put.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}