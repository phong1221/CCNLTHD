using Backend.DTO.AuthDTO;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public ActionResult<AuthResponse> Register([FromBody] RegisterRequest request)
        {
            var response = authService.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public ActionResult<AuthResponse> Login([FromBody] LoginRequest request)
        {
            var response = authService.Login(request);
            return Ok(response);
        }
    }
}
