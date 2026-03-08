using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities
{
    public class Follow
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        //thêm khóa ngoại đến user 
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }
        [ForeignKey("FollowingId")]
        public User Following { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
