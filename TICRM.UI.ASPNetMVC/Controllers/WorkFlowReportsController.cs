using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class WorkFlowReportsController : Controller
    {
        private WorkFlowReportManager workFlowReportManager = new WorkFlowReportManager();
        protected CRMEntities dbEnt = new CRMEntities();
        private WorkFlowManager wfm = new WorkFlowManager();
        private AccountManager ac = new AccountManager();
        private DeviceManager dvc = new DeviceManager();
        private DeviceManager dm = new DeviceManager();
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                List<WorkFlowReportDTO> workFlowReportDTOs = workFlowReportManager.GetWorkFlowReports(CurrentUserId, UserRole, UserCompanyID);
                return View(workFlowReportDTOs);
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
                WorkFlowReportDTO workFlowReportDTO = new WorkFlowReportDTO();
                workFlowReportDTO.WorkFlowIdDropdown= new SelectList(wfm.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name");
                workFlowReportDTO.AccountIdDropdown= new SelectList(ac.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workFlowReportDTO.DeviceIdDropdown= new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");


                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        /// <summary>
        /// POST request to create Work Flow Reports, Receive object of,
        /// new workflow Report validate it and creates a new workflow report
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkFlowReportDTO workFlowReportDTO)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    //string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID

                    bool condition = workFlowReportManager.SaveItWorkFlowReport(workFlowReportDTO, CurrentUserId, UserCompanyID, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Work flow report create message
                        TempData["Success"] = SuccessMessage.WorkFlowReportSubmit;

                        return RedirectToAction("Index");
                    }
                }
                workFlowReportDTO.WorkFlowIdDropdown = new SelectList(wfm.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name");
                workFlowReportDTO.AccountIdDropdown = new SelectList(ac.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workFlowReportDTO.DeviceIdDropdown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workFlowReportDTO);
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
                WorkFlowReportDTO workFlowReportDTO = workFlowReportManager.GetWorkFlowReportOnId(id);
                if (workFlowReportDTO == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                workFlowReportDTO.WorkFlowIdDropdown = new SelectList(wfm.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name");
                workFlowReportDTO.AccountIdDropdown = new SelectList(ac.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workFlowReportDTO.DeviceIdDropdown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                return View(workFlowReportDTO);
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
        public ActionResult Edit(WorkFlowReportDTO workFlowReportDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = workFlowReportManager.SaveItWorkFlowReport(workFlowReportDTO, CurrentUserId,null, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Work flow report Update message
                        TempData["Success"] = UpdateMessage.WorkFlowReportUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                workFlowReportDTO.WorkFlowIdDropdown = new SelectList(wfm.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name");
                workFlowReportDTO.AccountIdDropdown = new SelectList(ac.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workFlowReportDTO.DeviceIdDropdown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");


                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workFlowReportDTO);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult WorkFlowReportsDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Work Flow Reports Detail
                WorkFlowReportDTO workFlowReport= workFlowReportManager.GetWorkFlowReportOnId(id);

                if (workFlowReport == null)  //If Work Flow Reports detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(workFlowReport);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                WorkFlowReportDTO workFlowReportDTO = workFlowReportManager.GetWorkFlowReportOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = workFlowReportManager.SaveItWorkFlowReport(workFlowReportDTO, CurrentUserId,null, true, true);
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
    }
}