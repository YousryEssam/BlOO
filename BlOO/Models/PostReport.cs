using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{
    public enum ReportReason
    {
        Spam,
        Abuse,
        Inappropriate,
        Misinformation,
        Other
    }
    public enum ReportStatus
    {
        Pending,  
        Reviewed,
        Resolved
    }

    [Index(nameof(Status))]
    [Index(nameof(ReportedAt))]
    public class PostReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Reporter))]
        public int ReporterId { get; set; } // User who reported the post

        [Required]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }  // Post that was reported



        [Required]
        [EnumDataType(typeof(ReportReason))]
        [Column(TypeName = "nvarchar(20)")]
        public ReportReason Reason { get; set; } // The reason for the report


        [Required]
        [EnumDataType(typeof(ReportStatus))]
        [Column(TypeName = "nvarchar(20)")]
        public ReportStatus Status { get; set; } = ReportStatus.Pending; // Default to pending


        [Required]
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow; // When the post was reported


        // Navigation Properties
        public virtual Post Post { get; set; } // The reported post
        public virtual ApplicationUser Reporter { get; set; } // The user who made the report
       
    }
}
