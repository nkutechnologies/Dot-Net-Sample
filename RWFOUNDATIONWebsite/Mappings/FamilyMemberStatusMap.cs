using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class FamilyMemberStatusMap : IEntityTypeConfiguration<FamilyMemberStatus>
    {
        public void Configure(EntityTypeBuilder<FamilyMemberStatus> builder)
        {
            builder.HasKey(x => x.FamilyMemberStatusId);
            builder.Property(x => x.FamilyMemberStatusId);
            builder.Property(x => x.FamilyMemberStatusName).HasColumnName("FamilyMemberStatusName");

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");
        }
    }
}
