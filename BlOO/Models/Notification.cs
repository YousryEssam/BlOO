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
        Post ,
        Report
    }
    [Index(nameof(UserId))]
    [Index(nameof(ReadStatus))]
    [Index(nameof(CreationDate))]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Actor))]
        public int ActorId { get; set; }

        [Required]
        public string NotificationMessage { get; set; }
        
        [Required]
        public int ReferenceId { get; set; }

        [Required]
        [EnumDataType(typeof(NotificationType))]
        [Column(TypeName = "nvarchar(20)")]
        public NotificationType NotificationType { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public bool ReadStatus { get; set; } = false;

        // Navigation Properties
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Actor { get; set; }
    }
}
