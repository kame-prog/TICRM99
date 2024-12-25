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
    public class ServiceCallsController : Controller
    {
        ServiceCallManager scm = new ServiceCallManager();

        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                //Show all service on listing page
                List<ServiceCallDto> serviceCalls = scm.GetServiceCalls(CurrentUserId, UserRole, UserCompanyID);
                return View(serviceCalls);
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
                //Bind dropdown values with create form
                ServiceCallDto serviceCall = new ServiceCallDto();
                serviceCall.StatusDropdown = new SelectList(scm.StatusDropDown(), "StatusId", "Name");
                serviceCall.AssignedTeamDropdown = new SelectList(scm.TeamDropDown(), "TeamId", "Name");
                serviceCall.UrgencyDropdown = new SelectList(scm.UrgencyDropDown(), "UrgencyId", "Name");
                serviceCall.AssignedUserDropdown = new SelectList(scm.UserDropDown(), "UserId", "Name");
                serviceCall.ServiceCallStageDropdown = new SelectList(scm.WorkStageDropDown(), "WorkStageId", "Name");
                return View(serviceCall);
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
        /// POST request to create Service Calls, Receive object of,
        /// new workflow Mapping validate it and creates a new workflow mapping
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceCallDto serviceCall)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    bool condition = scm.SaveServiceCall(serviceCall, CurrentUserId, UserCompanyID, false, false);
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted, show successfully toaster on listing screen 
                        TempData["Success"] = SuccessMessage.ServiceCallSubmit;
                        return RedirectToAction("Index");
                    }
                }
                //Bind dropdown values with create form
                serviceCall.StatusDropdown = new SelectList(scm.StatusDropDown(), "StatusId", "Name");
                serviceCall.AssignedTeamDropdown = new SelectList(scm.TeamDropDown(), "TeamId", "Name");
                serviceCall.UrgencyDropdown = new SelectList(scm.UrgencyDropDown(), "UrgencyId", "Name");
                serviceCall.AssignedUserDropdown = new SelectList(scm.UserDropDown(), "UserId", "Name");
                serviceCall.ServiceCallStageDropdown = new SelectList(scm.WorkStageDropDown(), "WorkStageId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(serviceCall);
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
                //Get service call for Update from DB
                var serviceCall = scm.GetServiceCall(id);
                if (serviceCall == null)
                {
                    return HttpNotFound();
                }
                //Bind dropdown values with edit form
                serviceCall.StatusDropdown = new SelectList(scm.StatusDropDown(), "StatusId", "Name");
                serviceCall.AssignedTeamDropdown = new SelectList(scm.TeamDropDown(), "TeamId", "Name");
                serviceCall.UrgencyDropdown = new SelectList(scm.UrgencyDropDown(), "UrgencyId", "Name");
                serviceCall.AssignedUserDropdown = new SelectList(scm.UserDropDown(), "UserId", "Name");
                serviceCall.ServiceCallStageDropdown = new SelectList(scm.WorkStageDropDown(), "WorkStageId", "Name");
                return View(serviceCall);
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
        public ActionResult Edit(ServiceCallDto serviceCall)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    bool condition = scm.SaveServiceCall(serviceCall, CurrentUserId,null, true,false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = UpdateMessage.ServiceCallUpdate;
                        return RedirectToAction("Index");
                    }
                }
                //Bind dropdown values with edit form
                serviceCall.StatusDropdown = new SelectList(scm.StatusDropDown(), "StatusId", "Name");
                serviceCall.AssignedTeamDropdown = new SelectList(scm.TeamDropDown(), "TeamId", "Name");
                serviceCall.UrgencyDropdown = new SelectList(scm.UrgencyDropDown(), "UrgencyId", "Name");
                serviceCall.AssignedUserDropdown = new SelectList(scm.UserDropDown(), "UserId", "Name");
                serviceCall.ServiceCallStageDropdown = new SelectList(scm.WorkStageDropDown(), "WorkStageId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult ServiceCallDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Service Call Detail
                ServiceCallDto servicecall = scm.GetServiceCall(id);

                if (servicecall == null) //If Service Call detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(servicecall);
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
                ServiceCallDto serviceCall = scm.GetServiceCall(id);
                //Soft delete the service call record
                string CurrentUserId = User.Identity.GetUserId();
                scm.SaveServiceCall(serviceCall, CurrentUserId,null, true, true);
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