
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{

    /***************************************************************************************
    ||  Class [AccountManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             account are get from the database and mapped with the corrosponding DTO
    ||             object to send it back to the controller]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/

    public class AccountManager : BaseManager
    {

        public AccountManager()
        {
            //AccountSizes = GetAccountSizes();    
            //AccountTypes = GetAccountTypes();
            //Industries = GetIndustries();
            //Currencies = GetCurrencies();
        }

        #region Properties & Methods

        public List<AccountSizeDto> AccountSizes { get; set; }
        //public List<AccountTypeDto> AccountTypes { get; set; }
        //public List<IndustryDto> Industries { get; set; }
        //public List<CurrencyDto> Currencies { get; set; }

        /// <summary>
        /// Get Account Size Dt
        /// </summary>
        /// <returns></returns>
       
        //public List<AccountSizeDto> GetAccountSizes()
        //{
        //    try
        //    {
        //        InsertEventLog("GetAccountSize", EventType.Log, EventColor.yellow, "Successfully Enter in GetAccountSizes", "TICRM.BusinessLayer.AccountManager", "");

        //        List<AccountSizeDto> accountSizeDtos = new List<AccountSizeDto>();

        //        foreach (AccountSize item in dbEnt.AccountSizes.CollectionNotNull())
        //        {
        //            accountSizeDtos.Add(objMapper.GetAccountSizeDTO(item));
        //        }
        //        return accountSizeDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetAccountSize", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get Currencies 
        /// </summary>
        /// <returns></returns>
        
        //public List<CurrencyDto> GetCurrencies()
        //{
        //    try
        //    {
        //        InsertEventLog("GetCurrencies", EventType.Log, EventColor.yellow, "Get List Of CurrencyDto", "TICRM.BusinessLayer.OpportunityManager.GetCurrencies", "");
        //        List<CurrencyDto> currenciesDtos = new List<CurrencyDto>();

        //        foreach (Currency item in dbEnt.Currencies.CollectionNotNull())
        //        {
        //            currenciesDtos.Add(objMapper.GetCurrencyDTO(item));
        //        }
        //        return currenciesDtos;
        //    }
        //    catch (Exception ex)
        //    {

        //        InsertEventMonitor("GetCurrencies", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.OpportunityManager.GetCurrencies", "");
        //        throw;
        //    }
        //}


        /// <summary>
        /// Get Account Types Dtos list
        /// </summary>
        /// <returns></returns>
        public AccountViewModel GetAccTypes()
        {
            return null;
        }

        /// <summary>
        /// Gets the account types.
        /// </summary>
        /// <returns></returns>
        

        //public List<AccountTypeDto> GetAccountTypes()
        //{
        //    try
        //    {
        //        InsertEventLog("GetAccountTypes", EventType.Log, EventColor.yellow, "Successfully Enter in GetAccountTypes", "TICRM.BusinessLayer.AccountManager", "");

        //        List<AccountTypeDto> accountTypeDtos = new List<AccountTypeDto>();

        //        foreach (AccountType item in dbEnt.AccountTypes.CollectionNotNull())
        //        {
        //            accountTypeDtos.Add(objMapper.GetAccountTypeDTO(item));
        //        }
        //        //string json = JsonConvert.SerializeObject(accountTypeDtos);
        //        return accountTypeDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetAccountTypes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", "");
        //        throw;
        //    }
        //}


        /// <summary>
        /// Gets the accounts list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;AccountDto&gt;.</returns>
        public List<AccountDto> GetAccountsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetAccountsList", EventType.Log, EventColor.yellow, "Get List of Accounts Based on Datatable Query", "TICRM.BuisnessLayer.AccountManager.GetAccountsList", "");

                var Accounts = new List<Account>();
                var AccountDto = new List<AccountDto>();

                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;

                // apply iteration on workFlowMappings


                if (!string.IsNullOrEmpty(sSearch))
                {
                    Accounts = dbEnt.Accounts.Include(a => a.AccountSize).Include(a => a.AccountType).Include(a => a.Address)
                                .Include(a => a.Address1).Include(a => a.Industry).Include(a => a.Status).Include(a => a.Team)
                                .Include(a => a.User)
                                .Where(a => a.IsDeleted != true &&
                                (a.Name.ToLower().Contains(sSearch) || a.Email.ToLower().Contains(sSearch)
                                || a.Description.ToLower().Contains(sSearch)
                                || a.AccountType.Name.ToLower().Contains(sSearch)
                                || a.Opportunities.ToString().ToLower().Contains(sSearch)
                                || a.Locations.Count.ToString().ToLower().Contains(sSearch)
                                || a.CustomerAssets.Count.ToString().ToLower().Contains(sSearch))
                                ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    Accounts = dbEnt.Accounts.Include(a => a.AccountSize)
                                .Include(a => a.AccountType).Include(a => a.Address)
                                .Include(a => a.Address1).Include(a => a.Industry).Include(a => a.Status)
                                .Include(a => a.Team).Include(a => a.User).Where(a => a.IsDeleted != true)
                                .OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Account item in Accounts.CollectionNotNull())
                {
                    AccountDto.Add(objMapper.GetAccountDTO(item)); // add in a list object
                }

                foreach (var item in AccountDto)
                {
                    item.AssetCount = this.GetAssetsCounts(item.AccountId);
                    item.OppCount = this.GetOpportunityCount(item.AccountId);
                    item.LocationCount = this.GetLocationsCounts(item.AccountId);
                    item.DeviceCount = this.GetDevicesCounts(item.AccountId);
                }

                return AccountDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountsList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AccountManager.GetAccountsList", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Get Industries Dtos list
        /// </summary>
        /// <returns></returns>
      
        //public List<IndustryDto> GetIndustries()
        //{
        //    try
        //    {
        //        InsertEventLog("GetIndustries", EventType.Log, EventColor.yellow, "Successfully Enter in GetIndustries", "TICRM.BusinessLayer.AccountManager", "");

        //        List<IndustryDto> industriesDtos = new List<IndustryDto>();

        //        foreach (Industry item in dbEnt.Industries.CollectionNotNull())
        //        {
        //            industriesDtos.Add(objMapper.GetIndustryDTO(item));
        //        }
        //        return industriesDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEventMonitor("GetIndustries", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", "");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Accounts", "TICRM.BuisnessLayer.AccountManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Accounts.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AccountManager.GetTotalCount", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Gets the account.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>AccountDto.</returns>
        public AccountDto GetAccount(Guid? guid)
        {
            try
            {
                InsertEventLog("GetAccount", EventType.Log, EventColor.yellow, "Successfully Enter in GetAccount", "TICRM.BusinessLayer.AccountManager", "");
                return objMapper.GetAccountDTO(dbEnt.Accounts.Find(guid));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }


        /// <summary>
        /// Get account list 
        /// </summary>
        /// <returns></returns>
        public List<AccountDto> GetAccounts(string CurrentUserId, string UserRole, string UserCompany)
        {
            try
            {
                InsertEventLog("GetAccounts", EventType.Log, EventColor.yellow, "Successfully Enter in GetAccounts", "TICRM.BusinessLayer.AccountManager", "");
                List<AccountDto> accountDtos = new List<AccountDto>();
                
                List<Account> acc = dbEnt.sp_Accounts_Get(CurrentUserId, UserRole, UserCompany).ToList();

                foreach (Account item in acc.CollectionNotNull())
                {
                    accountDtos.Add(objMapper.GetAccountDTO(item));
                }

                return accountDtos;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccounts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// save and edit account 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool SaveAccount(AccountDto acc, string CurrentUserId,string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveAccount", EventType.Log, EventColor.yellow, "Successfully Enter in SaveAccount", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                Account account;
                if (isEditMode)
                {
                    account = objMapper.GetAccount(acc);
                    Account a = dbEnt.Accounts.FirstOrDefault(x => x.AccountId == account.AccountId);
                    if (a != null)
                    {
                      
                        if (isDeleteMode)
                        {
                            Account AccountDelete = dbEnt.Accounts.Include(X=>X.Contacts).Include(X => X.AccountSize).Include(X => X.Address).Include(X => X.Address1).Include(X => X.Industry).Include(X => X.Contacts).Include(X => X.Status).Include(X => X.Team).Include(X => X.User).Include(X => X.LeadAccounts).Include(X => X.AccountType).Include(X => X.Disconnections).Include(X => X.Consumptions).Include(X => X.Costs).Include(X => X.WorkOrders).Include(X => X.CustomerAssets).Include(X => X.Devices).Include(X => X.Locations).Include(X => X.Opportunities).FirstOrDefault(x => x.AccountId == account.AccountId);
                            AccountDelete.IsDeleted = true;
                            //dbEnt.Entry(AccountDelete).State = EntityState.Modified;
                        }
                        else
                        {
                            InsertEventLog("SaveAccount", EventType.Log, EventColor.yellow, "For Edit. Successfully Enter in SaveAccount", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                            a.Name = account.Name;
                            a.ShippingAddress = account.ShippingAddress;
                            a.BillingAddress = account.BillingAddress;
                            a.AccountTypeId = account.AccountTypeId;
                            a.PhoneOffice = account.PhoneOffice;
                            a.Email = account.Email;
                            a.Fax = account.Fax;
                            a.WebSite = account.WebSite;
                            a.AccountSizeId = account.AccountSizeId;
                            a.IndustryId = account.IndustryId;
                            a.Description = account.Description;
                            a.StatusId = account.StatusId;
                            a.AssignedUser = account.AssignedUser;
                            a.AssignedTeam = account.AssignedTeam;
                            a.CurrencyId = account.CurrencyId;
                            a.UpdatedBy = CurrentUserId;
                            a.UpdatedDate = DateTime.Now;
                        }
                        HttpContext.Current.Session["AccountObject"] = account;
                        HttpContext.Current.Session["CurrentUserId"] = CurrentUserId;
                        if (dbEnt.SaveChanges() > 0)
                        {
                            return true;
                        }
                        //dbEnt.Entry(account).State = EntityState.Modified;
                    }
                }
                else
                {
                    InsertEventLog("SaveAccount", EventType.Log, EventColor.yellow, "For Create. Successfully Enter in SaveAccount", "TICRM.BusinessLayer.AccountManager", CurrentUserId);

                    account = objMapper.GetAccount(acc);
                    account.AccountId = Guid.NewGuid();
                    account.Company = Guid.Parse(UserCompanyID);
                    account.CreatedBy = CurrentUserId;
                    account.CreatedDate = DateTime.Now;
                    dbEnt.Accounts.Add(account);
                }

                if (dbEnt.SaveChanges() > 0)
                {
                    HttpContext.Current.Session["AccountObject"] = account;
                    InsertEventLog("SaveAccount", EventType.Log, EventColor.yellow, "data saved successfully in SaveAccount", "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                    return true;
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveAccount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

            return false;
        }

        /// <summary>
        /// Get detailed object of account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// 


        public AccountViewModel GetAccountAndDetails(Guid accountId)
        {
            InsertEventLog("GetAccountAndDetails", EventType.Log, EventColor.yellow, "get accountviewmodel on accountid. Successfully Enter in GetAccountAndDetails", "TICRM.BusinessLayer.AccountManager", "");

            AccountViewModel avm = new AccountViewModel();
            try
            {

                LocationManager lm = new LocationManager();
                OpportunityManager om = new OpportunityManager();
                DeviceManager dm = new DeviceManager();
                CustomerAssetManager cam = new CustomerAssetManager();
                ActivityManager acc = new ActivityManager();
                WorkOrderManager workOrderManager = new WorkOrderManager();
                ContactManager contactManager = new ContactManager();
                WorkFlowReportManager workflowManager = new WorkFlowReportManager();
                WorkFlowManager workflowmanag = new WorkFlowManager();
                CaseManager cm = new CaseManager();
                avm.account = GetAccount(accountId);
                avm.accountLocations = lm.GetLocations(accountId);
                avm.accountDevices = dm.GetDevices(accountId);
                avm.accountOppertunities = om.GetOpportunities(accountId);
                avm.accountAssetes = cam.GetCustomerAssets(accountId);
                avm.accountActivity = acc.GetAccountActivities(accountId);
                avm.accountWorkOrder = workOrderManager.GetAccountWorkorders(accountId);
                avm.accountContact = contactManager.GetAccountContacts(accountId);
                avm.accountWorkflow = workflowManager.GetWorkFlowReportsAccount(accountId);
                avm.allworkflowsforaccount = workflowmanag.GetAccountWorkflows(accountId.ToString());
                avm.accountCases = cm.GetAccountCases(accountId);

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountAndDetails", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            InsertEventLog("GetAccountAndDetails", EventType.Log, EventColor.yellow, "Successfully get information in GetAccountAndDetails", "TICRM.BusinessLayer.AccountManager", "");
            return avm;
        }

        /// <summary>
        /// Gets the workflow reports.
        /// </summary>
        /// <returns></returns>
        public AccountViewModel GetWorkflowReports()
        {
            AccountViewModel avm = new AccountViewModel();
            WorkFlowReportManager wfm = new WorkFlowReportManager();
            avm.workflowReportAdmin = wfm.GetWorkFlowReports();
            return avm;
        }

        /// <summary>
        /// To get Device Counts related to a account
        /// </summary>
        /// <returns></returns>
        public int GetDevicesCounts(Guid accountId)
        {
            try
            {
                InsertEventLog("GetDevicesCounts", EventType.Log, EventColor.yellow, "to get total no of devices count", "TICRM.BuisnessLayer.AccountManager.GetDevicesCounts", "");

                DeviceManager dm = new DeviceManager();
                return dm.GetDevices(accountId).Count();

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevicesCounts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AccountManager.GetDevicesCounts", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// To get DevOpportunitesice Counts related to a account
        /// </summary>
        /// <returns></returns>
        public int GetOpportunityCount(Guid accountId)
        {

            try
            {
                InsertEventLog("GetOpportunityCount", EventType.Log, EventColor.yellow, "to get total no of Opportunities count", "TICRM.BuisnessLayer.AccountManager.GetOpportunityCount", "");
                OpportunityManager om = new OpportunityManager();
                return om.GetOpportunities(accountId).Count();

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetOpportunityCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AccountManager.GetOpportunityCount", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// To get Locations Counts related to a account
        /// </summary>
        /// <returns></returns>
        public int GetLocationsCounts(Guid accountId)
        {

            try
            {
                InsertEventLog("GetLocationsCounts", EventType.Log, EventColor.yellow, "to get total no of locations count", "TICRM.BuisnessLayer.AccountManager.GetLocationsCounts", "");
                LocationManager lm = new LocationManager();
                return lm.GetLocations(accountId).Count();

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetLocationsCounts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AccountManager.GetLocationsCounts", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }


        /// <summary>
        /// To get Assets Counts related to a account
        /// </summary>
        /// <returns></returns>
        public int GetAssetsCounts(Guid accountId)
        {

            try
            {
                InsertEventLog("GetAssetsCounts", EventType.Log, EventColor.yellow, "to get total no of Asset count", "TICRM.BuisnessLayer.AccountManager.GetAssetsCounts", "");
                CustomerAssetManager cam = new CustomerAssetManager();
                return cam.GetCustomerAssets(accountId).Count();

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAssetsCounts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AccountManager.GetAssetsCounts", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the account associates.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>AccountViewModel.</returns>
        public AccountViewModel Get_Account_Associates(Guid accountId)
        {
            InsertEventLog("Get_Account_Associates", EventType.Log, EventColor.yellow, "get Get_Account_Associates on accountid. Successfully Enter in Get_Account_Associates", "TICRM.BusinessLayer.AccountManager,Get_Account_Associates", "");

            AccountViewModel avm = new AccountViewModel();
            try
            {
                LocationManager lm = new LocationManager();
                OpportunityManager om = new OpportunityManager();
                DeviceManager dm = new DeviceManager();
                CustomerAssetManager cam = new CustomerAssetManager();
                ActivityManager acc = new ActivityManager();
                WorkFlowReportManager workflowreportManager = new WorkFlowReportManager();
                WorkFlowManager workflowmanager = new WorkFlowManager();

                avm.account = GetAccount(accountId);
                avm.accountLocations = lm.GetLocations(accountId);

                avm.accountDevices = dm.GetDevices(accountId);
                avm.accountOppertunities = om.GetOpportunities(accountId);
                avm.accountAssetes = cam.GetCustomerAssets(accountId);
                avm.accountActivity = acc.GetAccountActivities(accountId);
                avm.accountWorkflow = workflowreportManager.GetWorkFlowReports();
                avm.allworkflowsforaccount = workflowmanager.GetAccountWorkflows(accountId.ToString());
            }
            catch (Exception ex)
            {
                InsertEventMonitor("Get_Account_Associates", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.AccountManager.Get_Account_Associates", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            InsertEventLog("Get_Account_Associates", EventType.Log, EventColor.yellow, "Successfully get information in Get_Account_Associates", "TICRM.BusinessLayer.AccountManager.Get_Account_Associates", "");
            return avm;
        }

        public DeviceChannelDonutChartVM Get_ChannelPercentage(Guid accountId) 
        {
            DeviceChannelDonutChartVM objVM = new DeviceChannelDonutChartVM();

            List<double> lst = new List<double>();

            var Channels = dbEnt.sp_Channel_Get(accountId.ToString()).ToList();


            double mqtt = Convert.ToDouble(Channels.First().MQTT);
            double http = Convert.ToDouble(Channels.First().HTTP_P);
            double lorawan = Convert.ToDouble(Channels.First().LORAWAN);
            double total = Convert.ToDouble(Channels.First().Total);

            double mqttPr = (mqtt / total) * 100;
            double httpPr = (http / total) * 100;
            double lorawanPr = (lorawan / total) * 100;

            objVM.lstPercentage.Add(mqttPr);
            objVM.lstPercentage.Add(httpPr);
            objVM.lstPercentage.Add(lorawanPr);

            objVM.lstLabels.Add("MQTT: " + mqtt);
            objVM.lstLabels.Add("HTTP: " + http);
            objVM.lstLabels.Add("LORAWAN: " + lorawan);

            return objVM;
        }

        public DeviceNetworkProgressbar DeviceNetworkBar(Guid accountId)
        {
            DeviceNetworkProgressbar objDeviceNetwork = new DeviceNetworkProgressbar();

            string id = accountId.ToString();
            var Network = dbEnt.spGetNetwork(id).ToList();
            double WIFI = Convert.ToDouble(Network.First().WIFI);
            double Ethernet = Convert.ToDouble(Network.First().Ethernet);
            double Cellular = Convert.ToDouble(Network.First().Cellular);
            double Bluetooth = Convert.ToDouble(Network.First().Bluetooth);

            double Total = WIFI + Ethernet + Cellular + Bluetooth;

            double WIFI_Per = (WIFI / Total) * 100;
            double Ethernet_Per = (Ethernet / Total) * 100;
            double Cellular_Per = (Cellular / Total) * 100;
            double Bluetooth_Per = (Bluetooth / Total) * 100;

            objDeviceNetwork.lst_DeviceCount.Add(WIFI);
            objDeviceNetwork.lst_DeviceCount.Add(Ethernet);
            objDeviceNetwork.lst_DeviceCount.Add(Cellular);
            objDeviceNetwork.lst_DeviceCount.Add(Bluetooth);

            objDeviceNetwork.lst_DevicePercentage.Add(WIFI_Per);
            objDeviceNetwork.lst_DevicePercentage.Add(Ethernet_Per);
            objDeviceNetwork.lst_DevicePercentage.Add(Cellular_Per);
            objDeviceNetwork.lst_DevicePercentage.Add(Bluetooth_Per);


            return objDeviceNetwork;
        }
    }
}
