using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************EmailTemplates Controller************
    Class [EmailTemplatesController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with EmailTemplates like, 
    ||             navigating to the pages, getting associated modules for specific account]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||                  
     ********************************************/
    public class EmailTemplatesController : BaseController
    {
       
        private EmailTemplateManager emailTemplateManager = new EmailTemplateManager();
        private EmailConfigurationManager emailConfigurationManager = new EmailConfigurationManager();
        private WorkFlowManager workFlowManager = new WorkFlowManager();

        /// <summary>
        /// Get email templates and return on index view
        /// </summary>
        /// <returns></returns>
        
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                return View(emailTemplateManager.GetEmailTemplateDTOs(CurrentUserId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Details view for the specified email template
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailTemplateDTO emailTemplateDTO = emailTemplateManager.GetEmailTemplateDtoOnId(id, CurrentUserId);
                if (emailTemplateDTO == null)
                {
                    return HttpNotFound();
                }
                return View(emailTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial veiw for details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailTemplateDTO emailTemplateDTO = emailTemplateManager.GetEmailTemplateDtoOnId(id, CurrentUserId);
                return PartialView("_PartialEmailTemplatesDetails", emailTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Partial view for delete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailTemplateDTO emailTemplateDTO = emailTemplateManager.GetEmailTemplateDtoOnId(id, CurrentUserId);
                return PartialView("_PartialEmailTemplatesDelete", emailTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Create view
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                ViewBag.EmailConfigurationId = new SelectList(emailConfigurationManager.GetEmailConfigurationDTOs(CurrentUserId), "EmailConfigurationId", "Email");
                ViewBag.WorkFlowId = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name");
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified email template dto.
        /// </summary>
        /// <param name="emailTemplateDTO">The email template dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EmailTemplateDTO emailTemplateDTO)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                if (ModelState.IsValid)
                {
                    bool condition = emailTemplateManager.SaveEmailTemplate(emailTemplateDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");

                        TempData["FormSubmissionMessage"] = "Email Template is not saved.";
                        TempData["FormSubmissionStatus"] = "error";

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Email Template is saved successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.EmailConfigurationId = new SelectList(emailConfigurationManager.GetEmailConfigurationDTOs(CurrentUserId), "EmailConfigurationId", "Email", emailTemplateDTO.EmailConfigurationId);
                ViewBag.WorkFlowId = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name", emailTemplateDTO.WorkFlowId);
                return View(emailTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// edit view for the specified email template.
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailTemplateDTO emailTemplateDTO = emailTemplateManager.GetEmailTemplateDtoOnId(id, CurrentUserId);
                if (emailTemplateDTO == null)
                {
                    return HttpNotFound();
                }
                ViewBag.EmailConfigurationId = new SelectList(emailConfigurationManager.GetEmailConfigurationDTOs(CurrentUserId), "EmailConfigurationId", "Email", emailTemplateDTO.EmailConfigurationId);
                ViewBag.WorkFlowId = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name", emailTemplateDTO.WorkFlowId);
                return View(emailTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified email template dto.
        /// </summary>
        /// <param name="emailTemplateDTO">The email template dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmailTemplateDTO emailTemplateDTO)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                if (ModelState.IsValid)
                {
                    bool condition = emailTemplateManager.SaveEmailTemplate(emailTemplateDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Email Template is not updated.";
                        TempData["FormSubmissionStatus"] = "error";
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Email Template is Updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.EmailConfigurationId = new SelectList(emailConfigurationManager.GetEmailConfigurationDTOs(CurrentUserId), "EmailConfigurationId", "Email", emailTemplateDTO.EmailConfigurationId);
                ViewBag.WorkFlowId = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name", emailTemplateDTO.WorkFlowId);
                return View(emailTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view for the specified email template
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailTemplateDTO emailTemplateDTO = emailTemplateManager.GetEmailTemplateDtoOnId(id, CurrentUserId);
                if (emailTemplateDTO == null)
                {
                    return HttpNotFound();
                }
                return View(emailTemplateDTO);
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailTemplateDTO emailTemplateDTO = emailTemplateManager.GetEmailTemplateDtoOnId(id, CurrentUserId);
                bool condition = emailTemplateManager.SaveEmailTemplate(emailTemplateDTO, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    }
}
