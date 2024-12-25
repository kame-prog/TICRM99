using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************EmailConfigurations Controller************
   Class [EmailConfigurationsController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with EmailConfigurations like, 
   ||             navigating to the pages, getting associated modules for specific EmailConfiguration]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
   ||                  
    ********************************************/
    public class EmailConfigurationsController : BaseController
    {
        
        private EmailConfigurationManager emailConfigurationManager = new EmailConfigurationManager();

        /// <summary>
        /// Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                return View(emailConfigurationManager.GetEmailConfigurationDTOs(CurrentUserId));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailConfigurationDTO emailConfigurationDTO = emailConfigurationManager.GetEmailConfigurationDtoOnId(id, CurrentUserId);
                if (emailConfigurationDTO == null)
                {
                    return HttpNotFound();
                }
                return View(emailConfigurationDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Partial details view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailConfigurationDTO emailConfigurationDTO = emailConfigurationManager.GetEmailConfigurationDtoOnId(id, CurrentUserId);
                return PartialView("_PartialEmailConfigurationsDetails", emailConfigurationDTO);
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailConfigurationDTO emailConfigurationDTO = emailConfigurationManager.GetEmailConfigurationDtoOnId(id, CurrentUserId);
                return PartialView("_PartialEmailConfigurationsDelete", emailConfigurationDTO);
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
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified email configuration dto.
        /// </summary>
        /// <param name="emailConfigurationDTO">The email configuration dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmailConfigurationDTO emailConfigurationDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = emailConfigurationManager.SaveEmailConfiguration(emailConfigurationDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(emailConfigurationDTO);
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
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailConfigurationDTO emailConfigurationDTO = emailConfigurationManager.GetEmailConfigurationDtoOnId(id, CurrentUserId);
                if (emailConfigurationDTO == null)
                {
                    return HttpNotFound();
                }
                return View(emailConfigurationDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified email configuration dto.
        /// </summary>
        /// <param name="emailConfigurationDTO">The email configuration dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmailConfigurationDTO emailConfigurationDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = emailConfigurationManager.SaveEmailConfiguration(emailConfigurationDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(emailConfigurationDTO);
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
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                EmailConfigurationDTO emailConfigurationDTO = emailConfigurationManager.GetEmailConfigurationDtoOnId(id, CurrentUserId);
                if (emailConfigurationDTO == null)
                {
                    return HttpNotFound();
                }
                return View(emailConfigurationDTO);
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
                EmailConfigurationDTO emailConfigurationDTO = emailConfigurationManager.GetEmailConfigurationDtoOnId(id, CurrentUserId);
                bool condition = emailConfigurationManager.SaveEmailConfiguration(emailConfigurationDTO, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Save email.
        /// </summary>
        /// <param name="Email">The email.</param>
        /// <param name="Password">The password.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Email(String Email, String Password)
        {
            try
            {
                EmailConfigurationDTO emailconfig = new EmailConfigurationDTO();
                emailconfig.Email = Email;
                emailconfig.Password = Password;
                string CurrentUserId = User.Identity.GetUserId(); // get current userid

                bool condition = emailConfigurationManager.SaveEmailConfiguration(emailconfig, CurrentUserId, false, false);

                string status = "error";

                return Content(status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
       
    }
}
