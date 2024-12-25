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

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ReadingsController : Controller
    {
        ReadingManager rm = new ReadingManager();

        public ActionResult Index()
        {
            try
            {
                return View(rm.GetReadings());
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
                ReadingDto reading = new ReadingDto();
                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                return View(reading);
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
        public ActionResult Create(ReadingDto reading)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reading.CreatedBy = User.Identity.GetUserId();
                    bool condition = rm.SaveReading(reading);
                    if (condition == true)
                    {
                        return RedirectToAction("Index");
                    }
                }

                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                return View(reading);
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
                var reading = rm.GetReading(id);
                if (reading == null)
                {
                    return HttpNotFound();
                }
                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                return View(reading);
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
        public ActionResult Edit(ReadingDto reading)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reading.UpdatedBy = User.Identity.GetUserId();
                    bool condition = rm.SaveReading(reading, true);
                    if (condition == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                return View(reading);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var reading = rm.GetReading(id);
                rm.SaveReading(reading, true, true);
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