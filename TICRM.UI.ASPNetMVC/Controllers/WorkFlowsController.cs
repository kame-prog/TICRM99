using log4net;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;
using static TICRM.DTOs.WFDesignerViewModel;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class WorkFlowsController : Controller
    {
        private WorkFlowManager wfm = new WorkFlowManager();
        private AccountManager am = new AccountManager();
        private DeviceSensorGraphManager dsgm = new DeviceSensorGraphManager();
        private DeviceManager dm = new DeviceManager();

        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                //Get work flow in list form
                List<WorkFlowDTO> workFlows = wfm.GetWorkFlows(CurrentUserId, UserRole, UserCompanyID);
                return View(workFlows);
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
                //Bind dropdown values with create form
                WorkFlowDTO workflow = new WorkFlowDTO();
                workflow.SensorsDropDown = new SelectList(dsgm.SensorDropDown(), "SensorId", "SensorName");
                workflow.AccountsDropdown = new SelectList(am.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workflow.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "Name", "Name");
                workflow.ActionDropdown = new SelectList(wfm.ActionDropDown(), "Name", "Name");
                workflow.PriorityDropDown = new SelectList(wfm.PriorityDropDown(), "Value", "Name");
                return View(workflow);
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
        public ActionResult Create(WorkFlowDTO workFlow)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    //string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID

                    bool condition = wfm.SaveItWorkFlow(workFlow, CurrentUserId, UserCompanyID, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = SuccessMessage.WorkFlowSubmit;
                        return RedirectToAction("Index");
                    }
                }

                //Bind dropdown values with create form
                WorkFlowDTO workflow = new WorkFlowDTO();
                workflow.SensorsDropDown = new SelectList(dsgm.SensorDropDown(), "SensorId", "SensorName");
                workflow.AccountsDropdown = new SelectList(am.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workflow.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "Name", "Name");
                workflow.ActionDropdown = new SelectList(wfm.ActionDropDown(), "Name", "Name");
                workflow.PriorityDropDown = new SelectList(wfm.PriorityDropDown(), "Value", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workflow);
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
                WorkFlowDTO workflow = wfm.GetWorkFlowOnId(id);
                if (workflow == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Bind dropdown values with create form
                workflow.SensorsDropDown = new SelectList(dsgm.SensorDropDown(), "SensorId", "SensorName");
                workflow.AccountsDropdown = new SelectList(am.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workflow.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "Name", "Name");
                workflow.ActionDropdown = new SelectList(wfm.ActionDropDown(), "Name", "Name");
                workflow.PriorityDropDown = new SelectList(wfm.PriorityDropDown(), "Value", "Name");
                return View(workflow);
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
        public ActionResult Edit(WorkFlowDTO workflow)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = wfm.SaveItWorkFlow(workflow, CurrentUserId, null, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        TempData["Success"] = UpdateMessage.WorkFlowUpdate;
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Bind dropdown values with create form
                workflow.SensorsDropDown = new SelectList(dsgm.SensorDropDown(), "SensorId", "SensorName");
                workflow.AccountsDropdown = new SelectList(am.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workflow.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "Name", "Name");
                workflow.ActionDropdown = new SelectList(wfm.ActionDropDown(), "Name", "Name");
                workflow.PriorityDropDown = new SelectList(wfm.PriorityDropDown(), "Value", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(workflow);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        public ActionResult WorkFlowDetail(Guid? id)
        {
            try
            {
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting Work Flow Detail
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);

                if (workFlow == null)  //If Work Flow detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(workFlow);
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
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                //soft delete WorkFlow
                bool condition = wfm.SaveItWorkFlow(workFlow, CurrentUserId, null, true, true);
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

        public ActionResult WorkFlowDesigner(Guid? id)
        {
            try
            {
                ViewBag.id = "";
                if (id != null)
                {
                    ViewBag.id = id;
                }

                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID

                //Bind dropdown values with create form
                
                ViewBag.SensorsDropDown  = new SelectList(dsgm.SensorDropDown(), "SensorId", "SensorName");
                ViewBag.AccountsDropdown = new SelectList(am.AccountDropDown(UserCompanyID), "AccountId", "Name");
                ViewBag.DevicesDropDown  = new SelectList(dm.DeviceDropDown(UserCompanyID), "Name", "Name");
                ViewBag.ActionDropdown   = new SelectList(wfm.ActionDropDown(), "Name", "Name");
                ViewBag.PriorityDropDown = new SelectList(wfm.PriorityDropDown(), "Value", "Name");

                return View();
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
        public ActionResult WorkFlowFromDesigner(string mySavedModel, string WFName, string Priority, string Account, string Sensor, string Device, string Action, string WFDescription)
        {
            try
            {
                var workflow = SaveWorkFlowFromDesigner(mySavedModel, WFName, Priority, Account, Sensor, Device, Action, WFDescription);
                TempData["Success"] = SuccessMessage.WorkFlowSubmit;

                // Perform some logic

                // Assuming the redirect URL is dynamically generated
                string redirectToUrl = "/workflows/Index";

                // Create an anonymous object with the redirectTo property
                var responseObj = new { redirectTo = redirectToUrl };

                // Return the JSON response
                return Json(responseObj, JsonRequestBehavior.AllowGet);

                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public string SaveWorkFlowFromDesigner(string mySavedModel, string WFName, string Priority, string Account, string Sensor, string Device, string Action, string WFDescription)
        {
            try
            {
                WorkFlowDTO workFlowDTO = new WorkFlowDTO();

                workflowDesigner data = Newtonsoft.Json.JsonConvert.DeserializeObject(mySavedModel) as workflowDesigner;
                workflowDesigner workflowDesigner = new JavaScriptSerializer().Deserialize<workflowDesigner>(mySavedModel);

                List<WorkFlowNodeDTO> list = workflowDesigner.nodeDataArray;

                foreach (WorkFlowNodeDTO item in list)
                {
                    if (item.text == RelatedToEnum.Account.ToString() || item.text == RelatedToEnum.Leads.ToString() || item.text == RelatedToEnum.Oppertunities.ToString())
                    {
                        workFlowDTO.TargetOn = item.text;
                        workflowDesigner.TargetOn = item.text;
                    }


                    if (item.text == TrigegrConditionConstant.Pre_Event || item.text == TrigegrConditionConstant.Post_Event)
                    {
                        workFlowDTO.TriggerCondition = item.text;
                        workflowDesigner.TriggerCondition = item.text;
                    }

                    if (item.text == appliedToConstant.OnCreate || item.text == appliedToConstant.OnUpdate || item.text == appliedToConstant.CreateAndUpdate)
                    {
                        workFlowDTO.AppliedTo = TrigegrConditionConstant.Pre_Event;
                        workflowDesigner.AppliedTo = TrigegrConditionConstant.Pre_Event;
                    }

                    if (WFActionConstant.Email == item.text || WFActionConstant.Meeting == item.text || WFActionConstant.Note == item.text || WFActionConstant.Alert == item.text)
                    {
                        workFlowDTO.Action = item.text;
                        workflowDesigner.Action = item.text;
                    }
                }

                workflowDesigner.Name = WFName;
                workflowDesigner.AccountId = Account;
                workflowDesigner.Description = WFDescription;
                workflowDesigner.Priority = Convert.ToInt32(Priority);
                //workflowDesigner.Frequency = Convert.ToInt32(Frequency);
                //workflowDesigner.TriggerIn = Convert.ToDateTime(TriggerIn);
                //workflowDesigner.TriggerOut = Convert.ToDateTime(TriggerOut);
                workflowDesigner.WorkFlowStatus = WFStatusConstant.Paused;

                workFlowDTO.Name = WFName;
                workFlowDTO.AccountId = Account;
                workFlowDTO.Description = WFDescription;
                workFlowDTO.TargetOn = Sensor;
                workFlowDTO.Priority = Convert.ToInt32(Priority);
                workFlowDTO.DeviceName = Device;
                workFlowDTO.Action = Action;

                //workFlowDTO.Frequency = Convert.ToInt32(Frequency);
                //workFlowDTO.TriggerIn = Convert.ToDateTime(TriggerIn);
                //workFlowDTO.TriggerOut = Convert.ToDateTime(TriggerOut);
                workFlowDTO.WorkFlowStatus = WFStatusConstant.Paused;
                workFlowDTO.WorkFlowDesign = new JavaScriptSerializer().Serialize(workflowDesigner);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                //bool condition = wfm.SaveItWorkFlow(workFlowDTO, CurrentUserId, accountId, false, false);

                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
               /* string CurrentUserId = User.Identity.GetUserId(); */             // pass current userid
                bool conditionn = wfm.SaveItWorkFlow(workFlowDTO, CurrentUserId, UserCompanyID, false, false);

                string workflow = new JavaScriptSerializer().Serialize(workflowDesigner);
             
                return workflow;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


    }
}