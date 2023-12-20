using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Donors;
using RWFOUNDATIONWebsite.DataModels.Master;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.SignalR.Hubs;
using RWFOUNDATIONWebsite.ViewModels;
using RWFOUNDATIONWebsite.ViewModels.DonorViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    [Authorize(Roles = "Donor")]
    public class DonorsController : PrivateController
    {
        private readonly JsonReturn returnClass;
        private readonly RwDbContext _context;       
        private readonly GrocerykitService _grocerykitService;
        private readonly DonorService _donorService;
        private IHubContext<DonorRequestHub> _hubContext;
        private IWebHostEnvironment _hostingEnvironment;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        private string UserName { get { return HttpContext.Session.GetString("UserName"); } }
        private int CompanyId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId")); } }
        public DonorsController(JsonReturn _jsonReturn, RwDbContext context, GrocerykitService grocerykitService, 
            DonorService donorService, IHubContext<DonorRequestHub> hubContext, IWebHostEnvironment hostEnvironment)
        {
            this.returnClass = _jsonReturn;
            _context = context;
            _grocerykitService = grocerykitService;
            _donorService = donorService;
            _hubContext = hubContext;
            _hostingEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            ViewData["packages"] = new SelectList(_context.Packages.Where(x => x.IsActive == true && x.IsDeleted == false), "PackageId", "PackageName");
            var grocerylist = _grocerykitService.Getallgrocerykitfordonor(UserId).Select(x => new GroceryKitViewModel
            {
                GroceryKitId = x.GroceryKitId,
                FormNo = x.GroceryKit.FormNo,
                FirstName = x.GroceryKit.FirstName,
                LastName = x.GroceryKit.LastName,
                Occupation = x.GroceryKit.Occupation.OccupationName,
                Age = x.GroceryKit.Age,
                Status = x.GroceryKit.CurrentStatus.CurrentStatusName,
                Gender = x.GroceryKit.Gender,
                ResidenceStatus = x.GroceryKit.ResidenceStatus,
                Address = x.GroceryKit.Address,
                DateOfBirth = x.GroceryKit.DateOfBirth,
                ZakatAcceptable = x.GroceryKit.ZakatAcceptable,
                MedicalProb = x.GroceryKit.MedicalProb.MedicalProbName,
                PhoneNumber1 = x.GroceryKit.PhoneNumber1,
                FamilySize = x.GroceryKit.FamilySize,
                Salary = x.GroceryKit.Salary,
                Rating = x.GroceryKit.Rating
            }).ToList();
            return View(grocerylist);
        }
        [HttpGet]
        public IActionResult DonorProfile(int id)
        {
            var model = _context.Users.Where(x => x.Id == id).Select(x => new DonorProfileViewModel
            {
               UserId = x.Id,
               PhoneNumber = x.PhoneNumber,
               Address = x.Address,
               DateOfBirth = x.DateOfBirth,
               ToDonate = x.ToDonate,
               Estimate = x.Estimate
            }).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public IActionResult DonorProfile(DonorProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
                if(user != null)
                {                   
                    user.Id = model.UserId;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Address = model.Address;
                    user.DateOfBirth = model.DateOfBirth;
                    user.ToDonate = model.ToDonate;
                    user.Estimate = model.Estimate;

                    _context.SaveChanges();
                    return RedirectToAction("Index", "UserDashboard");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult RequestForBeneficiary()
        {
            ViewData["donationtype"] = new SelectList(_context.DonationTypes.Where(x => x.IsActive == true && x.IsDeleted == false), "DonationTypeId", "Title");
            return View();
        }
        [HttpPost]
        public IActionResult RequestedBeneficiary(RequestForBeneficiaryViewModel model)
        {
            try
            {
                var admin = _context.UserRoles.Include(x=>x.ApplicationUser).Include(x=>x.ApplicationRole).ToList();
                var adminId = 0;
                foreach (var userroles in admin)
                {
                    if (userroles.ApplicationUser.CompanyId == CompanyId && userroles.ApplicationRole.Name == "Admin")
                    {
                        adminId = userroles.UserId;
                        break;
                    }
                }
                var requestnotify = _donorService.GetRequestsByUserId(adminId).Count();
                var reqeust = new DonorRequestForBeneficiary();              
                reqeust.DonationTypeId = model.DonationTypeId; 
                reqeust.BeneficiaryType = model.BeneficiaryType;
                reqeust.ExpectedDonation = model.ExpectedDonation;
                reqeust.RequestTo = adminId;
                reqeust.DonorId = UserId;
                reqeust.CreatedBy = UserId;
                reqeust.CreatedOn = DateTime.Now;
                reqeust.IsActive = true;
                reqeust.IsDeleted = false;
                reqeust.IsRead = false;
                int req = _donorService.Create(ref reqeust);
                List<string> sizelist = new List<string>();
                foreach (var item in model.FamilyMemberSize)
                {
                    var m = new FamilyMemberForDonor();
                    m.DonorRequestId = req;
                    m.FamilyMember = item;
                    sizelist.Add(item);
                    _context.FamilyMemberForDonors.Add(m);
                    _context.SaveChanges();
                }               
                var donation = _context.DonationTypes.Where(x => x.DonationTypeId == model.DonationTypeId).FirstOrDefault();
                _hubContext.Clients.All.SendAsync("DonorRequest", requestnotify, model.BeneficiaryType);
                EmailBodyForRequest(UserName, string.Format("{0:MMMM dd, yyyy}", reqeust.CreatedOn), donation.Title, model.BeneficiaryType, model.ExpectedDonation, sizelist);
                if (model.BeneficiaryType == "Auto")
                {
                    List<GrocerykitFindForDonor> listkits = new List<GrocerykitFindForDonor>();                   
                    foreach (var size in model.FamilyMemberSize)
                    {
                        if(Convert.ToInt32(size) > 0)
                        {
                            switch (size)
                            {
                                case "1":
                                    var list1 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 1)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list1);
                                    break;
                                case "2":
                                    var list2 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 2)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list2);
                                    break;
                                case "3":
                                    var list3 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 3)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list3);
                                    break;
                                case "4":
                                    var list4 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 4)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list4);
                                    break;
                                case "5":
                                    var list5 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 5)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list5);
                                    break;
                                case "6":
                                    var list6 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 6)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list6);
                                    break;
                                case "7":
                                    var list7 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize == 7)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list7);
                                    break;
                                case "7+":
                                    var list8 = _grocerykitService.GetAll()
                                                .Where(x => x.CompanyId == CompanyId && x.ShortFallInCash > 0 &&
                                                x.ShortFallInCash < model.ExpectedDonation && x.FamilySize > 7)
                                                .OrderByDescending(x => x.Rating).Select(x => new GrocerykitFindForDonor
                                                {
                                                    GroceryKitId = x.GroceryKitId,
                                                    ShortFallInCash = x.ShortFallInCash,
                                                    FamilySize = x.FamilySize,
                                                }).ToList();
                                    listkits.AddRange(list8);
                                    break;
                            }
                        }
                       
                        
                    }
                                       
                    int donateamount = model.ExpectedDonation;
                    List<Assign> Extractedlist = new List<Assign>();                    
                    foreach (var item in listkits)
                    {
                        int amount = 0;
                        if(donateamount > 0)
                        {
                            var assign = _context.GroceryKitAssigns.Where(x => x.GroceryKitId == item.GroceryKitId && x.UserId == UserId).FirstOrDefault();
                            
                            if (item.ShortFallInCash < donateamount)
                            {                               
                                if(assign == null)
                                {
                                    Assign m = new Assign();
                                    amount = Convert.ToInt32(item.ShortFallInCash);
                                    donateamount -= amount;
                                    m.GroceryKitId = item.GroceryKitId;
                                    Extractedlist.Add(m);
                                }                                                                
                            }else{break;}                      
                        }else{break;}
                        
                    }
                    if(Extractedlist.Count > 0)
                    {
                        List<GroceryKitAssign> assignlist = new List<GroceryKitAssign>();
                        foreach (var item in Extractedlist)
                        {
                            var assign = new GroceryKitAssign();
                            assign.GroceryKitId = item.GroceryKitId;
                            assign.UserId = UserId;
                            assign.CreatedBy = UserId;                           
                            assign.CreatedOn = DateTime.Now;
                            assign.IsActive = true;
                            assign.IsDeleted = false;
                            assignlist.Add(assign);

                            var kit = _grocerykitService.Get(item.GroceryKitId);
                            if(kit != null)
                            {
                                kit.ApplicationStatus = "Transferred";
                                _context.SaveChanges();
                            }
                        }
                        _context.GroceryKitAssigns.AddRange(assignlist);
                        _context.SaveChanges();
                    }
                    else
                    {
                        returnClass.NullMessage = "We don't have any Beneficiary Data to Assign you on the behalf of your Preference. Please change your Preferences Or contact Admin for more detail";
                    }

                }
                else
                {
                    returnClass.Message = model.BeneficiaryType;
                }                
            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
        [HttpPost]
        public IActionResult AutoCalculation(GetValueToProcessViewModel model)
        {
            try
            {
                List<PackageValue> values = new List<PackageValue>();
                List<BeneficaryForCalculation> caludata = new List<BeneficaryForCalculation>();
                decimal EstimatedExpenses = 0;
                decimal Packagevalue = 0;
                int size = 0;
                if(model.FormNos != null)
                {
                    foreach (var item in model.FormNos)
                    {
                        if(item.FamilySize > 7)
                        {
                            var package = _context.PackageDetails
                           .Include(x => x.PackageItems)
                           .Where(x => x.PackageId == model.PackageId && x.TotalFamilyMember == 7 && x.IsActive == true && x.IsDeleted == false).ToList();
                            if (package != null)
                            {
                                foreach (var value in package)
                                {
                                    var pv = new PackageValue();
                                    pv.PackageValuePkr = value.PackageValuePKR;
                                    pv.TotalFamilyMember = value.TotalFamilyMember;
                                    pv.PackgeFor = value.PackageDetailName;
                                    Packagevalue += value.PackageValuePKR;
                                    values.Add(pv);
                                }
                            }
                        }
                        else
                        {
                            var package = _context.PackageDetails
                            .Include(x => x.PackageItems)
                            .Where(x => x.PackageId == model.PackageId && x.TotalFamilyMember == item.FamilySize && x.IsActive == true && x.IsDeleted == false).ToList();
                            if (package != null)
                            {
                                foreach (var value in package)
                                {
                                    var pv = new PackageValue();
                                    pv.PackageValuePkr = value.PackageValuePKR;
                                    pv.TotalFamilyMember = value.TotalFamilyMember;
                                    pv.PackgeFor = value.PackageDetailName;
                                    Packagevalue += value.PackageValuePKR;
                                    values.Add(pv);
                                }
                            }
                        }
                        
                        var beneficiary = _context.GroceryKits
                                            .Include(x => x.FamilyMembers)                                          
                                            .Include(x => x.CurrentStatus)                                            
                                            .Where(x => x.FormNo == item.FormNo && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        if (beneficiary != null)
                        {
                            var bc = new BeneficaryForCalculation();
                            bc.Status = beneficiary.CurrentStatus.CurrentStatusName;
                            bc.Dependents = beneficiary.FamilyMembers.Count();
                            bc.Shortfall = beneficiary.ShortFallInCash;
                            bc.Size = beneficiary.FamilySize;
                            EstimatedExpenses += beneficiary.ShortFallInCash;
                            size += beneficiary.FamilySize;
                            caludata.Add(bc);
                        }
                        int perdaymeals = size * 3;
                        int totalmeals = perdaymeals * (model.NoOfMonth * 30);
                        var list = new ListData();
                        list.PackageValues = values;
                        list.BeneficaryForCalculations = caludata;
                        returnClass.ObjectData = list;
                        returnClass.EstimatedExpense = EstimatedExpenses * model.NoOfMonth;
                        returnClass.Packagevalue = Packagevalue * model.NoOfMonth;
                        returnClass.TotalMeals = totalmeals;

                    }
                }
                
               

            }
            catch (Exception ex)
            {

                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }

            return Json(returnClass);
        }

        public IActionResult GetDataMonthWise(GetValueToProcessViewModel model)
        {
            try
            {
                decimal EstimatedExpenses = 0;
                decimal Packagevalue = 0;
                int size = 0;
                if (model.FormNos != null)
                {
                    foreach (var item in model.FormNos)
                    {
                        if(item.FamilySize > 7)
                        {
                            var package = _context.PackageDetails
                            .Include(x => x.PackageItems)
                            .Where(x => x.PackageId == model.PackageId && x.TotalFamilyMember == 7 && x.IsActive == true && x.IsDeleted == false).ToList();
                            if (package != null)
                            {
                                foreach (var value in package)
                                {
                                    Packagevalue += value.PackageValuePKR;
                                }
                            }
                        }
                        else
                        {
                            var package = _context.PackageDetails
                            .Include(x => x.PackageItems)
                            .Where(x => x.PackageId == model.PackageId && x.TotalFamilyMember == item.FamilySize && x.IsActive == true && x.IsDeleted == false).ToList();
                            if (package != null)
                            {
                                foreach (var value in package)
                                {
                                    Packagevalue += value.PackageValuePKR;
                                }
                            }
                        }
                        
                        var beneficiary = _context.GroceryKits
                                            .Include(x => x.FamilyMembers)                                           
                                            .Include(x => x.CurrentStatus)                                           
                                            .Where(x => x.FormNo == item.FormNo && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        if (beneficiary != null)
                        {
                            EstimatedExpenses += beneficiary.ShortFallInCash;
                            size += beneficiary.FamilySize;                           
                        }
                        int perdaymeals = size * 3;
                        int totalmeals = perdaymeals * (model.NoOfMonth * 30);
                        returnClass.EstimatedExpense = EstimatedExpenses * model.NoOfMonth;
                        returnClass.Packagevalue = Packagevalue * model.NoOfMonth;
                        returnClass.TotalMeals = totalmeals;

                    }
                }
            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);

        }
        public IActionResult PackageChange(GetValueToProcessViewModel model)
        {
            try
            {
                decimal EstimatedExpenses = 0;
                decimal Packagevalue = 0;
                int size = 0;
                if (model.FormNos != null)
                {
                    foreach (var item in model.FormNos)
                    {
                        if (item.FamilySize > 7)
                        {
                            var package = _context.PackageDetails
                            .Include(x => x.PackageItems)
                            .Where(x => x.PackageId == model.PackageId && x.TotalFamilyMember == 7 && x.IsActive == true && x.IsDeleted == false).ToList();
                            if (package != null)
                            {
                                foreach (var value in package)
                                {
                                    Packagevalue += value.PackageValuePKR;
                                }
                            }
                        }
                        else
                        {
                            var package = _context.PackageDetails
                            .Include(x => x.PackageItems)
                            .Where(x => x.PackageId == model.PackageId && x.TotalFamilyMember == item.FamilySize && x.IsActive == true && x.IsDeleted == false).ToList();
                            if (package != null)
                            {
                                foreach (var value in package)
                                {
                                    Packagevalue += value.PackageValuePKR;
                                }
                            }
                        }

                        var beneficiary = _context.GroceryKits
                                            .Include(x => x.FamilyMembers)
                                            .Include(x => x.CurrentStatus)
                                            .Where(x => x.FormNo == item.FormNo && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        if (beneficiary != null)
                        {
                            EstimatedExpenses += beneficiary.ShortFallInCash;
                            size += beneficiary.FamilySize;
                        }
                        int perdaymeals = size * 3;
                        int totalmeals = perdaymeals * (model.NoOfMonth * 30);
                        returnClass.EstimatedExpense = EstimatedExpenses * model.NoOfMonth;
                        returnClass.Packagevalue = Packagevalue * model.NoOfMonth;
                        returnClass.TotalMeals = totalmeals;

                    }
                }
            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);

        }
        public IActionResult SponcerTo(List<SponcerToViewModel> sponcers, int noofmonth, string totalmeals)
        {
            ViewData["NoOfMonth"] = noofmonth;
            ViewData["totalmeals"] = totalmeals;

            List<DonorSponcerViewModel> list = new List<DonorSponcerViewModel>();
            foreach (var item in sponcers)
            {
                var model = new DonorSponcerViewModel();
                var grocerykit = _context.GroceryKits.Include(x => x.CurrentStatus).Where(x => x.GroceryKitId == item.GroceryKitId).FirstOrDefault();
                model.GroceryKitId = grocerykit.GroceryKitId;
                model.FirstName = grocerykit.FirstName;
                model.LastName = grocerykit.LastName;
                model.CNIC = grocerykit.CNIC;
                model.Shortfallincash = grocerykit.ShortFallInCash;
                model.Status = grocerykit.CurrentStatus.CurrentStatusName;
                model.FamilySize = grocerykit.FamilySize;
                list.Add(model);
            }
            return PartialView("~/Views/GroceryKit/_SponcerToPartialView.cshtml", list);
        }
        public IActionResult SponsorToData(SponsorDataViewModel model)
        {
            try
            {
                int noofmonth = model.NoOfMonth;
                int totalfamilies = model.TotalFamilies;
                decimal estimates = model.EstimatedExpense;
                decimal totalmeals = model.TotalMeals;
                List<DonorSponcer> sposorlist = new List<DonorSponcer>();
                foreach (var item in model.Beneficiaries)
                {
                    var sponsor = new DonorSponcer();
                    sponsor.GroceryKitId = item.GroceryKitId;
                    sponsor.NoOfMonth = noofmonth;
                    sponsor.TotalFamilies = totalfamilies;
                    sponsor.TotalMeals = totalmeals;
                    sponsor.EstimatedExpense = estimates;
                    sponsor.DonorId = UserId;
                    sponsor.CreatedBy = UserId;
                    sponsor.CreatedOn = DateTime.Now;
                    sponsor.IsActive = true;
                    sponsor.IsDeleted = false;
                    sposorlist.Add(sponsor);

                    var benefi = _context.GroceryKits.Where(x => x.GroceryKitId == item.GroceryKitId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    benefi.SponsorStatus = "Sponsored";
                    _context.SaveChanges();
                }
                _context.DonorSponcers.AddRange(sposorlist);
                _context.SaveChanges();



            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }

        public IActionResult DonorSponsorList()
        {
            var model = _context.DonorSponcers
                .Include(x => x.GroceryKit)
                    .ThenInclude(x=>x.CurrentStatus)
                .Include(x => x.GroceryKit)
                    .ThenInclude(x => x.MedicalProb)
                .Where(x => x.DonorId == UserId && x.IsActive == true && x.IsDeleted == false).ToList();
            return View(model);
        }

        public IActionResult EmailBodyForRequest(string donorname, string createddate, string donationtype, string beneficiarytype, decimal amount, List<string> membersize)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var pathToFile = _hostingEnvironment.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "lib"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplates"
                + Path.DirectorySeparatorChar.ToString()
                + "DonorRequest.html";
                string subject = "Donor Request For Beneficiaries";
                var builder = new BodyBuilder();

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();

                }               
               
                var arrayTesting = membersize.ToArray<string>();
                var result = string.Join(",", arrayTesting);
                string messageBody = string.Format(builder.HtmlBody,
                  createddate,
                  donorname,
                  donationtype,
                  beneficiarytype,
                  amount,
                  result
                );
                Helper.EmailSending.SendMail("web@rwfoundation.org", subject, messageBody);
               //Helper.EmailSending.SendMail("musawirk30@gmail.com", subject, messageBody);
                returnClass.IsEmailSent = true;
            }
            catch (Exception ex)
            {
                returnClass.IsEmailSent = false;
            }
            return Json(returnClass);

        }
    }
}