using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlOO.Models
{
    public class Repost
    {
        [Key]
        public int Id { get; set; }

        public string? AddedContent { get; set; } 

        [Required]
        public DateTime RepostDate { get; set; } = DateTime.UtcNow; 

        [Required]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; } 

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; } 

        // Navigation Properties
        public virtual Post Post { get; set; } 
        public virtual ApplicationUser User { get; set; }
    }
}
