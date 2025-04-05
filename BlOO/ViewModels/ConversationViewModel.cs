namespace BlOO.ViewModels
{
    public class ConversationViewModel
    {

        public string UserImgURL { get; set; }
        public string FirstName { get; set; } = "Yousry";
        public string LastName { get; set; } = "Essam";
        public string LastMessage { get; set; } = "";
        public DateTime SendingDate { get; set; } = DateTime.UtcNow;
    }
}
