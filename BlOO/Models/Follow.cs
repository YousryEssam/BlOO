using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.Models
{
    // Ensures a user cannot follow another user multiple times
    [Index(nameof(FollowerId), nameof(FollowingId), IsUnique = true)]
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(FollowerUser))]
        public int FollowerId {get; set;}

        [Required]
        [ForeignKey(nameof(FollowingUser))]
        public int FollowingId { get; set; }

        public DateTime FollowingDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [InverseProperty("Followers")]
        public virtual ApplicationUser FollowerUser { get; set; } // The one who follows

        [InverseProperty("Following")]
        public virtual ApplicationUser FollowingUser { get; set; } // The one being followed

    }
}
