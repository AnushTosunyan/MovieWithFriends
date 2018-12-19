using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MovieUniverse.Abstract.Data_Access_Contract.Repositories;
using MovieUniverse.Abstract.DB;
using MovieUniverse.Abstract.Services.UserServices;
using MovieUniverse.API.Controllers;
using MovieUniverse.API.Models;
using MovieUniverse.API.Ninject;
using MovieUniverse.Facade.Facade;
using Ninject;

namespace MovieUniverse.API.Filters
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsAuthorized(actionContext))
                return;
            if (SkipAuthorization(actionContext))
                return;

            HandleUnauthorizedRequest(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as ApiControllerBase;
            
            UserFacade userFacade =
                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof (UserFacade)) as UserFacade;
            if (actionContext.Request.Headers.Authorization == null)
            {
                if(controller == null)
                    return false;

                controller.UserSession = new UserSessionModel()
                {
                    IsAuthenticated = false,
                    Id = -1,
                };
                return false;
            }

            var key = actionContext.Request.Headers.Authorization.Scheme;
            long userId = userFacade.GetUserIdByToken(key);
            
            
            if (userId != -1)
            {
                
                if (controller != null)
                    controller.UserSession = new UserSessionModel()
                    {
                        Id = userId,
                        Key = key,
                        IsAuthenticated = true,
                    };
                return true;
            }
            if (controller != null)
                controller.UserSession = new UserSessionModel()
                {
                    IsAuthenticated = false,
                };
            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var result = new
            {
                HasError = true,
                ErrorCode = 1000,
                ErrorDescription = "UnAuthorized Request",
                StatusCode =HttpStatusCode.Unauthorized,
            };
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, result);
        }
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}