using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.UserManagement
{
    public class Company : IBasic
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get ; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
