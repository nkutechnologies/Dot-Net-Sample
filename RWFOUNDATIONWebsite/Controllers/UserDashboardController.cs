using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    
    public class UserDashboardController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly GrocerykitService _grocerykitService;
        private readonly JsonReturn returnClass;
        private readonly RwDbContext _context;
        private int UserId { get { return Convert.ToInt32(HttpContext.Session.GetInt32("UserID")); } }
        private string RoleName { get { return HttpContext.Session.GetString("RoleName"); } }
        public UserDashboardController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager, GrocerykitService grocerykitService, JsonReturn _jsonReturn, RwDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _grocerykitService = grocerykitService;
            this.returnClass = _jsonReturn;
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
            int CompanyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));

            if (UserId > 0 && CompanyId > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
           
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UserList()
        {
            var model = _context.Users.Select(x => new ApplicationUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                DateOfBirth = x.DateOfBirth,
                ToDonate = x.ToDonate
            }).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = new ApplicationRole { Name = model.Name };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "UserDashboard");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {id}  not found";
                return RedirectToAction("ListRoles", "UserDashboard");
            }
            var model = new UpdateRoleViewModel
            {
                Id = role.Id,
                Name = role.Name

            };
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {model.Id}  not found";
                return RedirectToAction("ListRoles", "UserDashboard");
            }
            else
            {
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "UserDashboard");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditeUserInRole(int RoleId)
        {
            ViewBag.RoleId = RoleId;
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {RoleId.ToString()}  not found";
                return RedirectToAction("ListRoles", "Administration");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditeUserInRole(List<UserRoleViewModel> model, string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {RoleId}  not found";
                return RedirectToAction("UpdateRole", "Administration");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId.ToString());
                IdentityResult result = null;
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("UpdateRole", new { Id = RoleId });
                }

            }
            return RedirectToAction("UpdateRole", new { Id = RoleId });

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        [Authorize(Roles = "Beneficiary")]
        [HttpGet]
        public IActionResult ProfileBeneficiary(int id)
        {
            var detail = _grocerykitService.GetBeneficiary(id);              

            return View(detail);
        }      

        [Authorize(Roles ="Beneficiary")]
        [HttpGet]
        public IActionResult SuccessforBeneficiaryAdded()
        {
            return View();
        }
        [Authorize(Roles = "Beneficiary")]
        [HttpGet]
        public IActionResult UpdateSuccessforBeneficiary()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateExternalLoginUser(UpdateExternalUserViewModel model)
        {           
            try
            {
                var user = _context.AspNetUsers.Where(x => x.Id == UserId).FirstOrDefault();
                if (model.As == "Beneficiary")
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.DateOfBirth = model.DateOfBirth;
                    user.Address = model.Address;
                    await _userManager.AddToRoleAsync(user, "Beneficiary");
                    await _context.SaveChangesAsync();
                    HttpContext.Session.Clear();
                    await _signInManager.SignOutAsync();
                }
                else
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.DateOfBirth = model.DateOfBirth;
                    user.Address = model.Address;
                    user.ToDonate = model.ToDonate;
                    user.Estimate = model.Estimate;
                    await _userManager.AddToRoleAsync(user, "Donor");
                    await _context.SaveChangesAsync();
                    HttpContext.Session.Clear();
                    await _signInManager.SignOutAsync();
                }

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