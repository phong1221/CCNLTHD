using Backend.DTO;
using Backend.DTO.UserDTO;

namespace Backend.Services.Interfaces
{
    public interface IUserService
    {
        List<UserResponse> GetAll();
        UserResponse GetById(int id);
        UserResponse Update(int id, UserUpdateRequest request);
        void Delete(int id);
        List<UserResponse> Search(string keyword);
        PageResult<UserResponse> GetPage(int page, int pageSize);
        UserResponse ChangePassword(int id, ChangePasswordRequest request);
    }
}
