using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class DonorSponcerViewModel
    {
        public int GroceryKitId { get; set; }       
        public string CNIC { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public int FamilySize { get; set; }
        public decimal Shortfallincash { get; set; }
    }
}
