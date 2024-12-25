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

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class AlertsController : Controller
    {
        AlertManager am = new AlertManager();

        public ActionResult Index()
        {
            try
            {
                am.GetAlertCounts();
                return View(am.GetAlerts());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult Create()
        {
            try
            {
                AlertDto alert = new AlertDto();
                alert.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                alert.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                alert.UrgencyDropdown = new SelectList(am.UrgencyDropDown(), "UrgencyId", "Name");
                alert.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified alert.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlertDto alert)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // alert.AlertId = Guid.NewGuid();
                    //alert.CreatedBy = User.Identity.Name;

                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = am.SaveAlert(alert, CurrentUserId);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "Alert Created successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "Alert is not Created.";
                TempData["FormSubmissionStatus"] = "error";
                alert.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                alert.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                alert.UrgencyDropdown = new SelectList(am.UrgencyDropDown(), "UrgencyId", "Name");
                alert.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
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
                var alert = am.GetAlert(id);
                if (alert == null)
                {
                    return HttpNotFound();
                }
                alert.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                alert.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                alert.UrgencyDropdown = new SelectList(am.UrgencyDropDown(), "UrgencyId", "Name");
                alert.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlertDto alert)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //alert.UpdatedBy = User.Identity.Name;
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = am.SaveAlert(alert, CurrentUserId, true);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "Alert Updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }

                TempData["FormSubmissionMessage"] = "Alert is not Updated.";
                TempData["FormSubmissionStatus"] = "error";
                alert.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                alert.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                alert.UrgencyDropdown = new SelectList(am.UrgencyDropDown(), "UrgencyId", "Name");
                alert.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var alert = am.GetAlert(id);

                am.SaveAlert(alert,null, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}