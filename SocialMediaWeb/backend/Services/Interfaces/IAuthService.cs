using Backend.DTO.AuthDTO;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponse Register(RegisterRequest request);
        AuthResponse Login(LoginRequest request);
    }
}
