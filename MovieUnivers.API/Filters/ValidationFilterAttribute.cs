
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MovieUniverse.Abstract.ApiModels;

namespace MovieUniverse.API.Filters
{
    public class ValidationFilterAttribute:ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {



            if (!actionContext.ModelState.IsValid)
            {
                
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.OK, new
                    {
                        ErrorDescription =
                            string.Join(",",
                                actionContext.ModelState.Select(
                                    x => string.Join(",", x.Value.Errors.Select(y => y.ErrorMessage).ToList()))),
                        ErrorCode = -2,
                        StatusCode = HttpStatusCode.OK,
                        HasError = true,
                    });
            }
        }
    }
}