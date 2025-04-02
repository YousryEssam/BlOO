using BlOO.Models;

namespace BlOO.Repositories
{
    public class PostReportRepository : IPostReportRepository
    {
        BlooContext blooContext;
        public PostReportRepository(BlooContext blooContext) {
            this.blooContext = blooContext;
        }
        public void Delete(PostReport entity)
        {
            blooContext.postReports.Remove(entity);
        }

        public void DeleteById(int id)
        {
            PostReport postReport = GetById(id);
            blooContext.postReports.Remove(postReport);
        }

        public List<PostReport> GetAll()
        {
            return blooContext.postReports.ToList();
        }

        public PostReport GetById(int id)
        {
            return blooContext.postReports.FirstOrDefault(p => p.Id == id);
        }

        public void Insert(PostReport entity)
        {
            blooContext.postReports.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(PostReport entity)
        {
            blooContext.postReports.Update(entity);
        }
    }
}
