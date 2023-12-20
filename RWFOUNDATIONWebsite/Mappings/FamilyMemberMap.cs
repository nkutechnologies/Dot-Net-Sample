using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class FamilyMemberMap : IEntityTypeConfiguration<FamilyMember>
    {
        public void Configure(EntityTypeBuilder<FamilyMember> builder)
        {
            builder.HasKey(f => f.FamilyMemberId);
            builder.Property(f => f.FamilyMemberId);
            builder.Property(f => f.Name).HasColumnName("Name");           
            builder.Property(f => f.Age).HasColumnName("Age");           

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(f => f.GroceryKit).WithMany(f => f.FamilyMembers).HasForeignKey(f => f.GroceryKitId);
            builder.HasOne(f => f.Relation).WithMany(f => f.FamilyMembers).HasForeignKey(f => f.RelationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.FamilyMemberStatus).WithMany(f => f.FamilyMembers).HasForeignKey(f => f.FamilyMemberStatusId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
