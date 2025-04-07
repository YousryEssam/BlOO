using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        List<PostViewModel> GetPostsUsersFollow(int currentUserId);
        List<PostViewModel> GetRandomPosts(int currentUserId);

        List<PostViewModel> GetAllPostsWithId(int id);


    }
}
