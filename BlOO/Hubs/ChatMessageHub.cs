using BlOO.Models;
using Microsoft.AspNetCore.SignalR;
using static System.Net.Mime.MediaTypeNames;

namespace BLOO.Hubs
{
    public class ChatMessageHub:Hub
    {
       

        public void writeMessage(string text)
        {
            Clients.All.SendAsync("NewMessage",text);

        }

    
        //[Authorize]
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
