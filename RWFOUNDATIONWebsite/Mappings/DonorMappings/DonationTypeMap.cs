using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Donors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.DonorMappings
{
    public class DonationTypeMap : IEntityTypeConfiguration<DonationType>
    {
        public void Configure(EntityTypeBuilder<DonationType> builder)
        {
            builder.HasKey(x => x.DonationTypeId);
            builder.Property(x => x.DonationTypeId);
            builder.Property(x => x.Title).HasColumnName("Title");
           
            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");
        }
    }
}
