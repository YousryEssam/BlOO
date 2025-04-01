using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{

    [Index(nameof(UserId), nameof(PostId), IsUnique = true)]
    public class PostLike
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }

        [Required]
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
