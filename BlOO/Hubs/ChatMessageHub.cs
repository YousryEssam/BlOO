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
