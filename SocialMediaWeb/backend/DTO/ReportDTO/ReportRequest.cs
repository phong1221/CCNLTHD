using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.ReportDTO
{
    public class ReportRequest
    {
        [Required(ErrorMessage ="Required Post")]
        public int PostId { get; set; }
        [Required(ErrorMessage ="Required Reason")]
        [StringLength(500,MinimumLength =1,ErrorMessage ="Reason Length must between 1 and 500 characters")]
        public string Reason { get; set; }

    }
}
