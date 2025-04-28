using Lab5.Models;
using Microsoft.AspNetCore.SignalR;

namespace Lab5.Hubs
{
    public class ToDoHub : Hub
    {
        public async Task SendToDoUpdate()
        {
            await Clients.All.SendAsync("ReceiveToDoUpdate");
        }
    }

}
