using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.CategoryDTO
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "tên không được để trống")]
        [StringLength(100, ErrorMessage = "tên không được vượt quá 100 kí tự")]
        public string Name { get; set; }
        [Required(ErrorMessage ="mô tả không được để trống")]
        [StringLength(100, ErrorMessage = "mô tả không vượt quá 100 kí tự")]
        public string Description { get; set; }
    }
}
