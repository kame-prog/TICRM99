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
    ||             Getting Locations in general and specifically on Id and saving it and
    ||             location types. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class LocationManager : BaseManager
    {
        public LocationManager()
        {
            //LocationTypes = GetLocationTypes();
            //Location = GetLocations();
        }
        //public List<LocationTypeDto> LocationTypes { get; set; }
        public List<LocationDto> Location { get; set; }
        
        /// <summary>
        /// Get Location Types
        /// </summary>
        /// <returns></returns>
        
        //public List<LocationTypeDto> GetLocationTypes()
        //{
        //    try
        //    {
        //        InsertEventLog("GetLocationTypes", EventType.Log, EventColor.yellow, "Get List Of LocationTypeDto", "TICRM.BusinessLayer.LocationManager.GetLocationTypes", "");
        //        List<LocationTypeDto> urgencyDtos = new List<LocationTypeDto>();

        //        foreach (LocationType item in dbEnt.LocationTypes.CollectionNotNull())
        //        {
        //            urgencyDtos.Add(objMapper.GetLocationTypeDTO(item));
        //        }
        //        return urgencyDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("LocationTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LocationManager.GetLocationTypes", "");

        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the locations list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;LocationDto&gt;.</returns>
        public List<LocationDto> GetLocationsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetLocationsList", EventType.Log, EventColor.yellow, "to get list of Locations", "TICRM.BuisnessLayer.LocationsManager.GetLocationsList", "");
                List<LocationDto> locationDto = new List<LocationDto>();// create strongly type list Object of EventNotification DTO
                List<Location> location = new List<Location>(); // Get List Of EventNotifications from DB


                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    location = dbEnt.Locations.Include(l => l.Address).Include(l => l.LocationType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true && (a.Name.ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.Address.Street1.ToLower().Contains(sSearch)
                    || a.Latitude.ToString().ToLower().Contains(sSearch)
                    || a.Longitude.ToString().ToLower().Contains(sSearch)
                    || a.AccountId.ToString().ToLower().Contains(sSearch)
                    || a.Address.Street1.ToLower().Contains(sSearch)
                    || a.LocationType.Name.ToLower().Contains(sSearch)
                    || a.Status.Name.ToLower().Contains(sSearch)
                    || a.Team.Name.ToLower().Contains(sSearch)
                    || a.User.Name.ToLower().Contains(sSearch))).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    location = dbEnt.Locations.Include(l => l.Address).Include(l => l.LocationType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();


                // apply iteration on eventNotifications
                foreach (Location item in location.CollectionNotNull())
                {
                    locationDto.Add(objMapper.GetLocationDTO(item)); // add in a list object
                }
                return locationDto; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetLocationsList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.LocationsManager.GetLocationsList", "");
                throw;
            }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>LocationDto.</returns>
        public LocationDto GetLocation(Guid? guid)
        {
            try
            {
                InsertEventLog("GetLocation", EventType.Log, EventColor.yellow, "Get LocationDto", "TICRM.BusinessLayer.LocationManager.GetLocation", "");
                return objMapper.GetLocationDTO(dbEnt.Locations.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetLocation", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LocationManager.GetLocation", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        public bool SaveLocation(LocationDto location, string currentUserId, bool v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get location list 
        /// </summary>
        /// <returns></returns>
        public List<LocationDto> GetLocations(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetLocations", EventType.Log, EventColor.yellow, "Get list of LocationDto", "TICRM.BusinessLayer.LocationManager.GetLocations", "");
                List<LocationDto> locationDtos = new List<LocationDto>();
                //List<Location> locations = dbEnt.Locations.Include(l => l.Address).Include(l => l.LocationType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).ToList();
                List<Location> locations = dbEnt.sp_Locations_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (Location item in locations.CollectionNotNull())
                {
                    locationDtos.Add(objMapper.GetLocationDTO(item));
                }
                return locationDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetLocations", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LocationManager.GetLocations", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the locations on account id.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>List&lt;LocationDto&gt;.</returns>
        public List<LocationDto> GetLocations(Guid? accountId)
        {
            try
            {
                InsertEventLog("GetLocations", EventType.Log, EventColor.yellow, "Get list of LocationDto on accouont Id=" + accountId + "", "TICRM.BusinessLayer.LocationManager.GetLocations", "");
                List<LocationDto> locationDtos = new List<LocationDto>();
                List<Location> locations = dbEnt.Locations.Include(l => l.Address).Include(l => l.LocationType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true && a.AccountId == accountId).ToList();

                foreach (Location item in locations.CollectionNotNull())
                {
                    locationDtos.Add(objMapper.GetLocationDTO(item));
                }
                return locationDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetLocations", EventType.Exception, EventType.Exception, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LocationManager.GetLocations", "");
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
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Locations", "TICRM.BuisnessLayer.LocationManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Locations.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.LocationManager.GetTotalCount", "");
                throw ex;
            }
        }

        /// <summary>
        /// save and edit LocationDto 
        /// </summary>
        /// <param name="loca"></param>
        /// <returns></returns>
        public bool SaveLocation(LocationDto loca, string CurrentUserId, string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveLocation", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.LocationManager.SaveLocation", "");
                Location location;
                if (isEditMode)
                {
                    location = objMapper.GetLocation(loca);
                    Location objDB = dbEnt.Locations.FirstOrDefault(x => x.LocationId == location.LocationId);
                   
                    if (objDB != null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveLocation", EventType.Log, EventColor.yellow, "Enter in delete mode", "TICRM.BusinessLayer.LocationManager.SaveLocation", "");
                            Location locationforDelete = dbEnt.Locations.FirstOrDefault(x => x.LocationId == location.LocationId);
                            locationforDelete.IsDeleted = true;
                        }
                        else
                        {
                            Location locationEdit = dbEnt.Locations.FirstOrDefault(x => x.LocationId == location.LocationId);
                            locationEdit.Name = location.Name;
                            locationEdit.AccountId = location.AccountId;
                            locationEdit.Latitude = location.Latitude;
                            locationEdit.Longitude = location.Longitude;
                            locationEdit.AddressId = location.AddressId;
                            locationEdit.StatusId = location.StatusId;
                            locationEdit.LocationTypeId = location.LocationTypeId;
                            locationEdit.AssignedUser = location.AssignedUser;
                            locationEdit.AssignedTeam = location.AssignedTeam;
                            locationEdit.Description = location.Description;
                            locationEdit.UpdatedBy = CurrentUserId;
                            locationEdit.UpdatedDate = DateTime.Now;

                        }
                        HttpContext.Current.Session["LocationObject"] = location;
                        HttpContext.Current.Session["CurrentUserId"] = CurrentUserId;
                        if (dbEnt.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    InsertEventLog("SaveLocation", EventType.Log, EventColor.yellow, "Create a new Record", "TICRM.BusinessLayer.LocationManager.SaveLocation", "");

                    location = objMapper.GetLocation(loca);
                    location.LocationId = Guid.NewGuid();
                    location.CreatedDate = DateTime.Now;
                    location.CreatedBy = CurrentUserId;
                    location.Company = Guid.Parse(UserCompanyID);
                    dbEnt.Locations.Add(location);
                }

               if ( dbEnt.SaveChanges() >0)
                {
                    HttpContext.Current.Session["LocationObject"] = location;
                    InsertEventLog("SaveLocation", EventType.Log, EventColor.yellow, "Data saved successfully in SaveLocation", "TICRM.BusinessLayer.LocationManager.SaveLocation", "");
                    return true;

                }

                return false;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveLocation", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.LocationManager.SaveLocation", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

    }
}
