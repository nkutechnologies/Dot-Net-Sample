using Microsoft.AspNetCore.Identity;
using RWFOUNDATIONWebsite.DataModels.Donors;
using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.UserManagement
{
    public class ApplicationUser: IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ToDonate { get; set; }
        public string Estimate { get; set; }
        public int CompanyId { get; set; }
        public ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }   
        public ICollection<GroceryKitAssign> GroceryKitAssigns { get; set; }
        public ICollection<DonorSponcer> DonorSponcers { get; set; }
        public ICollection<DonorRequestForBeneficiary> DonorRequestForBeneficiaries { get; set; }
    }
}
