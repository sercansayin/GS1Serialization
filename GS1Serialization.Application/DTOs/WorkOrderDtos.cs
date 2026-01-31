namespace GS1Serialization.Application.DTOs
{
    public class CreateWorkOrderRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } 
        public required string LotNumber { get; set; }
        public DateOnly ExpireDate { get; set; }
        public long SerialNumberStart { get; set; } 
    }
    public class WorkOrderResponse
    {
        public int WorkOrderId { get; set; }
        public string WorkOrderCode { get; set; } = string.Empty;
        public int ProducedQuantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public ProductDto Product { get; set; }
        public List<PackageDto> Packages { get; set; } = [];
    }
    public class ProductDto
    {
        public string Name { get; set; }
        public string GTIN { get; set; }
        public string Description { get; set; }
    }
    public class PackageDto
    {
        public string SerialNumber { get; set; } // (21)
        public string? SSCC { get; set; }        // (00) - Koli/Palet ise dolu olur
        public string Level { get; set; }        // Item, Box, Pallet
        public string? FullGS1String { get; set; }
        public List<PackageDto> Children { get; set; } = [];
    }
}
