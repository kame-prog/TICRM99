using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
//using TICRM.UI.ASPNetMVC.CRMFilters;
using TICRM.DTOs;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using TICRM.DAL;
using System.IO;
using log4net;
using TICRM.UI.ASPNetMVC.App_Start;
using System.Web.Http.ExceptionHandling;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class AccountsController : Controller
    {
        private AccountManager am = new AccountManager();
        protected CRMEntities dbEnt = new CRMEntities();
        FirmwaresManager fManager = new FirmwaresManager();
        private DeviceSensorGraphManager dsgm = new DeviceSensorGraphManager();
        private DeviceManager dm = new DeviceManager();
        private WorkFlowManager wfm = new WorkFlowManager();
        private GraphManager gm = new GraphManager();


        // GET: Accounts
        public ActionResult Index()
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();                //Get User ID
                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                List<AccountDto> accounts = am.GetAccounts(CurrentUserId, UserRole, UserCompanyID);
                return View(accounts);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                AccountDto account = new AccountDto();
                account.AccountSizeDropdown = new SelectList(am.AccSizeDropDown(), "AccountSizeId", "Name");
                account.AccountTypeDropdown = new SelectList(am.AccTypeDropDown(), "AccountTypeId", "Name");
                account.AddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                account.BillingAddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                account.IndustryDropdown = new SelectList(am.IndustryDropDown(), "IndustryId", "Name");
                account.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                account.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                account.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                account.CurrencyDropdown = new SelectList(am.CurrencyDropDown(), "CurrencyId", "Name");
                return View(account);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");               
            }
        }

        /// <summary>
        /// Post request to create new accounts
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AccountActionFilter]
        public ActionResult Create(AccountDto account)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                if (ModelState.IsValid)
                {
                    //Checking account already exist or not
                    var AlreadyExists = dbEnt.Accounts.Any(x => x.IsDeleted != true && x.Email == account.Email && x.PhoneOffice == account.PhoneOffice);
                    if (AlreadyExists)
                    {
                        //Exist message.
                        ViewBag.exist = ExistMessage.AccountExist;
                        account.AccountSizeDropdown = new SelectList(am.AccSizeDropDown(), "AccountSizeId", "Name");
                        account.AccountTypeDropdown = new SelectList(am.AccTypeDropDown(), "AccountTypeId", "Name");
                        account.AddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                        account.BillingAddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                        account.IndustryDropdown = new SelectList(am.IndustryDropDown(), "IndustryId", "Name");
                        account.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                        account.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                        account.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                        account.CurrencyDropdown = new SelectList(am.CurrencyDropDown(), "CurrencyId", "Name");
                        return View(account);
                    }
                    else
                    {
                        //Account Create function
                        string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                        /*string UserCompanyID = Convert.ToString(Session["UserCompany"]);*/   //Pass Company ID
                        var condition = am.SaveAccount(account, CurrentUserId, UserCompanyID);
                        if (!condition)
                        {
                            //ViewBag.error = "Data Is Not Saved. Please Refresh the page.";
                            ModelState.AddModelError("", WarningMessage.DataNotSaved);
                        }
                        else
                        {
                            //Success message.
                            TempData["Success"] = SuccessMessage.AccountSubmit;
                            return RedirectToAction("Index");
                        }
                    }
                }
                //If model state is not valid then run this dropdown code
                account.AccountSizeDropdown = new SelectList(am.AccSizeDropDown(), "AccountSizeId", "Name");
                account.AccountTypeDropdown = new SelectList(am.AccTypeDropDown(), "AccountTypeId", "Name");
                account.AddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                account.BillingAddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                account.IndustryDropdown = new SelectList(am.IndustryDropDown(), "IndustryId", "Name");
                account.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                account.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                account.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                account.CurrencyDropdown = new SelectList(am.CurrencyDropDown(), "CurrencyId", "Name");

                //Warning message if any field remain blank.
                TempData["Warning"] = WarningMessage.EnterField;
                return View(account);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountDto account = am.GetAccount(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                account.AccountSizeDropdown = new SelectList(am.AccSizeDropDown(), "AccountSizeId", "Name");
                account.AccountTypeDropdown = new SelectList(am.AccTypeDropDown(), "AccountTypeId", "Name");
                account.AddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                account.BillingAddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                account.IndustryDropdown = new SelectList(am.IndustryDropDown(), "IndustryId", "Name");
                account.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                account.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                account.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                account.CurrencyDropdown = new SelectList(am.CurrencyDropDown(), "CurrencyId", "Name");
                return View(account);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        /// <summary>
        ///Edit account post action
        /// </summary>
        [HttpPost]
        //[AccountActionFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountDto account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    var condition = am.SaveAccount(account, CurrentUserId, null,true);                  
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Account Update message
                        TempData["Success"] =UpdateMessage.AccountUpdate;
                        //return RedirectToAction("Index", new { id = account.AccountId });
                        return RedirectToAction("Index");
                    }
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
                account.AccountSizeDropdown = new SelectList(am.AccSizeDropDown(), "AccountSizeId", "Name");
                account.AccountTypeDropdown = new SelectList(am.AccTypeDropDown(), "AccountTypeId", "Name");
                account.AddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street1");
                account.BillingAddressDropdown = new SelectList(am.AddresseDropDown(UserCompanyID), "AddressId", "Street2");
                account.IndustryDropdown = new SelectList(am.IndustryDropDown(), "IndustryId", "Name");
                account.StatusDropdown = new SelectList(am.StatusDropDown(), "StatusId", "Name");
                account.AssignedTeamDropdown = new SelectList(am.TeamDropDown(), "TeamId", "Name");
                account.AssignedUserDropdown = new SelectList(am.UserDropDown(), "UserId", "Name");
                account.CurrencyDropdown = new SelectList(am.CurrencyDropDown(), "CurrencyId", "Name");

                //Enter in blank field Warning message
                TempData["Warning"] = WarningMessage.EnterField;
                return View(account);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult AccountsDetail(Guid? id)
        {
            try
            { 
                if (id == null) //If condition is  true then the error page appears.
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Getting account Detail
                AccountDto account = am.GetAccount(id);
                AccountViewModel accWithDetail = am.GetAccountAndDetails(id.Value);

                string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                //Bind dropdown for WorkFlow Popup model, That's show in account detail screen(Device Tab)
                //by click on WorkFlow button
                WorkFlowDTO workflow = new WorkFlowDTO();
                workflow.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");
                workflow.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                workflow.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "Name", "Name");
                workflow.ActionDropdown = new SelectList(wfm.ActionDropDown(), "Name", "Name");
                workflow.PriorityDropDown = new SelectList(wfm.PriorityDropDown(), "Value", "Name");
                accWithDetail.workFlow = workflow;


                //Bind dropdown for Configuration Popup model, That's show in account detail screen(Device Tab)
                //by click on Configuration button

                DeviceSensorGraphDto deviceSensorGraph = new DeviceSensorGraphDto();
                deviceSensorGraph.ChannelsDropDown = new SelectList(dm.ChannelDropDown(), "Channel_Name", "Channel_Name");
                deviceSensorGraph.DataDropDown = new SelectList(dm.DataDurDropDown(), "Time", "Time");
                deviceSensorGraph.NetworksDropDown = new SelectList(dm.NetworkDropDown(), "Network_Name", "Network_Name");
                deviceSensorGraph.GraphsDropDown = new SelectList(gm.GetGraphList(), "GraphId", "GraphName");
                deviceSensorGraph.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");
                accWithDetail.deviceSensorGraph = deviceSensorGraph;

                if (account == null) //If Account detail could not fetch  then the error page appears.
                {
                    return HttpNotFound();
                }
                return View(accWithDetail);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                AccountDto account = am.GetAccount(id);
                // pass current userid
                string CurrentUserId = User.Identity.GetUserId();
                //soft delete for account
                am.SaveAccount(account, CurrentUserId, null, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FWCreate(AccountViewModel accountViewModel, HttpPostedFileBase file, string mac)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        FirmwaresController firmwaresController = new FirmwaresController();
                        //id and date
                        byte[] gb = Guid.NewGuid().ToByteArray();
                        int i = BitConverter.ToInt32(gb, 0);
                        accountViewModel.firmware.Id = i;
                        accountViewModel.firmware.File = accountViewModel.firmware.version + "_" + file.FileName;
                        bool condition = fManager.SaveFirmware(accountViewModel.firmware, false);
                        firmwaresController.PublishMqtt(file, mac, accountViewModel.firmware);
                        //In Condition check data submitted in DB successfully or not
                        if (!condition)
                        {
                            ViewBag.error = "Data Is Not Saved. Please Refresh the page.";
                        }
                        else
                        {
                            //When data submitted show successfully toaster on listing screen 
                            //TempData["Success"] = "Firmwares submitted successfully";
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }      
                }
                //Bind data with dropdown
                //firmware.DevicesDropDown = new SelectList(dm.GetDevices(), "DeviceId", "Name");
                //TempData["Warning"] = "Please enter required fields";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult WFCreate(AccountViewModel accountViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Convert.ToInt32(accountViewModel.workFlow.MinThreshold) < Convert.ToInt32(accountViewModel.workFlow.Threshold))
                    {
                        string CurrentUserId = User.Identity.GetUserId(); // get current userid

                        bool condition = wfm.SaveItWorkFlow(accountViewModel.workFlow, CurrentUserId, null, false, false);
                        if (!condition)
                        {
                            ViewBag.err = "Data Is Not Saved. Please Refresh the page.";
                        }
                        else
                        {
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DSConfiguration(AccountViewModel accountViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AlreadyExists = dbEnt.DeviceSensorGraphs.Any(x => x.IsDeleted != true && x.DeviceId == accountViewModel.deviceSensorGraph.DeviceId);
                    if (AlreadyExists)
                    {
                        //TempData["DS_Exist"] = "This Device is already configure";
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string CurrentUserId = User.Identity.GetUserId(); // get current userid
                        string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                        var condition = dsgm.SaveDeviceSensorGraph(accountViewModel.deviceSensorGraph, CurrentUserId, UserCompanyID, false, false);
                        if (!condition)
                        {
                            ModelState.AddModelError("", WarningMessage.DataNotSaved);
                        }
                        else
                        {
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        //*********************************************//
        //              Sales Tab Methods              //
        //*********************************************//

        //**  Won Leads Get Method  **//
        public JsonResult GetWonLeads(Guid id)
        {

            string guid = id.ToString();
            var WonLead = dbEnt.spGetWonLeads(guid);
            var json = JsonConvert.SerializeObject(WonLead);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //** Get Lead count from Lead source
        public JsonResult LeadCountFromSource()
        {
            var LeadCount = dbEnt.sp_LeadFromSourceCount_Get().ToList();
            return Json(LeadCount, JsonRequestBehavior.AllowGet);
        }

        //**  Get Opportuntiy Location Get Method againts account **//
        public JsonResult GetOpportunityLoc(Guid id)
        {
            string guid = id.ToString();
            var OppLoc = dbEnt.spGetOpportunityLoc(guid).ToList();
            //var json = JsonConvert.SerializeObject(mapdevice);
            return Json(OppLoc, JsonRequestBehavior.AllowGet);
        }

        //**  Current Year Sales Get method againts account  **//
        public JsonResult GetOppSaleMonthWise(Guid id)
        {
            string guid = id.ToString();
            var OppSaleMonthwise = dbEnt.spGetSalesMonthWise(guid);
            return Json(OppSaleMonthwise, JsonRequestBehavior.AllowGet);
        }

        //**  Work Order Get method againts account  **//
        public JsonResult WorkOrderDetail(Guid id)
        {
            string guid = id.ToString();
            var WorkOrderDetail = dbEnt.spWorkOrderDetail(guid).ToList();
            return Json(WorkOrderDetail, JsonRequestBehavior.AllowGet);
        }

        //**  Opportunity Get method againts account  **//
        public JsonResult GetOpportunity(Guid id)
        {
            string guid = id.ToString();
            var opportunity = dbEnt.spGetOpportunity(guid).ToList();
            return Json(opportunity, JsonRequestBehavior.AllowGet);
        }

          //*********************************************//
         //              Services Tab Methods           //
        //*********************************************//

        //**  Cases Location Get method againts account  **//
        public JsonResult GetCasesLoc(Guid id)
        {
            string guid = id.ToString();
            var CasesLoc = dbEnt.spGetCasesLoc(guid).ToList();
            //var json = JsonConvert.SerializeObject(mapdevice);
            return Json(CasesLoc, JsonRequestBehavior.AllowGet);
        }

        //**  Activities Get method againts account  **//
        public JsonResult GetAccActivity(Guid id)
        {
            string guid = id.ToString();
            var AccActivity = dbEnt.spGetAccActivity(guid).ToList();
            return Json(AccActivity, JsonRequestBehavior.AllowGet);
        }

        //**  Contact Get method againts account  **//
        public JsonResult GetContact(Guid id)
        {
            string guid = id.ToString();
            var contact = dbEnt.spGetContact(guid).ToList();
            return Json(contact, JsonRequestBehavior.AllowGet);
        }

 

        //*********************************************//
        //              Devices Tab Methods            //
        //*********************************************//


        public JsonResult GetDeviceType(Guid id)
        {
            string guid = id.ToString();
            var DeviceType = dbEnt.sp_DeviceType_Get(guid).ToList();
            return Json(DeviceType, JsonRequestBehavior.AllowGet);
        }

        //**  Device Connectivity Network Count method againts account  **//
        public JsonResult GetNetworks(Guid id)
        {
            try
            {
                var objDeviceNetworks = am.DeviceNetworkBar(id);
                return Json(objDeviceNetworks, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string logfilepath = System.Web.Configuration.WebConfigurationManager.AppSettings["logfile"];
                if (!Directory.Exists(logfilepath))
                {
                    Directory.CreateDirectory(logfilepath);
                }


                string createText = DateTime.Now.ToString() + Environment.NewLine;
                createText += e;
                System.IO.File.WriteAllText(logfilepath, createText);
                return Json("An error occured", JsonRequestBehavior.AllowGet);
            }
        }

        //**  Device Protocol Count method againts account  **//
        public JsonResult GetChannel(Guid id)
        {
            try
            {
               
                var objVM = am.Get_ChannelPercentage(id);
                return Json(objVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string logfilepath = System.Web.Configuration.WebConfigurationManager.AppSettings["logfile"];
                if (!Directory.Exists(logfilepath))
                {
                    Directory.CreateDirectory(logfilepath);
                }


                string createText = DateTime.Now.ToString() + Environment.NewLine;
                createText += e;
                System.IO.File.WriteAllText(logfilepath, createText);
                return Json("An error occured", JsonRequestBehavior.AllowGet);
            }
        }

        //**  Device Heart Beat Time method againts account  **//
        public JsonResult GetDeviceHeartBeatTime(Guid id)
        {

                string guid = id.ToString();
                var DisConnectivity = dbEnt.sp_DisConnectivityTime_Get(guid).ToList();
                return Json(DisConnectivity, JsonRequestBehavior.AllowGet);
           
        }


          //*********************************************//
         //            Work Flows Tab Methods           //
        //*********************************************//

        //**  Work Flow Get method againts account  **//
        public JsonResult GetWorkFlow(Guid id)
        {
            string guid = id.ToString();
            var WorkFlow = dbEnt.spGetWorkFlow(guid).ToList();
            return Json(WorkFlow, JsonRequestBehavior.AllowGet);
        }

        //**  Work Flow Reports Get method againts account  **//
        public JsonResult GetWorkFlowReport(Guid id)
        {
            string guid = id.ToString();
            var WorkFlowReport = dbEnt.spGetWorkFlowReports(guid).ToList();
            return Json(WorkFlowReport, JsonRequestBehavior.AllowGet);
        }

        //**  Work Flow Count  method againts account  **//
        public JsonResult GetWorkFlowCount(Guid id)
        {
            string guid = id.ToString();
            var WorkFlowCount = dbEnt.spGetWorkFlowCount(guid);
            var json = JsonConvert.SerializeObject(WorkFlowCount);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //**  Work Flow Status Get method for progress bar againts account  **//
        public JsonResult GetWorkFlowStatus(Guid id)
        {
            string guid = id.ToString();
            var WorkFlowstatus = dbEnt.spGetWorkStatus(guid);
            return Json(WorkFlowstatus, JsonRequestBehavior.AllowGet);
        }
    }
}