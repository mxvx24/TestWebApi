namespace TestWebAPI.Library.ActionFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The validation action filter.
    /// Source Article: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1
    /// </summary>
    public class ValidationActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The on action executing.
        /// If both asynchronous and synchronous interfaces are implemented in one class, only the async method is called.
        /// When using abstract classes like ActionFilterAttribute override only the synchronous methods or the async method
        /// for each filter type.
        /// </summary>
        /// <param name="context">
        /// The action context.
        /// </param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errorList = (from modelState in context.ModelState.Values
                                          from error in modelState.Errors
                                          select error.ErrorMessage).ToList();

                /* list.AddRange(from modelState in context.ModelState.Values 
                              from error in modelState.Errors 
                              select error.Exception.ToString()); */

                context.Result = new BadRequestObjectResult(errorList);
            }

            base.OnActionExecuting(context);
        }
    }
}
