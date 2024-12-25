using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
  /************Alerts Controller************
  Class [AlertsController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with Alerts like, 
  ||             navigating to the pages, getting associated modules for specific Alert]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ********************************************/
    
    public class AlertsController : BaseController
    {
        AlertManager am = new AlertManager();

        /// <summary>
        /// Get alerts and return in view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
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

        /// <summary>
        /// Detail view for the specified alert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Details(Guid? id)
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
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Partials details view for alert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var alert = am.GetAlert(id);
                return PartialView("_PartialAlertsDetails", alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partials delete view for alert.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var alert = am.GetAlert(id);
                return PartialView("_PartialAlertsDelete", alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Create view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                AlertDto alert = new AlertDto();
                alert.StatusDropdown = new SelectList(am.Status, "StatusId", "Name");
                alert.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name");
                alert.UrgencyDropdown = new SelectList(am.Urgencies, "UrgencyId", "Name");
               alert.AssignedUserDropdown= new SelectList(am.Users, "UserId", "Name");
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
        /// <param name="alert">The alert.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlertDto alert)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // alert.AlertId = Guid.NewGuid();
                    alert.CreatedBy = User.Identity.Name;
                    alert.CreatedDate = DateTime.Now;
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
                alert.StatusDropdown = new SelectList(am.Status, "StatusId", "Name", alert.StatusId);
                alert.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name", alert.AssignedTeam);
                alert.UrgencyDropdown = new SelectList(am.Urgencies, "UrgencyId", "Name", alert.UrgencyId);
                alert.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name", alert.AssignedUser);
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits view for a specified alert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
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
                alert.StatusDropdown = new SelectList(am.Status, "StatusId", "Name", alert.StatusId);
                alert.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name", alert.AssignedTeam);
                alert.UrgencyDropdown = new SelectList(am.Urgencies, "UrgencyId", "Name", alert.UrgencyId);
                alert.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name", alert.AssignedUser);
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlertDto alert)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    alert.UpdatedBy = User.Identity.Name;
                    alert.UpdatedDate = DateTime.Now;
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

                alert.StatusDropdown = new SelectList(am.Status, "StatusId", "Name", alert.StatusId);
                alert.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name", alert.AssignedTeam);
                alert.UrgencyDropdown = new SelectList(am.Urgencies, "UrgencyId", "Name", alert.UrgencyId);
                alert.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name", alert.AssignedUser);
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view for the specified alert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Delete(Guid? id)
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
                return View(alert);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var alert = am.GetAlert(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                am.SaveAlert(alert, CurrentUserId, true, true);
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
