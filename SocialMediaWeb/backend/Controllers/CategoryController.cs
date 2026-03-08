using Backend.DTO.Category;
using Backend.Services;
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
        public ActionResult GetProduct(int id)
        {
            return Ok(categoryServicecs.GetCategory(id));
        }
        [HttpPost]
        public ActionResult createProduct(CategoryRequest categoryRequest)
        {

            CategoryResponse category=categoryServicecs.createCategory(categoryRequest);
            return Ok(category);
        }
        [HttpPut("{id}")]
        public ActionResult updateProduct(int id, [FromBody] CategoryRequest categoryRequest)
        {
            CategoryResponse category= categoryServicecs.Update(id, categoryRequest);
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public ActionResult deleteProduct(int id)
        {
            categoryServicecs.Delete(id);
            return NoContent();
        }
    }
}
