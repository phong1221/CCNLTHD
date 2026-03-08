using System.ComponentModel.DataAnnotations;
using static Backend.Models.Entities.Reaction;

namespace Backend.DTO.ReactDTO
{
    public class ReactionRequest
    {
        [Required(ErrorMessage ="bài viết không được trống")]
        public int PostId { get; set; }
        [EnumDataType(typeof(ReactionType), ErrorMessage = "cảm xúc không hợp lệ")]
        public ReactionType ReactType { get; set; }
    }
}
