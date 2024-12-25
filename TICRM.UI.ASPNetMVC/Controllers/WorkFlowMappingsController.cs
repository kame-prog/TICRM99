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
    public class WorkFlowMappingsController : Controller
    {

        private WorkFlowMappingManager workFlowMappingManager = new WorkFlowMappingManager();

        private WorkFlowManager workFlowManager = new WorkFlowManager();

        // GET: WorkFlowMappings
        public ActionResult Index()
        {
            try
            {
                return View(workFlowMappingManager.GetWorkFlowMappingList());
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
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                WorkFlowMappingDTO workflowMapping = new WorkFlowMappingDTO();
                workflowMapping.WorkflowDropdown = new SelectList(workFlowManager.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name");
                workflowMapping.SourceTypeDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                workflowMapping.DestinationTypeDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                workflowMapping.SourceColumnDropdown = new SelectList("");
                workflowMapping.DestinationColumnDropdown = new SelectList("");
                workflowMapping.ActionDropdown = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text");
                return View(workflowMapping);
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
        public ActionResult Create(WorkFlowMappingDTO workFlowMappingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = workFlowMappingManager.SaveWorkFlowMapping(workFlowMappingDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Work flow mapping success message
                        TempData["Success"] = SuccessMessage.WorkFlowMappingSubmit;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                ViewBag.WorkFlowId = new SelectList(workFlowManager.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name", workFlowMappingDTO.WorkFlowId);
                ViewBag.SourceColumn = new SelectList("");

                ViewBag.Action = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text", workFlowMappingDTO.Action);

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workFlowMappingDTO);
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
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                if (workFlowMappingDTO == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                workFlowMappingDTO.WorkflowDropdown = new SelectList(workFlowManager.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name", workFlowMappingDTO.WorkFlowId);
                workFlowMappingDTO.SourceTypeDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", workFlowMappingDTO.SourceType);
                workFlowMappingDTO.ActionDropdown = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text", workFlowMappingDTO.Action);


                return View(workFlowMappingDTO);
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
        public ActionResult Edit(WorkFlowMappingDTO workFlowMappingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = workFlowMappingManager.SaveWorkFlowMapping(workFlowMappingDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                   
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = UpdateMessage.WorkFlowMappingUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                ViewBag.WorkFlowId = new SelectList(workFlowManager.WorkFlowDropDown(UserCompanyID), "WorkFlowId", "Name", workFlowMappingDTO.WorkFlowId);
                ViewBag.SourceType = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", workFlowMappingDTO.SourceType);
                ViewBag.Action = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text", workFlowMappingDTO.Action);

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult WorkFlowMappingDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Work Flow Mapping Detail
               WorkFlowMappingDTO workFlowMapping = workFlowMappingManager.GetWorkFlowMappingOnId(id);

                if (workFlowMapping == null)  //If Work Flow Mapping detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(workFlowMapping);
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
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = workFlowMappingManager.SaveWorkFlowMapping(workFlowMappingDTO, CurrentUserId, true, true);
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