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
    public class LoansController : ControllerBase
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
    }
}