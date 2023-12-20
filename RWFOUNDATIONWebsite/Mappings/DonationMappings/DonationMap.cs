using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Donations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.DonationMappings
{
    public class DonationMap : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.HasKey(x => x.DonationId);
            builder.Property(x => x.DonationId);
            builder.Property(x => x.DonationNumber).HasColumnName("DonationNumber");
            builder.Property(x => x.FirstName).HasColumnName("FirstName");
            builder.Property(x => x.LastName).HasColumnName("LastName");
            builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(x => x.PledgedAmount).HasColumnName("PledgedAmount").HasColumnType("decimal(18,4)");
            builder.Property(x => x.PledgedDate).HasColumnName("PledgedDate");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
        }
    }
}
