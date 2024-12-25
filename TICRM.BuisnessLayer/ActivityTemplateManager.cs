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
    ||  Class [ActivityTemplateManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             activity temmplates are get from the database and mapped with the
    ||             corrosponding DTO object to send it back to the controller]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ActivityTemplateManager : BaseManager
    {
        /// <summary>
        /// Gets the activity template dt os.
        /// </summary>
        /// <returns>List&lt;ActivityTemplateDTO&gt;.</returns>
        public List<ActivityTemplateDTO> GetActivityTemplateDTOs()
        {
            try
            {
                InsertEventLog("GetActivityTemplateDTOs", EventType.Log, EventColor.yellow, "Successfully Enter in to get list of actvity Temaplate dtos", "TICRM.BusinessLayer.ActivityTemplateManager.GetActivityTemplateDTOs", "");
                List<ActivityTemplateDTO> ActivityTemplateDtos = new List<ActivityTemplateDTO>(); // create new object as Collection Of ActivityTemplateDTO

                List<ActivityTemplate> ActivityTemplates = dbEnt.ActivityTemplates.ToList(); // Get Collection of ActivtiyTemplate from DB
                // apply iteration on activities template
                foreach (ActivityTemplate item in ActivityTemplates)
                {
                    ActivityTemplateDtos.Add(objMapper.GetActivityTemplateDTO(item)); // add item in object
                }
                return ActivityTemplateDtos; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountActivities", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ActivityTemplateManager.GetActivityTemplateDTOs", "");
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
        public List<ActivityTemplateDTO> GetActivityTemplatesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetActiviTemplatesList", EventType.Log, EventColor.yellow, "Get List of Activity Templates Based on Datatable Query", "TICRM.BuisnessLayer.ActivityManager.GetActiviTemplatesList", "");

                var ActivityTemplate = new List<ActivityTemplateDTO>();
                var ActivittTemplateDto = new List<ActivityTemplate>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    ActivittTemplateDto = dbEnt.ActivityTemplates.Where(a => a.ActivityTemplateType.ToLower().Contains(sSearch)
                    || a.PropertyName.ToLower().Contains(sSearch)
                    || a.PropertyValue.ToLower().Contains(sSearch)
                    || a.PropertyType.ToLower().Contains(sSearch)
                    || a.CreatedBy.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                {
                    ActivittTemplateDto = dbEnt.ActivityTemplates.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList(); 
                }
                foreach (ActivityTemplate item in ActivittTemplateDto.CollectionNotNull())
                {
                    ActivityTemplate.Add(objMapper.GetActivityTemplateDTO(item)); // add in a list object
                }


                return ActivityTemplate;

               

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetActiviTemplatesList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ActivityManager.GetActiviTemplatesList", "");

                throw ex;
            }

        }

        /// <summary>
        /// Count all activity Templates
        /// </summary>
        /// <returns>No of total activites</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Activities", "TICRM.BuisnessLayer.ActivityManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.ActivityTemplates.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ActivityManager.GetTotalCount", "");
                throw ex;
            }
        }

        /// <summary>
        /// Saveits the activity template.
        /// </summary>
        /// <param name="activityTemplateDTO">The activity template dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveitActivityTemplate(ActivityTemplateDTO activityTemplateDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveitActivityTemplate", EventType.Log, EventColor.yellow, "Successfully Enter", "TICRM.BusinessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                ActivityTemplate activityTemplate; // initilize a new instance
                activityTemplate = objMapper.GetActivityTemplate(activityTemplateDTO); // Convert activity template DTO object to AcitvityTemplate object
                if (isEditMode) // check if is is edit mode is true
                {


                    ActivityTemplate dbData = dbEnt.ActivityTemplates.FirstOrDefault(x => x.ActivityTemplateId == activityTemplate.ActivityTemplateId); // get data from database and pass in new activity tempalte object

                    if (dbData != null) // check if data is null
                    {

                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveitActivityTemplate", EventType.Log, EventColor.yellow, "For Delete Successfully Enter to Delete Data on id", "TICRM.BuisnessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                            //activity.IsDeleted = true;
                            dbEnt.ActivityTemplates.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveitActivityTemplate", EventType.Log, EventColor.yellow, "For Edit Successfully Enter to edit data", "TICRM.BuisnessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                            // update db recoder
                            dbData.ActivityTemplateId = activityTemplate.ActivityTemplateId;
                            dbData.ActivityTemplateType = activityTemplate.ActivityTemplateType;
                            dbData.PropertyName = activityTemplate.PropertyName;
                            dbData.PropertyType = activityTemplate.PropertyType;
                            dbData.PropertyValue = activityTemplate.PropertyValue;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveitActivityTemplate", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + activityTemplate.ActivityTemplateId + " ", "TICRM.BuisnessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SaveitActivityTemplate", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is Saved Successfully on id " + activityTemplate.ActivityTemplateId + "  ", "TICRM.BuisnessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                        return true;
                    }

                }
                else
                {
                    // create new record in DB
                    activityTemplate.ActivityTemplateId = Guid.NewGuid(); // create new guid id
                    activityTemplate.CreatedBy = CurrentUserId; // pass current UserId
                    activityTemplate.CreatedDate = DateTime.Now;
                    dbEnt.ActivityTemplates.Add(activityTemplate); // add in instance in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SaveitActivityTemplate", EventType.Log, EventColor.yellow, "New Record of Activity Template Is saved.", "TICRM.BuisnessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                        return true; // return true if data saved.
                    }
                }



            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveitActivityTemplate", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ActivityTemplateManager.SaveitActivityTemplate", "");
                throw ex;
            }
            return false;

        }

        /// <summary>
        /// Gets the activity template on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ActivityTemplateDTO.</returns>
        public ActivityTemplateDTO GetActivityTemplateOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetActivityTemplateOnId", EventType.Log, EventColor.yellow, "Get activity Template on id", "TICRM.BuisnessLayer.ActivityTemplateManager.GetActivityTemplateOnId", "");
                return objMapper.GetActivityTemplateDTO(dbEnt.ActivityTemplates.Find(guid)); // get ActivityTemplates on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetActivityTemplateOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ActivityTemplateManager.GetActivityTemplateOnId", "");
                throw ex;
            }


        }


    }
}
