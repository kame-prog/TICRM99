using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DTOs;
using TICRM.DAL;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [AlertManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [|This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             Alerts are get from the database and mapped with the
    ||             corrosponding DTO object to send it back to the controller]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class AlertManager : BaseManager
    {
        public AlertManager()
        {
            //Urgencies = GetUrgencies();
        }
        /// <summary>
        /// Gets or sets the urgencies.
        /// </summary>
        /// <value>The urgencies.</value>
        
        //public List<UrgencyDto> Urgencies { get; set; }


        /// <summary>
        /// Get Urgencies Domain Transfer Objects
        /// </summary>
        /// <returns></returns>
       
        //public List<UrgencyDto> GetUrgencies()
        //{
        //    try
        //    {
        //        InsertEventLog("GetUrgencies", EventType.Log, EventColor.yellow, "Get activity Template on id", "TICRM.BuisnessLayer.AlertManager.GetUrgencies", "");
        //        var urgencyDtos = new List<UrgencyDto>();

        //        foreach (var item in dbEnt.Urgencies.CollectionNotNull())
        //        {
        //            urgencyDtos.Add(objMapper.GetUrgencyDTO(item));
        //        }
        //        return urgencyDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetUrgencies", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AlertManager.GetUrgencies", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the alert counts.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetAlertCounts()
        {

            try
            {
                InsertEventLog("GetAlertCounts", EventType.Log, EventColor.yellow, "to get total no of Alerts count", "TICRM.BuisnessLayer.AlertManager.GetAlertCounts", "");
                return dbEnt.Alerts.Count();

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAlertCounts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AlertManager.GetAlertCounts", "");
                throw;
            }
            
        }
        /// <summary>
        /// Gets the alert.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>AlertDto.</returns>
        public AlertDto GetAlert(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAlert", EventType.Log, EventColor.yellow, "Get Alert on id=" + guid + " ", "TICRM.BuisnessLayer.AlertManager.GetAlert", "");
                return objMapper.GetAlertDTO(dbEnt.Alerts.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetAlert", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AlertManager.GetAlert", "");
                throw ex;
            }


        }
        /// <summary>
        /// Get alert list 
        /// </summary>
        /// <returns></returns>
        public List<AlertDto> GetAlerts()
        {
            try
            {
                InsertEventLog("GetAlerts", EventType.Log, EventColor.yellow, "Get Alert dto list", "TICRM.BuisnessLayer.AlertManager.GetAlerts", "");
                var alertDtos = new List<AlertDto>();
                var alerts = dbEnt.Alerts.Include(a => a.Status).Include(a => a.Team).Include(a => a.Urgency).Include(a => a.User).Where(a => a.IsDeleted != true).ToList();
                foreach (var item in alerts.CollectionNotNull())
                {
                    alertDtos.Add(objMapper.GetAlertDTO(item));
                }
                return alertDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAlerts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AlertManager.GetAlerts", "");

                throw;
            }


        }
        /// <summary>
        /// save and edit AlertDto 
        /// </summary>
        /// <param name="alr"></param>
        /// <returns></returns>
        public bool SaveAlert(AlertDto alr, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveAlert",  EventType.Log, EventColor.yellow, "Enter", "TICRM.BuisnessLayer.AlertManager.SaveAlert", "");

                Alert alert;
                if (isEditMode)
                {
                    InsertEventLog("SaveAlert",  EventType.Log, EventColor.yellow, "Entrer In Edit Mode", "TICRM.BuisnessLayer.AlertManager.SaveAlert", "");
                    alert = objMapper.GetAlert(alr);
                    if (isDeleteMode)
                    {
                        InsertEventLog("SaveAlert", EventType.Log, EventColor.yellow, "Entrer In Delete Mode", "TICRM.BuisnessLayer.AlertManager.SaveAlert", "");

                        Alert a = dbEnt.Alerts.FirstOrDefault(x => x.AlertId == alert.AlertId);
                         a.IsDeleted = true;
                        dbEnt.Entry(a).State = EntityState.Modified;
                    }
                    else
                    {
                        Alert dbData = dbEnt.Alerts.FirstOrDefault(x => x.AlertId == alert.AlertId); // get data from database 
                        dbData.AlertId = alert.AlertId;
                        dbData.Title = alert.Title; ;
                        dbData.UrgencyId = alert.UrgencyId;
                        dbData.Description = alert.Description;
                        dbData.StatusId = alert.StatusId;
                        dbData.AssignedUser = alert.AssignedUser;
                        dbData.AssignedTeam = alert.AssignedTeam;
                        dbData.UpdatedDate = DateTime.Now;
                        dbData.UpdatedBy = CurrentUserId;
                    }
                }
                else
                {
                    InsertEventLog("SaveAlert",  EventType.Log, EventColor.yellow, "Enter In Create New Record Alert","TICRM.BuisnessLayer.AlertManager.SaveAlert", "");
                    alert = objMapper.GetAlert(alr);
                    alert.CreatedBy = CurrentUserId;
                    alert.CreatedDate = DateTime.Now;
                    alert.AlertId = Guid.NewGuid();
                    dbEnt.Alerts.Add(alert);
                }

                dbEnt.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveAlert", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AlertManager.SaveAlert", "");
                throw ex;
            }
        }
    }
}
