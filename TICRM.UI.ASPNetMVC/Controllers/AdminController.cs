using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DAL;
using TICRM.DTOs;
using log4net;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;
using NUnit.Framework;
using Microsoft.AspNet.Identity;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminDashboard
        private AdminManager adminManager = new AdminManager();
        private EmailConfigurationManager emailconfig = new EmailConfigurationManager();
        protected CRMEntities dbEnt = new CRMEntities();

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }

        //Get:All Companies Accounts
        public ActionResult AllAccounts()
        {
            try
            {
                // Show all accounts 
                List<AccountDto> allAccounts = adminManager.AllCompaniesAccounts();
                return View(allAccounts);          // Render the view with the "AllAccounts"
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }
        //GET: AllDevice
        public ActionResult AllDevices()
        {
            try
            {
                //Show device list of all companies
                List<DeviceDto> AllDevices = adminManager.AllCompanyDevices();
                return View(AllDevices);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }

        //Payment Cobfiguratin can view on this page
        public ActionResult PaymentConfig()
        {
            try
            {
                List<PaymentDto> paymentList = new List<PaymentDto>();
                paymentList = adminManager.GetPayments();                   //Get Payment Link data
                var result = dbEnt.Payments.FirstOrDefault();               //Check Data is present or not in DB
                if (result == null)
                {
                    ViewBag.IsNew = true;                                   //Data not in DB, Show New button
                    ViewBag.IsUpdate = false;                               //Data not in DB, Hide Edit button
                    return View(paymentList);
                }
                ViewBag.IsNew = false;                                      //Hide new button if one data presentin DB
                ViewBag.IsUpdate = true;                                    //Show Edit button
                return View(paymentList);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }

        //GET: 2 checkout integration
        public ActionResult AddPayment()
        {
            return View();
        }
        //Post method of Add Payment
        [HttpPost]
        public ActionResult AddPayment(PaymentDto paymentDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var condition = adminManager.SavePaymentLink(paymentDto);
                    if (!condition)
                    {
                        //Set the error message in the warning to the WarningMessage file.
                        ViewBag.error = WarningMessage.DataNotSaved;
                    }
                    else
                    {
                        // Set the value of the "Success" key in TempData to the SuccessMessage file.
                        TempData["Success"] = SuccessMessage.PaymentLinkSubmit;
                        // Redirect to the "PaymentConfig" action
                        return RedirectToAction("PaymentConfig");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }

        //Get: Payment Edit method
        public ActionResult EditPaymentLink(Guid? id)
        {
            try
            {
                //Checking is is null or not.
                if (id==null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                //Get the payment details fron the DB using id.
                PaymentDto paymentDto = adminManager.GetPayment(id);
                //Checking the Payment is found from the DB or not.
                if (paymentDto==null)
                {
                    //Payment detail not found then, then we will recive HttpNotFound message.
                    return HttpNotFound();
                }
                return View(paymentDto);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }

        //Post:: Payment Edit method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPaymentLink(PaymentDto model)
        {
            try
            {
                //Checking the 
                if (ModelState.IsValid)
                {
                    //Here check payment link is update or not.
                    var condition= adminManager.SavePaymentLink(model,true);

                    if (!condition)
                    {
                        //Set the Data not saved message in the ViewBag.error.
                        ViewBag.error = WarningMessage.DataNotSaved;
                    }
                    else
                    {
                        //Set the payment link update message in the TempData["Success"].
                        TempData["Success"] = UpdateMessage.PaymentLinkUpdate;
                        //Redirect to the PaymentConfig view.
                        return RedirectToAction("PaymentConfig");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("AdminError");
            }
        }

        public ActionResult EmailSettingList()
        {
            try
            {
                string UserRole = Convert.ToString(Session["Role"]);        //User Role
                string UserCompanyID = Guid.Empty.ToString();   //Get User Company
                Guid company = Guid.Parse(UserCompanyID);
                List<EmailIntegrationDto> emailIntegrationDtos = emailconfig.GetEmailIntegration(UserCompanyID, UserRole);
                var result = dbEnt.EmailIntegrations.FirstOrDefault(x => x.Company == company && x.Role == UserRole);
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

        //Email setting method.
        public ActionResult EmailSetting()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailSetting(EmailIntegrationDto emailIntegration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    string CurrentUserId = Convert.ToString(Session["SuperAdminID"]);              // pass current userid
                    string UserRole = Convert.ToString(Session["Role"]);        //User Role
                    var condition = emailconfig.EmailIntegrations(emailIntegration, CurrentUserId, null, UserRole, false); //Email configure method
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = SuccessMessage.EmaiLSubmit;    //Success meesage
                        return RedirectToAction("EmailSettingList");
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

        //Email setting update get method
        public ActionResult EditEmailSetting(Guid? id)
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
        public ActionResult EditEmailSetting(EmailIntegrationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = Convert.ToString(Session["SuperAdminID"]);              // pass current userid
                    var condition = emailconfig.EmailIntegrations(model, CurrentUserId, null, null, true);  //Configuration method
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = UpdateMessage.EmailUpdate;   //Success message
                        return RedirectToAction("EmailsettingList");
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