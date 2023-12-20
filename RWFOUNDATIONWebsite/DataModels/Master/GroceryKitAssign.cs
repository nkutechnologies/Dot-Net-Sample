using RWFOUNDATIONWebsite.DataModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Master
{
    public class GroceryKitAssign: IBasic
    {
        public int Id { get; set; }
        public int GroceryKitId { get; set; }
        public int UserId { get; set; }
        public GroceryKit GroceryKit { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
