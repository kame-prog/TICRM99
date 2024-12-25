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
    /************WorkOrders Controller************
    Class [WorkOrdersController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with WorkOrders like, 
    ||             navigating to the pages, getting associated modules for specific WorkOrder]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
    ||                  [17/08/2020     Added Comment block to this All Action Methods    Sikandar Mustafa]        
    ||                  [21/08/2020     Sorting to the pagination method    Akhtar Zaman]        
     ********************************************/

    public class WorkOrdersController : BaseController
    {
        WorkOrderManager wom = new WorkOrderManager();

        /// <summary>
        /// List all the workorders on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                wom.WorkorderCount();
                return View(wom.GetWorkOrders());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Provides details of a workorder with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>WokrOrder with specifc id</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var workOrder = wom.GetWorkOrder(id);
                if (workOrder == null)
                {
                    return HttpNotFound();
                }
                return View(workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Provides details on a partial view of a workorder with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var workOrder = wom.GetWorkOrder(id);
                return PartialView("_PartialWorkOrderDetails", workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Provides details to delete on partail view 
        /// of a workorder with respect to id passed 
        /// to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var workOrder = wom.GetWorkOrder(id);
                return PartialView("_PartialWorkOrderDelete", workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET request for Create page with a, 
        /// form to create a new workorder
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                WorkOrderDto workorder = new WorkOrderDto();
                workorder.StatusDropdown = new SelectList(wom.Status, "StatusId", "Name");
                workorder.AssignedUserDropdown = new SelectList(wom.Users, "UserId", "Name");
                workorder.WorkStagesDropdown = new SelectList(wom.WorkStages, "WorkStageId", "Name");
                workorder.AssignedTeamDropdown = new SelectList(wom.Teams, "TeamId", "Name");
                workorder.AccountsDropdown = new SelectList(wom.Accounts, "AccountId", "Name");
                return View(workorder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// POST request for Create Action that Receive object 
        /// of Workorder validate and create new Order
        /// </summary>
        /// <param name="workOrder">The work order.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkOrderDto workOrder, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = wom.SaveWorkOrder(workOrder);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "WorkOrder is Created successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        if (loc != "False")
                        {
                            Guid temp = new Guid(loc);
                            return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                        }
                        else
                            return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "WorkOrder is not Created.";
                TempData["FormSubmissionStatus"] = "error";
                workOrder.StatusDropdown = new SelectList(wom.Status, "StatusId", "Name", workOrder.StatusId);
                workOrder.AssignedTeamDropdown = new SelectList(wom.Teams, "TeamId", "Name", workOrder.AssignedTeam);
                workOrder.AssignedUserDropdown = new SelectList(wom.Users, "UserId", "Name", workOrder.AssignedUser);
                workOrder.WorkStagesDropdown = new SelectList(wom.WorkStages, "WorkStageId", "Name", workOrder.WorkOrderStageId);
                workOrder.AccountsDropdown = new SelectList(wom.Accounts, "AccountId", "Name", workOrder.AccountId);
                if (loc != "False")
                {
                    Guid temp = new Guid(loc);
                    return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                }
                else
                    return View(workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET request for Edit form with workorder,
        /// data to update with respect to id
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
                var workOrder = wom.GetWorkOrder(id);
                if (workOrder == null)
                {
                    return HttpNotFound();
                }
                workOrder.StatusDropdown= new SelectList(wom.Status, "StatusId", "Name", workOrder.StatusId);
                workOrder.AssignedTeamDropdown = new SelectList(wom.Teams, "TeamId", "Name", workOrder.AssignedTeam);
                workOrder.AssignedUserDropdown = new SelectList(wom.Users, "UserId", "Name", workOrder.AssignedUser);
                workOrder.WorkStagesDropdown = new SelectList(wom.WorkStages, "WorkStageId", "Name", workOrder.WorkOrderStageId);
                workOrder.AccountsDropdown = new SelectList(wom.Accounts, "AccountId", "Name", workOrder.AccountId);

                return View(workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for workorder and update wordorder
        /// </summary>
        /// <param name="workOrder">The work order.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkOrderDto workOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = wom.SaveWorkOrder(workOrder, true);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "WorkOrder is updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "WorkOrder is not updated.";
                TempData["FormSubmissionStatus"] = "error";
                workOrder.StatusDropdown = new SelectList(wom.Status, "StatusId", "Name", workOrder.StatusId);
                workOrder.AssignedTeamDropdown = new SelectList(wom.Teams, "TeamId", "Name", workOrder.AssignedTeam);
                workOrder.AssignedUserDropdown = new SelectList(wom.Users, "UserId", "Name", workOrder.AssignedUser);
                workOrder.WorkStagesDropdown = new SelectList(wom.WorkStages, "WorkStageId", "Name", workOrder.WorkOrderStageId);
                workOrder.AccountsDropdown = new SelectList(wom.Accounts, "AccountId", "Name", workOrder.AccountId);

                return View(workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET request for Delete form with workorder,
        /// Wordoreder to delete with respect to id
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
                var workOrder = wom.GetWorkOrder(id);
                if (workOrder == null)
                {
                    return HttpNotFound();
                }
                return View(workOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// POST request for Delete Action, 
        /// Recieve confirmation for workorder Deletion and Delete wordorder
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
                var workOrder = wom.GetWorkOrder(id);
                wom.SaveWorkOrder(workOrder, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates new workorder from account details page.
        /// </summary>
        /// <param name="Title">The title.</param>
        /// <param name="Desc">The desc.</param>
        /// <param name="NTE">The nte.</param>
        /// <param name="WOUser">The wo user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Stage">The stage.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult CreateWOfromAccount(String Title, String Desc, String NTE, String WOUser, String Team, String Status, String Stage, String AccID)
        {
            try
            {
                WorkOrderDto work = new WorkOrderDto();
                work.AssignedTeam = Guid.Parse(Team);
                work.AssignedUser = Guid.Parse(WOUser);
                work.StatusId = Guid.Parse(Status);
                work.WorkOrderStageId = Guid.Parse(Stage);
                work.Title = Title;
                work.Description = Desc;
                work.NTE = Convert.ToDecimal(NTE);
                work.AccountId = Guid.Parse(AccID);
                bool condition = wom.SaveWorkOrder(work);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the workorder count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        /// <exception cref="Exception"></exception>
        public int GetWorkorderCount()
        {
            try
            {
                return wom.WorkorderCount();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the work orders list with respect 
        /// to recevied parameter Implemented as server side pagination.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetWorkOrdersList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();
            List<WorkOrderDto> obj = wom.GetWorkOrderList(sEcho, iDisplayStart, iDisplayLength, sSearch);
            int totalRecord = wom.GetTotalCount();


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
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.NTE).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.NTE).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Account.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Account.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.WorkStage.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.WorkStage.Name).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;

                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
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
