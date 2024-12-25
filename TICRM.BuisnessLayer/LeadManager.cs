using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [LeadManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting Leads in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class LeadManager : BaseManager
    {
        public LeadManager()
        {
            //Industries = GetIndustries();
            //LeadSources = GetLeadSources();
            //LeadTypes = GetLeadTypes();
        }

        #region Properties & Methods

        //public List<IndustryDto> Industries { get; set; }
        //public List<LeadSourceDto> LeadSources { get; set; }
        //public List<LeadTypeDto> LeadTypes { get; set; }


        /// <summary>
        /// Get Lead types
        /// </summary>
        /// <returns></returns>
        
        //public List<LeadTypeDto> GetLeadTypes()
        //{
        //    try
        //    {
        //        InsertEventLog("GetLeadTypes", EventType.Log, EventColor.yellow, "Get List Of LeadTypeDto", "TICRM.BusinessLayer.LeadManager.GetLeadTypes", "");
        //        List<LeadTypeDto> leadTypesDtos = new List<LeadTypeDto>();

        //        foreach (LeadType item in dbEnt.LeadTypes.CollectionNotNull())
        //        {
        //            leadTypesDtos.Add(objMapper.GetLeadTypeDTO(item));
        //        }
        //        return leadTypesDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetLeadTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLeadTypes", "");

        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the leads list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;LeadDto&gt;.</returns>
        public List<LeadDto> GetLeadsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetLeadsList", EventType.Log, EventColor.yellow, "to get list of leads", "TICRM.BuisnessLayer.LeadsManager.GetLeadsList", "");
                List<LeadDto> leadDto = new List<LeadDto>();// create strongly type list Object of EventNotification DTO
                List<Lead> lead = new List<Lead>(); // Get List Of EventNotifications from DB


                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    lead = dbEnt.Leads.Include(l => l.Address).Include(l => l.Industry).Include(l => l.LeadSource).Include(l => l.LeadType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true && (a.Name.ToLower().Contains(sSearch)
                    || a.PhoneNumber.ToLower().Contains(sSearch)
                    || a.Email.ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.Address.Street1.ToLower().Contains(sSearch)
                    || a.LeadSource.Name.ToLower().Contains(sSearch)
                    || a.LeadType.Name.ToLower().Contains(sSearch)
                    || a.Industry.Name.ToLower().Contains(sSearch)
                    || a.LeadSource.Name.ToLower().Contains(sSearch)
                    || a.Status.Name.ToLower().Contains(sSearch)
                    || a.Team.Name.ToLower().Contains(sSearch)
                    || a.User.Name.ToLower().Contains(sSearch))).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    lead = dbEnt.Leads.Include(l => l.Address).Include(l => l.Industry).Include(l => l.LeadSource).Include(l => l.LeadType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();


                // apply iteration on eventNotifications
                foreach (Lead item in lead.CollectionNotNull())
                {
                    leadDto.Add(objMapper.GetLeadDTO(item)); // add in a list object
                }
                return leadDto; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetLeadsList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.LeadsManager.GetLeadsList", "");
                throw;
            }
        }


        /// <summary>
        /// Get Lead Sources
        /// </summary>
        /// <returns></returns>
        
        //public List<LeadSourceDto> GetLeadSources()
        //{
        //    try
        //    {
        //        InsertEventLog("GetLeadSources", EventType.Log, EventColor.yellow, "Get List Of LeadSourceDto", "TICRM.BusinessLayer.LeadManager.GetLeadSources", "");
        //        List<LeadSourceDto> leadSourceDtos = new List<LeadSourceDto>();

        //        foreach (LeadSource item in dbEnt.LeadSources.CollectionNotNull())
        //        {
        //            leadSourceDtos.Add(objMapper.GetLeadSourceDTO(item));
        //        }
        //        return leadSourceDtos;
        //    }
        //    catch (Exception ex)
        //    {

        //        InsertEventMonitor("GetLeadSources", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLeadSources", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get Industries Dtos list
        /// </summary>
        /// <returns></returns>
        
        //public List<IndustryDto> GetIndustries()
        //{
        //    try
        //    {
        //        InsertEventLog("GetIndustries", EventType.Log, EventColor.yellow, "Get List Of IndustryDto", "TICRM.BusinessLayer.LeadManager.GetIndustries", "");
        //        List<IndustryDto> industriesDtos = new List<IndustryDto>();

        //        foreach (Industry item in dbEnt.Industries.CollectionNotNull())
        //        {
        //            industriesDtos.Add(objMapper.GetIndustryDTO(item));
        //        }
        //        return industriesDtos;
        //    }
        //    catch (Exception ex)
        //    {

        //        InsertEventMonitor("GetIndustries", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetIndustries", "");
        //        throw;
        //    }
        //}

   
      


        #endregion

        /// <summary>
        /// Gets the lead on the basis of id.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>LeadDto.</returns>
        public LeadDto GetLead(Guid? guid)
        {
            try
            {
                InsertEventLog("GetLead", EventType.Log, EventColor.yellow, "Get LeadDto", "TICRM.BusinessLayer.LeadManager.GetLead", "");
                return objMapper.GetLeadDTO(dbEnt.Leads.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetLead", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLead", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// Get Lead list 
        /// </summary>
        /// <returns></returns>
        public List<LeadDto> GetLeads(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetLeads", EventType.Log, EventColor.yellow, "Get list of LeadDto", "TICRM.BusinessLayer.LeadManager.GetLeads", "");
                List<LeadDto> LeadDtos = new List<LeadDto>();
                //List<Lead> leads = dbEnt.Leads.Include(l => l.Address).Include(l => l.Industry).Include(l => l.LeadSource).Include(l => l.LeadType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).ToList();
                List<Lead> leads = dbEnt.sp_Leads_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (Lead item in leads.CollectionNotNull())
                {
                    LeadDtos.Add(objMapper.GetLeadDTO(item));
                }
                return LeadDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetLeads", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLeads", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// save and edit Lead 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool SaveLead(LeadDto acc, string CurrentUserId, string UserCompanyID, bool isEditMode, bool isDeleteMode)
        {
            try
            {
                InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                Lead Lead;

                if (isEditMode)
                {
                    InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "going to edit Lead of id =" + acc.LeadId + "", "TICRM.BusinessLayer.LeadManager.SaveLead", "");

                    Lead = objMapper.GetLead(acc);
                    Lead leadEdit = dbEnt.Leads.FirstOrDefault(x => x.LeadId == Lead.LeadId);
                    if (leadEdit!=null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "going to delete Lead of id =" + Lead.LeadId + "", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                            //Soft Delete Lead
                            Lead leadDelete = dbEnt.Leads.FirstOrDefault(x => x.LeadId == Lead.LeadId);
                            leadDelete.IsDeleted = true;
                        }
                        else
                        {
                            //Update Lead
                            leadEdit.Name = Lead.Name;
                            leadEdit.LeadTypeId = Lead.LeadTypeId;
                            leadEdit.LeadSourceId = Lead.LeadSourceId;
                            leadEdit.PhoneNumber = Lead.PhoneNumber;
                            leadEdit.AddressId = Lead.AddressId;
                            leadEdit.Email = Lead.Email;
                            leadEdit.IndustryId = Lead.IndustryId;
                            leadEdit.Description = Lead.Description;
                            leadEdit.AssignedUser = Lead.AssignedUser;
                            leadEdit.AssignedTeam = Lead.AssignedTeam;
                            leadEdit.StatusId = Lead.StatusId;
                            leadEdit.UpdatedBy = CurrentUserId;
                            leadEdit.UpdatedDate = DateTime.Now;
                        }
                    }
                    
                }
                else
                {
                    //Save Lead in DB
                    InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "going to create new Record", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                    Lead = objMapper.GetLead(acc);
                    Lead.LeadId = Guid.NewGuid();
                    Lead.Company = Guid.Parse(UserCompanyID);
                    Lead.CreatedBy = CurrentUserId;
                    Lead.CreatedDate = DateTime.Now;
                    dbEnt.Leads.Add(Lead);
                }
                HttpContext.Current.Session["LeadObject"] = Lead;
                HttpContext.Current.Session["CurrentUserId"] = CurrentUserId;
                // apply condition to check if db changes is done then  return true in response 
                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "Lead saved Successfully of id =" + Lead.LeadId + "", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                    return true;
                }

            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveLead", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

            return false;
        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Leads", "TICRM.BuisnessLayer.LeadsManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Leads.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.LeadsManager.GetTotalCount", "");
                throw ex;
            }
        }
    }
}
