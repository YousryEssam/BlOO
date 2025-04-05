using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public interface IFollowRepository: IGenericRepository<Follow>
    {
        public  Task<List<int>> GetFollowedUsersAsync(int userId, int pageNumber, int pageSize);


    }
}
