using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.CommentDTO
{
    public class CommentRequest
    {
        [Required(ErrorMessage = "UserId không được để trống")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "PostId không được để trống")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 kí tự")]
        public string Content { get; set; }
    }
}
