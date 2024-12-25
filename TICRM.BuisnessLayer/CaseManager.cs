using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;
using System.Data.Entity;
using System.Web;

namespace TICRM.BuisnessLayer
{

    /***************************************************************************************
    ||  Class [AccountManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             Case are get from the database and mapped with the corrosponding DTO
    ||             object to send it back to the controller. also cases for different 
    ||             modules like account, opportunities and contacts are being fetched from here]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [18/09/2020     Created       Akhtar Zaman]
    ****************************************************************************************/

    public class CaseManager : BaseManager
    {
        /// <summary>
        /// Gets the cases.
        /// </summary>
        /// <returns></returns>
        public List<CaseDto> GetCases(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetCases", EventType.Log, EventColor.yellow, "Successfully Enter in GetCases", "TICRM.BusinessLayer.CaseManager", "");
                List<CaseDto> CaseDtos = new List<CaseDto>();
                //List<Case> Cases = dbEnt.Cases.Where(x => x.IsDeleted != true).Where(x=>x.CreatedBy== CurrentUserId).ToList();
                List<Case> Cases = dbEnt.sp_Cases_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (Case item in Cases.CollectionNotNull())
                {
                    CaseDtos.Add(objMapper.GetCaseDto(item));
                }

                return CaseDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCases", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CaseManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Gets the case on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public CaseDto GetCaseonId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetCaseonId", EventType.Log, EventColor.yellow, "Successfully Enter in GetCaseonId", "TICRM.BusinessLayer.CasemManager", "");
                return objMapper.GetCaseDto(dbEnt.Cases.Find(guid));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCaseonId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CasemManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the case resolutions list for dropdown.
        /// </summary>
        /// <returns></returns>
        public List<CaseResolutionDto> GetCaseResolutions()
        {
            try
            {
                InsertEventLog("GetCaseResolutions", EventType.Log, EventColor.yellow, "Successfully Enter in GetCaseResolutions", "TICRM.BusinessLayer.CasemManager", "");

                List<CaseResolutionDto> caseResolutionDtos = new List<CaseResolutionDto>();

                foreach (var item in dbEnt.CaseResolutions.CollectionNotNull())
                {
                    caseResolutionDtos.Add(objMapper.GetCaseResolutionDto(item));
                }

                return caseResolutionDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCaseResolutions", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CasemManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the case type list for dropdown.
        /// </summary>
        /// <returns></returns>
        public List<CaseTypeDto> GetCaseTypeDtos()
        {
            try
            {
                InsertEventLog("GetCaseTypeDtos", EventType.Log, EventColor.yellow, "Successfully Enter in GetCaseTypeDtos", "TICRM.BusinessLayer.CasemManager", "");

                List<CaseTypeDto> caseTypeDtos = new List<CaseTypeDto>();

                foreach (var item in dbEnt.CaseTypes.CollectionNotNull())
                {
                    caseTypeDtos.Add(objMapper.GetCaseTypeDto(item));
                }

                return caseTypeDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCaseTypeDtos", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CasemManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the case status list for dropdown.
        /// </summary>
        /// <returns></returns>
        public List<CaseStatusDto> GetCaseStatusDtos()
        {
            try
            {
                InsertEventLog("GetCaseStatusDtos", EventType.Log, EventColor.yellow, "Successfully Enter in GetCaseStatusDtos", "TICRM.BusinessLayer.CasemManager", "");

                List<CaseStatusDto> CaseStatusDtos = new List<CaseStatusDto>();

                foreach (var item in dbEnt.CaseStatus.CollectionNotNull())
                {
                    CaseStatusDtos.Add(objMapper.GetCaseStatusDto(item));
                }

                return CaseStatusDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCaseStatusDtos", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CasemManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the contact list.
        /// </summary>
        /// <returns></returns>
        public List<ContactDto> GetContactList()
        {
            try
            {
                InsertEventLog("GetContactList", EventType.Log, EventColor.yellow, "Successfully Enter in GetContactList", "TICRM.BusinessLayer.CasemManager", "");

                List<ContactDto> Contactlist = new List<ContactDto>();

                foreach (var item in dbEnt.Contacts.CollectionNotNull())
                {
                    Contactlist.Add(objMapper.GetContactDto(item));
                }

                return Contactlist;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetContactList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CasemManager", "");

                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Saves the case on edit create or delete.
        /// </summary>
        /// <param name="caseDto">The case dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns></returns>
        public bool SaveCase(CaseDto caseDto, string CurrentUserId,String UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveCase", EventType.Log, EventColor.yellow, "Successfully Enter in SaveCase", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                Case cases;
                if (isEditMode)
                {
                    cases = objMapper.GetCase(caseDto);
                    Case e = dbEnt.Cases.FirstOrDefault(x => x.CaseId == cases.CaseId);
                    if (e != null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveCase", EventType.Log, EventColor.yellow, "For Delete. Successfully Enter in SaveCase", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                            Case caseforDelete = dbEnt.Cases.FirstOrDefault(x => x.CaseId == caseDto.CaseId);
                            caseforDelete.IsDeleted = true;
                        }
                        else
                        {
                            InsertEventLog("SaveCase", EventType.Log, EventColor.yellow, "For Edit. Successfully Enter in SaveCase", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                            e.AssignedTeam = cases.AssignedTeam;
                            e.AssignedUser = cases.AssignedUser;
                            e.CaseId = cases.CaseId;
                            e.CaseStatusId = cases.CaseStatusId;
                            e.BillableTime = cases.BillableTime;
                            e.CaseTitle = cases.CaseTitle;
                            e.CaseTypeId = cases.CaseTypeId;
                            e.ContactId = cases.ContactId;
                            e.Description = cases.Description;
                            e.RelatedTo = cases.RelatedTo;
                            e.RelatedToId = cases.RelatedToId;
                            e.Origin = cases.Origin;
                            e.ResolutionType = cases.ResolutionType;
                            e.Resolution = cases.Resolution;
                            e.TotalTime = cases.TotalTime;
                            e.BillableTime = cases.BillableTime;
                            e.Remarks = cases.Remarks;
                            e.RelatedToId = cases.RelatedToId;
                            e.IsScheduled = cases.IsScheduled;
                            e.Scheduling = cases.Scheduling;
                            e.UpdatedDate = DateTime.Now;
                            e.UpdatedBy = CurrentUserId;
                        }


                        HttpContext.Current.Session["Casesobj"] = cases;
                        if (dbEnt.SaveChanges() > 0)
                        {
                            return true;
                        }
                        //dbEnt.Entry(account).State = EntityState.Modified;
                    }
                }
                else
                {
                    InsertEventLog("SaveCase", EventType.Log, EventColor.yellow, "For Create. Successfully Enter in SaveCase", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                    CaseStatu caseid = dbEnt.CaseStatus.FirstOrDefault(x => x.Name == "Active");
                    caseDto.CaseStatusId = caseid.CaseStatusId;

                    cases = objMapper.GetCase(caseDto);
                    cases.CreatedBy = CurrentUserId;
                    cases.Company = Guid.Parse(UserCompanyID);
                    dbEnt.Cases.Add(cases);
                }

                if (dbEnt.SaveChanges() > 0)
                {
                    HttpContext.Current.Session["Casesobj"] = cases;
                    InsertEventLog("SaveCase", EventType.Log, EventColor.yellow, "data saved successfully in SaveCase", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                    return true;
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveAccount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

            return false;
        }
        /// <summary>
        /// Gets the cases list for datatables.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns></returns>
        public List<CaseDto> GetCasesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetCasesList", EventType.Log, EventColor.yellow, "Get List of Activites Based on Datatable Query", "TICRM.BuisnessLayer.CaseManager.GetCasesList", "");

                var cases = new List<Case>();
                var casedtos = new List<CaseDto>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    cases = dbEnt.Cases.Where(a => (a.CaseTitle.ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.RelatedTo.ToLower().Contains(sSearch)
                    || a.Team.Name.ToLower().Contains(sSearch)
                    || a.User.Name.ToLower().Contains(sSearch)
                    || a.CaseType.Name.ToString().ToLower().Contains(sSearch)
                    || a.CaseStatu.Name.ToString().ToLower().Contains(sSearch)
                    || a.Contact.Name.ToString().ToLower().Contains(sSearch)) && (a.IsDeleted == false)
                    ).OrderBy(x => x.CaseTitle).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    cases = dbEnt.Cases.Where(x => x.IsDeleted == false).OrderBy(x => x.CaseTitle).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Case item in cases.CollectionNotNull())
                {
                    casedtos.Add(objMapper.GetCaseDto(item)); // add in a list object
                }

                return casedtos;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCasesList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CaseManager.GetCasesList", "");

                throw ex;
            }

        }

        /// <summary>
        /// Count all Cases
        /// </summary>
        /// <returns>No of total activites</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Cases", "TICRM.BuisnessLayer.CaseManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Cases.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CaseManager.GetTotalCount", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the account cases.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public List<CaseDto> GetAccountCases(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAccountCases", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.CaseManager.GetAccountCases", "");
                List<CaseDto> casedtos = new List<CaseDto>(); // create list Object of Activity DTo
                AccountManager am = new AccountManager();
                DeviceManager dm = new DeviceManager();
                AccountDto a = am.GetAccount(guid);
                var cases = dbEnt.Cases.Include(x => x.CaseResolution).Include(c => c.CaseStatu).Include(c => c.CaseType).Include(c => c.Contact)
                    .Where(x => x.RelatedTo == RelatedToEnum.Account.ToString() && x.RelatedToId == guid).ToList(); // Get List Of Activities from DB
                var DeviceCases = dbEnt.Cases.Where(x => x.RelatedTo == RelatedToEnum.Device.ToString()).ToList();
                List<Case> caseDev = new List<Case>();
                List<DeviceDto> accDevices = dm.GetDevices((Guid)guid);
                foreach(var item in accDevices)
                {
                    foreach(var caseitem in DeviceCases)
                    {
                        if(caseitem.RelatedToId == item.DeviceId)
                        {
                            cases.Add(caseitem);
                        }
                    }
                }
                //var query = (from Case in dbEnt.Cases
                //             join d in dbEnt.Devices on Case.RelatedToId equals d.DeviceId
                //             where d.AccountId == guid
                //             join a in dbEnt.Accounts on Case.RelatedToId equals a.AccountId
                //             where a.AccountId == guid
                //             select new { Case }).ToList();
                
                // apply iteration on activities    
                //List<Case> myList = query.Cast<Case>().ToList();
                foreach (Case item in cases.CollectionNotNull())
                {
                    casedtos.Add(objMapper.GetCaseDto(item)); // add in a list object
                }
                return casedtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountCases", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CaseManager.GetAccountCases", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the account casesfor devices.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public List<CaseDto> GetAccountCasesforDevices(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAccountCasesforDevices", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.CaseManager.GetAccountCases", "");
                List<CaseDto> casedtos = new List<CaseDto>(); // create list Object of Activity DTo
                AccountManager am = new AccountManager();
                DeviceManager dm = new DeviceManager();
                AccountDto a = am.GetAccount(guid);
                var cases = dbEnt.Cases.Include(x => x.CaseResolution).Include(c => c.CaseStatu).Include(c => c.CaseType).Include(c => c.Contact)
                    .Where(x => x.RelatedTo == RelatedToEnum.Account.ToString() && x.RelatedToId == guid).ToList(); // Get List Of Activities from DB
                var DeviceCases = dbEnt.Cases.Where(x => x.RelatedTo == RelatedToEnum.Device.ToString()).ToList();
                List<Case> caseDev = new List<Case>();
                List<DeviceDto> accDevices = dm.GetDevices((Guid)guid);
                foreach (var item in accDevices)
                {
                    foreach (var caseitem in DeviceCases)
                    {
                        if (caseitem.RelatedToId == item.DeviceId)
                        {
                            cases.Add(caseitem);
                        }
                    }
                }
                //var query = (from Case in dbEnt.Cases
                //             join d in dbEnt.Devices on Case.RelatedToId equals d.DeviceId
                //             where d.AccountId == guid
                //             join a in dbEnt.Accounts on Case.RelatedToId equals a.AccountId
                //             where a.AccountId == guid
                //             select new { Case }).ToList();

                // apply iteration on activities    
                //List<Case> myList = query.Cast<Case>().ToList();
                foreach (Case item in cases.CollectionNotNull())
                {
                    casedtos.Add(objMapper.GetCaseDto(item)); // add in a list object
                }
                return casedtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountCases", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CaseManager.GetAccountCases", "");
                throw;
            }
        }

        /// <summary>
        /// Gets the account casesfor devices.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public List<CaseDto> GetAccountCasesforDevices(Guid guid)
        {
            InsertEventLog("GetDeviceSensorGraphList", EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphList", "");
            try
            {
                DeviceManager dm = new DeviceManager();
                List<CaseDto> cases = new List<CaseDto>();
                var DeviceCases = dbEnt.Cases.Where(x => x.RelatedTo == RelatedToEnum.Device.ToString() && x.Resolution == null).ToList();
                List<Case> caseDev = new List<Case>();
                List<DeviceDto> accDevices = dm.GetDevices((Guid)guid);
                foreach (var item in accDevices)
                {
                    foreach (var caseitem in DeviceCases)
                    {
                        if (caseitem.RelatedToId == item.DeviceId)
                        {
                            CaseDto c = new CaseDto();
                            c = objMapper.GetCaseDto(caseitem);
                            c.dLat = item.Latitude.ToString();
                            c.dLong = item.Longitude.ToString();
                            cases.Add(c);
                        }
                    }
                }


                InsertEventLog("GetDeviceSensorGraphList", EventType.Log, EventColor.yellow, "Ready to response Json Status ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphList", "");
                return cases; // return in json
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDeviceSensorGraphList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphList", "");
                throw;
            }
        }

        /// <summary>
        /// Gets the opportunity cases.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public List<CaseDto> GetOpportunityCases(Guid? guid)
        {
            try
            {
                InsertEventLog("GetOpportunityCases", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.CaseManager.GetOpportunityCases", "");
                List<CaseDto> casedtos = new List<CaseDto>(); // create list Object of Activity DTo

                var cases = dbEnt.Cases.Include(c => c.CaseType).Include(c => c.Contact).Where(x => x.RelatedTo == RelatedToEnum.Oppertunities.ToString() && x.RelatedToId == guid).ToList(); // Get List Of Cases from DB
                // apply iteration on activities
                foreach (DAL.Case item in cases.CollectionNotNull())
                {
                    casedtos.Add(objMapper.GetCaseDto(item)); // add in a list object
                }
                return casedtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetOpportunityCases", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CaseManager.GetOpportunityCases", "");
                throw;
            }
        }

        /// <summary>
        /// Gets the contacts cases.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public List<CaseDto> GetContactsCases(long? guid)
        {
            try
            {
                InsertEventLog("GetContactsCases", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.CaseManager.GetContactsCases", "");
                List<CaseDto> casedtos = new List<CaseDto>(); // create list Object of Activity DTo
                var cases = dbEnt.Cases.Include(x => x.CaseResolution).Include(c => c.CaseStatu).Include(c => c.CaseType).Include(c => c.Contact).Where(x => x.RelatedTo == RelatedToEnum.Contacts.ToString() &&x.ContactId == guid).ToList(); // Get List Of Cases from DB
                // apply iteration on activities
                foreach (DAL.Case item in cases.CollectionNotNull())
                {
                    casedtos.Add(objMapper.GetCaseDto(item)); // add in a list object
                }
                return casedtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetContactsCases", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.CaseManager.GetContactsCases", "");
                throw;
            }
        }
        /// <summary>
        /// Res the activate case.
        /// </summary>
        /// <param name="CaseId">The case identifier.</param>
        /// <returns></returns>
        public bool ReActivateCase(Guid CaseId)
        {
            try
            {
                InsertEventLog("ReActivateCase", EventType.Log, EventColor.yellow, "Reactivate a resolved case", "TICRM.BuisnessLayer.CaseManager.ReActivateCase", "");

                CaseStatu caseid = dbEnt.CaseStatus.FirstOrDefault(x => x.Name == "Active");
                CaseDto casedto = new CaseDto();
                casedto.CaseId = CaseId;
                casedto.CaseStatusId = caseid.CaseStatusId;
                Case cases = objMapper.GetCase(casedto);
                Case e = dbEnt.Cases.FirstOrDefault(x => x.CaseId == CaseId);
                e.CaseStatusId = cases.CaseStatusId;

                if (dbEnt.SaveChanges() > 0)
                {
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("ReActivateCase", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CaseManager.ReActivateCase", "");
                throw ex;
            }
        }


    }
}
