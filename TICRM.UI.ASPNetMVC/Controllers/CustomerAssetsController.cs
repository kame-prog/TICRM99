using log4net;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DAL;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class CustomerAssetsController : Controller
    {
        CustomerAssetManager cam = new CustomerAssetManager();
        LocationManager locationManager = new LocationManager();
        protected CRMEntities dbEnt = new CRMEntities();

        // GET: CustomerAssets
        public ActionResult Index(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    //id --> Account ID,,,,When we come from account page to Customer asset page then this id use
                    List<CustomerAssetDto> customerassets = cam.GetCustomerAssets(id);

                    //Return the list of customer asset that's are related to the Account ID.
                    return View(customerassets);
                }
                else
                {
                    //Get Current User ID
                    string CurrentUserId = User.Identity.GetUserId();

                    //Get current User Role and convert it to a string
                    string UserRole = Convert.ToString(Session["UserRole"]);

                    //Get User Company ID and convert it to a string
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]);  
                    
                    //Retrive the all customer assets using GetCustomerAssests() method.
                    List<CustomerAssetDto> customerassets = cam.GetCustomerAssets(CurrentUserId, UserRole, UserCompanyID);

                    //Return value on the view in the form of customerassets.
                    return View(customerassets);
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
                //Get the User Company ID and convert it into the String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                //Create object of the CustomerAssetDto
                CustomerAssetDto customerAsset = new CustomerAssetDto();

                //Populate the values to the dropdowns.
                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CusAssTypeDropDown(), "CustomerAssetTypeId", "Name");
                customerAsset.StatusDropdown = new SelectList(cam.StatusDropDown(), "StatusId", "Name");
                customerAsset.AssignedTeamDropdown = new SelectList(cam.TeamDropDown(), "TeamId", "Name");
                customerAsset.AssignedUserDropdown = new SelectList(cam.UserDropDown(), "UserId", "Name");
                customerAsset.AccountsDropdown = new SelectList(cam.AccountDropDown(UserCompanyID), "AccountId", "Name");
                customerAsset.LocationDropdown = new SelectList(cam.LocationDropDown(UserCompanyID), "LocationId", "Name");

                //Return value to the view in form of customerAsset object
                return View(customerAsset);
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
        /// Creates the specified customer asset.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerAssetDto customerAsset)
        {
            try
            {
                //Get current company ID and convert it into the String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                // Check if the model is valid.
                if (ModelState.IsValid)
                {
                    // Check if the customer asset already exists
                    var AlreadyExists = dbEnt.CustomerAssets.Any(x => x.IsDeleted != true && x.SKU == customerAsset.SKU);
                    if (AlreadyExists)
                    {
                        //Set exist messge in the ViewBag
                        ViewBag.exist = ExistMessage.CustomerAssetExist;

                        // Populate dropdowns with the relevant value
                        customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CusAssTypeDropDown(), "CustomerAssetTypeId", "Name");
                        customerAsset.StatusDropdown = new SelectList(cam.StatusDropDown(), "StatusId", "Name");
                        customerAsset.AssignedTeamDropdown = new SelectList(cam.TeamDropDown(), "TeamId", "Name");
                        customerAsset.AssignedUserDropdown = new SelectList(cam.UserDropDown(), "UserId", "Name");
                        customerAsset.AccountsDropdown = new SelectList(cam.AccountDropDown(UserCompanyID), "AccountId", "Name");
                        customerAsset.LocationDropdown = new SelectList(cam.LocationDropDown(UserCompanyID), "LocationId", "Name");

                        //Return value on view in the form of customerAsset object.
                        return View(customerAsset);
                    }
                    else
                    {
                        // Get current userid
                        string CurrentUserId = User.Identity.GetUserId();              
                        //string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID

                        //Save the customer asset using SaveCustomerAsset() method.
                        bool condition = cam.SaveCustomerAsset(customerAsset, CurrentUserId, UserCompanyID);
                        if (!condition)
                        {
                            //If condition is null return the Modelstate error.
                            ModelState.AddModelError("",WarningMessage.DataNotSaved);
                        }
                        else
                        {
                            //set success message in the TempData.
                            TempData["Success"] = SuccessMessage.CustomerAssetSubmit;

                            // Redirect to the Index action method
                            return RedirectToAction("Index");
                        }
                    }

                }
                //Populate dropdown if the model state is not valid.
                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CusAssTypeDropDown(), "CustomerAssetTypeId", "Name");
                customerAsset.StatusDropdown = new SelectList(cam.StatusDropDown(), "StatusId", "Name");
                customerAsset.AssignedTeamDropdown = new SelectList(cam.TeamDropDown(), "TeamId", "Name");
                customerAsset.AssignedUserDropdown = new SelectList(cam.UserDropDown(), "UserId", "Name");
                customerAsset.AccountsDropdown = new SelectList(cam.AccountDropDown(UserCompanyID), "AccountId", "Name");
                customerAsset.LocationDropdown = new SelectList(cam.LocationDropDown(UserCompanyID), "LocationId", "Name");

                //set Warning message in the TempData.
                TempData["Warning"] = WarningMessage.EnterField;

                //Return value to the view in form of contact object
                return View(customerAsset);
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
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the customerAsset  based on the provided id
                var customerAsset = cam.GetCustomerAsset(id);
                if (customerAsset == null)
                {
                    // If the contact is null, return a "Not Found" status
                    return HttpNotFound();
                }

                //Get User company ID and convert it into the string.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                // Populate the dropdowns for the edit view
                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CusAssTypeDropDown(), "CustomerAssetTypeId", "Name");
                customerAsset.StatusDropdown = new SelectList(cam.StatusDropDown(), "StatusId", "Name");
                customerAsset.AssignedTeamDropdown = new SelectList(cam.TeamDropDown(), "TeamId", "Name");
                customerAsset.AssignedUserDropdown = new SelectList(cam.UserDropDown(), "UserId", "Name");
                customerAsset.AccountsDropdown = new SelectList(cam.AccountDropDown(UserCompanyID), "AccountId", "Name");
                customerAsset.LocationDropdown = new SelectList(cam.LocationDropDown(UserCompanyID), "LocationId", "Name");

                // Return the view with the populated object
                return View(customerAsset);


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
        public ActionResult Edit(CustomerAssetDto customerAsset)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the current user ID.
                    string CurrentUserId = User.Identity.GetUserId();

                    //Edit the customerAsset using SaveCustomerAsset() method.
                    bool condition = cam.SaveCustomerAsset(customerAsset, CurrentUserId, null, true);
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);                     
                    }
                    else
                    {
                        //Set a success message in tempdata
                        TempData["Success"] = UpdateMessage.CustomerAssetUpdate;

                        //Redirect to the index action method
                        return RedirectToAction("Index");
                    }
                }
                //Get current  User Company id
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);  

                //Populate values to the corresponding dropdown.
                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CusAssTypeDropDown(), "CustomerAssetTypeId", "Name");
                customerAsset.StatusDropdown = new SelectList(cam.StatusDropDown(), "StatusId", "Name");
                customerAsset.AssignedTeamDropdown = new SelectList(cam.TeamDropDown(), "TeamId", "Name");
                customerAsset.AssignedUserDropdown = new SelectList(cam.UserDropDown(), "UserId", "Name");
                customerAsset.AccountsDropdown = new SelectList(cam.AccountDropDown(UserCompanyID), "AccountId", "Name");
                customerAsset.LocationDropdown = new SelectList(cam.LocationDropDown(UserCompanyID), "LocationId", "Name");

                //Set Warning message in the TempData
                TempData["Warning"] = WarningMessage.EnterField;

                //Return value to the view with the populated object.
                return View(customerAsset);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult CustomerAssetDetail(Guid? id)
        {
            try
            {
                if (id == null) 
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the contact detail  based on the provided id
                CustomerAssetDto customerAsset = cam.GetCustomerAsset(id);

                if (customerAsset == null)  
                {
                    // If the Contact object is null, return a "Not Found" status
                    return HttpNotFound();
                }
                // Return the ContactDetail view with the Contact object
                return View(customerAsset);
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
                // Retrieve the customer asset to be deleted based on the provided id
                CustomerAssetDto customerAsset = cam.GetCustomerAsset(id);

                //Delete the CustomerAsset using SaveCustomerAsset() method.
                cam.SaveCustomerAsset(customerAsset, null,null, true, true);

                //Redirect to the Index Action method.
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

        public JsonResult GetAccCurrencyName(string value)
        {
            if (value != "")
            {
                var CurrencyName = dbEnt.sp_AccCurrencyName_Get(value).ToList();
                return Json(CurrencyName, JsonRequestBehavior.AllowGet);
            }
            return Json("");      
        }
    }
}