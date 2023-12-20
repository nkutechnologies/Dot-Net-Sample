using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Donors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class DonorSponcerMap : IEntityTypeConfiguration<DonorSponcer>
    {
        public void Configure(EntityTypeBuilder<DonorSponcer> builder)
        {
            builder.HasKey(x => x.SponcerId);
            builder.Property(x => x.SponcerId);
            builder.Property(x => x.NoOfMonth).HasColumnName("NoOfMonth");          
            builder.Property(x => x.TotalFamilies).HasColumnName("TotalFamilies");
            builder.Property(x => x.EstimatedExpense).HasColumnName("EstimatedExpense").HasColumnType("decimal(18,4)");
            builder.Property(x => x.TotalMeals).HasColumnName("TotalMeals").HasColumnType("decimal(18,4)");


            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(c => c.ApplicationUser).WithMany(c => c.DonorSponcers).HasForeignKey(c => c.DonorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.GroceryKit).WithMany(c => c.DonorSponcers).HasForeignKey(c => c.GroceryKitId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
