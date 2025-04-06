using BlOO.Models;
using BlOO.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using static System.Net.Mime.MediaTypeNames;

namespace BLOO.Hubs
{
    public class ChatMessageHub : Hub
    {
        IMessageRepository _messageRepository;

        public ChatMessageHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void NewMessage(ChatMessageViewModel message)
        {
            Message NewMessage = new Message();
            NewMessage.Content = message.Message;
            NewMessage.SenderId = message.SenderId;
            NewMessage.ReceiverId = message.ReceiverId;
            _messageRepository.Insert(NewMessage);
            _messageRepository.Save();

            int temp = message.ReceiverId;
            message.ReceiverId = message.SenderId;
            message.SenderId = temp;
            Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveNewMessage", message);
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
