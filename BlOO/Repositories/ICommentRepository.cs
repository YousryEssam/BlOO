using BlOO.Models;

namespace BlOO.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public void DeleteByPostId(int postId);
        public List<Comment> GetCommmentsByPostId(int postId);
    }
}
