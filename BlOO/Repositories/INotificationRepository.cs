using BlOO.Models;

namespace BlOO.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<bool> HasUnseenNotificationByUserId(int id);
        Task<List<Notification>> GetUserNotificationsById(int id);
    }
}