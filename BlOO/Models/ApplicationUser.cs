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

    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
        public string CoverImageUrl { get; set; } = "/assets/profile-covers/default-cover.jpg";

        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; } 
        public int PostCount { get; set; } 

        public string? Bio { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        public List<Post> Posts { get; set; }
    }

}
