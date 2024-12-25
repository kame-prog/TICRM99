using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DAL;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Models;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class UserController : Controller
    {
        UserAccountManager userAccountManager = new UserAccountManager();
        protected CRMEntities dbEnt = new CRMEntities();
        public UserController()
        {
        }
        public UserController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            //RoleManager = roleManager;
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

        // GET: User
        public ActionResult Index()
        {
            // Get the current user's ID
            //var userId = User.Identity.GetUserId();

            //// Retrieve the user from the database
            //var user = UserManager.FindById(userId);

            //// Pass the user object to the view
            //return View(user);
            return View();
        }
        [HttpGet]
        public ActionResult UserProfile(string id)
        {
            try
            {
                if (id == null)
                {
                    //Checking id is null or not.
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                EditUserDto user = userAccountManager.GetAccUser(id);
                //Checking user data is null or not.
                if (user == null)
                {
                    return HttpNotFound();
                }
                //Save company id in variable from the session and cpnvert it into Guid
                Guid CompanyId = Guid.Parse(Convert.ToString(Session["UserCompany"]));
                //In view bag we save company name.
                ViewBag.CompanyName = (from company in dbEnt.Companies
                                       where company.Company_ID == CompanyId
                                       select company.Name).FirstOrDefault();
                //Bind values into the dropdown.
                user.CountryDropdown = new SelectList(userAccountManager.CountryDropDown(), "ID", "Country_Name");
                user.IndustryDropdown = new SelectList(userAccountManager.IndustryDropDown(), "IndustryId", "Name");

                return View(user);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [HttpPost]
        //[AccountActionFilter]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(EditUserDto user)
        {
            try
            {
                //Here we check modelstate is valid or not
                if (ModelState.IsValid)
                {
                    var condition = userAccountManager.EditProfile(user);
                    //In Condition check data submitted in DB updated or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                        @ViewBag.error = WarningMessage.DataNotSaved;
                    }
                    else
                    {
                        Session["UserName"] = user.FirstName + " " + user.LastName;
                        //Account Update message
                        TempData["Success"] = UpdateMessage.ProfleUpdate;
                        //return RedirectToAction("Index", new { id = account.AccountId });
                        return RedirectToAction("index", "Dashboard");
                    }
                }
                //Save company id in variable from the session and cpnvert it into Guid
                Guid CompanyId = Guid.Parse(Convert.ToString(Session["UserCompany"]));
                //In view bag we save company name.
                ViewBag.CompanyName = (from company in dbEnt.Companies
                                       where company.Company_ID == CompanyId
                                       select company.Name).FirstOrDefault();
                //Bind values into the dropdown.
                user.CountryDropdown = new SelectList(userAccountManager.CountryDropDown(), "ID", "Country_Name");
                user.IndustryDropdown = new SelectList(userAccountManager.IndustryDropDown(), "IndustryId", "Name");
                return View(user);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
    }
}