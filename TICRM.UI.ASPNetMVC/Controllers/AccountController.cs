using TICRM.UI.ASPNetMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TICRM.DAL;
using TICRM.DTOs;
using System.IO;
using System.Web.Services.Description;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using TICRM.UI.ASPNetMVC.App_Start;
using log4net;
using NUnit.Framework.Constraints;
using System.Data.Entity.Core.Objects;
using TICRM.UI.ASPNetMVC.Helpers;
using System.Configuration;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using TweetSharp;
using System.Net.Http;
using System.Text;
using Facebook;
using Microsoft.Owin.Security.Twitter.Messages;
using Owin;
using System.Security.Policy;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        protected CRMEntities dbEnt = new CRMEntities();
        UserAccountManager userAccountManager = new UserAccountManager();
        private EmailConfigurationManager emailconfig = new EmailConfigurationManager();
        //Initialize a logger instance
        private static readonly ILog log = LogManager.GetLogger(typeof(ContactController));
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDto model, string returnUrl)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                // Require the user to have a confirmed email before they can log on.
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        ViewBag.EmailConfirmedErr = "You must have a confirmed email to log in.";
                        return View();
                    }
                }
                else
                {
                    return View("user null");
                }

                // This doen't count login failures towards lockout only two factor authentication
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        var paymentlink = dbEnt.Payments.FirstOrDefault();
                        Session["PaymentLink"] = paymentlink.PaymentLink;
                        Session["UserName"] = user.FirstName + " " + user.LastName;
                        Session["UserID"] = user.Id;
                        Session["UserCompany"] = user.CompanyId;
                        var userRoles = await UserManager.GetRolesAsync(user.Id);
                        Session["UserRole"] = userRoles[0];
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Your email or password is incorrect.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                //Create object of UserRegistrationVM class.
                UserRegistrationVM userRegistrationVM = new UserRegistrationVM();

                //Create object of UserRegisterDto class in form of this object userRegistrationVM.UserRegister
                userRegistrationVM.UserRegister = new UserRegisterDto();

                //Bind values into the dropdown.
                userRegistrationVM.UserRegister.CountryDropdown = new SelectList(userAccountManager.CountryDropDown(), "ID", "Country_Name");
                userRegistrationVM.UserRegister.IndustryDropdown = new SelectList(userAccountManager.IndustryDropDown(), "IndustryId", "Name");

                //Return value to the view in form of object.
                return View(userRegistrationVM);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserRegistrationVM model)
        {
            try
            {
                //Here we check model state is valid or not.
                if (ModelState.IsValid)
                {
                    //Check same name Company is already exist or not.
                    var CompanyExist = dbEnt.Companies.Any(x => x.Name == model.Company.Name);

                    //Check email is already exist or not.
                    var EmailExist = dbEnt.AspNetUsers.Any(x => x.Email == model.UserRegister.Email);
                    if (CompanyExist)
                    {
                        //Create object of UserRegisterDto class in form of this object userRegistrationVM.UserRegister
                        model.UserRegister = new UserRegisterDto();
                        //Bind values into the dropdown.
                        model.UserRegister.CountryDropdown = new SelectList(userAccountManager.CountryDropDown(), "ID", "Country_Name");
                        model.UserRegister.IndustryDropdown = new SelectList(userAccountManager.IndustryDropDown(), "IndustryId", "Name");
                        //Return value to the view in form of object.
                        //If Company is exist, then show Exist message.
                        ViewBag.Exist = "This company is already registered.";
                        return View(model);
                    }
                    else if (EmailExist)
                    {
                        //Create object of UserRegisterDto class in form of this object userRegistrationVM.UserRegister
                        model.UserRegister = new UserRegisterDto();
                        //Bind values into the dropdown.
                        model.UserRegister.CountryDropdown = new SelectList(userAccountManager.CountryDropDown(), "ID", "Country_Name");
                        model.UserRegister.IndustryDropdown = new SelectList(userAccountManager.IndustryDropDown(), "IndustryId", "Name");
                        //Return value to the view in form of object.
                        //If Email is exist then show the Email exist message.
                        ViewBag.Exist = "This email has already an account.";
                        return View(model);
                    }
                    else
                    {   //When company or email not exist we save the data in DB
                        model.Company.Company_ID = Guid.NewGuid();

                        //Save company in DB (Company Table)
                        var condition = userAccountManager.SaveCompany(model.Company);

                        //Save User data in the table in ASP.NETUSER table.
                        var user = new ApplicationUser
                        {
                            UserName = model.UserRegister.Email,
                            Email = model.UserRegister.Email,
                            FirstName = model.UserRegister.FirstName,
                            LastName = model.UserRegister.LastName,
                            CompanyId = model.Company.Company_ID,
                            PhoneNumber = model.UserRegister.PhoneNumber,
                            Countryid = model.UserRegister.Countryid,
                            Industryid = model.UserRegister.Industryid
                        };
                        var result = await UserManager.CreateAsync(user, model.UserRegister.Password);  //Save User in DB (ASP Net User Table)

                        if (result.Succeeded)
                        {

                            result = await UserManager.AddToRoleAsync(user.Id, "Admin");                //Add Role Admin by defualt when signup any user
                                                                                                        //SMTP Email send method
                            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            var regenratelink = Url.Action("RegenrateLink", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                            //Email format code
                            string body = string.Empty;
                            //using streamreader for reading my htmltemplate   
                            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/Email.html")))
                            {
                                body = reader.ReadToEnd();
                            }
                            body = body.Replace("{UserName}", user.Email); //replacing the required things  
                            body = body.Replace("{callbackUrl}", callbackUrl);
                            body = body.Replace("{regenratelink}", regenratelink);

                            await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);
                            ViewBag.Link = callbackUrl;
                            return View("DisplayEmail");
                        }

                    }
                }

                //Create object of UserRegisterDto class in form of this object userRegistrationVM.UserRegister
                model.UserRegister = new UserRegisterDto();
                //Bind values into the dropdown.
                model.UserRegister.CountryDropdown = new SelectList(userAccountManager.CountryDropDown(), "ID", "Country_Name");
                model.UserRegister.IndustryDropdown = new SelectList(userAccountManager.IndustryDropDown(), "IndustryId", "Name");
                //Return value to the view in form of object.
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }



        //If confirmation link is expired, we regenrate confirmation link by this.
        [AllowAnonymous]
        public ActionResult RegenrateLink()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegenrateLink(RegenrateConfirmationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist 
                        ViewBag.EmailNotReg = "Your email does not exist.";
                        return View();
                    }
                    else if (await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        //Email is already confirmed
                        ViewBag.EmailConfirmed = "Your email is already confirmed.";
                        return View();
                    }
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    var regenratelink = Url.Action("RegenrateLink", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    //Email format code
                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/Email.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{UserName}", user.Email); //replacing the required things  
                    body = body.Replace("{callbackUrl}", callbackUrl);
                    body = body.Replace("{regenratelink}", regenratelink);

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);
                    ViewBag.Link = callbackUrl;
                    return View("DisplayEmail");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }

        }


        //Get: Create User with Roles:
        [SessionExpire]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUserRole()
        {
            try
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (var role in RoleManager.Roles)
                    list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
                ViewBag.RoleList = list;

                Guid UserCompanyID = Guid.Parse(Convert.ToString(Session["UserCompany"])); //Pass Company ID

                var EmailData = dbEnt.EmailIntegrations.FirstOrDefault(x => x.Company == UserCompanyID);
                if (EmailData != null)
                {
                    return View();
                }
                ViewBag.EmailConfigmsg = "Please, configure your email account to receive notifications.";
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [SessionExpire]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUserRole(UserRoleDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        CompanyId = model.Company
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                        result = await UserManager.AddToRoleAsync(user.Id, model.RoleName);
                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        var regenratelink = Url.Action("RegenrateLink", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                        string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Email.html"));
                        body = body.Replace("{callbackUrl}", callbackUrl);
                        body = body.Replace("{regenratelink}", regenratelink);
                        emailconfig.CompanyEmailSend(user.Email, body, UserCompanyID);
                        ViewBag.Link = callbackUrl;
                        return RedirectToAction("UserRoleList");
                    }
                    ViewBag.ExistEmail = "This email has already an account.";
                }
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (var role in RoleManager.Roles)
                    list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
                ViewBag.RoleList = list;

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }

        }
        public ActionResult SendEmail()
        {
            try
            {
                var fromAddress = new MailAddress("dynamics@techimplement.com", "Usman");
                var toAddress = new MailAddress("usman@techimplement.com", "Asif");
                const string fromPassword = "dynamics@123!!";
                const string subject = "Subject";
                const string body = "Email Body";

                var smtpClient = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtpClient.Send(message);
                }

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [SessionExpire]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult UserRoleList()
        {
            try
            {
                Guid UserCompanyID = Guid.Parse(Convert.ToString(Session["UserCompany"]));   //Get User Company

                var usersWithRoles = (from user in dbEnt.AspNetUsers
                                      where user.CompanyId == UserCompanyID
                                      from userRole in user.AspNetRoles
                                      join role in dbEnt.AspNetRoles on userRole.Id equals
                                      role.Id
                                      select new UserRoleDto()
                                      {
                                          Name = user.FirstName + " " + user.LastName,
                                          Email = user.Email,
                                          PhoneNumber = user.PhoneNumber,
                                          RoleName = role.Name
                                      }).ToList();
                //var data = await UserManager.Users.ToListAsync();
                return View(usersWithRoles);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return View("Error");
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }

        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        ViewBag.EmailNotReg = "Your email does not exist";
                        return View();
                    }
                    else if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    {
                        ViewBag.EmailNotConfirmed = "Please confirm your account";
                        return View();
                    }
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    //Email format code
                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/ResetPassEmail.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{callbackUrl}", callbackUrl);

                    await UserManager.SendEmailAsync(user.Id, "Reset Password", body);
                    ViewBag.Link = callbackUrl;
                    return View("ForgotPasswordConfirmation");
                }
                catch (Exception ex)
                {
                    // Log the exception using log4net
                    log.Error("An error occurred", ex);
                    return View("Error");               // Display an error view to the user
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ViewBag.EmailNotExist = "You wrote different email.";
                return View();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //[AllowAnonymous]
        //public ActionResult FacebookAuth()
        //{
        //    var redirectUri = Url.Action("ExternalLoginCallback", "Account", null, Request.Url.Scheme);

        //    var facebookAuthenticationEndpoint = $"https://www.facebook.com/v2.7/dialog/oauth?client_id=617158460633025&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope=email&response_type=code";



        //    return Redirect(facebookAuthenticationEndpoint);
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                string clientId = ConfigurationManager.AppSettings["Google:clientId"];
                string url = ConfigurationManager.AppSettings["Google:url"];
                var response = GoogleAuth.GetAuthUrl(clientId, url);
                ViewBag.response = response;

                //  Uri uri = service.GetAuthenticationUrl(requesttoken);
                //  return Redirect(uri.ToString());

                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred", ex);
                return View("Error");
                // Display an error view to the user
            }

        }
        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string code)
        {
            string clientId = ConfigurationManager.AppSettings["Google:clientId"];
            string url = ConfigurationManager.AppSettings["Google:url"];
            string cliensecret = ConfigurationManager.AppSettings["Google:clientSecret"];

            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, cliensecret, url);
            var userprofile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
            UserDto userInfo = JsonConvert.DeserializeObject<UserDto>(userprofile);

            var user = await UserManager.FindByNameAsync(userInfo.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.EmailConfirmedErr = "You must have a confirmed email to log in.";
                    return View();
                }


                // This doen't count login failures towards lockout only two factor authentication
                // To enable password failures to trigger lockout, change to shouldLockout: true
                await SignInManager.SignInAsync(user, true, true);
                var paymentlink = dbEnt.Payments.FirstOrDefault();
                Session["PaymentLink"] = paymentlink.PaymentLink;
                Session["UserName"] = user.FirstName + " " + user.LastName;
                Session["UserID"] = user.Id;
                Session["UserCompany"] = user.CompanyId;
                var userRoles = await UserManager.GetRolesAsync(user.Id);
                Session["UserRole"] = userRoles[0];
                return RedirectToLocal(null);
            }
            else
            {
                // Handle case where user is not found
                ModelState.AddModelError("", "Your email or password is incorrect.");
                var response = GoogleAuth.GetAuthUrl(clientId, url);
                ViewBag.response = response;
                return View("Login");
                //return View("Login"); // Assuming you have a view named "Login"
            }
        }
        

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }


        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginurl = fb.GetLoginUrl(new
            {
                client_id = "617158460633025",
                client_secret = "a6ce3d4d919505a129bd014a21b16d65",
                redirect_uri = RedirectUri.AbsoluteUri,
                scope = "email",
                response_type = "code"
            });

            return Redirect(loginurl.AbsoluteUri);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Facebookcallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "617158460633025",
                client_secret = "a6ce3d4d919505a129bd014a21b16d65",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accesstoken = result.access_token;
            Session["accesstoken"] = accesstoken;
            fb.AccessToken = accesstoken;
            dynamic userprofile = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
            UserDto userInfo = new UserDto
            {
                
                Email = userprofile.email
                // Add other properties as needed
            };

            var user = await UserManager.FindByNameAsync(userInfo.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.EmailConfirmedErr = "You must have a confirmed email to log in.";
                    return View();
                }

                await SignInManager.SignInAsync(user, true, true);
                var paymentlink = dbEnt.Payments.FirstOrDefault();
                Session["PaymentLink"] = paymentlink.PaymentLink;
                Session["UserName"] = user.FirstName + " " + user.LastName;
                Session["UserID"] = user.Id;
                Session["UserCompany"] = user.CompanyId;
                var userRoles = await UserManager.GetRolesAsync(user.Id);
                Session["UserRole"] = userRoles[0];
                return RedirectToLocal(null);
            }
            else
            {
                // Handle case where user is not found
                ModelState.AddModelError("", "Your email or password is incorrect.");
                var response = GoogleAuth.GetAuthUrl("617158460633025","https://localhost:44378/Account/Facebookcallback");
                ViewBag.response = response;
                return View("Login");
                //return View("Login"); // Assuming you have a view named "Login"
            }
            //string email = me.email;
            //FormsAuthentication.SetAuthCookie(email, false);
          //  return RedirectToAction("index", "home");
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                TempData["WelcomeMsg"] = "Welcome";
                return Redirect(returnUrl);
            }
            TempData["WelcomeMsg"] = "Welcome";
            return RedirectToAction("index", "Dashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }



        #endregion
    }
}