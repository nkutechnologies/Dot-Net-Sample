using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Donors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.DonorMappings
{
    public class DonorRequestForBeneficiaryMap : IEntityTypeConfiguration<DonorRequestForBeneficiary>
    {
        public void Configure(EntityTypeBuilder<DonorRequestForBeneficiary> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.BeneficiaryType).HasColumnName("BeneficiaryType");           
            builder.Property(x => x.RequestTo).HasColumnName("RequestTo");           
            builder.Property(x => x.ExpectedDonation).HasColumnName("ExpectedDonation").HasColumnType("decimal(18,2)");

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");
            builder.Property(c => c.IsRead).HasColumnName("IsRead");
           
            builder.HasOne(x => x.DonationType).WithMany(x => x.DonorRequestForBeneficiaries).HasForeignKey(x => x.DonationTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ApplicationUser).WithMany(x => x.DonorRequestForBeneficiaries).HasForeignKey(x => x.DonorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
