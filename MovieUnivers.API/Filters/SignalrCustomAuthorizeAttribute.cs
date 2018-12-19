using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MovieUniverse.API.Hubs;
using MovieUniverse.API.Models;
using MovieUniverse.Facade.Assembler.RoomAssemblers;
using MovieUniverse.Facade.Facade;
using AuthorizeAttribute = Microsoft.AspNet.SignalR.AuthorizeAttribute;

namespace MovieUniverse.API.Filters
{
    public class SignalrCustomAuthorizeAttribute:AuthorizeAttribute
    {
        private static int count = 0;

        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            
            count++;
            var type = hubDescriptor.HubType;
            var token = request.QueryString["token"];
            UserFacade userFacade = GlobalHost.DependencyResolver.GetService(typeof(UserFacade)) as UserFacade;
            long userId = -1;
            Debug.WriteLine(count);
            if (userFacade != null && (userId = userFacade.GetUserIdByToken(token)) != -1)
            {

                var muHub = GlobalHost.DependencyResolver.GetService(type) as HubBase;
                if (muHub == null)
                    throw new ArgumentException("Invalid Hub Conenction");
                muHub.User = new UserSessionModel()
                {
                    Id = userId,
                    IsAuthenticated = true,
                    Key = token,
                };
                return true;
            }
            return false;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var token = hubIncomingInvokerContext.Hub.Context.Request.QueryString["token"];
            UserFacade userFacade =
               GlobalHost.DependencyResolver.GetService(typeof(UserFacade)) as UserFacade;
            long userId = -1;
            Debug.WriteLine(count);
            if (userFacade != null && (userId = userFacade.GetUserIdByToken(token)) != -1)
            {

                var muHub = hubIncomingInvokerContext.Hub as HubBase;
                if (muHub == null)
                    throw new ArgumentException("Invalid Hub Conenction");
                muHub.User = new UserSessionModel()
                {
                    Id = userId,
                    IsAuthenticated = true,
                    Key = token,
                };
                return true;
            }
            return false;
            

        }
        
    }
}