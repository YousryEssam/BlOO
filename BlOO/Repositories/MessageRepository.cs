using BlOO.Models;

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
                .OrderByDescending(m => m.SendingDate) // ترتيب تنازلي حسب أحدث رسالة
                .FirstOrDefault(); // استخدام FirstOrDefault بدلاً من LastOrDefault
        }
      



    }
}
