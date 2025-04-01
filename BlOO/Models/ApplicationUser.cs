using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlOO.Models;
using Microsoft.AspNetCore.Identity;

namespace BlOO.Models
{
    public enum AccountStatus
    {
        Active,
        Suspended,
        Banned,
        Deleted
    }

    public class ApplicationUser //: IdentityUser<int>
    {
        [Required]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "First name cannot exceed 20 characters.")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Last name cannot exceed 20 characters.")]
        public string LastName { get; set; }
        public string? Bio { get; set; }
        public string ProfileImageUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
        public string CoverImageUrl { get; set; } = "/assets/profile-covers/default-cover.jpg";
        public int PostCount { get; set; } 
        public int FollowersCount { get; set; } 
        public int FollowingCount { get; set; } 
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        [EnumDataType(typeof(AccountStatus))]
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        // Navigation Properties
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Repost> Reposts { get; set; } = new List<Repost>();
        public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public virtual ICollection<Follow> Following { get; set; } = new List<Follow>();
        public virtual ICollection<PostLike> LikedPosts { get; set; } = new List<PostLike>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public virtual ICollection<PostReport> ReportsMade { get; set; } = new List<PostReport>();
        public virtual ICollection<CommentLike> LikedComments { get; set; } = new List<CommentLike>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>(); 
        public virtual ICollection<Notification> NotificationsSent { get; set; } = new List<Notification>();


    }
}
