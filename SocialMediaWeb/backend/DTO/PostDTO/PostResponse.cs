using Backend.Models.Entities;

namespace Backend.DTO.PostDTO
{
    public class PostResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AuthorName {  get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }
}
