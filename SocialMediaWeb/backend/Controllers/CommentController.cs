using Backend.DTO;
using Backend.DTO.CommentDTO;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        public ActionResult GetAllComments()
        {
            return Ok(commentService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult GetComment(int id)
        {
            return Ok(commentService.GetById(id));
        }

        [HttpPost]
        public ActionResult CreateComment([FromBody] CommentRequest request)
        {
            CommentResponse comment = commentService.Create(request);
            return Ok(comment);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateComment(int id, [FromBody] CommentRequest request)
        {
            CommentResponse comment = commentService.Update(id, request);
            return Ok(comment);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteComment(int id)
        {
            commentService.Delete(id);
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult SearchComments([FromQuery] string keyword)
        {
            return Ok(commentService.Search(keyword));
        }

        [HttpGet("page")]
        public ActionResult GetPage([FromQuery] PageRequest pageRequest)
        {
            return Ok(commentService.GetPage(pageRequest.page, pageRequest.pageSize));
        }
    }
}
