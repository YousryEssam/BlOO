using BlOO.Models;
using Microsoft.EntityFrameworkCore;

namespace BlOO.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        BlooContext blooContext;
        public FollowRepository(BlooContext blooContext)
        {
            this.blooContext = blooContext;
        }

        public void Delete(Follow entity)
        {
            blooContext.follows.Remove(entity);
        }

        public void DeleteById(int id)
        {
            Follow follow = GetById(id);
            blooContext.follows.Remove(follow);
        }

        public List<Follow> GetAll()
        {
            return blooContext.follows.ToList();
        }

        public Follow GetById(int id)
        {
            return blooContext.follows.FirstOrDefault(f => f.Id == id);
        }


        public async Task<List<int>> GetFollowedUsersAsync(int userId, int pageNumber, int pageSize)
        {
            return await blooContext.follows
                .Where(f => f.FollowerId == userId)
                .OrderBy(f => f.FollowingDate)  // لضمان ترتيب البيانات
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => f.FollowingId)
                .ToListAsync();
        }
        public void Insert(Follow entity)
        {
            blooContext.follows.Add(entity);
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Follow entity)
        {
            blooContext.follows.Update(entity);
        }
    }
}
