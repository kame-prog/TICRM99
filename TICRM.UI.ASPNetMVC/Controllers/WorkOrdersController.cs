using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class WorkOrdersController : Controller
    {
        WorkOrderManager wom = new WorkOrderManager();

        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                List<WorkOrderDto> workOrderDto = wom.GetWorkOrders(CurrentUserId, UserRole, UserCompanyID);
                return View(workOrderDto);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }


        public ActionResult Create()
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Bind dropdown data
                WorkOrderDto workorder = new WorkOrderDto();
                workorder.WorkStagesDropdown = new SelectList(wom.WorkStageDropDown(), "WorkStageId", "Name");
                workorder.StatusDropdown = new SelectList(wom.StatusDropDown(), "StatusId", "Name");
                workorder.AssignedUserDropdown = new SelectList(wom.UserDropDown(), "UserId", "Name");
                workorder.AssignedTeamDropdown = new SelectList(wom.TeamDropDown(), "TeamId", "Name");
                workorder.AccountsDropdown = new SelectList(wom.AccountDropDown(UserCompanyID), "AccountId", "Name");
                return View(workorder);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkOrderDto workorder)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                   /* string UserCompanyID = Convert.ToString(Session["UserCompany"]);*/ //Pass Company ID
                    //Work Order Create function
                    bool condition = wom.SaveWorkOrder(workorder, CurrentUserId, UserCompanyID);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = SuccessMessage.WorkOrderSubmit;
                        return RedirectToAction("Index");
                    }
                }
                //Bind dropdown data
                workorder.WorkStagesDropdown = new SelectList(wom.WorkStageDropDown(), "WorkStageId", "Name");
                workorder.StatusDropdown = new SelectList(wom.StatusDropDown(), "StatusId", "Name");
                workorder.AssignedUserDropdown = new SelectList(wom.UserDropDown(), "UserId", "Name");
                workorder.AssignedTeamDropdown = new SelectList(wom.TeamDropDown(), "TeamId", "Name");
                workorder.AccountsDropdown = new SelectList(wom.AccountDropDown(UserCompanyID), "AccountId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workorder);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var workorder = wom.GetWorkOrder(id);
                if (workorder == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                //Bind dropdown data
                workorder.WorkStagesDropdown = new SelectList(wom.WorkStageDropDown(), "WorkStageId", "Name");
                workorder.StatusDropdown = new SelectList(wom.StatusDropDown(), "StatusId", "Name");
                workorder.AssignedUserDropdown = new SelectList(wom.UserDropDown(), "UserId", "Name");
                workorder.AssignedTeamDropdown = new SelectList(wom.TeamDropDown(), "TeamId", "Name");
                workorder.AccountsDropdown = new SelectList(wom.AccountDropDown(UserCompanyID), "AccountId", "Name");

                return View(workorder);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkOrderDto workorder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    //Edit WorK Order function
                    bool condition = wom.SaveWorkOrder(workorder, CurrentUserId,null, true);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = UpdateMessage.WorkOrderUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Bind dropdown data
                workorder.WorkStagesDropdown = new SelectList(wom.WorkStageDropDown(), "WorkStageId", "Name");
                workorder.StatusDropdown = new SelectList(wom.StatusDropDown(), "StatusId", "Name");
                workorder.AssignedUserDropdown = new SelectList(wom.UserDropDown(), "UserId", "Name");
                workorder.AssignedTeamDropdown = new SelectList(wom.TeamDropDown(), "TeamId", "Name");
                workorder.AccountsDropdown = new SelectList(wom.AccountDropDown(UserCompanyID), "AccountId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workorder);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult WorkOrderDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Work order Detail
              WorkOrderDto workOrder = wom.GetWorkOrder(id);

                if (workOrder == null)  //If Work Order detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(workOrder);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult Delete(Guid id)
        {
            try
            {
                var workOrder = wom.GetWorkOrder(id);
                //Soft delete work order
                wom.SaveWorkOrder(workOrder,null,null, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

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