using System;
using System.Net;
using System.Web.Mvc;
using TICRM.DTOs;
using TICRM.BuisnessLayer;

namespace TICRM.Controllers
{
    /************ReadingUnits Controller************
  Class [ReadingUnitsController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with ReadingUnits like, 
  ||             navigating to the pages, getting associated modules for specific ReadingUnit]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
  ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
  ||                  
   ********************************************/

    
    public class ReadingUnitsController : BaseController
    {
       
        ReadingUnitManager readingUnitManager = new ReadingUnitManager();
        ReadingTypeManager readingTypeManager = new ReadingTypeManager();

        /// <summary>
        ///  Provide all Reading Units on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(readingUnitManager.GetReadingUnits());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details of a Reading Unit with 
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
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                if (readingUnit == null)
                {
                    return HttpNotFound();
                }
                return View(readingUnit);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details on a partial view of a Reading Unit with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                return PartialView("_PartialReadingUnitsDetails", readingUnit);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details to delete on partail view 
        /// of a Reading Unit with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                return PartialView("_PartialReadingUnitsDelete", readingUnit);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new Reading Unit.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                ReadingUnitDto reading = new ReadingUnitDto();
                reading.ReadingTypeDropdown = new SelectList(readingTypeManager.GetReadingTypes(), "ReadingTypeId", "Name");
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request to create Resources, Receive object of,
        /// new Reading Unit validate it and creates a new Reading Unit
        /// </summary>
        /// <param name="readingUnit">The reading unit.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReadingUnitDto readingUnit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = readingUnitManager.SaveReadingUnit(readingUnit, false, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingUnit);
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
        /// GET request to edit a Reading Unit, 
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
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                if (readingUnit == null)
                {
                    return HttpNotFound();
                }
                readingUnit.ReadingTypeDropdown = new SelectList(readingTypeManager.GetReadingTypes(), "ReadingTypeId", "Name", readingUnit.Type);
                return View(readingUnit);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for Reading Unit and update specified Reading Unit
        /// </summary>
        /// <param name="readingUnit">The reading unit.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReadingUnitDto readingUnit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = readingUnitManager.SaveReadingUnit(readingUnit, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Reading Type is not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(readingUnit);
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
        /// GET request for Delete form with Reading Unit details,
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
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);

                if (readingUnit == null)
                {
                    return HttpNotFound();
                }
                return View(readingUnit);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Receive confirmation for Reading Unit Deletion and Delete.
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
                ReadingUnitDto readingUnit = readingUnitManager.GetReadingUnit(id);
                readingUnitManager.SaveReadingUnit(readingUnit, true, true);
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
