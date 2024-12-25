using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DTOs;
using TICRM.DAL;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [CustomerAssetManagers] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [|This class serves as a bridge between the front end and the database. 
    ||             Getting list for all the Customer Assets, Getting customer assets details for
                   a specific account or location, Getting customer asset types
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||                  [18/08/2020     Add GetCustomerAssetList() and GetTotalCount            Sikandar Mustafa]
    ****************************************************************************************/
    public class CustomerAssetManager : BaseManager
    {
       
        public CustomerAssetManager()
        {
            //CustomerAssetTypes = GetCustomerAssetTypes();
        }

        /// <summary>
        /// Gets or sets the customer asset types.
        /// </summary>
        /// <value>The customer asset types.</value>

        //public List<CustomerAssetTypeDto> CustomerAssetTypes { get; set; }


        /// <summary>
        /// Get Customer Asset type 
        /// </summary>
        /// <returns></returns>
        
        //public List<CustomerAssetTypeDto> GetCustomerAssetTypes()
        //{
        //    try
        //    {
        //        InsertEventLog("GetCustomerAssetTypes",  EventType.Log, EventColor.yellow, "Successfully Enter", "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssetTypes", "");
        //        InsertEventLog("GetCustomerAssetTypes", EventType.Log, EventColor.yellow, "Enter in to get list Of customer assets types", "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssetTypes", "");

        //        var customerAssetTypeDtos = new List<CustomerAssetTypeDto>();

        //        foreach (var item in dbEnt.CustomerAssetTypes.CollectionNotNull())
        //        {
        //            customerAssetTypeDtos.Add(objMapper.GetCustomerAssetTypeDTO(item));
        //        }
        //        return customerAssetTypeDtos;
                
        //    }
        //    catch (Exception ex)
        //    {

        //        InsertEventMonitor("GetCustomerAssetTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssetTypes", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the customer asset list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;CustomerAssetDto&gt;.</returns>
        public List<CustomerAssetDto> GetCustomerAssetList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetCustomerAssetList", EventType.Log, EventColor.yellow, "Get List of CustomerAssets Based on Datatable Query", "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssetList", "");

                var CustomerAssetDto = new List<CustomerAssetDto>();
                var CustomerAsset = new List<CustomerAsset>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                if (!string.IsNullOrEmpty(sSearch))
                {
                    CustomerAsset = dbEnt.CustomerAssets.Where(a => a.Title.ToLower().Contains(sSearch)
                    || a.Model.ToLower().Contains(sSearch)
                    || a.Description.ToLower().Contains(sSearch)
                    || a.Team.Name.ToLower().Contains(sSearch)
                    || a.User.Name.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    || a.Status.Name.ToLower().Contains(sSearch)
                    || a.Location.Name.ToLower().Contains(sSearch)
                    || a.Account.Name.ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    CustomerAsset = dbEnt.CustomerAssets.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (CustomerAsset item in CustomerAsset.CollectionNotNull())
                {
                    CustomerAssetDto.Add(objMapper.GetCustomerAssetDTO(item)); // add in a list object
                }

                return CustomerAssetDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCustomerAssetList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssetList", "");

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
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Customer Assets", "TICRM.BuisnessLayer.CustomerAssetManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.CustomerAssets.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetTotalCount", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the customer asset.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>CustomerAssetDto.</returns>
        public CustomerAssetDto GetCustomerAsset(Guid? guid)
        {
            try
            {
                InsertEventLog("GetCustomerAsset", EventType.Log, EventColor.yellow, "Enter in to get customer assets on id="+guid+ " ", "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAsset", "");
                return objMapper.GetCustomerAssetDTO(dbEnt.CustomerAssets.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetCustomerAsset", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAsset", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// Get account list 
        /// </summary>
        /// <returns></returns>
        public List<CustomerAssetDto> GetCustomerAssets(string CurrentUserId, string UserRole, string UserCompanyID)
        {
            try
            {
                InsertEventLog("GetCustomerAssets", EventType.Log, EventColor.yellow, "Enter in to get list of customer assets", "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssets", "");
                List<CustomerAssetDto> customerAssetDtos = new List<CustomerAssetDto>();
                //var customerAssets = dbEnt.CustomerAssets.Include(c => c.CustomerAssetType).Include(c => c.Status).Include(c => c.Team).Include(c => c.User).Where(a => a.IsDeleted != true && a.CreatedBy== CurrentUserId).ToList();
                List<CustomerAsset> customerAssets = dbEnt.sp_CustomerAssets_Get(CurrentUserId, UserRole, UserCompanyID).ToList();
                foreach (CustomerAsset item in customerAssets.CollectionNotNull())
                {
                    customerAssetDtos.Add(objMapper.GetCustomerAssetDTO(item));
                }
                return customerAssetDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCustomerAssets", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssets", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Gets the customer assets.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>List&lt;CustomerAssetDto&gt;.</returns>
        public List<CustomerAssetDto> GetCustomerAssets(Guid? accountId)
        {
            try
            {
                InsertEventLog("GetCustomerAssets", EventType.Log, EventColor.yellow, "Enter in to get list of customer assets on accountid="+ accountId + " ", "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssets", "");
                var customerAssetDtos = new List<CustomerAssetDto>();
                var customerAssets = dbEnt.CustomerAssets.Include(c => c.CustomerAssetType).Include(c => c.Status).Include(c => c.Team).Include(c => c.User).Where(a => a.IsDeleted != true && a.AccountId == accountId).ToList();

                foreach (var item in customerAssets.CollectionNotNull())
                {
                    customerAssetDtos.Add(objMapper.GetCustomerAssetDTO(item));
                }
                return customerAssetDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCustomerAssets", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetCustomerAssets", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Gets the asset types.
        /// </summary>
        /// <returns>Types.</returns>
        public Types GetAssetTypes()
        {
            try
            {
                InsertEventLog("GetAssetTypes", EventType.Log, EventColor.yellow, "Enter in to get list of customer assets Types ", "TICRM.BuisnessLayer.CustomerAssetManager.GetAssetTypes", "");

                var types = new Types
                {
                    Engine = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Engine").Count(),
                    Boiler = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Boiler").Count(),
                    Generator = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Generator").Count(),
                    Projector = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Projector").Count(),
                    Tours = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Tours").Count()
                };
                return types;
            }
            catch(Exception ex)
            {
                InsertEventMonitor("GetAssetTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetAssetTypes", "");
                throw ex;
            }
        }


        public Types GetAccountAssetTypes(Guid AccountId)
        {
            try
            {
                InsertEventLog("GetAssetTypes", EventType.Log, EventColor.yellow, "Enter in to get list of customer assets Types ", "TICRM.BuisnessLayer.CustomerAssetManager.GetAssetTypes", "");

                var types = new Types
                {
                    Engine = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Engine" && a.AccountId == AccountId).Count(),
                    Boiler = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Boiler" && a.AccountId == AccountId).Count(),
                    Generator = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Generator" && a.AccountId == AccountId).Count(),
                    Projector = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Projector" && a.AccountId == AccountId).Count(),
                    Tours = dbEnt.CustomerAssets.Include(o => o.CustomerAssetType).Where(a => a.CustomerAssetType.Name == "Tours" && a.AccountId == AccountId).Count()
                };
                return types;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAssetTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetAssetTypes", "");
                throw ex;
            }
        }

        /// <summary>
        /// Gets the location assets.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <returns>List&lt;CustomerAssetDto&gt;.</returns>
        public List<CustomerAssetDto> GetLocationAssets(Guid locationId)
        {
            try
            {
                InsertEventLog("GetLocationAssets", EventType.Log, EventColor.yellow, "Enter in to get list of customer assets on locationId=" + locationId + " ", "TICRM.BuisnessLayer.CustomerAssetManager.GetLocationAssets", "");
                var customerAssetDtos = new List<CustomerAssetDto>();
                var customerAssets = dbEnt.CustomerAssets.Include(c => c.CustomerAssetType).Include(c => c.Status).Include(c => c.Team).Include(c => c.User).Where(a => a.IsDeleted != true && a.LocationId == locationId).ToList();

                foreach (var item in customerAssets.CollectionNotNull())
                {
                    customerAssetDtos.Add(objMapper.GetCustomerAssetDTO(item));
                }
                return customerAssetDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetLocationAssets", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.GetLocationAssets", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// save and edit account 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool SaveCustomerAsset(CustomerAssetDto acc,string CurrentUserId,string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveCustomerAsset",  EventType.Log, EventColor.yellow, "Successfully Enter", "TICRM.BuisnessLayer.CustomerAssetManager.SaveCustomerAsset", "");

                CustomerAsset customerAsset;
                if (isEditMode)
                {
                    InsertEventLog("SaveCustomerAsset", EventType.Log, EventColor.yellow, "Enter in edit Mode to edit on id="+ acc.CustomerAssetId, "TICRM.BuisnessLayer.CustomerAssetManager.SaveCustomerAsset", "");

                    customerAsset = objMapper.GetCustomerAsset(acc);
                    CustomerAsset dbData = dbEnt.CustomerAssets.FirstOrDefault(x => x.CustomerAssetId == customerAsset.CustomerAssetId);
                    if (dbData != null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveCustomerAsset", EventType.Log, EventColor.yellow, "Enter in delete Mode to delete on id=" + acc.CustomerAssetId + " ", "TICRM.BuisnessLayer.CustomerAssetManager.SaveCustomerAsset", "");
                            CustomerAsset CustomerDelete = dbEnt.CustomerAssets.FirstOrDefault(x => x.CustomerAssetId == customerAsset.CustomerAssetId);
                            //Soft Delete Lead
                            CustomerDelete.IsDeleted = true;
                        }
                        else
                        {
                            //Update Customer Asset
                            dbData.CustomerAssetId = customerAsset.CustomerAssetId;
                            dbData.Title = customerAsset.Title;
                            dbData.CustomerAssetTypeId = customerAsset.CustomerAssetTypeId;
                            dbData.Manufacture = customerAsset.Manufacture;
                            dbData.Model = customerAsset.Model;
                            dbData.YearOfManufacture = customerAsset.YearOfManufacture;
                            dbData.Value = customerAsset.Value;
                            dbData.DepriciatedValue = customerAsset.DepriciatedValue;
                            dbData.SKU = customerAsset.SKU;
                            dbData.Description = customerAsset.Description;
                            dbData.AccountId = customerAsset.AccountId;
                            dbData.LocationId = customerAsset.LocationId;
                            dbData.StatusId = customerAsset.StatusId;
                            dbData.AssignedUser = customerAsset.AssignedUser;
                            dbData.AssignedTeam = customerAsset.AssignedTeam;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                        }
                    }
                }
                else
                {
                    //Save Customer Asset in DB
                    InsertEventLog("SaveCustomerAsset", EventType.Log, EventColor.yellow, "Enter in Create New Record of Customer Asset", "TICRM.BuisnessLayer.CustomerAssetManager.SaveCustomerAsset", "");
                    customerAsset = objMapper.GetCustomerAsset(acc);
                    customerAsset.CustomerAssetId = Guid.NewGuid();
                    customerAsset.CreatedDate = DateTime.Now;
                    customerAsset.CreatedBy = CurrentUserId;
                    customerAsset.Company = Guid.Parse(UserCompanyID);
                    dbEnt.CustomerAssets.Add(customerAsset);
                }
                // apply condition to check if db changes is done then  return true in response 
                if (dbEnt.SaveChanges()>0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveCustomerAsset", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CustomerAssetManager.SaveCustomerAsset", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false; //there is on changes in db against the object
        }

        /// <summary>
        /// Class Types.
        /// </summary>
        public class Types
        {
            public int Projector;
            public int Engine;
            public int Boiler;
            public int Generator;
            public int Tours;

        }
    }
}
