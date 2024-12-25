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
using MQTTnet.Client;


namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ActivitiesController : Controller
    {
        private ActivityManager am = new ActivityManager();
        private DeviceManager dm = new DeviceManager();
        // GET: Activities
        //Show Activities on Listing Page
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompany = Convert.ToString(Session["UserCompany"]);   //Get User Company
                List<ActivityDTO> activities = am.GetActivities(CurrentUserId, UserRole,UserCompany);
                return View(activities);
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
                ActivityDTO activity = new ActivityDTO();
                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnumforactivity e in Enum.GetValues(typeof(RelatedToEnumforactivity)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToIDDropdown = new SelectList("");

                // Use collection initializer.
                activity.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                activity.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                activity.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                return View(activity);
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
        public ActionResult Create(ActivityDTO activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    var condition = am.SaveActivity(activity, CurrentUserId, UserCompanyID, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Success message
                        TempData["Success"] = SuccessMessage.ActivitySubmit;
                        return RedirectToAction("Index");
                    }               
                }

                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnumforactivity e in Enum.GetValues(typeof(RelatedToEnumforactivity)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToIDDropdown = new SelectList("");
                activity.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                activity.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                activity.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                ViewBag.Dropdown = activity.RelatedToID;
                TempData["Warning"] =WarningMessage.EnterField;
                return View(activity);
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
                ActivityDTO activity = am.GetActivity(id);
                if (activity == null)
                {
                    return HttpNotFound();
                }

                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnumforactivity e in Enum.GetValues(typeof(RelatedToEnumforactivity)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToIDDropdown = new SelectList("");
                activity.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                activity.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                activity.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                ViewBag.Dropdown = activity.RelatedToID;
                return View(activity);
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
        public ActionResult Edit(ActivityDTO activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    var condition=  am.SaveActivity(activity, CurrentUserId,null , true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Activity Update message
                        TempData["Success"] = UpdateMessage.ActivityUpdate;
                        return RedirectToAction("Index");
                    }
                  
                }
                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnumforactivity e in Enum.GetValues(typeof(RelatedToEnumforactivity)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToIDDropdown = new SelectList("");
                activity.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                activity.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                activity.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                ViewBag.Dropdown = activity.RelatedToID;

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(activity);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult ActivityDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Activity Detail
                ActivityDTO activity = am.GetActivity(id);

                if (activity == null)  //If Activity detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(activity);
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
                ActivityDTO activity = am.GetActivity(id);
                //string CurrentUserId = User.Identity.GetUserId(); // get current userid
                am.SaveActivity(activity, null,null , true, true);
           
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