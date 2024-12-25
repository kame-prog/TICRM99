using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Authorize]
    public class CasesController : Controller
    {
        private CaseManager cm = new CaseManager();
        private AccountManager accountmangr = new AccountManager();
        private OpportunityManager om = new OpportunityManager();
        private LeadManager lm = new LeadManager();
        protected CRMEntities dbEnt = new CRMEntities();

        // GET: Cases
        public ActionResult Index()
        {
            try
            {
                // Get the current user ID using the User.Identity.GetUserId() method
                string CurrentUserId = User.Identity.GetUserId();               
                // Get the user role from the Session and convert it to a string
                string UserRole = Convert.ToString(Session["UserRole"]);        
                // Get the user company from the Session and convert it to a string
                string UserCompany = Convert.ToString(Session["UserCompany"]); 
                // Using CaseManager instance  call the GetCases method to retrieve the cases
                List<CaseDto> cases = cm.GetCases(CurrentUserId, UserRole, UserCompany);
                // Pass the list of cases to the View and render the corresponding view
                return View(cases);
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
                // Create a new instance of the CaseDto class 
                CaseDto casedto = new CaseDto();
                //Assign values to the dropdowns
                casedto.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name");
                casedto.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name");
                casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
                casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
                casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "ContactId", "Name");

                // Use enum class to populate RelatedToDropdown
                casedto.RelatedToDropdown = new SelectList(from RelatedToEnumforcase e in Enum.GetValues(typeof(RelatedToEnumforcase)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                casedto.RelatedToIdDropdown = new SelectList("");

                //Return value on the view in form of casedto object
                return View(casedto);
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
        public ActionResult Create(CaseDto casedto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Generate a new GUID for the CaseId
                    Guid CaseId = Guid.NewGuid();
                    casedto.CaseId = CaseId;

                    // Get the current user ID
                    string CurrentUserId = User.Identity.GetUserId();

                    // Get the UserCompany ID from the session
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                    // Save the case using the cm.SaveCase method
                     var condition = cm.SaveCase(casedto, CurrentUserId, UserCompanyID);

                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        // Set a success message in TempData
                        TempData["Success"] = SuccessMessage.CaseSubmit;

                        // Redirect to the Index action
                        return RedirectToAction("Index");
                    }

                   
                }

                // If the model state is not valid, populate the dropdowns again
                casedto.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name");
                casedto.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name");
                casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
                casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
                casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "ContactId", "Name");
                casedto.RelatedToDropdown = new SelectList(from RelatedToEnumforcase e in Enum.GetValues(typeof(RelatedToEnumforcase)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                casedto.RelatedToIdDropdown = new SelectList("");

                // Set the dropdown value in ViewBag
                ViewBag.Dropdown = casedto.RelatedToId;

                // Set a warning message in TempData
                TempData["Warning"] = WarningMessage.EnterField;

                // Return the view with the populated casedto object
                return View(casedto);
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
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the CaseDto object based on the provided id
                CaseDto casedto = cm.GetCaseonId(id);
                if (casedto == null)
                {
                    // If the casedto is null, return a "Not Found" status
                    return HttpNotFound();
                }
                // Populate the dropdowns for the edit view
                casedto.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name");
                casedto.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name");
                casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
                casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
                casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "ContactId", "Name");
                casedto.RelatedToDropdown = new SelectList(from RelatedToEnumforcase e in Enum.GetValues(typeof(RelatedToEnumforcase)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                casedto.RelatedToIdDropdown = new SelectList("");

                // Set the dropdown value in ViewBag
                ViewBag.Dropdown = casedto.RelatedToId;

                // Return the view with the populated casedto object
                return View(casedto);
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
        public ActionResult Edit(CaseDto casedto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the current user ID
                    string CurrentUserId = User.Identity.GetUserId();

                    // Edit the case using the cm.SaveCase method
                    var condition= cm.SaveCase(casedto, CurrentUserId, null, true);

                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        // Set a success message in TempData
                        TempData["Success"] = UpdateMessage.CaseUpdate;

                        // Redirect to the Index action
                        return RedirectToAction("Index");
                    }
                   
                }

                //if the model state is not valid, populate the dropdow again.
                casedto.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name");
                casedto.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name");
                casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
                casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
                casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "ContactId", "Name");
                casedto.RelatedToDropdown = new SelectList(from RelatedToEnumforcase e in Enum.GetValues(typeof(RelatedToEnumforcase)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                casedto.RelatedToIdDropdown = new SelectList("");

                //set dropdown value in the viewbag
                ViewBag.Dropdown = casedto.RelatedToId;

                //Set a warnining message in TempData
                TempData["Warning"] = WarningMessage.EnterField;

                // Return the view with the populated casedto object
                return View(casedto);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            } 
        }
        public ActionResult CaseDetail(Guid? id)
        {
            try
            {
                if (id == null) 
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the Case detail  based on the provided id
                CaseDto Case = cm.GetCaseonId(id);

                if (Case == null)    
                {
                    // If the Case object is null, return a "Not Found" status
                    return HttpNotFound();
                }
                // Return the CaseDetail view with the Case object
                return View(Case);
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
                // Retrieve the Case to be deleted based on the provided id
                CaseDto caseDelete = cm.GetCaseonId(id);

                string CurrentUserId = User.Identity.GetUserId();

                //Delete the case by using  SaveCase() method
                cm.SaveCase(caseDelete, CurrentUserId, null, true, true);

                //Set delete message in the TempData
                TempData["Delete"] = "Case deleted Successfully";

                //Redirct to the Index action method
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
        public JsonResult GetRelatedToValues(string value)
        {
            try
            {
                if (value != "")
                {
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    var RelatedToValues = dbEnt.sp_RelatedToValues_Get(value, UserCompanyID).ToList();
                    return Json(RelatedToValues, JsonRequestBehavior.AllowGet);
                }
                return Json("");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                throw;
            }
            
        }
    }
}