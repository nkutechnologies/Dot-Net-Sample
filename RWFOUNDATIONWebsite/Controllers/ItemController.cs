using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.Packages;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels.ItemViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemController : PrivateController
    {
        private readonly ItemService _itemService;
        private RwDbContext _context;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        public ItemController(ItemService itemService, RwDbContext context)
        {
            _itemService = itemService;
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _itemService.GetAll().Select(x => new ItemViewModel
            {
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                ItemNameUrdu = x.ItemNameUrdu,
                Price = x.Price,
                Unit = x.Unit.Name
            }).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddItem()
        {
            ViewData["units"] = new SelectList(_context.Units, "UnitId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult AddItem(ItemCreateOrUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new Item();
                item.ItemName = model.ItemName;
                item.ItemNameUrdu = model.ItemNameUrdu;
                item.Price = model.Price;
                item.UnitId = model.UnitId;
                item.CreatedBy = UserId;
                item.CreatedOn = DateTime.Now;
                item.IsActive = true;
                item.IsDeleted = false;

                _itemService.Add(ref item);
                return RedirectToAction("Index", "Item");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult UpdateItem(int Id)
        {
            ViewData["units"] = new SelectList(_context.Units, "UnitId", "Name");
            var data = _itemService.Get(Id);
            var model = new ItemCreateOrUpdateViewModel
            {
                ItemId = data.ItemId,
                ItemName = data.ItemName,
                ItemNameUrdu = data.ItemNameUrdu,
                Price = data.Price,
                UnitId = data.UnitId
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateItem(ItemCreateOrUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new Item();
                item.ItemId = model.ItemId;
                item.ItemName = model.ItemName;
                item.ItemNameUrdu = model.ItemNameUrdu;
                item.Price = model.Price;
                item.UnitId = model.UnitId;
                item.UpdatedBy = UserId;
                item.UpdatedOn = DateTime.Now;
                _itemService.Update(item);
                return RedirectToAction("Index", "Item");

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult DeleteItem(int Id)
        {
            var data = _itemService.Get(Id);
            var model = new ItemDeleteViewModel
            {
                ItemId = data.ItemId,
                ItemName = data.ItemName,
                ItemNameUrdu = data.ItemNameUrdu,
                Price = data.Price                
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteItem(ItemDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new Item();
                item.ItemId = model.ItemId;
                _itemService.Delete(item);
                return RedirectToAction("Index", "Item");
            }
            return View(model);
        }
    }
}