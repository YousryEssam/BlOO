using BlOO.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace BlOO.Hubs
{
    public class FollowersSystemHub : Hub
    {
        IFollowRepository followsRepository;
        IApplicationUserRepository applicationUserRepository;

        public FollowersSystemHub(IFollowRepository followsRepository, IApplicationUserRepository applicationUserRepository)
        {
            this.followsRepository = followsRepository;
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task IsFollower(int profileId , int userId )
        {
            bool isFollower = await followsRepository.IsFollowingAsync(userId, profileId);
            await Clients.Client(Context.ConnectionId).SendAsync("CheekFollower", isFollower);
        }

        public async Task FollowUser(int profileId, int userId) {
            var followingUser = applicationUserRepository.GetById(userId);
            var followedUser = applicationUserRepository.GetById(profileId);
            followedUser.FollowersCount++;
            followingUser.FollowingCount++;
            applicationUserRepository.Update(followedUser);
            applicationUserRepository.Update(followingUser);
            applicationUserRepository.Save();

            Follow newFollow = new Follow();
            newFollow.FollowerId = userId;
            newFollow.FollowingId = profileId;
            followsRepository.Insert(newFollow);
            followsRepository.Save();

            await Clients.Client(Context.ConnectionId).SendAsync("ChangeFollowStata");
        }
        public async Task UnfollowUser(int profileId, int userId)
        {
            var followingUser = applicationUserRepository.GetById(userId);
            var followedUser = applicationUserRepository.GetById(profileId);
            followedUser.FollowersCount--;
            followingUser.FollowingCount--;
            applicationUserRepository.Update(followedUser);
            applicationUserRepository.Update(followingUser);
            applicationUserRepository.Save();

            Follow follow = await followsRepository.GetByUsersIds(userId, profileId);
            followsRepository.Delete(follow);
            followsRepository.Save();

            await Clients.Client(Context.ConnectionId).SendAsync("ChangeFollowStata");
        }
    }
}
