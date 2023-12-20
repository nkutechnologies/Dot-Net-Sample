using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.CompanyId);
            builder.Property(x => x.CompanyId);
            builder.Property(x => x.CompanyName).HasColumnName("CompanyName");

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");
        }
    }
}
