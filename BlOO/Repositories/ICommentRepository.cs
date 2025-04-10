using BlOO.Models;

namespace BlOO.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public List<Comment> GetCommmentsByPostId(int postId);
    }
}
