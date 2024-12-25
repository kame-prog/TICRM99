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
    public class AddressesController : Controller
    {
        private AddressManager addressManager = new AddressManager();

        // GET: Addresses
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                List<AddressDto> addressDtos = addressManager.GetAllAddresses(CurrentUserId, UserRole, UserCompanyID);
                return View(addressDtos);
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
                AddressDto address = new AddressDto();
                address.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");

                return View(address);
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
        /// Creates the specified address.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddressDto address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    bool condition = addressManager.SaveAddress(address, CurrentUserId, UserCompanyID, false, false);
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                      
                        ViewBag.error = WarningMessage.DataNotSaved;
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = SuccessMessage.AddressSubmit;
                        return RedirectToAction("Index");
                    }

                }
                address.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(address);
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
                AddressDto address = addressManager.GetAddress(id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                address.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                return View(address);
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
        public ActionResult Edit(AddressDto address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    bool condition = addressManager.SaveAddress(address, CurrentUserId, null, true, false);
                    if (!condition)
                    {

                        address.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                        ViewBag.error = WarningMessage.DataNotSaved;

                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = UpdateMessage.AddressUpdate;
                        return RedirectToAction("Index");
                    }
                }
                address.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(address);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }

        }
        public ActionResult AddressDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Address Detail
                AddressDto address = addressManager.GetAddress(id);

                if (address == null)    //If Address detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(address);
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
                AddressDto address = addressManager.GetAddress(id);
                addressManager.SaveAddress(address,null,null, true, true);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}