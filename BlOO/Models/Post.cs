using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } 


        public string Content { get; set; }

        public string ImgUrl { get; set; }
        public DateTime PostDate { get; set; } = DateTime.UtcNow;
        public int LikeCount { get; set; } 
        public int CommentCount { get; set; } 
        public int RepostCount { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }
}
