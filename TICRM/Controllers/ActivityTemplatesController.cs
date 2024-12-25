using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************ActivityTemplates Controller************
  Class [ActivityTemplatesController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with ActivityTemplates like, 
  ||             navigating to the pages, getting associated modules for specific ActivityTemplate]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ********************************************/
    public class ActivityTemplatesController : BaseController
    {
        private ActivityTemplateManager activityTemplateManager = new ActivityTemplateManager();

        /// <summary>
        /// Get the activities and return in view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                //activityTemplateManager.GetActivityTemplateDTOs()
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Return a string with all activitiy templates for DataTable to render
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns></returns>
        public string GetActivitesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();


            var obj =  activityTemplateManager.GetActivityTemplatesList(sEcho, iDisplayStart, iDisplayLength, sSearch);

            switch (sortColumnIndex)
            {
                
                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.ActivityTemplateType).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc") 
                    {
                        obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.PropertyName).ToList();
                    }
                        break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.PropertyValue).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.PropertyValue).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.PropertyType).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.PropertyType).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedBy).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedBy).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.UpdatedDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.UpdatedDate).ToList();
                    }
                    break;
                
                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.UpdatedBy).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.UpdatedBy).ToList();
                    }
                    break;
            }
            
            int totalRecord = activityTemplateManager.GetTotalCount();

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
        /// Get the activity on the basis of id and return in detail view.
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
                ActivityTemplateDTO activityTemplateDTO = activityTemplateManager.GetActivityTemplateOnId(id);
                if (activityTemplateDTO == null)
                {
                    return HttpNotFound();
                }
                return View(activityTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partials detail for activity on identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                ActivityTemplateDTO activityTemplateDTO = activityTemplateManager.GetActivityTemplateOnId(id);
                return PartialView("_PartialActivityTemplatesDetails", activityTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial delete view on the basis of id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                ActivityTemplateDTO activityTemplateDTO = activityTemplateManager.GetActivityTemplateOnId(id);
                return PartialView("_PartialActivityTemplateDelete", activityTemplateDTO);
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
        /// Creates the specified activity template dto.
        /// </summary>
        /// <param name="activityTemplateDTO">The activity template dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActivityTemplateDTO activityTemplateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = activityTemplateManager.SaveitActivityTemplate(activityTemplateDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(activityTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits view for the specified activity template.
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
                ActivityTemplateDTO activityTemplateDTO = activityTemplateManager.GetActivityTemplateOnId(id);
                if (activityTemplateDTO == null)
                {
                    return HttpNotFound();
                }
                return View(activityTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified activity template dto.
        /// </summary>
        /// <param name="activityTemplateDTO">The activity template dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActivityTemplateDTO activityTemplateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = activityTemplateManager.SaveitActivityTemplate(activityTemplateDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(activityTemplateDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view for the specified activity template.
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
                ActivityTemplateDTO activityTemplateDTO = activityTemplateManager.GetActivityTemplateOnId(id);
                if (activityTemplateDTO == null)
                {
                    return HttpNotFound();
                }
                return View(activityTemplateDTO);
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
                ActivityTemplateDTO activityTemplateDTO = activityTemplateManager.GetActivityTemplateOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = activityTemplateManager.SaveitActivityTemplate(activityTemplateDTO, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


    }
}
