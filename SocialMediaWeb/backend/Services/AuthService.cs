using Backend.Data;
using Backend.DTO.AuthDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly BlogDbContext context;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(BlogDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public AuthResponse Register(RegisterRequest request)
        {
            // Kiểm tra trùng UserName
            if (context.Users.Any(u => u.UserName == request.UserName))
            {
                throw new Exception("Tên đăng nhập đã tồn tại");
            }

            // Kiểm tra trùng Email
            if (context.Users.Any(u => u.Email == request.Email))
            {
                throw new Exception("Email đã được sử dụng");
            }

            // Tạo user mới
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                AvatarUrl = "",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = User.UserRole.User
            };

            context.Users.Add(user);
            context.SaveChanges();

            // Tạo token và trả về
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }

        public AuthResponse Login(LoginRequest request)
        {
            // Tìm user theo UserName
            var user = context.Users.FirstOrDefault(u => u.UserName == request.UserName);

            if (user == null)
            {
                throw new Exception("Tên đăng nhập không tồn tại");
            }

            // Kiểm tra tài khoản có bị khóa không
            if (!user.IsActive)
            {
                throw new Exception("Tài khoản đã bị khóa");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Mật khẩu không đúng");
            }

            // Tạo token và trả về
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public  User GetCurrentUser()
        {
            var userId = httpContextAccessor.HttpContext?.User
                ?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Vui lòng đăng nhập");
            }

            var user = context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));

            if (user == null)
            {
                throw new Exception("User không tồn tại");
            }

            if (!user.IsActive)
            {
                throw new Exception("Tài khoản đã bị khóa");
            }

            return user;
        }
        public bool IsAdmin()
        {
            return GetCurrentUser().Role == User.UserRole.Admin;
        }
        public bool IsAuthor(int authorId)
        {
            return GetCurrentUser().Id == authorId;
        }

    }
}
