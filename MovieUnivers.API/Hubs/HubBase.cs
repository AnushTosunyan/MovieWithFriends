using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MovieUniverse.API.Models;
using MovieUniverse.Facade.Facade;

namespace MovieUniverse.API.Hubs
{
    public class HubBase:Hub
    {
        public UserSessionModel User { get; set; }
        protected UserSessionModel GetUser(HubCallerContext context)
        {
            var token = context.QueryString["token"];

            UserFacade userFacade = GlobalHost.DependencyResolver.GetService(typeof(UserFacade)) as UserFacade;
            long userId = -1;

            if (userFacade != null && (userId = userFacade.GetUserIdByToken(token)) != -1)
            {
                var user = new UserSessionModel()
                {
                    Id = userId,
                    IsAuthenticated = true,
                    Key = token,
                };
                return user;
            }

            return new UserSessionModel()
            {
                Id = -1,
                IsAuthenticated = false,
            };

        }
    }
}