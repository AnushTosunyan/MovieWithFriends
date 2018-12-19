using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.UI.WebControls;
using MovieUniverse.Abstract.ApiModels;
using MovieUniverse.Abstract.Exceptions;

namespace MovieUniverse.API.Filters
{
    public class GlobalExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var request = actionExecutedContext.ActionContext.Request;
            var exp = actionExecutedContext.Exception as MovieUniverseException;
            object result;
            if (exp == null)
            {
                result = new
                {
                    ErrorDescription = String.Join(",", actionExecutedContext.Exception.Message),
                    ErrorCode = -1,
                    HasError = true,
                    StatusCode = HttpStatusCode.OK,
                };
                

            }
            else
            {
                result = new
                {
                    ErrorDescription = exp.Errors != null ? String.Join(",", exp.Errors) : exp.Type.ToString(),
                    ErrorCode = (int)exp.Type,
                    HasError = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }

            
            actionExecutedContext.Response = request.CreateResponse(HttpStatusCode.OK, result);

            base.OnException(actionExecutedContext);
        }
    }
}