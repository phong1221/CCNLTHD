namespace Backend.DTO.ReportDTO
{
    public class ReportResponse
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PostTitle { get; set; }
        public string Reason { get; set; }
        public bool IsAccept {  get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted{ get; set; }
    }
}
