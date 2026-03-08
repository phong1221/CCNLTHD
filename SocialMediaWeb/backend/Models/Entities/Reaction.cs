using System.Security.Cryptography.X509Certificates;

namespace Backend.Models.Entities
{
    public class Reaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ReactionType ReactType { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public enum ReactionType
        {
            Like,
            Love,
            Haha,
            Sad,
            Wow
        }
    }
}
