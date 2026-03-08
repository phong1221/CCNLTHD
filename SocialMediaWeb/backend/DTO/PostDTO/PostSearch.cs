using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.PostDTO
{
    public class PostSearch
    {
        [Range(1, int.MaxValue)]
        public int page {  get; set; }
        [Range(1, 100)]
        public int pageSize { get; set; }
        public string tittle { get; set; } = "";
        public string categoryName { get; set; } = "";
    }
}
