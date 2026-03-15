using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.UserDTO
{
    public class UserUpdateRequest
    {
        [StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 kí tự")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
