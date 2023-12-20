using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Packages
{
    public class Package: IBasic
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }      
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PackageDetail> PackageDetails { get; set; }
    }
}
