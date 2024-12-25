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
    [Authorize(Roles = "Admin")]
    public class GlobalSearchController : Controller
    {
        private GlobalSearchManager gsm = new GlobalSearchManager();

        public ActionResult Index()
        {
            try
            {
                return View(gsm.GetGlobalSearch());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GlobalSearchDto globalSearchDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gsm.SaveGlobalSearch(globalSearchDto, false);
                    TempData["Success"] = "Global search submit Successfully";
                    return RedirectToAction("Index");
                }
                TempData["Warning"] = "Please enter required fields";
                return View(globalSearchDto);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Edit(Guid id)
        {
            try
            {
                GlobalSearchDto data = new GlobalSearchDto(); // create an object of Global Search DTO
                data = gsm.GetGlobalSearchOnId(id); // get global search on id and place in data object
                return View(data); // return data in json format
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
        public ActionResult Edit(GlobalSearchDto globalSearch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     gsm.SaveGlobalSearch(globalSearch, true);
                    TempData["Success"] = "Global Search updated Successfully";
                    return RedirectToAction("Index");
                }
                TempData["Warning"] = "Please enter required fields";
                return View(globalSearch);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult GlobalSearchDetail(Guid id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Global Search  Detail
                GlobalSearchDto globalSearch = gsm.GetGlobalSearchOnId(id);

                if (globalSearch == null)  //If Global Search detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(globalSearch);
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