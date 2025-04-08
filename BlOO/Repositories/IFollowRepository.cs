using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public interface IFollowRepository: IGenericRepository<Follow>
    {
        public  Task<List<int>> GetFollowedUsersAsync(int userId, int pageNumber, int pageSize);
        public bool IsFollowing(int userId, int profileId);
        public Task<bool> IsFollowingAsync(int userId, int profileId);
        public Task<Follow> GetByUsersIds(int userId, int profileId);
    }
}
