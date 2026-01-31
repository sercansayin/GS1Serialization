using GS1Serialization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS1Serialization.Infrastructure.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.HasIndex(x => x.WorkOrderCode).IsUnique();
            builder.Property(x => x.WorkOrderCode)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(x => x.LotNumber)
                .IsRequired()
                .HasMaxLength(20);
            builder.HasOne(x =>x.Product)
                .WithMany(x => x.WorkOrders)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
