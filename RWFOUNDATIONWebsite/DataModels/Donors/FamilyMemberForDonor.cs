using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Donors
{
    public class FamilyMemberForDonor
    {
        public int Id { get; set; }
        public string FamilyMember { get; set; }
        public int DonorRequestId { get; set; }
        public DonorRequestForBeneficiary DonorRequestForBeneficiary { get; set; }
    }
}
