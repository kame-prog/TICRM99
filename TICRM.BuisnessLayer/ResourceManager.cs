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
    ||  Class [ResourceManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving resources . Getting a specific resource
    ||             on the basis of Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ResourceManager : BaseManager
    {
        public ResourceManager()
        {

        }
        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ResourceDto.</returns>
        public ResourceDto GetResource(Guid? guid)
        {
            try
            {
                InsertEventLog("GetResource", EventType.Log, EventColor.yellow, "Get ResourceDto on resource id","TICRM.BusinessLayer.ResourceManager.GetResource", "");
                return objMapper.GetResourceDTO(dbEnt.Resources.Find(guid));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetResource", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ResourceManager.GetResource", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Gets the resources list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;ResourceDto&gt;.</returns>
        public List<ResourceDto> GetResourcesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetResourcesList", EventType.Log, EventColor.yellow, "Get List of Resources Based on Datatable Query", "TICRM.BuisnessLayer.ResourceManager.GetResourcesList", "");

                var Resource = new List<Resource>();
                var ResourceDto = new List<ResourceDto>();

                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                if (!string.IsNullOrEmpty(sSearch))
                {
                    Resource = dbEnt.Resources.Include(r => r.Status)
                                .Include(r => r.Team).Include(r => r.User)
                                .Include(r => r.Address1).Include(r => r.Address2)
                                .Where(a => a.IsDeleted != true &&
                                (a.Name.ToLower().Contains(sSearch) || a.Email.ToLower().Contains(sSearch)
                                || a.Description.ToLower().Contains(sSearch)
                                || a.PhoneHome.ToLower().Contains(sSearch)
                                || a.PhoneOffice.ToLower().Contains(sSearch)
                                || a.Website.ToLower().Contains(sSearch)
                                || a.Status.Name.ToLower().Contains(sSearch)
                                || a.Address1.Street1.ToLower().Contains(sSearch)
                                || a.Address2.Street1.ToLower().Contains(sSearch)
                                || a.Team.Name.ToLower().Contains(sSearch)
                                || a.User.Name.ToLower().Contains(sSearch))
                                ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    Resource = dbEnt.Resources.Include(r => r.Status)
                               .Include(r => r.Team).Include(r => r.User)
                               .Include(r => r.Address1).Include(r => r.Address2)
                               .Where(a => a.IsDeleted != true)
                               .OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Resource item in Resource.CollectionNotNull())
                {
                    ResourceDto.Add(objMapper.GetResourceDTO(item)); // add in a list object
                }
                return ResourceDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetResourcesList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ResourceManager.GetResourcesList", "");

                throw ex;
            }
        }

        /// <summary>
        /// Get resource list 
        /// </summary>
        /// <returns></returns>
        public List<ResourceDto> GetResources(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetResources", EventType.Log, EventColor.yellow, "Get list of ResourceDto on resource id","TICRM.BusinessLayer.ResourceManager.GetResources", "");
                List<ResourceDto> resourceDtos = new List<ResourceDto>();
                //List<Resource> resources = dbEnt.Resources.Include(r => r.Status).Include(r => r.Team).Include(r => r.User).Include(r => r.Address1).Include(r => r.Address2).Where(a => a.IsDeleted != true).ToList();
                List<Resource> resources = dbEnt.sp_Resources_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (Resource item in resources.CollectionNotNull())
                {
                    resourceDtos.Add(objMapper.GetResourceDTO(item));
                }
                return resourceDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetResources", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ResourceManager.GetResources", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// save and edit ResourceDto 
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool SaveResource(ResourceDto res,string CurrentUserId,string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveResource", EventType.Log, EventColor.yellow, "Enter","TICRM.BusinessLayer.ResourceManager.SaveResource", "");
                Resource resource;
                if (isEditMode)
                {
                    InsertEventLog("SaveResource", EventType.Log, EventColor.yellow, "going to edit Resource","TICRM.BusinessLayer.ResourceManager.SaveResource", "");
                    resource = objMapper.GetResource(res);
                    if (isDeleteMode)
                    {
                        //Soft delete Data
                        InsertEventLog("SaveResource", EventType.Log, EventColor.yellow, "going to delete on id","TICRM.BusinessLayer.ResourceManager.SaveResource", "");
                        Resource resourceForDelete = dbEnt.Resources.FirstOrDefault(x => x.ResourceId == resource.ResourceId);
                        resourceForDelete.IsDeleted = true;
                    }
                    else
                    {
                        //Update data in DB
                        Resource resourceEdit = dbEnt.Resources.FirstOrDefault(x => x.ResourceId == resource.ResourceId);
                        resourceEdit.Name = resource.Name;
                        resourceEdit.PhoneOffice = resource.PhoneOffice;
                        resourceEdit.Address = resource.Address;
                        resourceEdit.CurrentAddress = resource.CurrentAddress;
                        resourceEdit.Description = resource.Description;
                        resourceEdit.StatusId = resource.StatusId;
                        resourceEdit.PhoneHome = resource.PhoneHome;
                        resourceEdit.AssignedUser = resource.AssignedUser;
                        resourceEdit.AssignedTeam = resource.AssignedTeam;
                        resourceEdit.Email = resource.Email;
                        resourceEdit.Website = resource.Website;
                        resourceEdit.UpdatedBy = CurrentUserId;
                        resourceEdit.UpdatedDate = DateTime.Now;
                    }
                }
                else
                {
                    //Save data in DB
                    InsertEventLog("SaveResource", EventType.Log, EventColor.yellow, "Create new Record of Resource","TICRM.BusinessLayer.ResourceManager.SaveResource", "");
                    resource = objMapper.GetResource(res);
                    resource.ResourceId = Guid.NewGuid();
                    resource.CreatedDate = DateTime.Now;
                    resource.CreatedBy = CurrentUserId;
                    resource.Company = Guid.Parse(UserCompanyID);
                    dbEnt.Resources.Add(resource);
                }

                dbEnt.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveResource", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ResourceManager.SaveResource", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Resources", "TICRM.BuisnessLayer.ResourceManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Resources.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ResourceManager.GetTotalCount", "");
                throw ex;
            }
        }
    }
}
