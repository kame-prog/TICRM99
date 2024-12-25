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
    /************Opportunities Controller************
Class [OpportunitiesController] 
||  Author:  [Undefined]
||
||  Purpose:  [The class serves all the functionlities related with Opportunitieslike, 
||             navigating to the pages, getting associated modules for specific Opportunitie]
||
||  Inherits From:  [Controller]
||
||  Changes Made:   [10/08/2020     Added Comment block to this Class                           Sikandar Mustafa]
||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
||                  [20/08/2020     Added method for Server side pagination                     Akhtar Zaman]
||                  
********************************************/

   
    public class OpportunitiesController : BaseController
    {
        
        OpportunityManager om = new OpportunityManager();
        CaseManager cm = new CaseManager();

        /// <summary>
        /// Provide all Opportunities on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                List<OpportunityDto> Opp = om.GetOpportunities();
                return View(Opp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details of a Opportunity with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var opportunity = om.GetOpportunity(id);

                if (opportunity == null)
                {
                    return HttpNotFound();
                }
                return View(opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details on a partial view of a Opportunity with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var opportunity = om.GetOpportunity(id);
                opportunity.OpportunityCasesList = cm.GetOpportunityCases(id);
                ViewBag.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
                ViewBag.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                ViewBag.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                ViewBag.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
                ViewBag.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
                ViewBag.ContactsDropdown = new SelectList(cm.GetContactList(), "ContactId", "Name");
                return PartialView("_PartialRightSideDetail", opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details to delete on partail view 
        /// of a Opportunity with respect to id passed
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var opportunity = om.GetOpportunity(id);
                return PartialView("_PartialOpportunityDelete", opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new Opportunity.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                OpportunityDto opportunity = new OpportunityDto();
                opportunity.AccountsDropdown = new SelectList(om.Accounts, "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.Status, "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.Teams, "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.Users, "UserId", "Name");
                opportunity.CurrencyDropdown = new SelectList(om.Currencies, "CurrencyId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OpportunityStages, "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.Probabilities, "ProbabilityId", "Name");
                return View(opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request to create Opportunity, Receive object of,
        /// new Opportunity validate it and creates a new Opportunity.
        /// </summary>
        /// <param name="opportunity">The opportunity.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OpportunityDto opportunity, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    bool condition = om.SaveOpportunity(opportunity, CurrentUserId);
                    if (condition == true)
                    {
                        if (loc != "False")
                        {
                            Guid temp = new Guid(loc);
                            return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                        }
                        else
                            return RedirectToAction("Index");
                    }
                }

                opportunity.AccountsDropdown = new SelectList(om.Accounts, "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.Status, "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.Teams, "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.Users, "UserId", "Name");
                opportunity.CurrencyDropdown = new SelectList(om.Currencies, "CurrencyId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OpportunityStages, "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.Probabilities, "ProbabilityId", "Name"); 
               
                if (loc != "False")
                {
                    Guid temp = new Guid(loc);
                    return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                }
                else
                    return View(opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a Opportunity, 
        /// with request to Id passed to this action method.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var opportunity = om.GetOpportunity(id);
                if (opportunity == null)
                {
                    return HttpNotFound();
                }
                opportunity.AccountsDropdown = new SelectList(om.Accounts, "AccountId", "Name", opportunity.AccountId);
                opportunity.CurrencyDropdown = new SelectList(om.Currencies, "CurrencyId", "Name", opportunity.CurrencyId);
                opportunity.AssignedTeamDropdown = new SelectList(om.Teams, "TeamId", "Name", opportunity.AssignedTeam);
                opportunity.AssignedUserDropdown = new SelectList(om.Users, "UserId", "Name", opportunity.AssignedUser);
                opportunity.OpportunityStageDropdown = new SelectList(om.OpportunityStages, "OpportunityStageId", "Name", opportunity.OpportunityStageId);
                opportunity.ProbabilityDropdown = new SelectList(om.Probabilities, "ProbabilityId", "Name", opportunity.ProbabilityId);
                opportunity.StatusDropdown = new SelectList(om.Status, "StatusId", "Name", opportunity.StatusId);
                return View(opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for Opportunity and update specified Opportunity
        /// </summary>
        /// <param name="opportunity">The opportunity.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OpportunityDto opportunity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    bool condition = om.SaveOpportunity(opportunity, CurrentUserId, true);
                    if (condition == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                opportunity.AccountsDropdown = new SelectList(om.Accounts, "AccountId", "Name", opportunity.AccountId);
                opportunity.CurrencyDropdown = new SelectList(om.Currencies, "CurrencyId", "Name", opportunity.CurrencyId);
                opportunity.AssignedTeamDropdown = new SelectList(om.Teams, "TeamId", "Name", opportunity.AssignedTeam);
                opportunity.AssignedUserDropdown = new SelectList(om.Users, "UserId", "Name", opportunity.AssignedUser);
                opportunity.OpportunityStageDropdown = new SelectList(om.OpportunityStages, "OpportunityStageId", "Name", opportunity.OpportunityStageId);
                opportunity.ProbabilityDropdown = new SelectList(om.Probabilities, "ProbabilityId", "Name", opportunity.ProbabilityId);
                opportunity.StatusDropdown = new SelectList(om.Status, "StatusId", "Name", opportunity.StatusId);
                return View(opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Delete form with Opportunity details,
        /// delete with respect to id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var opportunity = om.GetOpportunity(id);
                if (opportunity == null)
                {
                    return HttpNotFound();
                }
                return View(opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Receive confirmation for Opportunity Deletion and Delete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var opportunity = om.GetOpportunity(id);
                // pass current userid
                string CurrentUserId = User.Identity.GetUserId();
                om.SaveOpportunity(opportunity, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the Opportunity from account Details page.
        /// </summary>
        /// <param name="Amount">The amount.</param>
        /// <param name="Desc">The desc.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OUser">The o user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Prob">The prob.</param>
        /// <param name="Stage">The stage.</param>
        /// <param name="Curr">The curr.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <param name="Latitude">The latitude.</param>
        /// <param name="Longitude">The longitude.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult CreateOfromAccount(String Amount, String Desc, String Title, String OUser, String Team, String Status, String Prob, String Stage, String Curr, String AccID,String Latitude, String Longitude)
        {
            try
            {
                OpportunityDto opp = new OpportunityDto();
                opp.AccountId = Guid.Parse(AccID);
                opp.Amount = Convert.ToDecimal(Amount);
                opp.Description = Desc;
                opp.Title = Title;
                opp.AssignedUser = Guid.Parse(OUser);
                opp.AssignedTeam = Guid.Parse(Team);
                opp.StatusId = Guid.Parse(Status);
                opp.OpportunityStageId = Guid.Parse(Stage);
                opp.ProbabilityId = Guid.Parse(Prob);
                opp.CurrencyId = Guid.Parse(Curr);
                opp.Latitude = Convert.ToDecimal(Latitude);
                opp.Longitude = Convert.ToDecimal(Longitude);
                string CurrentUserId = User.Identity.GetUserId();
                bool condition = om.SaveOpportunity(opp, CurrentUserId);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the Opportunity counts.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetOppCounts()
        {
            try
            {
                //var res = om.GetOpportunitiesCount().Lost;
                //var sres = om.GetOpportunitiesCount().Open;
                //var rdes = om.GetOpportunitiesCount().Lostwon;
                //string JSON = JsonConvert.SerializeObject(om.GetOpportunitiesCount());
                //string json = JsonConvert.SerializeObject(list);

                return Json(om.GetOpportunitiesCount(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }


        /// <summary>
        /// Gets all opportunities longitude & latitiude.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetAllOpportunitiesLongLat()
        {
            try
            {
                return Json(om.GetOpportunities(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Get opportunities list data for pagination.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetopportunitiesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            var obj = om.GetopportunitiesList(sEcho, iDisplayStart, iDisplayLength, sSearch);


            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Title).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Account).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Account).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Currency.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Currency.Name).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.User.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.User.Name).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.OpportunityStage.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.OpportunityStage.Name).ToList();
                    }
                    break;
                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Pobability.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Pobability.Name).ToList();
                    }
                    break;

                default:
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

            int totalRecord = om.GetTotalCount();

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
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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
