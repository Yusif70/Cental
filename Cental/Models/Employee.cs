using Elfie.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cental.Models
{
    public class Employee : BaseEntity
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Profession { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
