using System.ComponentModel.DataAnnotations.Schema;

namespace Cental.Models
{
    public class Author : BaseEntity
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
