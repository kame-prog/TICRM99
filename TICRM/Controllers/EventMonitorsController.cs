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
    /************EventMonitors Controller************
    Class [EventMonitorsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with EventMonitors,
    ||             and Logging of exceptions]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||                  
     ********************************************/
    public class EventMonitorsController : BaseController
    {
        private EventMonitorManager eventMonitorManager = new EventMonitorManager();


        /// <summary>
        /// Index View
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
        /// Gets the event moniter list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetEventMonitorList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();


            List<EventMonitorDTO> obj = eventMonitorManager.GetEventMonitorList(sEcho, iDisplayStart, iDisplayLength, sSearch);

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
                        obj = obj.OrderBy(x => x.Type).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Type).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Message).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Message).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Color).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Color).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.IPAddress).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.IPAddress).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedBy).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedBy).ToList();
                    }
                    break;

            }
            int totalRecord = eventMonitorManager.GetTotalCount();

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
        /// Details view for the specified event monitor.
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
                EventMonitorDTO eventMonitorDTO = eventMonitorManager.GetEventMonitorOnId(id);
                if (eventMonitorDTO == null)
                {
                    return HttpNotFound();
                }
                return View(eventMonitorDTO);
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
        /// Creates the specified event monitor dto.
        /// </summary>
        /// <param name="eventMonitorDTO">The event monitor dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventMonitorDTO eventMonitorDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = eventMonitorManager.SaveEventMonitor(eventMonitorDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(eventMonitorDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Details view for the specified event monitor.
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
                EventMonitorDTO eventMonitorDTO = eventMonitorManager.GetEventMonitorOnId(id);
                if (eventMonitorDTO == null)
                {
                    return HttpNotFound();
                }
                return View(eventMonitorDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified event monitor dto.
        /// </summary>
        /// <param name="eventMonitorDTO">The event monitor dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventMonitorDTO eventMonitorDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = eventMonitorManager.SaveEventMonitor(eventMonitorDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(eventMonitorDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view for the specified event monitor
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
                EventMonitorDTO eventMonitorDTO = eventMonitorManager.GetEventMonitorOnId(id);
                if (eventMonitorDTO == null)
                {
                    return HttpNotFound();
                }
                return View(eventMonitorDTO);
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
                EventMonitorDTO eventMonitorDTO = eventMonitorManager.GetEventMonitorOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = eventMonitorManager.SaveEventMonitor(eventMonitorDTO, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    
    }
}
