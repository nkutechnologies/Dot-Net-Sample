using RWFOUNDATIONWebsite.DataModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Donors
{
    public class DonorRequestForBeneficiary: IBasic
    {
        public int Id { get; set; }
        public int DonationTypeId { get; set; }       
        public DonationType DonationType { get; set; }
        public int DonorId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ExpectedDonation { get; set; }
        public string BeneficiaryType { get; set; }
        public int RequestTo { get; set; }
        public bool IsRead { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<FamilyMemberForDonor> FamilyMemberForDonors { get; set; }
    }
}
