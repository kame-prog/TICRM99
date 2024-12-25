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
    public class LeadManager : BaseManager
    {
        public LeadManager()
        {
            Industries = GetIndustries();
            LeadSources = GetLeadSources();
            LeadTypes = GetLeadTypes();
        }

        #region Properties & Methods

        public List<IndustryDto> Industries { get; set; }
        public List<LeadSourceDto> LeadSources { get; set; }
        public List<LeadTypeDto> LeadTypes { get; set; }


        /// <summary>
        /// Get Lead types
        /// </summary>
        /// <returns></returns>
        public List<LeadTypeDto> GetLeadTypes()
        {
            try
            {
                InsertEventLog("GetLeadTypes", EventType.Log, EventColor.yellow, "Get List Of LeadTypeDto", "TICRM.BusinessLayer.LeadManager.GetLeadTypes", "");
                List<LeadTypeDto> leadTypesDtos = new List<LeadTypeDto>();

                foreach (LeadType item in dbEnt.LeadTypes.CollectionNotNull())
                {
                    leadTypesDtos.Add(objMapper.GetLeadTypeDTO(item));
                }
                return leadTypesDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetLeadTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLeadTypes", "");

                throw;
            }
        }


        /// <summary>
        /// Get Lead Sources
        /// </summary>
        /// <returns></returns>
        public List<LeadSourceDto> GetLeadSources()
        {
            try
            {
                InsertEventLog("GetLeadSources", EventType.Log, EventColor.yellow, "Get List Of LeadSourceDto", "TICRM.BusinessLayer.LeadManager.GetLeadSources", "");
                List<LeadSourceDto> leadSourceDtos = new List<LeadSourceDto>();

                foreach (LeadSource item in dbEnt.LeadSources.CollectionNotNull())
                {
                    leadSourceDtos.Add(objMapper.GetLeadSourceDTO(item));
                }
                return leadSourceDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetLeadSources", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLeadSources", "");
                throw;
            }
        }

        /// <summary>
        /// Get Industries Dtos list
        /// </summary>
        /// <returns></returns>
        public List<IndustryDto> GetIndustries()
        {
            try
            {
                InsertEventLog("GetIndustries", EventType.Log, EventColor.yellow, "Get List Of IndustryDto", "TICRM.BusinessLayer.LeadManager.GetIndustries", "");
                List<IndustryDto> industriesDtos = new List<IndustryDto>();

                foreach (Industry item in dbEnt.Industries.CollectionNotNull())
                {
                    industriesDtos.Add(objMapper.GetIndustryDTO(item));
                }
                return industriesDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetIndustries", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetIndustries", "");
                throw;
            }
        }



        #endregion

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
                throw ex;
            }


        }
        /// <summary>
        /// Get Lead list 
        /// </summary>
        /// <returns></returns>
        public List<LeadDto> GetLeads()
        {
            try
            {
                InsertEventLog("GetLeads", EventType.Log, EventColor.yellow, "Get list of LeadDto", "TICRM.BusinessLayer.LeadManager.GetLeads", "");
                List<LeadDto> LeadDtos = new List<LeadDto>();
                List<Lead> leads = dbEnt.Leads.Include(l => l.Address).Include(l => l.Industry).Include(l => l.LeadSource).Include(l => l.LeadType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).ToList();
                foreach (Lead item in leads.CollectionNotNull())
                {
                    LeadDtos.Add(objMapper.GetLeadDTO(item));
                }
                return LeadDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetLeads", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.GetLeads", "");
                throw;
            }


        }
        /// <summary>
        /// save and edit Lead 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool SaveLead(LeadDto acc, bool isEditMode, bool isDeleteMode, string CurrentUserId)
        {
            try
            {
                InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                Lead Lead;

                if (isEditMode)
                {
                    InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "going to edit Lead of id =" + acc.LeadId + "", "TICRM.BusinessLayer.LeadManager.SaveLead", "");

                    Lead = objMapper.GetLead(acc);
                    if (isDeleteMode)
                    {
                        InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "going to delete Lead of id =" + Lead.LeadId + "", "TICRM.BusinessLayer.LeadManager.SaveLead", "");

                        Lead leadDelete = dbEnt.Leads.FirstOrDefault(x => x.LeadId == Lead.LeadId);
                        leadDelete.IsDeleted = true;
                        dbEnt.Entry(leadDelete).State = EntityState.Modified;
                    }
                    else
                    {
                        dbEnt.Entry(Lead).State = EntityState.Modified;
                    }
                }
                else
                {
                    InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "going to create new Record", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                    Lead = objMapper.GetLead(acc);
                    Lead.LeadId = Guid.NewGuid();
                    dbEnt.Leads.Add(Lead);
                }
                HttpContext.Current.Session["LeadObject"] = Lead;
                HttpContext.Current.Session["CurrentUserId"] = CurrentUserId;

                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("SaveLead", EventType.Log, EventColor.yellow, "Lead saved Successfully of id =" + Lead.LeadId + "", "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                    return true;
                }

            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveLead", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LeadManager.SaveLead", "");
                throw ex;
            }

            return false;
        }


    }
}
