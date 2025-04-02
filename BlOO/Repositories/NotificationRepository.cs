using BlOO.Models;

namespace BlOO.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        BlooContext blooContext;
        public NotificationRepository(BlooContext blooContext) {
        this.blooContext=blooContext;
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
