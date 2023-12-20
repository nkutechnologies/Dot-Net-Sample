using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class SponsorDataViewModel
    {
        public int NoOfMonth { get; set; }
        public int TotalFamilies { get; set; }
        public decimal EstimatedExpense { get; set; }
        public decimal TotalMeals { get; set; }
        public List<SponcerToViewModel> Beneficiaries { get; set; }
    }
}
