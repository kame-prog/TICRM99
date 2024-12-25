using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using Microsoft.Owin.Security;
using System.Web.ModelBinding;
using TICRM.DAL;
using TICRM.UI.ASPNetMVC.Helpers;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    public class SuperAdminAuthController : Controller
    {
        private AdminManager adminManager = new AdminManager();
        protected CRMEntities dbEnt = new CRMEntities();

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        // GET: SuperAdminAuth
        public ActionResult Index()
        {
            return View();
        }

        // GET: /SuperAdminAuth/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SuperAdminLoginDto superAdminLoginDto)
        {
            try
            {
                //Checking model state is valid or not,
                if (ModelState.IsValid)
                {
                    // Authenticate the super admin credentials
                    bool isAuthenticated = adminManager.AuthenticateSuperAdmin(superAdminLoginDto);
                    //Check isAuthenticated  is True or Not,
                    if (isAuthenticated) 
                    {
                        var superAdmin = dbEnt.SuperAdminCreds.Where(x => x.Email == superAdminLoginDto.Email).FirstOrDefault();
                        //Store First name and Last name in the session as a user name
                        Session["UserName"] = superAdmin.First_Name + " " + superAdmin.Last_Name;
                        //Store Role in the this session
                        Session["Role"] = superAdmin.Role;
                        //Store id in the Session
                        Session["SuperAdminID"] = superAdmin.ID;
                        FormsAuthentication.SetAuthCookie(superAdminLoginDto.Email, false);

                        //Return Index action method view when user is authenticate
                        return RedirectToAction("Index", "Admin");
                    }
                    // Invalid credentials, show an error
                    ModelState.AddModelError("", "Invalid username or password");
                    //Return same view with error message.
                    return View(superAdminLoginDto);
                }
                return View(superAdminLoginDto);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                throw ex;
            }
        }

        public ActionResult LogOff()
        {
            // Perform the necessary logout actions

            // Clear authentication cookies
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            //// Clear session variables if needed
            //Session.Clear();
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "SuperAdminAuth");
        }

    }
}