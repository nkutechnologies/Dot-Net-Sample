using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.SaveAsDrafts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings.SaveAsDraftMappings
{
    public class BeneficiaryFamilyMemberSaveAsMap : IEntityTypeConfiguration<BeneficiaryFamilyMemberSaveAsDraft>
    {
        public void Configure(EntityTypeBuilder<BeneficiaryFamilyMemberSaveAsDraft> builder)
        {
            builder.HasKey(f => f.FamilyMemberId);
            builder.Property(f => f.FamilyMemberId);
            builder.Property(f => f.Name).HasColumnName("Name");
            builder.Property(f => f.Age).HasColumnName("Age");            

            builder.HasOne(f => f.BeneficiarySaveAsDraft).WithMany(f => f.FamilyMembers).HasForeignKey(f => f.GroceryKitId);
            builder.HasOne(f => f.Relation).WithMany(f => f.BeneficiaryFamilyMemberSaveAsDrafts).HasForeignKey(f => f.RelationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.FamilyMemberStatus).WithMany(f => f.BeneficiaryFamilyMemberSaveAsDrafts).HasForeignKey(f => f.FamilyMemberStatusId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
