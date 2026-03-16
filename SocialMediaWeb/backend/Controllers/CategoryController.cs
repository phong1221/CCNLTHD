using Backend.DTO;
using Backend.DTO.CategoryDTO;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        public readonly CategoryServicecs categoryServicecs;

        public CategoryController(CategoryServicecs categoryServicecs)
        {
            this.categoryServicecs = categoryServicecs;
        }

        [HttpGet]

        public ActionResult GetAllCategory()
        {
            return Ok(categoryServicecs.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetCategory(int id)
        {
            return Ok(categoryServicecs.GetCategory(id));
        }
        [HttpPost]
        public ActionResult createCategory(CategoryRequest categoryRequest)
        {

            CategoryResponse category=categoryServicecs.createCategory(categoryRequest);
            return Ok(category);
        }
        [HttpPut("{id}")]
        public ActionResult updateCategory(int id, [FromBody] CategoryRequest categoryRequest)
        {
            CategoryResponse category= categoryServicecs.Update(id, categoryRequest);
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public ActionResult deleteCategory(int id)
        {
            categoryServicecs.Delete(id);
            return NoContent();
        }
        [HttpGet("page")]
        public IActionResult getPage([FromQuery] PageRequest pageRequest)
        {
            return Ok(categoryServicecs.GetPage(pageRequest.page, pageRequest.pageSize));
        }
    }
}
