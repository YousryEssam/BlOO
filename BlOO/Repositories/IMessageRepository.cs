using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IMessageRepository: IGenericRepository<Message>
    {
       public Message getLastMessage(int senderID,int reciverID);
    }
}
