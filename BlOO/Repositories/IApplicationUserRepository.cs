using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        bool IsAvailableEmail(string email);
        List<ApplicationUser> SearchByName(string name);
    }
}
