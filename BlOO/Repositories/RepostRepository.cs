using BlOO.Models;

namespace BlOO.Repositories
{
    public class RepostRepository : IRepostRepository
    {
        BlooContext blooContext;
        public RepostRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(Repost entity)
        {
            blooContext.reposts.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Repost repost = GetById(id);
            blooContext.reposts.Remove(repost);
        }

        public List<Repost> GetAll()
        {
            return blooContext.reposts.ToList();
        }

        public Repost GetById(int id)
        {
            return blooContext.reposts.FirstOrDefault(r => r.Id == id);
        }

        public void Insert(Repost entity)
        {
            blooContext.reposts.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Repost entity)
        {
            blooContext.reposts.Update(entity);
        }
    }
}
