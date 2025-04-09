namespace BlOO.ViewModels
{
    public class UserFollowersViewModel
    {
        public int UserId { get; set; }
        public List<ApplicationUser> Followers { get; set; }
    }
}
