using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using TICRM.ViewModels;

namespace TICRM.Controllers
{
    /************GlobalSearch Controller************
    Class [GlobalSearchController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with searchbar and all search features]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||  Changes Made:   [18/08/2020     changed the flow of search data           Akhtar Zaman]
    ||  Changes Made:   [20/08/2020     Added method for free search              Akhtar Zaman]
    ||                  
     ********************************************/
    public class GlobalSearchController : BaseController
    {
        // GET: GlobalSearch
        private GlobalSearchManager gsm = new GlobalSearchManager();
        /// <summary>
        /// Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// save the global search.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="URL">The URL.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string SubmitGlobalSearch(string Name, string URL)
        {
            try
            {
                bool condition = gsm.SavedGlobalSearch(Name, URL); // pass name and URL to global Search Manager and get response true and false

                if (condition == true) //condotion Check to return success and error
                {
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Updates the global search.
        /// </summary>
        /// <param name="GlobalSearchId">The global search identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="URL">The URL.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string UpdateGlobalSearch(Guid GlobalSearchId, string Name, string URL)
        {
            try
            {
                bool condition = gsm.UpdateGlobalSearch(GlobalSearchId, Name, URL); // pass id, name and URL to global Search Manager and get response true and false
                if (condition == true) //condotion Check to return success and error
                {
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the global search list.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string GetGlobalSearchList()
        {
            try
            {
                return gsm.GetGlobalSearchList(); // get list from Global Search Manager and return to view
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the global search.
        /// </summary>
        /// <param name="GlobalSearchId">The global search identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult EditGlobalSearch(Guid GlobalSearchId)
        {
            try
            {
                GlobalSearchDto data = new GlobalSearchDto(); // create an object of Global Search DTO
                data = gsm.GetGlobalSearchOnId(GlobalSearchId); // get global search on id and place in data object
                return Json(data, JsonRequestBehavior.AllowGet); // return data in json format
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the global search.
        /// </summary>
        /// <param name="GlobalSearchId">The global search identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteGlobalSearch(Guid GlobalSearchId)
        {
            try
            {
                bool status = gsm.DeleteGlobalSearchOnId(GlobalSearchId); // pass id to delete data global search

                if (status == true) // check condotion to return status to view
                {
                    return "success";
                }
                return "error";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the eac search list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpGet]
        public JsonResult GetEACSearchList()
        {
            try
            {
                // declare a string array with name urlSegment and pass Current URL Segments in string array
                string[] urlSegments = Request.UrlReferrer.Segments;

                List<SearchDataViewModel> global = new List<SearchDataViewModel>();     // create a SeachDataViewModel List Object
                string SegmentOne = urlSegments.Length > 1 ? urlSegments[1].Replace(@"/", "") : ""; // Get Segment One In Array if is null then it pass empty.tostring     

                GlobalSearchViewModel gsvm = new GlobalSearchViewModel();   // create an object to save in session for further used
                gsvm.URL = Request.UrlReferrer.AbsolutePath;        // save a absolute path of URL in gsvm object
                gsvm.SearchDataList = new List<SearchDataViewModel>();      // create a new List object of  SearchDataViewModel

                GlobalSearchViewModel query = Session["GlobalSearchViewSession"] as GlobalSearchViewModel;      // query in session get data

                if (query == null) { gsvm.FirstInSearch = gsm.FirstInSearchData(); } else { gsvm.FirstInSearch = query.FirstInSearch; }




                // switch statement is used for get dynamically for Global Search
                switch (SegmentOne)
                {
                    case "Devices":
                        int pos = Array.IndexOf(urlSegments, "device", 2);      // check position 2 is exist with name device
                        if (pos > -1)
                        {
                            string segmentTwo = urlSegments[2].Replace("/", "");    // remove '/' from '/device' and 
                            string MACoptional = Session["MacAddress"] as string;   // Get MACAddress from session
                                                                                    //global = gsm.DeviceDataOfMAC(MACoptional);      
                            gsvm.SearchDataList = gsm.DeviceDataOfMAC(MACoptional);// Pass MacAddress to get data against  MacAddress
                        }
                        else
                        {
                            gsvm.SearchDataList = gsm.DeviceDataForSearch();    // Get Device Data for Edit,Details and View
                        }
                        break;
                    case "devices":
                        int pos1 = Array.IndexOf(urlSegments, "device", 2);// check position 2 is exist with name device
                        if (pos1 > -1)
                        {
                            string segmentTwo = urlSegments[2].Replace("/", "");    // remove '/' from '/device' and 

                            string MACoptional = Session["MacAddress"] as string;   // Get MACAddress from session

                            gsvm.SearchDataList = gsm.DeviceDataOfMAC(MACoptional); // Pass MacAddress to get data against  MacAddress

                        }
                        else
                        {
                            gsvm.SearchDataList = gsm.DeviceDataForSearch();    // Get Device Data for Edit,Details and View
                        }
                        break;

                    case "Leads":
                        gsvm.SearchDataList = gsm.LeadsDataForSearch(); // Get Lead Data for Edit,Details and View

                        // gsvm.SearchDataList = gsm.LeadsExtraSearch(); // Get Lead Data for Edit,Details and View
                        break;
                    case "Opportunities":
                        gsvm.SearchDataList = gsm.OpportunitiesDataForSearch(); // Get Opportunities Data for Edit,Details and View
                        break;
                    case "Accounts":
                        int Account_pos = Array.IndexOf(urlSegments, "AccountsDetail/", 2);
                        if (Account_pos > -1)
                        {

                            //gsvm.SearchDataList.AddRange(gsm.GetAccountsDetailForSearch(new Guid(urlSegments[3])));

                        }

                        gsvm.SearchDataList.AddRange(gsm.AccountsDataForSearch());

                        //gsvm.SearchDataList = gsm.AccountsDataForSearch();// Get Accounts Data for Edit,Details and View
                        break;
                    case "CustomerAssets":
                        gsvm.SearchDataList = gsm.CustomerAssetsDataForSearch();// Get CustomerAssets Data for Edit,Details and View
                        break;
                    case "Readings":
                        gsvm.SearchDataList = gsm.ReadingsDataForSearch();// Get Readings Data for Edit,Details and View
                        break;
                    case "ServiceCalls":
                        gsvm.SearchDataList = gsm.ServiceCallsDataForSearch();// Get ServiceCalls Data for Edit,Details and View
                        break;
                    case "Resources":
                        gsvm.SearchDataList = gsm.ResourcesDataForSearch();// Get Resources Data for Edit,Details and View
                        break;
                    case "WorkOrders":
                        gsvm.SearchDataList = gsm.WorkOrdersDataForSearch();// Get WorkOrders Data for Edit,Details and View
                        break;
                    case "ReadingTypes":
                        gsvm.SearchDataList = gsm.ReadingTypesDataForSearch();// Get ReadingTypes Data for Edit,Details and View
                        break;
                    case "ReadingUnits":
                        gsvm.SearchDataList = gsm.ReadingUnitsDataForSearch();// Get ReadingUnits Data for Edit,Details and View
                        break;

                    case "Addresses":
                        gsvm.SearchDataList = gsm.AddressesDataForSearch();// Get Addresses Data for Edit,Details and View
                        break;
                    case "Locations":
                        gsvm.SearchDataList = gsm.LocationsDataForSearch();// Get Locations Data for Edit,Details and View
                        break;
                    case "Alerts":
                        gsvm.SearchDataList = gsm.AlertsDataForSearch();// Get Alerts Data for Edit,Details and View
                        break;
                    case "WorkFlows":
                        gsvm.SearchDataList = gsm.WorkFlowsDataForSearch();// Get WorkFlows Data for Edit,Details and View
                        break;
                    case "WorkFlowMappings":
                        gsvm.SearchDataList = gsm.WorkFlowMappingDataForSearch();// Get WorkFlowMapping Data for Edit,Details and View
                        break;
                    case "WorkFlowReports":
                        gsvm.SearchDataList = gsm.WorkFlowReportsDataForSearch();// Get workflowreport Data for Edit,Details and View
                        break;

                }

                Session["GlobalSearchViewSession"] = gsvm;      // Pass gsvm Data in Session to fast its functionality


                return Json(gsvm);  // return a json list of gsvm in response
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Get all the search items on load event.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetEAC_On_LoadEvent(string value)
        {
            try
            {
                // declare a string array with name urlSegment and pass Current URL Segments in string array
                string[] urlSegments = value.Split(' ', ',', '/', '>');

                List<SearchDataViewModel> global = new List<SearchDataViewModel>();     // create a SeachDataViewModel List Object
                string SegmentOne = urlSegments.Length > 1 ? urlSegments[1].Replace(@"/", "") : ""; // Get Segment One In Array if is null then it pass empty.tostring     

                GlobalSearchViewModel gsvm = new GlobalSearchViewModel();   // create an object to save in session for further used
                gsvm.URL = Request.UrlReferrer.AbsolutePath;        // save a absolute path of URL in gsvm object
                gsvm.SearchDataList = new List<SearchDataViewModel>();      // create a new List object of  SearchDataViewModel

                GlobalSearchViewModel query = Session["GlobalSearchViewSession"] as GlobalSearchViewModel;      // query in session get data


                // switch statement is used for get dynamically for Global Search
                switch (value)
                {
                    case "Devices":
                        int pos = Array.IndexOf(urlSegments, "device", 2);      // check position 2 is exist with name device
                        if (pos > -1)
                        {
                            string segmentTwo = urlSegments[2].Replace("/", "");    // remove '/' from '/device' and 

                            string MACoptional = Session["MacAddress"] as string;   // Get MACAddress from session
                            gsvm.SearchDataList = gsm.DeviceDataOfMAC(MACoptional);// Pass MacAddress to get data against  MacAddress

                        }
                        else
                        {
                            gsvm.SearchDataList = gsm.DeviceDataForSearch();    // Get Device Data for Edit,Details and View
                        }
                        break;
                    case "devices":
                        int pos1 = Array.IndexOf(urlSegments, "device", 2);// check position 2 is exist with name device
                        if (pos1 > -1)
                        {
                            string segmentTwo = urlSegments[2].Replace("/", "");    // remove '/' from '/device' and 

                            string MACoptional = Session["MacAddress"] as string;   // Get MACAddress from session

                            gsvm.SearchDataList = gsm.DeviceDataOfMAC(MACoptional); // Pass MacAddress to get data against  MacAddress

                        }
                        else
                        {
                            gsvm.SearchDataList = gsm.DeviceDataForSearch();    // Get Device Data for Edit,Details and View
                        }
                        break;

                    case "Leads":
                        gsvm.SearchDataList = gsm.LeadsDataForSearch(); // Get Lead Data for Edit,Details and View

                        // gsvm.SearchDataList = gsm.LeadsExtraSearch(); // Get Lead Data for Edit,Details and View
                        break;
                    case "Opportunities":
                        gsvm.SearchDataList = gsm.OpportunitiesDataForSearch(); // Get Opportunities Data for Edit,Details and View
                        break;
                    case "Accounts":
                        int Account_pos = Array.IndexOf(urlSegments, "AccountsDetail/", 2);
                        if (Account_pos > -1)
                        {

                            //gsvm.SearchDataList.AddRange(gsm.GetAccountsDetailForSearch(new Guid(urlSegments[3])));

                        }
                        else
                            gsvm.SearchDataList.AddRange(gsm.AccountsDataForSearch());

                        //gsvm.SearchDataList = gsm.AccountsDataForSearch();// Get Accounts Data for Edit,Details and View
                        break;
                    case "CustomerAssets":
                        gsvm.SearchDataList = gsm.CustomerAssetsDataForSearch();// Get CustomerAssets Data for Edit,Details and View
                        break;
                    case "Readings":
                        gsvm.SearchDataList = gsm.ReadingsDataForSearch();// Get Readings Data for Edit,Details and View
                        break;
                    case "ServiceCalls":
                        gsvm.SearchDataList = gsm.ServiceCallsDataForSearch();// Get ServiceCalls Data for Edit,Details and View
                        break;
                    case "Resources":
                        gsvm.SearchDataList = gsm.ResourcesDataForSearch();// Get Resources Data for Edit,Details and View
                        break;
                    case "WorkOrders":
                        gsvm.SearchDataList = gsm.WorkOrdersDataForSearch();// Get WorkOrders Data for Edit,Details and View
                        break;
                    case "ReadingTypes":
                        gsvm.SearchDataList = gsm.ReadingTypesDataForSearch();// Get ReadingTypes Data for Edit,Details and View
                        break;
                    case "ReadingUnits":
                        gsvm.SearchDataList = gsm.ReadingUnitsDataForSearch();// Get ReadingUnits Data for Edit,Details and View
                        break;

                    case "Addresses":
                        gsvm.SearchDataList = gsm.AddressesDataForSearch();// Get Addresses Data for Edit,Details and View
                        break;
                    case "Locations":
                        gsvm.SearchDataList = gsm.LocationsDataForSearch();// Get Locations Data for Edit,Details and View
                        break;
                    case "Alerts":
                        gsvm.SearchDataList = gsm.AlertsDataForSearch();// Get Alerts Data for Edit,Details and View
                        break;
                    case "WorkFlows":
                        gsvm.SearchDataList = gsm.WorkFlowsDataForSearch();// Get WorkFlows Data for Edit,Details and View
                        break;
                    case "WorkFlowMappings":
                        gsvm.SearchDataList = gsm.WorkFlowMappingDataForSearch();// Get WorkFlowMapping Data for Edit,Details and View
                        break;
                    case "WorkFlowReports":
                        gsvm.SearchDataList = gsm.WorkFlowReportsDataForSearch();// Get workflowreport Data for Edit,Details and View
                        break;

                }

                Session["GlobalSearchViewSession"] = gsvm;      // Pass gsvm Data in Session to fast its functionality

                return Json(gsvm);  // return a json list of gsvm in response
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets all search items from Manager and return to the view.
        /// </summary>
        /// <returns>JsonResult.</returns>
        public JsonResult GetAllSearchItems()
        {
            GlobalSearchViewModel gsvms = new GlobalSearchViewModel();   // create an object to save in session for further used
            gsvms.URL = Request.UrlReferrer.AbsolutePath;        // save a absolute path of URL in gsvm object
            gsvms.FirstInSearch = gsm.LoadAllSerachItems();
            return Json(gsvms);
        }
        /// <summary>
        /// Modules wise search list
        /// </summary>
        public JsonResult Get_All_EACSearchListModule(String Module)
        {
            try
            {
                GlobalSearchViewModel gsvms = new GlobalSearchViewModel();   // create an object to save in session for further used
                gsvms.URL = Request.UrlReferrer.AbsolutePath;        // save a absolute path of URL in gsvm object

                if (Module == "Devices")
                {
                    gsvms.FirstInSearch = gsm.DeviceDataForSearch();

                }

                //if (!string.IsNullOrEmpty(MACoptional))
                //{
                //  gsvm.FirstInSearch.AddRange(gsm.DeviceDataOfMAC(MACoptional));
                //}
                //else
                //{
                //    gsvm.FirstInSearch.AddRange(gsm.DeviceDataForSearch());
                //}

                //lead
                if (Module == "Leads")
                {
                    gsvms.FirstInSearch = gsm.LeadsDataForSearch();

                }

                if (Module == "Opportunities")
                {
                    gsvms.FirstInSearch = gsm.OpportunitiesDataForSearch();
                }

                if (Module == "CustomerAssets")
                {
                    gsvms.FirstInSearch = gsm.CustomerAssetsDataForSearch();
                }

                if (Module == "Accounts")
                {
                    gsvms.FirstInSearch = gsm.AccountsDataForSearch_accounts();

                }

                if (Module == "Readings" )
                {
                    gsvms.FirstInSearch = gsm.ReadingsDataForSearch();
                }


                if (Module == "ServiceCalls")
                {
                    gsvms.FirstInSearch = gsm.ServiceCallsDataForSearch();
                }

                if(Module == "Resources")
                {
                    gsvms.FirstInSearch = gsm.ResourcesDataForSearch();
                }

                if (Module == "WorkOrders")
                {
                    gsvms.FirstInSearch = gsm.WorkOrdersDataForSearch();
                }

                if (Module == "ReadingTypes")
                {
                    gsvms.FirstInSearch = gsm.ReadingTypesDataForSearch();
                }

                if(Module == "ReadingUnits")
                {
                    gsvms.FirstInSearch = gsm.ReadingUnitsDataForSearch();
                }

                if (Module == "Addresses")
                {
                    gsvms.FirstInSearch = gsm.AddressesDataForSearch();
                }

                if (Module == "Locations")
                {
                    gsvms.FirstInSearch = gsm.LocationsDataForSearch();
                }

                if (Module == "Alerts" )
                {
                    gsvms.FirstInSearch = gsm.AlertsDataForSearch();
                }

                if (Module == "WorkFlows")
                {
                    gsvms.FirstInSearch = gsm.WorkFlowsDataForSearch();
                }

                if (Module == "WorkFlowMappings")
                {
                    gsvms.FirstInSearch = gsm.WorkFlowMappingDataForSearch();
                }

                if (Module == "WorkFlowReports" )
                {
                    gsvms.FirstInSearch = gsm.WorkFlowReportsDataForSearch();
                }
               
                if(gsvms.SearchDataList != null && gsvms.FirstInSearch != null) {
                    gsvms.SearchDataList.OrderBy(x => x.Text);
                    gsvms.FirstInSearch.OrderBy(x => x.Text);
                }


                return Json(gsvms);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                throw ex;
            }
           
        }
        /// <summary>
        /// Assocates Search for accounts
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="Module"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public JsonResult Get_ALL_EACSearchListAccountModule(Guid AccountId, String Module, String Name)
        {
            try
            {
                GlobalSearchViewModel gsvms = new GlobalSearchViewModel();   // create an object to save in session for further used
                gsvms.FirstInSearch = gsm.GetAccountsDetailForSearch(AccountId, Module, Name);
                return Json(gsvms);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        /// <summary>
        /// funtion gets the module's detial and edit page options and post it to the view
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns> a list of pages params</returns>
        public JsonResult Get_All_EACSearchListModulePages(string Module, string Name, Guid Id)
        {
            try
            {
                GlobalSearchViewModel gsvms = new GlobalSearchViewModel();   // create an object to save in session for further used
                                                                             //gsvms.URL = Request.UrlReferrer.AbsolutePath;        // save a absolute path of URL in gsvm object
                if (Module.Contains("CustomerAssets"))
                {
                    gsvms.FirstInSearch = gsm.AssetsDetailsForSearch(Module, Name, Id);

                }
               
                if (Module.Contains("Accounts"))
                {
                    gsvms.FirstInSearch = gsm.AccountsDetailsForSearch(Module, Name, Id);

                }
                if (Module.Contains("Opportunities"))
                {
                    gsvms.FirstInSearch = gsm.OppDetailsForSearch(Module, Name, Id);
                }
                if (Module.Contains("Leads"))
                {
                    gsvms.FirstInSearch = gsm.LeadsDataForSearch_lead(Module, Name, Id);

                }
                if (Module.Contains("Readings"))
                {
                    gsvms.FirstInSearch = gsm.ReadingsDataForSearch_read(Module, Name, Id);
                }

                if (Module.Contains("ServiceCalls"))
                {
                    gsvms.FirstInSearch = gsm.ServiceCallsDataForSearch_service(Module, Name, Id);
                }

                if (Module.Contains("Resources"))
                {
                    gsvms.FirstInSearch = gsm.ResourcesDataForSearch_resource(Module, Name, Id);
                }

                if (Module.Contains("WorkOrders"))
                {
                    gsvms.FirstInSearch = gsm.WorkOrdersDataForSearch_workorders(Module, Name, Id);
                }

                if (Module.Contains("ReadingTypes"))
                {
                    gsvms.FirstInSearch = gsm.ReadingTypesDataForSearch_readingT(Module, Name, Id);
                }

                if (Module.Contains("ReadingUnits"))
                {
                    gsvms.FirstInSearch = gsm.ReadingUnitsDataForSearch_readingU(Module, Name, Id);
                }

                if (Module.Contains("Addresses"))
                {
                    gsvms.FirstInSearch = gsm.AddressesDataForSearch_add(Module, Name, Id);
                }

                if (Module.Contains("Locations"))
                {
                    gsvms.FirstInSearch = gsm.LocationsDataForSearch_location(Module, Name, Id);
                }

                if (Module.Contains("Alerts"))
                {
                    gsvms.FirstInSearch = gsm.AlertsDataForSearch_alert(Module, Name, Id);
                }

                if (Module.Contains("WorkFlows"))
                {
                    gsvms.FirstInSearch = gsm.WorkFlowsDataForSearch_wf(Module, Name, Id);
                }

                if (Module.Contains("WorkFlowMappings"))
                {
                    gsvms.FirstInSearch = gsm.WorkFlowMappingDataForSearch_wfMapping(Module, Name, Id);
                }

                if (Module.Contains("WorkFlowReports"))
                {
                    gsvms.FirstInSearch = gsm.WorkFlowReportsDataForSearch_wfReports(Module, Name, Id);
                }
                return Json(gsvms);
                

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw ex;
            }

        }

        //
        public JsonResult Get_All_EACSearchList()
        {
            try
            {
                string[] urlSegments = Request.UrlReferrer.Segments;
                string SegmentOne = urlSegments.Length > 1 ? urlSegments[1].Replace(@"/", "") : ""; // Get Segment One In Array if is null then it pass empty.tostring     

                GlobalSearchViewModel gsvm = new GlobalSearchViewModel();   // create an object to save in session for further used

                gsvm.FirstInSearch = gsm.FirstInSearchData();
                string MACoptional = Session["MacAddress"] as string;

                return Json(gsvm);  // return a json list of gsvm in response

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Saves the mac for device search.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string SaveMac(string MacAddress)
        {
            try
            {
                Session["MacAddress"] = null; // first declare a seession null;

                Session["MacAddress"] = MacAddress; // add a MACAddress in a sesssion
                if (Session["MacAddress"] != null)
                {
                    return "success"; // Return Success on save MACAddress In Session
                }
                return "error"; // return Error Status
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


    }
}