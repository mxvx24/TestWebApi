namespace TestWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using TestWebAPI.DTOs;
    using TestWebAPI.Library.ActionFilters;

    /// <summary>
    /// The values controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// The update status.
        /// </summary>
        /// <param name="updateStatusRequest">
        /// The update status request.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPut("statuses")]
        [ValidationActionFilter]
        public ActionResult UpdateStatus([FromBody]UpdateStatusRequest updateStatusRequest)
        {
            /*if (!ModelState.IsValid)
            {
                // return this.BadRequest(this.ModelState);
                return this.BadRequest(new { errors = this.ModelState });
            } */

            return this.Ok(updateStatusRequest);
        }

        /// <summary>
        /// The add status.
        /// </summary>
        /// <param name="addStatusRequest">
        /// The add status request.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("statuses")]
        [ValidationActionFilter]
        public ActionResult AddStatus([FromBody]AddStatusRequest addStatusRequest)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is not valid.");
            }

            return this.Created("/", addStatusRequest);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
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
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("async")]
        public async Task<ActionResult> GetAsync()
        {
            List<string> values = await this.ValueRepo(default);
            return this.Ok(values);
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("async/{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            List<string> values = await this.ValueRepo(id);
            return this.Ok(values);
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

        /// <summary>
        /// The value repo.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private Task<List<string>> ValueRepo(int id)
        {
            var values = new List<string>();

            if (id == default)
            {
                for (var i = 0; i < 1000; i++)
                {
                    values.Add($"value{i}");
                }
            }
            else
            {
                values.Add($"value{id}");
            }

            return Task.FromResult(values);
        }
    }
}