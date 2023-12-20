using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class GroceryKitDonorViewModel
    {
        public int GroceryKitId { get; set; }
        public string FormNo { get; set; }
        public string Name { get; set; }       
        public string Address { get; set; }
        public string MedicalProbs { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Occupation { get; set; }
        public string Status { get; set; }       
        public string Gender { get; set; }
        public string ResidenceStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }       
        public string ZakatAcceptable { get; set; }
        public int FamilySize { get; set; }
        public decimal Salary { get; set; }          
    }
}
