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
    ||  Class [ReadingUnitManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving reading unit. Getting a specific reading
    ||             unit on the basis of Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ReadingUnitManager : BaseManager
    {
        /// <summary>
        /// Code by AKhtar Zaman
        /// 20/7/2020
        /// the method gets all the reading units and returns it in the list
        /// </summary>
        /// <returns></returns>
        public List<ReadingUnitDto> GetReadingUnits()
        {
            try
            {
                InsertEventLog("GetReadingUnits", EventType.Log, EventColor.yellow, "Successfully Enter in GetReadingUnits", "TICRM.BusinessLayer.ReadingunitManager", "");

                List<ReadingUnitDto> readingDto = new List<ReadingUnitDto>(); // create list Object of Reading unit DTO

                List<ReadingUnit> reading = dbEnt.ReadingUnits.Include(r => r.ReadingType).Where(a => a.IsDeleted != true).ToList(); // Get List Of reading unit from DB
                // apply iteration on getting ReadingTypes
                foreach (ReadingUnit item in reading.CollectionNotNull())
                {
                    readingDto.Add(objMapper.GetReadingUnitDTO(item)); // add in a list object
                }
                return readingDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingUnits", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingunitManager" +
                    "", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        /// <summary>
        /// Code by AKhtar Zaman
        /// 20/7/2020
        /// the method gets all the reading units and returns it in the list
        /// </summary>
        /// <returns></returns>
        public ReadingUnitDto GetReadingUnit(Guid? guid)
        {
            try
            {
                InsertEventLog("GetReadingUnit", EventType.Log, EventColor.yellow, "Successfully Enter in GetReadingUnit to Get Data on id", "TICRM.BusinessLayer.ReadingUnitManager", "");
                return objMapper.GetReadingUnitDTO(dbEnt.ReadingUnits.Find(guid)); // get Reading on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingUnit", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingUnitManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Saves the reading unit.
        /// </summary>
        /// <param name="add">The add.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveReadingUnit(ReadingUnitDto add, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveReadingUnit", EventType.Log, EventColor.yellow, "Successfully Enter in SaveReadingUnit", "TICRM.BusinessLayer.ReadingUnitManager.SaveReadingUnit", "");

                ReadingUnit readignUnit; // create a new object
                readignUnit = objMapper.GetReadingUnit(add); // pass parameter object to ReadingUnit object
                if (isEditMode) // check if is is edit mode is true
                {
                    ReadingUnit dbData = dbEnt.ReadingUnits.FirstOrDefault(x => x.ReadingUnitId == readignUnit.ReadingUnitId); // get data from database and pass in new ReadingUnit class object

                    if (dbData != null) // check if data is null
                    {

                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveReadingUnit", EventType.Log, EventColor.yellow, "For Delete Successfully Enter in SaveReadingUnit", "TICRM.BusinessLayer.ReadingUnitManager.SaveReadingUnit", "");

                            ReadingUnit ReadingUnitDel = dbEnt.ReadingUnits.FirstOrDefault(x => x.ReadingUnitId == readignUnit.ReadingUnitId);
                            ReadingUnitDel.IsDeleted = true;
                            dbEnt.Entry(ReadingUnitDel).State = EntityState.Modified;
                        }
                        else
                        {
                            InsertEventLog("SaveReadingUnit", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.ReadingUnitManager.SaveReadingUnit", "");
                            ReadingUnit ReadingUnitEdit = dbEnt.ReadingUnits.FirstOrDefault(x => x.ReadingUnitId == readignUnit.ReadingUnitId);
                            ReadingUnitEdit.Name = readignUnit.Name;
                            ReadingUnitEdit.Type = readignUnit.Type;
                            dbEnt.Entry(ReadingUnitEdit).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveReadingUnit", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + add.ReadingUnitId, "TICRM.BuisnessLayer.ReadingUnitManager.SaveReadingUnit", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveReadingUnit", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.ReadingUnitManager.SaveReadingType", "");

                    readignUnit = objMapper.GetReadingUnit(add);  // pass parameter readingunitdto object to readingunit object
                    readignUnit.ReadingUnitId = Guid.NewGuid();
                    dbEnt.ReadingUnits.Add(readignUnit); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveReadingUnit", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRMTICRM.BuisnessLayer.ReadingUnitManager.SaveReadingUnit", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;

        }



    }
}
