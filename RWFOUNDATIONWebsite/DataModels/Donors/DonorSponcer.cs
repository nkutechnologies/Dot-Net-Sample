using RWFOUNDATIONWebsite.DataModels.Master;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Donors
{
    public class DonorSponcer: IBasic
    {
        public int SponcerId { get; set; }
        public int NoOfMonth { get; set; }
        public int TotalFamilies { get; set; }
        public decimal EstimatedExpense { get; set; }
        public decimal TotalMeals { get; set; }
        public int GroceryKitId { get; set; }
        public int DonorId { get; set; }
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
