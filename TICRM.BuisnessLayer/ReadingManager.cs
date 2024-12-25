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
    ||  Class [ReadingManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving reading . Getting a specific reading
    ||             on the basis of Id, getting reading units and reading types]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ReadingManager : BaseManager
    {
        public ReadingManager()
        {
            ReadingTypes = GetReadingTypes();
            ReadingUnits = GetReadingUnits();
        }

        public List<ReadingTypeDto> ReadingTypes { get; set; }
        public List<ReadingUnitDto> ReadingUnits { get; set; }

        /// <summary>
        /// Get Reading Types
        /// </summary>
        /// <returns></returns>
        public List<ReadingTypeDto> GetReadingTypes()
        {
            try
            {
                InsertEventLog("GetReadingTypes", EventType.Log, EventColor.yellow, "Get list of ReadingTypeDto", "TICRM.BusinessLayer.ReadingManager.GetReadingTypes", "");
                List<ReadingTypeDto> readingTypeDtos = new List<ReadingTypeDto>();

                foreach (ReadingType item in dbEnt.ReadingTypes)
                {
                    readingTypeDtos.Add(objMapper.GetReadingTypeDTO(item));
                }
                return readingTypeDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingManager.GetReadingTypes", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the reading units.
        /// </summary>
        /// <returns>List&lt;ReadingUnitDto&gt;.</returns>
        public List<ReadingUnitDto> GetReadingUnits()
        {
            try
            {
                InsertEventLog("GetReadingUnits", EventType.Log, EventColor.yellow, "Get list of GetReadingUnits", "TICRM.BusinessLayer.ReadingManager.GetReadingUnits", "");
                List<ReadingUnitDto> readingUnitDtos = new List<ReadingUnitDto>();

                foreach (ReadingUnit item in dbEnt.ReadingUnits.CollectionNotNull())
                {
                    readingUnitDtos.Add(objMapper.GetReadingUnitDTO(item));
                }
                return readingUnitDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingUnits", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingManager.GetReadingUnits", "");

                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the reading.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ReadingDto.</returns>
        public ReadingDto GetReading(Guid? guid)
        {
            try
            {
                InsertEventLog("GetReading", EventType.Log, EventColor.yellow, "going to Get ReadingDto object", "TICRM.BusinessLayer.ReadingManager.GetReading", "");
                return objMapper.GetReadingDTO(dbEnt.Readings.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetReading", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingManager.GetReading", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// Get reading list 
        /// </summary>
        /// <returns></returns>
        public List<ReadingDto> GetReadings()
        {
            try
            {
                InsertEventLog("GetReadings", EventType.Log, EventColor.yellow, "going to Get ReadingDto list", "TICRM.BusinessLayer.ReadingManager.GetReadings", "");
                List<ReadingDto> readingDtos = new List<ReadingDto>();
                List<Reading> readings = dbEnt.Readings.Include(r => r.ReadingType).Include(r => r.ReadingUnit).Include(r => r.Status).Include(r => r.Team).Include(r => r.User).Where(a => a.IsDeleted != true).ToList();
                foreach (Reading item in readings)
                {
                    readingDtos.Add(objMapper.GetReadingDTO(item));
                }
                return readingDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadings", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingManager.GetReadings", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// save and edit ReadingDto 
        /// </summary>
        /// <param name="readng"></param>
        /// <returns></returns>
        public bool SaveReading(ReadingDto read, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveReading", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.ReadingManager.SaveReading", "");
                Reading reading;
                if (isEditMode)
                {

                    InsertEventLog("SaveReading", EventType.Log, EventColor.yellow, "going to edit Reading on id=" + read.ReadingId + "", "TICRM.BusinessLayer.ReadingManager.SaveReading", "");
                    reading = objMapper.GetReading(read);
                    if (isDeleteMode)
                    {
                        InsertEventLog("SaveReading", EventType.Log, EventColor.yellow, "going to Delete Reading on id=" + read.ReadingId + "", "TICRM.BusinessLayer.ReadingManager.SaveReading", "");
                        Reading readingForDelete = dbEnt.Readings.FirstOrDefault(x => x.ReadingId == reading.ReadingId);
                        readingForDelete.IsDeleted = true;
                        dbEnt.Entry(readingForDelete).State = EntityState.Modified;
                    }
                    else
                    {
                        Reading readingEdit = dbEnt.Readings.FirstOrDefault(x => x.ReadingId == read.ReadingId);
                        readingEdit.Value = reading.Value;
                        readingEdit.ReadingTypeId = reading.ReadingTypeId;
                        readingEdit.ReadingUnitId = reading.ReadingUnitId;
                        readingEdit.MarginOfErrorInPercent = reading.MarginOfErrorInPercent;
                        readingEdit.AssignedTeam = reading.AssignedTeam;
                        readingEdit.AssignedUser = reading.AssignedUser;
                        readingEdit.StatusId = reading.StatusId;
                        readingEdit.Description = reading.Description;
                        readingEdit.UpdatedBy = reading.UpdatedBy;
                        readingEdit.UpdatedDate = DateTime.Now;
                    }
                }
                else
                {
                    InsertEventLog("SaveReading", EventType.Log, EventColor.yellow, "Create New Record Reading", "TICRM.BusinessLayer.ReadingManager.SaveReading", "");
                    reading = objMapper.GetReading(read);
                    reading.ReadingId = Guid.NewGuid();
                    reading.CreatedDate = DateTime.Now;
                    dbEnt.Readings.Add(reading);
                }

                dbEnt.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveReading", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingManager.SaveReading", "");
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
        public List<ReadingDto> GetReadingsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetReadingsList", EventType.Log, EventColor.yellow, "Get List of Readings Based on Datatable Query", "TICRM.BuisnessLayer.ReadingManager.GetReadingsList", "");

                var readings = new List<Reading>();
                var readingDto = new List<ReadingDto>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                if (!string.IsNullOrEmpty(sSearch))
                {
                    readings = dbEnt.Readings.Where(a => a.Value.ToString().Contains(sSearch)
                    || a.MarginOfErrorInPercent.ToString().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.ReadingType.Name.ToString().ToLower().Contains(sSearch)
                    || a.ReadingUnit.Name.ToString().ToLower().Contains(sSearch)
                    || a.Status.Name.ToString().ToLower().Contains(sSearch)
                    || a.Team.Name.ToString().ToLower().Contains(sSearch)
                    || a.User.Name.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    readings = dbEnt.Readings.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Reading item in readings.CollectionNotNull())
                {
                    readingDto.Add(objMapper.GetReadingDTO(item)); // add in a list object
                }

                return readingDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingsList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ReadingManager.GetReadingsList", "");

                throw ex;
            }

        }

        /// <summary>
        /// Count all Readings
        /// </summary>
        /// <returns>No of total activites</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total reading of Devices", "TICRM.BuisnessLayer.ReadingManageer.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Opportunities.AsQueryable().Count();

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ReadingManageer.GetTotalCount", "");
                throw ex;
            }
        }

    }
}
