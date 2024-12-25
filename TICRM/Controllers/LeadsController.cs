
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.CRMFilters;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Leads Controller************
    Class [LeadsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with Leads like, 
    ||             navigating to the pages, getting associated modules for specific Lead]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||  Changes Made:   [20/08/2020     Added Server Side Processing For DataTables     Sikandar Mustafa]
    ||  Changes Made:   [07/09/2020     Added Opportunity creation method on qualifying a lead     Akhtar Zaman]
    ||                  
     ********************************************/

    
    public class LeadsController : BaseController
    {
       
        private LeadManager lm = new LeadManager();
        private OpportunityManager om = new OpportunityManager();
        private AccountManager am = new AccountManager();
        private ContactManager cm = new ContactManager();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Index view.
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
        /// Gets the leads list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetLeadsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<LeadDto> obj = lm.GetLeadsList(sEcho, iDisplayStart, iDisplayLength, sSearch);

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
                        obj = obj.OrderBy(x => x.PhoneNumber).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.PhoneNumber).ToList();
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
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Address.Street1).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Address.Street1).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Industry.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Industry.Name).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.LeadSource.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.LeadSource.Name).ToList();
                    }
                    break;
                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.LeadType.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.LeadType.Name).ToList();
                    }
                    break;
                case 8:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;
                case 9:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                case 10:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.User.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.User.Name).ToList();
                    }
                    break;


            }
            int totalRecord = lm.GetTotalCount();

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


        /// <summary>
        /// Details view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LeadDto lead = lm.GetLead(id);
                if (lead == null)
                {
                    return HttpNotFound();
                }
                return View(lead);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Partial detail view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                LeadDto lead = lm.GetLead(id);
                return PartialView("_PartialLeadDetails", lead);
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
                LeadDto lead = new LeadDto();
                lead.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1");
                lead.IndustryDropdown = new SelectList(lm.Industries, "IndustryId", "Name");
                lead.LeadSourceDropdown = new SelectList(lm.LeadSources, "LeadSourceId", "Name");
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypes, "LeadTypeId", "Name");
                lead.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name");
                lead.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name");
                lead.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name");
                return View(lead);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Creates the specified lead.
        /// </summary>
        /// <param name="lead">The lead.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [LeadActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeadDto lead)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();

                    lm.SaveLead(lead, false, false, CurrentUserId);
                    return RedirectToAction("Index");
                }

                lead.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1", lead.AddressId);
                lead.IndustryDropdown = new SelectList(lm.Industries, "IndustryId", "Name", lead.IndustryId);
                lead.LeadSourceDropdown = new SelectList(lm.LeadSources, "LeadSourceId", "Name", lead.LeadSourceId);
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypes, "LeadTypeId", "Name", lead.LeadTypeId);
                lead.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name", lead.StatusId);
                lead.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name", lead.AssignedTeam);
                lead.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name", lead.AssignedUser);
                return View(lead);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edit view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                LeadDto lead = lm.GetLead(id);
                if (lead == null)
                {
                    return HttpNotFound();
                }
                lead.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1", lead.AddressId);
                lead.IndustryDropdown = new SelectList(lm.Industries, "IndustryId", "Name", lead.IndustryId);
                lead.LeadSourceDropdown = new SelectList(lm.LeadSources, "LeadSourceId", "Name", lead.LeadSourceId);
                lead.LeadTypeDropdown = new SelectList(lm.LeadTypes, "LeadTypeId", "Name", lead.LeadTypeId);
                lead.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name", lead.StatusId);
                lead.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name", lead.AssignedTeam);
                lead.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name", lead.AssignedUser);
                return View(lead);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Edits the specified lead.
        /// </summary>
        /// <param name="lead">The lead.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [LeadActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeadDto lead)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();

                    lm.SaveLead(lead, true, false, CurrentUserId);
                    TempData["FormSubmissionMessage"] = "Form submitted successfully";
                    TempData["FormSubmissionStatus"] = "success";

                    return RedirectToAction("Index");
                }
                ViewBag.AddressId = new SelectList(lm.Addresses, "AddressId", "Street1", lead.AddressId);
                ViewBag.IndustryId = new SelectList(lm.Industries, "IndustryId", "Name", lead.IndustryId);
                ViewBag.LeadSourceId = new SelectList(lm.LeadSources, "LeadSourceId", "Name", lead.LeadSourceId);
                ViewBag.LeadTypeId = new SelectList(lm.LeadTypes, "LeadTypeId", "Name", lead.LeadTypeId);
                ViewBag.StatusId = new SelectList(lm.Status, "StatusId", "Name", lead.StatusId);
                ViewBag.AssignedTeam = new SelectList(lm.Teams, "TeamId", "Name", lead.AssignedTeam);
                ViewBag.AssignedUser = new SelectList(lm.Users, "UserId", "Name", lead.AssignedUser);
                return View(lead);
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
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                LeadDto lead = lm.GetLead(id);
                return PartialView("_PartialLeadDelete", lead);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LeadDto lead = lm.GetLead(id);
                if (lead == null)
                {
                    return HttpNotFound();
                }
                return View(lead);
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
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LeadDto lead = lm.GetLead(id);
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    //soft delete for lead
                    lm.SaveLead(lead, true, true, CurrentUserId);

                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult QualifyLead(Guid id)
        {
            LeadDto lead = lm.GetLead(id);
            OpportunityDto opportunity = new OpportunityDto();
            AccountDto account = new AccountDto();
            ContactDto contact = new ContactDto();
            string CurrentUserId = User.Identity.GetUserId();

            account.AccountId = Guid.NewGuid();
            account.Name = lead.Name;
            account.ShippingAddress = lead.AddressId;
            account.BillingAddress = lead.AddressId;
            account.PhoneOffice = lead.PhoneNumber;
            account.Email = lead.Email;
            account.IndustryId = lead.IndustryId;
            account.AssignedTeam = lead.AssignedTeam;
            account.AssignedUser = lead.AssignedUser;
            account.CreatedBy = lead.CreatedBy;
            account.CreatedDate = lead.CreatedDate;
            account.UpdatedDate = lead.UpdatedDate;
            account.UpdatedBy = lead.UpdatedBy;
            account.StatusId = lead.StatusId;
            am.SaveAccount(account, CurrentUserId, false, false);

            contact.AccountId = account.AccountId;
            contact.Name = lead.Name;
            contact.AssignedTeam = lead.AssignedTeam;
            contact.AssignedUser = lead.AssignedUser;
            contact.CreatedBy = lead.CreatedBy;
            contact.CreatedDate = lead.CreatedDate;
            contact.UpdatedDate = lead.UpdatedDate;
            contact.UpdatedBy = lead.UpdatedBy;
            contact.StatusId = lead.StatusId;
            contact.Address = lead.AddressId.ToString();
            contact.Email = lead.Email;
            contact.Phone = lead.PhoneNumber;
            cm.SaveContact(contact, false, false, CurrentUserId);

            opportunity.Title = lead.Name;
            opportunity.Description = lead.Description;
            opportunity.AccountId = account.AccountId;
            opportunity.AssignedTeam = lead.AssignedTeam;
            opportunity.AssignedUser = lead.AssignedUser;
            opportunity.CreatedBy = lead.CreatedBy;
            opportunity.CreatedDate = lead.CreatedDate;
            opportunity.UpdatedDate = lead.UpdatedDate;
            opportunity.UpdatedBy = lead.UpdatedBy;
            opportunity.StatusId = lead.StatusId;
            opportunity.CurrencyId = Guid.Parse("2210d79a-d2cd-46f9-8ea4-0318e281e9e3");
            opportunity.OpportunityStageId = Guid.Parse("dee42c37-a1fb-4ff0-8e32-ceb9aa6d3396");
            opportunity.Amount = 123;
            om.SaveOpportunity(opportunity, CurrentUserId, false, false);

            return RedirectToAction("Edit", "Accounts", new { id = account.AccountId });
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
