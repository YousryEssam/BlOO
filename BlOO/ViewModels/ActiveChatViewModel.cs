namespace BlOO.ViewModels
{
    public class ActiveChatViewModel
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "Yousry";
        public string LastName { get; set; } = "Essam";
        public string ProfileImageUrl { get; set; } = "/assets/icons/user.png";
        public List<Message> messages { get; set; } = new List<Message>();
        public ActiveChatViewModel() { }
        public ActiveChatViewModel(ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfileImageUrl = user.ProfileImageUrl;
        }
    }
}
