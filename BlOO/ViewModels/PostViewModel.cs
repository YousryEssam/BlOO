using BlOO.Models;

namespace BlOO.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string UserName { get; set; } = "Nour Maged";
        public string UserImgUrl { get; set; } = "/assets/profile-pictures/default-user.jpg";
        public string Content { get; set; }
        public string? ImgUrl { get; set; } 
        public int LikeCount { get; set; }
        public int CommentCount { get; set; } 
        public int RepostCount { get; set; }

        public List<CommentWithUserDataViewModel> Comments { get; set; } = new List<CommentWithUserDataViewModel>();
        public List<PostLikesViewModel> PostLikes { get; set; } = new List<PostLikesViewModel>();



    }
}
