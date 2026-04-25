namespace Backend.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
       public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public Category Category { get; set; }
    }
}
