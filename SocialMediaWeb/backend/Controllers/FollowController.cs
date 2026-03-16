using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : Controller
    {
        private readonly IFollowService followService;

        public FollowController(IFollowService followService)
        {
            this.followService = followService;
        }

        [HttpGet("{userId}/followers")]
        public ActionResult GetFollowers(int userId)
        {
            return Ok(followService.GetFollowers(userId));
        }

        [HttpGet("{userId}/followings")]
        public ActionResult GetFollowings(int userId)
        {
            return Ok(followService.GetFollowings(userId));
        }

        [HttpDelete("{followerId}/unfollow/{followingId}")]
        public ActionResult Unfollow(int followerId, int followingId)
        {
            followService.Unfollow(followerId, followingId);
            return NoContent();
        }
    }
}
