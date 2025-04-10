using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.ViewModels
{
    public class CommentWithUserDataViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public int LikeCount { get; set; } = 0;      
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = "First Name";
        public string LastName { get; set; } = "Last Name";
        public string ProfileImageUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
    }
}
