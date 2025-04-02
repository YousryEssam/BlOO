using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Models
{
    public enum PostStatus
    {
        Active,
        Deleted
    }
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }


        [MaxLength(500, ErrorMessage = "Post content cannot exceed 500 characters.")]
        public string? Content { get; set; }
        
        public string? ImgUrl { get; set; }
        
        public DateTime PostDate { get; set; } = DateTime.UtcNow;

        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int RepostCount { get; set; } = 0;


        [Required]
        [EnumDataType(typeof(PostStatus))]
        [Column(TypeName = "nvarchar(10)")]
        public PostStatus Status { get; set; } = PostStatus.Active;
        
        public DateTime? DeletedAt { get; set; }

        // Navigation Property
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Repost> Reposts { get; set; }
        public virtual ICollection<PostLike> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostReport> Reports { get; set; }
    }
}
