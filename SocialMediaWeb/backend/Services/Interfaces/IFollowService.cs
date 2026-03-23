using Backend.DTO.FollowDTO;

namespace Backend.Services.Interfaces
{
    public interface IFollowService
    {
        List<FollowResponse> GetFollowers(int userId);

        List<FollowResponse> GetFollowings(int userId);

        void FollowUser(int followerId, int followingId);

        void Unfollow(int followerId, int followingId);
    }
}
