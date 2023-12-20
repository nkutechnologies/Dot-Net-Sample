using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.SaveAsDrafts;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    public class BeneficiarySaveAsDraftController : PrivateController
    {
        private readonly JsonReturn returnClass;
        private readonly RwDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly BeneficiarySaveAsDraftService _beneficiaryService;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        private int CompanyId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId")); } }
        public BeneficiarySaveAsDraftController(JsonReturn _jsonReturn, RwDbContext context, IWebHostEnvironment hostingEnvironment,
            BeneficiarySaveAsDraftService beneficiaryService)
        {
            this.hostingEnvironment = hostingEnvironment;
            _beneficiaryService = beneficiaryService;
            this.returnClass = _jsonReturn;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Beneficiary")]
        [HttpPost]
        public IActionResult SaveAsDraft(IFormFile photo, IFormFile cnicfront, IFormFile cnicback,
            IFormFile disabilitycertificate, IFormFile deathcertificate, IFormFile bform, IFormFile electricitybill,
            IFormFile ptclbill, IFormFile application, IFormFile OtherDocument, IFormFile OtherDocument2, IFormFile video, IFormFile audio, string model)
        {

            try
            {
                var Checksavedata = _beneficiaryService.Get(UserId);
                if(Checksavedata == null)
                {
                    var data = JsonConvert.DeserializeObject<GroceryKitCreateViewModel>(model);
                    var datamodel = new BeneficiaryFormSaveAsDraft();
                    datamodel.FormNo = null;
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
                    datamodel.ImageUrl = ProcessUploadImage(photo, CompanyId);
                    datamodel.CNICFrontUrl = ProcessUploadImageCnic(cnicfront, CompanyId);
                    datamodel.CNICBackUrl = ProcessUploadImageCnic(cnicback, CompanyId);
                    datamodel.DisabilityCertificateUrl = ProcessUploadImagedis(disabilitycertificate, CompanyId);
                    datamodel.DeathCertificateUrl = ProcessUploadImagedeath(deathcertificate, CompanyId);
                    datamodel.BFormUrl = ProcessUploadImagebform(bform, CompanyId);
                    datamodel.ElectricityBill = ProcessUploadImageelectricity(electricitybill, CompanyId);
                    datamodel.PtclBillUrl = ProcessUploadImageptcl(ptclbill, CompanyId);
                    datamodel.ApplicationUrl = ProcessUploadImageapp(application, CompanyId);
                    datamodel.OtherDocument1Url = ProcessUploadOtherDocument(OtherDocument, CompanyId);
                    datamodel.OtherDocument2Url = ProcessUploadOtherDocument(OtherDocument2, CompanyId);
                    datamodel.OtherDocumentName1 = data.OtherDocumentName1;
                    datamodel.OtherDocumentName2 = data.OtherDocumentName2;
                    datamodel.VideoUrl = ProcessUploadVideo(video, CompanyId);
                    datamodel.AudioUrl = ProcessUploadAudio(audio, CompanyId);


                    datamodel.CreatedBy = UserId;
                    datamodel.CreatedOn = DateTime.Now;

                    int grocerykitId = _beneficiaryService.SaveDraft(ref datamodel);

                    foreach (var item in data.FamilyMembers)
                    {
                        var familymember = new BeneficiaryFamilyMemberSaveAsDraft();
                        familymember.Name = item.Name;
                        familymember.RelationId = item.RelationId;
                        familymember.Age = item.Age;
                        familymember.FamilyMemberStatusId = item.FamilyMemberStatusId;
                        familymember.GroceryKitId = grocerykitId;

                        _context.BeneficiaryFamilyMemberSaveAsDrafts.Add(familymember);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<GroceryKitCreateViewModel>(model);                    
                    var details = _context.BeneficiaryFamilyMemberSaveAsDrafts.Where(c => c.GroceryKitId == data.GroceryKitId).ToList();
                    _context.RemoveRange(details);

                    Checksavedata.GroceryKitId = data.GroceryKitId;
                    Checksavedata.FormNo = data.FormNo;
                    Checksavedata.FirstName = data.FirstName;
                    Checksavedata.LastName = data.LastName;
                    Checksavedata.FatherOrHusbandName = data.FatherOrHusbandName;
                    Checksavedata.Address = data.Address;
                    Checksavedata.CNIC = data.CNIC;
                    Checksavedata.OldFormNo = data.OldFormNo;
                    Checksavedata.MedicalProbId = data.MedicalProbId;
                    Checksavedata.PhoneNumber1 = data.PhoneNumber1;
                    Checksavedata.PhoneNumber2 = data.PhoneNumber2;
                    Checksavedata.OccupationId = data.OccupationId;
                    Checksavedata.CityId = data.CityId;
                    Checksavedata.ProvinceId = data.ProvinceId;
                    Checksavedata.CurrentStatusId = data.CurrentStatusId;
                    Checksavedata.ApplicationDate = DateTime.Now;
                    Checksavedata.Gender = data.Gender;
                    Checksavedata.ResidenceStatus = data.ResidenceStatus;
                    Checksavedata.MeritalStatus = data.MeritalStatus;
                    Checksavedata.DateOfBirth = data.DateOfBirth;
                    Checksavedata.Age = Convert.ToInt32(DateTime.Now.Year - data.DateOfBirth.Year);
                    Checksavedata.FoodCost = data.FoodCost;
                    Checksavedata.HouseRent = data.HouseRent;
                    Checksavedata.SchoolCost = data.SchoolCost;
                    Checksavedata.UtilitiesCost = data.UtilitiesCost;
                    Checksavedata.MedicalCost = data.MedicalCost;
                    Checksavedata.OtherCost = data.OtherCost;
                    Checksavedata.TotalCost = data.TotalCost;
                    Checksavedata.ZakatAcceptable = data.ZakatAcceptable;
                    Checksavedata.FamilySize = data.FamilySize;
                    Checksavedata.Salary = data.Salary;
                    Checksavedata.Donations = data.Donations;
                    Checksavedata.OtherIncome = data.OtherIncome;
                    Checksavedata.TotalIncome = data.TotalIncome;
                    Checksavedata.ShortFallInCash = data.ShortFallInCash;
                    Checksavedata.Remarks = data.Remarks;

                    if (photo != null)
                    {
                        if (!String.IsNullOrEmpty(data.ExistingPhotoUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_MainPhoto", data.ExistingPhotoUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.ImageUrl = ProcessUploadImage(photo, CompanyId);
                    }
                    if (cnicfront != null)
                    {
                        if (!String.IsNullOrEmpty(data.CNICFrontUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_CNIC", data.CNICFrontUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.CNICFrontUrl = ProcessUploadImageCnic(cnicfront, CompanyId);
                    }
                    if (cnicback != null)
                    {
                        if (!String.IsNullOrEmpty(data.CNICBackUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_CNIC", data.CNICBackUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.CNICBackUrl = ProcessUploadImageCnic(cnicback, CompanyId);
                    }
                    if (disabilitycertificate != null)
                    {
                        if (!String.IsNullOrEmpty(data.DisabilityCertificateUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_DisabilityCertificate", data.DisabilityCertificateUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.DisabilityCertificateUrl = ProcessUploadImagedis(disabilitycertificate, CompanyId);
                    }
                    if (deathcertificate != null)
                    {
                        if (!String.IsNullOrEmpty(data.DeathCertificateUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_DeathCertificate", data.DeathCertificateUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.DeathCertificateUrl = ProcessUploadImagedeath(deathcertificate, CompanyId);
                    }

                    if (bform != null)
                    {
                        if (!String.IsNullOrEmpty(data.BFormUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_B-FORM", data.BFormUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.BFormUrl = ProcessUploadImagebform(bform, CompanyId);
                    }

                    if (electricitybill != null)
                    {
                        if (!String.IsNullOrEmpty(data.ElectricityBill))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_ElectricityBill", data.ElectricityBill);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.ElectricityBill = ProcessUploadImageelectricity(electricitybill, CompanyId);
                    }

                    if (ptclbill != null)
                    {
                        if (!String.IsNullOrEmpty(data.PtclBillUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_PTCLBILL", data.PtclBillUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.PtclBillUrl = ProcessUploadImageptcl(ptclbill, CompanyId);
                    }

                    if (application != null)
                    {
                        if (!String.IsNullOrEmpty(data.ApplicationUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Applications", data.ApplicationUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.ApplicationUrl = ProcessUploadImageapp(application, CompanyId);
                    }

                    if (OtherDocument != null)
                    {
                        if (!String.IsNullOrEmpty(data.OtherDocument1Url))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_OtherDocuments", data.OtherDocument1Url);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.OtherDocument1Url = ProcessUploadOtherDocument(OtherDocument, CompanyId);
                    }
                    if (OtherDocument2 != null)
                    {
                        if (!String.IsNullOrEmpty(data.OtherDocument2Url))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_OtherDocuments", data.OtherDocument2Url);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.OtherDocument2Url = ProcessUploadOtherDocument(OtherDocument2, CompanyId);
                    }
                    if (video != null)
                    {
                        if (!String.IsNullOrEmpty(data.VideoUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Videos", data.VideoUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.VideoUrl = ProcessUploadVideo(video, CompanyId);
                    }
                    if (audio != null)
                    {
                        if (!String.IsNullOrEmpty(data.AudioUrl))
                        {
                            string fileUrl = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Audios", data.AudioUrl);
                            System.IO.File.Delete(fileUrl);
                        }
                        Checksavedata.AudioUrl = ProcessUploadAudio(audio, CompanyId);
                    }
                    Checksavedata.OtherDocumentName1 = data.OtherDocumentName1;
                    Checksavedata.OtherDocumentName2 = data.OtherDocumentName2;                  

                    List<BeneficiaryFamilyMemberSaveAsDraft> listfamily = new List<BeneficiaryFamilyMemberSaveAsDraft>();
                    foreach (var item in data.FamilyMembers)
                    {
                        var familymember = new BeneficiaryFamilyMemberSaveAsDraft();
                        familymember.Name = item.Name;
                        familymember.RelationId = item.RelationId;
                        familymember.Age = item.Age;
                        familymember.FamilyMemberStatusId = item.FamilyMemberStatusId;
                        familymember.GroceryKitId = data.GroceryKitId;                        
                        listfamily.Add(familymember);

                    }
                    _context.BeneficiaryFamilyMemberSaveAsDrafts.AddRange(listfamily);
                    _context.SaveChanges();
                }
               
               
            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }

        private string ProcessUploadAudio(IFormFile audio, int companyId)
        {
            string UniqueFileName = null;
            if (audio != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Audios");
                UniqueFileName = "_" + companyId + "_" + audio.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    audio.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadVideo(IFormFile video, int companyId)
        {
            string UniqueFileName = null;
            if (video != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Videos");
                UniqueFileName =  "_" + companyId + "_" + video.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    video.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadOtherDocument(IFormFile otherDocument, int companyId)
        {
            string UniqueFileName = null;
            if (otherDocument != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_OtherDocuments");
                UniqueFileName =  "_" + companyId + "_" + otherDocument.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    otherDocument.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageapp(IFormFile application, int companyId)
        {
            string UniqueFileName = null;
            if (application != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_Applications");
                UniqueFileName = "_" + companyId + "_" + application.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    application.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageptcl(IFormFile ptclbill, int companyId)
        {
            string UniqueFileName = null;
            if (ptclbill != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_PTCLBILL");
                UniqueFileName =  "_" + companyId + "_" + ptclbill.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    ptclbill.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageelectricity(IFormFile electricitybill, int companyId)
        {
            string UniqueFileName = null;
            if (electricitybill != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_ElectricityBill");
                UniqueFileName = "_" + companyId + "_" + electricitybill.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    electricitybill.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImagebform(IFormFile bform, int companyId)
        {
            string UniqueFileName = null;
            if (bform != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_B-FORM");
                UniqueFileName = "_" + companyId + "_" + bform.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    bform.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImagedeath(IFormFile deathcertificate, int companyId)
        {
            string UniqueFileName = null;
            if (deathcertificate != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_DeathCertificate");
                UniqueFileName =  "_" + companyId + "_" + deathcertificate.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    deathcertificate.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImagedis(IFormFile disabilitycertificate, int companyId)
        {
            string UniqueFileName = null;
            if (disabilitycertificate != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_DisabilityCertificate");
                UniqueFileName =  "_" + companyId + "_" + disabilitycertificate.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    disabilitycertificate.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImageCnic(IFormFile cnic, int companyId)
        {
            string UniqueFileName = null;
            if (cnic != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_CNIC");
                UniqueFileName = "_" + companyId + "_" + cnic.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    cnic.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }

        private string ProcessUploadImage(IFormFile photo, int companyId)
        {
            string UniqueFileName = null;
            if (photo != null)
            {
                string Folder = Path.Combine(hostingEnvironment.WebRootPath, "DraftImages/Draft_MainPhoto");
                UniqueFileName = "_" + companyId + "_" + photo.FileName;
                string filePath = Path.Combine(Folder, UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(filestream);
                }

            }
            return UniqueFileName;
        }
    }
}