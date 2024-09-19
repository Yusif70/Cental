namespace Cental.Models
{
    public class Comment : BaseEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string Message { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public int? OpId { get; set; }
        public Comment? Op { get; set; }
        public int? ReplyId { get; set;}
        public Comment? Reply { get; set; }
    }
}
