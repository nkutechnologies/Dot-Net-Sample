using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Donors;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels.AdminViewModels;
using RWFOUNDATIONWebsite.ViewModels.DonorViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : PrivateController
    {
        private readonly JsonReturn returnClass;
        private readonly RwDbContext _context;       
        private readonly GrocerykitService _grocerykitService;
        private readonly DonorService _donorService;
        private readonly UserManager<ApplicationUser> _userManager;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        private int CompanyId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId")); } }
        public AdminController(JsonReturn _jsonReturn, RwDbContext context, GrocerykitService grocerykitService, 
            DonorService donorService, UserManager<ApplicationUser> userManager)
        {
            this.returnClass = _jsonReturn;
            _context = context;
            _grocerykitService = grocerykitService;
            _donorService = donorService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetBeneficiaryAppeal()
        {
            var appeals = _grocerykitService.GetAllAppeal().Where(x => x.CompanyId == CompanyId && !String.IsNullOrEmpty(x.VideoUrl) || !String.IsNullOrEmpty(x.AudioUrl)).Select(x => new BeneficiaryAppealViewModel
            {
                VideoUrl = x.VideoUrl,
                AudioUrl = x.AudioUrl,
                BeneficiaryFirtName = x.FirstName,
                BeneficiaryLastName = x.LastName,
                BeneficiayFormNo = x.FormNo,
                PhotoUrl = x.ImageUrl,
                PhoneNumber1 = x.PhoneNumber1,
                PhoneNumber2 = x.PhoneNumber2
            }).ToList();
            return View(appeals) ;
        }
        [HttpGet]
        public IActionResult GetAllRequests()
        {
            var requests = _donorService.GetRequestsByUserId(UserId);
            return View(requests);
        }
        [AllowAnonymous]
        public IActionResult GetCount()
        {
            try
            { 
                var unReadRequest = _donorService.GetunreadRequestByUserId(UserId);
                if (User.IsInRole("Admin"))
                {
                    returnClass.Message = "Admin";
                }                             
                returnClass.RequestCount = unReadRequest.Count();
                returnClass.IsError = false;
            }

            catch (Exception ex)
            {
                returnClass.IsError = true;
            }
            return Json(returnClass);
        }
        public async Task<IActionResult> GetDonorRequest(bool isSessionSetCount)
        {
            try
            {
                // get Current Loged in user id  
                Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
                var user = await GetCurrentUserAsync();
                var userId = user.Id;

                var requests = _donorService.GetRequestsByUserId(userId).Select(x=> new DonorRequestForAdminViewModel {
                    Id = x.Id,
                    DonorName = x.ApplicationUser.UserName,
                    CreatedOn = x.CreatedOn,
                    ExpectedDonation = x.ExpectedDonation
                }).ToList();                
                // Un Read Notification Update
                var unReadRequest = _donorService.GetunreadRequestByUserId(userId);

                List<DonorRequestForBeneficiary> ListofRequests = new List<DonorRequestForBeneficiary>();

                if (unReadRequest != null)
                {
                    foreach (var item in unReadRequest)
                    {
                        var objRequest = _donorService.Get(item.Id);
                        objRequest.IsRead = true;
                        ListofRequests.Add(objRequest);
                        
                    }
                    _donorService.UpdateRange(ListofRequests);
                }
                // SET SESSION 
                if (isSessionSetCount == true)
                    HttpContext.Session.SetInt32("unReadRequestCount", 0);                

                returnClass.ObjectData = requests;
                returnClass.RequestCount = requests.Count();
                returnClass.IsError = false;
            }

            catch (Exception ex)
            {
                returnClass.IsError = true;
            }
            return Json(returnClass);
        }
    }
}