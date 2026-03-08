using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class PageRequest
    {
        [Range(1, int.MaxValue)]
        public int page {  get; set; }
        [Range(1, 100)]
        public int pageSize { get; set; }
    }
}
