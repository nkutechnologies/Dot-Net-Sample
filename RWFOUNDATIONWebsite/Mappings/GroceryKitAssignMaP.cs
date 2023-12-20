using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class GroceryKitAssignMaP : IEntityTypeConfiguration<GroceryKitAssign>
    {
        public void Configure(EntityTypeBuilder<GroceryKitAssign> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id);

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(c => c.ApplicationUser).WithMany(c => c.GroceryKitAssigns).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.GroceryKit).WithMany(c => c.GroceryKitAssigns).HasForeignKey(c => c.GroceryKitId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
