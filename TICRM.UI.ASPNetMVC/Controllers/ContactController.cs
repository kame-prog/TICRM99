    using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DAL;
using TICRM.DTOs;
using log4net;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class ContactController : Controller
    {
        private ContactManager cm = new ContactManager();
        protected CRMEntities dbEnt = new CRMEntities();
        private AddressManager addressManager = new AddressManager();

        // GET: Contact
        public ActionResult Index()
        {
            try
            {
                //Get Current user ID
                string CurrentUserId = User.Identity.GetUserId();   
                
                //Get current user role and also convert it into string.
                string UserRole = Convert.ToString(Session["UserRole"]);        

                //Get current user company and convert it into String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   

                //Retrive the Contacts list by using the GetContacts() method.
                List<ContactDto> contacts = cm.GetContacts(CurrentUserId, UserRole,UserCompanyID);

                //Pass the list of contacts to the view.
                return View(contacts);
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

                //Create the object of the ContactDto class.
                ContactDto contact = new ContactDto();

                //Populate the values to the dropdowns.
                contact.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                contact.AccountsDropdown = new SelectList(cm.AccountDropDown(UserCompanyID), "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.StatusDropDown(), "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name", contact.AssignedUser);
                
                //Return value to the view in form of contact object
                return View(contact);
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
        /// Creates the specified contact.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactDto contact)
        {
            try
            {
                //Get current company ID and convert it into the String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                // Check if the model is valid.
                if (ModelState.IsValid)
                {
                    // Check if the contact already exists
                    var AlreadyExists = dbEnt.Contacts.Any(x => x.IsDeleted != true && x.Email == contact.Email);
                    if (AlreadyExists)
                    {
                        //Set exist messge in the ViewBag
                        ViewBag.exist = ExistMessage.ContactExist;

                        // Populate dropdowns with the relevant value
                        contact.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                        contact.AccountsDropdown = new SelectList(cm.AccountDropDown(UserCompanyID), "AccountId", "Name", contact.AccountId);
                        contact.StatusDropdown = new SelectList(cm.StatusDropDown(), "StatusId", "Name", contact.StatusId);
                        contact.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name", contact.AssignedTeam);
                        contact.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name", contact.AssignedUser);

                        //Return value on view in the form of contact object.
                        return View(contact);
                    }
                    else
                    {
                        //Get current user ID.
                        string CurrentUserId = User.Identity.GetUserId();

                        //string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID

                        // Save the contact using savecontact() method
                        var condition = cm.SaveContact(contact, CurrentUserId,UserCompanyID, false, false);
                        if (!condition)
                        {
                            //If condition is null return the Modelstate error.
                            ModelState.AddModelError("", WarningMessage.DataNotSaved);
                        }
                        else
                        {
                            //set success message in the TempData.
                            TempData["Success"] = SuccessMessage.ContactSubmit;

                            // Redirect to the Index action method
                            return RedirectToAction("Index");
                        }

                    }
                       
                }
                //Populate dropdown if the model state is not valid.
                contact.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                contact.AccountsDropdown = new SelectList(cm.AccountDropDown(UserCompanyID), "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.StatusDropDown(), "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name", contact.AssignedUser);

                //set Warning message in the TempData.
                TempData["Warning"] = WarningMessage.EnterField;

                //Return value to the view in form of contact object
                return View(contact);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the conatct  based on the provided id
                ContactDto contact = cm.GetContact(id);
                if (contact == null)
                {
                    // If the contact is null, return a "Not Found" status
                    return HttpNotFound();
                }
                //Get User company ID and convert it into the string.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                // Populate the dropdowns for the edit view
                contact.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                contact.AccountsDropdown = new SelectList(cm.AccountDropDown(UserCompanyID), "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.StatusDropDown(), "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name", contact.AssignedUser);

                // Return the view with the populated  object
                return View(contact);
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
        public ActionResult Edit(ContactDto contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the current user ID.
                    string CurrentUserId = User.Identity.GetUserId();

                    //Edit the contact using svecontact() method.
                    var condition = cm.SaveContact(contact, CurrentUserId,null, true, false);                    
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Set a success message in tempdata
                        TempData["Success"] = UpdateMessage.ContactUpdate;

                        //Redirect to the index action method
                        return RedirectToAction("Index");
                    }
                }
                //Get current  User Company id
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                //Populate values to the corresponding dropdown.
                contact.CountryDropdown = new SelectList(addressManager.CountryDropDown(), "Country_Name", "Country_Name");
                contact.AccountsDropdown = new SelectList(cm.AccountDropDown(UserCompanyID), "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.StatusDropDown(), "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.TeamDropDown(), "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.UserDropDown(), "UserId", "Name", contact.AssignedUser);

                //Set Warning message in the TempData
                TempData["Warning"] = WarningMessage.EnterField;

                //Return value to the view in form of contact object
                return View(contact);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult ContactDetail(int? id)
        {
            try
            {
                if (id == null)
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the contact detail  based on the provided id
                ContactDto contact = cm.GetContact(id);
                
                if (contact == null)  
                {
                    // If the Contact object is null, return a "Not Found" status
                    return HttpNotFound();
                }

                // Return the ContactDetail view with the Contact object
                return View(contact);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult Delete(long id)
        {
            try
            {
                // Retrieve the Contact to be deleted based on the provided id
                ContactDto contact = cm.GetContact(id);

                // Get current userid
                string CurrentUserId = User.Identity.GetUserId();

                 //Delete the Contact using SaveContact() method.
                cm.SaveContact(contact, CurrentUserId,null, true, true);

                //Redirect ti the Index Action method.
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