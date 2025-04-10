using BlOO.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlOO.Hubs
{
    public class NotificationsSystemHub : Hub 
    {
        IFollowRepository followsRepository;
        INotificationRepository notificationRepository;
        IApplicationUserRepository applicationUserRepository;
        public NotificationsSystemHub(IFollowRepository followsRepository, 
            IApplicationUserRepository applicationUserRepository, INotificationRepository notificationRepository) 
        {
            this.followsRepository = followsRepository;
            this.notificationRepository = notificationRepository;
            this.applicationUserRepository = applicationUserRepository;
        }
        public async Task DeleteNotification(int notificationId, int userId)
        {
            Notification notification = notificationRepository.GetById(notificationId);
            if (notification == null || notification.UserId != userId)
                return;
            notificationRepository.Delete(notification);
            notificationRepository.Save();
            await HasNoNotification(userId);
        }

        public async Task ReadNotification(int notificationId, int userId) {
            Notification notification = notificationRepository.GetById(notificationId);
            if (notification == null || notification.UserId != userId)
            {
                return;
            }
            notification.ReadStatus = true;
            notificationRepository.Update(notification);
            notificationRepository.Save();
            await HasNoNotification(userId);
        }

        //=======================Helpers==============================\\
        private async Task HasNoNotification(int id)
        {
            var hasUnseenNotifications = await notificationRepository.HasUnseenNotificationByUserId(id);
            if (hasUnseenNotifications)
            {
                return;
            }
            await Clients.Caller.SendAsync("NoMoreUnseenNotifications");
        }

    }
}
