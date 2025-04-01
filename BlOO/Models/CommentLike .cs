using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{
    [Index(nameof(UserId),nameof(CommentId),IsUnique =true)]
    public class CommentLike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Comment Comment { get; set; }
        public ApplicationUser User { get; set; }
    }
}
