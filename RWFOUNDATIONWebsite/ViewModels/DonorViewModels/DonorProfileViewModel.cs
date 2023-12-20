using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels.DonorViewModels
{
    public class DonorProfileViewModel
    {
        public int UserId { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        [Remote(action: "checkdate", controller: "Authentication")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }       
        public string ToDonate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Estimate { get; set; }
      
    }
}
