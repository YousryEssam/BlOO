using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime CommentDate { get; set; } = DateTime.Now;
       
        public int LikeCount { get; set; } = 0;
      
        [Required]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        
        // Navigation Properties
        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
    }
}
