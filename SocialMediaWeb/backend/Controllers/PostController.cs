using Backend.DTO;
using Backend.DTO.PostDTO;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        public readonly PostService postService;

        public PostController(PostService postService)
        {
            this.postService = postService;
        }
        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(postService.GetAll());
        }
        [HttpPost]
        public IActionResult create(PostRequest requet)
        {
            var post=postService.createPost(requet);
            return Ok(post);
        }
        [HttpPut("{id}")]
        public IActionResult update(int id, PostRequest requet)
        {
            var post = postService.Update(id, requet);
            return Ok(post);
        }
        [HttpDelete]
        public IActionResult delete(int id) { 
            postService.Delete(id);
            return NoContent();
        }
        [HttpGet("page")]
        public IActionResult getPage([FromQuery]  PageRequest pageRequest) { 
            return Ok(postService.GetPage(pageRequest.page,pageRequest.pageSize));
        }
        [HttpGet("search")]
        public IActionResult search([FromQuery] PostSearch postSearch)
        {
            return Ok(postService.search(postSearch));
        }
    }
}
