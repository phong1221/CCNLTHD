using Backend.DTO;
using Backend.DTO.CategoryDTO;
using Backend.DTO.PostDTO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Backend.Services.Interfaces
{
    public interface IPostService
    {
        List<PostResponse> GetAll();
        PageResult<PostResponse> GetPage(int page, int pageSize);
        PostResponse GetPost(int id);
        PostResponse createPost(PostRequest postRequest);
        PostResponse Update(int id, PostRequest request);
        void Delete(int id);
        PageResult<PostResponse> search(PostSearch postSearch);
    }
}
