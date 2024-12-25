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
    ||  Class [EventMonitorManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting Event Monitors in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class EventMonitorManager : BaseManager
    {
        /// <summary>
        /// Gets the event monitor list.
        /// </summary>
        /// <returns>List&lt;EventMonitorDTO&gt;.</returns>
        public List<EventMonitorDTO> GetEventMonitorList()
        {
            try
            {
                InsertEventLog("GetEventMonitorList", EventType.Log, EventColor.yellow, "to get list of event Monitor ", "TICRM.BuisnessLayer.EventMonitorManager.GetEventMonitorList", "");
                List<EventMonitorDTO> eventMonitorDTOs = new List<EventMonitorDTO>();// create strongly type list Object of EventMonitor DTO

                List<EventMonitor> eventMonitors = dbEnt.EventMonitors.ToList(); // Get List Of EventMonitors from DB
                // apply iteration on workFlowMappings
                foreach (EventMonitor item in eventMonitors.CollectionNotNull())
                {
                    eventMonitorDTOs.Add(objMapper.GetEventMonitorDTO(item)); // add in a list object
                }
                return eventMonitorDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventMonitorList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventMonitorManager.GetEventMonitorList", "");
                throw;
            }
        }

        /// <summary>
        /// Gets the event Monitor list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;EventLogDTO&gt;.</returns>
        public List<EventMonitorDTO> GetEventMonitorList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetEventMoniterList", EventType.Log, EventColor.yellow, "Get List of Event Moniter Based on Datatable Query", "TICRM.BuisnessLayer.EventMoniterManager.GetEventMoniterList", "");

                var Events = new List<EventMonitorDTO>();
                var eventLogs = new List<EventMonitor>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    eventLogs = dbEnt.EventMonitors.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.Message.ToLower().Contains(sSearch)
                    || a.Color.ToLower().Contains(sSearch)
                    || a.IPAddress.ToLower().Contains(sSearch)
                    || a.CreatedBy.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    eventLogs = dbEnt.EventMonitors.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (EventMonitor item in eventLogs.CollectionNotNull())
                {
                    Events.Add(objMapper.GetEventMonitorDTO(item)); // add in a list object
                }

                return Events;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventMoniterList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventMoniterManager.GetEventMoniterList", "");

                throw ex;
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
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Event Monitor", "TICRM.BuisnessLayer.EventMonitorManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.EventMonitors.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventMonitorManager.GetTotalCount", "");
                throw ex;
            }
        }

        /// <summary>
        /// Saves the event monitor.
        /// </summary>
        /// <param name="eventMonitorDTO">The event monitor dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveEventMonitor(EventMonitorDTO eventMonitorDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveEventMonitor", EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.EventMonitorManager.SaveEventMonitor", CurrentUserId);

                EventMonitor eventMonitor; // create a new object
                eventMonitor = objMapper.GetEventMonitor(eventMonitorDTO); // pass parameter object to eventMonitor object
                if (isEditMode) // check if is is edit mode is true
                {
                    EventMonitor dbData = dbEnt.EventMonitors.FirstOrDefault(x => x.EventMonitorId == eventMonitor.EventMonitorId); // get data from database and pass in new EventMonitor class object

                    if (dbData != null) // check if data is null
                    {

                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveEventMonitor", EventType.Log, EventColor.yellow, "to delete event monitor on id="+eventMonitor.EventMonitorId+ " ", "TICRM.BuisnessLayer.EventMonitorManager.SaveEventMonitor", CurrentUserId);

                            dbEnt.EventMonitors.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveEventMonitor", EventType.Log, EventColor.yellow, "to update event monitor on id="+eventMonitor.EventMonitorId+ " ", "TICRM.BuisnessLayer.EventMonitorManager.SaveEventMonitor", CurrentUserId);
                            dbData.Name = eventMonitor.Name;
                            dbData.Type = eventMonitor.Type;
                            dbData.Status = eventMonitor.Status;
                            dbData.Message = eventMonitor.Message;
                            dbData.Color = eventMonitor.Color;
                            dbData.IPAddress = eventMonitor.IPAddress;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveEventMonitor", EventType.Log, EventColor.yellow, "to event monitor on id=" + eventMonitor.EventMonitorId + " return null data ", "TICRM.BuisnessLayer.EventMonitorManager.SaveEventMonitor", CurrentUserId);
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveEventMonitor", EventType.Log, EventColor.yellow, "create new record of activity monitor event ", "TICRM.BuisnessLayer.EventMonitorManager.SaveEventMonitor", CurrentUserId);
                    eventMonitor.EventMonitorId = Guid.NewGuid();
                    eventMonitor.CreatedBy = CurrentUserId;
                    eventMonitor.CreatedDate = DateTime.Now;
                    dbEnt.EventMonitors.Add(eventMonitor); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveEventMonitor", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventMonitorManager.SaveEventMonitor", CurrentUserId);
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the event monitor on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>EventMonitorDTO.</returns>
        public EventMonitorDTO GetEventMonitorOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetEventMonitorOnId", EventType.Log, EventColor.yellow, "to get event monitor on id=" + guid + " ", "TICRM.BuisnessLayer.EventMonitorManager.GetEventMonitorOnId", "");
                return objMapper.GetEventMonitorDTO(dbEnt.EventMonitors.FirstOrDefault(x => x.EventMonitorId == guid)); // Get EventMonitor On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventMonitorOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventMonitorManager.GetEventMonitorOnId", "");
                throw ex;
            }
        }
    }
}
