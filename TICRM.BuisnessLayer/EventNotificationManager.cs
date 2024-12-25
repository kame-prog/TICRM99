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
    /************************************************************************************
    ||  Class [EventNotificationManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting Event Notifications in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class EventNotificationManager : BaseManager
    {
        /// <summary>
        /// Gets the event notification list.
        /// </summary>
        /// <returns>List&lt;EventNotificationDTO&gt;.</returns>
        public List<EventNotificationDTO> GetEventNotificationList()
        {
            try
            {
                InsertEventLog("GetEventNotificationList", EventType.Log, EventColor.yellow, "to get list of event Notification ", "TICRM.BuisnessLayer.EventNotificationManager.GetEventNotificationList", "");
                List<EventNotificationDTO> eventNotificationDTO = new List<EventNotificationDTO>();// create strongly type list Object of EventNotification DTO

                List<EventNotification> eventNotifications = dbEnt.EventNotifications.ToList(); // Get List Of EventNotifications from DB
                // apply iteration on eventNotifications
                foreach (EventNotification item in eventNotifications.CollectionNotNull())
                {
                    eventNotificationDTO.Add(objMapper.GetEventNotificationDTO(item)); // add in a list object
                }
                return eventNotificationDTO; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventNotificationList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventNotificationManager.GetEventNotificationList", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the event notification list for datatable via ajax.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;EventNotificationDTO&gt;.</returns>
      public List<EventNotificationDTO> GetEventNotificationList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetEventNotificationList", EventType.Log, EventColor.yellow, "to get list of event Notification ", "TICRM.BuisnessLayer.EventNotificationManager.GetEventNotificationList", "");
                List<EventNotificationDTO> eventNotificationDTO = new List<EventNotificationDTO>();// create strongly type list Object of EventNotification DTO
                List<EventNotification> eventNotifications = new List<EventNotification>(); // Get List Of EventNotifications from DB


                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    eventNotifications = dbEnt.EventNotifications.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.Message.ToLower().Contains(sSearch)
                    || a.Color.ToLower().Contains(sSearch)
                    || a.IPAddress.ToLower().Contains(sSearch)
                    || a.CreatedBy.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    eventNotifications = dbEnt.EventNotifications.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();


                // apply iteration on eventNotifications
                foreach (EventNotification item in eventNotifications.CollectionNotNull())
                {
                    eventNotificationDTO.Add(objMapper.GetEventNotificationDTO(item)); // add in a list object
                }
                return eventNotificationDTO; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventNotificationList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventNotificationManager.GetEventNotificationList", "");
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
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Event Notifications", "TICRM.BuisnessLayer.EventNotificationsManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.EventNotifications.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventNotificationManager.GetTotalCount", "");
                throw ex;
            }
        }

        /// <summary>
        /// Saves the event notification.
        /// </summary>
        /// <param name="eventNotificationDTO">The event notification dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveEventNotification(EventNotificationDTO eventNotificationDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveEventNotification", EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.EventNotificationManager.SaveEventNotification", CurrentUserId);

                EventNotification eventNotification; // create a new object
                eventNotification = objMapper.GetEventNotification(eventNotificationDTO); // pass parameter object to Event Notification object
                if (isEditMode) // check if is is edit mode is true
                {
                    EventNotification dbData = dbEnt.EventNotifications.FirstOrDefault(x => x.EventNotificationId == eventNotification.EventNotificationId); // get data from database and pass in new EventNotifications class object

                    if (dbData != null) // check if data is null
                    {

                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveEventNotification", EventType.Log, EventColor.yellow, "to delete event monitor on id=" + eventNotification.EventNotificationId + " ", "TICRM.BuisnessLayer.EventNotificationManager.SaveEventNotification", CurrentUserId);

                            dbEnt.EventNotifications.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveEventMonitor", EventType.Log, EventColor.yellow, "to update event monitor on id=" + eventNotification.EventNotificationId + " ", "TICRM.BuisnessLayer.EventNotificationManager.SaveEventNotification", CurrentUserId);
                            dbData.Name = eventNotification.Name;
                            dbData.Type = eventNotification.Type;
                            dbData.Status = eventNotification.Status;
                            dbData.Message = eventNotification.Message;
                            dbData.Color = eventNotification.Color;
                            dbData.IPAddress = eventNotification.IPAddress;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveEventNotification", EventType.Log, EventColor.yellow, "to event monitor on id=" + eventNotification.EventNotificationId + " return null data ", "TICRM.BuisnessLayer.EventNotificationManager.SaveEventNotification", CurrentUserId);
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveEventNotification", EventType.Log, EventColor.yellow, "create new record of Event Nootification ", "TICRM.BuisnessLayer.EventNotificationManager.SaveEventNotification", CurrentUserId);
                    eventNotification.EventNotificationId = Guid.NewGuid();
                    eventNotification.CreatedBy = CurrentUserId;
                    eventNotification.CreatedDate = DateTime.Now;
                    dbEnt.EventNotifications.Add(eventNotification); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveEventNotification", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventNotificationManager.SaveEventNotification", CurrentUserId);
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the event notification on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>EventNotificationDTO.</returns>
        public EventNotificationDTO GetEventNotificationOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetEventNotificationOnId", EventType.Log, EventColor.yellow, "to get event monitor on id=" + guid + " ", "TICRM.BuisnessLayer.EventNotificationManager.GetEventNotificationOnId", "");
                return objMapper.GetEventNotificationDTO(dbEnt.EventNotifications.FirstOrDefault(x => x.EventNotificationId == guid)); // Get EventNotification On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventNotificationOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventNotificationManager.GetEventNotificationOnId", "");
                throw ex;
            }
        }
    }
}
