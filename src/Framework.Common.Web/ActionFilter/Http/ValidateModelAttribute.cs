using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Framework.Common.Web.Responses;

namespace Framework.Common.Web.ActionFilter.Mvc
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            if (actionContext.ModelState.IsValid) return;

            var response = new ServiceErrorResult(actionContext.ModelState);

            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.BadRequest, response);

            //var errorList = actionContext.ModelState.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

            //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorList);
        }
    }

    
}