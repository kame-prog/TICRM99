using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /****************************************************************************************
    ||  Class [WorkOrderManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, updating and saving workorders, getting workorders specifically
    ||             on Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||  Changes Made:   [13/8/2020      Added Methods for Datatable Pagination  Akhtar Zaman]
    ****************************************************************************************/
    public class WorkOrderManager : BaseManager
    {
        public WorkOrderManager()
        {
            //WorkStages = GetWorkStages();
        }
        //public List<WorkStageDto> WorkStages { get; set; }

        /// <summary>
        ///  Get Workorders count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int WorkorderCount()
        {
            try
            {
                InsertEventLog("WorkorderCount", EventType.Log, EventColor.yellow, "Getting Work order count", "TICRM.BusinessLayer.WorkOrderManager.WorkorderCount", "");

                return dbEnt.WorkOrders.Count();
            }
            catch(Exception ex)
            {

                InsertEventMonitor("WorkorderCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkOrderManager.WorkorderCount", "");
                throw;

            }
        }
        /// <summary>
        /// Get Work Stage DTOs
        /// </summary>
        /// <returns></returns>
        
        //public List<WorkStageDto> GetWorkStages()
        //{
        //    try
        //    {
        //        InsertEventLog("GetWorkStages", EventType.Log, EventColor.yellow, "getting List of WorkStageDto", "TICRM.BusinessLayer.WorkOrderManager.GetWorkStages", "");
        //        List<WorkStageDto> workStageDtos = new List<WorkStageDto>();

        //        foreach (WorkStage item in dbEnt.WorkStages.CollectionNotNull())
        //        {
        //            workStageDtos.Add(objMapper.GetWorkStageDTO(item));
        //        }
        //        return workStageDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetWorkStages", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkOrderManager.GetWorkStages", "");
        //        throw;
        //    }
        //}

        public WorkOrderDto GetWorkOrder(Guid? guid)
        {
            try
            {
                InsertEventLog("GetWorkOrder", EventType.Log, EventColor.yellow, "getting WorkOrder on id='" + guid + "'", "TICRM.BusinessLayer.WorkOrderManager.GetWorkOrder", "");
                return objMapper.GetWorkOrderDTO(dbEnt.WorkOrders.Find(guid));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkStages", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkOrderManager.GetWorkOrder", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// Code By Akhtar Zaman
        /// 5/8/2020
        /// Get specific Workorders on the basis of Account Id
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public List<WorkOrderDto> GetAccountWorkorders(Guid AccountId)
        {
            try
            {
                InsertEventLog("GetAccountWorkorders", EventType.Log, EventColor.yellow, "Get list of Account WorkOrders", "TICRM.BusinessLayer.WorkOrderManager.GetAccountWorkorders", "");
                List<WorkOrderDto> workOrderDto = new List<WorkOrderDto>();
                List<WorkOrder> workorder = dbEnt.WorkOrders.Where(x => x.AccountId == AccountId).ToList(); 
                foreach (WorkOrder item in workorder.CollectionNotNull())
                {
                    workOrderDto.Add(objMapper.GetWorkOrderDTO(item));

                }
                return workOrderDto;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetAccountWorkorders", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkOrderManager.GetAccountWorkorders", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Get WorkOrder list 
        /// </summary>
        /// <returns></returns>
        public List<WorkOrderDto> GetWorkOrders(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetWorkOrders", EventType.Log, EventColor.yellow, "getting list of WorkOrderDto", "TICRM.BusinessLayer.WorkOrderManager.GetWorkOrders", "");
                List<WorkOrderDto> WorkOrderDtos = new List<WorkOrderDto>();
                //List<WorkOrder> workOrders = dbEnt.WorkOrders.Include(w => w.Status).Include(w => w.Team).Include(w => w.User).Include(w => w.WorkStage).Where(a => a.IsDeleted != true).ToList();
                List<WorkOrder> workOrders = dbEnt.sp_WorkOrders_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (WorkOrder item in workOrders.CollectionNotNull())
                {
                    WorkOrderDtos.Add(objMapper.GetWorkOrderDTO(item));
                }
                return WorkOrderDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetWorkOrder", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkOrderManager.GetWorkOrders", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// save and edit WorkOrder 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool SaveWorkOrder(WorkOrderDto workOrderDto,string CurrentUserId, string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveWorkOrder", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.WorkOrderManager.SaveWorkOrder", "");
                WorkOrder WorkOrder;
                if (isEditMode)
                {
                    WorkOrder = objMapper.GetWorkOrder(workOrderDto);
                    WorkOrder WorkOrderDB = dbEnt.WorkOrders.FirstOrDefault(x => x.WorkOrderId == WorkOrder.WorkOrderId);
                    if (WorkOrderDB!=null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveWorkOrder", EventType.Log, EventColor.yellow, "going to delete workorder on id='" + workOrderDto.WorkOrderId + "'", "TICRM.BusinessLayer.WorkOrderManager.SaveWorkOrder", "");
                            WorkOrder OrderDelete = dbEnt.WorkOrders.FirstOrDefault(x => x.WorkOrderId == WorkOrder.WorkOrderId);
                            OrderDelete.IsDeleted = true;
                            //dbEnt.Entry(OrderDelete).State = EntityState.Modified;
                        }
                        else
                        {
                            WorkOrder OrderEdit = dbEnt.WorkOrders.FirstOrDefault(x => x.WorkOrderId == WorkOrder.WorkOrderId);
                            OrderEdit.Title = WorkOrder.Title;
                            OrderEdit.StatusId = WorkOrder.StatusId;
                            OrderEdit.NTE = WorkOrder.NTE;
                            OrderEdit.AssignedUser = WorkOrder.AssignedUser;
                            OrderEdit.AssignedTeam = WorkOrder.AssignedTeam;
                            OrderEdit.WorkOrderStageId = WorkOrder.WorkOrderStageId;
                            OrderEdit.Description = WorkOrder.Description;
                            OrderEdit.AccountId = WorkOrder.AccountId;
                            OrderEdit.UpdatedDate = DateTime.Now;
                            OrderEdit.UpdatedBy = CurrentUserId;
                        }
                        if (dbEnt.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    InsertEventLog("SaveWorkOrder", EventType.Log, EventColor.yellow, "going to create new record of workorder", "TICRM.BusinessLayer.WorkOrderManager.SaveWorkOrder", "");
                    WorkOrder = objMapper.GetWorkOrder(workOrderDto);
                    WorkOrder.WorkOrderId = Guid.NewGuid();
                    WorkOrder.CreatedDate = DateTime.Now;
                    WorkOrder.CreatedBy = CurrentUserId;
                    WorkOrder.Company = Guid.Parse(UserCompanyID);
                    dbEnt.WorkOrders.Add(WorkOrder);
                }

                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("SaveWorkOrder", EventType.Log, EventColor.yellow, "workorder Saved Successfully", "TICRM.BusinessLayer.WorkOrderManager.SaveWorkOrder", "");
                    return true;
                }

            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetWorkOrders", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkOrderManager.SaveWorkOrder", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

            return false;
        }
        /// <summary>
        /// Get count for all the workorders
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of WorkOrders Log", "TICRM.BuisnessLayer.WorkOrderManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.WorkOrders.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkOrderManager.GetTotalCount", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Getting data for data tables pagination
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns></returns>
        public List<WorkOrderDto> GetWorkOrderList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetEventLogList", EventType.Log, EventColor.yellow, "Get List of WorkOrders Based on Datatable Query", "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");

                var workorderDto = new List<WorkOrderDto>();
                var workorders = new List<WorkOrder>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings

                if (!string.IsNullOrEmpty(sSearch))
                {
                    workorders = dbEnt.WorkOrders.Where(a => a.Title.ToLower().Contains(sSearch)
                    || a.NTE.ToString().ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.Status.Name.ToLower().Contains(sSearch)
                    || a.AssignedTeam.ToString().ToLower().Contains(sSearch)
                    || a.AssignedUser.Value.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.Title).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    workorders = dbEnt.WorkOrders.OrderBy(x => x.Title).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (WorkOrder item in workorders.CollectionNotNull())
                {
                    workorderDto.Add(objMapper.GetWorkOrderDTO(item)); // add in a list object
                }
                return workorderDto;

               

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventLogList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");

                throw ex;
            }

        }
    }
}
