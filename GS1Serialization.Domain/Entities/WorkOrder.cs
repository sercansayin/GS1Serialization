using GS1Serialization.Domain.Enum;

namespace GS1Serialization.Domain.Entities
{
    public class WorkOrder : BaseEntity
    {
        public int ProductId { get; set; }
        public string WorkOrderCode { get; set; } // İş Emri No
        public int TargetQuantity { get; set; }   // Üretilecek Adet
        public string LotNumber { get; set; }     // (10) Batch No
        public DateOnly ExpireDate { get; set; }  // (17) SKT

        public WorkOrderStatus Status { get; set; } // Enum: Created, Running, Completed, Cancelled

        // Navigation Properties
        public Product? Product { get; set; }
        public ICollection<Package> Packages { get; set; } = [];
    }
}
