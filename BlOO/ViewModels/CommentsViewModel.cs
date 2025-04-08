using System.ComponentModel.DataAnnotations.Schema;

namespace BlOO.ViewModels
{
    public class CommentsViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
     
        public DateTime CommentDate { get; set; } = DateTime.Now;

        public int LikeCount { get; set; } = 0;
        public int PostId { get; set; }

        public int UserId { get; set; }

    
    }
}
