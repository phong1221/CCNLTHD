using Backend.DTO;
using Backend.DTO.UserDTO;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            return Ok(userService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            return Ok(userService.GetById(id));
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] UserUpdateRequest request)
        {
            var user = userService.Update(id, request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            userService.Delete(id);
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult SearchUsers([FromQuery] string keyword)
        {
            return Ok(userService.Search(keyword));
        }

        [HttpGet("page")]
        public ActionResult GetPage([FromQuery] PageRequest pageRequest)
        {
            return Ok(userService.GetPage(pageRequest.page, pageRequest.pageSize));
        }

        [HttpPut("{id}/change-password")]
        public ActionResult ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            var user = userService.ChangePassword(id, request);
            return Ok(user);
        }
    }
}
