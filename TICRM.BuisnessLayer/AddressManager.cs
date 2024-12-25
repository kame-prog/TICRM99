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
    ||  Class [AddressManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             Addresses are get from the database and mapped with the
    ||             corrosponding DTO object to send it back to the controller]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class AddressManager : BaseManager
    {
        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets all the addresses and returns it in the list
        /// </summary>
        /// <returns></returns>

        public List<AddressDto> GetAllAddresses(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetAddresses", EventType.Log, EventColor.yellow, "Successfully Enter in GetAddresses", "TICRM.BusinessLayer.AddressManager", "");

                List<AddressDto> addressDto = new List<AddressDto>(); // create list Object of Address DTO

                List<Address> address = dbEnt.sp_Addresses_Get(CurrentUserId, UserRole, UserCompany).ToList(); // Get List Of Address from DB
                // apply iteration on getting addresses
                foreach (Address item in address.CollectionNotNull())
                {
                    addressDto.Add(objMapper.GetAddressDTO(item)); // add in a list object
                }
                return addressDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAddresses", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AddressManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets a specific address on Id and returns it 
        /// </summary>
        /// <returns></returns>

        public AddressDto GetAddress(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAddress", EventType.Log, EventColor.yellow, "Successfully Enter in GetActivity to Get Data on id", "TICRM.BusinessLayer.AddressManager", "");
                return objMapper.GetAddressDTO(dbEnt.Addresses.Find(guid)); // get Address on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAddress", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AddressManager", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method save a specific address on edit, create and delete actions 
        /// </summary>
        /// <returns></returns>
        public bool SaveAddress(AddressDto add,string CurrentUserId, string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveActivity", EventType.Log, EventColor.yellow, "Successfully Enter in SaveActivity", "TICRM.BusinessLayer.ActivityManager", "");

                Address address; // create a new object
                
                if (isEditMode) // check if is is edit mode is true
                {
                    address = objMapper.GetAddress(add); // pass parameter object to activity object
                    Address dbData = dbEnt.Addresses.FirstOrDefault(x => x.AddressId == address.AddressId); // get data from database and pass in new activity class object
                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveAddress", EventType.Log, EventColor.yellow, "For Delete Successfully Enter in SaveAddress", "TICRM.BusinessLayer.AddressManger", "");
                            Address AddressDelete = dbEnt.Addresses.FirstOrDefault(x => x.AddressId == address.AddressId);
                            AddressDelete.IsDeleted = true;
                        }
                        else
                        {
                            InsertEventLog("SaveAddress", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.AddressManger.SaveAddress", "");
                            dbData.AddressId = address.AddressId;
                            dbData.Street1 = address.Street1;
                            dbData.Street2 = address.Street2;
                            dbData.City = address.City;
                            dbData.State = address.State;
                            dbData.Zip = address.Zip;
                            dbData.Country = address.Country;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            //dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveAddress", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + add.AddressId, "TICRM.BuisnessLayer.AddressManger.SaveAddress", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }
                }
                else
                {
                    InsertEventLog("SaveAddress", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.AddressManger.SaveAddress", "");

                    address = objMapper.GetAddress(add);  // pass parameter address object to address object
                    address.AddressId= Guid.NewGuid();
                    address.CreatedDate= DateTime.Now;
                    address.CreatedBy= CurrentUserId;
                    address.Company=Guid.Parse(UserCompanyID);
                    dbEnt.Addresses.Add(address); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveAddress", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace + " /n " + ex.InnerException, "TICRMTICRM.BuisnessLayer.AddressManger.SaveAddress", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;

        }


    }
}
