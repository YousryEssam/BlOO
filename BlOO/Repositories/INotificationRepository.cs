using BlOO.Models;

namespace BlOO.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        int GetNumberOfUserNotificationsById(int id);
        Task<bool> HasUnseenNotificationByUserId(int id);
        Task<List<Notification>> GetUserNotificationsById(int id);
        Task<List<Notification>> GetUserNotificationsById(int id, int pageNumber, int size);
    }
}