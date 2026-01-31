using GS1Serialization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS1Serialization.Infrastructure.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasIndex(x => x.SerialNumber).IsUnique();
            builder
                .HasOne(x => x.ParentPackage)
                .WithMany(x =>x.ChildPackages)
                .HasForeignKey(x => x.ParentPackageId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => x.SSCC).IsUnique();
        }
    }
}
