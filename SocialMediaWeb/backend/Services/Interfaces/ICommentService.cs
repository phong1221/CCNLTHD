using Backend.DTO;
using Backend.DTO.CommentDTO;

namespace Backend.Services.Interfaces
{
    public interface ICommentService
    {
        List<CommentResponse> GetAll();
        CommentResponse GetById(int id);
        CommentResponse Create(CommentRequest request);
        CommentResponse Update(int id, CommentRequest request);
        void Delete(int id);
        List<CommentResponse> Search(string keyword);
        PageResult<CommentResponse> GetPage(int page, int pageSize);
    }
}
