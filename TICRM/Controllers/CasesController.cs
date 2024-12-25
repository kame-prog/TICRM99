using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.DTOs;
using TICRM.BuisnessLayer;
using Microsoft.AspNet.Identity;
using System.Text;
using Newtonsoft.Json;
using TICRM.Helper;
using TICRM.CRMFilters;
using System.Net.Mail;

namespace TICRM.Controllers
{

    /************Cases Controller************
   Class [CasesController] 
   ||  Author:  [Akhtar Zaman]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Cases like, 
   ||             Indexing the case, creating, creating notes for a case, resolving a case and
   ||             Creating it from right side panel]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [18/09/2020     Created    Akhtar]
    ********************************************/

    public class CasesController : BaseController
    {

        private CaseManager cm = new CaseManager();
        /// <summary>
        /// Indexes Cases.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Details for the specified case.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseDto casedto = cm.GetCaseonId(id);
            if (casedto == null)
            {
                return HttpNotFound();
            }
            return View(casedto);
        }

        /// <summary>
        /// Create page for case.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            CaseDto casedto = new CaseDto();
            casedto.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
            casedto.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
            casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
            casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
            casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
            casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "ContactId", "Name");
            casedto.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
            casedto.RelatedToIdDropdown = new SelectList("");

            
            return View(casedto);
        }
        /// <summary>
        /// Creates the specified casedto.
        /// </summary>
        /// <param name="casedto">The casedto.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CaseDto casedto)
        {
            if (ModelState.IsValid)
            {
                Guid CaseId = Guid.NewGuid();
                //string CurrentUserId = User.Identity.GetUserId();
                casedto.CaseId = CaseId;
                cm.SaveCase(casedto, null);
                return RedirectToAction("Index");
            }

            casedto.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
            casedto.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
            casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
            casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
            casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
            casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "Id", "Name");
            casedto.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");

            return View(casedto);
        }

        /// <summary>
        /// Edit page for a specific case.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseDto casedto = cm.GetCaseonId(id);
            if (casedto == null)
            {
                return HttpNotFound();
            }
            casedto.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
            casedto.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
            casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
            casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
            casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
            casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "Id", "Name");
            casedto.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");

            return View(casedto);
        }

        /// <summary>
        /// Edits the specified casedto.
        /// </summary>
        /// <param name="casedto">The casedto.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CaseDto casedto)
        {
            if (ModelState.IsValid)
            {
                string CurrentUserId = User.Identity.GetUserId();
                cm.SaveCase(casedto, CurrentUserId, true, false);
                return RedirectToAction("Index");
            }
            casedto.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
            casedto.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
            casedto.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
            casedto.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
            casedto.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
            casedto.ContactsDropdown = new SelectList(cm.GetContactList(), "Id", "Name");
            casedto.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");

            return View(casedto);
        }

        /// <summary>
        /// Delete Page for a specific case
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseDto caseDelete = cm.GetCaseonId(id);

            if (caseDelete == null)
            {
                return HttpNotFound();
            }

            return View(caseDelete);
        }

        /// <summary>
        /// Deleting a specific case.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CaseDto caseDelete = cm.GetCaseonId(id);
           
            string CurrentUserId = User.Identity.GetUserId();
            cm.SaveCase(caseDelete, CurrentUserId, true, true);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the cases list for datatables.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns></returns>
        public string GetCasesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<CaseDto> obj = cm.GetCasesList(sEcho, iDisplayStart, iDisplayLength, sSearch);


            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CaseTitle).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CaseStatusDto.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CaseStatusDto.Name).ToList();
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
                        obj = obj.OrderBy(x => x.CaseTypeDto.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CaseTypeDto.Name).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.RelatedTo).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.RelatedTo).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.ContactDto.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.ContactDto.Name).ToList();
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

        /// <summary>
        /// Partial details on identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                ActivityManager am = new ActivityManager();
                NotesManager nm = new NotesManager();
                CaseDto cases = cm.GetCaseonId(id);
                cases.CasesActivities = am.GetCasesActivities(id);
                cases.CaseNotes = nm.GetCaseNotes(id);
                cases.ActivityType = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                cases.StatusDropdown = new SelectList(cm.Status, "StatusId", "Name");
                cases.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
                cases.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                cases.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                cases.ResulutionTypeDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                return PartialView("PartialCaseDetails", cases);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partials the details on identifier user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnIdUser(Guid? id)
        {
            try
            {
                ActivityManager am = new ActivityManager();
                NotesManager nm = new NotesManager();
                CaseDto cases = cm.GetCaseonId(id);
                cases.CasesActivities = am.GetCasesActivities(id);
                cases.CaseNotes = nm.GetCaseNotes(id);
                cases.ActivityType = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                return PartialView("PartialCaseDetailsUser", cases);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                ActivityManager am = new ActivityManager();
                NotesManager nm = new NotesManager();
                CaseDto cases = cm.GetCaseonId(id);
                cases.CasesActivities = am.GetCasesActivities(id);
                cases.CaseNotes = nm.GetCaseNotes(id);
                cases.ActivityType = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                cases.StatusDropdown = new SelectList(cm.Status, "StatusId", "Name");
                cases.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
                cases.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                cases.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                cases.ResulutionTypeDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                return PartialView("_PartialCaseDelete", cases);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the note for a specific case.
        /// </summary>
        /// <param name="Note">The note.</param>
        /// <param name="Relatedto">The relatedto.</param>
        /// <param name="RelatedToId">The related to identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ActionResult CreateNote(String Note, String Relatedto, String RelatedToId)
        {
            try
            {
                NotesManager nm = new NotesManager();
                NotesDto notesdto = new NotesDto();

                string CurrentUserId = User.Identity.GetUserName();
                notesdto.CreatedBy = CurrentUserId;
                notesdto.Note1 = Note;
                notesdto.RelatedTo = Relatedto;
                notesdto.RelatedToId = Guid.Parse(RelatedToId);
                nm.SaveNote(notesdto);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Resolves the case.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <param name="Resolution">The resolution.</param>
        /// <param name="ResolutionType">Type of the resolution.</param>
        /// <param name="TotalTime">The total time.</param>
        /// <param name="BillableTime">The billable time.</param>
        /// <param name="Remarks">The remarks.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [CasesActionFilter]
        public ActionResult ResolveCase(String CaseId, String Resolution, String ResolutionType, String TotalTime, String BillableTime, String Remarks)
        {
            try
            {
                CaseDto cases = cm.GetCaseonId(Guid.Parse(CaseId));
                cases.Resolution = Resolution;
                cases.TotalTime = Convert.ToInt16(TotalTime);
                cases.BillableTime = Convert.ToInt16(BillableTime);
                cases.Remarks = Remarks;
                cases.ResolutionType = Guid.Parse(ResolutionType);
                cases.CaseStatusId = cm.GetCaseStatus("Resolved").CaseStatusId;
                cm.SaveCase(cases, "", true, false);
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
            
        }
        /// <summary>
        /// Resolves the case from user.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <param name="Resolution">The resolution.</param>
        /// <param name="ResolutionType">Type of the resolution.</param>
        /// <param name="TotalTime">The total time.</param>
        /// <param name="BillableTime">The billable time.</param>
        /// <param name="Remarks">The remarks.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [CasesActionFilter]
        public ActionResult ResolveCaseUser(String CaseId, String Resolution, String ResolutionType, String TotalTime, String BillableTime, String Remarks, String UserId)
        {
            try
            {
                UserManager userManager = new UserManager();
                CaseDto cases = cm.GetCaseonId(Guid.Parse(CaseId));
                cases.Resolution = Resolution;
                cases.TotalTime = Convert.ToInt16(TotalTime);
                cases.BillableTime = Convert.ToInt16(BillableTime);
                cases.Remarks = Remarks;
                cases.ResolutionType = Guid.Parse(ResolutionType);
                cases.CaseStatusId = cm.GetCaseStatus("Resolved").CaseStatusId;
                cm.SaveCase(cases, "", true, false);

                UserDto user = userManager.GetUser(Guid.Parse(UserId));
                user.AssignedItem = null;
                user.AssignedItemId = null;
                user.AssignedItemTime = null;
                user.IsAssigned = false;
                user.StatusId = cm.GetStatus("Active").StatusId;
                bool isupdated = userManager.SaveUser(user, true, false);
               
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Schedules the case.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <param name="CaseDate">The case date.</param>
        /// <param name="CaseTime">The case time.</param>
        /// <param name="CaseTeam">The case team.</param>
        /// <param name="CaseUser">The case user.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ActionResult ScheduleCase(String CaseId, String CaseDate, String CaseTime, String CaseTeam, String CaseUser)
        {
            try
            {
                UserManager um = new UserManager();
                string schedule = CaseDate + " " + CaseTime;
                CaseDto cases = cm.GetCaseonId(Guid.Parse(CaseId));
                cases.IsScheduled = true;
                cases.Scheduling = Convert.ToDateTime(schedule);
                cases.AssignedTeam = Guid.Parse(CaseTeam);
                cases.AssignedUser = Guid.Parse(CaseUser);
                cm.SaveCase(cases, "", true, false);

                UserDto user = um.GetUser(Guid.Parse(CaseUser));
                user.AssignedItem = EntityTypes.Cases.ToString();
                user.IsAssigned = true;
                user.AssignedItemId = Guid.Parse(CaseId);
                user.AssignedItemTime = DateTime.Now;
                user.StatusId = cm.GetStatus("Active").StatusId;
                bool userstatus = um.SaveUser(user, true, false);
                if(userstatus == true)
                {
                    string smtpAddress = "smtp.gmail.com";
                    int portNumber = 587;
                    bool enableSSL = true;
                    string emailFromAddress = "swuichcrm@gmail.com"; //Sender Email Address  
                    string password = "Swuich@123"; //Sender Password  
                    string emailToAddress = user.Email; //Receiver Email Address  
                    string subject = "Case Assignment";
                    string Body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                    Body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                    Body += "</HEAD><BODY><DIV>";
                    Body += "<p>A new case has been assigned to you</p><br/>";
                    Body += "<h1>" + cases.CaseTitle + "</h1><br/>";
                    Body += "<p> Respond to the case in given time else it will be automatucally awarded to someone else</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
                    Body += "</DIV></BODY></HTML>";
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFromAddress);
                        mail.To.Add(emailToAddress);
                        mail.Subject = subject;
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                            smtp.EnableSsl = enableSSL;
                            smtp.Send(mail);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Creates the case.
        /// </summary>
        /// <param name="CaseTitle">The case title.</param>
        /// <param name="Description">The description.</param>
        /// <param name="AssignedUser">The assigned user.</param>
        /// <param name="AssignedTeam">The assigned team.</param>
        /// <param name="CaseType">Type of the case.</param>
        /// <param name="Contact">The contact.</param>
        /// <param name="Relatedto">The relatedto.</param>
        /// <param name="RelatedToId">The related to identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonResult CreateCase(String CaseTitle, String Description, String AssignedUser, String AssignedTeam, String CaseType, String Contact, String Relatedto, String RelatedToId)
        {
            try
            {
                CaseDto cases = new CaseDto();
                cases.CaseId = Guid.NewGuid();
                cases.CaseTitle = CaseTitle;
                cases.Description = Description;
                cases.CaseTypeId = Guid.Parse(CaseType);
                if (Contact != null)
                {
                    cases.ContactId = Convert.ToInt16(Contact);
                }
                cases.AssignedTeam = Guid.Parse(AssignedTeam);
                cases.AssignedUser = Guid.Parse(AssignedUser);
                cases.RelatedTo = Relatedto;
                cases.RelatedToId = Guid.Parse(RelatedToId);
                cm.SaveCase(cases, "", false, false);
                return Json(cases.CaseId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
            
        }
        /// <summary>
        /// Res the activate case.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonResult ReActivateCase(String CaseId)
        {
            try
            {
                cm.ReActivateCase(Guid.Parse(CaseId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
            return null;
        }
    }
}
