using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [GlobalSearchManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Saving, updating global search options. Getting search 
    ||             results and items for different modules in general and on Id}
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||  Changes Made:   [18/08/2020     Changed the flow of search methods      Akhtar Zaman]
    ||  Changes Made:   [20/08/2020     Added method for free Search case       Akhtar Zaman]
    ****************************************************************************************/
    public class GlobalSearchManager : BaseManager
    {
        /// <summary>
        /// Get List  global search.
        /// </summary>

        public List<GlobalSearchDto> GetGlobalSearch()
        {
            try
            {
                InsertEventLog("GetGlobalSearch", EventType.Log, EventColor.yellow, "To get Device Dto list ", "TICRM.BuisnessLayer.GlobalSearchManager.GetGlobalSearch", "");
                List<GlobalSearchDto> globalSearchDtos = new List<GlobalSearchDto>();
                List<GlobalSearch> globalseach = dbEnt.GlobalSearches.ToList();  // Get Globalsearch List
               
                foreach (GlobalSearch item in globalseach.CollectionNotNull())
                {
                    globalSearchDtos.Add(objMapper.GetGlobalSearchDto(item));
                }
                return globalSearchDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetGlobalSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.GlobalSearchManager.GetGlobalSearch", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        public bool SaveGlobalSearch(GlobalSearchDto globalSearch, bool isEditMode = false)
        {
            try
            {
               
                GlobalSearch global;
                
                if (isEditMode)
                {
                    global = objMapper.GetGlobalSearchdata(globalSearch);
                    GlobalSearch dbData = dbEnt.GlobalSearches.FirstOrDefault(x => x.GlobalSearchId == global.GlobalSearchId);
                    if (dbData!= null)
                    {
                        dbData.GlobalSearchId = globalSearch.GlobalSearchId;
                        dbData.Name = globalSearch.Name;
                        dbData.URL = globalSearch.URL;
                        dbData.Type = "URL";
                        dbEnt.Entry(dbData).State = EntityState.Modified;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    InsertEventLog("SaveAlert", EventType.Log, EventColor.yellow, "Enter In Create New Record Alert", "TICRM.BuisnessLayer.AlertManager.SaveAlert", "");
                    global = objMapper.GetGlobalSearchdata(globalSearch);
                    global.GlobalSearchId = Guid.NewGuid();
                    dbEnt.GlobalSearches.Add(global);
                }

                dbEnt.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveAlert", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.AlertManager.SaveAlert", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        /// <summary>
        /// Save the global search.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="URL">The URL.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SavedGlobalSearch(string Name, string URL)
        {
            try
            {
                InsertEventLog("SavedGlobalSearch", EventType.Log, EventColor.yellow, "save global search ", "TICRM.BuisnessLayer.GlobalSearchManager.SavedGlobalSearch", "");
                GlobalSearch data = new GlobalSearch(); // Decalre an object for to save a data
                data.GlobalSearchId = Guid.NewGuid();   // inset a new guid in object
                data.Name = Name;
                data.URL = URL;
                data.Type = "URL"; // set type as URL its useful in searching
                dbEnt.GlobalSearches.Add(data); // add objct to save in db
                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("SavedGlobalSearch", EventType.Log, EventColor.yellow, "successfully saved global search ", "TICRM.BuisnessLayer.GlobalSearchManager.SavedGlobalSearch", "");
                    return true;    // return true if data saved.
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SavedGlobalSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.GlobalSearchManager.SavedGlobalSearch", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

            return false; // return false if data not saved
        }

        /// <summary>
        /// Updates the global search.
        /// </summary>
        /// <param name="GlobalSearchId">The global search identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="URL">The URL.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UpdateGlobalSearch(Guid GlobalSearchId, string Name, string URL)
        {
            try
            {
                InsertEventLog("UpdateGlobalSearch", EventType.Log, EventColor.yellow, "going to get globalsearch on id=" + GlobalSearchId + "", "TICRM.BusinessLayer.GlobalSearchManager.UpdateGlobalSearch", "");
                GlobalSearch data = dbEnt.GlobalSearches.FirstOrDefault(x => x.GlobalSearchId == GlobalSearchId);       // Get global search object by globalsearchid
                data.Name = Name;
                data.URL = URL;
                data.Type = "URL";
                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("UpdateGlobalSearch", EventType.Log, EventColor.yellow, "successfully updated global search", "TICRM.BusinessLayer.GlobalSearchManager.UpdateGlobalSearch", "");
                    return true; // return true if data is saved
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("UpdateGlobalSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.UpdateGlobalSearch", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false; // return false if data is not saved.
        }

        /// <summary>
        /// Gets the global search list.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetGlobalSearchList()
        {
            try
            {
                InsertEventLog("GetGlobalSearchList", EventType.Log, EventColor.yellow, "going to get globalsearch list", "TICRM.BusinessLayer.GlobalSearchManager.GetGlobalSearchList", "");
                List<GlobalSearch> query = dbEnt.GlobalSearches.ToList();  // Get Globalsearch List
                string status = Newtonsoft.Json.JsonConvert.SerializeObject(query); // convert in json string
                return status;  // return json string
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetGlobalSearchList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.GetGlobalSearchList", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the global search on identifier.
        /// </summary>
        /// <param name="GlobalSearchId">The global search identifier.</param>
        /// <returns>GlobalSearchDto.</returns>
        public GlobalSearchDto GetGlobalSearchOnId(Guid GlobalSearchId)
        {
            try
            {
                InsertEventLog("GetGlobalSearchOnId", EventType.Log, EventColor.yellow, "get global searchDto On Id=" + GlobalSearchId + "", "TICRM.BusinessLayer.GlobalSearchManager.GetGlobalSearchOnId", "");
                // return object GlobalSearches on GlobalSearchId
                return objMapper.GetGlobalSearchDto(dbEnt.GlobalSearches.FirstOrDefault(x => x.GlobalSearchId == GlobalSearchId));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetGlobalSearchOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.GetGlobalSearchOnId", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Deletes global search on identifier.
        /// </summary>
        /// <param name="GlobalSearchId">The global search identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteGlobalSearchOnId(Guid GlobalSearchId)
        {
            try
            {
                InsertEventLog("DeleteGlobalSearchOnId", EventType.Log, EventColor.yellow, "enter in delete global search On Id=" + GlobalSearchId + "", "TICRM.BusinessLayer.GlobalSearchManager.DeleteGlobalSearchOnId", "");
                GlobalSearch dsg = dbEnt.GlobalSearches.FirstOrDefault(x => x.GlobalSearchId == GlobalSearchId); // find object in database on GlobalSearchId
                if (dsg != null)
                {
                    dbEnt.GlobalSearches.Remove(dsg); // remove object in database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("DeleteGlobalSearchOnId", EventType.Log, EventColor.yellow, "successfully deleted global search On Id=" + GlobalSearchId + "", "TICRM.BusinessLayer.GlobalSearchManager.DeleteGlobalSearchOnId", "");

                        return true; // if data removed return true
                    }
                }
                else
                {
                    InsertEventLog("DeleteGlobalSearchOnId", EventType.Log, EventColor.yellow, "for delete global search On Id=" + GlobalSearchId + " data is null", "TICRM.BusinessLayer.GlobalSearchManager.DeleteGlobalSearchOnId", "");
                }
                return false; // if data not saved
            }
            catch (Exception ex)
            {
                InsertEventMonitor("DeleteGlobalSearchOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.DeleteGlobalSearchOnId", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// First in search data.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> FirstInSearchData()
        {
            try
            {
                InsertEventLog("FirstInSearchData", EventType.Log, EventColor.yellow, "going to getting list first time load URL", "TICRM.BusinessLayer.GlobalSearchManager.FirstInSearchData", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>(); // declare a new list object
                List<GlobalSearch> query = dbEnt.GlobalSearches.ToList();  // query in db to get List
                // apply iteration on query and add in list object
                foreach (GlobalSearch item in query.CollectionNotNull())
                {
                    // body of foreach loop
                    // First object for navigations 
                    SearchDataViewModel SearchData = new SearchDataViewModel();
                    SearchData.Result = "<a href='" + item.URL + "'>" + item.Name + "</a>";
                    SearchData.FirstURL = item.URL;
                    SearchData.Text = item.Name;
                    SearchData.Type = item.Type;
                    SearchData.value = "List";
                    list.Add(SearchData); // add an Object in list object
                    // Second object for Items
                    SearchDataViewModel SearchDataUrl = new SearchDataViewModel();
                    SearchDataUrl.Result = "<a href='" + item.URL + "'>" + item.Name + "</a>";
                    SearchDataUrl.FirstURL = item.URL;
                    SearchDataUrl.Text = item.Name;
                    SearchDataUrl.Type = "More";
                    SearchDataUrl.value = "More";
                    list.Add(SearchDataUrl); // add an Object in list object
                }
                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("FirstInSearchData", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.FirstInSearchData", "");
                throw ex;
            }

        }

        /// <summary>
        /// Devices data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> DeviceDataForSearch()
        {
            try
            {
                InsertEventLog("DeviceDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Device For Search", "TICRM.BusinessLayer.GlobalSearchManager.DeviceDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Devices.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Device.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    //    // body of foreach loop
                    SearchDataViewModel deviceDetails = new SearchDataViewModel();  // declare a new object to add data for search
                                                                                   

                    deviceDetails.Result = "<a href='/Devices/Details/" + item.DeviceId + "'>Device</a>";
                    deviceDetails.FirstURL = item.DeviceId.ToString();
                    deviceDetails.Text = "Devices > " + item.Name + " > Details";
                    deviceDetails.Type = "Modal";
                    deviceDetails.value = item.Mac;
                    deviceDetails.JS_function = "Devices_Details_Modal('" + item.DeviceId + "')";
                    list.Add(deviceDetails); // add an Object in list object

                }
                return list; // return list object

            }
            catch (Exception ex)
            {

                InsertEventMonitor("DeviceDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.DeviceDataForSearch", "");
                throw ex;
            }

        }
        /// <summary>
        /// Devices data for search device.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> DeviceDataForSearch_device()
        {
            try
            {
                InsertEventLog("DeviceDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Device For Search", "TICRM.BusinessLayer.GlobalSearchManager.DeviceDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Devices.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Device.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    // body of foreach loop
                    SearchDataViewModel deviceEdit = new SearchDataViewModel();     // declare a new object to add data for search
                    SearchDataViewModel deviceDetails = new SearchDataViewModel();  // declare a new object to add data for search
                    SearchDataViewModel MacData = new SearchDataViewModel();        // declare a new object to add data for search
                    deviceEdit.Result = "<a href='/Devices/Edit/" + item.DeviceId + "'>Device</a>";
                    deviceEdit.FirstURL = "/Devices/Edit/" + item.DeviceId;
                    deviceEdit.Text = "Devices > " + item.Name + " > Edit";
                    deviceEdit.Type = "URL";
                    deviceEdit.value = item.Mac;

                    list.Add(deviceEdit); // add an Object in list object

                    deviceDetails.Result = "<a href='/Devices/Details/" + item.DeviceId + "'>Device</a>";
                    deviceDetails.FirstURL = item.DeviceId.ToString();
                    deviceDetails.Text = "Devices > " + item.Name + " > Details";
                    deviceDetails.Type = "Modal";
                    deviceDetails.value = item.Mac;
                    deviceDetails.JS_function = "Devices_Details_Modal('" + item.DeviceId + "')";
                    list.Add(deviceDetails); // add an Object in list object

                    MacData.Result = item.Mac;
                    MacData.FirstURL = "/Devices/device";
                    MacData.Text = "Devices > " + item.Name + " > View";
                    MacData.value = item.Mac;
                    MacData.Type = "MACAddress";
                    list.Add(MacData); // add an Object in list object

                    foreach (workflowDataTypeDTO selected in attr.CollectionNotNull())
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Devices/Edit/" + item.DeviceId + "'>Edit Devices</a>";
                        s2.FirstURL = "/Devices/Edit/" + item.DeviceId;
                        s2.Text = "Devices > " + item.Name + " > " + selected.ColumnName;
                        string value = (String)item.GetType().GetProperty(selected.ColumnName).GetValue(item);
                        s2.value = value;
                        s2.Type = "URL";
                        list.Add(s2); // add an Object in list object
                    }

                }
                


                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("DeviceDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.DeviceDataForSearch", "");
                throw ex;
            }

        }

        /// <summary>
        /// get Devices data of mac.
        /// </summary>
        /// <param name="MAC">The mac.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> DeviceDataOfMAC(string MAC)
        {
            try
            {
                InsertEventLog("DeviceDataOfMAC", EventType.Log, EventColor.yellow, "Get List Of MAC For Search", "TICRM.BusinessLayer.GlobalSearchManager.DeviceDataOfMAC", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                Device query = dbEnt.Devices.Include(x => x.Status).Include(d => d.Team).Include(d => d.User).FirstOrDefault(x => x.Mac == MAC);
                if (query != null)
                {
                    SearchDataViewModel deviceName = new SearchDataViewModel();                     // declare a new object to add data in a list
                    SearchDataViewModel deviceMAC = new SearchDataViewModel();                      // declare a new object to add data in a list
                    SearchDataViewModel deviceEMEI = new SearchDataViewModel();                     // declare a new object to add data in a list
                    SearchDataViewModel deviceRegistrationDate = new SearchDataViewModel();         // declare a new object to add data in a list
                    SearchDataViewModel deviceLatitude = new SearchDataViewModel();                 // declare a new object to add data in a list
                    SearchDataViewModel deviceLongitude = new SearchDataViewModel();                // declare a new object to add data in a list
                    SearchDataViewModel deviceStatus = new SearchDataViewModel();                   // declare a new object to add data in a list
                    SearchDataViewModel deviceAccountId = new SearchDataViewModel();                // declare a new object to add data in a list
                    SearchDataViewModel deviceAssignedUser = new SearchDataViewModel();             // declare a new object to add data in a list
                    SearchDataViewModel deviceAssignedTeam = new SearchDataViewModel();             // declare a new object to add data in a list




                    deviceName.FirstURL = "/Devices/device";
                    deviceName.Result = query.Mac;
                    deviceName.Text = "Device Name";
                    deviceName.value = query.Name;
                    deviceName.Type = "MacInfo";
                    list.Add(deviceName); // add an Object in list object

                    deviceMAC.FirstURL = "/Devices/device";
                    deviceMAC.Result = query.Mac;
                    deviceMAC.Text = "MAC";
                    deviceMAC.value = query.Mac;
                    deviceMAC.Type = "MacInfo";
                    list.Add(deviceMAC); // add an Object in list object


                    deviceEMEI.FirstURL = "/Devices/device";
                    deviceEMEI.Result = query.Mac;
                    deviceEMEI.Text = "EMEI";
                    deviceEMEI.value = query.EMEINumber;
                    deviceEMEI.Type = "MacInfo";
                    list.Add(deviceEMEI); // add an Object in list object


                    deviceRegistrationDate.FirstURL = "/Devices/device";
                    deviceRegistrationDate.Result = query.Mac;
                    deviceRegistrationDate.Text = "Registration Date";
                    deviceRegistrationDate.value = query.RegistrationDate == null ? "" : query.RegistrationDate.Value.ToShortDateString();
                    deviceRegistrationDate.Type = "MacInfo";
                    list.Add(deviceRegistrationDate); // add an Object in list object


                    deviceLatitude.FirstURL = "/Devices/device";
                    deviceLatitude.Result = query.Mac;
                    deviceLatitude.Text = "Latitude";
                    deviceLatitude.value = query.Latitude == null ? "" : query.Latitude.ToString();
                    deviceLatitude.Type = "MacInfo";
                    list.Add(deviceLatitude); // add an Object in list object



                    deviceLongitude.FirstURL = "/Devices/device";
                    deviceLongitude.Result = query.Mac;
                    deviceLongitude.Text = "Longitude";
                    deviceLongitude.value = query.Longitude == null ? "" : query.Longitude.ToString();
                    deviceLongitude.Type = "MacInfo";
                    list.Add(deviceLongitude); // add an Object in list object



                    deviceStatus.FirstURL = "/Devices/device";
                    deviceStatus.Result = query.Mac;
                    deviceStatus.Text = "Status";
                    deviceStatus.value = query.Status == null ? "" : query.Status.Name.ToString();
                    deviceStatus.Type = "MacInfo";
                    list.Add(deviceStatus); // add an Object in list object


                    deviceAccountId.FirstURL = "/Devices/device";
                    deviceAccountId.Result = query.Mac;
                    deviceAccountId.Text = "Account";
                    deviceAccountId.value = dbEnt.Accounts.FirstOrDefault(x => x.AccountId == query.AccountId).Name;
                    deviceAccountId.Type = "MacInfo";
                    list.Add(deviceAccountId); // add an Object in list object


                    deviceAssignedUser.FirstURL = "/Devices/device";
                    deviceAssignedUser.Result = query.Mac;
                    deviceAssignedUser.Text = "Assigned User";
                    deviceAssignedUser.value = query.User.Name;
                    deviceAssignedUser.Type = "MacInfo";
                    list.Add(deviceAssignedUser); // add an Object in list object

                    deviceAssignedTeam.FirstURL = "/Devices/device";
                    deviceAssignedTeam.Result = query.Mac;
                    deviceAssignedTeam.Text = "Assigned Team";
                    deviceAssignedTeam.value = query.Team.Name;
                    deviceAssignedTeam.Type = "MacInfo";
                    list.Add(deviceAssignedTeam); // add an Object in list object

                }

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("DeviceDataOfMAC", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.DeviceDataOfMAC", "");
                throw ex;
            }

        }

        /// <summary>
        /// Leads data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> LeadsDataForSearch()
        {
            try
            {
                InsertEventLog("LeadsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Lead For Search", "TICRM.BusinessLayer.GlobalSearchManager.LeadsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object
                var query = dbEnt.Leads.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Lead.ToString()).Where(x => x.DataType == "String").ToList();
                SearchDataViewModel LeadCreate = new SearchDataViewModel();// declare a new object to add data in a list
                LeadCreate.Result = "<a href='/Leads/Create'>Create Lead</a>";
                LeadCreate.FirstURL = "/Leads/Create";
                LeadCreate.Text = "Leads > Create";
                LeadCreate.Type = "URL";
                LeadCreate.value = "Navigate to Lead Create Page.";
                list.Add(LeadCreate); // add an Object in list object
                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel leadDetail = new SearchDataViewModel(); // declare a new object to add data in a list
                    

                    leadDetail.Result = "<a href='/Leads/Details/" + item.LeadId + "'>Detail Lead</a>";
                    leadDetail.FirstURL = item.LeadId.ToString();
                    leadDetail.Text = "Leads > " + item.Name + " > Details";
                    leadDetail.Type = "Modal";
                    leadDetail.JS_function = "Leads_Details_Modal('"+ item.LeadId + "')";
                    list.Add(leadDetail); // add an Object in list object

                }
                

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("LeadsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.LeadsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Leads data for search lead.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> LeadsDataForSearch_lead(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("LeadsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Lead For Search", "TICRM.BusinessLayer.GlobalSearchManager.LeadsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object
                var query = dbEnt.Leads.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Lead.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel leadEdit = new SearchDataViewModel();   // declare a new object to add data in a list
                    SearchDataViewModel leadDetail = new SearchDataViewModel(); // declare a new object to add data in a list
                    leadEdit.Result = "<a href='/Leads/Edit/" + Id+ "'>Edit Lead</a>";
                    leadEdit.FirstURL = "/Leads/Edit/" + Id;
                    leadEdit.Text = "Leads > " + Name + " > Edit";
                    leadEdit.Type = "URL";
                    leadEdit.value = "Navigate to Edit Page.";
                    list.Add(leadEdit); // add an Object in list object

                    leadDetail.Result = "<a href='/Leads/Details/" + Id + "Detail Lead</a>";
                    leadDetail.FirstURL = Id.ToString();
                    leadDetail.Text = "Leads > " + Name + " > Details";
                    leadDetail.Type = "Modal";
                    leadDetail.JS_function = "Leads_Details_Modal('" + Id + "')";
                    list.Add(leadDetail); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("LeadsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.LeadsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Opportunities data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> OpportunitiesDataForSearch()
        {
            try
            {

                InsertEventLog("OpportunitiesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Opportunity For Search", "TICRM.BusinessLayer.GlobalSearchManager.OpportunitiesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object
                SearchDataViewModel OpportunitiesCreate = new SearchDataViewModel();// declare a new object to add data in a list
                OpportunitiesCreate.Result = "<a href='/Opportunities/Create'>Create Opportunity</a>";
                OpportunitiesCreate.FirstURL = "/Opportunities/Create";
                OpportunitiesCreate.Text = "Opportunities > Create";
                OpportunitiesCreate.Type = "URL";
                OpportunitiesCreate.value = "Navigate to Oppertunity Create Page.";
                list.Add(OpportunitiesCreate); // add an Object in list object
                var query = dbEnt.Opportunities.ToList();
                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel OpportunitiesDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    
                    OpportunitiesDetail.Result = "<a href='/Opportunities/Details/" + item.OpportunityId + "'>Detail Opportunities</a>";
                    OpportunitiesDetail.FirstURL = item.OpportunityId.ToString();
                    OpportunitiesDetail.Text = "Opportunities > " + item.Title;
                    OpportunitiesDetail.Type = "Modal";
                    OpportunitiesDetail.JS_function = "Opportunities_Details_Modal('" + item.OpportunityId + "')";
                    list.Add(OpportunitiesDetail); // add an Object in list object
                }
                

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("OpportunitiesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.OpportunitiesDataForSearch", "");

                throw ex;
            }
        }

        /// <summary>
        /// Opportunities data for search opp.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> OpportunitiesDataForSearch_opp()
        {
            try
            {

                InsertEventLog("OpportunitiesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Opportunity For Search", "TICRM.BusinessLayer.GlobalSearchManager.OpportunitiesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Opportunities.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Oppertunity.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel OpportunitiesEdit = new SearchDataViewModel();  // declare a new object to add data in a list
                    SearchDataViewModel OpportunitiesDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    OpportunitiesEdit.Result = "<a href='/Opportunities/Edit/" + item.OpportunityId + "'>Edit Opportunities</a>";
                    OpportunitiesEdit.FirstURL = "/Opportunities/Edit/" + item.OpportunityId;
                    OpportunitiesEdit.Text = "Opportunities > " + item.Title + " > Edit";
                    OpportunitiesEdit.Type = "URL";
                    OpportunitiesEdit.value = "Navigate to Oppertunity Edit Page.";
                    list.Add(OpportunitiesEdit); // add an Object in list object

                    OpportunitiesDetail.Result = "<a href='/Opportunities/Details/" + item.OpportunityId + "'>Detail Opportunities</a>";
                    OpportunitiesDetail.FirstURL = item.OpportunityId.ToString();
                    OpportunitiesDetail.Text = "Opportunities > " + item.Title+ " > Details";
                    OpportunitiesDetail.Type = "Modal";
                    OpportunitiesDetail.JS_function = "Opportunities_Details_Modal('" + item.OpportunityId + "')";
                    list.Add(OpportunitiesDetail); // add an Object in list object

                    foreach (workflowDataTypeDTO selected in attr)
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Opportunities/Edit/" + item.OpportunityId + "'>Edit Opportunities</a>";
                        s2.FirstURL = "/Opportunities/Edit/" + item.OpportunityId;
                        s2.Text = "Opportunities > " + item.Title + " > " + selected.ColumnName;
                        string value = (String)item.GetType().GetProperty(selected.ColumnName).GetValue(item);
                        s2.value = value;
                        s2.Type = "URL";
                        list.Add(s2); // add an Object in list object
                    }

                }
                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("OpportunitiesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.OpportunitiesDataForSearch", "");

                throw ex;
            }
        }

        /// <summary>
        /// Accounts data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AccountsDataForSearch()
        {
            try
            {
                InsertEventLog("AccountsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of account For Search", "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var accounts = dbEnt.Accounts.ToList();
                
                foreach (Account item in accounts.CollectionNotNull())
                {

                    SearchDataViewModel AccountsDetail_Page = new SearchDataViewModel();
                    AccountsDetail_Page.Result = "<a href='/Accounts/AccountsDetail/" + item.AccountId + "'>Edit Customer</a>";
                    AccountsDetail_Page.FirstURL = "/Accounts/AccountsDetail/" + item.AccountId;
                    AccountsDetail_Page.Text = "Accounts > " + item.Name + " > Details";
                    AccountsDetail_Page.Type = "URL";
                    list.Add(AccountsDetail_Page); // add an Object in list object

                }

                


                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Accounts data for search attribute.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AccountsDataForSearch_attribute()
        {
            try
            {
                InsertEventLog("AccountsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of account For Search", "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<Account> query = dbEnt.Accounts.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Account.ToString()).Where(x => x.DataType == "String").ToList();

                
                foreach (Account item in query.CollectionNotNull())
                {
                    SearchDataViewModel AccountsEdit = new SearchDataViewModel();
                    AccountsEdit.Result = "<a href='/Accounts/Edit/" + item.AccountId + "'>Edit Customer</a>";
                    AccountsEdit.FirstURL = "/Accounts/Edit/" + item.AccountId;
                    AccountsEdit.Text = "Accounts > " + item.Name + " > Edit";
                    AccountsEdit.Type = "URL";
                    AccountsEdit.value = "Navigate to Edit Account Page.";
                    list.Add(AccountsEdit); // add an Object in list object

                    SearchDataViewModel AccountsDetail_Page = new SearchDataViewModel();
                    AccountsDetail_Page.Result = "<a href='/Accounts/AccountsDetail/" + item.AccountId + "'>Edit Customer</a>";
                    AccountsDetail_Page.FirstURL = "/Accounts/AccountsDetail/" + item.AccountId;
                    AccountsDetail_Page.Text = "Accounts > " + item.Name + " > Details";
                    AccountsDetail_Page.Type = "URL";
                    list.Add(AccountsDetail_Page); // add an Object in list object

                    foreach (workflowDataTypeDTO selected in attr)
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Accounts/Edit/" + item.AccountId + "'>Edit Accounts</a>";
                        s2.FirstURL = "/Accounts/Edit/" + item.AccountId;
                        s2.Text = "Accounts > " + item.Name + " > " + selected.ColumnName;
                        string value = (String)item.GetType().GetProperty(selected.ColumnName).GetValue(item);
                        s2.value = value;
                        s2.Type = "URL";
                        list.Add(s2); // add an Object in list object
                    }

                }
                SearchDataViewModel AccountsCreate = new SearchDataViewModel();
                AccountsCreate.Result = "<a href='/Accounts/Create'>Create Accoount</a>";
                AccountsCreate.FirstURL = "/Accounts/Create";
                AccountsCreate.Text = "Accounts > Create";
                AccountsCreate.Type = "URL";
                AccountsCreate.value = "Navigate to Account Create Page.";
                list.Add(AccountsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }
        /// <summary>
        /// Accounts data for search accounts.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AccountsDataForSearch_accounts()
        {
            try
            {
                InsertEventLog("AccountsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of account For Search", "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<Account> query = dbEnt.Accounts.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Account.ToString()).Where(x => x.DataType == "String").ToList();


                foreach (Account item in query.CollectionNotNull())
                {
                    SearchDataViewModel AccountsDetail_Page = new SearchDataViewModel();
                    AccountsDetail_Page.Result = "<a href='/Accounts/AccountsDetail/" + item.AccountId + "'>Edit Customer</a>";
                    AccountsDetail_Page.FirstURL = "/Accounts/AccountsDetail/" + item.AccountId;
                    AccountsDetail_Page.Text = "Accounts > " + item.Name + " > Details";
                    AccountsDetail_Page.Type = "URL";
                    AccountsDetail_Page.value = "Navigate to the Account";
                    list.Add(AccountsDetail_Page); // add an Object in list object

                }
                SearchDataViewModel AccountsCreate = new SearchDataViewModel();
                AccountsCreate.Result = "<a href='/Accounts/Create'>Create Accoount</a>";
                AccountsCreate.FirstURL = "/Accounts/Create";
                AccountsCreate.Text = "Accounts > Create";
                AccountsCreate.Type = "URL";
                AccountsCreate.value = "Navigate to Account Create Page.";
                list.Add(AccountsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }



        /// <summary>
        /// Load All search items for free search
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> LoadAllSerachItems()
        {
            try
            {
                InsertEventLog("LoadAllSerachItems", EventType.Log, EventColor.yellow, "Get List Of Search Items", "TICRM.BusinessLayer.GlobalSearchManager.LoadAllSerachItems", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var accounts = dbEnt.Accounts.ToList();
                var devices = dbEnt.Devices.ToList();
                var leads = dbEnt.Leads.ToList();
                var opportunities = dbEnt.Opportunities.ToList();
                var customerAssets = dbEnt.CustomerAssets.ToList();
                var workOrders = dbEnt.WorkOrders.ToList();
                var locations = dbEnt.Locations.ToList();
                var alerts = dbEnt.Alerts.ToList();
                var workflows = dbEnt.WorkFlows.ToList();
                var workflowReports = dbEnt.WorkFlowReports.ToList();

                SearchDataViewModel DashboardPage = new SearchDataViewModel();
                DashboardPage.Result = "<a href='/Dashboard/Index/> Dashboard</a>";
                DashboardPage.FirstURL = "/Dashboard/Index/";
                DashboardPage.Text = "Dashboard";
                DashboardPage.Type = "URL";
                DashboardPage.value = "Dashboard";
                list.Add(DashboardPage); // add an Object in list object

                foreach (Account item in accounts.CollectionNotNull())
                {
                    SearchDataViewModel AccountsDetail_Page = new SearchDataViewModel();
                    AccountsDetail_Page.Result = "<a href='/Accounts/AccountsDetail/" + item.AccountId + "'>Edit Account</a>";
                    AccountsDetail_Page.FirstURL = "/Accounts/AccountsDetail/" + item.AccountId;
                    AccountsDetail_Page.Text = item.Name ;
                    AccountsDetail_Page.Type = "URL";
                    AccountsDetail_Page.value = "Account";
                    list.Add(AccountsDetail_Page); // add an Object in list object

                }

                foreach (var item in opportunities.CollectionNotNull())
                {
                    SearchDataViewModel OpportunitiesDetail = new SearchDataViewModel();// declare a new object to add data in a list

                    OpportunitiesDetail.Result = "<a href='/Opportunities/Details/" + item.OpportunityId + "'>Detail Opportunities</a>";
                    OpportunitiesDetail.FirstURL = item.OpportunityId.ToString();
                    OpportunitiesDetail.Text = item.Title;
                    OpportunitiesDetail.value = "Opportunity";
                    OpportunitiesDetail.Type = "Modal";
                    OpportunitiesDetail.JS_function = "Opportunities_Details_Modal('" + item.OpportunityId + "')";
                    list.Add(OpportunitiesDetail); // add an Object in list object
                }

                foreach (var item in devices.CollectionNotNull())
                {
                    //    // body of foreach loop
                    SearchDataViewModel deviceDetails = new SearchDataViewModel();  // declare a new object to add data for search

                    deviceDetails.Result = "<a href='/Devices/Details/" + item.DeviceId + "'>Device</a>";
                    deviceDetails.FirstURL = item.DeviceId.ToString();
                    deviceDetails.Text = item.Name;
                    deviceDetails.value = "Device";
                    deviceDetails.Type = "Modal";
                    deviceDetails.JS_function = "Devices_Details_Modal('" + item.DeviceId + "')";
                    list.Add(deviceDetails); // add an Object in list object

                }

                foreach (var item in leads.CollectionNotNull())
                {
                    SearchDataViewModel leadDetail = new SearchDataViewModel(); // declare a new object to add data in a list

                    leadDetail.Result = "<a href='/Leads/Details/" + item.LeadId + "'>Detail Lead</a>";
                    leadDetail.FirstURL = item.LeadId.ToString();
                    leadDetail.Text = item.Name;
                    leadDetail.value = "Lead";
                    leadDetail.Type = "Modal";
                    leadDetail.JS_function = "Leads_Details_Modal('" + item.LeadId + "')";
                    list.Add(leadDetail); // add an Object in list object
                }

                foreach (CustomerAsset item in customerAssets.CollectionNotNull())
                {
                    SearchDataViewModel CustomerAssetsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    CustomerAssetsDetail.Result = "<a href='/CustomerAssets/Details/" + item.CustomerAssetId + "'>Detail Customer Assets</a>";
                    CustomerAssetsDetail.FirstURL = item.CustomerAssetId.ToString();
                    CustomerAssetsDetail.Text = item.Title;
                    CustomerAssetsDetail.value = "CustomerAsset";
                    CustomerAssetsDetail.Type = "Modal";
                    CustomerAssetsDetail.JS_function = "CustomerAssets_Details_Modal('" + item.CustomerAssetId + "')";
                    list.Add(CustomerAssetsDetail); // add an Object in list object
                }

                foreach (var item in workOrders.CollectionNotNull())
                {
                    SearchDataViewModel WorkOrdersDetail = new SearchDataViewModel();   // declare a new object to add data in a list

                    WorkOrdersDetail.Result = "<a href='/WorkOrders/Details/" + item.WorkOrderId + "'>Detail WorkOrders</a>";
                    WorkOrdersDetail.FirstURL = item.WorkOrderId.ToString();
                    WorkOrdersDetail.Text = item.Title;
                    WorkOrdersDetail.value = "WorkOrder";
                    WorkOrdersDetail.Type = "Modal";
                    WorkOrdersDetail.JS_function = "WorkOrders_Details_Modal('" + item.WorkOrderId + "')";
                    list.Add(WorkOrdersDetail); // add an Object in list object
                }

                foreach (var item in locations.CollectionNotNull())
                {
                    SearchDataViewModel LocationsDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    LocationsDetail.Result = "<a href='/Locations/Details/" + item.LocationId + "'>Detail Locations</a>";
                    LocationsDetail.FirstURL = item.LocationId.ToString();
                    LocationsDetail.Text = item.Name;
                    LocationsDetail.value = "Location";
                    LocationsDetail.Type = "Modal";
                    LocationsDetail.JS_function = "Locations_Details_Modal('" + item.LocationId + "')";
                    list.Add(LocationsDetail); // add an Object in list object
                }

                foreach (var item in alerts.CollectionNotNull())
                {
                    SearchDataViewModel AlertsDetail = new SearchDataViewModel();   // declare a new object to add data in a list

                    AlertsDetail.Result = "<a href='/Alerts/Details/" + item.AlertId + "'>Detail Alerts</a>";
                    AlertsDetail.FirstURL = item.AlertId.ToString();
                    AlertsDetail.Text = item.Title;
                    AlertsDetail.value = "Alert";
                    AlertsDetail.Type = "Modal";
                    AlertsDetail.JS_function = "Alerts_Details_Modal('" + item.AlertId + "')";
                    list.Add(AlertsDetail); // add an Object in list object
                }

                foreach (var item in workflowReports.CollectionNotNull())
                {
                    SearchDataViewModel WorkFlowReportsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkFlowReportsDetail.Result = "<a href='/WorkFlowReports/Details/" + item.WorkFlowReportId + "'>Detail WorkFlowReports</a>";
                    WorkFlowReportsDetail.FirstURL = item.WorkFlowReportId.ToString();
                    WorkFlowReportsDetail.Text = item.WorkFlow.Name ;
                    WorkFlowReportsDetail.value = "WorkflowReport" ;
                    WorkFlowReportsDetail.Type = "Modal";
                    WorkFlowReportsDetail.JS_function = "WorkFlowReports_Details_Modal('" + item.WorkFlowReportId + "')";
                    list.Add(WorkFlowReportsDetail); // add an Object in list object
                }

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("LoadAllSerachItems", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.LoadAllSerachItems", "");
                throw ex;
            }
        }



        /// <summary>
        /// Code by Akhtar Zaman
        /// 9/7/2020
        /// funtion gets the module's detial and edit page options
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns> a list of pages params</returns>
        public List<SearchDataViewModel> AccountsDetailsForSearch(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("AccountsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of account For Search", "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object
               
                

                SearchDataViewModel accountEdit = new SearchDataViewModel();
                accountEdit.Result = "<a href='/Accounts/Eidt/" + Id + "'>Edit Account</a>";
                accountEdit.FirstURL = "/Accounts/AccountsDetail/" + Id;
                accountEdit.Text = "Accounts > " + Name.TrimStart() + " > Details > Edit";
                accountEdit.Type = "URL";
                list.Add(accountEdit); // add an Object in list object

                SearchDataViewModel AccountAsscociates_Opp = new SearchDataViewModel();
                AccountAsscociates_Opp.Result = "<a href='/Accounts/AccountsDetail/" + Id + "'>List Opportunities</a>";
                AccountAsscociates_Opp.FirstURL = "/Accounts/AccountsDetail/" + Id;
                AccountAsscociates_Opp.Text = "Accounts  > " + Name.TrimStart() + " > Details > Opportunities";
                AccountAsscociates_Opp.Type = "Associates";
                list.Add(AccountAsscociates_Opp);

                SearchDataViewModel AccountAsscociates_Assets = new SearchDataViewModel();
                AccountAsscociates_Assets.Result = "<a href='/Accounts/AccountsDetail/" + Id + "'>List Customer Assets</a>";
                AccountAsscociates_Assets.FirstURL = "/Accounts/AccountsDetail/" + Id;
                AccountAsscociates_Assets.Text = "Accounts > " + Name.TrimStart() + " > Details > CustomerAssets";
                AccountAsscociates_Assets.Type = "Associates";
                list.Add(AccountAsscociates_Assets);

                SearchDataViewModel AccountAsscociates_Devices = new SearchDataViewModel();
                AccountAsscociates_Devices.Result = "<a href='/Accounts/AccountsDetail/" + Id + "'>List Devices</a>";
                AccountAsscociates_Devices.FirstURL = "/Accounts/AccountsDetail/" + Id;
                AccountAsscociates_Devices.Text = "Accounts > " + Name.TrimStart() + " > Details > Devices";
                AccountAsscociates_Devices.Type = "Associates";
                list.Add(AccountAsscociates_Devices);

                SearchDataViewModel AccountAsscociates_Locations = new SearchDataViewModel();
                AccountAsscociates_Locations.Result = "<a href='/Accounts/AccountsDetail/" + Id + "'>List Locations</a>";
                AccountAsscociates_Locations.FirstURL = "/Accounts/AccountsDetail/" + Id;
                AccountAsscociates_Locations.Text = "Accounts > " + Name.TrimStart() + " > Details > Locations";
                AccountAsscociates_Locations.Type = "Associates";
                list.Add(AccountAsscociates_Locations);


                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Assets details for search.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AssetsDetailsForSearch(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("AssetsDetailsForSearch", EventType.Log, EventColor.yellow, "Get List Of pages for specific Asset For nav", "TICRM.BusinessLayer.GlobalSearchManager.AssetsDetailsForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                SearchDataViewModel CustomerAssetsEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                SearchDataViewModel CustomerAssetsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                CustomerAssetsEdit.Result = "<a href='/CustomerAssets/Edit/" + Id + "'>Edit Customer Assets</a>";
                CustomerAssetsEdit.FirstURL = "/CustomerAssets/Edit/" + Id;
                CustomerAssetsEdit.Text = "CustomerAssets > " + Name.TrimStart() + " > Edit";
                CustomerAssetsEdit.Type = "URL";
                CustomerAssetsEdit.value = "Navigate to Edit Page.";
                list.Add(CustomerAssetsEdit); // add an Object in list object

                CustomerAssetsDetail.Result = "<a href='/CustomerAssets/Details/" + Id + "'>Detail Customer Assets</a>";
                CustomerAssetsDetail.FirstURL = Id.ToString();
                CustomerAssetsDetail.Text = "CustomerAssets > " + Name.TrimStart() + " > Details";
                CustomerAssetsDetail.Type = "Modal";
                CustomerAssetsDetail.JS_function = "CustomerAssets_Details_Modal('" + Id + "')";
                list.Add(CustomerAssetsDetail); // add an Object in list object


                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Opps the details for search.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> OppDetailsForSearch(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("OpportunitiesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Opportunity For Search", "TICRM.BusinessLayer.GlobalSearchManager.OpportunitiesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Opportunities.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Oppertunity.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel OpportunitiesEdit = new SearchDataViewModel();  // declare a new object to add data in a list
                    SearchDataViewModel OpportunitiesDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    OpportunitiesEdit.Result = "<a href='/Opportunities/Edit/" + Id + "'>Edit Opportunities</a>";
                    OpportunitiesEdit.FirstURL = "/Opportunities/Edit/" + Id;
                    OpportunitiesEdit.Text = "Opportunities > " + Name.TrimStart() + " > Edit";
                    OpportunitiesEdit.Type = "URL";
                    OpportunitiesEdit.value = "Navigate to Oppertunity Edit Page.";
                    list.Add(OpportunitiesEdit); // add an Object in list object

                    OpportunitiesDetail.Result = "<a href='/Opportunities/Details/" + Id + "'>Detail Opportunities</a>";
                    OpportunitiesDetail.FirstURL = Id.ToString();
                    OpportunitiesDetail.Text = "Opportunities > " + Name.TrimStart() + " > Details";
                    OpportunitiesDetail.Type = "Modal";
                    OpportunitiesDetail.JS_function = "Opportunities_Details_Modal('" + Id + "')";
                    list.Add(OpportunitiesDetail); // add an Object in list object
                
                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Gets the accounts detail for search.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> GetAccountsDetailForSearch(Guid accountId, string Module, string Name)
        {
            try
            {
                InsertEventLog("AccountsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of account For Search", "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                AccountManager account = new AccountManager();
                var account_associate = account.Get_Account_Associates(accountId);
                if (Module.Contains("Opportunities"))
                {
                    List<workflowDataTypeDTO> Opportunity_attr = GetAttributesOfEntity(EntityTypes.Oppertunity.ToString()).Where(x => x.DataType == "String").ToList();
                    foreach (var item in account_associate.accountOppertunities.CollectionNotNull())
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Opportunities/Details/" + item.OpportunityId + "'>Detail Opportunities</a>";
                        s2.FirstURL = item.OpportunityId.ToString();
                        s2.Text = "Accounts > " + account_associate.account.Name + " > Details > Opportunities > " + item.Title + " > Details";
                        
                        s2.value = "View Detials";
                        s2.JS_function = "Opportunities_Details_Modal('" + item.OpportunityId + "')";

                        s2.Type = "Modal";
                        list.Add(s2); // add an Object in list object

                    }
                }
                if (Module.Contains("CustomerAssets"))
                {
                    List<workflowDataTypeDTO> CustomerAssets_attr = GetAttributesOfEntity(EntityTypes.CustomerAsset.ToString()).Where(x => x.DataType == "String").ToList();

                    foreach (var item in account_associate.accountAssetes.CollectionNotNull())
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/CustomerAssets/Details/" + item.AccountId + "'>Edit CustomerAssets</a>";
                        s2.FirstURL = item.CustomerAssetId.ToString();
                        s2.Text = "Accounts > " + account_associate.account.Name + " > Details > CustomerAssets > " + item.Title + " > Details";
                        s2.value = "View Details";
                        s2.Type = "Modal";
                        s2.JS_function = "CustomerAssets_Details_Modal('" + item.CustomerAssetId + "')";

                        list.Add(s2); // add an Object in list object

                    }

                }
                if (Module.Contains("Devices"))
                {
                    List<workflowDataTypeDTO> Device_attr = GetAttributesOfEntity(EntityTypes.Device.ToString()).Where(x => x.DataType == "String").ToList();

                    foreach (var item in account_associate.accountDevices.CollectionNotNull())
                    {
                        //Devices_Details_Modal
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Devices/details/" + item.DeviceId + "'>Edit Devices</a>";
                        s2.FirstURL = item.DeviceId.ToString();
                        s2.Text = "Accounts > " + account_associate.account.Name + " > Details > Device > " + item.Name + " > Details ";
                        s2.value = "View Details";
                        s2.Type = "Modal";
                        s2.JS_function = "Devices_Details_Modal('" + item.DeviceId + "')";

                        list.Add(s2); // add an Object in list object
                       
                    }

                }

                if (Module.Contains("Locations"))
                {
                    List<workflowDataTypeDTO> Location_attr = GetAttributesOfEntity(EntityTypes.Location.ToString()).Where(x => x.DataType == "String").ToList();

                    foreach (var item in account_associate.accountLocations.CollectionNotNull())
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Locations/details/" + item.LocationId + "'>Edit Locations</a>";
                        s2.FirstURL = item.LocationId.ToString();
                        s2.Text = "Accounts > " + account_associate.account.Name + " > Details > Location > " + item.Name + " > Details ";
                        s2.value = "View Details";
                        s2.Type = "Modal";
                        s2.JS_function = "Locations_Details_Modal('" + item.LocationId + "')";

                        list.Add(s2); // add an Object in list object

                    }

                }

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("AccountsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AccountsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Customer assets data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> CustomerAssetsDataForSearch()
        {
            try
            {
                InsertEventLog("CustomerAssetsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of customer Assets For Search", "TICRM.BusinessLayer.GlobalSearchManager.CustomerAssetsDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<CustomerAsset> query = dbEnt.CustomerAssets.ToList();

                foreach (CustomerAsset item in query.CollectionNotNull())
                {
                    SearchDataViewModel CustomerAssetsDetail = new SearchDataViewModel();   // declare a new object to add data in a list

                    CustomerAssetsDetail.Result = "<a href='/CustomerAssets/Details/" + item.CustomerAssetId + "'>Detail Customer Assets</a>";
                    CustomerAssetsDetail.FirstURL = item.CustomerAssetId.ToString();
                    CustomerAssetsDetail.Text = "CustomerAssets > " + item.Title;
                    CustomerAssetsDetail.Type = "Modal";
                    CustomerAssetsDetail.JS_function = "CustomerAssets_Details_Modal('" + item.CustomerAssetId + "')";
                    list.Add(CustomerAssetsDetail); // add an Object in list object
                }
                SearchDataViewModel CustomerAssetsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                CustomerAssetsCreate.Result = "<a href='/CustomerAssets/Create'>Create Customer Assets</a>";
                CustomerAssetsCreate.FirstURL = "/CustomerAssets/Create";
                CustomerAssetsCreate.Text = "CustomerAssets > Create";
                CustomerAssetsCreate.Type = "URL";
                CustomerAssetsCreate.value = "Navigate to Create Page.";
                list.Add(CustomerAssetsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("CustomerAssetsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.CustomerAssetsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Customer assets data for search customer asset.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> CustomerAssetsDataForSearch_custAsset()
        {
            try
            {
                InsertEventLog("CustomerAssetsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of customer Assets For Search", "TICRM.BusinessLayer.GlobalSearchManager.CustomerAssetsDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<CustomerAsset> query = dbEnt.CustomerAssets.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.CustomerAsset.ToString()).Where(x => x.DataType == "String").ToList();


                foreach (CustomerAsset item in query.CollectionNotNull())
                {
                    SearchDataViewModel CustomerAssetsEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel CustomerAssetsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    CustomerAssetsEdit.Result = "<a href='/CustomerAssets/Edit/" + item.CustomerAssetId + "'>Edit Customer Assets</a>";
                    CustomerAssetsEdit.FirstURL = "/CustomerAssets/Edit/" + item.CustomerAssetId;
                    CustomerAssetsEdit.Text = "CustomerAssets > " + item.Title+ " > Edit";
                    CustomerAssetsEdit.Type = "URL";
                    CustomerAssetsEdit.value = "Navigate to Edit Page.";
                    list.Add(CustomerAssetsEdit); // add an Object in list object

                    CustomerAssetsDetail.Result = "<a href='/CustomerAssets/Details/" + item.CustomerAssetId + "'>Detail Customer Assets</a>";
                    CustomerAssetsDetail.FirstURL = item.CustomerAssetId.ToString();
                    CustomerAssetsDetail.Text = "CustomerAssets > " + item.Title+ " > Details";
                    CustomerAssetsDetail.Type = "Modal";
                    CustomerAssetsDetail.JS_function = "CustomerAssets_Details_Modal('" + item.CustomerAssetId + "')";
                    list.Add(CustomerAssetsDetail); // add an Object in list object

                    foreach (workflowDataTypeDTO selected in attr.CollectionNotNull())
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/CustomerAssets/Edit/" + item.AccountId + "'>Edit CustomerAssets</a>";
                        s2.FirstURL = "/CustomerAssets/Edit/" + item.AccountId;
                        s2.Text = "CustomerAssets > " + item.Title + " > " + selected.ColumnName;
                        string value = (String)item.GetType().GetProperty(selected.ColumnName).GetValue(item);
                        s2.value = value;
                        s2.Type = "URL";
                        list.Add(s2); // add an Object in list object
                    }

                }
                SearchDataViewModel CustomerAssetsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                CustomerAssetsCreate.Result = "<a href='/CustomerAssets/Create'>Create Customer Assets</a>";
                CustomerAssetsCreate.FirstURL = "/CustomerAssets/Create";
                CustomerAssetsCreate.Text = "CustomerAssets > Create";
                CustomerAssetsCreate.Type = "URL";
                CustomerAssetsCreate.value = "Navigate to Create Page.";
                list.Add(CustomerAssetsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("CustomerAssetsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.CustomerAssetsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Readings data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ReadingsDataForSearch()
        {
            try
            {
                InsertEventLog("ReadingsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Reading For Search", "TICRM.BusinessLayer.GlobalSearchManager.ReadingsDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<Reading> query = dbEnt.Readings.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Reading.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel ReadingsDetail = new SearchDataViewModel();     // declare a new object to add data in a list
                    
                    ReadingsDetail.Result = "<a href='/Readings/Details/" + item.ReadingId + "'>Detail Reading Name</a>";
                    ReadingsDetail.FirstURL = item.ReadingId.ToString();
                    ReadingsDetail.Text = "Readings > " + item.Value;
                    ReadingsDetail.Type = "Modal";
                    ReadingsDetail.JS_function = "Readings_Details_Modal('" + item.ReadingId + "')";
                    list.Add(ReadingsDetail); // add an Object in list object
                    
                }
                SearchDataViewModel ReadingsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                ReadingsCreate.Result = "<a href='/Readings/Create'>Create Reading Name</a>";
                ReadingsCreate.FirstURL = "/Readings/Create";
                ReadingsCreate.Text = "Readings > Create";
                ReadingsCreate.Type = "URL";
                ReadingsCreate.value = "Navigate to Create Page.";
                list.Add(ReadingsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("ReadingsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ReadingsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Readings data for search read.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ReadingsDataForSearch_read(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("ReadingsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Reading For Search", "TICRM.BusinessLayer.GlobalSearchManager.ReadingsDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<Reading> query = dbEnt.Readings.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Reading.ToString()).Where(x => x.DataType == "String").ToList();

                    SearchDataViewModel ReadingsEdit = new SearchDataViewModel();       // declare a new object to add data in a list
                    SearchDataViewModel ReadingsDetail = new SearchDataViewModel();     // declare a new object to add data in a list
                    ReadingsEdit.Result = "<a href='/Readings/Edit/" + Id + "'>Edit Reading Name</a>";
                    ReadingsEdit.FirstURL = "/Readings/Edit/" + Id;
                    ReadingsEdit.Text = "Readings > " + Name.TrimStart() + " > Edit";
                    ReadingsEdit.Type = "URL";
                    ReadingsEdit.value = "Navigate to Edit Page.";
                    list.Add(ReadingsEdit); // add an Object in list object

                    ReadingsDetail.Result = "<a href='/Readings/Details/" + Id + "'>Detail Reading Name</a>";
                    ReadingsDetail.FirstURL = Id.ToString();
                    ReadingsDetail.Text = "Readings > " + Name + " > Details";
                    ReadingsDetail.Type = "Modal";
                    ReadingsDetail.JS_function = "Readings_Details_Modal('" + Id + "')";
                    list.Add(ReadingsDetail); // add an Object in list object

              

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ReadingsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ReadingsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Service calls data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ServiceCallsDataForSearch()
        {
            try
            {
                InsertEventLog("ServiceCallsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Service call For Search", "TICRM.BusinessLayer.GlobalSearchManager.ServiceCallsDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.ServiceCalls.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.ServiceCall.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel ServiceCallsDetail = new SearchDataViewModel(); // declare a new object to add data in a list
                   

                    ServiceCallsDetail.Result = "<a href='/ServiceCalls/Details/" + item.ServiceCallId + "'>Detail Reading Name</a>";
                    ServiceCallsDetail.FirstURL = item.ServiceCallId.ToString();
                    ServiceCallsDetail.Text = "ServiceCalls > " + item.Title;
                    ServiceCallsDetail.Type = "Modal";
                    ServiceCallsDetail.JS_function = "ServiceCalls_Details_Modal('" + item.ServiceCallId + "')";
                    list.Add(ServiceCallsDetail); // add an Object in list object

                }
                SearchDataViewModel ServiceCallsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                ServiceCallsCreate.Result = "<a href='/ServiceCalls/Create'>Create Reading Name</a>";
                ServiceCallsCreate.FirstURL = "/ServiceCalls/Create";
                ServiceCallsCreate.Text = "ServiceCalls > Create";
                ServiceCallsCreate.Type = "URL";
                ServiceCallsCreate.value = "Navigate to Create Page.";
                list.Add(ServiceCallsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ServiceCallsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ServiceCallsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Service calls data for search service.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ServiceCallsDataForSearch_service(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("ServiceCallsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Service call For Search", "TICRM.BusinessLayer.GlobalSearchManager.ServiceCallsDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.ServiceCalls.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.ServiceCall.ToString()).Where(x => x.DataType == "String").ToList();
                    SearchDataViewModel ServiceCallsEdit = new SearchDataViewModel();   // declare a new object to add data in a list
                    SearchDataViewModel ServiceCallsDetail = new SearchDataViewModel(); // declare a new object to add data in a list
                    ServiceCallsEdit.Result = "<a href='/ServiceCalls/Edit/" + Id + "'>Edit Reading Name</a>";
                    ServiceCallsEdit.FirstURL = "/ServiceCalls/Edit/" + Id;
                    ServiceCallsEdit.Text = "ServiceCalls > " + Name.TrimStart() + " > Edit";
                    ServiceCallsEdit.Type = "URL";
                    ServiceCallsEdit.value = "Navigate to Edit Page.";
                    list.Add(ServiceCallsEdit); // add an Object in list object

                    ServiceCallsDetail.Result = "<a href='/ServiceCalls/Details/" + Id + "'>Detail Reading Name</a>";
                    ServiceCallsDetail.FirstURL = Id.ToString();
                    ServiceCallsDetail.Text = "ServiceCalls > " + Name.TrimStart() + " > Details";
                    ServiceCallsDetail.Type = "Modal";
                    ServiceCallsDetail.JS_function = "ServiceCalls_Details_Modal('" + Id + "')";
                    list.Add(ServiceCallsDetail); // add an Object in list object

                
                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ServiceCallsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ServiceCallsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Resources data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ResourcesDataForSearch()
        {
            try
            {
                InsertEventLog("ResourcesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Resources For Search", "TICRM.BusinessLayer.GlobalSearchManager.ResourcesDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Resources.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Resource.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel ResourcesDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    
                    ResourcesDetail.Result = "<a href='/Resources/Details/" + item.ResourceId + "'>Detail Resources Name</a>";
                    ResourcesDetail.FirstURL = item.ResourceId.ToString();
                    ResourcesDetail.Text = "Resources > " + item.Name+ " > Details";
                    ResourcesDetail.Type = "Modal";
                    ResourcesDetail.JS_function = "Resources_Details_Modal('" + item.ResourceId + "')";
                    list.Add(ResourcesDetail);

                }
                SearchDataViewModel ResourcesCreate = new SearchDataViewModel();// declare a new object to add data in a list
                ResourcesCreate.Result = "<a href='/Resources/Create'>Create Resources Name</a>";
                ResourcesCreate.FirstURL = "/Resources/Create";
                ResourcesCreate.Text = "Resources > Create";
                ResourcesCreate.Type = "URL";
                ResourcesCreate.value = "Navigate to Create Page.";
                list.Add(ResourcesCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ResourcesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ResourcesDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Resources data for search resource.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ResourcesDataForSearch_resource(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("ResourcesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Resources For Search", "TICRM.BusinessLayer.GlobalSearchManager.ResourcesDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Resources.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Resource.ToString()).Where(x => x.DataType == "String").ToList();

                    SearchDataViewModel ResourcesEdit = new SearchDataViewModel();  // declare a new object to add data in a list
                    SearchDataViewModel ResourcesDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    ResourcesEdit.Result = "<a href='/Resources/Edit/" + Id + "'>Edit Resources Name</a>";
                    ResourcesEdit.FirstURL = "/Resources/Edit/" + Id;
                    ResourcesEdit.Text = "Resources > " + Name.TrimStart() + " > Edit";
                    ResourcesEdit.Type = "URL";
                    ResourcesEdit.value = "Navigate to Edit Page.";
                    list.Add(ResourcesEdit); // add an Object in list object

                    ResourcesDetail.Result = "<a href='/Resources/Details/" + Id+ "'>Detail Resources Name</a>";
                    ResourcesDetail.FirstURL = Id.ToString();
                    ResourcesDetail.Text = "Resources > " + Name.TrimStart() + " > Details";
                    ResourcesDetail.Type = "Modal";
                    ResourcesDetail.JS_function = "Resources_Details_Modal('" + Id+ "')";
                    list.Add(ResourcesDetail);
              
                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ResourcesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ResourcesDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work orders data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkOrdersDataForSearch()
        {
            try
            {
                InsertEventLog("WorkOrdersDataForSearch", EventType.Log, EventColor.yellow, "Get List Of work Order For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkOrdersDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkOrders.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkOrder.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel WorkOrdersDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                 
                    WorkOrdersDetail.Result = "<a href='/WorkOrders/Details/" + item.WorkOrderId + "'>Detail WorkOrders</a>";
                    WorkOrdersDetail.FirstURL = item.WorkOrderId.ToString();
                    WorkOrdersDetail.Text = "WorkOrders > " + item.Title;
                    WorkOrdersDetail.Type = "Modal";
                    WorkOrdersDetail.JS_function = "WorkOrders_Details_Modal('" + item.WorkOrderId + "')";
                    list.Add(WorkOrdersDetail); // add an Object in list object
                    
                }
                SearchDataViewModel WorkOrdersCreate = new SearchDataViewModel();// declare a new object to add data in a list
                WorkOrdersCreate.Result = "<a href='/WorkOrders/Create'>Create WorkOrders</a>";
                WorkOrdersCreate.FirstURL = "/WorkOrders/Create";
                WorkOrdersCreate.Text = "WorkOrders > Create";
                WorkOrdersCreate.Type = "URL";
                WorkOrdersCreate.value = "Navigate to Create Page.";
                list.Add(WorkOrdersCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("WorkOrdersDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkOrdersDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work orders data for search workorders.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkOrdersDataForSearch_workorders(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("WorkOrdersDataForSearch", EventType.Log, EventColor.yellow, "Get List Of work Order For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkOrdersDataForSearch", "");

                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkOrders.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkOrder.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel WorkOrdersEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel WorkOrdersDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkOrdersEdit.Result = "<a href='/WorkOrders/Edit/" + Id + "'>Edit WorkOrders</a>";
                    WorkOrdersEdit.FirstURL = "/WorkOrders/Edit/" + Id;
                    WorkOrdersEdit.Text = "WorkOrders > " + Name.TrimStart() + " > Edit";
                    WorkOrdersEdit.Type = "URL";
                    WorkOrdersEdit.value = "Navigate to Edit Page.";
                    list.Add(WorkOrdersEdit); // add an Object in list object

                    WorkOrdersDetail.Result = "<a href='/WorkOrders/Details/" + Id + "'>Detail WorkOrders</a>";
                    WorkOrdersDetail.FirstURL = Id.ToString();
                    WorkOrdersDetail.Text = "WorkOrders > " + Name.TrimStart() + " > Details";
                    WorkOrdersDetail.Type = "Modal";
                    WorkOrdersDetail.JS_function = "WorkOrders_Details_Modal('" + Id + "')";
                    list.Add(WorkOrdersDetail); // add an Object in list object
              

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("WorkOrdersDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkOrdersDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Readings the types data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ReadingTypesDataForSearch()
        {
            try
            {
                InsertEventLog("ReadingTypesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Reading Type For Search", "TICRM.BusinessLayer.GlobalSearchManager.ReadingTypesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.ReadingTypes.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.ReadingType.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel ReadingTypesDetail = new SearchDataViewModel();     // declare a new object to add data in a list
                    ReadingTypesDetail.Result = "<a href='/ReadingTypes/Details/" + item.ReadingTypeId + "'>Detail ReadingTypes</a>";
                    ReadingTypesDetail.FirstURL = item.ReadingTypeId.ToString();
                    ReadingTypesDetail.Text = "ReadingTypes > " + item.Name;
                    ReadingTypesDetail.Type = "Modal";
                    ReadingTypesDetail.JS_function = "ReadingTypes_Details_Modal('" + item.ReadingTypeId + "')";
                    list.Add(ReadingTypesDetail); // add an Object in list object
                   
                }
                SearchDataViewModel ReadingTypesCreate = new SearchDataViewModel();// declare a new object to add data in a list
                ReadingTypesCreate.Result = "<a href='/ReadingTypes/Create'>Create ReadingTypes</a>";
                ReadingTypesCreate.FirstURL = "/ReadingTypes/Create";
                ReadingTypesCreate.Text = "ReadingTypes > Create";
                ReadingTypesCreate.Type = "URL";
                ReadingTypesCreate.value = "Navigate to Create Page.";
                list.Add(ReadingTypesCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ReadingTypesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ReadingTypesDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Reading types data for search reading t.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ReadingTypesDataForSearch_readingT(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("ReadingTypesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Reading Type For Search", "TICRM.BusinessLayer.GlobalSearchManager.ReadingTypesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.ReadingTypes.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.ReadingType.ToString()).Where(x => x.DataType == "String").ToList();

                    SearchDataViewModel ReadingTypesEdit = new SearchDataViewModel();       // declare a new object to add data in a list
                    SearchDataViewModel ReadingTypesDetail = new SearchDataViewModel();     // declare a new object to add data in a list
                    ReadingTypesEdit.Result = "<a href='/ReadingTypes/Edit/" + Id + "'>Edit ReadingTypes</a>";
                    ReadingTypesEdit.FirstURL = "/ReadingTypes/Edit/" + Id;
                    ReadingTypesEdit.Text = "ReadingTypes > " + Name.TrimStart() + " > Edit";
                    ReadingTypesEdit.Type = "URL";
                    ReadingTypesEdit.value = "Navigate to Edit Page.";
                    list.Add(ReadingTypesEdit); // add an Object in list object

                    ReadingTypesDetail.Result = "<a href='/ReadingTypes/Details/" + Id + "'>Detail ReadingTypes</a>";
                    ReadingTypesDetail.FirstURL = Id.ToString();
                    ReadingTypesDetail.Text = "ReadingTypes > " + Name.TrimStart() + " > Details";
                    ReadingTypesDetail.Type = "Modal";
                    ReadingTypesDetail.JS_function = "ReadingTypes_Details_Modal('" + Id + "')";
                    list.Add(ReadingTypesDetail); // add an Object in list object
                    
                

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ReadingTypesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ReadingTypesDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Readings units data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ReadingUnitsDataForSearch()
        {
            try
            {
                InsertEventLog("ReadingUnitsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Reading Units For Search", "TICRM.BusinessLayer.GlobalSearchManager.ReadingUnitsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.ReadingUnits.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.ReadingUnit.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel ReadingUnitsDetail = new SearchDataViewModel();     // declare a new object to add data in a list
                   
                    ReadingUnitsDetail.Result = "<a href='/ReadingUnits/Details/" + item.ReadingUnitId + "'>Detail ReadingUnits</a>";
                    ReadingUnitsDetail.FirstURL = item.ReadingUnitId.ToString();
                    ReadingUnitsDetail.Text = "ReadingUnits > " + item.Name;
                    ReadingUnitsDetail.Type = "Modal";
                    ReadingUnitsDetail.JS_function = "ReadingUnits_Details_Modal('" + item.ReadingUnitId + "')";
                    list.Add(ReadingUnitsDetail); // add an Object in list object
                    
                }
                SearchDataViewModel ReadingUnitsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                ReadingUnitsCreate.Result = "<a href='/ReadingUnits/Create'>Create ReadingUnits</a>";
                ReadingUnitsCreate.FirstURL = "/ReadingUnits/Create";
                ReadingUnitsCreate.Text = "ReadingUnits > Create";
                ReadingUnitsCreate.Type = "URL";
                ReadingUnitsCreate.value = "Navigate to Create Page.";
                list.Add(ReadingUnitsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ReadingUnitsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ReadingUnitsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Readings units data for search reading u.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> ReadingUnitsDataForSearch_readingU(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("ReadingUnitsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Reading Units For Search", "TICRM.BusinessLayer.GlobalSearchManager.ReadingUnitsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.ReadingUnits.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.ReadingUnit.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel ReadingUnitsEdit = new SearchDataViewModel();       // declare a new object to add data in a list
                    SearchDataViewModel ReadingUnitsDetail = new SearchDataViewModel();     // declare a new object to add data in a list
                    ReadingUnitsEdit.Result = "<a href='/ReadingUnits/Edit/" + Id + "'>Edit ReadingUnits</a>";
                    ReadingUnitsEdit.FirstURL = "/ReadingUnits/Edit/" + Id;
                    ReadingUnitsEdit.Text = "ReadingUnits > " + Name.TrimStart() + " > Edit";
                    ReadingUnitsEdit.Type = "URL";
                    ReadingUnitsEdit.value = "Navigate to Edit Page.";
                    list.Add(ReadingUnitsEdit); // add an Object in list object

                    ReadingUnitsDetail.Result = "<a href='/ReadingUnits/Details/" + Id + "'>Detail ReadingUnits</a>";
                    ReadingUnitsDetail.FirstURL = Id.ToString();
                    ReadingUnitsDetail.Text = "ReadingUnits > " + Name.TrimStart() + " > Details";
                    ReadingUnitsDetail.Type = "Modal";
                    ReadingUnitsDetail.JS_function = "ReadingUnits_Details_Modal('" + Id + "')";
                    list.Add(ReadingUnitsDetail); // add an Object in list object
                   
                
                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("ReadingUnitsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.ReadingUnitsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Addresses data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AddressesDataForSearch()
        {
            try
            {
                InsertEventLog("AddressesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Address Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.AddressesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Addresses.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Address.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel AddressesDetail = new SearchDataViewModel();// declare a new object to add data in a list

                    AddressesDetail.Result = "<a href='/Addresses/Details/" + item.AddressId + "'>Detail Addresses</a>";
                    AddressesDetail.FirstURL = item.AddressId.ToString();
                    AddressesDetail.Text = "Addresses > " + item.Street2;
                    AddressesDetail.Type = "Modal";
                    AddressesDetail.JS_function = "Addresses_Details_Modal('" + item.AddressId + "')";
                    list.Add(AddressesDetail); // add an Object in list object


                    

                }
                SearchDataViewModel AddressesCreate = new SearchDataViewModel();    // declare a new object to add data in a list
                AddressesCreate.Result = "<a href='/Addresses/Create'>Create Addresses</a>";
                AddressesCreate.FirstURL = "/Addresses/Create";
                AddressesCreate.Text = "Addresses > Create";
                AddressesCreate.Type = "URL";
                AddressesCreate.value = "Navigate to Create Page.";
                list.Add(AddressesCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("AddressesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AddressesDataForSearch", "");

                throw ex;
            }
        }

        /// <summary>
        /// Addresses data for search add.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AddressesDataForSearch_add(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("AddressesDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Address Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.AddressesDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Addresses.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Address.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel AddressesEdit = new SearchDataViewModel();  // declare a new object to add data in a list
                    SearchDataViewModel AddressesDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    AddressesEdit.Result = "<a href='/Addresses/Edit/" + Id + "'>Edit Addresses</a>";
                    AddressesEdit.FirstURL = "/Addresses/Edit/" + Id;
                    AddressesEdit.Text = "Addresses > " + Name.TrimStart() + " > Edit";
                    AddressesEdit.Type = "URL";
                    AddressesEdit.value = "Navigate to Edit Page.";
                    list.Add(AddressesEdit); // add an Object in list object

                   
                    AddressesDetail.Result = "<a href='/Addresses/Details/" + Id + "'>Detail Addresses</a>";
                    AddressesDetail.FirstURL = Id.ToString();
                    AddressesDetail.Text = "Addresses > " + Name.TrimStart() + " > Details";
                    AddressesDetail.Type = "Modal";
                    AddressesDetail.JS_function = "Addresses_Details_Modal('" + Id + "')";
                    list.Add(AddressesDetail); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("AddressesDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AddressesDataForSearch", "");

                throw ex;
            }
        }

        /// <summary>
        /// Locations data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> LocationsDataForSearch()
        {
            try
            {
                InsertEventLog("LocationsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Location Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.LocationsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Locations.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Location.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel LocationsDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    
                    LocationsDetail.Result = "<a href='/Locations/Details/" + item.LocationId + "'>Detail Locations</a>";
                    LocationsDetail.FirstURL = item.LocationId.ToString();
                    LocationsDetail.Text = "Locations > " + item.Name;
                    LocationsDetail.Type = "Modal";
                    LocationsDetail.JS_function = "Locations_Details_Modal('" + item.LocationId + "')";
                    list.Add(LocationsDetail); // add an Object in list object
                    
                }
                SearchDataViewModel LocationsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                LocationsCreate.Result = "<a href='/Locations/Create'>Create Locations</a>";
                LocationsCreate.FirstURL = "/Locations/Create";
                LocationsCreate.Text = "Locations > Create";
                LocationsCreate.Type = "URL";
                list.Add(LocationsCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("LocationsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.LocationsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Locations data for search location.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> LocationsDataForSearch_location(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("LocationsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Location Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.LocationsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Locations.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Location.ToString()).Where(x => x.DataType == "String").ToList();

                 SearchDataViewModel LocationsEdit = new SearchDataViewModel();  // declare a new object to add data in a list
                    SearchDataViewModel LocationsDetail = new SearchDataViewModel();// declare a new object to add data in a list
                    LocationsEdit.Result = "<a href='/Locations/Edit/" + Id + "'>Edit Locations</a>";
                    LocationsEdit.FirstURL = "/Locations/Edit/" + Id;
                    LocationsEdit.Text = "Locations > " + Name.TrimStart() + " > Edit";
                    LocationsEdit.Type = "URL";
                    list.Add(LocationsEdit); // add an Object in list object

                    LocationsDetail.Result = "<a href='/Locations/Details/" + Id + "'>Detail Locations</a>";
                    LocationsDetail.FirstURL = Id.ToString();
                    LocationsDetail.Text = "Locations > " + Name.TrimStart() + " > Details";
                    LocationsDetail.Type = "Modal";
                    LocationsDetail.JS_function = "Locations_Details_Modal('" + Id + "')";
                    list.Add(LocationsDetail); // add an Object in list object
                    
              
                return list; // return list object
            }
            catch (Exception ex)
            {

                InsertEventMonitor("LocationsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.LocationsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Alerts data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AlertsDataForSearch()
        {
            try
            {

                InsertEventLog("AlertsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Alerts Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.AlertsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Alerts.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Alert.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel AlertsDetail = new SearchDataViewModel();   // declare a new object to add data in a list

                    AlertsDetail.Result = "<a href='/Alerts/Details/" + item.AlertId + "'>Detail Alerts</a>";
                    AlertsDetail.FirstURL = item.AlertId.ToString();
                    AlertsDetail.Text = "Alerts > " + item.Title;
                    AlertsDetail.Type = "Modal";
                    AlertsDetail.JS_function = "Alerts_Details_Modal('" + item.AlertId + "')";
                    list.Add(AlertsDetail); // add an Object in list object
                   
                }
                SearchDataViewModel AlertsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                AlertsCreate.Result = "<a href='/Alerts/Create'>Create Alerts</a>";
                AlertsCreate.FirstURL = "/Alerts/Create";
                AlertsCreate.Text = "Alerts > Create";
                AlertsCreate.Type = "URL";
                AlertsCreate.value = "Navigate to Create Page.";
                list.Add(AlertsCreate);

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("AlertsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AlertsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Alerts data for search alert.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> AlertsDataForSearch_alert(string Module, string Name, Guid Id)
        {
            try
            {

                InsertEventLog("AlertsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of Alerts Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.AlertsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.Alerts.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Alert.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel AlertsEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel AlertsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    AlertsEdit.Result = "<a href='/Alerts/Edit/" + Id + "'>Edit Alerts</a>";
                    AlertsEdit.FirstURL = "/Alerts/Edit/" + Id;
                    AlertsEdit.Text = "Alerts > " + Name.TrimStart() + " > Edit";
                    AlertsEdit.Type = "URL";
                    AlertsEdit.value = "Navigate to Edit Page.";
                    list.Add(AlertsEdit); // add an Object in list object

                    AlertsDetail.Result = "<a href='/Alerts/Details/" + Id + "'>Detail Alerts</a>";
                    AlertsDetail.FirstURL = Id.ToString();
                    AlertsDetail.Text = "Alerts > " + Name.TrimStart() + " > Details";
                    AlertsDetail.Type = "Modal";
                    AlertsDetail.JS_function = "Alerts_Details_Modal('" + Id + "')";
                    list.Add(AlertsDetail); // add an Object in list object
               

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("AlertsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.AlertsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work flows data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkFlowsDataForSearch()
        {
            try
            {

                InsertEventLog("WorkFlowsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of WorkFlows Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkFlows.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkFlow.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel WorkFlowsDetail = new SearchDataViewModel();   // declare a new object to add data in a list

                    WorkFlowsDetail.Result = "<a href='/WorkFlows/Details/" + item.WorkFlowId + "'>Detail WorkFlows</a>";
                    WorkFlowsDetail.FirstURL = item.WorkFlowId.ToString();
                    WorkFlowsDetail.Text = "WorkFlows > " + item.Name;
                    WorkFlowsDetail.Type = "Modal";
                    WorkFlowsDetail.JS_function = "WorkFlows_Details_Modal('" + item.WorkFlowId + "')";
                    list.Add(WorkFlowsDetail); // add an Object in list object
                }
                SearchDataViewModel AlertsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                AlertsCreate.Result = "<a href='/WorkFlows/Create'>Create WorkFlows</a>";
                AlertsCreate.FirstURL = "/WorkFlows/Create";
                AlertsCreate.Text = "WorkFlows > Create";
                AlertsCreate.Type = "URL";
                AlertsCreate.value = "Navigate to Create Page.";
                list.Add(AlertsCreate);

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("WorkFlowsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work flows data for search wf.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkFlowsDataForSearch_wf(string Module, string Name, Guid Id)
        {
            try
            {

                InsertEventLog("WorkFlowsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of WorkFlows Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkFlows.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkFlow.ToString()).Where(x => x.DataType == "String").ToList();
                
                    SearchDataViewModel WorkFlowsEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel WorkFlowsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkFlowsEdit.Result = "<a href='/WorkFlows/Edit/" + Id + "'>Edit WorkFlows</a>";
                    WorkFlowsEdit.FirstURL = "/WorkFlows/Edit/" + Id;
                    WorkFlowsEdit.Text = "WorkFlows > " + Name.TrimStart() + " > Edit";
                    WorkFlowsEdit.Type = "URL";
                    WorkFlowsEdit.value = "Navigate to Edit Page.";
                    list.Add(WorkFlowsEdit); // add an Object in list object

                    WorkFlowsDetail.Result = "<a href='/WorkFlows/Details/" + Id + "'>Detail WorkFlows</a>";
                    WorkFlowsDetail.FirstURL = Id.ToString();
                    WorkFlowsDetail.Text = "WorkFlows > " + Name.TrimStart() + " > Details";
                    WorkFlowsDetail.Type = "Modal";
                    WorkFlowsDetail.JS_function = "WorkFlows_Details_Modal('" + Id + "')";
                    list.Add(WorkFlowsDetail); // add an Object in list object
                  
               

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("WorkFlowsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work flow mapping data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkFlowMappingDataForSearch()
        {
            try
            {

                InsertEventLog("WorkFlowMappingDataForSearch", EventType.Log, EventColor.yellow, "Get List Of WorkFlows Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowMappingDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkFlowMappings.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkFlowMapping.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel WorkFlowDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkFlowDetail.Result = "<a href='/WorkFlowMappings/Details/" + item.WorkFlowMappingId + "'>Detail WorkFlowMappings</a>";
                    WorkFlowDetail.FirstURL = item.WorkFlowMappingId.ToString();
                    WorkFlowDetail.Text = "WorkFlowMappings > " + item.SourceType;
                    WorkFlowDetail.Type = "Modal";
                    WorkFlowDetail.JS_function = "WorkFlowMappings_Details_Modal('" + item.WorkFlowMappingId + "')";
                    list.Add(WorkFlowDetail); // add an Object in list object
                    
                }
                SearchDataViewModel AlertsCreate = new SearchDataViewModel();// declare a new object to add data in a list
                AlertsCreate.Result = "<a href='/WorkFlowMappings/Create'>Create WorkFlowMappings</a>";
                AlertsCreate.FirstURL = "/WorkFlowMappings/Create";
                AlertsCreate.Text = "WorkFlowMappings > Create";
                AlertsCreate.Type = "URL";
                AlertsCreate.value = "Navigate to Create Page.";
                list.Add(AlertsCreate);

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("WorkFlowMappingDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowMappingDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work flow mapping data for search wf mapping.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkFlowMappingDataForSearch_wfMapping(string Module, string Name, Guid Id)
        {
            try
            {

                InsertEventLog("WorkFlowMappingDataForSearch", EventType.Log, EventColor.yellow, "Get List Of WorkFlows Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowMappingDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkFlowMappings.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkFlowMapping.ToString()).Where(x => x.DataType == "String").ToList();

                
                    SearchDataViewModel WorkFlowEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel WorkFlowDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkFlowEdit.Result = "<a href='/WorkFlowMappings/Edit/" + Id + "'>Edit WorkFlowMappings</a>";
                    WorkFlowEdit.FirstURL = "/WorkFlowMappings/Edit/" + Id;
                    WorkFlowEdit.Text = "WorkFlowMappings > " + Name.TrimStart() + " > Edit";
                    WorkFlowEdit.Type = "URL";
                    WorkFlowEdit.value = "Navigate to Eidt Page.";
                    list.Add(WorkFlowEdit); // add an Object in list object

                    WorkFlowDetail.Result = "<a href='/WorkFlowMappings/Details/" + Id + "'>Detail WorkFlowMappings</a>";
                    WorkFlowDetail.FirstURL = Id.ToString();
                    WorkFlowDetail.Text = "WorkFlowMappings > " + Name.TrimStart() + " > Details";
                    WorkFlowDetail.Type = "Modal";
                    WorkFlowDetail.JS_function = "WorkFlowMappings_Details_Modal('" + Id + "')";
                    list.Add(WorkFlowDetail); // add an Object in list object
                    
              

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("WorkFlowMappingDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowMappingDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work flow reports data for search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkFlowReportsDataForSearch()
        {
            try
            {
                InsertEventLog("WorkFlowReportsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of WorkFlows Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowReportsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkFlowReports.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkFlowReport.ToString()).Where(x => x.DataType == "String").ToList();
                foreach (var item in query.CollectionNotNull())
                {
                    SearchDataViewModel WorkFlowReportsEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel WorkFlowReportsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkFlowReportsDetail.Result = "<a href='/WorkFlowReports/Details/" + item.WorkFlowReportId + "'>Detail WorkFlowReports</a>";
                    WorkFlowReportsDetail.FirstURL = item.WorkFlowReportId.ToString();
                    WorkFlowReportsDetail.Text = "WorkFlowReports > " + item.WorkFlow.Name;
                    WorkFlowReportsDetail.Type = "Modal";
                    WorkFlowReportsDetail.JS_function = "WorkFlowReports_Details_Modal('" + item.WorkFlowReportId + "')";
                    list.Add(WorkFlowReportsDetail); // add an Object in list object
                }

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("WorkFlowReportsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowReportsDataForSearch", "");
                throw ex;
            }
        }

        /// <summary>
        /// Work flow reports data for search wf reports.
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> WorkFlowReportsDataForSearch_wfReports(string Module, string Name, Guid Id)
        {
            try
            {
                InsertEventLog("WorkFlowReportsDataForSearch", EventType.Log, EventColor.yellow, "Get List Of WorkFlows Data For Search", "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowReportsDataForSearch", "");
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                var query = dbEnt.WorkFlowReports.ToList();
                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.WorkFlowReport.ToString()).Where(x => x.DataType == "String").ToList();
               
                    SearchDataViewModel WorkFlowReportsEdit = new SearchDataViewModel();     // declare a new object to add data in a list
                    SearchDataViewModel WorkFlowReportsDetail = new SearchDataViewModel();   // declare a new object to add data in a list
                    WorkFlowReportsEdit.Result = "<a href='/WorkFlowReports/Details/" + Id + "'>Detail WorkFlowReports</a>";
                    WorkFlowReportsEdit.FirstURL = Id.ToString();
                    WorkFlowReportsEdit.Text = "WorkFlowReports > " + Name.TrimStart() + ".Details";
                    WorkFlowReportsEdit.Type = "Modal";
                    WorkFlowReportsEdit.JS_function = "WorkFlowReports_Details_Modal('" + Id + "')";
                    list.Add(WorkFlowReportsEdit); // add an Object in list object
                    

                return list; // return list object
            }
            catch (Exception ex)
            {
                InsertEventMonitor("WorkFlowReportsDataForSearch", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.WorkFlowReportsDataForSearch", "");
                throw ex;
            }
        }


        /// <summary>
        /// Leads extra search.
        /// </summary>
        /// <returns>List&lt;SearchDataViewModel&gt;.</returns>
        public List<SearchDataViewModel> LeadsExtraSearch()
        {
            try
            {
                List<SearchDataViewModel> list = new List<SearchDataViewModel>();// declare a new list object

                List<Lead> query = dbEnt.Leads.ToList();

                List<workflowDataTypeDTO> attr = GetAttributesOfEntity(EntityTypes.Lead.ToString()).Where(x => x.DataType == "String").ToList();

                foreach (Lead item in query.CollectionNotNull())
                {
                    SearchDataViewModel leadEdit = new SearchDataViewModel();   // declare a new object to add data in a list
                    leadEdit.Result = "<a href='/Leads/Edit/" + item.LeadId + "'>Edit Lead</a>";
                    leadEdit.FirstURL = "/Leads/Edit/" + item.LeadId;
                    leadEdit.Text = "Lead/" + item.Name + "/Edit";
                    leadEdit.Type = "URL";
                    leadEdit.Type = "Navigate to Edit Page.";
                    list.Add(leadEdit); // add an Object in list object

                    SearchDataViewModel leadDetail = new SearchDataViewModel(); // declare a new object to add data in a list
                    leadDetail.Result = "<a href='/Leads/Details/" + item.LeadId + "'>Detail Lead</a>";
                    leadDetail.FirstURL = item.LeadId.ToString();
                    leadDetail.Text = "Lead/" + item.Name + "/Details";
                    leadDetail.Type = "Modal";
                    leadDetail.JS_function = "Leads_Details_Modal('" + item.LeadId + "')";
                    list.Add(leadDetail); // add an Object in list object

                    foreach (workflowDataTypeDTO selected in attr)
                    {
                        SearchDataViewModel s2 = new SearchDataViewModel(); // declare a new object to add data in a list
                        s2.Result = "<a href='/Leads/Edit/" + item.LeadId + "'>Edit Lead</a>";
                        s2.FirstURL = "/Leads/Edit/" + item.LeadId;
                        s2.Text = "Lead/" + item.Name + "/" + selected.ColumnName;
                        string value = (String)item.GetType().GetProperty(selected.ColumnName).GetValue(item);
                        s2.value = value;
                        s2.Type = "URL";
                        list.Add(s2); // add an Object in list object
                    }
                }
                SearchDataViewModel LeadCreate = new SearchDataViewModel();// declare a new object to add data in a list
                LeadCreate.Result = "<a href='/Leads/Create'>Create Lead</a>";
                LeadCreate.FirstURL = "/Leads/Create";
                LeadCreate.Text = "Create Lead";
                LeadCreate.Type = "URL";
                LeadCreate.value = "Navigate to Create Page.";
                list.Add(LeadCreate); // add an Object in list object

                return list; // return list object
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Gets the attributes of entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>List&lt;workflowDataTypeDTO&gt;.</returns>
        public List<workflowDataTypeDTO> GetAttributesOfEntity(string type)
        {
            try
            {
                InsertEventLog("GetAttributesOfEntity", EventType.Log, EventColor.yellow, "going to get attribute of " + type, "TICRM.BusinessLayer.GlobalSearchManager.GetAttributesOfEntity", "");
                List<workflowDataTypeDTO> vs = new List<workflowDataTypeDTO>();


                PropertyInfo[] Query = type == EntityTypes.Account.ToString() ? typeof(Account).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Lead.ToString() ? typeof(Lead).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Device.ToString() ? typeof(Device).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.WorkOrder.ToString() ? typeof(WorkOrder).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Oppertunity.ToString() ? typeof(Opportunity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.CustomerAsset.ToString() ? typeof(CustomerAsset).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Reading.ToString() ? typeof(Reading).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.ServiceCall.ToString() ? typeof(ServiceCall).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Resource.ToString() ? typeof(Resource).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.ReadingType.ToString() ? typeof(ReadingType).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.ReadingUnit.ToString() ? typeof(ReadingUnit).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Address.ToString() ? typeof(Address).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Location.ToString() ? typeof(Location).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.Alert.ToString() ? typeof(Alert).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.WorkFlow.ToString() ? typeof(WorkFlow).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.WorkFlowMapping.ToString() ? typeof(WorkFlowMapping).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : type == EntityTypes.WorkFlowReport.ToString() ? typeof(WorkFlowReport).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        : null;

                foreach (PropertyInfo item in Query.CollectionNotNull())
                {
                    if ((item.PropertyType.Name == "String" || item.PropertyType.FullName.Contains("System.Int32") || item.PropertyType.FullName.Contains("System.Guid"))
                        && ExcludedColumns.CreatedDate != item.Name
                        && ExcludedColumns.CreatedBy != item.Name
                        && ExcludedColumns.UpdatedDate != item.Name
                        && ExcludedColumns.UpdatedBy != item.Name
                        && ExcludedColumns.AccountId != item.Name)
                    {
                        workflowDataTypeDTO obj = new workflowDataTypeDTO();
                        obj.ColumnName = item.Name.ToString();
                        // apply tennary operator and another name of it is misc operator
                        obj.DataType = item.PropertyType.Name.ToString() == "String" ? item.PropertyType.Name.ToString()
                                        : item.PropertyType.FullName.Contains("System.Int32") ? "int"
                                        : item.PropertyType.FullName.Contains("System.Guid") ? "Guid" : "";
                        vs.Add(obj);
                    }
                }

                return vs;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAttributesOfEntity", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GlobalSearchManager.GetAttributesOfEntity", "");
                throw;
            }
        }

    }

}
