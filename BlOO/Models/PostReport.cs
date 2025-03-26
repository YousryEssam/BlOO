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
        public int Id { get; set; }
        public int ReporterId { get; set; } // uncompleted
        public int PostId { get; set; }  // uncompleted

        [Required]
        [EnumDataType(typeof(ReportReason))]
        [Column(TypeName = "nvarchar(20)")]
        public ReportReason Reason { get; set; } // uncompleted
        [Required]
        [EnumDataType(typeof(ReportStatus))]
        [Column(TypeName = "nvarchar(20)")]
        public ReportStatus Status { get; set; } = ReportStatus.Pending; // uncompleted

        /*
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<PostReport>()
                    .Property(e => e.Status)
                    .HasConversion<string>();

                modelBuilder.Entity<PostReport>()
                    .Property(e => e.Reason)
                    .HasConversion<string>();
            }
         */

        [Required]
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

    }
}
