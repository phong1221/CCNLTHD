using Backend.DTO;
using Backend.DTO.PostDTO;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    
    [ApiController]
    [Route("/test")]
    //[ApiVersion("2.0")]
    //[Route("api/v{version:apiVersion}/test")]
    public class TestController : Controller
    {
        private readonly TestPostService testPostService;

        public TestController(TestPostService testPostService)
        {
            this.testPostService = testPostService;
        }

        [HttpGet("")]
        public IActionResult searchPostAdvance([FromQuery] PostSearch postSearch)
        {
            return Ok(testPostService.searchPostAdvance(postSearch));
        }
        [HttpGet("searchUserAdvanceWithRedis")]
        public IActionResult searchUserAdvanceWithRedis([FromQuery] UserSearchRequest request)
        {
            return Ok(testPostService.searchUserAdvanceWithRedis(request.FullName,request.date1,request.date2, request.page,request.pageSize));
        }
    }
}
