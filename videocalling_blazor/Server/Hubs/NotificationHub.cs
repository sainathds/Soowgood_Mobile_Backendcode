using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using videocalling_blazor.Shared;

namespace videocalling_blazor.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public Task RoomsUpdated(string room) =>
            Clients.All.SendAsync(HubEndpoints.RoomsUpdated, room);
    }
}
