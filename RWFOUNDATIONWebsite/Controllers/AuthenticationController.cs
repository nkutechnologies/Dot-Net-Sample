using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using NETCore.MailKit.Core;
using RWFOUNDATIONWebsite.Data;
using RWFOUNDATIONWebsite.DataModels.UserManagement;
using RWFOUNDATIONWebsite.Helper;
using RWFOUNDATIONWebsite.Services;
using RWFOUNDATIONWebsite.ViewModels;

namespace RWFOUNDATIONWebsite.Controllers
{
    //[Route("[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly RwDbContext _context;
        private readonly RoleService _roleService;
        private readonly IEmailService _emailSerivce;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly JsonReturn returnClass;
        public AuthenticationController(IConfiguration config, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<AuthenticationController> logger, RwDbContext context,
            RoleService roleService, IEmailService emailService, IWebHostEnvironment hostingEnvironment, JsonReturn _jsonReturn)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _roleService = roleService;
            _emailSerivce = emailService;
            _hostingEnvironment = hostingEnvironment;
            this.returnClass = _jsonReturn;
        }
        //[Route("Login")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
          
            return View(model);
        }
        //[Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if(user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
                    {
                        ModelState.AddModelError(string.Empty, "Your Email is not confirmed yet");
                        return View(model);
                    }
                    if (result.Succeeded)
                    {
                        var User = _context.Users.FirstOrDefault(c => c.Email == model.Email);                        
                        string roleName = _roleService.GetUserRole(User.Id);
                        if (User != null)
                        {                            
                            HttpContext.Session.SetString("UserName", User.UserName);
                            HttpContext.Session.SetString("Email", User.Email);
                            HttpContext.Session.SetInt32("UserID", User.Id);
                            HttpContext.Session.SetString("RoleName", roleName);
                            HttpContext.Session.SetInt32("CompanyId", user.CompanyId);

                        }

                        _logger.LogInformation("User logged in.");
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "UserDashboard");
                        }
                        
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");                    

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return View(model);

        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult checkdate(DateTime dateOfBirth)
        {
            DateTime date = DateTime.Now;
            DateTime date1 = dateOfBirth;
            int year1 = date.Year;
            int year2 = date1.Year;
            int year = year1 - year2;
            if (year < 18)
            {
                return Json($"Your Age Should be Minimum 18 Years");
            }
            else
            {
                return Json(true);
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(UserDashboardController.Index), "UserDashboard");
        }
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult SignUp()
        {           
            return View();
        }
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.As == "Beneficiary")
                {
                    var User = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateOfBirth = model.DateOfBirth,
                        Address = model.Address,
                        CompanyId = 1,
                        ToDonate = null,
                        Estimate = null,
                    };
                    var resultbeneficiary = await _userManager.CreateAsync(User, model.Password);
                    if (resultbeneficiary.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                        var confirmationlink = Url.Action("ConfirmEmail", "Authentication", new { userId = User.Id, token = token }, Request.Scheme, Request.Host.ToString());
                        
                        await _userManager.AddToRoleAsync(User, "Beneficiary");
                        EmailBodyMaker(User.UserName, User.Email, model.Password, confirmationlink, "Beneficiary");
                        ViewData["success"] = "Registration Successful";
                        ViewData["Message"] = "Before you can Login , please confirm your email, by clicking on confirmation link we have emailed you. ";
                        return View("RegistrationSuccess");
                    }
                    foreach (var error in resultbeneficiary.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    var Usercreate = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateOfBirth = model.DateOfBirth,
                        Address = model.Address,
                        ToDonate = model.ToDonate,
                        Estimate = model.Estimate,
                        CompanyId = 1
                    };
                    var result = await _userManager.CreateAsync(Usercreate, model.Password);
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(Usercreate);
                        var confirmationlink = Url.Action("ConfirmEmail", "Authentication", new { userId = Usercreate.Id, token = token }, Request.Scheme, Request.Host.ToString());
                        await _userManager.AddToRoleAsync(Usercreate, "Donor");
                        EmailBodyMaker(Usercreate.UserName, Usercreate.Email, model.Password, confirmationlink, "Donor");
                        ViewData["success"] = "Registration Successful";
                        ViewData["message"] = "Before you can Login , please confirm your email, by clicking on confirmation link we have emailed you. ";
                        return View("RegistrationSuccess");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }             
                return View(model);
            }
            return BadRequest();
        }
        [Route("Donor")]
        [AllowAnonymous]
        public IActionResult DonorRegister()
        {
            return View();
        }
        [Route("Donor")]
        [HttpPost]
        public async Task<IActionResult> DonorRegister(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.As == "Donor")
                {
                    var Usercreate = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateOfBirth = model.DateOfBirth,
                        Address = model.Address,
                        ToDonate = model.ToDonate,
                        Estimate = model.Estimate,
                        CompanyId = 1
                    };
                    var result = await _userManager.CreateAsync(Usercreate, model.Password);
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(Usercreate);
                        var confirmationlink = Url.Action("ConfirmEmail", "Authentication", new { userId = Usercreate.Id, token = token }, Request.Scheme, Request.Host.ToString());

                        await _userManager.AddToRoleAsync(Usercreate, "Donor");                                              
                        EmailBodyMaker(Usercreate.UserName, Usercreate.Email, model.Password, confirmationlink, "Donor");
                        ViewData["success"] = "Registration Successful";
                        ViewData["message"] = "Before you can Login , please confirm your email, by clicking on confirmation link we have emailed you. ";
                        return View("RegistrationSuccess");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    var User = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateOfBirth = model.DateOfBirth,
                        Address = model.Address,
                        CompanyId = 1,
                        ToDonate = null,
                        Estimate = null,
                    };
                    var resultbeneficiary = await _userManager.CreateAsync(User, model.Password);
                    if (resultbeneficiary.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                        var confirmationlink = Url.Action("ConfirmEmail", "Authentication", new { userId = User.Id, token = token }, Request.Scheme, Request.Host.ToString());

                        await _userManager.AddToRoleAsync(User, "Beneficiary");
                        EmailBodyMaker(User.UserName, User.Email, model.Password, confirmationlink, "Beneficiary");

                        ViewData["success"] = "Registration Successful";
                        ViewData["Message"] = "Before you can Login , please confirm your email, by clicking on confirmation link we have emailed you. ";
                        return View("RegistrationSuccess");
                    }
                    foreach (var error in resultbeneficiary.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }               
                return View(model);
            }
            return BadRequest();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin (string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginviewmodel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if(remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginviewmodel);
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information");
                return View("Login", loginviewmodel);
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;
            if(email != null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if(user !=null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Your Email is not confirmed yet");
                    return View("Login", loginviewmodel);
                }
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor:true);
            if (signInResult.Succeeded)
            {
                var User = _context.Users.FirstOrDefault(c => c.Email == info.Principal.FindFirstValue(ClaimTypes.Email));

                if (User != null)
                {
                    HttpContext.Session.SetString("UserName", User.UserName);
                    HttpContext.Session.SetString("Email", User.Email);
                    HttpContext.Session.SetInt32("UserID", User.Id);
                    HttpContext.Session.SetInt32("CompanyId", User.CompanyId);

                }
                return RedirectToAction("Index", "UserDashboard");
            }
            else
            {
               
                if (email != null)
                {                    
                    if(user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            CompanyId = 1
                        };
                        await _userManager.CreateAsync(user);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationlink = Url.Action("ConfirmEmail", "Authentication", new { userId = user.Id, token = token }, Request.Scheme, Request.Host.ToString());
                        EmailBodyMaker(user.UserName, user.Email, null, confirmationlink, null);
                        ViewData["success"] = "Registration Successful";
                        ViewData["message"] = "Before you can Login , please confirm your email, by clicking on confirmation link we have emailed you. ";
                        return View("RegistrationSuccess");
                    }
                    await _userManager.AddLoginAsync(user, info);                    
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var User = _context.Users.FirstOrDefault(c => c.Email == user.Email);

                    if (User != null)
                    {
                        HttpContext.Session.SetString("UserName", User.UserName);
                        HttpContext.Session.SetString("Email", User.Email);
                        HttpContext.Session.SetInt32("UserID", User.Id);
                        HttpContext.Session.SetInt32("CompanyId", User.CompanyId);

                    }
                    return RedirectToAction("Index", "UserDashboard");                   
                }
            }

            ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
            ViewBag.ErrorMessage = "Please contact support on info@rwfoundation.org";
            return View("Error");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId == null && token == null)
            {
                return RedirectToAction("Index", "UserDashboard");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewData["error"] = $"The User Id  {userId} is invalid";
                return View("notfound");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewData["error"] = "Email cannot confirmed";
            return View("notfound");
        }
        public IActionResult RegistrationSuccess()
        {
            return View();
        }
        public IActionResult notfound()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult EmailBodyMaker(string username, string email, string password, string confirmationlink, string rolename)
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
                + "ConfirmEmail.html";
                string subject = "Email Confirmation";
                var builder = new BodyBuilder();

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();

                }
                string messageBody = string.Format(builder.HtmlBody,                 
                  username,
                  rolename,
                  confirmationlink,
                  email,
                  username,
                  password
                );               
                Helper.EmailSending.SendMail(email, subject, messageBody);
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