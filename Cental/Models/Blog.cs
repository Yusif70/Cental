using System.ComponentModel.DataAnnotations.Schema;

namespace Cental.Models
{
    public class Blog : BaseEntity
    {
        public required string Title1 { get; set; }
        public required string Description1 { get; set; }
        public required string Title2 { get; set; }
        public required string Description2 { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public required int AuthorId { get; set; }
        public Author? Author { get; set; }
        public required int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<BlogsTags>? BlogsTags { get; set; }
        [NotMapped]
        public required int[] TagIds { get; set; }
    }
}
