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
    public class ResourcesController : Controller
    {
        ResourceManager rm = new ResourceManager();

        // GET: Resources
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                //Show all Resources on listing page
                List<ResourceDto> resources = rm.GetResources(CurrentUserId, UserRole, UserCompanyID);
                return View(resources);
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
                //Bind dropdown values with create form
                ResourceDto resource = new ResourceDto();
                resource.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                return View(resource);
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
        /// new Resources validate it and creates a new Resources
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResourceDto resource)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                   /* string UserCompanyID = Convert.ToString(Session["UserCompany"]);*/ //Pass Company ID
                    //Resource save in DB 
                    bool condition = rm.SaveResource(resource, CurrentUserId, UserCompanyID, false,false);
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted, show successfully toaster on listing screen 
                        TempData["Success"] = SuccessMessage.ResourceSubmit;
                        return RedirectToAction("Index");
                    }
                }
                resource.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(resource);
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
                //Get Resource for Update from DB
                var resource = rm.GetResource(id);
                if (resource == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);
                //Bind dropdown values with edit form
                resource.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                return View(resource);
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
        public ActionResult Edit(ResourceDto resource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    //Resource update in DB 
                    bool condition = rm.SaveResource(resource, CurrentUserId,null, true,false);
                    //Check here data save in DB or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = UpdateMessage.ResourceUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);
                //Bind dropdown values with edit form
                resource.StatusDropdown = new SelectList(rm.StatusDropDown(), "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.TeamDropDown(), "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.UserDropDown(), "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(resource);
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
        ///  POST request for Delete Action, 
        /// Recieve confirmation for Resources Deletion and Delete.
        /// </summary>
        public ActionResult Delete(Guid id)
        {
            try
            {
                ResourceDto resource = rm.GetResource(id);
                //Soft delete the service call record
                string CurrentUserId = User.Identity.GetUserId();
                rm.SaveResource(resource, CurrentUserId,null, true, true);
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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}