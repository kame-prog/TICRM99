using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /****************************************************************************************
    ||  Class [WorkFlowActivityManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Sending emails, Creating meetings, creating notes, creating alerts, creating
    ||             workflow reports, run workflow mappings and updaing workflow mappings]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class WorkFlowActivityManager : BaseManager
    {
        private EntityTypes _obj;
        public WorkFlowActivityManager(EntityTypes obj, bool isPreEvent)
        {
            _obj = obj;
        }
        /// <summary>
        /// Execute work flows 
        /// </summary>
        public void ExecuteWorkFlow()
        {
            InsertEventLog("ExecuteWorkFlow", EventType.Log, EventColor.yellow, "Enter","TICRM.BusinessLayer.WorkFlowActivityManager.ExecuteWorkFlow", "");
            List<WorkFlow> workFlows = dbEnt.WorkFlows.Where(x => x.TargetOn == _obj.ToString() && x.TriggerIn <= DateTime.Now && x.TriggerOut >= DateTime.Now).OrderBy(x => x.Priority).ToList();

            if (_obj == EntityTypes.Lead)
            {
                InsertEventLog("ExecuteWorkFlow", EventType.Log, EventColor.yellow, "work flow activity running on Lead", "TICRM.BuisnessLayer.WorkFlowActivityManager.ExecuteWorkFlow", "");

                #region Leads
                Lead leadObject = HttpContext.Current.Session["LeadObject"] as Lead;
                if (leadObject == null) { return; }

                //TODO: get all work flows of account from database based on prioity 
                List<WorkFlowDTO> GetLeadList = new List<WorkFlowDTO>();

                //TODO: Get actionable items and create actionalble items object 
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    //TODO: perform the operaion
                    if (WFActionConstant.Email == item.Action)
                    {


                        List<string> emails = new List<string>();
                        string AssignedUserEmail = dbEnt.Users.FirstOrDefault(x => x.UserId == leadObject.AssignedUser).Email;
                        emails.Add(AssignedUserEmail);
                        emails.Add(leadObject.Email);
                        //emails.Add("aqil@techimplement.com");

                        var data = from tu in dbEnt.TeamUsers
                                   join u in dbEnt.Users on tu.TeamUserId equals u.UserId
                                   where tu.TeamId == leadObject.AssignedTeam
                                   select new { u.Email, u.Name };

                        foreach (var item1 in data)
                        {
                            emails.Add(item1.Email);
                        }

                        string status = SendEmail(emails,item.WorkFlowId);

                        WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                        workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                        workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                        workFlowReportDTO.Action = item.Action;
                        workFlowReportDTO.WorkFlowActionStatus = status;
                        workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                        workFlowReportDTO.AppliedTo = item.AppliedTo;
                        workFlowReportDTO.CreatedDate = DateTime.Now;
                        workFlowReportDTO.CreatedBy = leadObject.CreatedBy;
                        workFlowReportDTO.IsDeleted = false;
                        workFlowReportDTO.Priority = item.Priority;
                        workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                        workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);

                        CreateWorkFlowReport(workFlowReportDTO);


                    }
                    else if (WFActionConstant.Meeting == item.Action)
                    {

                    }
                    else if (WFActionConstant.Alert == item.Action)
                    {

                    }
                    else if (WFActionConstant.Note == item.Action)
                    {

                    }
                    else if (WFActionConstant.Account == item.Action)
                    {

                    }
                    string workflowmappingStatus = RunWorkFlowMapping(item.WorkFlowId);


                }



                #endregion

            }
            else if (_obj == EntityTypes.Account)
            {

                #region Account
                InsertEventLog("ExecuteWorkFlow", EventType.Log, EventColor.yellow, "work flow activity running on Account", "TICRM.BuisnessLayer.WorkFlowActivityManager.ExecuteWorkFlow", "");

                Account accountObject = HttpContext.Current.Session["AccountObject"] as Account;
                if (accountObject == null) { return; }

                //TODO: Get actionable items and create actionalble items object 
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    //TODO: perform the operaion
                    if (WFActionConstant.Email == item.Action)
                    {

                        List<string> emails = new List<string>();
                        string AssignedUserEmail = dbEnt.Users.FirstOrDefault(x => x.UserId == accountObject.AssignedUser).Email;
                        emails.Add(AssignedUserEmail);
                        emails.Add(accountObject.Email);
                        emails.Add("aqil@techimplement.com");

                        var data = from tu in dbEnt.TeamUsers
                                   join u in dbEnt.Users on tu.TeamUserId equals u.UserId
                                   where tu.TeamId == accountObject.AssignedTeam
                                   select new { u.Email, u.Name };

                        foreach (var item1 in data)
                        {
                            emails.Add(item1.Email);
                        }

                        string status = SendEmail(emails,item.WorkFlowId);

                        WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                        workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                        workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                        workFlowReportDTO.Action = item.Action;
                        workFlowReportDTO.WorkFlowActionStatus = status;
                        workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                        workFlowReportDTO.AppliedTo = item.AppliedTo;
                        workFlowReportDTO.CreatedDate = DateTime.Now;
                        workFlowReportDTO.CreatedBy = accountObject.CreatedBy;
                        workFlowReportDTO.IsDeleted = false;
                        workFlowReportDTO.Priority = item.Priority;
                        workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                        workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);
                    

                        CreateWorkFlowReport(workFlowReportDTO);

                    }
                    else if (WFActionConstant.Meeting == item.Action)
                    {

                        MeetingActionDTO meeting = new MeetingActionDTO();

                        meeting.Body = "new account Meeting is Ready";
                        meeting.Email = "aqil@techimplement.com";
                        meeting.Location = "tech implement office";
                        meeting.Name = "Account Meeting";
                        meeting.StartDate = DateTime.Now.AddHours(6);
                        meeting.EndDate = DateTime.Now.AddHours(7);
                        string status = CreateMeeting(meeting);


                        WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                        workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                        workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                        workFlowReportDTO.Action = item.Action;
                        workFlowReportDTO.WorkFlowActionStatus = status;
                        workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                        workFlowReportDTO.AppliedTo = item.AppliedTo;
                        workFlowReportDTO.CreatedDate = DateTime.Now;
                        workFlowReportDTO.CreatedBy = accountObject.CreatedBy;
                        workFlowReportDTO.IsDeleted = false;
                        workFlowReportDTO.Priority = item.Priority;
                        workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                        workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);

                        CreateWorkFlowReport(workFlowReportDTO);
                    }
                    else if (WFActionConstant.Alert == item.Action)
                    {

                    }
                    else if (WFActionConstant.Note == item.Action)
                    {

                    }
                    else if (WFActionConstant.Account == item.Action)
                    {

                    }
                }
                #endregion

            }
            else if (_obj == EntityTypes.Oppertunity)
            {
                #region Oppertunity
                InsertEventLog("ExecuteWorkFlow", EventType.Log, EventColor.yellow, "work flow activity running on Oppertunity", "TICRM.BuisnessLayer.WorkFlowActivityManager.ExecuteWorkFlow", "");
                Opportunity opportunityObject = HttpContext.Current.Session["OpportunityObject"] as Opportunity;
                if (opportunityObject == null) { return; }
                //TODO: Get actionable items and create actionalble items object 
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    //TODO: perform the operaion
                    if (WFActionConstant.Email == item.Action)
                    {

                        List<string> emails = new List<string>();
                        string AssignedUserEmail = dbEnt.Users.FirstOrDefault(x => x.UserId == opportunityObject.AssignedUser).Email;
                        emails.Add(AssignedUserEmail);
                        emails.Add("aqil@techimplement.com");

                        var data = from tu in dbEnt.TeamUsers
                                   join u in dbEnt.Users on tu.TeamUserId equals u.UserId
                                   where tu.TeamId == opportunityObject.AssignedTeam
                                   select new { u.Email, u.Name };

                        foreach (var item1 in data)
                        {
                            emails.Add(item1.Email);
                        }

                        string status = SendEmail(emails, item.WorkFlowId);

                        WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                        workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                        workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                        workFlowReportDTO.Action = item.Action;
                        workFlowReportDTO.WorkFlowActionStatus = status;
                        workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                        workFlowReportDTO.AppliedTo = item.AppliedTo;
                        workFlowReportDTO.CreatedDate = DateTime.Now;
                        workFlowReportDTO.CreatedBy = opportunityObject.CreatedBy;
                        workFlowReportDTO.IsDeleted = false;
                        workFlowReportDTO.Priority = item.Priority;
                        workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                        workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);

                        CreateWorkFlowReport(workFlowReportDTO);

                    }
                    else if (WFActionConstant.Meeting == item.Action)
                    {

                        MeetingActionDTO meeting = new MeetingActionDTO();

                        meeting.Body = "new Oppertunity Meeting is Ready";
                        meeting.Email = "aqil@techimplement.com";
                        meeting.Location = "tech implement office";
                        meeting.Name = "Oppertunity Meeting";
                        meeting.StartDate = DateTime.Now.AddHours(6);
                        meeting.EndDate = DateTime.Now.AddHours(7);
                        string status = CreateMeeting(meeting);

                        WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                        workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                        workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                        workFlowReportDTO.Action = item.Action;
                        workFlowReportDTO.WorkFlowActionStatus = status;
                        workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                        workFlowReportDTO.AppliedTo = item.AppliedTo;
                        workFlowReportDTO.CreatedDate = DateTime.Now;
                        workFlowReportDTO.CreatedBy = opportunityObject.CreatedBy;
                        workFlowReportDTO.IsDeleted = false;
                        workFlowReportDTO.Priority = item.Priority;
                        workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                        workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);

                        CreateWorkFlowReport(workFlowReportDTO);
                    }
                    else if (WFActionConstant.Alert == item.Action)
                    {

                    }
                    else if (WFActionConstant.Note == item.Action)
                    {

                    }
                    else if (WFActionConstant.Account == item.Action)
                    {

                    }
                }

                #endregion
            }

            else if(_obj == EntityTypes.Device)
            {
                #region Device
                InsertEventLog("ExecuteWorkFlow", EventType.Log, EventColor.yellow, "work flow activity running on Device", "TICRM.BuisnessLayer.WorkFlowActivityManager.ExecuteWorkFlow", "");
                Device deviceObj = HttpContext.Current.Session["DeiveObj"] as Device;
                if(deviceObj == null) { return; }
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    if(WFActionConstant.Email == item.Action)
                    {
                        List<string> emails = new List<string>();
                        string AssignedUserEmail = dbEnt.Users.FirstOrDefault(x => x.UserId == deviceObj.AssignedUser).Email;
                        emails.Add(AssignedUserEmail);
                        emails.Add("aqil@techimplement.com");

                        var data = from tu in dbEnt.TeamUsers
                                   join u in dbEnt.Users on tu.TeamUserId equals u.UserId
                                   where tu.TeamId == deviceObj.AssignedTeam
                                   select new { u.Email, u.Name };

                        foreach (var item1 in data)
                        {
                            emails.Add(item1.Email);
                        }

                        string status = SendEmail(emails, item.WorkFlowId);
                        WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                        workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                        workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                        workFlowReportDTO.Action = item.Action;
                        workFlowReportDTO.WorkFlowActionStatus = status;
                        workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                        workFlowReportDTO.AppliedTo = item.AppliedTo;
                        workFlowReportDTO.CreatedDate = DateTime.Now;
                        workFlowReportDTO.CreatedBy = deviceObj.CreatedBy;
                        workFlowReportDTO.IsDeleted = false;
                        workFlowReportDTO.Priority = item.Priority;
                        workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                        workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);
                        CreateWorkFlowReport(workFlowReportDTO);
                    }
                }
                
                #endregion
            }
            else if (_obj == EntityTypes.Cases)
            {
                #region Cases
                InsertEventLog("ExecuteWorkFlow", EventType.Log, EventColor.yellow, "work flow activity running on Cases", "TICRM.BuisnessLayer.WorkFlowActivityManager.ExecuteWorkFlow", "");
                Case caseObj = HttpContext.Current.Session["Casesobj"] as Case;
                if (caseObj == null) { return; }
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    if (item.RelatedToId == caseObj.CaseId & item.AppliedTo == "OnUpdate") 
                    {
                        if (WFActionConstant.WorkOrder == item.Action)
                        {
                            CaseDto cas = objMapper.GetCaseDto(caseObj);
                            WorkOrderDto wo = new WorkOrderDto();
                            wo.WorkOrderId = Guid.NewGuid();
                            wo.Title = cas.CaseTitle;
                            wo.NTE = Convert.ToDecimal(1542.23);
                            wo.WorkOrderStageId = new Guid("fd9313c9-65c9-44b7-b34f-f67fd7a8f90c"); // to insert new WorkOrder stage
                            wo.Description = "Device Need To be Serviced.";
                            wo.IsDeleted = false;
                            wo.StatusId = new Guid("192f959f-2dfa-4d41-8464-dd482325dc6c"); // insert a active entry in database that have a value "192f959f-2dfa-4d41-8464-dd482325dc6c"
                            wo.AssignedUser = cas.AssignedUser;
                            wo.AssignedTeam = cas.AssignedTeam;
                            wo.CreatedDate = DateTime.Now;
                            string status = GenerateWorkOrder(wo);
                            //string status = SendEmail(emails, item.WorkFlowId);
                            WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                            workFlowReportDTO.WorkFlowReportId = Guid.NewGuid();
                            workFlowReportDTO.WorkFlowStatus = ActionStatusConstant.Success;
                            workFlowReportDTO.Action = item.Action;
                            workFlowReportDTO.WorkFlowActionStatus = status;
                            workFlowReportDTO.WorkFlowDesign = item.WorkFlowDesign;
                            workFlowReportDTO.AppliedTo = item.AppliedTo;
                            workFlowReportDTO.CreatedDate = DateTime.Now;
                            workFlowReportDTO.IsDeleted = false;
                            workFlowReportDTO.Priority = item.Priority;
                            workFlowReportDTO.WorkFlowId = item.WorkFlowId;
                            workFlowReportDTO.AccountID = Guid.Parse(item.AccountId);
                            CreateWorkFlowReport(workFlowReportDTO);
                        }
                    }
                    
                }

                #endregion
            }

        }

        public string GenerateWorkOrder(WorkOrderDto wo)
        {

            dbEnt.WorkOrders.Add(objMapper.GetWorkOrder(wo));
            if (dbEnt.SaveChanges() > 0)
            {
                return ActionStatusConstant.Success;
            }
            else
                return ActionStatusConstant.Failure;
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <param name="WorkFlowId">The work flow identifier.</param>
        /// <returns>System.String.</returns>
        public string SendEmail(List<string> emails,Guid WorkFlowId)
        {
            try
            {
                InsertEventLog("SendEmail", EventType.Log, EventColor.yellow, "work flow activity is going to send multiple email on " + _obj ,"TICRM.BuisnessLayer.WorkFlowActivityManager.SendEmail", "");
                // TODO: send email functionality


                var emailTemplate = (from t in dbEnt.EmailTemplates
                                               join c in dbEnt.EmailConfigurations on t.EmailConfigurationId equals c.EmailConfigurationId
                                               where t.WorkFlowId == WorkFlowId
                                               select new { t.Subject,t.Body,c.Email,c.Password,c.UserName}).FirstOrDefault();

                if (emailTemplate == null)
                {
                    MailAddress fromAddress = new MailAddress(dbEnt.Users.First().Email, "TI CRM");
                    //var toAddress = new MailAddress("mansoor@techimplement.com", "Mansoor");
                    const string fromPassword = "@dmin1234";

                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();

                    message.From = new MailAddress("mansoorsmtp@gmail.com", "TI CRM");
                    
                    foreach (string item in emails)
                    {
                        message.To.Add(new MailAddress(item));

                    }
                    message.Subject = "INCA Update";

                    //string Body = "WorkFlow is Completed successfully against " + _obj.ToString();
                    string Body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                    Body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                    Body += "</HEAD><BODY><DIV>";
                    Body += "<p> No any Email Template Is Define.</p><br/>";
                    Body += "<p> A WorkFlow Is Completed on " + _obj + " </p><footer class=\"footer\"><img src=\"E:\\Aqil\\TFS Project\\TI.CRM\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
                    Body += "</DIV></BODY></HTML>";
                    ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                    
                    // Add the alternate body to the message.
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(Body, mimeType);
                    LinkedResource inline = new LinkedResource(@"E:\Aqil\TFS Project\TI.CRM\TICRM\TICRM\Content\Images\TI_Logo.png", MediaTypeNames.Image.Jpeg);
                    inline.ContentId = Guid.NewGuid().ToString();
                    alternate.LinkedResources.Add(inline);

                    message.AlternateViews.Add(alternate);


                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);

                    smtp.Send(message);
                }
                else
                {
                    MailAddress fromAddress = new MailAddress(emailTemplate.Email, emailTemplate.UserName);
                    //var toAddress = new MailAddress("mansoor@techimplement.com", "Mansoor");
                    //const string fromPassword = emailTemplate.Password.ToString();

                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();

                    message.From = new MailAddress(emailTemplate.Email, emailTemplate.UserName);

                    foreach (string item in emails)
                    {
                        message.To.Add(new MailAddress(item));

                    }
                    message.Subject = emailTemplate.Subject;

                    //string Body = "WorkFlow is Completed successfully against " + _obj.ToString();
                    string Body = emailTemplate.Body;
                    ContentType mimeType = new System.Net.Mime.ContentType("text/html");

                    // Add the alternate body to the message.
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(Body, mimeType);
                    //LinkedResource inline = new LinkedResource(@"E:\Aqil\TFS Project\TI.CRM\TICRM\TICRM\Content\Images\TI_Logo.png", MediaTypeNames.Image.Jpeg);
                    //inline.ContentId = Guid.NewGuid().ToString();
                    //alternate.LinkedResources.Add(inline);

                    message.AlternateViews.Add(alternate);

                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromAddress.Address, emailTemplate.Password);

                    smtp.Send(message);
                }


                

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SendEmail", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.SendEmail", "");
                InsertEventNotification("SendEmail", EventType.Notification, EventColor.red, "Email is not send to " + string.Join(",", emails) + " on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.SendEmail", "");
                return ActionStatusConstant.Failure;
            }
            InsertEventNotification("SendEmail", EventType.Notification, EventColor.green, "Succesfully Email is send " + string.Join(",", emails) + " on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.SendEmail", "");
            return ActionStatusConstant.Success;
        }

        /// <summary>
        /// Creates the meeting.
        /// </summary>
        /// <param name="objApptEmail">The object appt email.</param>
        /// <returns>System.String.</returns>
        public string CreateMeeting(MeetingActionDTO objApptEmail)
        {
            try
            {
                InsertEventLog("CreateMeeting", EventType.Log, EventColor.yellow, "work flow activity is going to Create Meeting on " + _obj ,"TICRM.BuisnessLayer.WorkFlowActivityManager.CreateMeeting", "");

                SmtpClient sc = new SmtpClient("smtp.gmail.com");

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.From = new MailAddress("mansoorsmtp@gmail.com", "TI CRM");
                msg.To.Add(new MailAddress("aqil@techimplement.com"));
                msg.Subject = "Meeting Invite";
                msg.Body = "you are invited for meeting";

                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");
                str.AppendLine("PRODID:-//" + objApptEmail.Email);
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");
                str.AppendLine("BEGIN:VEVENT");

                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", objApptEmail.StartDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", (objApptEmail.EndDate - objApptEmail.StartDate).Minutes.ToString()));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", objApptEmail.EndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
                //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", objApptEmail.StartDate.ToString()));
                //str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", objApptEmail.EndDate.ToString()));
                str.AppendLine("LOCATION:" + objApptEmail.Location);
                str.AppendLine(string.Format("DESCRIPTION:{0}", objApptEmail.Body));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", objApptEmail.Body));
                str.AppendLine(string.Format("SUMMARY:{0}", objApptEmail.Subject));
                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", objApptEmail.Email));

                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));
                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:DISPLAY");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");
                str.AppendLine("END:VCALENDAR");
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
                ct.Parameters.Add("method", "REQUEST");
                AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
                msg.AlternateViews.Add(avCal);
                NetworkCredential nc = new NetworkCredential("mansoorsmtp@gmail.com", "@dmin1234");

                sc.Port = 587;
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = nc;


                sc.Send(msg);
                InsertEventNotification("CreateMeeting", EventType.Notification, EventColor.green, "Succesfully Meeting Invite is Send to " + objApptEmail.Email + " on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateMeeting", "");
                return ActionStatusConstant.Success;
            }
            catch (Exception ex)
            {
                InsertEventNotification("CreateMeeting", EventType.Notification, EventColor.red, "Meeting Invite is not Send to " + objApptEmail.Email + " on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateMeeting", "");
                InsertEventMonitor("CreateMeeting", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateMeeting", "");
                return ActionStatusConstant.Failure;
            }
        }

        /// <summary>
        /// Creates the note.
        /// </summary>
        /// <returns>System.String.</returns>
        public string CreateNote()
        {
            try
            {
                // TODO: add functionalioty for Crete Note
                InsertEventLog("CreateNote", EventType.Log, EventColor.yellow, "work flow activity is going to Create Note on " + _obj ,"TICRM.BuisnessLayer.WorkFlowActivityManager.CreateNote", "");
                InsertEventNotification("CreateNote", EventType.Notification, EventColor.green, "Succesfully Note is Created on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateNote", "");
                return ActionStatusConstant.Success;
            }
            catch (Exception ex)
            {
                InsertEventNotification("CreateNote", EventType.Notification, EventColor.red, "Note is not Created on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateNote", "");
                InsertEventMonitor("CreateNote", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateNote", "");
                return ActionStatusConstant.Failure;
            }
        }

        /// <summary>
        /// Creates the alert.
        /// </summary>
        /// <returns>System.String.</returns>
        public string CreateAlert()
        {
            try
            {

                // TODO: add functionality of create alert
                InsertEventLog("CreateAlert", EventType.Log, EventColor.yellow, "work flow activity is going to Create Alert on " + _obj,"TICRM.BuisnessLayer.WorkFlowActivityManager.CreateAlert", "");


                InsertEventNotification("CreateAlert", EventType.Notification, EventColor.green, "Alert is Created on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateAlert", "");
                return ActionStatusConstant.Success;
            }
            catch (Exception ex)
            {
                InsertEventNotification("CreateAlert", EventType.Notification, EventColor.red, "Alert is not Created on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateAlert", "");
                InsertEventMonitor("CreateAlert", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateAlert", "");
                return ActionStatusConstant.Failure;
            }
        }

        /// <summary>
        /// Creates the work flow report.
        /// </summary>
        /// <param name="workFlowReportDTO">The work flow report dto.</param>
        public void CreateWorkFlowReport(WorkFlowReportDTO workFlowReportDTO)
        {
            try
            {
                InsertEventLog("CreateWorkFlowReport", EventType.Log, EventColor.yellow, "work flow activity is going to Create Work Flow Report on " + _obj ,"TICRM.BuisnessLayer.WorkFlowActivityManager.CreateWorkFlowReport", "");
                WorkFlowReport workFlowReport = objMapper.GetWorkFlowReport(workFlowReportDTO);
                dbEnt.WorkFlowReports.Add(workFlowReport);
                WorkFlow workFlow = dbEnt.WorkFlows.FirstOrDefault(x => x.WorkFlowId == workFlowReportDTO.WorkFlowId);
                if (workFlow != null)
                {
                    workFlow.FrequencyOut += 1;
                    dbEnt.SaveChanges();
                }
                workFlowReport.Frequency = workFlow.FrequencyOut;
                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventNotification("CreateWorkFlowReport", EventType.Notification, EventColor.green, "WorkFlow report is Created on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateWorkFlowReport", "");
                }
                else
                {
                    InsertEventNotification("CreateWorkFlowReport", EventType.Notification, EventColor.red, "WorkFlow report is not Created on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateWorkFlowReport", "");
                }
            }
            catch (Exception ex)
            {
                InsertEventNotification("CreateWorkFlowReport", EventType.Notification, EventColor.red, " exception occur on create WorkFlow report on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateWorkFlowReport", "");
                InsertEventMonitor("CreateWorkFlowReport", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.CreateWorkFlowReport", "");
            }
        }

        /// <summary>
        /// Runs the work flow mapping.
        /// </summary>
        /// <param name="WorkFlowId">The work flow identifier.</param>
        /// <returns>System.String.</returns>
        public string RunWorkFlowMapping(Guid WorkFlowId)
        {
            try
            {
                InsertEventLog("RunWorkFlowMapping", EventType.Log, EventColor.yellow, "work flow activity is going to run WorkFlow Mapping on " + _obj ,"TICRM.BuisnessLayer.WorkFlowActivityManager.RunWorkFlowMapping", "");
                // TODO: add functionalioty for Crete Note
                List<WorkFlowMapping> workFlowMappings = dbEnt.WorkFlowMappings.Where(x => x.WorkFlowId == WorkFlowId && x.SourceType == _obj.ToString() && x.IsDone == false).ToList();
                string CurrentUserId = HttpContext.Current.Session["CurrentUserId"] as string;

                if (_obj == EntityTypes.Lead)
                {
                    LeadManager leadManager = new LeadManager();

                    foreach (WorkFlowMapping item in workFlowMappings.CollectionNotNull())
                    {
                        LeadDto lead = new JavaScriptSerializer().Deserialize<LeadDto>(item.SourceData);
                        if (item.Action == "Create")
                        {
                            bool condition = leadManager.SaveLead(lead, CurrentUserId,null, false, false);
                        }
                        else if (item.Action == "Update")
                        {
                            lead.LeadId = new Guid(item.SourceValue);
                            bool condition = leadManager.SaveLead(lead, CurrentUserId,null, true, false);
                        }
                        UpdateWorkFlowMapping(item.WorkFlowMappingId, CurrentUserId);
                    }
                }
                else if (_obj == EntityTypes.Account)
                {
                    AccountManager accountManager = new AccountManager();
                    foreach (WorkFlowMapping item in workFlowMappings.CollectionNotNull())
                    {
                        AccountDto accountDto = new JavaScriptSerializer().Deserialize<AccountDto>(item.SourceData);
                        if (item.Action == "Create")
                        {
                            bool condition = accountManager.SaveAccount(accountDto, CurrentUserId, null,false, false);
                        }
                        else if (item.Action == "Update")
                        {
                            accountDto.AccountId = new Guid(item.SourceValue);
                            bool condition = accountManager.SaveAccount(accountDto, CurrentUserId,null, true, false);
                        }
                        UpdateWorkFlowMapping(item.WorkFlowMappingId, CurrentUserId);
                    }
                }

                InsertEventNotification("RunWorkFlowMapping", EventType.Notification, EventColor.red, "Successfully run WorkFlow Mapping on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.RunWorkFlowMapping", "");
                return ActionStatusConstant.Success;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("RunWorkFlowMapping", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.RunWorkFlowMapping", "");
                InsertEventNotification("RunWorkFlowMapping", EventType.Notification, EventColor.red, "WorkFlow Mapping is not run on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.RunWorkFlowMapping", "");
                return ActionStatusConstant.Failure;
            }
        }

        /// <summary>
        /// Updates the work flow mapping.
        /// </summary>
        /// <param name="WorkFlowMappingId">The work flow mapping identifier.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        public void UpdateWorkFlowMapping(Guid WorkFlowMappingId, string CurrentUserId)
        {
            try
            {
                InsertEventLog("UpdateWorkFlowMapping", EventType.Log, EventColor.yellow, "work flow activity is going to Update workflow mapping on id=" + WorkFlowMappingId + " " + _obj ,"TICRM.BuisnessLayer.WorkFlowActivityManager.UpdateWorkFlowMapping", CurrentUserId);
                WorkFlowMapping workFlowMapping = dbEnt.WorkFlowMappings.FirstOrDefault(x => x.WorkFlowMappingId == WorkFlowMappingId);
                workFlowMapping.UpdatedBy = CurrentUserId;
                workFlowMapping.UpdatedDate = DateTime.Now;
                workFlowMapping.IsDone = true;
                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventNotification("UpdateWorkFlowMapping", EventType.Notification, EventColor.green, "Successfully Updated WorkFlow Mapping of id='" + WorkFlowMappingId + "' on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.UpdateWorkFlowMapping", CurrentUserId);
                }
            }
            catch (Exception ex)
            {
                InsertEventNotification("UpdateWorkFlowMapping", EventType.Notification, EventColor.red, "WorkFlow Mapping is not updated of id='" + WorkFlowMappingId + "' on workflow " + _obj, "TICRM.BuisnessLayer.WorkFlowActivityManager.UpdateWorkFlowMapping", CurrentUserId);
                InsertEventMonitor("UpdateWorkFlowMapping", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowActivityManager.UpdateWorkFlowMapping", CurrentUserId);
                throw;
            }
        }

    }

}
