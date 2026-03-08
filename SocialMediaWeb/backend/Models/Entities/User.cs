namespace Backend.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt {  get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public ICollection<Post>Posts { get; set; }= new List<Post>();
        public ICollection<Reaction> Reactions { get; set; }= new List<Reaction>();
        public ICollection<Comment> Comments { get; set; }=new List<Comment>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        // tôi follow ai
        public ICollection<Follow> Following { get; set; } = new List<Follow>();

        // ai follow tôi
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public enum UserRole
        {
            User,
            Admin
        }

    }
}
