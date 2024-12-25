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
    ||  Class [EventLogManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting Event Logs in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class EventLogManager : BaseManager
    {
        /// <summary>
        /// Gets the event log list.
        /// </summary>
        /// <returns>List&lt;EventLogDTO&gt;.</returns>
        public List<EventLogDTO> GetEventLogList()
        {
            try
            {
                InsertEventLog("GetEventLogList", EventType.Log, EventColor.yellow, "to get list of event logs ", "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");
                List<EventLogDTO> eventLogDTOs = new List<EventLogDTO>();// create strongly type list Object of EventLog DTO

                List<EventLog> eventLogs = dbEnt.EventLogs.Take(10).ToList(); // Get List Of EventLogs from DB
                // apply iteration on workFlowMappings
                foreach (EventLog item in eventLogs.CollectionNotNull())
                {
                    eventLogDTOs.Add(objMapper.GetEventLogDTO(item)); // add in a list object
                }
                return eventLogDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventLogList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");
                throw;
            }
        }

        /// <summary>
        /// Saves the event log.
        /// </summary>
        /// <param name="eventLogDTO">The event log dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveEventLog(EventLogDTO eventLogDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "enter ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");

                EventLog eventLog; // create a new object
                eventLog = objMapper.GetEventLog(eventLogDTO); // pass parameter object to eventLog object
                if (isEditMode) // check if is is edit mode is true
                {
                    EventLog dbData = dbEnt.EventLogs.FirstOrDefault(x => x.EventLogId == eventLog.EventLogId); // get data from database and pass in new EventLog class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "enter in Delete mode to delete event log ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                            dbEnt.EventLogs.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "enter in edit mode to update Data event log ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                            dbData.Name = eventLog.Name;
                            dbData.Type = eventLog.Type;
                            dbData.Status = eventLog.Status;
                            dbData.Message = eventLog.Message;
                            dbData.Color = eventLog.Color;
                            dbData.IPAddress = eventLog.IPAddress;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "enter in edit mode data is null ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "for edit and delete data is saved in DB ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "Enter In Create new record ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                    eventLog.EventLogId = Guid.NewGuid();
                    eventLog.CreatedBy = CurrentUserId;
                    eventLog.CreatedDate = DateTime.Now;
                    dbEnt.EventLogs.Add(eventLog); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SaveEventLog", EventType.Log, EventColor.yellow, "New Record is saved ", "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveEventLog", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.SaveEventLog", "");
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the event log on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>EventLogDTO.</returns>
        public EventLogDTO GetEventLogOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetEventLogOnId", EventType.Log, EventColor.yellow, "get event log on id ", "TICRM.BuisnessLayer.EventLogManager.GetEventLogOnId", "");
                return objMapper.GetEventLogDTO(dbEnt.EventLogs.FirstOrDefault(x => x.EventLogId == guid)); // Get EventLog On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventLogOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.GetEventLogOnId", "");
                throw ex;
            }
        }

        /// <summary>
        /// Gets the total count for pagination.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Event Log", "TICRM.BuisnessLayer.EventLogManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.EventLogs.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.GetTotalCount", "");
                throw ex;
            }
        }

        /// <summary>
        /// Gets the event log list for pagination.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public List<EventLogDTO> GetEventLogList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetEventLogList", EventType.Log, EventColor.yellow, "Get List of Event Log Based on Datatable Query", "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");

                var Events = new List<EventLogDTO>();
                var eventLogs = new List<EventLog>();

                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
               
                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    eventLogs = dbEnt.EventLogs.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.Message.ToLower().Contains(sSearch)
                    || a.Color.ToLower().Contains(sSearch)
                    || a.IPAddress.ToLower().Contains(sSearch)
                    || a.CreatedBy.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    eventLogs = dbEnt.EventLogs.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (EventLog item in eventLogs.CollectionNotNull())
                {
                    Events.Add(objMapper.GetEventLogDTO(item)); // add in a list object
                }

                return Events;
               

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEventLogList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EventLogManager.GetEventLogList", "");

                throw ex;
            }
           
        }

    }
}
