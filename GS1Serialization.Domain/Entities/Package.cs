using GS1Serialization.Domain.Enum;

namespace GS1Serialization.Domain.Entities
{
    public class Package : BaseEntity
    {
        public int WorkOrderId { get; set; }

        public required string SerialNumber { get; set; } // (21)
        public string? SSCC { get; set; }   // Opsiyonel (Sadece koli/palet ise dolar)

        public PackageLevel Level { get; set; }  // Enum: Item(1), Box(2), Pallet(3)

        public int? ParentPackageId { get; set; } // Hangi kolinin içinde?

        // Navigation Properties
        public WorkOrder? WorkOrder { get; set; }
        public Package? ParentPackage { get; set; } // Üst paketi
        public ICollection<Package> ChildPackages { get; set; } = [];
    }
}
