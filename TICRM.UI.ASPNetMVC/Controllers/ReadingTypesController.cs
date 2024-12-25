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
    public class ReadingTypesController : Controller
    {

        private ReadingTypeManager rtm = new ReadingTypeManager();

        public ActionResult Index()
        {
            try
            {
                return View(rtm.GetReadingTypes());
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

        /// <summary>
        /// POST request to create Resources, Receive object of,
        /// new Reading Type validate it and creates a new Reading Type
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReadingTypeDto readingType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rtm.SaveReadingType(readingType, false, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingType);
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
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                if (readingType == null)
                {
                    return HttpNotFound();
                }
                return View(readingType);
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
        public ActionResult Edit(ReadingTypeDto readingType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rtm.SaveReadingType(readingType, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingType);
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
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                bool condition = rtm.SaveReadingType(readingType, true, true);
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