namespace BlOO.ViewModels
{
    public class ChatsViewModel
    {
        public int Id { get; set; }
        public int TargetUserId { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; } = "/assets/icons/user.png";
        public string ReciverImageURL { get; set; }
        public List<Message>? OpenChat {  get; set; }
        public bool MessageSeen { get; set; } = false;
        public DateTime SendingDate { get; set; } = DateTime.UtcNow;
        public ActiveChatViewModel ActiveChat { get; set; } = new ActiveChatViewModel();
        public List<ConversationViewModel> Conversations { get; set; } = new List<ConversationViewModel>();
        public ChatsViewModel() { }

        public ChatsViewModel(ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfileImageUrl = user.ProfileImageUrl;
        }
        public ChatsViewModel(ApplicationUser user, ApplicationUser TargetUser)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ProfileImageUrl = user.ProfileImageUrl;
            this.TargetUserId = TargetUser.Id;
            ActiveChat = new ActiveChatViewModel(TargetUser);
            ReciverImageURL = TargetUser.ProfileImageUrl;
        }
    }
}
