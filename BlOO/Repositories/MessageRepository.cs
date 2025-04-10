using BlOO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlOO.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        BlooContext blooContext;
        public MessageRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(Message entity)
        {
            blooContext.messages.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Message message=GetById(id);
            blooContext.messages.Remove(message);
        }

        public List<Message> GetAll()
        {
            return blooContext.messages.ToList();
        }

        public Message GetById(int id)
        {
            return blooContext.messages.FirstOrDefault(m => m.Id == id);
        }

        public void Insert(Message entity)
        {
            blooContext.messages.Add(entity);
            NewMessageNotification(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Message entity)
        {
            blooContext.messages.Update(entity);
        }
        public Message? getLastMessage(int senderID, int receiverID)
        {
            return blooContext.messages
                .Where(m => (m.SenderId == senderID && m.ReceiverId == receiverID) ||
                            (m.SenderId == receiverID && m.ReceiverId == senderID))
                .OrderByDescending(m => m.SendingDate)
                .FirstOrDefault();
        }

        public List<Message> GetChatMessages(int senderID, int reciverID)
        {
            return blooContext.messages.
                Where(msg => (msg.SenderId == senderID && msg.ReceiverId == reciverID) || (msg.SenderId == reciverID && msg.ReceiverId == senderID)).
                OrderBy(msg => msg.SendingDate).AsNoTracking().ToList();

        }
        //================================ Helper Methods ========================\\
        private void NewMessageNotification(Message message)
        {
            Notification notification = new Notification();
            var receiver = blooContext.applicationUsers.FirstOrDefault(u => u.Id == message.ReceiverId);
            var Sender = blooContext.applicationUsers.FirstOrDefault(u => u.Id == message.SenderId);
            notification.UserId = receiver.Id;
            notification.ActorId = message.SenderId;
            notification.NotificationMessage = $"You have a new message from {Sender.FirstName} {Sender.LastName}.";
            notification.ReferenceId = message.Id;
            notification.NotificationType = Models.NotificationType.Message;
            blooContext.notifications.Add(notification);
            blooContext.SaveChanges();
        }
    }
}
