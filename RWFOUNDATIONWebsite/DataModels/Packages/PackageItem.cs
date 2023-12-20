using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Packages
{
    public class PackageItem:IBasic
    {
        public int PackageItemId { get; set; }
        public decimal PackageQuantity { get; set; }
        public int PackageDetailId { get; set; }
        public PackageDetail PackageDetail { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
