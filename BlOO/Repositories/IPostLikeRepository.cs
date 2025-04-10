using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {

        public List<PostLike> GetAllPostLikesByPostId(int postId);
    }
}