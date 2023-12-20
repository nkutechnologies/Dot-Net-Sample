using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels
{
    public class GroceryKitViewModel
    {
        public int GroceryKitId { get; set; }
        public string FormNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string CNIC { get; set; }
        public string OldFormNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }       
        public string Province { get; set; }      
        public string MedicalProb { get; set; }       
        public string Occupation { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string MeritalStatus { get; set; }      
        public string  Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Gender { get; set; }
        public string ResidenceStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public decimal FoodCost { get; set; }
        public decimal HouseRent { get; set; }
        public decimal SchoolCost { get; set; }
        public decimal UtilitiesCost { get; set; }
        public decimal MedicalCost { get; set; }
        public decimal OtherCost { get; set; }
        public decimal TotalCost { get; set; }
        public string ZakatAcceptable { get; set; }
        public int FamilySize { get; set; }
        public decimal Salary { get; set; }
        public decimal Donations { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal ShortFallInCash { get; set; }
        public string Remarks { get; set; }
        public string ImageUrl { get; set; }
        public decimal Rating { get; set; }
        public string CNICFrontUrl { get; set; }
        public string CNICBackUrl { get; set; }
        public string DisabilityCertificateUrl { get; set; }
        public string DeathCertificateUrl { get; set; }
        public string BFormUrl { get; set; }
        public string ElectricityBill { get; set; }
        public string PtclBillUrl { get; set; }
        public string ApplicationUrl { get; set; }
        public string OtherDocumentName1 { get; set; }
        public string OtherDocumentName2 { get; set; }
        public string OtherDocument1Url { get; set; }
        public string OtherDocument2Url { get; set; }          
        public ICollection<FamilyMembersViewModel> FamilyMembers { get; set; }       
    }
}
