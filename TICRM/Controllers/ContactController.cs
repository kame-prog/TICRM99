using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Contact Controller************
   Class [ContactController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Contact like, 
   ||             navigating to the pages, getting associated modules for specific Contact]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
   ||                  
    ********************************************/
    public class ContactController : BaseController
    {
        private ContactManager cm = new ContactManager();
        private CaseManager casem = new CaseManager();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // executes before the action method execution 

        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        /// <summary>
        /// Details view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Details(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContactDto contact = cm.GetContact(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial Detail View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PartialDetailsOnId(long? id)
        {
            try
            {
                ContactDto contact = cm.GetContact(id);
                contact.ContactCases = casem.GetContactsCases(id);
                ViewBag.AssignedTeamDropdown = new SelectList(casem.Teams, "TeamId", "Name");
                ViewBag.AssignedUserDropdown = new SelectList(casem.Users, "UserId", "Name");
                ViewBag.CaseResolutionDropdown = new SelectList(casem.GetCaseResolutions(), "CaseResolutionType", "Name");
                ViewBag.CaseStatusDropdown = new SelectList(casem.GetCaseStatusDtos(), "CaseStatusId", "Name");
                ViewBag.CaseTypeDropdown = new SelectList(casem.GetCaseTypeDtos(), "CaseTypeId", "Name");
                ViewBag.ContactsDropdown = new SelectList(casem.GetContactList(), "ContactId", "Name");
                return PartialView("_PartialContactDetails", contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Get contacts and return on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the contacts list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetContactsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortColumnDir = Request["sSortDir_0"];
                sortColumnDir.ToLower();

                var obj = cm.GetContactsList(sEcho, iDisplayStart, iDisplayLength, sSearch);


                switch (sortColumnIndex)
                {

                    case 0:
                        if (sortColumnDir == "asc")
                        {
                            //obj = obj.OrderBy(x => x.PropertyName).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.Name).ToList();
                        }
                        break;
                    case 1:
                        if (sortColumnDir == "asc")
                        {
                            obj = obj.OrderBy(x => x.Phone).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.Phone).ToList();
                        }
                        break;
                    case 2:
                        if (sortColumnDir == "asc")
                        {
                            obj = obj.OrderBy(x => x.Email).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.Email).ToList();
                        }
                        break;
                    case 3:
                        if (sortColumnDir == "asc")
                        {
                            obj = obj.OrderBy(x => x.Address).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.Address).ToList();
                        }
                        break;
                    case 4:
                        if (sortColumnDir == "asc")
                        {
                            obj = obj.OrderBy(x => x.AssignedTeam).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.AssignedTeam).ToList();
                        }
                        break;
                    case 5:
                        if (sortColumnDir == "asc")
                        {
                            obj = obj.OrderBy(x => x.AssignedUser).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.AssignedUser).ToList();
                        }
                        break;
                    case 6:
                        if (sortColumnDir == "asc")
                        {
                            obj = obj.OrderBy(x => x.Status.Name).ToList();
                        }
                        else
                        {
                            obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                        }
                        break;

                }

                int totalRecord = cm.GetTotalCount();

                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("{");
                sb.Append("\"sEcho\": ");
                sb.Append(sEcho);
                sb.Append(",");
                sb.Append("\"iTotalRecords\": ");
                sb.Append(totalRecord);
                sb.Append(",");
                sb.Append("\"iTotalDisplayRecords\": ");
                sb.Append(totalRecord);
                sb.Append(",");
                sb.Append("\"aaData\": ");
                sb.Append(JsonConvert.SerializeObject(obj));
                sb.Append("}");
                return sb.ToString();
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
                ContactDto contact = new ContactDto();
                contact.AccountsDropdown = new SelectList(cm.Accounts, "AccountId", "Name");
                contact.StatusDropdown = new SelectList(cm.Status, "StatusId", "Name");
                contact.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
                contact.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                return View(contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactDto contact, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    cm.SaveContact(contact, false, false, CurrentUserId);
                    return RedirectToAction("Index");
                }

                contact.AccountsDropdown = new SelectList(cm.Accounts, "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.Status, "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name", contact.AssignedUser);

                return View(contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        // GET: contact/Edit/5
        public ActionResult Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ContactDto contact = cm.GetContact(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                contact.AccountsDropdown = new SelectList(cm.Accounts, "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.Status, "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name", contact.AssignedUser);

                return View(contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactDto contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();

                    cm.SaveContact(contact, true, false, CurrentUserId);
                    TempData["FormSubmissionMessage"] = "Form submitted successfully";
                    TempData["FormSubmissionStatus"] = "success";

                    return RedirectToAction("Index");
                }
                contact.AccountsDropdown = new SelectList(cm.Accounts, "AccountId", "Name", contact.AccountId);
                contact.StatusDropdown = new SelectList(cm.Status, "StatusId", "Name", contact.StatusId);
                contact.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name", contact.AssignedTeam);
                contact.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name", contact.AssignedUser);

                return View(contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial delete view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(long? id)
        {
            try
            {
                ContactDto contact = cm.GetContact(id);
                return PartialView("_PartialContactDelete", contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContactDto contact = cm.GetContact(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
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
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactDto contact = cm.GetContact(id);
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    //soft delete for contact
                    cm.SaveContact(contact, true, true, CurrentUserId);

                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates contact from account.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Email">The email.</param>
        /// <param name="Phone">The phone.</param>
        /// <param name="Add">The add.</param>
        /// <param name="Create">The create.</param>
        /// <param name="Update">The update.</param>
        /// <param name="CUser">The c user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Status">The status.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult CreateCfromAccount(String Name, String Email, String Phone, String Add, String Create, String Update, String CUser, String Team, String Status, String AccID)
        {

            
            //string CurrentUserId = User.Identity.GetUserId();
            try
            {
                ContactDto conAcc = new ContactDto();
                conAcc.Name = Name;
                conAcc.Email = Email;
                conAcc.Phone = Phone;
                conAcc.Address = Add;
                conAcc.CreatedBy = Create;
                conAcc.UpdatedBy = Update;
                conAcc.AssignedUser = Guid.Parse(CUser);
                conAcc.AssignedTeam = Guid.Parse(Team);
                conAcc.StatusId = Guid.Parse(Status);
                conAcc.StatusId = Guid.Parse(Status);
                conAcc.AccountId = Guid.Parse(AccID);
                cm.SaveContact(conAcc, false, false, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            return null;
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