using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Donors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.DonorMappings
{
    public class FamilyMemberForDonorMap : IEntityTypeConfiguration<FamilyMemberForDonor>
    {
        public void Configure(EntityTypeBuilder<FamilyMemberForDonor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.FamilyMember).HasColumnName("FamilyMember");

            builder.HasOne(x => x.DonorRequestForBeneficiary).WithMany(x => x.FamilyMemberForDonors).HasForeignKey(x => x.DonorRequestId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
