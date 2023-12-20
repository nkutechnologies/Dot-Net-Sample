using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class ListData
    {
        public List<PackageValue> PackageValues { get; set; }
        public List<BeneficaryForCalculation> BeneficaryForCalculations { get; set; }
    }
}
