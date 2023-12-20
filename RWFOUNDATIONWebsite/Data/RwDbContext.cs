using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RWFOUNDATIONWebsite.DataModels.Donations;
using RWFOUNDATIONWebsite.DataModels.Donors;
using RWFOUNDATIONWebsite.DataModels.Master;
using RWFOUNDATIONWebsite.DataModels.Packages;
using RWFOUNDATIONWebsite.DataModels.SaveAsDrafts;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using RWFOUNDATIONWebsite.Mappings;
using RWFOUNDATIONWebsite.Mappings.DonationMappings;
using RWFOUNDATIONWebsite.Mappings.DonorMappings;
using RWFOUNDATIONWebsite.Mappings.PackageMappings;
using RWFOUNDATIONWebsite.Mappings.SaveAsDraftMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Data
{
    public class RwDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public RwDbContext(DbContextOptions<RwDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<ApplicationUserRole>(new ApplicationUserRoleMap());
            modelBuilder.ApplyConfiguration<GroceryKit>(new GrocerykitMap());
            modelBuilder.ApplyConfiguration<FamilyMember>(new FamilyMemberMap());          
            modelBuilder.ApplyConfiguration<Relation>(new RelationMap());          
            modelBuilder.ApplyConfiguration<Occupation>(new OccupationMap());
            modelBuilder.ApplyConfiguration<MedicalProb>(new MedicalProbMap());
            modelBuilder.ApplyConfiguration<CurrentStatus>(new CurrentStatusMap());
            modelBuilder.ApplyConfiguration<FamilyMemberStatus>(new FamilyMemberStatusMap());
            modelBuilder.ApplyConfiguration<Province>(new ProvinceMap());
            modelBuilder.ApplyConfiguration<City>(new CityMap());
            modelBuilder.ApplyConfiguration<GroceryKitAssign>(new GroceryKitAssignMaP());
            modelBuilder.ApplyConfiguration<DonorSponcer>(new DonorSponcerMap());
            modelBuilder.ApplyConfiguration<Donation>(new DonationMap());
            //Packages Mappings
            modelBuilder.ApplyConfiguration<Package>(new PackageMap());
            modelBuilder.ApplyConfiguration<PackageDetail>(new PackageDetailMap());
            modelBuilder.ApplyConfiguration<PackageItem>(new PackageItemMap());
            modelBuilder.ApplyConfiguration<Item>(new ItemMap());
            modelBuilder.ApplyConfiguration<Unit>(new UnitMap());
            modelBuilder.ApplyConfiguration<Company>(new CompanyMap());
            modelBuilder.ApplyConfiguration<DonationType>(new DonationTypeMap());
            modelBuilder.ApplyConfiguration<DonorRequestForBeneficiary>(new DonorRequestForBeneficiaryMap());
            modelBuilder.ApplyConfiguration<FamilyMemberForDonor>(new FamilyMemberForDonorMap());
            modelBuilder.ApplyConfiguration<BeneficiaryFormSaveAsDraft>(new BeneficiarySaveAsDraftMap());
            modelBuilder.ApplyConfiguration<BeneficiaryFamilyMemberSaveAsDraft>(new BeneficiaryFamilyMemberSaveAsMap());
        }


        public DbSet<GroceryKit> GroceryKits { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }       
        public DbSet<Relation> Relations { get; set; }       
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<MedicalProb> MedicalProbs { get; set; }
        public DbSet<CurrentStatus> CurrentStatuses { get; set; }
        public DbSet<FamilyMemberStatus> FamilyMemberStatuses { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<GroceryKitAssign> GroceryKitAssigns { get; set; }
        public DbSet<DonorSponcer> DonorSponcers { get; set; }
        public DbSet<Donation> Donations { get; set; }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<ApplicationRole> AspNetRoles { get; set; }

        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageDetail> PackageDetails { get; set; }
        public DbSet<PackageItem> PackageItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DonationType> DonationTypes { get; set; }
        public DbSet<DonorRequestForBeneficiary> DonorRequestForBeneficiaries { get; set; }
        public DbSet<FamilyMemberForDonor> FamilyMemberForDonors { get; set; }
        public DbSet<BeneficiaryFormSaveAsDraft> BeneficiaryFormSaveAsDrafts { get; set; }
        public DbSet<BeneficiaryFamilyMemberSaveAsDraft> BeneficiaryFamilyMemberSaveAsDrafts { get; set; }
    }
}
