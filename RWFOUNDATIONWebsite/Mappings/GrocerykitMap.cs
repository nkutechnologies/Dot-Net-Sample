using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Mappings
{
    public class GrocerykitMap : IEntityTypeConfiguration<GroceryKit>
    {
        public void Configure(EntityTypeBuilder<GroceryKit> builder)
        {
            builder.HasKey(g => g.GroceryKitId);
            builder.Property(g => g.GroceryKitId);
            builder.Property(g => g.FormNo).HasColumnName("FormNo").IsRequired(true);
            builder.Property(g => g.FirstName).HasColumnName("FirstName");
            builder.Property(g => g.LastName).HasColumnName("LastName");
            builder.Property(g => g.NameUrdu).HasColumnName("NameUrdu");
            builder.Property(g => g.FatherOrHusbandName).HasColumnName("FatherOrHusbandName");
            builder.Property(g => g.FatherOrHusbandNameUrdu).HasColumnName("FatherOrHusbandNameUrdu");
            builder.Property(g => g.CNIC).HasColumnName("CNIC");
            builder.Property(g => g.OldFormNo).HasColumnName("OldFormNo");
            builder.Property(g => g.Address).HasColumnName("Address");
            builder.Property(g => g.PhoneNumber1).HasColumnName("PhoneNumber1");
            builder.Property(g => g.PhoneNumber2).HasColumnName("PhoneNumber2");         
            builder.Property(g => g.ApplicationDate).HasColumnName("ApplicationDate");
            builder.Property(g => g.Gender).HasColumnName("Gender");
            builder.Property(g => g.ResidenceStatus).HasColumnName("ResidenceStatus");
            builder.Property(g => g.DateOfBirth).HasColumnName("DateOfBirth");
            builder.Property(g => g.Age).HasColumnName("Age");
            builder.Property(g => g.FoodCost).HasColumnName("FoodCost").HasColumnType("decimal(18,2)");
            builder.Property(g => g.HouseRent).HasColumnName("HouseRent").HasColumnType("decimal(18,2)");
            builder.Property(g => g.SchoolCost).HasColumnName("SchoolCost").HasColumnType("decimal(18,2)");
            builder.Property(g => g.UtilitiesCost).HasColumnName("UtilitiesCost").HasColumnType("decimal(18,2)");
            builder.Property(g => g.MedicalCost).HasColumnName("MedicalCost").HasColumnType("decimal(18,2)");
            builder.Property(g => g.OtherCost).HasColumnName("OtherCost").HasColumnType("decimal(18,2)");
            builder.Property(g => g.TotalCost).HasColumnName("TotalCost").HasColumnType("decimal(18,2)");
            builder.Property(g => g.ZakatAcceptable).HasColumnName("ZakatAcceptable");
            builder.Property(g => g.FamilySize).HasColumnName("FamilySize");
            builder.Property(g => g.Salary).HasColumnName("Salary").HasColumnType("decimal(18,2)");
            builder.Property(g => g.Donations).HasColumnName("Donations").HasColumnType("decimal(18,2)");
            builder.Property(g => g.OtherIncome).HasColumnName("OtherIncome").HasColumnType("decimal(18,2)");
            builder.Property(g => g.TotalIncome).HasColumnName("TotalIncome").HasColumnType("decimal(18,2)");
            builder.Property(g => g.ShortFallInCash).HasColumnName("ShortFallInCash").HasColumnType("decimal(18,2)");
            builder.Property(g => g.Remarks).HasColumnName("Remarks");
            builder.Property(g => g.Rating).HasColumnName("Rating").HasColumnType("decimal(18,2)");
            builder.Property(g => g.ApplicationStatus).HasColumnName("ApplicationStatus");
            builder.Property(g => g.SponsorStatus).HasColumnName("SponsorStatus");
            builder.Property(g => g.ImageUrl).HasColumnName("ImageUrl");           

            builder.Property(g => g.CNICFrontUrl).HasColumnName("CNICFrontUrl");
            builder.Property(g => g.CNICBackUrl).HasColumnName("CNICBackUrl");
            builder.Property(g => g.DisabilityCertificateUrl).HasColumnName("DisabilityCertificateUrl");
            builder.Property(g => g.DeathCertificateUrl).HasColumnName("DeathCertificateUrl");
            builder.Property(g => g.BFormUrl).HasColumnName("BFormUrl");
            builder.Property(g => g.ElectricityBill).HasColumnName("ElectricityBill");
            builder.Property(g => g.PtclBillUrl).HasColumnName("PtclBillUrl");
            builder.Property(g => g.ApplicationUrl).HasColumnName("ApplicationUrl");
            builder.Property(g => g.OtherDocument1Url).HasColumnName("OtherDocument1Url");
            builder.Property(g => g.OtherDocument2Url).HasColumnName("OtherDocument2Url");
            builder.Property(g => g.OtherDocumentName1).HasColumnName("OtherDocumentName1");
            builder.Property(g => g.OtherDocumentName2).HasColumnName("OtherDocumentName2");
            builder.Property(g => g.VideoUrl).HasColumnName("VideoUrl");
            builder.Property(g => g.AudioUrl).HasColumnName("AudioUrl");

            builder.Property(c => c.CreatedBy).HasColumnName("CreatedBy").IsRequired(true);
            builder.Property(c => c.UpdatedBy).HasColumnName("UpdatedBy");
            builder.Property(c => c.CreatedOn).HasColumnName("CreatedOn").IsRequired(true);
            builder.Property(c => c.UpdatedOn).HasColumnName("UpdatedOn");
            builder.Property(c => c.IsDeleted).HasColumnName("IsDeleted");
            builder.Property(c => c.IsActive).HasColumnName("IsActive");

            builder.HasOne(c => c.Occupation).WithMany(c => c.GroceryKits).HasForeignKey(x => x.OccupationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.MedicalProb).WithMany(c => c.GroceryKits).HasForeignKey(x => x.MedicalProbId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.CurrentStatus).WithMany(c => c.GroceryKits).HasForeignKey(x => x.CurrentStatusId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.City).WithMany(c => c.GroceryKits).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.Province).WithMany(c => c.GroceryKits).HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
