using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Master;
using RWFOUNDATIONWebsite.Extensions;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Models;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels;
using RWFOUNDATIONWebsite.ViewModels.DonorViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    public class GroceryKitController : PrivateController
    {
        private readonly JsonReturn returnClass;
        private readonly RwDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly GrocerykitService _grocerykitService;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        private int CompanyId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId")); } }
        public GroceryKitController(JsonReturn _jsonReturn, RwDbContext context, IWebHostEnvironment hostingEnvironment,
            GrocerykitService grocerykitService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.returnClass = _jsonReturn;
            _context = context;
            _grocerykitService = grocerykitService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {            
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody]DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "GroceryKitId";
                orderAscendingDirection = true;
            }

            var result = await _context.GroceryKits
                 .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.Relation)
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.FamilyMemberStatus)
                .Include(x => x.Occupation)
                .Include(x => x.MedicalProb)
                .Include(x => x.CurrentStatus)
                .Include(x => x.Province)
                .Include(x => x.City)
                .Where(x => x.IsActive == true && x.IsDeleted == false && x.CompanyId == CompanyId)
                .Select(x => new GroceryKitViewModel
                {
                    GroceryKitId = x.GroceryKitId,
                    FormNo = x.FormNo,
                    OldFormNo = x.OldFormNo,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Occupation = x.Occupation.OccupationName,
                    Age = x.Age,
                    Status = x.CurrentStatus.CurrentStatusName,
                    Gender = x.Gender,
                    ResidenceStatus = x.ResidenceStatus,
                    Address = x.Address,
                    DateOfBirth = x.DateOfBirth,
                    ZakatAcceptable = x.ZakatAcceptable,
                    MedicalProb = x.MedicalProb.MedicalProbName,
                    PhoneNumber1 = x.PhoneNumber1,
                    FamilySize = x.FamilySize,
                    Salary = x.Salary
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.FormNo != null && r.FormNo.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.OldFormNo != null && r.OldFormNo.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.FirstName != null && r.FirstName.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.LastName != null && r.LastName.ToUpper().Contains(searchBy.ToUpper()))                                          
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.GroceryKits.CountAsync();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)                    
                    .ToList()
            });
        }
        [Authorize(Roles = "Admin,Donor,Beneficiary")]
        [HttpGet]
        public IActionResult Detail(int id)
        {            
            var detail = _grocerykitService.Get(id);           
            return View(detail);
        }  
        [Authorize(Roles = "Admin")]
        public IActionResult GetFormNo()
        {
            try
            {
                var number = _context.GroceryKits.OrderByDescending(x=>x.FormNo).FirstOrDefault();
                string formNo = "0";
                int no = 0;
                if (number != null)
                {
                    int num = Convert.ToInt32(number.FormNo) + 1;
                    formNo = "Form No #" + num;
                    no = num;
                }
                else
                {
                    formNo = "Form No#1";
                    no = 1;
                }
                returnClass.Message = formNo;
                returnClass.formno = no;
            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
        [Authorize(Roles = "Admin,Beneficiary")]
        [HttpGet]
        public IActionResult GroceryKitForm()
        {
            var draftdata = _context.BeneficiaryFormSaveAsDrafts
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.Relation)
                .Include(x => x.FamilyMembers)
                    .ThenInclude(x => x.FamilyMemberStatus)
                .Include(x => x.Occupation)
                .Include(x => x.MedicalProb)
                .Include(x => x.CurrentStatus)
                .Include(x => x.Province)
                .Include(x => x.City)
                .Where(x => x.CreatedBy == UserId).FirstOrDefault();
            if(draftdata == null)
            {
                if (User.IsInRole("Beneficiary") && _context.GroceryKits.Where(x => x.DataCollectedById == UserId).FirstOrDefault() != null)
                {
                    ViewData["alreadyadded"] = "You have Already Added Your Application. You cannot add more than one Application";
                }
                else
                {
                    ViewData["Relations"] = new SelectList(_context.Relations.Where(x => x.IsActive == true && x.IsDeleted == false), "RelationId", "RelationName");
                    ViewData["MedicalProb"] = new SelectList(_context.MedicalProbs.Where(x => x.IsActive == true && x.IsDeleted == false), "MedicalProbId", "MedicalProbName");
                    ViewData["Occupation"] = new SelectList(_context.Occupations.Where(x => x.IsActive == true && x.IsDeleted == false), "OccupationId", "OccupationName");
                    ViewData["CurrentStatus"] = new SelectList(_context.CurrentStatuses.Where(x => x.IsActive == true && x.IsDeleted == false), "CurrentStatusId", "CurrentStatusName");
                    ViewData["FamilyMemberStatus"] = new SelectList(_context.FamilyMemberStatuses.Where(x => x.IsActive == true && x.IsDeleted == false), "FamilyMemberStatusId", "FamilyMemberStatusName");
                    ViewData["Province"] = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
                    ViewData["City"] = new SelectList(_context.Cities, "CityId", "CityName");
                }
                return View();
            }
            else
            {
                if (User.IsInRole("Beneficiary") && _context.GroceryKits.Where(x => x.DataCollectedById == UserId).FirstOrDefault() != null)
                {
                    ViewData["alreadyadded"] = "You have Already Added Your Application. You cannot add more than one Application";
                }
                else
                {
                    ViewData["Relations"] = new SelectList(_context.Relations.Where(x => x.IsActive == true && x.IsDeleted == false), "RelationId", "RelationName");
                    ViewData["MedicalProb"] = new SelectList(_context.MedicalProbs.Where(x => x.IsActive == true && x.IsDeleted == false), "MedicalProbId", "MedicalProbName");
                    ViewData["Occupation"] = new SelectList(_context.Occupations.Where(x => x.IsActive == true && x.IsDeleted == false), "OccupationId", "OccupationName");
                    ViewData["CurrentStatus"] = new SelectList(_context.CurrentStatuses.Where(x => x.IsActive == true && x.IsDeleted == false), "CurrentStatusId", "CurrentStatusName");
                    ViewData["FamilyMemberStatus"] = new SelectList(_context.FamilyMemberStatuses.Where(x => x.IsActive == true && x.IsDeleted == false), "FamilyMemberStatusId", "FamilyMemberStatusName");
                    ViewData["Province"] = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
                    ViewData["City"] = new SelectList(_context.Cities, "CityId", "CityName");
                }
                return View(draftdata);
            }          
           
           
        }
        [Authorize(Roles = "Admin,Beneficiary")]
        [HttpPost]
        public IActionResult GroceryKitData(IFormFile photo, IFormFile cnicfront, IFormFile cnicback, 
            IFormFile disabilitycertificate, IFormFile deathcertificate, IFormFile bform, IFormFile electricitybill, 
            IFormFile ptclbill, IFormFile application,IFormFile OtherDocument,IFormFile OtherDocument2, IFormFile video, IFormFile audio, string model)
        {
            try
            {
                if(CompanyId > 0 && UserId > 0)
                {
                    var checkDraftData = _context.BeneficiaryFormSaveAsDrafts
                                         .Include(x => x.FamilyMembers)
                                            .ThenInclude(x => x.Relation)
                                         .Include(x => x.FamilyMembers)
                                            .ThenInclude(x => x.FamilyMemberStatus)
                                         .Include(x => x.Occupation)
                                         .Include(x => x.MedicalProb)
                                         .Include(x => x.CurrentStatus)
                                         .Include(x => x.Province)
                                         .Include(x => x.City)
                                         .Where(x => x.CreatedBy == UserId).FirstOrDefault();
                    var number = _context.GroceryKits.ToList().LastOrDefault();
                    string formNo = "0";
                    int no = 0;
                    if (number != null)
                    {
                        int num = Convert.ToInt32(number.FormNo) + 1;
                        formNo = "Form No #" + num;
                        no = num;
                    }
                    else
                    {
                        formNo = "Form No#1";
                        no = 1;
                    }

                    var data = JsonConvert.DeserializeObject<GroceryKitCreateViewModel>(model);
                    var datamodel = new GroceryKit();
                    datamodel.FormNo = Convert.ToString(no);
                    datamodel.FirstName = data.FirstName;
                    datamodel.LastName = data.LastName;
                    datamodel.FatherOrHusbandName = data.FatherOrHusbandName;
                    datamodel.Address = data.Address;
                    datamodel.CNIC = data.CNIC;
                    datamodel.OldFormNo = data.OldFormNo;
                    datamodel.MedicalProbId = data.MedicalProbId;
                    datamodel.PhoneNumber1 = data.PhoneNumber1;
                    datamodel.PhoneNumber2 = data.PhoneNumber2;
                    datamodel.OccupationId = data.OccupationId;
                    datamodel.CityId = data.CityId;
                    datamodel.ProvinceId = data.ProvinceId;
                    datamodel.CurrentStatusId = data.CurrentStatusId;
                    datamodel.ApplicationDate = DateTime.Now;
                    datamodel.Gender = data.Gender;
                    datamodel.ResidenceStatus = data.ResidenceStatus;
                    datamodel.MeritalStatus = data.MeritalStatus;
                    datamodel.DateOfBirth = data.DateOfBirth;
                    datamodel.Age = Convert.ToInt32(DateTime.Now.Year - data.DateOfBirth.Year);
                    datamodel.FoodCost = data.FoodCost;
                    datamodel.HouseRent = data.HouseRent;
                    datamodel.SchoolCost = data.SchoolCost;
                    datamodel.UtilitiesCost = data.UtilitiesCost;
                    datamodel.MedicalCost = data.MedicalCost;
                    datamodel.OtherCost = data.OtherCost;
                    datamodel.TotalCost = data.TotalCost;
                    datamodel.ZakatAcceptable = data.ZakatAcceptable;
                    datamodel.FamilySize = data.FamilySize;
                    datamodel.Salary = data.Salary;
                    datamodel.Donations = data.Donations;
                    datamodel.OtherIncome = data.OtherIncome;
                    datamodel.TotalIncome = data.TotalIncome;
                    datamodel.ShortFallInCash = data.ShortFallInCash;
                    datamodel.Remarks = data.Remarks;                    
                    if(checkDraftData != null)
                    {
                        if (!String.IsNullOrEmpty(checkDraftData.ImageUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/MainPhoto");
                            string filename = no + checkDraftData.ImageUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_MainPhoto", checkDraftData.ImageUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.ImageUrl = filename;
                        }
                        else
                        {
                            datamodel.ImageUrl = ProcessUploadImage(photo, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.CNICFrontUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/CNIC");
                            string filename = no + checkDraftData.CNICFrontUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_CNIC", checkDraftData.CNICFrontUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.CNICFrontUrl = filename;
                        }
                        else
                        {
                            datamodel.CNICFrontUrl = ProcessUploadImageCnic(cnicfront, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.CNICBackUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/CNIC");
                            string filename = no + checkDraftData.CNICBackUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_CNIC", checkDraftData.CNICBackUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.CNICBackUrl = filename;
                        }
                        else
                        {
                            datamodel.CNICBackUrl = ProcessUploadImageCnic(cnicback, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.DisabilityCertificateUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/DisabilityCertificate");
                            string filename = no + checkDraftData.DisabilityCertificateUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_DisabilityCertificate", checkDraftData.DisabilityCertificateUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.DisabilityCertificateUrl = filename;
                        }
                        else
                        {
                            datamodel.DisabilityCertificateUrl = ProcessUploadImagedis(disabilitycertificate, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.DeathCertificateUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/DeathCertificate");
                            string filename = no + checkDraftData.DeathCertificateUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_DeathCertificate", checkDraftData.DeathCertificateUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.DeathCertificateUrl = filename;
                        }
                        else
                        {
                            datamodel.DeathCertificateUrl = ProcessUploadImagedeath(deathcertificate, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.BFormUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/B-FORM");
                            string filename = no + checkDraftData.BFormUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_B-FORM", checkDraftData.BFormUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.BFormUrl = filename;
                        }
                        else
                        {
                            datamodel.BFormUrl = ProcessUploadImagebform(bform, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.ElectricityBill))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ElectricityBill");
                            string filename = no + checkDraftData.ElectricityBill;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_ElectricityBill", checkDraftData.ElectricityBill);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.ElectricityBill = filename;
                        }
                        else
                        {
                            datamodel.ElectricityBill = ProcessUploadImageelectricity(electricitybill, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.PtclBillUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/PTCLBILL");
                            string filename = no + checkDraftData.PtclBillUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_PTCLBILL", checkDraftData.PtclBillUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.PtclBillUrl = filename;
                        }
                        else
                        {
                            datamodel.PtclBillUrl = ProcessUploadImageptcl(ptclbill, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.ApplicationUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Applications");
                            string filename = no + checkDraftData.ApplicationUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Applications", checkDraftData.ApplicationUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.ApplicationUrl = filename;
                        }
                        else
                        {
                            datamodel.ApplicationUrl = ProcessUploadImageapp(application, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.OtherDocument1Url))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/OtherDocuments");
                            string filename = no + checkDraftData.OtherDocument1Url;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_OtherDocuments", checkDraftData.OtherDocument1Url);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.OtherDocument1Url = filename;
                        }
                        else
                        {
                            datamodel.OtherDocument1Url = ProcessUploadOtherDocument(OtherDocument, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.OtherDocument2Url))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/OtherDocuments");
                            string filename = no + checkDraftData.OtherDocument2Url;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_OtherDocuments", checkDraftData.OtherDocument2Url);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.OtherDocument2Url = filename;
                        }
                        else
                        {
                            datamodel.OtherDocument2Url = ProcessUploadOtherDocument(OtherDocument2, CompanyId, no);
                        }                       
                        if (!String.IsNullOrEmpty(checkDraftData.VideoUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Videos");
                            string filename = no + checkDraftData.VideoUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Videos", checkDraftData.VideoUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.VideoUrl = filename;
                        }
                        else
                        {
                            datamodel.VideoUrl = ProcessUploadVideo(video, CompanyId, no);
                        }
                        if (!String.IsNullOrEmpty(checkDraftData.AudioUrl))
                        {
                            string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Audios");
                            string filename = no + checkDraftData.AudioUrl;
                            string filePath = Path.Combine(Folder, filename);
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Audios", checkDraftData.AudioUrl);
                            System.IO.File.Copy(fileUrl, filePath, true);
                            System.IO.File.Delete(fileUrl);
                            datamodel.AudioUrl = filename;
                        }
                        else
                        {
                            datamodel.AudioUrl = ProcessUploadAudio(audio, CompanyId, no);
                        }

                    }
                    else
                    {
                        datamodel.ImageUrl = ProcessUploadImage(photo, CompanyId, no);
                        datamodel.CNICFrontUrl = ProcessUploadImageCnic(cnicfront, CompanyId, no);
                        datamodel.CNICBackUrl = ProcessUploadImageCnic(cnicback, CompanyId, no);
                        datamodel.DisabilityCertificateUrl = ProcessUploadImagedis(disabilitycertificate, CompanyId, no);
                        datamodel.DeathCertificateUrl = ProcessUploadImagedeath(deathcertificate, CompanyId, no);
                        datamodel.BFormUrl = ProcessUploadImagebform(bform, CompanyId, no);
                        datamodel.ElectricityBill = ProcessUploadImageelectricity(electricitybill, CompanyId, no);
                        datamodel.PtclBillUrl = ProcessUploadImageptcl(ptclbill, CompanyId, no);
                        datamodel.ApplicationUrl = ProcessUploadImageapp(application, CompanyId, no);
                        datamodel.OtherDocument1Url = ProcessUploadOtherDocument(OtherDocument, CompanyId, no);
                        datamodel.OtherDocument2Url = ProcessUploadOtherDocument(OtherDocument2, CompanyId, no);
                        datamodel.VideoUrl = ProcessUploadVideo(video, CompanyId, no);
                        datamodel.AudioUrl = ProcessUploadAudio(audio, CompanyId, no);
                    }
                    datamodel.OtherDocumentName1 = data.OtherDocumentName1;
                    datamodel.OtherDocumentName2 = data.OtherDocumentName2;

                    datamodel.DataCollectedById = UserId;
                    datamodel.AuthorizedById = 0;
                    datamodel.CompanyId = CompanyId;
                    var currentstatus = _context.CurrentStatuses.Where(x => x.CurrentStatusId == data.CurrentStatusId).FirstOrDefault();
                    if(currentstatus != null)
                    {
                        datamodel.Rating = Convert.ToDecimal(getrating(data.FamilySize, data.ShortFallInCash, data.ResidenceStatus, currentstatus.CurrentStatusName));
                    }
                    else
                    {
                        datamodel.Rating = 1;
                    }
                    
                    datamodel.ApplicationStatus = "Submitted";
                    datamodel.CreatedBy = UserId;
                    datamodel.CreatedOn = DateTime.Now;
                    datamodel.IsActive = true;
                    datamodel.IsDeleted = false;
                    int grocerykitId = _grocerykitService.AddGrocerykit(ref datamodel);

                    foreach (var item in data.FamilyMembers)
                    {
                        var familymember = new FamilyMember();
                        familymember.Name = item.Name;
                        familymember.RelationId = item.RelationId;
                        familymember.Age = item.Age;
                        familymember.FamilyMemberStatusId = item.FamilyMemberStatusId;
                        familymember.GroceryKitId = grocerykitId;
                        familymember.CreatedBy = UserId;
                        familymember.CreatedOn = DateTime.Now;
                        familymember.IsActive = true;
                        familymember.IsDeleted = false;
                        _context.FamilyMembers.Add(familymember);
                        _context.SaveChanges();
                    }
                    if(checkDraftData != null)
                    {
                        _context.BeneficiaryFormSaveAsDrafts.Remove(checkDraftData);
                        _context.SaveChanges();
                    }
                    returnClass.Message = formNo;
                    returnClass.RoleName = RoleName;
                }
                else
                {                                     
                    return RedirectToAction("Index", "UserDashboard");
                }

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }      

        [Authorize(Roles = "Admin,Beneficiary")]
        public IActionResult Update(int Id)
        {
            var grocery = _grocerykitService.Get(Id);
            ViewData["Relations"] = new SelectList(_context.Relations.Where(x => x.IsActive == true && x.IsDeleted == false), "RelationId", "RelationName");
            ViewData["MedicalProb"] = new SelectList(_context.MedicalProbs.Where(x => x.IsActive == true && x.IsDeleted == false), "MedicalProbId", "MedicalProbName");
            ViewData["Occupation"] = new SelectList(_context.Occupations.Where(x => x.IsActive == true && x.IsDeleted == false), "OccupationId", "OccupationName");
            ViewData["CurrentStatus"] = new SelectList(_context.CurrentStatuses.Where(x => x.IsActive == true && x.IsDeleted == false), "CurrentStatusId", "CurrentStatusName");
            ViewData["FamilyMemberStatus"] = new SelectList(_context.FamilyMemberStatuses.Where(x => x.IsActive == true && x.IsDeleted == false), "FamilyMemberStatusId", "FamilyMemberStatusName");
            ViewData["Province"] = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
            ViewData["City"] = new SelectList(_context.Cities, "CityId", "CityName");

            return View(grocery);
        }
        [Authorize(Roles = "Admin,Beneficiary")]
        [HttpPost]
        public IActionResult UpdateForm(IFormFile photoupdate, IFormFile cnicfrontupdate, IFormFile cnicbackupdate,
           IFormFile disabilitycertificateupdate, IFormFile deathcertificateupdate, IFormFile bformupdate, IFormFile electricitybillupdate,
           IFormFile ptclbillupdate, IFormFile applicationupdate, IFormFile OtherDocumentupdate, IFormFile OtherDocument2update, 
           IFormFile VideoUpdate, IFormFile AudioUpdate, string modelupdate)
        {
            try
            {
                if(CompanyId > 0 && UserId > 0)
                {
                    var data = JsonConvert.DeserializeObject<GroceryKitCreateViewModel>(modelupdate);
                    var details = _context.FamilyMembers.Where(c => c.GroceryKitId == data.GroceryKitId).ToList();
                    _context.RemoveRange(details);

                    var datamodel = _context.GroceryKits.FirstOrDefault(x => x.GroceryKitId == data.GroceryKitId);
                    datamodel.GroceryKitId = data.GroceryKitId;
                    datamodel.FormNo = data.FormNo;
                    datamodel.FirstName = data.FirstName;
                    datamodel.LastName = data.LastName;
                    datamodel.FatherOrHusbandName = data.FatherOrHusbandName;
                    datamodel.Address = data.Address;
                    datamodel.CNIC = data.CNIC;
                    datamodel.OldFormNo = data.OldFormNo;
                    datamodel.MedicalProbId = data.MedicalProbId;
                    datamodel.PhoneNumber1 = data.PhoneNumber1;
                    datamodel.PhoneNumber2 = data.PhoneNumber2;
                    datamodel.OccupationId = data.OccupationId;
                    datamodel.CityId = data.CityId;
                    datamodel.ProvinceId = data.ProvinceId;
                    datamodel.CurrentStatusId = data.CurrentStatusId;
                    datamodel.ApplicationDate = DateTime.Now;
                    datamodel.Gender = data.Gender;
                    datamodel.ResidenceStatus = data.ResidenceStatus;
                    datamodel.MeritalStatus = data.MeritalStatus;
                    datamodel.DateOfBirth = data.DateOfBirth;
                    datamodel.Age = Convert.ToInt32(DateTime.Now.Year - data.DateOfBirth.Year);
                    datamodel.FoodCost = data.FoodCost;
                    datamodel.HouseRent = data.HouseRent;
                    datamodel.SchoolCost = data.SchoolCost;
                    datamodel.UtilitiesCost = data.UtilitiesCost;
                    datamodel.MedicalCost = data.MedicalCost;
                    datamodel.OtherCost = data.OtherCost;
                    datamodel.TotalCost = data.TotalCost;
                    datamodel.ZakatAcceptable = data.ZakatAcceptable;
                    datamodel.FamilySize = data.FamilySize;
                    datamodel.Salary = data.Salary;
                    datamodel.Donations = data.Donations;
                    datamodel.OtherIncome = data.OtherIncome;
                    datamodel.TotalIncome = data.TotalIncome;
                    datamodel.ShortFallInCash = data.ShortFallInCash;
                    datamodel.Remarks = data.Remarks;

                    var currentstatus = _context.CurrentStatuses.Where(x => x.CurrentStatusId == data.CurrentStatusId).FirstOrDefault();
                    if (currentstatus != null)
                    {
                        datamodel.Rating = Convert.ToDecimal(getrating(data.FamilySize, data.ShortFallInCash, data.ResidenceStatus, currentstatus.CurrentStatusName));
                    }

                    if (photoupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.ExistingPhotoUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/MainPhoto", data.ExistingPhotoUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.ImageUrl = ProcessUploadImage(photoupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (cnicfrontupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.CNICFrontUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/CNIC", data.CNICFrontUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.CNICFrontUrl = ProcessUploadImageCnic(cnicfrontupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (cnicbackupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.CNICBackUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/CNIC", data.CNICBackUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.CNICBackUrl = ProcessUploadImageCnic(cnicbackupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (disabilitycertificateupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.DisabilityCertificateUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/DisabilityCertificate", data.DisabilityCertificateUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.DisabilityCertificateUrl = ProcessUploadImagedis(disabilitycertificateupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (deathcertificateupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.DeathCertificateUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/DeathCertificate", data.DeathCertificateUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.DeathCertificateUrl = ProcessUploadImagedeath(deathcertificateupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }

                    if (bformupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.BFormUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/B-FORM", data.BFormUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.BFormUrl = ProcessUploadImagebform(bformupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }

                    if (electricitybillupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.ElectricityBill))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/ElectricityBill", data.ElectricityBill);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.ElectricityBill = ProcessUploadImageelectricity(electricitybillupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }

                    if (ptclbillupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.PtclBillUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/PTCLBILL", data.PtclBillUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.PtclBillUrl = ProcessUploadImageptcl(ptclbillupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }

                    if (applicationupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.ApplicationUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/Applications", data.ApplicationUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.ApplicationUrl = ProcessUploadImageapp(applicationupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }

                    if (OtherDocumentupdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.OtherDocument1Url))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/OtherDocuments", data.OtherDocument1Url);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.OtherDocument1Url = ProcessUploadOtherDocument(OtherDocumentupdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (OtherDocument2update != null)
                    {
                        if (!String.IsNullOrEmpty(data.OtherDocument2Url))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/OtherDocuments", data.OtherDocument2Url);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.OtherDocument2Url = ProcessUploadOtherDocument(OtherDocument2update, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (VideoUpdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.VideoUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/Videos", data.VideoUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.VideoUrl = ProcessUploadVideo(VideoUpdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    if (AudioUpdate != null)
                    {
                        if (!String.IsNullOrEmpty(data.AudioUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "Images/Audios", data.AudioUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        datamodel.AudioUrl = ProcessUploadAudio(AudioUpdate, CompanyId, Convert.ToInt32(data.FormNo));
                    }
                    datamodel.OtherDocumentName1 = data.OtherDocumentName1;
                    datamodel.OtherDocumentName2 = data.OtherDocumentName2;
                    datamodel.UpdatedBy = UserId;
                    datamodel.UpdatedOn = DateTime.Now;

                    List<FamilyMember> listfamily = new List<FamilyMember>();
                    foreach (var item in data.FamilyMembers)
                    {
                        var familymember = new FamilyMember();
                        familymember.Name = item.Name;
                        familymember.RelationId = item.RelationId;
                        familymember.Age = item.Age;
                        familymember.FamilyMemberStatusId = item.FamilyMemberStatusId;
                        familymember.GroceryKitId = data.GroceryKitId;
                        familymember.CreatedBy = UserId;
                        familymember.CreatedOn = DateTime.Now;
                        familymember.IsActive = true;
                        familymember.IsDeleted = false;
                        listfamily.Add(familymember);

                    }
                    _context.FamilyMembers.AddRange(listfamily);
                    _context.SaveChanges();

                    returnClass.Message = data.FormNo;
                    returnClass.RoleName = RoleName;
                }
                else
                {
                    return RedirectToAction("Index", "UserDashboard");
                }

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }

       

        [Authorize(Roles = "Admin")]
        public IActionResult AssignTo(List<Assign> assign)
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "UserName");

            List<AssignViewModel> list = new List<AssignViewModel>();
            foreach (var item in assign)
            {
                var model = new AssignViewModel();
                var grocerykit = _context.GroceryKits.Include(x=>x.CurrentStatus).Where(x => x.GroceryKitId == item.GroceryKitId).FirstOrDefault();
                model.GroceryKitId = grocerykit.GroceryKitId;
                model.FirstName = grocerykit.FirstName;
                model.LastName = grocerykit.LastName;
                model.FormNumber = grocerykit.FormNo;
                model.CNIC = grocerykit.CNIC;
                model.Status = grocerykit.CurrentStatus.CurrentStatusName;
                model.FamilySize = grocerykit.FamilySize;
                list.Add(model);
            }
            return PartialView("~/Views/GroceryKit/_AssignGroceryKitPartialView.cshtml", list);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AssignToData(GroceryKitAssignViewModel model)
        {
            try
            {
                var userid = model.UserId;
                List<GroceryKitAssign> assignlist = new List<GroceryKitAssign>();
                foreach (var item in model.Assign)
                {
                    var assign = new GroceryKitAssign();
                    assign.GroceryKitId = item.GroceryKitId;
                    assign.UserId = userid;
                    assign.CreatedBy = UserId;
                    assign.CreatedOn = DateTime.Now;
                    assign.IsActive = true;
                    assign.IsDeleted = false;
                    assignlist.Add(assign);
                }
                _context.GroceryKitAssigns.AddRange(assignlist);
                _context.SaveChanges();

               

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var grocerykit = _context.GroceryKits.Where(x => x.GroceryKitId == Id).FirstOrDefault();
                _context.GroceryKits.Remove(grocerykit);
                _context.SaveChanges();


                returnClass.Message = grocerykit.FormNo;

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult Import()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ImportExcelFile(IFormCollection formCollection)
        {
            if (Request != null)
            {
                var file = Request.Form.Files[0];
                if ((file != null) && !string.IsNullOrEmpty(file.FileName))
                {
                    var data = file.OpenReadStream();
                    var grocerykits = new List<GroceryKit>();
                    var familymemberlist = new List<FamilyMemberExcelViewModel>();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(data))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        ExcelWorksheet worksheet1 = package.Workbook.Worksheets[1];
                        var rowCount = worksheet.Dimension.Rows;
                        var rowfamilies = worksheet1.Dimension.Rows;
                        for (int rowfamily = 2; rowfamily <= rowfamilies; rowfamily++)
                        {
                            var fm = new FamilyMemberExcelViewModel();
                            fm.OldFormNo = worksheet1.Cells[rowfamily, 1].Text;
                            fm.Name = worksheet1.Cells[rowfamily, 2].Text;
                            fm.Age =Convert.ToInt32(worksheet1.Cells[rowfamily, 3].Text);
                            fm.RelationId =Convert.ToInt32(worksheet1.Cells[rowfamily, 4].Text);
                            fm.StatusId =Convert.ToInt32(worksheet1.Cells[rowfamily, 5].Text);
                            familymemberlist.Add(fm);
                        }
                        for (int rowIterator = 2; rowIterator <= rowCount; rowIterator++)
                        {
                            var number = _context.GroceryKits.ToList().LastOrDefault();
                            string formNo = "0";
                            int no = 0;
                            if (number != null)
                            {
                                int num = Convert.ToInt32(number.FormNo) + 1;
                                formNo = "Form No #" + num;
                                no = num;
                            }
                            else
                            {
                                formNo = "Form No#1";
                                no = 1;
                            }

                            var kit = new GroceryKit();
                            kit.OldFormNo = worksheet.Cells[rowIterator, 1].Text;
                            kit.FirstName = worksheet.Cells[rowIterator, 2].Text;
                            kit.LastName = worksheet.Cells[rowIterator, 3].Text;
                            kit.FatherOrHusbandName = worksheet.Cells[rowIterator, 4].Text;
                            kit.Address = worksheet.Cells[rowIterator, 5].Text;
                            kit.CityId = Convert.ToInt32(worksheet.Cells[rowIterator, 6].Text);
                            kit.ProvinceId = Convert.ToInt32(worksheet.Cells[rowIterator, 7].Text);
                            kit.MedicalProbId = Convert.ToInt32(worksheet.Cells[rowIterator, 8].Text);
                            kit.OccupationId = Convert.ToInt32(worksheet.Cells[rowIterator, 9].Text);
                            kit.PhoneNumber1 = worksheet.Cells[rowIterator, 10].Text;
                            kit.PhoneNumber2 = worksheet.Cells[rowIterator, 11].Text;
                            kit.MeritalStatus = worksheet.Cells[rowIterator, 12].Text;
                            kit.CurrentStatusId = Convert.ToInt32(worksheet.Cells[rowIterator, 13].Text);
                            kit.Gender = worksheet.Cells[rowIterator, 14].Text;
                            kit.ResidenceStatus = worksheet.Cells[rowIterator, 15].Text;
                            kit.DateOfBirth = Convert.ToDateTime( worksheet.Cells[rowIterator, 16].Text);                           
                            kit.FoodCost = Convert.ToDecimal(worksheet.Cells[rowIterator, 17].Text);
                            kit.HouseRent = Convert.ToDecimal(worksheet.Cells[rowIterator, 18].Text);
                            kit.SchoolCost = Convert.ToDecimal(worksheet.Cells[rowIterator, 19].Text);
                            kit.UtilitiesCost = Convert.ToDecimal(worksheet.Cells[rowIterator, 20].Text);
                            kit.MedicalCost = Convert.ToDecimal(worksheet.Cells[rowIterator, 21].Text);
                            kit.OtherCost = Convert.ToDecimal(worksheet.Cells[rowIterator, 22].Text);
                            kit.TotalCost = Convert.ToDecimal(worksheet.Cells[rowIterator, 23].Text);
                            kit.ZakatAcceptable = worksheet.Cells[rowIterator, 24].Text;
                            kit.FamilySize = Convert.ToInt32(worksheet.Cells[rowIterator, 25].Text);
                            kit.Salary = Convert.ToDecimal(worksheet.Cells[rowIterator, 26].Text);
                            kit.Donations = Convert.ToDecimal(worksheet.Cells[rowIterator, 27].Text);
                            kit.OtherIncome = Convert.ToDecimal(worksheet.Cells[rowIterator, 28].Text);
                            kit.TotalIncome = Convert.ToDecimal(worksheet.Cells[rowIterator, 29].Text);
                            kit.ShortFallInCash = Convert.ToDecimal(worksheet.Cells[rowIterator, 30].Text);
                            kit.Remarks = worksheet.Cells[rowIterator, 31].Text;
                            kit.CNIC = worksheet.Cells[rowIterator, 32].Text;
                            kit.FormNo = Convert.ToString(no);
                            kit.ApplicationDate = DateTime.Now;
                            kit.Age = Convert.ToInt32(DateTime.Now.Year - kit.DateOfBirth.Year);
                            kit.DataCollectedById = UserId;
                            kit.AuthorizedById = 0;
                            kit.CreatedBy = UserId;
                            kit.CreatedOn = DateTime.Now;
                            kit.IsActive = true;
                            kit.IsDeleted = false;                          
                           int KITID = _grocerykitService.AddGrocerykit(ref kit);
                          
                            foreach (var item in familymemberlist.Where(x=>x.OldFormNo == kit.OldFormNo).ToList())
                            {
                                if(item != null)
                                {
                                    var member = new FamilyMember();
                                    member.Name = item.Name;
                                    member.Age = item.Age;
                                    member.RelationId = item.RelationId;
                                    member.FamilyMemberStatusId = item.StatusId;
                                    member.CreatedBy = UserId;
                                    member.CreatedOn = DateTime.Now;
                                    member.IsActive = true;
                                    member.IsDeleted = false;
                                    member.GroceryKitId = KITID;
                                    _context.FamilyMembers.Add(member);
                                    _context.SaveChanges();
                                }
                                
                            }
                            
                        }
                    }                   
                }
            }
            return RedirectToAction("Index");
        }

        private string ProcessUploadAudio(IFormFile audio, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (audio != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Audios");
                UniqueFileName = formNo + "_" + companyId + "_" + audio.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    audio.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadVideo(IFormFile video, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (video != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Videos");
                UniqueFileName = formNo + "_" + companyId + "_" + video.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    video.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }
        private string ProcessUploadOtherDocument(IFormFile otherDocument, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (otherDocument != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/OtherDocuments");
                UniqueFileName = formNo + "_" + companyId + "_" + otherDocument.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    otherDocument.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }
        private string ProcessUploadImageapp(IFormFile application, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (application != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Applications");
                UniqueFileName = formNo + "_" + companyId + "_" + application.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    application.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageptcl(IFormFile ptclbill, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (ptclbill != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/PTCLBILL");
                UniqueFileName = formNo + "_" + companyId + "_" + ptclbill.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    ptclbill.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageelectricity(IFormFile electricitybill, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (electricitybill != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ElectricityBill");
                UniqueFileName = formNo + "_" + companyId + "_" + electricitybill.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    electricitybill.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImagebform(IFormFile bform, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (bform != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/B-FORM");
                UniqueFileName = formNo + "_" + companyId + "_" + bform.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    bform.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImagedeath(IFormFile deathcertificate, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (deathcertificate != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/DeathCertificate");
                UniqueFileName = formNo + "_" + companyId + "_" + deathcertificate.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    deathcertificate.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImagedis(IFormFile disabilitycertificate, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (disabilitycertificate != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/DisabilityCertificate");
                UniqueFileName = formNo + "_" + companyId + "_" + disabilitycertificate.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    disabilitycertificate.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageCnic(IFormFile cnic, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (cnic != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/CNIC");
                UniqueFileName = formNo + "_" + companyId + "_" + cnic.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    cnic.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImage(IFormFile photo, int companyId, int formNo)
        {
            string UniqueFileName = null;
            if (photo != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "Images/MainPhoto");
                UniqueFileName = formNo + "_" + companyId + "_" + photo.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private double getrating(int familySize, decimal shortFallInCash, string residenceStatus, string currentStatusName)
        {
            double rating = 0;
            int sizescore = 0;
            double shortfallscore = 0;
            int housescore = 0;
            int statusscore = 0;
            switch (familySize)
            {
                case 1:
                    sizescore = 6;
                    break;
                case 2:
                    sizescore = 7;
                    break;
                case 3:
                    sizescore = 8;
                    break;
                case 4:
                    sizescore = 9;
                    break;
                case 5:
                    sizescore = 10;
                    break;
                case 6:
                    sizescore = 10;
                    break;
                case 7:
                    sizescore = 10;
                    break;
                default:
                    sizescore = 10;
                    break;
            }
            if (shortFallInCash < 1000)
            {
                shortfallscore = 0;
            }
            else if (shortFallInCash >= 1000 && shortFallInCash <= 3000)
            {
                shortfallscore = 5.5;
            }
            else if (shortFallInCash > 3000 && shortFallInCash <= 5000)
            {
                shortfallscore = 6;
            }
            else if (shortFallInCash > 5000 && shortFallInCash <= 7000)
            {
                shortfallscore = 6.5;
            }
            else if (shortFallInCash > 7000 && shortFallInCash <= 9000)
            {
                shortfallscore = 7;
            }
            else if (shortFallInCash > 9000 && shortFallInCash <= 11000)
            {
                shortfallscore = 7.5;
            }
            else if (shortFallInCash > 11000 && shortFallInCash <= 13000)
            {
                shortfallscore = 8;
            }
            else if (shortFallInCash > 13000 && shortFallInCash <= 15000)
            {
                shortfallscore = 8.5;
            }
            else if (shortFallInCash > 15000 && shortFallInCash <= 17000)
            {
                shortfallscore = 9;
            }
            else if (shortFallInCash > 17000 && shortFallInCash <= 19000)
            {
                shortfallscore = 9.5;
            }
            else
            {
                shortfallscore = 10;
            }
            switch (residenceStatus)
            {
                case "Own":
                    housescore = 5;
                    break;
                case "Rented":
                    housescore = 8;
                    break;
                case "Others":
                    housescore = 9;
                    break;
                case "Homeless":
                    housescore = 10;
                    break;
                default:
                    break;
            }
            switch (currentStatusName)
            {
                case "Widow":
                    statusscore = 8;
                    break;
                case "Deserving":
                    statusscore = 8;
                    break;
                case "Divorcee":
                    statusscore = 7;
                    break;
                case "Orphans":
                    statusscore = 9;
                    break;
                case "Old Aged":
                    statusscore = 10;
                    break;
                case "Imam Masjid":
                    statusscore = 5;
                    break;
                case "Disabled":
                    statusscore = 8;
                    break;
                default:
                    break;
            }

            rating = (sizescore + shortfallscore + housescore + statusscore) / 4;
            return rating;
        }
    }
}