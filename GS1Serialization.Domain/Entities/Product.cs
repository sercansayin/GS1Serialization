namespace GS1Serialization.Domain.Entities
{
    public class Product: BaseEntity
    {
        public int CustomerId { get; set; }
        public required string Name { get; set; }
        public required string GTIN { get; set; } // (01) alanı için, 14 hane genelde.

        public Customer? Customer { get; set; }
        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
