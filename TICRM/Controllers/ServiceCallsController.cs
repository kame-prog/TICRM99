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

    /************ServiceCalls Controller************
    Class [ServiceCallsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with ServiceCalls like, 
    ||             navigating to the pages, getting associated modules for specific ServiceCall]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
    ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
    ||                  [21/08/2020     Added Sorting to pagination method     Akhtar Zaman]
    ********************************************/

   
    public class ServiceCallsController : BaseController
    {
       
        ServiceCallManager scm = new ServiceCallManager();


        /// <summary>
        /// Provide list of all Service Calls on index page
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(scm.GetServiceCalls());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details of a Service Calls with 
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
                var serviceCall = scm.GetServiceCall(id);
                if (serviceCall == null)
                {
                    return HttpNotFound();
                }
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details on a partial view of a service calls with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var serviceCall = scm.GetServiceCall(id);
                return PartialView("_PartialServiceCallsDetails", serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details to delete on partail view 
        /// of a service calls with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var serviceCall = scm.GetServiceCall(id);
                return PartialView("_PartialServiceCallsDelete", serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new Service Calls.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                ServiceCallDto serviceCall = new ServiceCallDto();
                serviceCall.StatusDropdown = new SelectList(scm.Status, "StatusId", "Name");
                serviceCall.AssignedTeamDropdown = new SelectList(scm.Teams, "TeamId", "Name");
                serviceCall.UrgencyDropdown = new SelectList(scm.Urgencies, "UrgencyId", "Name");
                serviceCall.AssignedUserDropdown = new SelectList(scm.Users, "UserId", "Name");
                serviceCall.ServiceCallStageDropdown = new SelectList(scm.WorkStages, "WorkStageId", "Name");
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// POST request to create Service Calls, Receive object of,
        /// new workflow Mapping validate it and creates a new workflow mapping
        /// </summary>
        /// <param name="serviceCall">The service call.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceCallDto serviceCall)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = scm.SaveServiceCall(serviceCall);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = serviceCall.Title + " Service Call Created successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = serviceCall.Title + " Service Call not Created.";
                TempData["FormSubmissionStatus"] = "error";
                ViewBag.StatusId = new SelectList(scm.Status, "StatusId", "Name", serviceCall.StatusId);
                ViewBag.AssignedTeam = new SelectList(scm.Teams, "TeamId", "Name", serviceCall.AssignedTeam);
                ViewBag.UrgencyId = new SelectList(scm.Urgencies, "UrgencyId", "Name", serviceCall.UrgencyId);
                ViewBag.AssignedUser = new SelectList(scm.Users, "UserId", "Name", serviceCall.AssignedUser);
                ViewBag.ServiceCallStageId = new SelectList(scm.WorkStages, "WorkStageId", "Name", serviceCall.ServiceCallStageId);
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a serivce calls, 
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
                var serviceCall = scm.GetServiceCall(id);
                if (serviceCall == null)
                {
                    return HttpNotFound();
                }
                serviceCall.StatusDropdown = new SelectList(scm.Status, "StatusId", "Name", serviceCall.StatusId);
                serviceCall.AssignedTeamDropdown = new SelectList(scm.Teams, "TeamId", "Name", serviceCall.AssignedTeam);
                serviceCall.UrgencyDropdown = new SelectList(scm.Urgencies, "UrgencyId", "Name", serviceCall.UrgencyId);
                serviceCall.AssignedUserDropdown = new SelectList(scm.Users, "UserId", "Name", serviceCall.AssignedUser);
                serviceCall.ServiceCallStageDropdown = new SelectList(scm.WorkStages, "WorkStageId", "Name", serviceCall.ServiceCallStageId);
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for service calls and update specified service calls.
        /// </summary>
        /// <param name="serviceCall">The service call.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceCallDto serviceCall)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = scm.SaveServiceCall(serviceCall, true);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = serviceCall.Title + " Service Call Updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "Service Call not Updated.";
                TempData["FormSubmissionStatus"] = "error";
                ViewBag.StatusId = new SelectList(scm.Status, "StatusId", "Name", serviceCall.StatusId);
                ViewBag.AssignedTeam = new SelectList(scm.Teams, "TeamId", "Name", serviceCall.AssignedTeam);
                ViewBag.UrgencyId = new SelectList(scm.Urgencies, "UrgencyId", "Name", serviceCall.UrgencyId);
                ViewBag.AssignedUser = new SelectList(scm.Users, "UserId", "Name", serviceCall.AssignedUser);
                ViewBag.ServiceCallStageId = new SelectList(scm.WorkStages, "WorkStageId", "Name", serviceCall.ServiceCallStageId);
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Delete form with Service calls details,
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
                var serviceCall = scm.GetServiceCall(id);
                if (serviceCall == null)
                {
                    return HttpNotFound();
                }
                return View(serviceCall);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Recieve confirmation for service calls Deletion and Delete.
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
                var serviceCall = scm.GetServiceCall(id);
                scm.SaveServiceCall(serviceCall, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the Service Call list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetServiceCallList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();
            List<ServiceCallDto> obj = scm.GetServiceCallList(sEcho, iDisplayStart, iDisplayLength, sSearch);
            int totalRecord = scm.GetTotalCount();

            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Title).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Detail).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Detail).ToList();
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
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Urgency.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Urgency.Name).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.WorkStage.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.WorkStage.Name).ToList();
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

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
