using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using System.Linq;
using static TICRM.DTOs.WFDesignerViewModel;
using System.Text;
using Newtonsoft.Json;

namespace TICRM.Controllers
{
    /************WorkFlows Controller************
    Class [WorkFlowsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with WorkFlows like, 
    ||             navigating to the pages, getting associated modules for specific WorkFlow]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||  Changes Made:   [13/8/2020      Added Method for Datatable Pagination Akhtar Zaman]
     ********************************************/
    public class WorkFlowsController : BaseController
    {

        private WorkFlowManager wfm = new WorkFlowManager();
        private AccountManager am = new AccountManager();

        /// <summary>
        /// View for index page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                return View(wfm.GetWorkFlows());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Workflow designer action result method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult WorkFlowDesigner(Guid? id)
        {
            try
            {
                ViewBag.id = "";
                if (id != null)
                {
                    ViewBag.id = id;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Editing workflow from the designer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string EditFromDesigner(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return "";
                }
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                if (workFlow.WorkFlowDesign == null)
                {
                    return wfm.ConvertToDesigner(workFlow);
                }
                return workFlow.WorkFlowDesign;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Saving workflow from a designer
        /// </summary>
        /// <param name="mySavedModel"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="Priority"></param>
        /// <param name="Frequency"></param>
        /// <param name="TriggerIn"></param>
        /// <param name="TriggerOut"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public string SaveWorkFlowFromDesigner(string mySavedModel, string Name, string Description, string Priority, string Frequency, string TriggerIn, string TriggerOut, string accountId)
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

                workflowDesigner.Name = Name;
                workflowDesigner.Description = Description;
                workflowDesigner.Priority = Convert.ToInt32(Priority);
                workflowDesigner.Frequency = Convert.ToInt32(Frequency);
                workflowDesigner.TriggerIn = Convert.ToDateTime(TriggerIn);
                workflowDesigner.TriggerOut = Convert.ToDateTime(TriggerOut);
                workflowDesigner.WorkFlowStatus = WFStatusConstant.Paused;

                workFlowDTO.Name = Name;
                workFlowDTO.Description = Description;
                workFlowDTO.Priority = Convert.ToInt32(Priority);
                workFlowDTO.Frequency = Convert.ToInt32(Frequency);
                workFlowDTO.TriggerIn = Convert.ToDateTime(TriggerIn);
                workFlowDTO.TriggerOut = Convert.ToDateTime(TriggerOut);
                workFlowDTO.WorkFlowStatus = WFStatusConstant.Paused;
                workFlowDTO.WorkFlowDesign = new JavaScriptSerializer().Serialize(workflowDesigner);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = wfm.SaveItWorkFlow(workFlowDTO, CurrentUserId, accountId, false, false);

                string workflow = new JavaScriptSerializer().Serialize(workflowDesigner);

                return workflow;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Detail Page for workflow
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                if (workFlow == null)
                {
                    return HttpNotFound();
                }
                return View(workFlow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial details page for workflow
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                return PartialView("_PartialWorkFlowsDetails", workFlow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial delete page for workflow
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                return PartialView("_PartialWorkFlowsDelete", workFlow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Workflow create page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            try
            {
                WorkFlowDTO workflow = new WorkFlowDTO();
                workflow.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name");
                workflow.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name");

                workflow.TriggerConditionDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = TrigegrConditionConstant.Post_Event, Value = TrigegrConditionConstant.Post_Event},
        new SelectListItem { Text = TrigegrConditionConstant.Pre_Event, Value = TrigegrConditionConstant.Pre_Event}}, "Value", "Text");

                workflow.AppliedToDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = appliedToConstant.OnCreate, Value = appliedToConstant.OnCreate},
        new SelectListItem { Text = appliedToConstant.OnUpdate, Value = appliedToConstant.OnUpdate},
        new SelectListItem { Text = appliedToConstant.CreateAndUpdate, Value = appliedToConstant.CreateAndUpdate}}, "Value", "Text");

                workflow.ActionDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = WFActionConstant.Email, Value = WFActionConstant.Email},
        new SelectListItem { Text = WFActionConstant.Alert, Value = WFActionConstant.Alert},
        new SelectListItem { Text = WFActionConstant.Meeting, Value = WFActionConstant.Meeting},
        new SelectListItem { Text = WFActionConstant.Note, Value = WFActionConstant.Note},
        new SelectListItem { Text = WFActionConstant.Account, Value = WFActionConstant.Account},
        new SelectListItem { Text = WFActionConstant.WorkOrder, Value = WFActionConstant.WorkOrder}
                },
            "Value", "Text");
                workflow.RelatedToIdDropdown = new SelectList("");

                workflow.TargetOnDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                return View(workflow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Workflow creaet page action
        /// </summary>
        /// <param name="workFlow"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkFlowDTO workFlow, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = wfm.SaveItWorkFlow(workFlow, CurrentUserId, loc, false, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = workFlow.Name + " WorkFlow not Created.";
                        TempData["FormSubmissionStatus"] = "error";
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = workFlow.Name + " WorkFlow Created successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.AssignedTeam = new SelectList(am.Teams, "TeamId", "Name", workFlow.AssignedTeam);
                ViewBag.AssignedUser = new SelectList(am.Users, "UserId", "Name", workFlow.AssignedUser);

                ViewBag.TriggerCondition = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = TrigegrConditionConstant.Post_Event, Value = TrigegrConditionConstant.Post_Event},
        new SelectListItem { Text = TrigegrConditionConstant.Pre_Event, Value = TrigegrConditionConstant.Pre_Event}}, "Value", "Text", workFlow.TriggerCondition);

                ViewBag.AppliedTo = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = appliedToConstant.OnCreate, Value = appliedToConstant.OnCreate},
        new SelectListItem { Text = appliedToConstant.OnUpdate, Value = appliedToConstant.OnUpdate},
        new SelectListItem { Text = appliedToConstant.CreateAndUpdate, Value = appliedToConstant.CreateAndUpdate}}, "Value", "Text", workFlow.AppliedTo);


                ViewBag.Action = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = WFActionConstant.Email, Value = WFActionConstant.Email},
        new SelectListItem { Text = WFActionConstant.Alert, Value = WFActionConstant.Alert},
        new SelectListItem { Text = WFActionConstant.Meeting, Value = WFActionConstant.Meeting},
        new SelectListItem { Text = WFActionConstant.Note, Value = WFActionConstant.Note},
            new SelectListItem { Text = WFActionConstant.Account, Value = WFActionConstant.Account},
            new SelectListItem { Text = WFActionConstant.SugarCRM, Value = WFActionConstant.SugarCRM},
            new SelectListItem { Text = WFActionConstant.Dyn, Value = WFActionConstant.Dyn}},
            "Value", "Text", workFlow.Action);

                ViewBag.TargetOn = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", workFlow.TargetOn);

                return View(workFlow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Workflow edit page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                workflow.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name");
                workflow.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name");

                workflow.TriggerConditionDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = TrigegrConditionConstant.Post_Event, Value = TrigegrConditionConstant.Post_Event},
        new SelectListItem { Text = TrigegrConditionConstant.Pre_Event, Value = TrigegrConditionConstant.Pre_Event}}, "Value", "Text");

                workflow.AppliedToDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = appliedToConstant.OnCreate, Value = appliedToConstant.OnCreate},
        new SelectListItem { Text = appliedToConstant.OnUpdate, Value = appliedToConstant.OnUpdate},
        new SelectListItem { Text = appliedToConstant.CreateAndUpdate, Value = appliedToConstant.CreateAndUpdate}}, "Value", "Text");

                workflow.ActionDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = WFActionConstant.Email, Value = WFActionConstant.Email},
        new SelectListItem { Text = WFActionConstant.Alert, Value = WFActionConstant.Alert},
        new SelectListItem { Text = WFActionConstant.Meeting, Value = WFActionConstant.Meeting},
        new SelectListItem { Text = WFActionConstant.Note, Value = WFActionConstant.Note},
        new SelectListItem { Text = WFActionConstant.Account, Value = WFActionConstant.Account}},
            "Value", "Text");

                workflow.TargetOnDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                return View(workflow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Workflow edit page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkFlowDTO workFlow)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = wfm.SaveItWorkFlow(workFlow, CurrentUserId, null, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = workFlow.Name + " WorkFlow not updated.";
                        TempData["FormSubmissionStatus"] = "error";
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = workFlow.Name + " WorkFlow updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.AssignedTeam = new SelectList(am.Teams, "TeamId", "Name", workFlow.AssignedTeam);
                ViewBag.AssignedUser = new SelectList(am.Users, "UserId", "Name", workFlow.AssignedUser);

                ViewBag.TriggerCondition = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = TrigegrConditionConstant.Post_Event, Value = TrigegrConditionConstant.Post_Event},
        new SelectListItem { Text = TrigegrConditionConstant.Pre_Event, Value = TrigegrConditionConstant.Pre_Event}}, "Value", "Text", workFlow.TriggerCondition);

                ViewBag.AppliedTo = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = appliedToConstant.OnCreate, Value = appliedToConstant.OnCreate},
        new SelectListItem { Text = appliedToConstant.OnUpdate, Value = appliedToConstant.OnUpdate},
        new SelectListItem { Text = appliedToConstant.CreateAndUpdate, Value = appliedToConstant.CreateAndUpdate}}, "Value", "Text", workFlow.AppliedTo);

                ViewBag.Action = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = WFActionConstant.Email, Value = WFActionConstant.Email},
        new SelectListItem { Text = WFActionConstant.Alert, Value = WFActionConstant.Alert},
        new SelectListItem { Text = WFActionConstant.Meeting, Value = WFActionConstant.Meeting},
        new SelectListItem { Text = WFActionConstant.Note, Value = WFActionConstant.Note},
            new SelectListItem { Text = WFActionConstant.Account, Value = WFActionConstant.Account}},
            "Value", "Text", workFlow.Action);

                ViewBag.TargetOn = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", workFlow.TargetOn);
                return View(workFlow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Workflow delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                if (workFlow == null)
                {
                    return HttpNotFound();
                }
                return View(workFlow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Workflow delete confirm Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                WorkFlowDTO workFlow = wfm.GetWorkFlowOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = wfm.SaveItWorkFlow(workFlow, CurrentUserId, null, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creating a workflow for device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult workflowDevice(String Name, String Sensor, String Thresh, String Action, int Priority, String desc, String AccountId, String DevName, String mac, String Cloud, String DeviceId)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid


                WorkFlowDTO workflow = new WorkFlowDTO();
                workflow.Threshold = Thresh;
                workflow.AccountId = AccountId;
                workflow.Name = Name;
                workflow.TriggerCondition = Sensor;
                workflow.Action = Action;
                workflow.Priority = Priority;
                workflow.Description = desc;
                workflow.DeviceName = DevName;
                workflow.DeviceMac = mac;
                workflow.Cloud = Cloud;
                workflow.RelatedToId = Guid.Parse(DeviceId);
                bool condition = wfm.SaveItWorkFlow(workflow, CurrentUserId, AccountId, false, false);

                string status = "error";

                //try
                //{

                //}
                //catch (Exception ex)
                //{

                //}
                return Content(status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Getting queue list for workflows
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetQueueList()
        {
            try
            {
                return Json(wfm.GetWorkFlows(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Getting Workflow Lists for pagination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetWorkflowList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();
            List<WorkFlowDTO> obj = wfm.GetWorkflowList(sEcho, iDisplayStart, iDisplayLength, sSearch);
            int totalRecord = wfm.GetTotalCount();

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
                        obj = obj.OrderBy(x => x.TriggerCondition).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.TriggerCondition).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.TriggerIn).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.TriggerOut).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.TargetOn).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.TargetOn).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AppliedTo).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AppliedTo).ToList();
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
                        obj = obj.OrderBy(x => x.Frequency).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Frequency).ToList();
                    }
                    break;
                case 8:
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
        /// Gets the related to value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="selectedvalue">The selectedvalue.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetRelatedToValue(string TargetOn)
        {
            try
            {
                AccountManager accountmangr = new AccountManager();
                CaseManager caseManager = new CaseManager();
                if (EntityTypes.Account.ToString() == TargetOn)
                {

                    var data = new SelectList( accountmangr.GetAccounts(), "AccountId", "Name");

                    return Json(data, JsonRequestBehavior.AllowGet);
                    //return Json(accountmangr.GetAccounts(), JsonRequestBehavior.AllowGet);
                }
                else if(EntityTypes.Cases.ToString() == TargetOn)
                {
                    var data = new SelectList(caseManager.GetCases(), "CaseId", "CaseTitle");

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    }
}
