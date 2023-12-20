using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class DonorRequestForAdminViewModel
    {
        public int Id { get; set; }
        public string DonorName { get; set;}
        public DateTime CreatedOn { get; set; }
        public decimal ExpectedDonation { get; set; }
    }
}
