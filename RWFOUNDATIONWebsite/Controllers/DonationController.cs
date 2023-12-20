using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Donations;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.ViewModels.DonationViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    public class DonationController : Controller
    {
        private readonly JsonReturn returnClass;
        private readonly RwDbContext _context;
        public DonationController(JsonReturn _jsonReturn, RwDbContext context)
        {
            this.returnClass = _jsonReturn;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDonation(DonationViewModel model)
        {

            try
            {
                var number = _context.Donations.ToList().LastOrDefault();
                string donationnumber = "0";
                int no = 0;
                if (number != null)
                {
                    int num = Convert.ToInt32(number.DonationNumber) + 1;
                    donationnumber = "Donation No #" + num;
                    no = num;
                }
                else
                {
                    donationnumber = "Form No#1";
                    no = 1;
                }
                var donations = new Donation();
                donations.DonationNumber = Convert.ToString(no);
                donations.FirstName = model.FirstName;
                donations.LastName = model.LastName;
                donations.PhoneNumber = model.PhoneNumber;
                donations.Email = model.Email;
                donations.PledgedAmount = model.PledgedAmount;
                donations.PledgedDate = DateTime.Now;
                donations.CreatedOn = DateTime.Now;

                _context.Donations.Add(donations);
                _context.SaveChanges();



                returnClass.Message = Convert.ToString(no);

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
    }
}