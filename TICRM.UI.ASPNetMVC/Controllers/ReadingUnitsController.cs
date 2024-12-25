using log4net;
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
    public class ReadingUnitsController : Controller
    {
        ReadingUnitManager readingUnitManager = new ReadingUnitManager();
        ReadingTypeManager readingTypeManager = new ReadingTypeManager();

        // GET: ReadingUnits
        public ActionResult Index()
        {
            try
            {
                return View(readingUnitManager.GetReadingUnits());
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
                ReadingUnitDto reading = new ReadingUnitDto();
                reading.ReadingTypeDropdown = new SelectList(readingTypeManager.GetReadingTypes(), "ReadingTypeId", "Name");
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
        public ActionResult Create(ReadingUnitDto readingUnit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = readingUnitManager.SaveReadingUnit(readingUnit, false, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingUnit);
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type Created.";
                        TempData["FormSubmissionStatus"] = "Success";
                    }
                }
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

        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                if (readingUnit == null)
                {
                    return HttpNotFound();
                }
                readingUnit.ReadingTypeDropdown = new SelectList(readingTypeManager.GetReadingTypes(), "ReadingTypeId", "Name", readingUnit.Type);
                return View(readingUnit);
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
        public ActionResult Edit(ReadingUnitDto readingUnit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = readingUnitManager.SaveReadingUnit(readingUnit, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingUnit);
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type Created.";
                        TempData["FormSubmissionStatus"] = "Success";
                    }
                }
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

        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                    readingUnitManager.SaveReadingUnit(readingUnit, true, true);
                }
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