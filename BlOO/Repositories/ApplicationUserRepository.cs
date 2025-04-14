using BlOO.Models;
using Microsoft.EntityFrameworkCore;

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

        public bool IsAvailableEmail(string email)
        {
            return blooContext.applicationUsers.FirstOrDefault(u => u.NormalizedEmail == email.ToUpper()) == null;
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public List<ApplicationUser> SearchByName(string name)
        {
            List<ApplicationUser> applicationUsers = blooContext.applicationUsers.Include(u => u.Posts).AsEnumerable()
                .Where(u => u.FirstName.ToLower().Contains(name.ToLower())
                || u.LastName.ToLower().Contains(name.ToLower())).ToList();
            return applicationUsers;
        }

        public void Update(ApplicationUser entity)
        {
            blooContext.applicationUsers.Update(entity);
        }
    }
}