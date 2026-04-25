using Backend.Data;
using Backend.DTO;
using Backend.DTO.UserDTO;
using Backend.Models.Entities;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly BlogDbContext context;

        private readonly IAuthService authService;

        public UserService(BlogDbContext context, IAuthService authService)
        {
            this.context = context;
            this.authService = authService;
          
        }

        public List<UserResponse> GetAll()
        {
            if (!authService.IsAdmin())
            {
                throw new Exception("chỉ có admin mới có quyền này");
            }
            return context.Users
                .Where(u => u.IsActive)
                .Select(u => MapToResponse(u))
                .ToList();
        }

        public UserResponse GetById(int id)
        {
            if (!authService.IsAdmin())
            {
                throw new Exception("chỉ có admin mới có quyền này");
            }
            var user = context.Users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + id);
            }
            return MapToResponse(user);
        }

        public UserResponse Update(int id, UserUpdateRequest request)
        {

            if (!(authService.IsAdmin() && authService.IsAuthor(authService.GetCurrentUser().Id)))
            {
                throw new Exception("chỉ có admin mới có quyền này");
            }
            var user = context.Users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + id);
            }

            // Kiểm tra email trùng (nếu có thay đổi email)
            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                if (context.Users.Any(u => u.Email == request.Email && u.Id != id))
                {
                    throw new Exception("Email đã được sử dụng");
                }
                user.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.FullName))
                user.FullName = request.FullName;

            if (request.DateOfBirth.HasValue)
                user.DateOfBirth = request.DateOfBirth.Value;

            if (!string.IsNullOrEmpty(request.Gender))
                user.Gender = request.Gender;

            if (request.AvatarUrl != null)
                user.AvatarUrl = request.AvatarUrl;

            user.UpdatedAt = DateTime.Now;
            context.SaveChanges();

            return MapToResponse(user);
        }

        public void Delete(int id)
        {
            if (!(authService.IsAdmin() && authService.IsAuthor(authService.GetCurrentUser().Id)))
            {
                throw new Exception("chỉ có admin mới có quyền này");
            }
            var user = context.Users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + id);
            }
            user.IsActive = false;
            user.UpdatedAt = DateTime.Now;
            context.SaveChanges();
        }

        public List<UserResponse> Search(string keyword)
        {
            var lowerKeyword = keyword.ToLower();
            return context.Users
                .Where(u => u.IsActive &&
                    (u.UserName.ToLower().Contains(lowerKeyword) ||
                     u.FullName.ToLower().Contains(lowerKeyword) ||
                     u.Email.ToLower().Contains(lowerKeyword)))
                .Select(u => MapToResponse(u))
                .ToList();
        }

        public PageResult<UserResponse> GetPage(int page, int pageSize)
        {
            var items = context.Users
                .Where(u => u.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = items.Select(u => MapToResponse(u)).ToList();

            return new PageResult<UserResponse>
            {
                Items = result,
                Total = context.Users.Count(u => u.IsActive)
            };
        }

        public UserResponse ChangePassword(int id, ChangePasswordRequest request)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id && u.IsActive);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại với id: " + id);
            }
            if (!authService.IsAuthor(user.Id))
            {
                throw new Exception("bạn không có quyền sửa  mật khẩu");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            {
                throw new Exception("Mật khẩu hiện tại không đúng");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.Now;
            context.SaveChanges();

            return MapToResponse(user);
        }
        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                AvatarUrl = user.AvatarUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = user.Role.ToString()
            };
        }
    }


}
