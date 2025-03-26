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
    
        [MaxLength(100)]
        public string DisplayName { get; set; }

        public string ProfileImageUrl { get; set; }
        public string BannerImageUrl { get; set; }

        public int FollowerCount { get; set; } 
        public int FollowingCount { get; set; } 
        public int PostCount { get; set; } 

        public string Bio { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        public List<Post> Posts { get; set; }
    }

}
