using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MovieUniverse.Abstract.Enums;
using MovieUniverse.Abstract.Services.UserServices;
using MovieUniverse.Abstract.Utils;
using MovieUniverse.API.Filters;
using MovieUniverse.API.Models;
using MovieUniverse.Facade.Facade;

namespace MovieUniverse.API.Hubs
{
    
    
    public class RoomHub:HubBase
    {
        private readonly IRoomService _roomService = GlobalHost.DependencyResolver.GetService(typeof(IRoomService)) as IRoomService;

        public static readonly ConcurrentDictionary<long,string> Conenctions = new ConcurrentDictionary<long, string>();
        private static readonly ConcurrentDictionary<long,string> UserIdGroupNames = new ConcurrentDictionary<long, string>();
        private static readonly ConcurrentDictionary<string,bool> GroupsVideoState = new ConcurrentDictionary<string, bool>();//true is played
        private static object _lockObject = new object();
        
        public override Task OnConnected()
        {
            User = GetUser(Context);

            Conenctions.AddOrUpdate(User.Id, Context.ConnectionId);
            
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            User = GetUser(Context);
            string roomName;
            string connectionId;

            if (UserIdGroupNames.ContainsKey(User.Id))
            {
                var roomId = UserIdGroupNames[User.Id];
                if (GroupsVideoState.ContainsKey(roomId))
                    GroupsVideoState[roomId] = false;
            }

            
            Conenctions.TryRemove(User.Id, out connectionId);
            UserIdGroupNames.TryRemove(User.Id, out roomName);

            if (!string.IsNullOrEmpty(roomName))
                Groups.Remove(connectionId, roomName);

            return base.OnDisconnected(stopCalled);
        }

        public async Task JoinRoom(long roomId)
        {
            var roomName = roomId.ToString();

            User = GetUser(Context);

            if (_roomService.GetRoomOwnerId(roomId) == User.Id)
                _roomService.MakeRoomOnline(User.Id, roomId);

            if (_roomService.IsRoomActive(roomId) && _roomService.IsMember(User.Id, roomId))
            {
                _roomService.ModifyUserStatus(User.Id, roomId, RoomMemberState.Online);
                UserIdGroupNames.AddOrUpdate(User.Id, roomName);
                await Groups.Add(Conenctions[User.Id], roomName);
                

            }
        }

        public async Task RemoveFromRoom()
        {
            try
            {
                string value;
                UserIdGroupNames.TryRemove(User.Id,out value);
                await Groups.Remove(Conenctions[User.Id], value);
            }
            catch (TaskCanceledException ex)
            {

            }
            finally
            {
                
            }
        }

        public void Play(double minute)
        {
            User = GetUser(Context);
            
            var roomId = UserIdGroupNames[User.Id];
            var roomIdLong = long.Parse(roomId);

            if (!GroupsVideoState.ContainsKey(roomId))
            {
                GroupsVideoState.TryAdd(roomId, false);
            }
            lock (_lockObject)
            {
                if (!GroupsVideoState[roomId])
                {
                    if (UserIdGroupNames.ContainsKey(User.Id) && UserIdGroupNames[User.Id] == roomId && _roomService.IsRoomActive(roomIdLong) && _roomService.IsMember(User.Id, roomIdLong))
                        Clients.Group(roomId, Conenctions[User.Id]).play(minute);
                    GroupsVideoState[roomId] = true;
                }
            }
            
        }

        public void Pause(double minute)
        {
            User = GetUser(Context);
            var roomId = UserIdGroupNames[User.Id];
            var roomIdLong = long.Parse(roomId);

            lock (_lockObject)
            {
                if (GroupsVideoState.ContainsKey(roomId) && GroupsVideoState[roomId])
                {
                    if (UserIdGroupNames.ContainsKey(User.Id) && UserIdGroupNames[User.Id] == roomId && _roomService.IsRoomActive(roomIdLong) && _roomService.IsMember(User.Id, roomIdLong))
                        Clients.Group(roomId, Conenctions[User.Id]).pause(minute);
                    GroupsVideoState[roomId] = false;
                }
            }
            
        }


        
    }
}