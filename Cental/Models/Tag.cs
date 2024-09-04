namespace Cental.Models
{
    public class Tag : BaseEntity
    {
        public required string Name { get; set; }
        public List<BlogsTags>? BlogsTags { get; set; }
    }
}
