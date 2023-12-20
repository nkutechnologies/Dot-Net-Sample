using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.PackageViewModels
{
    public class PackageDetailViewModel
    {
        public int PackageDetailId { get; set; }
        public string PackageDetailName { get; set; }
        public int TotalFamilyMember { get; set; }
        public decimal PackageValuePKR { get; set; }
        public decimal PackageValueSR { get; set; }
        public decimal PackageValueUSD { get; set; }       
        public int PackageId { get; set; }
        public ICollection<PackageItemViewModel> PackageItems { get; set; }
        
    }
}
