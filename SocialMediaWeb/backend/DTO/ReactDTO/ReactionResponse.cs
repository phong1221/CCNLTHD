using Backend.Models.Entities;
using static Backend.Models.Entities.Reaction;

namespace Backend.DTO.ReactDTO
{
    public class ReactionResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ReactionType ReactType { get; set; }
        public string UserName { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

    }
}
