using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels
{
    public class SignUpViewModel
    {
       
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        [Remote(action: "checkdate", controller: "Authentication")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }       
        public string As { get; set; }       
        public string ToDonate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Estimate { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Authentication")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
