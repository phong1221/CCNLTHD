using Microsoft.AspNetCore.Mvc;

namespace Backend.Models.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsAccept { get; set; } = false;
        
    }
}
