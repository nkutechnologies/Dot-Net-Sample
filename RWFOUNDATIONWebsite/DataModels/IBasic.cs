using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels
{
    public interface IBasic
    {
        int CreatedBy { get; set; }
        int? UpdatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
    }
}
