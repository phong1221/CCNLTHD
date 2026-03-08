using Backend.DTO;
using Backend.DTO.Category;

namespace Backend.Services.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryResponse> GetAll();
        PageResult<CategoryResponse>GetPage(int page,int pageSize);
        CategoryResponse GetCategory(int id);
        CategoryResponse createCategory(CategoryRequest categoryRequest);
        CategoryResponse Update(int  id, CategoryRequest request);
        void Delete(int id);

    }
}
