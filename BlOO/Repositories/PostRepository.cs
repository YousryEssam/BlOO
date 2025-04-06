using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public class PostRepository : IPostRepository
    {
        BlooContext blooContext;
        public PostRepository(BlooContext blooContext) { 
            this.blooContext = blooContext;   
        }
        public void Delete(Post entity)
        {
            blooContext.posts.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Post post = GetById(id);
            blooContext.posts.Remove(post);
        }

        public List<Post> GetAll()
        {
            return blooContext.posts.ToList();
        }


        public List<PostViewModel> GetAllPostsWithUsers()
        {
            List<PostViewModel> posts = blooContext.posts
                .Include(p => p.User)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    OwnerId = p.UserId,
                    UserName = p.User.FirstName + " " + p.User.LastName,
                    UserImgUrl = p.User.ProfileImageUrl,
                    Content = p.Content,
                    ImgUrl = p.ImgUrl,
                    LikeCount = p.LikeCount,
                    CommentCount = p.CommentCount
                })
                .ToList();

            return posts;
        }
        public List<PostViewModel> GetAllPostsWithId(int id)
        {
            List<PostViewModel> posts = blooContext.posts
                .Include(p => p.User)
                .Where(p => p.UserId==id)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    OwnerId = p.UserId,
                    UserName = p.User.FirstName + " " + p.User.LastName,
                    UserImgUrl = p.User.ProfileImageUrl,
                    Content = p.Content,
                    ImgUrl = p.ImgUrl,
                    LikeCount = p.LikeCount,
                    CommentCount = p.CommentCount
                })
                .ToList();

            return posts;
        }

        public Post GetById(int id)
        {
            return blooContext.posts.FirstOrDefault(p => p.Id == id);
        }

        public void Insert(Post entity)
        {
            blooContext.posts.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Post entity)
        {
            blooContext.posts.Update(entity);
        }
    }
}
