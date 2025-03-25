using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BlOO.Models
{
    public enum NotificationType
    {
        Like,
        Repost,
        Comment,
        Message,
        Follow,
        Post
    }
    [Index(nameof(UserId))]
    [Index(nameof(ReadStatus))]
    [Index(nameof(CreationDate))]
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } // uncompleted
        public int ActorId { get; set; } // uncompleted

        [NotNull]
        public string NotificationMessage { get; set; }
        public int ReferenceId { get; set; } // uncompleted

        [Required] 
        [EnumDataType(typeof(NotificationType))]
        [Column(TypeName = "nvarchar(20)")]
        public NotificationType NotificationType { get; set; }
        /*
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Notification>()
                    .Property(n => n.NotificationType)
                    .HasConversion<string>();
            }

         */

        public DateTime CreationDate { get; set; } = DateTime.UtcNow; 

        public bool ReadStatus { get; set; } = false; 
    }
}
