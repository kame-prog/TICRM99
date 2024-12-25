using log4net;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
//using TICRM.CRMFilters;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class LeadsController : Controller
    {
        private LeadManager lm = new LeadManager();

        // GET: Leads
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                List<LeadDto> leads = lm.GetLeads(CurrentUserId, UserRole, UserCompanyID);
                return View(leads);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }


        public ActionResult Create()
        {
            try
            {
                //Show all dropdown with values on Create Lead page
                LeadDto lead = new LeadDto();
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                lead.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                lead.IndustryDropdown = new SelectList(lm.IndustryDropDown(), "IndustryId", "Name");
                lead.LeadSourceDropdown = new SelectList(lm.LeadSourDropDown(), "LeadSourceId", "Name");
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypeDropDown(), "LeadTypeId", "Name");
                lead.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                lead.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                lead.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                return View(lead);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }


        /// <summary>
        /// Creates the specified lead.
        /// </summary>
        //[LeadActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeadDto lead)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    //string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    var condition = lm.SaveLead(lead, CurrentUserId,UserCompanyID, false, false);  //Lead save in DB method     
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = SuccessMessage.LeadSubmit;
                        return RedirectToAction("Index");
                    }
                }
                //Show all dropdown with values on Create Lead page
                lead.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                lead.IndustryDropdown = new SelectList(lm.IndustryDropDown(), "IndustryId", "Name");
                lead.LeadSourceDropdown = new SelectList(lm.LeadSourDropDown(), "LeadSourceId", "Name");
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypeDropDown(), "LeadTypeId", "Name");
                lead.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                lead.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                lead.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(lead);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Get data from DB
                LeadDto lead = lm.GetLead(id);
                if (lead == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                                                                                 //Show all dropdown with values on Update Lead page
                lead.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                lead.IndustryDropdown = new SelectList(lm.IndustryDropDown(), "IndustryId", "Name");
                lead.LeadSourceDropdown = new SelectList(lm.LeadSourDropDown(), "LeadSourceId", "Name");
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypeDropDown(), "LeadTypeId", "Name");
                lead.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                lead.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                lead.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                return View(lead);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        //[LeadActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeadDto lead)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    var  condition = lm.SaveLead(lead, CurrentUserId,null, true, false);               
                    //In Condition we check data updated in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = UpdateMessage.LeadUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                                                                                 //Show all dropdown with values on Update Lead page
                lead.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                lead.IndustryDropdown = new SelectList(lm.IndustryDropDown(), "IndustryId", "Name");
                lead.LeadSourceDropdown = new SelectList(lm.LeadSourDropDown(), "LeadSourceId", "Name");
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypeDropDown(), "LeadTypeId", "Name");
                lead.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                lead.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                lead.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(lead);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }


        public ActionResult LeadsDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Leads Detail
                LeadDto lead = lm.GetLead(id);

                if (lead == null)   //If Leads detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(lead);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                //Get Lead from DB for Soft delete
                LeadDto lead = lm.GetLead(id);
                  // pass current userid
                 string CurrentUserId = User.Identity.GetUserId();
                 //soft delete for lead
                lm.SaveLead(lead, CurrentUserId,null, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}