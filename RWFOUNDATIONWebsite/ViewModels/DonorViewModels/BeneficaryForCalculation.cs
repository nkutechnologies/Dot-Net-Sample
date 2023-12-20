using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class BeneficaryForCalculation
    {
        public string Status { get; set; }
        public int Dependents { get; set; }
        public decimal Shortfall { get; set; }
        public int Size { get; set; }
    }
}
