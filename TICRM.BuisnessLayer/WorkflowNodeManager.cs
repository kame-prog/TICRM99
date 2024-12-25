using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /****************************************************************************************
    ||  Class [WorkflowNodeManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, updating and saving workflow nodes, getting workflow nodes 
    ||             specifically on Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class WorkflowNodeManager : BaseManager
    {
        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets all the Workflow Nodes and returns it in the list
        /// </summary>
        /// <returns></returns>
        public List<WorkFlowNodeDTO> GetWorkflowNodes()
        {
            try
            {
                InsertEventLog("GetWorkflowNode", EventType.Log, EventColor.yellow, "Successfully Enter in GetWorkflowNode", "TICRM.BusinessLayer.WorkflowNodeManager", "");

                List<WorkFlowNodeDTO> workflowNodeDto = new List<WorkFlowNodeDTO>(); // create list Object of WorkflowNode DTO

                List<WorkFlowNode> workflowNode = dbEnt.WorkFlowNodes.ToList(); // Get List Of WorkflowNode from DB
                // apply iteration on getting ReadingTypes
                foreach (WorkFlowNode item in workflowNode.CollectionNotNull())
                {
                    workflowNodeDto.Add(objMapper.GetWorkFlowNodeDto(item)); // add in a list object
                }
                return workflowNodeDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkflowNode", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkflowNodeManager", "");
                throw;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets a specific Workflow Node on id and returns it as an object
        /// </summary>
        /// <returns></returns>
        public WorkFlowNodeDTO GetWorkflowNode(Guid? guid)
        {
            try
            {
                InsertEventLog("GetWorkflowNode", EventType.Log, EventColor.yellow, "Successfully Enter in GetWorkflowNode to Get Data on id", "TICRM.BusinessLayer.WorkflowNodeManager", "");
                return objMapper.GetWorkFlowNodeDto(dbEnt.WorkFlowNodes.Find(guid)); // get WorkflowNode on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkflowNode", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkflowNodeManager", "");
                throw ex;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method save a specific Workflow Node on edit, create and delete actions 
        /// </summary>
        /// <returns></returns>
        public bool SaveWorkflowNode(WorkFlowNodeDTO workflowNodeDto, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveWorkflowNode", EventType.Log, EventColor.yellow, "Successfully Enter in SaveWorkflowNode", "TICRM.BusinessLayer.WorkflowNodeManager", "");

                WorkFlowNode workflow; // create a new object
                workflow = objMapper.GetWorkFlowNode(workflowNodeDto); // pass parameter object to WorkflowNode  object
                if (isEditMode) // check if is is edit mode is true
                {
                    WorkFlowNode dbData = dbEnt.WorkFlowNodes.FirstOrDefault(x => x.NodeDataId == workflow.NodeDataId); // get data from database and pass in new WorkflowNode  class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveWorkflowNode", EventType.Log, EventColor.yellow, "For Delete Successfully Enter in SaveWorkflowNode", "TICRM.BusinessLayer.WorkflowNodeManager", "");
                            dbEnt.WorkFlowNodes.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveWorkflowNode", EventType.Log, EventColor.yellow, "For Create Successfully Enter SaveWorkflowNode", "TICRMTICRM.BuisnessLayer.WorkflowNodeManager", "");
                            dbData.NodeDataId = workflow.NodeDataId;
                            dbData.text = workflow.text;
                            dbData.key = workflow.key;
                            dbData.loc = workflow.loc;
                            dbData.figure = workflow.figure;
                            dbData.fill = workflow.fill;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveWorkflowNode", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + workflowNodeDto.NodeDataId, "TICRM.BuisnessLayer.WorkflowNodeManager", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveWorkflowNode", EventType.Log, EventColor.yellow, "For Create Successfully Enter SaveWorkflowNode", "TICRMTICRM.BuisnessLayer.WorkflowNodeManager", "");

                    workflow = objMapper.GetWorkFlowNode(workflowNodeDto);  // pass parameter WorkflowNode  object to WorkflowNode  object
                    workflow.NodeDataId = Guid.NewGuid();
                    dbEnt.WorkFlowNodes.Add(workflow); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveWorkflowNode", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRMTICRM.BuisnessLayer.WorkflowNodeManager", "");
                throw ex;
            }
            return false;

        }

    }
}
