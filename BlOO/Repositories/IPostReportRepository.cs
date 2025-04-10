using BlOO.Models;

namespace BlOO.Repositories
{
    public interface IPostReportRepository : IGenericRepository<PostReport>
    {
        public void DeleteReportsByPostId(int postId);

    }
}
