using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
namespace TICRM.Controllers
{
    /************ReadingTypes Controller************
    Class [ReadingTypesController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with ReadingTypes like, 
    ||             navigating to the pages, getting associated modules for specific ReadingType]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
    ||                  [17/07/2020     the methods now use businnes layer to get and set the entities    Akhtar Zaman]
     ********************************************/
    
   
    public class ReadingTypesController : BaseController
    {
        
        private ReadingTypeManager rtm = new ReadingTypeManager();


        /// <summary>
        /// Provide all Reading Types on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(rtm.GetReadingTypes());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details of a Reading Type with 
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
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                if (readingType == null)
                {
                    return HttpNotFound();
                }
                return View(readingType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details on a partial view of a Reading Type with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            ReadingTypeDto readingType = rtm.GetReadingType(id);
            return PartialView("_PartialReadingTypesDetails", readingType);
        }


        /// <summary>
        /// Provides details to delete on partail view 
        /// of a Reading Type with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                return PartialView("_PartialReadingTypesDelete", readingType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new Reading Type.
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
        /// POST request to create Resources, Receive object of,
        /// new Reading Type validate it and creates a new Reading Type
        /// </summary>
        /// <param name="readingType">Type of the reading.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReadingTypeDto readingType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rtm.SaveReadingType(readingType, false, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingType);
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type Created.";
                        TempData["FormSubmissionStatus"] = "Success";
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
        /// GET request to edit a Reading Type, 
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
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                if (readingType == null)
                {
                    return HttpNotFound();
                }
                return View(readingType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for Reading Type and update specified Reading Type
        /// </summary>
        /// <param name="readingType">Type of the reading.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReadingTypeDto readingType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rtm.SaveReadingType(readingType, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingType);
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type Created.";
                        TempData["FormSubmissionStatus"] = "Success";
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
        /// GET request for Delete form with Reading Type details,
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
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                if (readingType == null)
                {
                    return HttpNotFound();
                }
                return View(readingType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Receive confirmation for Reading Type Deletion and Delete.
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
                ReadingTypeDto readingType = rtm.GetReadingType(id);
                bool condition = rtm.SaveReadingType(readingType, false, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
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
