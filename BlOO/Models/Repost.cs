using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Repost
    {
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string? AddedContent { get; set; }
        [Required]
        public DateTime RepostDate { get; set; } = DateTime.Now;
        public int PostId { get; set; }
        public int UserId { get; set; }
        //[ForeignKey("PostId")]
        //public virtual Post? post { get; set; }
        //[ForeignKey("UserId")]
        //public virtual User? user { get; set; }
    }
}
