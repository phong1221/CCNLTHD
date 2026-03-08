
using Backend.DTO;
using Backend.DTO.ReactDTO;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactController : Controller
    {
        public readonly ReactService reactService;

        public ReactController(ReactService reactService)
        {
            this.reactService = reactService;
        }
        [HttpGet("{id}")]
        public ActionResult GetAllByPost(int id)
        {
            return Ok(reactService.GetAllByPost(id));
        }
        [HttpPost]
        public ActionResult createReact(ReactionRequest reactionRequest)
        {
            ReactionResponse response=reactService.create(reactionRequest);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public ActionResult deleteReact(int id)
        {
            reactService.Delete(id);
            return NoContent();
        }
        
    }
}
