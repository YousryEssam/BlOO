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

        public async Task<Follow> GetByUsersIds(int userId, int profileId)
        {
            return await blooContext.follows.FirstOrDefaultAsync(f=> f.FollowingId == profileId && f.FollowerId == userId);
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

        public async Task<List<int>> GetUserFollowersIds(int userId)
        {
            return await blooContext.follows
                .Where(f => f.FollowingId == userId)
                .Select(f => f.FollowerId)
                .ToListAsync();
        }

        public async Task<List<int>> GetUserFollowingIds(int userId)
        {
            return await blooContext.follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToListAsync();
        }

        public void Insert(Follow entity)
        {
            blooContext.follows.Add(entity);
            NewFollowNotification(entity);
        }

        public bool IsFollowing(int userId, int profileId)
        {
            var follow = blooContext.follows.FirstOrDefault(f => f.FollowerId == userId && f.FollowingId == profileId);
            if (follow != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsFollowingAsync(int userId, int profileId)
        {
            var follow = await blooContext.follows
                .FirstOrDefaultAsync(f => f.FollowerId == userId && f.FollowingId == profileId);

            return follow != null;
        }

        public void Save()
        {
            blooContext.SaveChanges();
        }

        public void Update(Follow entity)
        {
            blooContext.follows.Update(entity);
        }

        //================================ Helper Methods ========================\\
        private void NewFollowNotification(Follow follow)
        {
            Notification notification = new Notification();
            var follower = blooContext.applicationUsers.FirstOrDefault(u => u.Id == follow.FollowerId);
            var following = blooContext.applicationUsers.FirstOrDefault(u => u.Id == follow.FollowingId);
            notification.UserId = following.Id;
            notification.ActorId = follower.Id;
            notification.NotificationMessage = $"{follower.FirstName} {follower.LastName} started following you.";
            notification.ReferenceId = follow.Id;
            notification.NotificationType = Models.NotificationType.Follow;
            blooContext.notifications.Add(notification);
            blooContext.SaveChanges();
        }
    }
}
