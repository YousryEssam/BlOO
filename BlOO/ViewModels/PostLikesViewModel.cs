namespace BlOO.ViewModels
{
    public class PostLikesViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = "First Name";
        public string LastName { get; set; } = "Last Name";
        public string ProfileImageUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
    }
}
