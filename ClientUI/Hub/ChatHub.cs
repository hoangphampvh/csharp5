using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace ChatHubs.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage()
        {
            return Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
