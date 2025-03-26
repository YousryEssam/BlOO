using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Required]
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public int LikeCount { get; set; } = 0;
        public int PostId { get; set; }
        public int UserId { get; set; }
        //[ForeignKey("PostId")]
        //public virtual Post? post { get; set; }
        //[ForeignKey("UserId")]
        //public virtual User? user { get; set; }
    }
}
