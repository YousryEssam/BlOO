namespace BlOO.ViewModels
{
    public class UserFollowingViewModel
    {
        public int UserId { get; set; }
        public List<ApplicationUser> Following { get; set; }
    }
}
