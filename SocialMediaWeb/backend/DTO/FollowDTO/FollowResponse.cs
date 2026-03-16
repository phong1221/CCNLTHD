namespace Backend.DTO.FollowDTO
{
    public class FollowResponse
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
