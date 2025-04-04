namespace BlOO.ViewModels
{
    public class ChatsViewModel
    {
        public int Id { get; set; }
        public int TargetUserId { get; set; } = 0;
        public string FirstName { get; set; } = "Madonna";
        public string LastName { get; set; } = "Hany";
        public string ProfileImageUrl { get; set; } = "/assets/icons/user.png";
        [Required]
        public string Content { get; set; } = "How are you ?";
        public bool MessageSeen { get; set; } = false;
        public DateTime SendingDate { get; set; } = DateTime.UtcNow;

        public ChatsViewModel() { }

        public ChatsViewModel(ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfileImageUrl = user.ProfileImageUrl;
        }
        public ChatsViewModel(ApplicationUser user, int TargetUserId)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfileImageUrl = user.ProfileImageUrl;
            this.TargetUserId = TargetUserId;

        }
    }
}
