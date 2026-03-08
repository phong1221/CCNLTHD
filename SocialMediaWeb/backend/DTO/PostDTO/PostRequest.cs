using Backend.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.PostDTO
{
    public class PostRequest
    {
        [Required(ErrorMessage = "tựa đề không được để trống")]
        [StringLength(100, ErrorMessage = "tựa đề không được vượt quá 100 kí tự")]
        public string Title { get; set; }
        [Required(ErrorMessage = "nội dung không được  để trống")]
        [StringLength(100, ErrorMessage = "tựa đề không được vượt quá 100 kí tự")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required(ErrorMessage = "thể loại không được để trống")]
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
