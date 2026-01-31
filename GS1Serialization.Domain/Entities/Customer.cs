namespace GS1Serialization.Domain.Entities
{
    public class Customer: BaseEntity
    {
        public required string CompanyName { get; set; }
        public required string GLN { get; set; } 
        public string? Description { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; } = [];
    }
}
