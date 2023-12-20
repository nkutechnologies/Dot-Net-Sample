using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.PackageMappings
{
    public class PackageDetailMap : IEntityTypeConfiguration<PackageDetail>
    {
        public void Configure(EntityTypeBuilder<PackageDetail> builder)
        {
            builder.HasKey(x => x.PackageDetailId);
            builder.Property(x => x.PackageDetailId);
            builder.Property(x => x.PackageDetailName).HasColumnName("PackageDetailName");
            builder.Property(x => x.TotalFamilyMember).HasColumnName("TotalFamilyMember");
            builder.Property(x => x.PackageValuePKR).HasColumnName("PackageValuePKR");
            builder.Property(x => x.PackageValueSR).HasColumnName("PackageValueSR");
            builder.Property(x => x.PackageValueUSD).HasColumnName("PackageValueUSD");

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(x => x.Package).WithMany(x => x.PackageDetails).HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
