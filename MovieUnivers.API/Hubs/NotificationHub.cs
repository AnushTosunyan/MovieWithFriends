using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using MovieUniverse.Abstract.Utils;

namespace MovieUniverse.API.Hubs
{
    public class NotificationHub : HubBase
    {
        public static ConcurrentDictionary<long, string> Connections = new ConcurrentDictionary<long, string>();
        public override Task OnConnected()
        {
            User = GetUser(Context);
            Connections.AddOrUpdate(User.Id, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            User = GetUser(Context);
            var connectionId = "";
            Connections.TryRemove(User.Id, out connectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}