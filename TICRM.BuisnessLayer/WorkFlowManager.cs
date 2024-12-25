using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;
using static TICRM.DTOs.WFDesignerViewModel;

namespace TICRM.BuisnessLayer
{
    /****************************************************************************************
    ||  Class [WorkFlowManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, updating and saving workflows, getting workflows specifically on
    ||             Id, Converting the workflows to be viewed in designer]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||                  [21/08/2020     Added Sorting to the pagination method       Akhtar Zaman]
    ****************************************************************************************/
    public class WorkFlowManager : BaseManager
    {
        /// <summary>
        /// Gets the work flows.
        /// </summary>
        /// <returns>List&lt;WorkFlowDTO&gt;.</returns>
        public List<WorkFlowDTO> GetWorkFlows(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetWorkFlows", EventType.Log, EventColor.yellow, "going to Get WorkFlows list", "TICRM.BusinessLayer.WorkFlowManager.GetWorkFlows", "");
                List<WorkFlowDTO> workFlowDTOs = new List<WorkFlowDTO>(); // create list Object of workflow DTO

                //List<WorkFlow> workFlows = dbEnt.WorkFlows.Include(x => x.Team).Include(x => x.User).Include(x => x.WorkFlowMappings).Include(x => x.WorkFlowReports).Where(x => x.IsDeleted != true).ToList(); // declare a local variable to Get List Of workflows from DB
                List<WorkFlow> workFlows =dbEnt.sp_WorkFlows_Get(CurrentUserId, UserRole, UserCompany).ToList();
                // apply iteration on Workflow
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    workFlowDTOs.Add(objMapper.GetWorkFlowDTO(item)); // add in a list object
                }
                return workFlowDTOs; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkFlows", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowManager.GetWorkFlows", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        public List<WorkFlowDTO> GetAccountWorkflows(string AccId)
        {
            try
            {
                InsertEventLog("GetAccountWorkflows", EventType.Log, EventColor.yellow, "going to Get WorkFlows list based on account", "TICRM.BusinessLayer.WorkFlowManager.GetAccountWorkflows", "");
                List<WorkFlowDTO> workFlowDTOs = new List<WorkFlowDTO>(); // create list Object of workflow DTO

                List<WorkFlow> workFlows = dbEnt.WorkFlows.Where(x => x.AccountId == AccId && x.IsDeleted != true).ToList(); // declare a local variable to Get List Of workflows from DB
                // apply iteration on Workflow
                foreach (WorkFlow item in workFlows.CollectionNotNull())
                {
                    workFlowDTOs.Add(objMapper.GetWorkFlowDTO(item)); // add in a list object
                }
                return workFlowDTOs; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountWorkflows", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowManager.GetAccountWorkflows", "");
                throw;
            }
        }

        /// <summary>
        /// Saves it work flow.
        /// </summary>
        /// <param name="workFlowDTO">The work flow dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="loc">The loc.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveItWorkFlow(WorkFlowDTO workFlowDTO, string CurrentUserId,string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "enter", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                WorkFlow workFlow; // create a new object
                if (isEditMode) // check if is is edit mode is true
                {
                    workFlow = objMapper.GetWorkFlow(workFlowDTO);
                    WorkFlow WorkFlowDB = dbEnt.WorkFlows.FirstOrDefault(x => x.WorkFlowId == workFlow.WorkFlowId); // get data from database and pass in new activity class object

                    if (WorkFlowDB != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "going to delete workflow", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                            WorkFlow workflowDelete = dbEnt.WorkFlows.FirstOrDefault(x => x.WorkFlowId == workFlow.WorkFlowId);
                            workflowDelete.IsDeleted = true;
                            //dbEnt.Entry(workflowDelete).State = EntityState.Modified;
                        }
                        else
                        {
                            InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "going to update workflow", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                            WorkFlow workflowEdit = dbEnt.WorkFlows.FirstOrDefault(x => x.WorkFlowId == workFlow.WorkFlowId);
                            workflowEdit.Name = workFlow.Name;
                            workflowEdit.TargetOn = workFlow.TargetOn;
                            workflowEdit.Description = workFlow.Description;
                            workflowEdit.Priority = workFlow.Priority;
                            workflowEdit.UpdatedDate = DateTime.Now;
                            workflowEdit.UpdatedBy = CurrentUserId;
                            workflowEdit.Action = workFlow.Action;
                            workflowEdit.AccountId = workFlow.AccountId;
                            workflowEdit.DeviceName = workFlow.DeviceName;
                            workflowEdit.Threshold = workFlowDTO.MinThreshold + "," + workFlowDTO.Threshold;
                            //workflowEdit.TriggerCondition = workFlow.TriggerCondition;
                            //workflowEdit.TriggerIn = workFlow.TriggerIn;
                            //workflowEdit.TriggerOut = workFlow.TriggerOut;
                            //workflowEdit.Frequency = workFlow.Frequency;
                            //workflowEdit.FrequencyOut = workFlow.FrequencyOut;
                            //workflowEdit.WorkFlowStatus = workFlow.WorkFlowStatus;
                            //workflowEdit.AppliedTo = workFlow.AppliedTo;
                            //workflowEdit.AssignedUser = workFlow.AssignedUser;
                            //workflowEdit.AssignedTeam = workFlow.AssignedTeam;

                        }
                    }
                    else
                    {
                        InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "work flow data is null on id" + workFlowDTO.WorkFlowId + "", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                        return false; // return false if no any condition found for edit and delete

                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "WorkFlow Is Updated Successfully", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "Going to create new Record of work flow", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                    workFlow = objMapper.GetWorkFlow(workFlowDTO);
                    workFlow.WorkFlowId = Guid.NewGuid();
                    workFlow.Company = Guid.Parse(UserCompanyID);
                    workFlow.CreatedBy = CurrentUserId;
                    workFlow.CreatedDate = DateTime.Now;
                    if (workFlow.WorkFlowDesign == null)
                    {
                        workFlow.WorkFlowDesign = ConvertToDesigner(workFlowDTO);
                    }

                    dbEnt.WorkFlows.Add(workFlow); //add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SaveItWorkFlow", EventType.Log, EventColor.yellow, "new WorkFlow Is Save Successfully", "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveItWorkFlow", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowManager.SaveItWorkFlow", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;

        }

        /// <summary>
        /// Converts to designer.
        /// </summary>
        /// <param name="workFlowDTO">The work flow dto.</param>
        /// <returns>System.String.</returns>
        public string ConvertToDesigner(WorkFlowDTO workFlowDTO)
        {
            InsertEventLog("ConvertToDesigner", EventType.Log, EventColor.yellow, "enter in to convert workflow to design", "TICRM.BusinessLayer.WorkFlowManager.ConvertToDesigner", "");
            workflowDesigner designer = new workflowDesigner();
            designer.@class = "go.GraphLinksModel";
            designer.nodeKeyProperty = "text";
            designer.linkKeyProperty = "iterate";


            List<WorkFlowNodeDTO> nodeList = new List<WorkFlowNodeDTO>();

            WorkFlowNodeDTO nodeDataStart = new WorkFlowNodeDTO();
            //nodeDataStart.NodeDataId = Guid.NewGuid();
            nodeDataStart.text = "Start";
            nodeDataStart.key = "Start";
            nodeDataStart.figure = "Circle";
            nodeDataStart.fill = "#00AD5F";
            nodeDataStart.loc = "175 0";
            nodeList.Add(nodeDataStart);



            WorkFlowNodeDTO nodeDataTrigger = new WorkFlowNodeDTO();
            //nodeDataTrigger.NodeDataId = Guid.NewGuid();
            //nodeDataTrigger.text = workFlowDTO.TriggerCondition == null ? "" : workFlowDTO.TriggerCondition;
            //nodeDataTrigger.key = workFlowDTO.TriggerCondition == null ? "" : workFlowDTO.TriggerCondition;
            nodeDataTrigger.figure = "";
            nodeDataTrigger.fill = "white";
            nodeDataTrigger.loc = "175 100";
            nodeList.Add(nodeDataTrigger);




            WorkFlowNodeDTO nodeDataAppliedTo = new WorkFlowNodeDTO();
            //nodeDataAppliedTo.NodeDataId = Guid.NewGuid();
            //nodeDataAppliedTo.text = workFlowDTO.AppliedTo == null ? "" : workFlowDTO.AppliedTo;
            //nodeDataAppliedTo.key = workFlowDTO.AppliedTo == null ? "" : workFlowDTO.AppliedTo;
            nodeDataAppliedTo.figure = "";
            nodeDataAppliedTo.fill = "white";
            nodeDataAppliedTo.loc = "175 300";
            nodeList.Add(nodeDataAppliedTo);


            WorkFlowNodeDTO nodeDataTargetOn = new WorkFlowNodeDTO();
            //nodeDataTargetOn.NodeDataId = Guid.NewGuid();
            nodeDataTargetOn.text = workFlowDTO.TargetOn == null ? "" : workFlowDTO.TargetOn;
            nodeDataTargetOn.key = workFlowDTO.TargetOn == null ? "" : workFlowDTO.TargetOn;
            nodeDataTargetOn.figure = "Database";
            nodeDataTargetOn.fill = "lightgray";
            nodeDataTargetOn.loc = "375 300";
            nodeList.Add(nodeDataTargetOn);


            WorkFlowNodeDTO nodeDataAction = new WorkFlowNodeDTO();
            //nodeDataAppliedTo.NodeDataId = Guid.NewGuid();
            nodeDataAction.text = workFlowDTO.Action == null ? "" : workFlowDTO.Action;
            nodeDataAction.key = workFlowDTO.Action == null ? "" : workFlowDTO.Action;
            nodeDataAction.figure = "Diamond";
            nodeDataAction.fill = "white";
            nodeDataAction.loc = "575 300";
            nodeList.Add(nodeDataAction);


            designer.nodeDataArray = nodeList;

            List<LinkDataArray> link = new List<LinkDataArray>();

            int count = designer.nodeDataArray.Count;
            int loop = 0;


            for (int i = 1; i < designer.nodeDataArray.Count; i++)
            {
                LinkDataArray obj = new LinkDataArray();
                obj.from = designer.nodeDataArray[i - 1].text;
                obj.to = designer.nodeDataArray[i].text;
                obj.iterate = -i;
                link.Add(obj);
            }
            designer.linkDataArray = link;

            designer.Name = workFlowDTO.Name;
            designer.Description = workFlowDTO.Description;
            designer.Priority = workFlowDTO.Priority;
            //designer.Frequency = workFlowDTO.Frequency;
            //designer.WorkFlowStatus = workFlowDTO.WorkFlowStatus;




            string workflow = new JavaScriptSerializer().Serialize(designer);
            return workflow;
        }

        /// <summary>
        /// Gets the work flow on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>WorkFlowDTO.</returns>
        public WorkFlowDTO GetWorkFlowOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetWorkFlowOnId", EventType.Log, EventColor.yellow, "to get WorkFlowDTO on id", "TICRM.BusinessLayer.WorkFlowManager.GetWorkFlowOnId", "");
                return objMapper.GetWorkFlowDTO(dbEnt.WorkFlows.Find(guid)); // get workflow on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkFlowOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowManager.GetWorkFlowOnId", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// getting workflows counts for pagination
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Workflows", "TICRM.BuisnessLayer.WorkFlowsManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.WorkFlows.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowsManager.GetTotalCount", "");
                throw ex;
            }
        }
        /// <summary>
        /// Getting data for pagination
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns></returns>
        public List<WorkFlowDTO> GetWorkflowList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetWorkflowList", EventType.Log, EventColor.yellow, "Get List of Workflows Based on Datatable Query", "TICRM.BuisnessLayer.WorkFlowsManager.GetWorkflowList", "");

                var workflowDto = new List<WorkFlowDTO>();
                var workflows = new List<WorkFlow>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings

                if (!string.IsNullOrEmpty(sSearch))
                {
                    workflows = dbEnt.WorkFlows.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.TriggerCondition.ToString().ToLower().Contains(sSearch)
                    || a.TriggerIn.ToString().ToLower().Contains(sSearch)
                    || a.TriggerOut.ToString().ToLower().Contains(sSearch)
                    || a.TargetOn.ToString().ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.AppliedTo.ToString().ToString().ToLower().Contains(sSearch)
                    || a.Priority.ToString().Contains(sSearch)
                    || a.Frequency.ToString().Contains(sSearch)
                    || a.AssignedUser.Value.ToString().Contains(sSearch)
                    || a.AssignedTeam.Value.ToString().Contains(sSearch)
                    ).OrderBy(x => x.Name).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    workflows = dbEnt.WorkFlows.OrderBy(x => x.Name).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (WorkFlow item in workflows.CollectionNotNull())
                {
                    workflowDto.Add(objMapper.GetWorkFlowDTO(item)); // add in a list object
                }



                return workflowDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventLogList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");

                throw ex;
            }

        }

       


    }
}
