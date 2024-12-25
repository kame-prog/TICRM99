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
    /************************************************************************************
    ||  Class [ActivityManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             activity are get from the database and mapped with the corrosponding DTO
    ||             object to send it back to the controller]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ActivityManager : BaseManager
    {

        /// <summary>
        /// Gets the activities.
        /// </summary>
        /// <returns>List&lt;ActivityDTO&gt;.</returns>
        public List<ActivityDTO> GetActivities( string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetActivities", EventType.Log, EventColor.yellow, "Successfully Enter in GetActivities","TICRM.BusinessLayer.ActivityManager", "");

                List<ActivityDTO> ActivityDtos = new List<ActivityDTO>(); // create list Object of Activity DTO

                //List<DAL.Activity> activities = dbEnt.Activities.Where(x=>x.IsDeleted!=true).Where(x=>x.CreatedBy== CurrentUserId).ToList(); // Get List Of Activities from DB
                List<Activity> activities = dbEnt.sp_Activities_Get(CurrentUserId, UserRole, UserCompany).ToList();
                // apply iteration on getting activities
                foreach (DAL.Activity item in activities.CollectionNotNull())
                {
                    ActivityDtos.Add(objMapper.GetActivityDTO(item)); // add in a list object
                }
                return ActivityDtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetActivities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ActivityManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        /// <summary>
        /// Saves the activity.
        /// </summary>
        /// <param name="dvc">The DVC.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveActivity(ActivityDTO objactivity, string CurrentUserId, String UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveActivity", EventType.Log, EventColor.yellow, "Successfully Enter in SaveActivity","TICRM.BusinessLayer.ActivityManager", "");
                Activity activity; // create a new object
                
                if (isEditMode) // check if is is edit mode is true
                {
                    activity = objMapper.GetActivity(objactivity); // pass parameter object to activity object
                    Activity dbData = dbEnt.Activities.FirstOrDefault(x => x.ActivityId == activity.ActivityId); // get data from database and pass in new activity class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveActivity", EventType.Log, EventColor.yellow, "For Delete Successfully Enter in SaveActivity","TICRM.BusinessLayer.ActivityManager", "");
                            Activity deleteactivity = dbEnt.Activities.FirstOrDefault(x => x.ActivityId == activity.ActivityId);
                            deleteactivity.IsDeleted = true;
                        }
                        else
                        {
                        InsertEventLog("SaveActivity", EventType.Log, EventColor.yellow, "For Edit Successfully Enter", "TICRMTICRM.BuisnessLayer.ActivityManager.SaveActivity", "");
                            dbData.Name = activity.Name;
                            dbData.Description = activity.Description;
                            dbData.Type = activity.Type;
                            dbData.RelatedTo = activity.RelatedTo;
                            dbData.RelatedToID = activity.RelatedToID;
                            dbData.StatusId = activity.StatusId;
                            dbData.AssignedUser = activity.AssignedUser;
                            dbData.AssignedTeam = activity.AssignedTeam;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveActivity", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id "+ objactivity.ActivityId ,"TICRM.BuisnessLayer.ActivityTemplateManager.SaveActivity", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveActivity", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.ActivityManager.SaveActivity", "");

                    activity = objMapper.GetActivity(objactivity);  // pass parameter activity object to activity object
                    activity.ActivityId = Guid.NewGuid();
                    activity.Company = Guid.Parse(UserCompanyID);
                    activity.CreatedBy = CurrentUserId;
                    activity.CreatedDate= DateTime.Now;

                    dbEnt.Activities.Add(activity); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }



            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveActivity", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRMTICRM.BuisnessLayer.ActivityManager.SaveActivity", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;

        }

        /// <summary>
        /// Gets the activity.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ActivityDTO.</returns>
        public ActivityDTO GetActivity(Guid? guid)
        {
            try
            {
                        InsertEventLog("GetActivity", EventType.Log, EventColor.yellow, "Successfully Enter in GetActivity to Get Data on id","TICRM.BusinessLayer.ActivityManager", "");
                        return objMapper.GetActivityDTO(dbEnt.Activities.Find(guid)); // get activity on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountSize", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ActivityManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the account activities.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>List&lt;ActivityDTO&gt;.</returns>
        public List<ActivityDTO> GetAccountActivities(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAccountActivities", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.ActivityManager.GetAccountActivities", "");
                List<ActivityDTO> ActivityDtos = new List<ActivityDTO>(); // create list Object of Activity DTo

                List<DAL.Activity> activities = dbEnt.Activities.Where(x => x.RelatedTo == RelatedToEnum.Account.ToString() && x.RelatedToID == guid).ToList(); // Get List Of Activities from DB
                // apply iteration on activities
                foreach (DAL.Activity item in activities.CollectionNotNull())
                {
                    ActivityDtos.Add(objMapper.GetActivityDTO(item)); // add in a list object
                }
                return ActivityDtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountActivities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ActivityManager.GetAccountActivities", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the account activities.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>List&lt;ActivityDTO&gt;.</returns>
        public List<ActivityDTO> GetCasesActivities(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAccountActivities", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.ActivityManager.GetAccountActivities", "");
                List<ActivityDTO> ActivityDtos = new List<ActivityDTO>(); // create list Object of Activity DTo

                List<DAL.Activity> activities = dbEnt.Activities.Where(x => x.RelatedTo == RelatedToEnum.Cases.ToString() && x.RelatedToID == guid).ToList(); // Get List Of Activities from DB
                // apply iteration on activities
                foreach (DAL.Activity item in activities.CollectionNotNull())
                {
                    ActivityDtos.Add(objMapper.GetActivityDTO(item)); // add in a list object
                }
                return ActivityDtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountActivities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ActivityManager.GetAccountActivities", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }


        /// <summary>
        ///   Return a string of data for datatables to render on front end
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns>String of data</returns>
        public List<ActivityDTO> GetActivitesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetActiviesList", EventType.Log, EventColor.yellow, "Get List of Activites Based on Datatable Query", "TICRM.BuisnessLayer.ActivityManager.GetActiviesList", "");

                var Activies = new List<ActivityDTO>();
                var ActivitiesDto = new List<Activity>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    ActivitiesDto = dbEnt.Activities.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.RelatedTo.ToLower().Contains(sSearch)
                    || a.Team.Name.ToLower().Contains(sSearch)
                    || a.User.Name.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    ActivitiesDto = dbEnt.Activities.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Activity item in ActivitiesDto.CollectionNotNull())
                {
                    Activies.Add(objMapper.GetActivityDTO(item)); // add in a list object
                }

                return Activies;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetActiviesList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ActivityManager.GetActiviesList", "");

                throw ex;
            }

        }

        /// <summary>
        /// Count all activites
        /// </summary>
        /// <returns>No of total activites</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Activities", "TICRM.BuisnessLayer.ActivityManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Activities.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ActivityManager.GetTotalCount", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
    }
}
