using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IMessageRepository: IGenericRepository<Message>
    {
        Message getLastMessage(int senderID,int reciverID);
        List<Message> GetChatMessages(int senderID, int reciverID);
    }
}
