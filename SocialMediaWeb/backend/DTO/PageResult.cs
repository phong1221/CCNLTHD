namespace Backend.DTO
{
    public class PageResult<T>
    {
        public int Total { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
