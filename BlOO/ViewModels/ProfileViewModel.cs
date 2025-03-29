using BlOO.Models;
namespace BlOO.ViewModels
{
    public class ProfileViewModel
    {
       
        public string? Bio { get; set; }
        public int PostCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public string FirstName { get; set; } = "Yousry";
        public string LastName { get; set; } = "Essam";
        public string UserName { get; set; } = "User_Name";
        public string ProfileImageUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
        public string CoverImageUrl { get; set; } = "/assets/profile-covers/default-cover.jpg";
    }
}
