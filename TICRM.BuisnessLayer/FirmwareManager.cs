using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.DTOs;
using TICRM.DAL;
using TICRM.BuisnessLayer.Base;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [FirmwaresManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting Firmwares in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||                  [20/08/2020     Added GetFirmwaresList() and GetTotalCount()    Sikandar]
    ****************************************************************************************/
    public class FirmwaresManager : BaseManager
    {
        /// <summary>
        /// Creating deleteting or editing firmwares
        /// </summary>
        /// <param name="firmware"></param>
        /// <returns></returns>
        public bool SaveFirmware(FirmwareDto firmwareDto,bool Delete)
        {
            try
            {
                InsertEventLog("SaveFirmware", EventType.Log, EventColor.yellow, "Getting the data from controller and saving it in the db", "TICRM.BuisnessLayer.FirmwareManager.SaveFirmware", "");
                Firmware firmware;
                if (Delete)
                {
                    InsertEventLog("SaveFirmware", EventType.Log, EventColor.yellow, "Delete FirmWare from DB", "TICRM.BuisnessLayer.FirmwareManager.SaveFirmware", "");
                    //Delete FirmWare From DB
                    Firmware DeleteFirmware = dbEnt.Firmwares.FirstOrDefault(x => x.Id == firmwareDto.Id);
                    dbEnt.Firmwares.Remove(DeleteFirmware);
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    //Create firmware 
                    InsertEventLog("SaveFirmware", EventType.Log, EventColor.yellow, "For Create. Successfully Enter in SaveAccount", "TICRM.BusinessLayer.FirmwareManager","");
                    firmware = objMapper.GetFirmware(firmwareDto);
                    firmware.Date = DateTime.Now;
                    dbEnt.Firmwares.Add(firmware);
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveFirmware", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.FirmwareManager.SaveFirmware", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        
        /// <summary>
        /// Get the lists of all the firmwares
        /// </summary>
        /// <returns></returns>
        public List<FirmwareDto> GetFirmwares()
        {
            try
            {
                InsertEventLog("GetFirmwares", EventType.Log, EventColor.yellow, "Enter in to get list of Firmware", "TICRM.BuisnessLayer.FirmwareManager.GetFirmwares", "");
                var firmwareDto = new List<FirmwareDto>();
                var firmware = dbEnt.Firmwares.ToList();

                foreach (var item in firmware.CollectionNotNull())
                {
                    firmwareDto.Add(objMapper.GetFirmwareDto(item));
                }
                return firmwareDto;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetFirmware", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.FirmwareManager.GetFirmwares", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the firmwares list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;FirmwareDto&gt;.</returns>
        public List<FirmwareDto> GetFirmwaresList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetFirmwaresList", EventType.Log, EventColor.yellow, "to get list of event Notification ", "TICRM.BuisnessLayer.FirmwareManager.GetFirmwaresList", "");
                List<FirmwareDto> firmwareDTO = new List<FirmwareDto>();// create strongly type list Object of EventNotification DTO
                List<Firmware> firmware = new List<Firmware>(); // Get List Of EventNotifications from DB


                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    firmware = dbEnt.Firmwares.Where(a => a.version.ToLower().Contains(sSearch)
                    || a.description.ToLower().Contains(sSearch)
                    || a.Date.ToString().ToLower().Contains(sSearch)
                    || a.server.ToLower().Contains(sSearch)
                    ).OrderBy(x => x.Date).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    firmware = dbEnt.Firmwares.OrderBy(x => x.Date).Skip(iDisplayStart).Take(iDisplayLength).ToList();


                // apply iteration on eventNotifications
                foreach (Firmware item in firmware.CollectionNotNull())
                {
                    firmwareDTO.Add(objMapper.GetFirmwareDto(item)); // add in a list object
                }
                return firmwareDTO; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetFirmwaresList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.FirmwareManager.GetFirmwaresList", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

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
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Event Notifications", "TICRM.BuisnessLayer.FirmwareManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Firmwares.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.FirmwareManager.GetTotalCount", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }


        public FirmwareDto GetFirmware(int guid)
        {
            try
            {
                InsertEventLog("GetFirmware", EventType.Log, EventColor.yellow, "Successfully Enter in GetFirmware", "TICRM.BusinessLayer.FirmwareManager", "");
                return objMapper.GetFirmwareDto(dbEnt.Firmwares.Find(guid));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetFirmware", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.FirmwareManager", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
    }

}
