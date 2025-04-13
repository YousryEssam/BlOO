using BlOO.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

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

        public  int GetNumberOfUserNotificationsById(int id)
        {
            return blooContext.notifications.AsNoTracking().ToList().Count;
        }

        public async Task<List<Notification>> GetUserNotificationsById(int id)
        {
            return await blooContext.notifications
                .Where(n => n.UserId == id)
                .AsNoTracking()
                .OrderByDescending(n => n.CreationDate)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUserNotificationsById(int id, int pageNumber, int size)
        {
            return await blooContext.notifications
                .Where(n => n.UserId == id)
                .AsNoTracking()
                .OrderByDescending(n => n.CreationDate)
                .Skip(pageNumber * size)
                .Take(size)
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
