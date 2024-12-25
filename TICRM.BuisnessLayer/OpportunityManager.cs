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
    ||             Getting, Updating and Saving opportunities. Getting opportunity stages,
    ||             getting currencies, getting probabilties]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||  Changes Made:   [20/08/2020     Server Side Pagination Added            Akhtar Zaman]
    ****************************************************************************************/
    public class OpportunityManager : BaseManager
    {
        public OpportunityManager()
        {
            //Currencies = GetCurrencies();
            //OpportunityStages = GetOpportunityStages();
            //Probabilities = GetProbabilities();
        }

        #region Properties 

        //public List<CurrencyDto> Currencies { get; set; }
        //public List<OpportunityStageDto> OpportunityStages { get; set; }
        //public List<PobabilityDto> Probabilities { get; set; }


        #endregion

        /// <summary>
        /// Get Currencies 
        /// </summary>
        /// <returns></returns>
        
        //public List<CurrencyDto> GetCurrencies()
        //{
        //    try
        //    {
        //        InsertEventLog("GetCurrencies", EventType.Log, EventColor.yellow, "Get List Of CurrencyDto", "TICRM.BusinessLayer.OpportunityManager.GetCurrencies", "");
        //        List<CurrencyDto> currenciesDtos = new List<CurrencyDto>();

        //        foreach (Currency item in dbEnt.Currencies.CollectionNotNull())
        //        {
        //            currenciesDtos.Add(objMapper.GetCurrencyDTO(item));
        //        }
        //        return currenciesDtos;
        //    }
        //    catch (Exception ex)
        //    {

        //        InsertEventMonitor("GetCurrencies", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetCurrencies", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Opportunity Stages Dto
        /// </summary>
        /// <returns></returns>
        
        //public List<OpportunityStageDto> GetOpportunityStages()
        //{
        //    try
        //    {
        //        InsertEventLog("GetOpportunityStages", EventType.Log, EventColor.yellow, "Get List Of OpportunityStageDto", "TICRM.BusinessLayer.OpportunityManager.GetOpportunityStages", "");
        //        List<OpportunityStageDto> opportunityStageDtos = new List<OpportunityStageDto>();

        //        foreach (OpportunityStage item in dbEnt.OpportunityStages.CollectionNotNull())
        //        {
        //            opportunityStageDtos.Add(objMapper.GetOpportunityStageDTO(item));
        //        }
        //        return opportunityStageDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetOpportunityStages", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetOpportunityStages", "");

        //        throw;
        //    }
        //}

        /// <summary>
        /// Get Probability Dtos
        /// </summary>
        /// <returns></returns>
        
        //public List<PobabilityDto> GetProbabilities()
        //{
        //    try
        //    {
        //        InsertEventLog("GetProbabilities", EventType.Log, EventColor.yellow, "Get List Of PobabilityDto", "TICRM.BusinessLayer.OpportunityManager.GetProbabilities", "");
        //        List<PobabilityDto> pobabilityDtos = new List<PobabilityDto>();

        //        foreach (Pobability item in dbEnt.Pobabilities.CollectionNotNull())
        //        {
        //            pobabilityDtos.Add(objMapper.GetPobabilityDTO(item));
        //        }
        //        return pobabilityDtos;
        //    }
        //    catch (Exception ex)
        //    {

        //        InsertEventMonitor("GetProbabilities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetProbabilities", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the opportunities count.
        /// </summary>
        /// <returns>Counts.</returns>
        public Counts GetOpportunitiesCount()
        {
            try
            {
                InsertEventLog("GetOpportunitiesCount", EventType.Log, EventColor.yellow, "to get total no of Oppourtunities count as open lost ,won", "TICRM.BuisnessLayer.OpportunityManager.GetOpportunitiesCount", "");
                var c = new Counts
                {
                    Open = dbEnt.Opportunities.Include(o => o.OpportunityStage).Where(a => a.OpportunityStage.Name == "Negotiation").Count(),
                    Lost = dbEnt.Opportunities.Include(o => o.OpportunityStage).Where(a => a.OpportunityStage.Name == "Closed Lost").Count(),
                    Lostwon = dbEnt.Opportunities.Include(o => o.OpportunityStage).Where(a => a.OpportunityStage.Name == "Closed New").Count()
                };
                
                return c;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetOpportunitiesCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.OpportunityManager.GetOpportunitiesCount", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the opportunities count for account.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Counts.</returns>
        public Counts GetOpportunitiesCountAccount(Guid id)
        {
            try
            {

                InsertEventLog("GetOpportunitiesCount", EventType.Log, EventColor.yellow, "to get total no of Oppourtunities count as open lost ,won", "TICRM.BuisnessLayer.OpportunityManager.GetOpportunitiesCount", "");
                var c = new Counts
                {
                    Open = dbEnt.Opportunities.Include(o => o.OpportunityStage).Where(a => a.OpportunityStage.Name == "Negotiation" && a.AccountId == id).Count(),
                    Lost = dbEnt.Opportunities.Include(o => o.OpportunityStage).Where(a => a.OpportunityStage.Name == "Closed Lost" && a.AccountId == id).Count(),
                    Lostwon = dbEnt.Opportunities.Include(o => o.OpportunityStage).Where(a => a.OpportunityStage.Name == "Closed New" && a.AccountId == id).Count()
                };

                return c;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetOpportunitiesCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.OpportunityManager.GetOpportunitiesCount", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the opportunity.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>OpportunityDto.</returns>
        public OpportunityDto GetOpportunity(Guid? guid)
        {
            try
            {
                InsertEventLog("GetOpportunity", EventType.Log, EventColor.yellow, "going to Get OpportunityDto on id" + guid + "", "TICRM.BusinessLayer.OpportunityManager.GetOpportunity", "");
                return objMapper.GetOpportunityDTO(dbEnt.Opportunities.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetOpportunity", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetOpportunity", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        
        /// <summary>
        /// Get opportunity list 
        /// </summary>
        /// <returns></returns>
        public List<OpportunityDto> GetOpportunities(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetOpportunities", EventType.Log, EventColor.yellow, "going to Get List of OpportunityDto", "TICRM.BusinessLayer.OpportunityManager.GetOpportunities", "");
                List<OpportunityDto> opportunityDtos = new List<OpportunityDto>();
                //List<Opportunity> opportunities = dbEnt.Opportunities.Include(o => o.Currency).Include(o => o.Team).Include(o => o.User).Include(o => o.OpportunityStage).Include(o => o.Pobability).Include(o => o.Status).Where(a => a.IsDeleted != true).ToList();
                List<Opportunity> opportunities= dbEnt.sp_Opportunities_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (Opportunity item in opportunities.CollectionNotNull())
                {
                    var s = item.OpportunityStage;
                    opportunityDtos.Add(objMapper.GetOpportunityDTO(item));
                }
                return opportunityDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetOpportunities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetOpportunities", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Gets the opportunities on account ID.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>List&lt;OpportunityDto&gt;.</returns>
        public List<OpportunityDto> GetOpportunities(Guid? accountId)
        {
            try
            {
                InsertEventLog("GetOpportunities", EventType.Log, EventColor.yellow, "going to Get List of OpportunityDto on account id = " + accountId + "", "TICRM.BusinessLayer.OpportunityManager.GetOpportunities", "");
                List<OpportunityDto> opportunityDtos = new List<OpportunityDto>();
                List<Opportunity> opportunities = dbEnt.Opportunities.Include(o => o.Currency).Include(o => o.Team).Include(o => o.User).Include(o => o.OpportunityStage).Include(o => o.Pobability).Include(o => o.Status).Where(a => a.IsDeleted != true && a.AccountId == accountId).ToList();

                foreach (Opportunity item in opportunities.CollectionNotNull())
                {
                    opportunityDtos.Add(objMapper.GetOpportunityDTO(item));

                }
                return opportunityDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetOpportunities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetOpportunities", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// save and edit opportunityDtos 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool SaveOpportunity(OpportunityDto opportunityDto, string CurrentUserId,string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveOpportunity", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.OpportunityManager.SaveOpportunity", CurrentUserId);
                Opportunity opportunity;
                if (isEditMode)
                {

                    InsertEventLog("SaveOpportunity", EventType.Log, EventColor.yellow, "going to update Opportunity on id=" + opportunityDto.OpportunityId + "", "TICRM.BusinessLayer.OpportunityManager.SaveOpportunity", CurrentUserId);
                    opportunity = objMapper.GetOpportunity(opportunityDto);
                    Opportunity DbOpportunity = dbEnt.Opportunities.FirstOrDefault(x => x.OpportunityId == opportunity.OpportunityId);
                    if (DbOpportunity != null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveOpportunity", EventType.Log, EventColor.yellow, "going to update Opportunity on id=" + opportunityDto.OpportunityId + "", "TICRM.BusinessLayer.OpportunityManager.SaveOpportunity", CurrentUserId);
                            //Soft Delete Opportunity
                            Opportunity opportunityForDelete = dbEnt.Opportunities.FirstOrDefault(x => x.OpportunityId == opportunity.OpportunityId);
                            opportunityForDelete.IsDeleted = true;
                        }
                        else
                        {
                            //Edit opportunity method 
                            DbOpportunity.Amount = opportunity.Amount;
                            DbOpportunity.ProbabilityId = opportunity.ProbabilityId;
                            DbOpportunity.OpportunityStageId = opportunity.OpportunityStageId;
                            DbOpportunity.Title = opportunity.Title;
                            //DbOpportunity.CurrencyId = opportunity.CurrencyId;
                            DbOpportunity.StatusId = opportunity.StatusId;
                            DbOpportunity.AssignedUser = opportunity.AssignedUser;
                            DbOpportunity.AssignedTeam = opportunity.AssignedTeam;
                            DbOpportunity.Description = opportunity.Description;
                            DbOpportunity.AccountId = opportunity.AccountId;
                            DbOpportunity.Latitude = opportunity.Latitude;
                            DbOpportunity.Longitude = opportunity.Longitude;
                            DbOpportunity.UpdatedBy = CurrentUserId;
                            DbOpportunity.UpdatedDate = DateTime.Now;
                        }
                    }
                }
                else
                {
                    opportunity = objMapper.GetOpportunity(opportunityDto);
                    opportunity.OpportunityId = Guid.NewGuid();
                    opportunity.CreatedBy = CurrentUserId;
                    opportunity.Company = Guid.Parse(UserCompanyID);
                    opportunity.CreatedDate = DateTime.Now;
                    dbEnt.Opportunities.Add(opportunity);
                }
                HttpContext.Current.Session["OpportunityObject"] = opportunity;
                HttpContext.Current.Session["CurrentUserId"] = CurrentUserId;

                if (dbEnt.SaveChanges() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveOpportunity", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.SaveOpportunity", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;
        }

        /// <summary>
        ///   Return a string of data for datatables to render on front end
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns>String of data</returns>
        public List<OpportunityDto> GetopportunitiesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetopportunitiesList", EventType.Log, EventColor.yellow, "Get List of Opportunities Based on Datatable Query", "TICRM.BuisnessLayer.OpportunityManager.GetopportunitiesList", "");

                var opportunities = new List<Opportunity>();
                var opportunitiesDto = new List<OpportunityDto>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                if (!string.IsNullOrEmpty(sSearch))
                {
                    opportunities = dbEnt.Opportunities.Where(a => a.Title.ToLower().Contains(sSearch)
                    || a.Amount.Equals(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.Currency.Name.ToString().ToLower().Contains(sSearch)
                    || a.Team.Name.ToString().ToLower().Contains(sSearch)
                    || a.User.Name.ToString().ToLower().Contains(sSearch)
                    || a.OpportunityStage.Name.ToString().ToLower().Contains(sSearch)
                    || a.Pobability.Name.ToString().ToLower().Contains(sSearch)
                    || a.Status.Name.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    opportunities = dbEnt.Opportunities.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Opportunity item in opportunities.CollectionNotNull())
                {
                    opportunitiesDto.Add(objMapper.GetOpportunityDTO(item)); // add in a list object
                }

                return opportunitiesDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetopportunitiesList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.OpportunityManager.GetopportunitiesList", "");

                throw ex;
            }

        }

        /// <summary>
        /// Count all Opportunites
        /// </summary>
        /// <returns>No of total activites</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Devices", "TICRM.BuisnessLayer.OpportunityManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Opportunities.AsQueryable().Count(); 

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.OpportunityManager.GetTotalCount", "");
                throw ex;
            }
        }

        #region Object classes
        public class Counts
        {
            public int Open;
            public int Lost;
            public int Lostwon;

        }

        public class Admin
        {
            public int Team;
            public int Users;
        }
        #endregion

    }
}
