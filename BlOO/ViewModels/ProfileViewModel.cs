namespace BlOO.ViewModels
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public int PostCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public string FirstName { get; set; } = "First Name";
        public string LastName { get; set; } = "Last Name";
        public string UserName { get; set; } = "User_Name";
        public string ProfileImageUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
        public string CoverImageUrl { get; set; } = "/assets/profile-covers/default-cover.jpg";

        public List<PostViewModel> Posts { get; set; }

        public ProfileViewModel() { }
        public ProfileViewModel(ApplicationUser user)
        {
            this.Bio = user.Bio;
            this.UserId = user.Id;
            this.LastName = user.LastName;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.FollowersCount = user.FollowersCount;
            this.FollowingCount = user.FollowingCount;
            this.PostCount = user.PostCount;
            this.CoverImageUrl = user.CoverImageUrl;
            this.ProfileImageUrl = user.ProfileImageUrl;
        }
    }
}
