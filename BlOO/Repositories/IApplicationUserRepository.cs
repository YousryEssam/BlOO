using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        List<ApplicationUser> SearchByName(string name);
    }
}
