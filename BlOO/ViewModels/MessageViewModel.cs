using System.ComponentModel.DataAnnotations;

namespace BlOO.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "Madonna";
        public string LastName { get; set; } = "Hany";
        public string ProfileImageUrl { get; set; } = "/assets/icons/user.png";
        [Required]
        public string Content { get; set; } = "How are you ?";
        public bool MessageSeen { get; set; } = false;
        public DateTime SendingDate { get; set; } = DateTime.UtcNow;
    }
}
