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
    /************WorkFlowReports Controller************
    Class [WorkFlowReportsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with WorkFlowReports like, 
    ||             navigating to the pages, getting associated modules for specific WorkFlowReport]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
    ||                  [21/08/2020     Added Sorting to pagination method     Akhtar Zaman]
     ********************************************/
    public class WorkFlowReportsController : BaseController
    {
        private WorkFlowReportManager workFlowReportManager = new WorkFlowReportManager();

        /// <summary>
        /// Provide all Workflow Reports on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(workFlowReportManager.GetWorkFlowReports());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Provides details of a workFlow Report with 
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
                WorkFlowReportDTO workFlowReportDTO = workFlowReportManager.GetWorkFlowReportOnId(id);
                if (workFlowReportDTO == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET request for Create page to create new WorkFlow Reports.
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
        /// POST request to create Work Flow Reports, Receive object of,
        /// new workflow Report validate it and creates a new workflow report
        /// </summary>
        /// <param name="workFlowReportDTO">The work flow report dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkFlowReportDTO workFlowReportDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = workFlowReportManager.SaveItWorkFlowReport(workFlowReportDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET request to edit a workflow report, 
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
                WorkFlowReportDTO workFlowReportDTO = workFlowReportManager.GetWorkFlowReportOnId(id);
                if (workFlowReportDTO == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for WorkFlow Report and update WorkFlow Report
        /// </summary>
        /// <param name="workFlowReportDTO">The work flow report dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkFlowReportDTO workFlowReportDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = workFlowReportManager.SaveItWorkFlowReport(workFlowReportDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET request for Delete form with workorder report details,
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
                WorkFlowReportDTO workFlowReportDTO = workFlowReportManager.GetWorkFlowReportOnId(id);
                if (workFlowReportDTO == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// POST request for Delete Action, 
        /// Recieve confirmation for workflow report Deletion and Delete.
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
                WorkFlowReportDTO workFlowReportDTO = workFlowReportManager.GetWorkFlowReportOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = workFlowReportManager.SaveItWorkFlowReport(workFlowReportDTO, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the work flow report list with respect 
        /// to recevied parameter Implemented as server side pagination.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetWorkflowReportList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();
            List<WorkFlowReportDTO> obj = workFlowReportManager.GetWorkflowReportList(sEcho, iDisplayStart, iDisplayLength, sSearch);
            int totalRecord = workFlowReportManager.GetTotalCount();

            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.WorkFlow.Name).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Action).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Action).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.WorkFlowStatus).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.WorkFlowStatus).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.WorkFlowActionStatus).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.WorkFlowActionStatus).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AppliedTo).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AppliedTo).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Frequency).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Frequency).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Priority).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Priority).ToList();
                    }
                    break;

                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    break;
                case 8:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedBy).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedBy).ToList();
                    }
                    break;
                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.User.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.User.Name).ToList();
                    }
                    break;
            }

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

    }
}
