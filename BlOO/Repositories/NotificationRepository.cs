using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        BlooContext blooContext;
        public NotificationRepository(BlooContext blooContext) {
            this.blooContext = blooContext;
        }
        public void Delete(Notification entity)
        {
            blooContext.notifications.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Notification notification = GetById(id);
            blooContext.notifications.Remove(notification);
        }

        public List<Notification> GetAll()
        {
            return blooContext.notifications.ToList();
        }

        public Notification GetById(int id)
        {
            return blooContext.notifications.FirstOrDefault(n => n.Id == id);
        }

        public async Task<List<Notification>> GetUserNotificationsById(int id)
        {
            return await blooContext.notifications
                .Where(n => n.UserId == id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> HasUnseenNotificationByUserId(int id)
        {
            var notification = await blooContext.notifications.Where(n => n.UserId == id && n.ReadStatus == false).FirstOrDefaultAsync();
            return notification != null;
        }

        public void Insert(Notification entity)
        {
            blooContext.notifications.Add(entity);
        }
        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Notification entity)
        {
            blooContext.notifications.Update(entity);
        }
    }
}
