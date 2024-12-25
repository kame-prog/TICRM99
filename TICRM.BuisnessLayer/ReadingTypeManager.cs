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
    ||  Class [ReadingTypeManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving reading type. Getting a specific reading
    ||             type on the basis of Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ReadingTypeManager : BaseManager
    {
        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets all the addresses and returns it in the list
        /// </summary>
        /// <returns></returns>
        public List<ReadingTypeDto> GetReadingTypes()
        {
            try
            {
                InsertEventLog("GetReadingTypes", EventType.Log, EventColor.yellow, "Successfully Enter in GetReadings", "TICRM.BusinessLayer.ReadingTypeManager", "");

                List<ReadingTypeDto> readingDto = new List<ReadingTypeDto>(); // create list Object of Reading Types DTO

                List<ReadingType> reading = dbEnt.ReadingTypes.Where(a => a.IsDeleted != true).ToList(); // Get List Of REadingTypes from DB
                // apply iteration on getting ReadingTypes
                foreach (ReadingType item in reading.CollectionNotNull())
                {
                    readingDto.Add(objMapper.GetReadingTypeDTO(item)); // add in a list object
                }
                return readingDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingTypess", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingTypeManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets a specific ReadingTyoe and returns it as an object
        /// </summary>
        /// <returns></returns>
        public ReadingTypeDto GetReadingType(Guid? guid)
        {
            try
            {
                InsertEventLog("GetReadingType", EventType.Log, EventColor.yellow, "Successfully Enter in ReadingTypeManager to Get Data on id", "TICRM.BusinessLayer.ReadingTypeManager", "");
                return objMapper.GetReadingTypeDTO(dbEnt.ReadingTypes.Find(guid)); // get Reading on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetReadingType", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ReadingTypeManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method save a specific ReadingType on edit, create and delete actions 
        /// </summary>
        /// <returns></returns>
        public bool SaveReadingType(ReadingTypeDto add, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveReadingType", EventType.Log, EventColor.yellow, "Successfully Enter in SaveReadingType", "TICRM.BusinessLayer.ReadingTypeManager.SaveReadingType", "");

                ReadingType readignType; // create a new object
                readignType = objMapper.GetReadingType(add); // pass parameter object to radingType object
                if (isEditMode) // check if is is edit mode is true
                {


                    ReadingType dbData = dbEnt.ReadingTypes.FirstOrDefault(x => x.ReadingTypeId == readignType.ReadingTypeId); // get data from database and pass in new reading class object

                    if (dbData != null) // check if data is null
                    {

                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveReadingType", EventType.Log, EventColor.yellow, "For Delete Successfully Enter in SaveReadingType", "TICRM.BusinessLayer.ReadingTypeManager.SaveReadingType", "");
                            //dbEnt.ReadingTypes.Remove(dbData); // remove object in database
                            ReadingType readingDelete = dbEnt.ReadingTypes.FirstOrDefault(x => x.ReadingTypeId == readignType.ReadingTypeId);
                            readingDelete.IsDeleted = true;
                            dbEnt.Entry(readingDelete).State = EntityState.Modified;
                        }
                        else
                        {
                            InsertEventLog("SaveReadingType", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.ReadingTypeManager.SaveReadingType", "");                          
                            dbData.Name= readignType.Name;   
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveReadingType", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + add.ReadingTypeId, "TICRM.BuisnessLayer.ReadingTypeManager.SaveReadingType", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveReadingType", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.ReadingTypeManager.SaveReadingType", "");

                    readignType = objMapper.GetReadingType(add);  // pass parameter readigntypedto object to readingtype object
                    readignType.ReadingTypeId = Guid.NewGuid();
                    dbEnt.ReadingTypes.Add(readignType); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveReadingType", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRMTICRM.BuisnessLayer.ReadingTypeManager.SaveReadingType", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;

        }

    }
}
