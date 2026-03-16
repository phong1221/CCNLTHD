using Backend.DTO.AuthDTO;
using Backend.Models.Entities;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponse Register(RegisterRequest request);
        AuthResponse Login(LoginRequest request);
        User GetCurrentUser();
        bool IsAdmin();
        bool IsAuthor(int authorId);
    }
}
