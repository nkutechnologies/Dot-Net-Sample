using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Donors
{
    public class DonationType: IBasic
    {
        public int DonationTypeId { get; set; }
        public string Title { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<DonorRequestForBeneficiary> DonorRequestForBeneficiaries { get; set; }
    }
}
