using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        List<PostViewModel> GetAllPostsWithUsers();
        List<PostViewModel> GetAllPostsWithId(int id);


    }
}
