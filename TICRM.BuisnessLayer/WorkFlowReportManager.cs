using Newtonsoft.Json;
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
    ||  Class [WorkFlowReportManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, updating and saving workflow reports, getting workflow reports 
    ||             specifically on Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class WorkFlowReportManager : BaseManager
    {

        /// <summary>
        /// Gets the work flow reports.
        /// </summary>
        /// <returns>List&lt;WorkFlowReportDTO&gt;.</returns>
        public List<WorkFlowReportDTO> GetWorkFlowReports(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {

            InsertEventLog("GetWorkFlowReports", EventType.Log, EventColor.yellow, "getting List of WorkFlowReportDTO","TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReports", "");
                List<WorkFlowReportDTO> workflowreportDTO = new List<WorkFlowReportDTO>(); // create list Object of WorkFlowReports DTO

                //List<WorkFlowReport> workFlowReports = dbEnt.WorkFlowReports.Include(x=>x.WorkFlow).Where(x => x.IsDeleted != true).ToList(); // declare a local variable to Get List Of WorkFlowReports from DB
                List<WorkFlowReport> workFlowReports = dbEnt.sp_WorkFlowReports_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (WorkFlowReport item in workFlowReports.CollectionNotNull())
                {
                    workflowreportDTO.Add(objMapper.GetWorkFlowReportDTO(item)); // add in a list object

                }
                return workflowreportDTO; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkFlowReports", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReports", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        public List<WorkFlowReportDTO> GetWorkFlowReports()
        {
            try
            {

                InsertEventLog("GetWorkFlowReports", EventType.Log, EventColor.yellow, "getting List of WorkFlowReportDTO", "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReports", "");
                List<WorkFlowReportDTO> workflowreportDTO = new List<WorkFlowReportDTO>(); // create list Object of WorkFlowReports DTO

                List<WorkFlowReport> workFlowReports = dbEnt.WorkFlowReports.Include(x => x.WorkFlow).Where(x => x.IsDeleted != true).ToList(); // declare a local variable to Get List Of WorkFlowReports from DB

                foreach (WorkFlowReport item in workFlowReports.CollectionNotNull())
                {
                    workflowreportDTO.Add(objMapper.GetWorkFlowReportDTO(item)); // add in a list object

                }
                return workflowreportDTO; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkFlowReports", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReports", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the work flow reports for admin page.
        /// </summary>
        /// <returns>AccountViewModel.</returns>
        public AccountViewModel GetWorkFlowReportsAdmuin()
        {
            try
            {
                AccountViewModel avm = new AccountViewModel();


                InsertEventLog("GetWorkFlowReports", EventType.Log, EventColor.yellow, "getting List of WorkFlowReportDTO", "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReports", "");
                List<WorkFlowReportDTO> workflowreportDTO = new List<WorkFlowReportDTO>(); // create list Object of WorkFlowReports DTO

                List<WorkFlowReport> workFlowReports = dbEnt.WorkFlowReports.Include(x => x.WorkFlow).ToList(); // declare a local variable to Get List Of WorkFlowReports from DB
                // apply iteration on WorkFlowReport
                foreach (WorkFlowReport item in workFlowReports.CollectionNotNull())
                {
                    workflowreportDTO.Add(objMapper.GetWorkFlowReportDTO(item)); // add in a list object
                }
                avm.workflowReportAdmin = workflowreportDTO;
                return avm; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkFlowReports", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReports", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the work flow reports for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>List&lt;WorkFlowReportDTO&gt;.</returns>
        public List<WorkFlowReportDTO> GetWorkFlowReportsAccount(Guid accountId)
        {
            try
            {
                InsertEventLog("GetWorkflows for Account", EventType.Log, EventColor.yellow, "going to Get List of Workflowreports on account id = " + accountId + "", "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReportsAccount", "");
                List<WorkFlowReportDTO> workflowreportDTO = new List<WorkFlowReportDTO>();
                List<WorkFlowReport> workFlowReports = dbEnt.WorkFlowReports.Include(x => x.WorkFlow).Where(a => a.AccountID == accountId).ToList(); // declare a local variable to Get List Of WorkFlowReports from DB
                
                foreach (WorkFlowReport item in workFlowReports.CollectionNotNull())
                {
                    workflowreportDTO.Add(objMapper.GetWorkFlowReportDTO(item)); // add in a list object
                }
                return workflowreportDTO; // return List Object in Response
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetOpportunities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetOpportunities", "");
                throw;
            }


        }

        /// <summary>
        /// Saves the work flow report.
        /// </summary>
        /// <param name="workFlowReportDTO">The work flow report dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveItWorkFlowReport(WorkFlowReportDTO workFlowReportDTO, string CurrentUserId, string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "Enter","TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                WorkFlowReport workFlowReport; // create a new object
                workFlowReport = objMapper.GetWorkFlowReport(workFlowReportDTO); // pass parameter object to workflow object

                if (isEditMode) // check if is is edit mode is true
                {

                    WorkFlowReport dbData = dbEnt.WorkFlowReports.FirstOrDefault(x => x.WorkFlowReportId == workFlowReportDTO.WorkFlowReportId); // get data from database and pass in new activity class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                           InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "going to delete workflow report report on id='"+dbData.WorkFlowReportId+"'","TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                            WorkFlowReport workflowdelete = dbEnt.WorkFlowReports.FirstOrDefault(x => x.WorkFlowReportId == workFlowReport.WorkFlowReportId);
                            workflowdelete.IsDeleted = true;
                            dbEnt.Entry(workflowdelete).State = EntityState.Modified;
                        }
                        else
                        {
                            InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "going to update workflow report report on id='" + dbData.WorkFlowReportId + "'","TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                             WorkFlowReport workflowEdit= dbEnt.WorkFlowReports.FirstOrDefault(x => x.WorkFlowReportId == workFlowReport.WorkFlowReportId);
                            workflowEdit.WorkFlowId= workFlowReport.WorkFlowId;
                            workflowEdit.Frequency = workFlowReport.Frequency;
                            workflowEdit.WorkFlowStatus = workFlowReport.WorkFlowStatus;
                            workflowEdit.WorkFlowActionStatus = workFlowReport.WorkFlowActionStatus;
                            workflowEdit.AppliedTo = workFlowReport.AppliedTo;
                            workflowEdit.Action = workFlowReport.Action;
                            workflowEdit.Priority = workFlowReport.Priority;
                            workflowEdit.WorkFlowDesign = workFlowReport.WorkFlowDesign;
                            workflowEdit.AccountID = workFlowReport.AccountID;
                            workflowEdit.DeviceID = workFlowReport.DeviceID;
                            workflowEdit.UpdatedDate = DateTime.Now;
                            workflowEdit.UpdatedBy = CurrentUserId;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "Data Is null of workflow report report on id='" + dbData.WorkFlowReportId + "'", "TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "workflow report Updated Successfully", "TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                        return true;
                    }

                }
                else
                {
            InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "going to create new record of","TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                    workFlowReport.WorkFlowReportId = Guid.NewGuid();
                    workFlowReport.CreatedBy = CurrentUserId;
                    workFlowReport.Company = Guid.Parse(UserCompanyID);
                    workFlowReport.CreatedDate = DateTime.Now;
                    dbEnt.WorkFlowReports.Add(workFlowReport); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
            InsertEventLog("SaveItWorkFlowReport", EventType.Log, EventColor.yellow, "new work flow report created Successfully","TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveItWorkFlowReport", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowReportManager.SaveItWorkFlowReport", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;

        }

        /// <summary>
        /// Gets the work flow report on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>WorkFlowReportDTO.</returns>
        public WorkFlowReportDTO GetWorkFlowReportOnId(Guid? guid)
        {
            try
            {
            InsertEventLog("GetWorkFlowReportOnId", EventType.Log, EventColor.yellow, "get WorkFlowReportDTO on id'"+guid+"'","TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReportOnId", "");
                return objMapper.GetWorkFlowReportDTO(dbEnt.WorkFlowReports.Find(guid)); // get workFlowReport on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkFlowReportOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.WorkFlowReportManager.GetWorkFlowReportOnId", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// getting workflows counts
        /// </summary>
        /// <returns></returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetWorkflowReportList", EventType.Log, EventColor.yellow, "Get total Count of Workflow reports", "TICRM.BuisnessLayer.WorkFlowReportManager.GetWorkflowReportList", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.WorkFlowReports.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkflowReportList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowReportManager.GetWorkflowReportList", "");
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
        public List<WorkFlowReportDTO> GetWorkflowReportList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetWorkflowReportList", EventType.Log, EventColor.yellow, "Get List of Workflows reports Based on Datatable Query", "TICRM.BuisnessLayer.WorkFlowReportManager.GetWorkflowReportList", "");

                var workflowReportDto = new List<WorkFlowReportDTO>();
                var workflowReports = new List<WorkFlowReport>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings

                if (!string.IsNullOrEmpty(sSearch))
                {
                    workflowReports = dbEnt.WorkFlowReports.Where(a => a.WorkFlow.Name.ToLower().Contains(sSearch)
                    || a.Action.ToString().ToLower().Contains(sSearch)
                    || a.WorkFlowStatus.ToString().ToLower().Contains(sSearch)
                    || a.WorkFlowActionStatus.ToString().ToLower().Contains(sSearch)
                    || a.AppliedTo.ToString().ToString().ToLower().Contains(sSearch)
                    || a.Priority.ToString().Contains(sSearch)
                    || a.Frequency.ToString().Contains(sSearch)
                    || a.CreatedDate.Value.ToString().Contains(sSearch)
                    || a.CreatedBy.ToString().Contains(sSearch)
                    ).OrderBy(x => x.WorkFlow.Name).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    workflowReports = dbEnt.WorkFlowReports.OrderBy(x => x.WorkFlow.Name).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (WorkFlowReport item in workflowReports.CollectionNotNull())
                {
                    workflowReportDto.Add(objMapper.GetWorkFlowReportDTO(item)); // add in a list object
                }

               
                return workflowReportDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetWorkflowReportList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.WorkFlowReportManager.GetWorkflowReportList", "");

                throw ex;
            }

        }

    }
}
