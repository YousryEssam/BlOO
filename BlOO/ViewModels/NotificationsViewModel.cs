using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.ViewModels
{
    public enum NotificationType
    {
        Like,
        Repost,
        Comment,
        Message,
        Follow,
        Post,
        Report
    }
    public class NotificationsViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; } // user 
        public int ActorId { get; set; } // user did the action 
        public int ReferenceId { get; set; } // id of (post , user follow , comment .....)
        public DateTime CreationDate { get; set; }
        public bool ReadStatus { get; set; } = false;
        public string NotificationMessage { get; set; }
        public NotificationType NotificationTypeContent { get; set; }

        public NotificationsViewModel(Notification notification)
        {
            this.Id = notification.Id;
            this.UserId = notification.UserId;
            this.ActorId = notification.ActorId;
            this.ReferenceId = notification.ReferenceId;
            this.NotificationMessage = notification.NotificationMessage;
            this.ReadStatus = notification.ReadStatus;
            this.CreationDate = notification.CreationDate;
            this.NotificationTypeContent = (NotificationType)notification.NotificationType;
        }

        public string FormatTime()
        {
            var now = DateTime.UtcNow;
            var diff = now - CreationDate.ToUniversalTime();

            if (diff.TotalSeconds < 60)
            {
                return "Just now";
            }
            else if (diff.TotalMinutes < 60)
            {
                int minutes = (int)Math.Floor(diff.TotalMinutes);
                return $"{minutes} minute{(minutes > 1 ? "s" : "")} ago";
            }
            else if (diff.TotalHours < 24)
            {
                int hours = (int)Math.Floor(diff.TotalHours);
                return $"{hours} hour{(hours > 1 ? "s" : "")} ago";
            }
            else
            {
                int days = (int)Math.Floor(diff.TotalDays);
                return $"{days} day{(days > 1 ? "s" : "")} ago";
            }
        }
    }
}
