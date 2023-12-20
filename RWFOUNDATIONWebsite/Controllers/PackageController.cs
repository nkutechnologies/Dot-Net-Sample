using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Packages;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels.PackageViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PackageController : PrivateController
    {
        private readonly PackageService _packageService;
        private readonly RwDbContext _context;
        private readonly JsonReturn returnClass;
        private readonly ItemService _itemService;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        private int CompanyId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId")); } }
        public PackageController(PackageService packageService, RwDbContext context, JsonReturn _jsonReturn, ItemService itemService)
        {
            _packageService = packageService;
            _context = context;
            this.returnClass = _jsonReturn;
            _itemService = itemService;
        }
        public IActionResult Index()
        {
            var model = _packageService.GetAll().Where(x=>x.CreatedBy == UserId).Select(x => new PackageViewModel
            {
                PackageId = x.PackageId,
                PackageName = x.PackageName
            }).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult CreatePackage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePackage(PackageCRUPDlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var package = new Package();
                package.PackageName = model.PackageName;
                package.CreatedBy = UserId;
                package.CreatedOn = DateTime.Now;
                package.IsActive = true;
                package.IsDeleted = false;
                _packageService.Add(ref package);
                return RedirectToAction("Index", "Package");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult UpdatePackage(int Id)
        {           
            var data = _packageService.Get(Id);
            var model = new PackageCRUPDlViewModel
            {
                PackageId = data.PackageId,
                PackageName = data.PackageName               
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdatePackage(PackageCRUPDlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var package = new Package();
                package.PackageId = model.PackageId;
                package.PackageName = model.PackageName;               
                package.UpdatedBy = UserId;
                package.UpdatedOn = DateTime.Now;
                _packageService.Update(package);
                return RedirectToAction("Index", "Package");

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult DeletePackage(int Id)
        {
            var data = _packageService.Get(Id);
            var model = new PackageCRUPDlViewModel
            {
                PackageId = data.PackageId,
                PackageName = data.PackageName
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult DeletePackage(PackageCRUPDlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var package = new Package();
                package.PackageId = model.PackageId;
                _packageService.Delete(package);
                return RedirectToAction("Index", "Package");
            }
            return View(model);
        }
        public IActionResult PackageItemsList()
        {
            var model = _packageService.GetAllPackageItems();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddPackageItem()
        {
            ViewData["packages"] = new SelectList(_context.Packages.Where(x=>x.IsActive == true && x.IsDeleted == false), "PackageId", "PackageName");
            ViewData["items"] = new SelectList(_context.Items.Where(x => x.IsActive == true && x.IsDeleted == false), "ItemId", "ItemName");
            return View();
        }
        [HttpPost]
        public IActionResult AddPackageItem(PackageDetailViewModel model)
        {
            try
            {
                var packagedetail = new PackageDetail();
                packagedetail.PackageDetailName = model.PackageDetailName;
                packagedetail.TotalFamilyMember = model.TotalFamilyMember;
                packagedetail.PackageId = model.PackageId;
                packagedetail.PackageValuePKR = model.PackageValuePKR;
                packagedetail.PackageValueSR = model.PackageValueSR;
                packagedetail.PackageValueUSD = model.PackageValueUSD;
                packagedetail.CreatedBy = UserId;
                packagedetail.CreatedOn = DateTime.Now;
                packagedetail.IsActive = true;
                packagedetail.IsDeleted = false;

                int id = _packageService.AddPackageDetail(ref packagedetail);

                foreach (var item in model.PackageItems)
                {
                    var packageItem = new PackageItem();
                    packageItem.PackageDetailId = id;
                    packageItem.PackageQuantity = item.PackageQuantity;
                    packageItem.ItemId = item.ItemId;
                    packageItem.CreatedBy = UserId;
                    packageItem.CreatedOn = DateTime.Now;
                    packageItem.IsActive = true;
                    packageItem.IsDeleted = false;
                    _context.PackageItems.Add(packageItem);
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

        public IActionResult getPrice(int ItemId)
        {
            try
            {
                var item = _itemService.Get(ItemId);
                returnClass.Message = Convert.ToString(Convert.ToInt32(item.Price));
            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
        public IActionResult DetailPackageItem(int Id)
        {
            var model = _packageService.GetPackageItem(Id);
            return View(model);
        }
        [HttpGet]
        public IActionResult UpdatePackageItem(int Id)
        {
            ViewData["packages"] = new SelectList(_context.Packages.Where(x => x.IsActive == true && x.IsDeleted == false), "PackageId", "PackageName");
            ViewData["items"] = new SelectList(_context.Items.Where(x => x.IsActive == true && x.IsDeleted == false), "ItemId", "ItemName");
            var model = _packageService.GetPackageItem(Id);
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdatePackageItem(PackageDetailViewModel model)
        {
            try
            {
                var details = _context.PackageItems.Where(c => c.PackageDetailId == model.PackageDetailId).ToList();
                _context.RemoveRange(details);
                var entity = _packageService.GetPackageItem(model.PackageDetailId);
                entity.PackageDetailId = model.PackageDetailId;
                entity.PackageDetailName = model.PackageDetailName;
                entity.TotalFamilyMember = model.TotalFamilyMember;
                entity.PackageId = model.PackageId;
                entity.PackageValuePKR = model.PackageValuePKR;
                entity.PackageValueSR = model.PackageValueSR;
                entity.PackageValueUSD = model.PackageValueUSD;
                entity.UpdatedBy = UserId;
                entity.UpdatedOn = DateTime.Now;


                List<PackageItem> list = new List<PackageItem>();
                foreach (var item in model.PackageItems)
                {
                    var packageItem = new PackageItem();
                    packageItem.PackageDetailId = model.PackageDetailId;
                    packageItem.PackageQuantity = item.PackageQuantity;
                    packageItem.ItemId = item.ItemId;
                    packageItem.CreatedBy = UserId;
                    packageItem.CreatedOn = DateTime.Now;
                    packageItem.IsActive = true;
                    packageItem.IsDeleted = false;
                    list.Add(packageItem);
                    
                }
                _context.PackageItems.AddRange(list);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
        [HttpPost]
        public IActionResult DeletePackageItem(int Id)
        {
            try
            {
                var package = _context.PackageDetails.Where(x => x.PackageDetailId == Id).FirstOrDefault();
                _context.PackageDetails.Remove(package);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                returnClass.IsError = true;
                returnClass.Message = ex.Message;
            }
            return Json(returnClass);
        }
        public IActionResult getRates()
        {
            try
            {
                //string URL = "https://api.exchangeratesapi.io/latest";
                string URL = "https://api.ratesapi.io/api/latest";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(URL));
                request.UserAgent = ".NET Framework Test Client";
                request.Accept = "text/xml-standard-api";
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = 600000;

                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Encoding enc = Encoding.GetEncoding(12000);



                StreamReader loResponseStream = new StreamReader(response.GetResponseStream());
                string result = loResponseStream.ReadToEnd();
                response.Close();
                loResponseStream.Close();

                var responcedata = JsonConvert.DeserializeObject<CurrencyConversionViewModel>(result);
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