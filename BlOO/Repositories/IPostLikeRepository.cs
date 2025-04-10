using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        public void DeleteLikesByPostId(int postId);

        public List<PostLike> GetAllPostLikesByPostId(int postId);
    }
}