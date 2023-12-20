using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class ApplicationUserRoleMap : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasKey(c => new { c.UserId, c.RoleId });
            builder.HasOne(c => c.ApplicationUser).WithMany(c => c.ApplicationUserRoles).HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.ApplicationRole).WithMany(c => c.ApplicationUserRoles).HasForeignKey(c => c.RoleId);
        }
    }
}
