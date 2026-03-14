using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.UserDTO
{
    public class ChangeCredentialsRequest
    {
        [StringLength(50, ErrorMessage = "Tên đăng nhập không vượt quá 50 kí tự")]
        public string? NewUserName { get; set; }

        public string? CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu mới phải từ 6 đến 100 kí tự")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmNewPassword { get; set; }
    }
}
