using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Users Controller************
   Class [UsersController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Users like, 
   ||             navigating to the pages, getting associated modules for specific User]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ||                  [17/07/2020     the methods now use businnes layer to get and set the entities     Akhtar Zaman]
   ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
    ********************************************/

    public class UsersController : BaseController
    {
        private UserManager userManager = new UserManager();
        private CaseManager caseManager = new CaseManager();
        
        /// <summary>
        /// Provide list of all users on index page
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(userManager.GetAllUsers());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Users the details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ActionResult UserDetails(Guid id)
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
        /// Gets the caseson user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetCasesonUserId(Guid userId)
        {
            try
            {
                return Json(userManager.GetUserCases(userId), JsonRequestBehavior.AllowGet);  // return json List
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details of a user with 
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
                UserDto user = userManager.GetUser(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new user
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
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
        /// POST request to create user, Receive object of,
        /// new user validate it and creates a new user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.UserId = Guid.NewGuid();
                    bool condition = userManager.SaveUser(user, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                        TempData["FormSubmissionMessage"] = "User is not created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(user);

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "User is created successfully.";
                        TempData["FormSubmissionStatus"] = "success";

                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a user, 
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
                UserDto user = userManager.GetUser(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for user and update specified user
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = userManager.SaveUser(user, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                        TempData["FormSubmissionMessage"] = "User is not edited.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(user);

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "User is edited successfully.";
                        TempData["FormSubmissionStatus"] = "success";

                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }


        /// <summary>
        /// GET request for Delete form with user details,
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
                UserDto user = userManager.GetUser(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Recieve confirmation for user Deletion and Delete.
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
                UserDto user = userManager.GetUser(id);
                userManager.SaveUser(user, false, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Accepts the case.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        
        public ActionResult AcceptCase(Guid CaseId, Guid UserId)
        {
            UserDto user = userManager.GetUser(UserId);
            CaseDto cases = caseManager.GetCaseonId(CaseId);

            user.StatusId = userManager.GetStatus("InActive").StatusId;
            bool isUserUpdated = userManager.SaveUser(user, true, false);

            bool isCaseUpdated = caseManager.SaveCase(cases, "", true, false);
            if(isCaseUpdated && isUserUpdated)
            {
                SendEmail(user, cases, true, false);
            }

            return null;
        }
        /// <summary>
        /// Rejects the case.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public ActionResult RejectCase(Guid CaseId, Guid UserId)
        {
            UserDto user = userManager.GetUser(UserId);
            CaseDto cases = caseManager.GetCaseonId(CaseId);
            UserDto NewUser = userManager.EscalateUser(user, cases);
            if(NewUser != null)
            {
                cases.AssignedUser = NewUser.UserId;
                caseManager.SaveCase(cases, "", true, false);
                SendEmailDeclined(user, NewUser, cases);

            }
            return null;
        }

        /// <summary>
        /// Sends the case declination email .
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newuser">The newuser.</param>
        /// <param name="cases">The cases.</param>
        /// <returns></returns>
        public string SendEmailDeclined(UserDto user, UserDto newuser, CaseDto cases)
        {
            TeamDto team = userManager.GetUserTeam(user.UserId);
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = "swuichcrm@gmail.com"; //Sender Email Address  
            string password = "Swuich@123"; //Sender Password  
            string emailToAddress = user.Email; //Receiver Email Address  
            string newemailToAddress = newuser.Email; //Receiver Email Address  
            string teamemailToAddress = team.Email; //Receiver Email Address  
            string subject = "Case Assignment";
            string Body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            Body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            Body += "</HEAD><BODY><DIV>";
            Body += "<p>You have declined the case</p><br/>";
            Body += "<h1>" + cases.CaseTitle + "</h1><br/>";
            Body += "<p>It is now assigned to " + newuser.Email + "</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            Body += "</DIV></BODY></HTML>";

            string BodyNewUser = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            BodyNewUser += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            BodyNewUser += "</HEAD><BODY><DIV>";
            BodyNewUser += "<p>A case has been assigned to you</p><br/>";
            BodyNewUser += "<h1>" + cases.CaseTitle + "</h1><br/>";
            BodyNewUser += "<p>To ensure the case stays with you accept it in the given time frame</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            BodyNewUser += "</DIV></BODY></HTML>";

            string BodyTeam = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            BodyTeam += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            BodyTeam += "</HEAD><BODY><DIV>";
            BodyTeam += "<p> The user " + user.Name + " declined the case</p><br/>";
            BodyTeam += "<h1>" + cases.CaseTitle + "</h1><br/>";
            BodyTeam += "<p>it has been assigned to " + newuser.Name + "</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            BodyTeam += "</DIV></BODY></HTML>";

            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
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

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(newemailToAddress);
                mail.Subject = subject;
                mail.Body = BodyNewUser;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(teamemailToAddress);
                mail.Subject = subject;
                mail.Body = BodyTeam;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }

            return null;
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cases">The cases.</param>
        /// <param name="isaccepted">if set to <c>true</c> [isaccepted].</param>
        /// <param name="isDeclined">if set to <c>true</c> [is declined].</param>
        /// <returns></returns>
        public string SendEmail(UserDto user, CaseDto cases, bool isaccepted, bool isDeclined)
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
            Body += "<p>You have accepted the case</p><br/>";
            Body += "<h1>" + cases.CaseTitle + "</h1><br/>";
            Body += "<p>Follow up on the case in the given scheduled time</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            Body += "</DIV></BODY></HTML>";

            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
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
                    Console.WriteLine("Email has been sent Successfully to " + emailToAddress);
                }
            }
            return null;
        }

        /// <summary>
        /// Escalates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cases">The cases.</param>
        /// <returns></returns>
        public bool EscalateUser(UserDto user, CaseDto cases)
        {
            return false;
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
