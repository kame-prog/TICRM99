using log4net;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DAL;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize(Roles = "Admin")]
    public class EmailIntegrationController : Controller
    {
        private EmailConfigurationManager emailconfig= new EmailConfigurationManager();

        protected CRMEntities dbEnt = new CRMEntities();


        // GET: EmailIntegration

        public ActionResult Index()
        {
            try
            {
                string UserRole = Convert.ToString(Session["UserRole"]);        //User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                Guid company = Guid.Parse(UserCompanyID);
                List<EmailIntegrationDto> emailIntegrationDtos = emailconfig.GetEmailIntegration(UserCompanyID,UserRole);
                var result = dbEnt.EmailIntegrations.FirstOrDefault(x => x.Company == company && x.Role==UserRole);
                if (result == null)
                {
                    ViewBag.IsNew = true;
                    ViewBag.IsUpdate = false;
                    return View(emailIntegrationDtos);
                }
                ViewBag.IsNew = false;
                ViewBag.IsUpdate = true;
                return View(emailIntegrationDtos);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
            
        }

        //Email configuration get method
        public ActionResult EmailConfiguration()
        {
            try
            {
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
      
        //Email configuration post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailConfiguration(EmailIntegrationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    string UserRole = Convert.ToString(Session["UserRole"]);        //User Role
                    var condition = emailconfig.EmailIntegrations(model, CurrentUserId, UserCompanyID, UserRole, false); //Email configure method
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = SuccessMessage.EmaiLSubmit;    //Success meesage
                        return RedirectToAction("Index");
                    }
                }
                //Set Warning message in the TempData
                TempData["Warning"] = WarningMessage.EnterField;
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

        //Email confiuration update get method
        public ActionResult EditEmailConfiguration(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                EmailIntegrationDto emailIntegrationDto = emailconfig.GetEmail(id);
                if (emailIntegrationDto == null)
                {
                    return HttpNotFound();
                }

                return View(emailIntegrationDto);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        //Email configuration update post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmailConfiguration(EmailIntegrationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    var condition = emailconfig.EmailIntegrations(model,CurrentUserId,null,null,true);  //Configuration method
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = UpdateMessage.EmailUpdate;   //Success message
                        return RedirectToAction("Index");
                    }
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
    }
}