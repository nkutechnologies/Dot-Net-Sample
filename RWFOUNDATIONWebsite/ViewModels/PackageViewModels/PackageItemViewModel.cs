using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.PackageViewModels
{
    public class PackageItemViewModel
    {
        public int PackageItemId { get; set; }
        public decimal PackageQuantity { get; set; }      
        public int ItemId { get; set; }
        public decimal Price { get; set; }
    }
}
