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

        public List<PostViewModel> GetPostsUsersFollow(int currentUserId)
        {
            List<int> followedUserIds = blooContext.follows
             .Where(f => f.FollowerId == currentUserId) // User
             .Select(f => f.FollowingId)                 // الناس اللي يوزر متابعهم
             .ToList();


            List<PostViewModel> posts = blooContext.posts
                .Include(p => p.User)
                .Where(p => followedUserIds.Contains(p.UserId))
                .OrderByDescending(p => p.PostDate) 
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

        public List<PostViewModel> GetRandomPosts(int currentUserId)
        {
            var followedUserIds = blooContext.follows
                .Where(f => f.FollowerId == currentUserId)
                .Select(f => f.FollowingId)
                .ToList();
    
            var posts = blooContext.posts
                .Include(p => p.User)
                .Where(p =>
                    !followedUserIds.Contains(p.UserId) && // استبعاد اللي متابعهم
                    p.UserId != currentUserId              // استبعاد البوستات بتاعتي
                )
                .OrderBy(r => Guid.NewGuid())
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

        public List<Post> SearchByName(string searchvalue)
        {
            var postRepositories = blooContext.posts.Include(p => p.User).AsEnumerable()
                .Where(p => p.User.FirstName.ToLower().Contains(searchvalue.ToLower()) ||
                p.User.LastName.ToLower().Contains(searchvalue.ToLower())
                ).ToList();
            return postRepositories;
        }

        public Post GetByIdWithComments(int postId)
        {
            return blooContext.posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == postId);
        }
    }
}
