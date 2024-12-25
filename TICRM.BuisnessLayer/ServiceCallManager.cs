using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [ServiceCallManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving service calls . Getting a specific resource
    ||             on the basis of Id, getting urgencies and workstages]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ServiceCallManager : BaseManager
    {
        public ServiceCallManager()
        {
            //Urgencies = GetUrgencies();
            //WorkStages = GetWorkStages();
        }

        //public List<UrgencyDto> Urgencies { get; set; }
        //public List<WorkStageDto> WorkStages { get; set; }

        /// <summary>
        /// Get Urgencies 
        /// </summary>
        /// <returns></returns>
        
        //public List<UrgencyDto> GetUrgencies()
        //{
        //    try
        //    {
        //        InsertEventLog("GetUrgencies", EventType.Log, EventColor.yellow, "going to getting list of UrgencyDto","TICRM.BusinessLayer.ServiceCallManager.GetUrgencies", "");
        //        List<UrgencyDto> urgencyDtos = new List<UrgencyDto>();

        //        // apply iteration on dbEnt.Urgencies
        //        foreach (Urgency item in dbEnt.Urgencies)
        //        {
        //            urgencyDtos.Add(objMapper.GetUrgencyDTO(item));
        //        }
        //        return urgencyDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetUrgencies", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ServiceCallManager.GetUrgencies", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get Work Stages DTOs
        /// </summary>
        /// <returns></returns>
        /// 
        //public List<WorkStageDto> GetWorkStages()
        //{
        //    try
        //    {
        //        InsertEventLog("GetWorkStages", EventType.Log, EventColor.yellow, "going to getting list of WorkStageDto","TICRM.BusinessLayer.ServiceCallManager.GetWorkStages", "");
        //        List<WorkStageDto> workStageDtos = new List<WorkStageDto>();

        //        // apply iteration on dbEnt.WorkStages
        //        foreach (WorkStage item in dbEnt.WorkStages.CollectionNotNull())
        //        {
        //            workStageDtos.Add(objMapper.GetWorkStageDTO(item));
        //        }
        //        return workStageDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetWorkStages", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ServiceCallManager.GetWorkStages", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the service call.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ServiceCallDto.</returns>
        public ServiceCallDto GetServiceCall(Guid? guid)
        {
            try
            {
                InsertEventLog("GetServiceCall", EventType.Log, EventColor.yellow, "going to ServiceCall on id=" + guid + "","TICRM.BusinessLayer.ServiceCallManager.GetServiceCall", "");
                return objMapper.GetServiceCallDTO(dbEnt.ServiceCalls.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetServiceCall", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ServiceCallManager.GetWorkStages", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// Get serviceCall list 
        /// </summary>
        /// <returns></returns>
        public List<ServiceCallDto> GetServiceCalls(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetServiceCalls", EventType.Log, EventColor.yellow, "going to getting list of ServiceCallDto","TICRM.BusinessLayer.ServiceCallManager.GetServiceCalls", "");
                List<ServiceCallDto> serviceCallDtos = new List<ServiceCallDto>();
                //List<ServiceCall> serviceCalls = dbEnt.ServiceCalls.Include(s => s.Status).Include(s => s.Team).Include(s => s.Urgency).Include(s => s.User).Include(s => s.WorkStage).Where(a => a.IsDeleted != true).ToList();
                List<ServiceCall> serviceCalls = dbEnt.sp_ServiceCalls_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (ServiceCall item in serviceCalls.CollectionNotNull())
                {
                    serviceCallDtos.Add(objMapper.GetServiceCallDTO(item));
                }
                return serviceCallDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetServiceCalls", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ServiceCallManager.GetServiceCalls", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }


        /// <summary>
        /// save and edit ServiceCallDto 
        /// </summary>
        /// <param name="readng"></param>
        /// <returns></returns>
        public bool SaveServiceCall(ServiceCallDto serviceCallDto, string CurrentUserId,string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveServiceCall", EventType.Log, EventColor.yellow, "Enter","TICRM.BusinessLayer.ServiceCallManager.SaveServiceCall", "");
                ServiceCall serviceCall;
                if (isEditMode)
                {

                    InsertEventLog("SaveServiceCall", EventType.Log, EventColor.yellow, "going to update service call on id=" + serviceCallDto.ServiceCallId + "","TICRM.BusinessLayer.ServiceCallManager.SaveServiceCall", "");
                    serviceCall = objMapper.GetServiceCall(serviceCallDto);
                    ServiceCall DbService = dbEnt.ServiceCalls.FirstOrDefault(x => x.ServiceCallId == serviceCall.ServiceCallId);
                    if (DbService != null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveServiceCall", EventType.Log, EventColor.yellow, "going to delete service call on id", "TICRM.BusinessLayer.ServiceCallManager.SaveServiceCall", "");

                            ServiceCall ServiceDelete = dbEnt.ServiceCalls.FirstOrDefault(x => x.ServiceCallId == serviceCall.ServiceCallId);
                            ServiceDelete.IsDeleted = true;
                        }
                        else
                        {
                            DbService.Title = serviceCall.Title;
                            DbService.Detail = serviceCall.Detail;
                            DbService.UrgencyId = serviceCall.UrgencyId;
                            DbService.ServiceCallStageId = serviceCall.ServiceCallStageId;
                            DbService.AssignedUser = serviceCall.AssignedUser;
                            DbService.AssignedTeam = serviceCall.AssignedTeam;
                            DbService.Description = serviceCall.Description;
                            DbService.StatusId = serviceCall.StatusId;
                            DbService.UpdatedBy = CurrentUserId;
                            DbService.UpdatedDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveServiceCall", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + serviceCallDto.ServiceCallId, "TICRM.BuisnessLayer.ServiceCallManger.SaveServiceCall", "");
                        return false; // return false if no any condition found for edit and delete
                    }
                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }
                }
                else
                {
                    InsertEventLog("SaveServiceCall", EventType.Log, EventColor.yellow, "going to Create new record in DB","TICRM.BusinessLayer.ServiceCallManager.SaveServiceCall", "");
                    serviceCall = objMapper.GetServiceCall(serviceCallDto);
                    serviceCall.ServiceCallId = Guid.NewGuid();
                    serviceCall.CreatedDate = DateTime.Now;
                    serviceCall.CreatedBy = CurrentUserId;
                    serviceCall.Company = Guid.Parse(UserCompanyID);
                    dbEnt.ServiceCalls.Add(serviceCall);
                }
                if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveServiceCall", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ServiceCallManager.SaveServiceCall", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;
        }

        /// <summary>
        /// getting workflows counts
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Workflow mapping", "TICRM.BuisnessLayer.ServiceCallManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.ServiceCalls.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ServiceCallManager.GetTotalCount", "");
                throw ex;
            }
        }
        /// <summary>
        /// Getting data for data tables for workflows
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns></returns>
        public List<ServiceCallDto> GetServiceCallList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetServiceCallList", EventType.Log, EventColor.yellow, "Get List of Service call Based on Datatable Query", "TICRM.BuisnessLayer.ServiceCallManager.GetServiceCallList", "");

                var servicecallDto = new List<ServiceCallDto>();
                var servicecalls = new List<ServiceCall>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings

                if (!string.IsNullOrEmpty(sSearch))
                {
                    servicecalls = dbEnt.ServiceCalls.Where(a => a.Title.ToLower().Contains(sSearch)
                    || a.Description.ToString().ToLower().Contains(sSearch)
                    || a.Status.Name.ToString().ToLower().Contains(sSearch)
                    || a.Urgency.Name.ToString().ToLower().Contains(sSearch)
                    || a.Team.Name.ToString().ToLower().Contains(sSearch)
                    || a.User.Name.ToString().ToString().ToLower().Contains(sSearch)
                    || a.WorkStage.Name.ToString().Contains(sSearch)
                    ).OrderBy(x => x.Title).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    servicecalls = dbEnt.ServiceCalls.OrderBy(x => x.Title).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (ServiceCall item in servicecalls.CollectionNotNull())
                {
                    servicecallDto.Add(objMapper.GetServiceCallDTO(item)); // add in a list object
                }

                return servicecallDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetServiceCallList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ServiceCallManager.GetServiceCallList", "");

                throw ex;
            }

        }
    }
}
