using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.PackageMappings
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(x => x.ItemId);
            builder.Property(x => x.ItemId);
            builder.Property(x => x.ItemName).HasColumnName("ItemName");
            builder.Property(x => x.ItemNameUrdu).HasColumnName("ItemNameUrdu");
            builder.Property(x => x.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
         

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(x => x.Unit).WithMany(x => x.Items).HasForeignKey(x => x.UnitId);
        }
    }
}
