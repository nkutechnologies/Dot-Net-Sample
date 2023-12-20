using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.PackageMappings
{
    public class PackageItemMap : IEntityTypeConfiguration<PackageItem>
    {
        public void Configure(EntityTypeBuilder<PackageItem> builder)
        {
            builder.HasKey(x => x.PackageItemId);
            builder.Property(x => x.PackageItemId);
            builder.Property(x => x.PackageQuantity).HasColumnName("PackageQuantity").HasColumnType("decimal(18,2)");

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(x => x.PackageDetail).WithMany(x => x.PackageItems).HasForeignKey(x => x.PackageDetailId);
            builder.HasOne(x => x.Item).WithMany(x => x.PackageItems).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
