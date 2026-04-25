using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class UserSearchRequest
    {
        [Range(1, int.MaxValue)]
        public int page { get; set; }
        [Range(1, 100)]
        public int pageSize { get; set; }
        public string FullName { get; set; } = "";
        
        public DateTime date1 {  get; set; } 
       
        public DateTime date2 {  get; set; } 
    }
}
