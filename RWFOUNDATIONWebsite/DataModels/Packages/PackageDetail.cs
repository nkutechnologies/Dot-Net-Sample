using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Packages
{
    public class PackageDetail: IBasic
    {
        public int PackageDetailId { get; set; }
        public string PackageDetailName { get; set; }
        public int TotalFamilyMember { get; set; }
        public decimal PackageValuePKR { get; set; }
        public decimal PackageValueSR { get; set; }
        public decimal PackageValueUSD { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PackageItem> PackageItems { get; set; }
    }
}
