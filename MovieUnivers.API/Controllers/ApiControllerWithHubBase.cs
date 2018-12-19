using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;

namespace MovieUniverse.API.Controllers
{
    public class ApiControllerWithHubBase<THub> : ApiControllerBase where THub : Hub
    {
        private readonly Lazy<IHubContext> _hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
            );

        protected IHubContext Hub => _hub.Value;
    }
}
