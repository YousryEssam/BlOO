using BlOO.Models;

namespace BlOO.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "Nour Maged";
        public string UserImgUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
        public string Content { get; set; }
        public string ImgUrl { get; set; } = "/assets/post-pictures/post-img.jpeg";
        public int LikeCount { get; set; }
        public int CommentCount { get; set; } 
        public int RepostCount { get; set; }
    }
}
