using log4net;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
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
    public class LocationsController : Controller
    {
        LocationManager lm = new LocationManager();

        // GET: Locations
        public ActionResult Index(Guid? id)
        {
            //Show locations on Listing Page
            try
            {
                if (id != null)
                {
                    List<LocationDto> loca = lm.GetLocations(id);
                    return View(loca);
                }
                else
                {
                    string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                    string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                    List<LocationDto> loca = lm.GetLocations(CurrentUserId, UserRole, UserCompanyID);
                    return View(loca);
                }
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
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                //creating the location
                LocationDto location = new LocationDto();
                location.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                location.LocationTypeDropdown = new SelectList(lm.LocaTypeDropDown(), "LocationTypeId", "Name");
                location.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                location.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                location.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                location.AccountsDropdown = new SelectList(lm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                return View(location);
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
        /// Creates the specified location.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationDto location)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                   /* string UserCompanyID = Convert.ToString(Session["UserCompany"]);*/ //Pass Company ID
                    bool condition = lm.SaveLocation(location, CurrentUserId, UserCompanyID, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = SuccessMessage.LocationSubmit; // For the  Creating Toaster
                        return RedirectToAction("Index");
                    }
                }
                // if model state does not work it will run this dropdown.
                location.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                location.LocationTypeDropdown = new SelectList(lm.LocaTypeDropDown(), "LocationTypeId", "Name");
                location.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                location.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                location.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                location.AccountsDropdown = new SelectList(lm.AccountDropDown(UserCompanyID), "AccountId", "Name");

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(location);
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
                var location = lm.GetLocation(id); //Getting location via Id
                if (location == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                location.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                location.LocationTypeDropdown = new SelectList(lm.LocaTypeDropDown(), "LocationTypeId", "Name");
                location.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                location.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                location.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                location.AccountsDropdown = new SelectList(lm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                return View(location);
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
        public ActionResult Edit(LocationDto location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();      // pass current userid
                    bool condition = lm.SaveLocation(location, CurrentUserId,null, true, false);
                    if (!condition) 
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = UpdateMessage.LocationUpdate; // Editing Toaster
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                                                                                   // if model state is not valid this code will run
                location.AddressDropdown = new SelectList(lm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                location.LocationTypeDropdown = new SelectList(lm.LocaTypeDropDown(), "LocationTypeId", "Name");
                location.StatusDropdown = new SelectList(lm.StatusDropDown(), "StatusId", "Name");
                location.AssignedTeamDropdown = new SelectList(lm.TeamDropDown(), "TeamId", "Name");
                location.AssignedUserDropdown = new SelectList(lm.UserDropDown(), "UserId", "Name");
                location.AccountsDropdown = new SelectList(lm.AccountDropDown(UserCompanyID), "AccountId", "Name");

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(location);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult DeviceLocationDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Device Location Detail
                LocationDto deviceLocation = lm.GetLocation(id);

                if (deviceLocation == null)    //If Device Location detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(deviceLocation);
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
                //Getting location  the user ID
                var location = lm.GetLocation(id);
                lm.SaveLocation(location, null,null, true, true);
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