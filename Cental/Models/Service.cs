namespace Cental.Models
{
    public class Service : BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Icon { get; set; }
    }
}
