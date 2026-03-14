using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.UserDTO
{
    public class ChangeUserNameRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập mới không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không vượt quá 50 kí tự")]
        public string NewUserName { get; set; }
    }
}
