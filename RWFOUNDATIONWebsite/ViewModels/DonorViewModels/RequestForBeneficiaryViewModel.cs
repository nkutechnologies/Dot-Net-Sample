using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class RequestForBeneficiaryViewModel
    {
        public int DonationTypeId { get; set; }
        public string BeneficiaryType { get; set; }
        public int ExpectedDonation { get; set; }
        public List<string> FamilyMemberSize { get; set; }
    }
}
