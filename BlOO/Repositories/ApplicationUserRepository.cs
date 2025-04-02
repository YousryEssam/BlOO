using BlOO.Models;

namespace BlOO.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        BlooContext blooContext;
        public ApplicationUserRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(ApplicationUser entity)
        {
            blooContext.applicationUsers.Remove(entity);
        }

        public void DeleteById(int id)
        {
            ApplicationUser applicationUser = GetById(id);
            blooContext.applicationUsers.Remove(applicationUser);
        }

        public List<ApplicationUser> GetAll()
        {
            return blooContext.applicationUsers.ToList();
        }
        public ApplicationUser GetById(int id)
        {
            return blooContext.applicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public void Insert(ApplicationUser entity)
        {
            blooContext.applicationUsers.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(ApplicationUser entity)
        {
            blooContext.applicationUsers.Update(entity);
        }
    }
}