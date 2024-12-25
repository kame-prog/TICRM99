using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class OpportunitiesController : Controller
    {
        OpportunityManager om = new OpportunityManager();

        // GET: Opportunities
        public ActionResult Index(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    //id --> Account ID,,,,When we come from account page to opportuntiy page then this id use
                    List<OpportunityDto> Opportunity = om.GetOpportunities(id);
                    return View(Opportunity);
                }
                else
                {
                    string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                    string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                    List<OpportunityDto> Opportunity = om.GetOpportunities(CurrentUserId, UserRole, UserCompanyID);
                    return View(Opportunity);
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
       

        public ActionResult Create()
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                //Show all dropdown with values on Create opportuntiy page
                OpportunityDto opportunity = new OpportunityDto();
                opportunity.AccountsDropdown = new SelectList(om.AccountDropDown(UserCompanyID), "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.StatusDropDown(), "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.TeamDropDown(), "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.UserDropDown(), "UserId", "Name");
                //opportunity.CurrencyDropdown = new SelectList(om.CurrencyDropDown(), "CurrencyId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OppStageDropDown(), "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.ProbabilityDropDown(), "ProbabilityId", "Name");
                return View(opportunity);
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
        public ActionResult Create(OpportunityDto opportunity)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    //string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    var condition = om.SaveOpportunity(opportunity, CurrentUserId, UserCompanyID);
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = SuccessMessage.OpportunitySubmit;
                        return RedirectToAction("Index");
                    }
                }
                //Show all dropdown with values on Create opportuntiy page
                opportunity.AccountsDropdown = new SelectList(om.AccountDropDown(UserCompanyID), "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.StatusDropDown(), "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.TeamDropDown(), "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.UserDropDown(), "UserId", "Name");
                //opportunity.CurrencyDropdown = new SelectList(om.CurrencyDropDown(), "CurrencyId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OppStageDropDown(), "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.ProbabilityDropDown(), "ProbabilityId", "Name");

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(opportunity);
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
                var opportunity = om.GetOpportunity(id);
                if (opportunity == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Show all dropdown with values on Edit opportuntiy page
                opportunity.AccountsDropdown = new SelectList(om.AccountDropDown(UserCompanyID), "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.StatusDropDown(), "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.TeamDropDown(), "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.UserDropDown(), "UserId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OppStageDropDown(), "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.ProbabilityDropDown(), "ProbabilityId", "Name");
                return View(opportunity);
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
        public ActionResult Edit(OpportunityDto opportunity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();                          // pass current userid
                    var condition = om.SaveOpportunity(opportunity, CurrentUserId,null, true);
                    //In Condition we check data updated in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = UpdateMessage.OpportunityUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                opportunity.AccountsDropdown = new SelectList(om.AccountDropDown(UserCompanyID), "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.StatusDropDown(), "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.TeamDropDown(), "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.UserDropDown(), "UserId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OppStageDropDown(), "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.ProbabilityDropDown(), "ProbabilityId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(opportunity);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult OpportunityDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Opportunity Detail
                OpportunityDto opportunity = om.GetOpportunity(id);

                if (opportunity == null)  //If Opportunity detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(opportunity);
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
                var opportunity = om.GetOpportunity(id);
                // pass current userid
                string CurrentUserId = User.Identity.GetUserId();
                //soft delete for opportunity
                om.SaveOpportunity(opportunity, CurrentUserId,null, true, true);
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
    }
}