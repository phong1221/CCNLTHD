using Backend.Data;
using Backend.DTO.FollowDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class FollowService : IFollowService
    {
        private readonly BlogDbContext context;

        public FollowService(BlogDbContext context)
        {
            this.context = context;
        }

        public List<FollowResponse> GetFollowers(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userId && u.IsActive);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + userId);
            }

            return context.Follows
                .Where(f => f.FollowingId == userId)
                .Select(f => MapToResponse(f))
                .ToList();
        }

        public List<FollowResponse> GetFollowings(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userId && u.IsActive);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + userId);
            }

            return context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => MapToResponse(f))
                .ToList();
        }

        public void Unfollow(int followerId, int followingId)
        {
            var follower = context.Users.FirstOrDefault(u => u.Id == followerId && u.IsActive);
            if (follower == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + followerId);
            }

            var following = context.Users.FirstOrDefault(u => u.Id == followingId && u.IsActive);
            if (following == null)
            {
                throw new Exception("Người dùng muốn unfollow không tồn tại với id: " + followingId);
            }

            var follow = context.Follows
                .FirstOrDefault(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow == null)
            {
                throw new Exception($"Người dùng {followerId} chưa follow người dùng {followingId}");
            }

            context.Follows.Remove(follow);
            context.SaveChanges();
        }

        private static FollowResponse MapToResponse(Follow follow)
        {
            return new FollowResponse
            {
                Id = follow.Id,
                FollowerId = follow.FollowerId,
                FollowingId = follow.FollowingId,
                CreatedAt = follow.CreatedAt
            };
        }
    }
}
