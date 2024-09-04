namespace Cental.Models
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
