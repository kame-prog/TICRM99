using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TICRM.DAL;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Xml.Linq;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using Microsoft.AspNet.Identity;
using log4net;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;
//using TICRM.CRMFilters;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class DashboardController : Controller
    {
        protected CRMEntities dbEnt = new CRMEntities();
        private DeviceManager deviceManager = new DeviceManager();
        private AccountManager am = new AccountManager();
        private DeviceManager dm = new DeviceManager();
        CustomerAssetManager cam = new CustomerAssetManager();
        WorkOrderManager wom = new WorkOrderManager();
        OpportunityManager om = new OpportunityManager();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        //Quick Action:: Account Popup modal method :: GET Method
        public ActionResult AccountModalPopup()
        {
            try
            {
                AccountDto account = new AccountDto();
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
                return PartialView("~/Views/Dashboard/_AccountPopupPV.cshtml", account);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                return PartialView("~/Views/Shared/Error.cshtml"); // Return an error partial view to the user
            } 
        }
        //Quick Action:: Account Popup modal method :: POST Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AccountActionFilter]
        public JsonResult AccountModalPopup_Post(AccountDto account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Checking account already exist or not
                    var AlreadyExists = dbEnt.Accounts.Any(x => x.IsDeleted != true && x.Email == account.Email && x.PhoneOffice == account.PhoneOffice);
                    if (AlreadyExists)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Account Create function
                        string CurrentUserId = User.Identity.GetUserId();      // pass current userid
                        string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                        var condition = am.SaveAccount(account, CurrentUserId, UserCompanyID);
                        if (!condition)
                        {
                            ViewBag.error = WarningMessage.DataNotSaved;
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
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                throw;
            }
        }
       
        //Quick Action:: Device Popup modal method :: GET Method
        public ActionResult DeviceModalPopup()
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Bind dropdown values with create form
                DeviceDto device = new DeviceDto();
                device.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                device.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                device.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                device.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                device.CustomerAssetDropdown = new SelectList(cam.GetAllCustomerAssets(), "CustomerAssetId", "Title");
                device.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name");

                device.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = CloudServiceForDD.Swuich, Value = CloudServiceForDD.Swuich},
                    new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
                    new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
                    new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
                    new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text");

                device.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
                    new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
                    new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text");
                return PartialView("~/Views/Dashboard/_DevicePopupPV.cshtml", device);
            }
            catch (Exception ex)
            {  // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                return PartialView("~/Views/Shared/Error.cshtml"); // Return an error partial view to the user
            }
        }
        //Quick Action:: Device Popup modal method :: POST Method
        //[DeviceActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeviceModalPopup_Post(IBMCloudViewModel IBMCloud, DeviceDto deviceDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AlreadyExists = dbEnt.Devices.Any(x => x.IsDeleted != true && x.Mac == deviceDto.Mac || x.EMEINumber == deviceDto.EMEINumber);
                    if (AlreadyExists)
                    {
                        return Json("Exist", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        deviceDto.IsGateway = true;
                         string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                        string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                        var condition = dm.SaveDevice(deviceDto, CurrentUserId,UserCompanyID, IBMCloud);
                        if (!condition)
                        {
                            ViewBag.error = WarningMessage.DataNotSaved;
                        }
                        else
                        {
                            //When data submitted, show successfully toaster on listing screen 
                            return Json("success", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json("DateError", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                throw;
            }
        }
       
        //Quick Action:: Opportunity Popup modal method :: GET Method
        public ActionResult OppModalPopup()
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Show all dropdown with values on Create opportuntiy page
                OpportunityDto opportunity = new OpportunityDto();
                opportunity.AccountsDropdown = new SelectList(om.AccountDropDown(UserCompanyID), "AccountId", "Name");
                opportunity.StatusDropdown = new SelectList(om.StatusDropDown(), "StatusId", "Name");
                opportunity.AssignedTeamDropdown = new SelectList(om.TeamDropDown(), "TeamId", "Name");
                opportunity.AssignedUserDropdown = new SelectList(om.UserDropDown(), "UserId", "Name");
                opportunity.CurrencyDropdown = new SelectList(om.CurrencyDropDown(), "CurrencyId", "Name");
                opportunity.OpportunityStageDropdown = new SelectList(om.OppStageDropDown(), "OpportunityStageId", "Name");
                opportunity.ProbabilityDropdown = new SelectList(om.ProbabilityDropDown(), "ProbabilityId", "Name");
                return PartialView("~/Views/Dashboard/_OpportunityPopupPV.cshtml", opportunity);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                return PartialView("~/Views/Shared/Error.cshtml"); // Return an error partial view to the user
            }
        }
        //Quick Action:: Opportunity Popup modal method :: POST Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OppModalPopup_Post(OpportunityDto opportunity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    var condition = om.SaveOpportunity(opportunity, CurrentUserId, UserCompanyID);
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                        ViewBag.error = WarningMessage.DataNotSaved;
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                throw;
            }
        }
       
        //Quick Action:: Work Order Popup modal method :: GET Method
        public ActionResult WOModalPopup()
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                //Bind dropdown data
                WorkOrderDto workorder = new WorkOrderDto();
                workorder.WorkStagesDropdown = new SelectList(wom.WorkStageDropDown(), "WorkStageId", "Name");
                workorder.StatusDropdown = new SelectList(wom.StatusDropDown(), "StatusId", "Name");
                workorder.AssignedUserDropdown = new SelectList(wom.UserDropDown(), "UserId", "Name");
                workorder.AssignedTeamDropdown = new SelectList(wom.TeamDropDown(), "TeamId", "Name");
                workorder.AccountsDropdown = new SelectList(wom.AccountDropDown(UserCompanyID), "AccountId", "Name");
                return PartialView("~/Views/Dashboard/_WorkOrderPopupPV.cshtml", workorder);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                return PartialView("~/Views/Shared/Error.cshtml"); // Return an error partial view to the user
            }
        }
        //Quick Action:: Work Order Popup modal method :: POST Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult WOModalPopup_Post(WorkOrderDto workorder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    //Work Order Create function
                    bool condition = wom.SaveWorkOrder(workorder, CurrentUserId, UserCompanyID);
                    if (!condition)
                    {
                        ViewBag.error = WarningMessage.DataNotSaved;
                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                throw;
            }
        }

        //**************************************//
        // This Week, Month, Year Sales Methods //
        //**************************************//

        //All Year Sales Get from opportunitiy Table
        public JsonResult GetAllYearSale()
        {
            //Show All Year Sales according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var AllYearSales = dbEnt.spGetOpportuniySales(UserCompanyID);
            var json = JsonConvert.SerializeObject(AllYearSales);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Week Sales Get from opportunitiy Table
        public JsonResult GetThisWeekSales()
        {
            //Show This Week Sales according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisWeekSale = dbEnt.sp_ThisWeekSales_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisWeekSale);
            return Json(json, JsonRequestBehavior.AllowGet);
        }       
        //This Month Sales Get from opportunitiy Table
        public JsonResult GetThisMonthSales()
        {
            //Show This Month Sales according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisMonthSale = dbEnt.sp_ThisMonthSales_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisMonthSale);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Year Sales Get from opportunitiy Table
        public JsonResult GetThisYearSales()
        {
            //Show This Year Sales according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisYearSale = dbEnt.sp_ThisYearSales_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisYearSale);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        //**************************************//
        // This Week, Month, Year Cost Methods  //
        //**************************************//

        //All Year Opportunity count Get from Opportunity Table
        public JsonResult GetAllYearOpp()
        {
            //Show All Year opportunity according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var AllYearOpp = dbEnt.sp_AllYearOpp_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(AllYearOpp);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Week Opportunity count Get from Opportunity Table
        public JsonResult GetThisWeekOpp()
        {
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisWeekOpp = dbEnt.sp_ThisWeekOpp_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisWeekOpp);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Month Opportunity count Get from Opportunity Table
        public JsonResult GetThisMonthOpp()
        {
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisMonthOpp = dbEnt.sp_ThisMonthOpp_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisMonthOpp);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Year Opportunity count Get from Opportunity Table
        public JsonResult GetThisYearOpp()
        {
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisYearOpp = dbEnt.sp_ThisYearOpp_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisYearOpp);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


          //*********************************************//
         //  This Week, Month, Year Work-Order Methods  //
        //*********************************************//

        //All Year Work-Order Get from Work-Order Table
        public JsonResult GetAllYearWorkOrder()
        {
            //Show All Year Work Order according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var AllYearWorkOrder = dbEnt.spWorkOrderCount(UserCompanyID);
            var json = JsonConvert.SerializeObject(AllYearWorkOrder);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Weak Work-Order Get from Work-Order Table
        public JsonResult GetThisWeekWorkOrder()
        {
            //Show this week Work Order according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var ThisWeekWorkOrder = dbEnt.sp_ThisWeekWorkOrder_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(ThisWeekWorkOrder);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Month Work-Order Get from Work-Order Table
        public JsonResult GetThisMonthWorkOrder()
        {
            //Show This Month Work Order according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var MonthWorkOrder = dbEnt.sp_ThisMonthWorkOrder_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(MonthWorkOrder);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Year Work-Order Get from Work-Order Table
        public JsonResult GetThisYearWorkOrder()
        {
            //Show This Year Work Order according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var YearWorkOrder = dbEnt.sp_ThisYearWorkOrder_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(YearWorkOrder);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


          //*********************************************//
         //  This Week, Month, Year Lead Count Methods  //
        //*********************************************//

        //All Year Lead Get from Lead Table
        public JsonResult GetAllYearLead()
        {
            //Show All Year Lead according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var AllYearLead = dbEnt.spLeadCount(UserCompanyID);
            var json = JsonConvert.SerializeObject(AllYearLead);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Week Lead Get from Lead Table
        public JsonResult GetThisWeekLead()
        {
            //Show This Week Lead according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var WeeKLead = dbEnt.sp_ThisWeekLead_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(WeeKLead);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Month Lead Get from Lead Table
        public JsonResult GetThisMonthLead()
        {
            //Show This Month Lead according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var MonthLead = dbEnt.sp_ThisMonthLead_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(MonthLead);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //This Year Lead Get from Lead Table
        public JsonResult GetThisYearLead()
        {
            //Show This Year Lead according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var YearLead = dbEnt.sp_ThisYearLead_Get(UserCompanyID);
            var json = JsonConvert.SerializeObject(YearLead);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


          //**********************************************//
         //  This Week, Month, Year Lead Report Methods  //
        //**********************************************//

        //All Year Lead Report
        public JsonResult GetAllYearsLeadRepo()
        {
            //Show Leads reports in table according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Compan
            var AllYearsLeadRepo = dbEnt.spGetLeadsReport(UserCompanyID).ToList();
            return Json(AllYearsLeadRepo, JsonRequestBehavior.AllowGet);
        }
        //This Week Report
        public JsonResult GetWeekLeadRepo()
        {
            //Show Leads reports in table according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Compan
            var ThisWeekLeadRepo = dbEnt.sp_ThisWeekLeadsRepo_Get(UserCompanyID).ToList();
            return Json(ThisWeekLeadRepo, JsonRequestBehavior.AllowGet);
        }
        //This Month Report
        public JsonResult GetMonthLeadRepo()
        {
            //Show Leads reports in table according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Compan
            var ThisMonthLeadRepo = dbEnt.sp_ThisMonthLeadsRepo_Get(UserCompanyID).ToList();
            return Json(ThisMonthLeadRepo, JsonRequestBehavior.AllowGet);
        }
        //This Year Report
        public JsonResult GetYearLeadRepo()
        {
            //Show Leads reports in table according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Compan
            var ThisYearLeadRepo = dbEnt.sp_ThisYearLeadsRepo_Get(UserCompanyID).ToList();
            return Json(ThisYearLeadRepo, JsonRequestBehavior.AllowGet);
        }


          //************************************************//
         //  Today, This Week, This Year Activity Methods  //
        //************************************************//

        //Today Activity
        public JsonResult GetTodayActivities()
        {
            //Show Activity according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var TodayActivities = dbEnt.sp_TodayActivity_Get(UserCompanyID).ToList();
            return Json(TodayActivities, JsonRequestBehavior.AllowGet);
        }
        //Yesterday Activity
        public JsonResult GetYesterdayActivities()
        {
            //Show Activity according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var YesterdayActivities = dbEnt.sp_YesterdayActivity_Get(UserCompanyID).ToList();
            return Json(YesterdayActivities, JsonRequestBehavior.AllowGet);
        }
        //This Week Activity
        public JsonResult GetThisWeekActivities()
        {
            //Show Activity according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var WeekActivities = dbEnt.sp_ThisWeekActivity_Get(UserCompanyID).ToList();
            return Json(WeekActivities, JsonRequestBehavior.AllowGet);
        }
        //All Year Activity
        public JsonResult GetAllYearsActivities()
        {
            //Show Activity according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var activities = dbEnt.spGetActivities(UserCompanyID).ToList();
            return Json(activities, JsonRequestBehavior.AllowGet);
        }




        //**  Get Cloud devices  count  **//
        public JsonResult GetCloudDevice()
        {
            try
            {
                //Show just own company device in dount graph
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Compan
                var clouddevice = deviceManager.Get_CloudDevicesCount(UserCompanyID);
                return Json(clouddevice, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        //**  Get Live devices Information **//
        public JsonResult GetLiveDeviceInfo()
        {
            //Show Activity according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var DeviceInfo = dbEnt.spGetLiveDevice(UserCompanyID).ToList();
            //var json = JsonConvert.SerializeObject(LeadReport);
            return Json(DeviceInfo, JsonRequestBehavior.AllowGet);
        }

        //**  Get All devices location **//
        public JsonResult GetDevicemap()
        {
            //Show Device in Map according to their company and if any user login show data according to their account
            string CurrentUserId = User.Identity.GetUserId();                //Get User ID
            string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var mapdevice = dbEnt.spGetDevice(CurrentUserId,UserRole,UserCompanyID).ToList();
            //var json = JsonConvert.SerializeObject(mapdevice);
            return Json(mapdevice, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GettrackingDevicesmap()
        {
            //Show Device in Map according to their company and if any user login show data according to their account
            string CurrentUserId = User.Identity.GetUserId();                //Get User ID
            string UserRole = Convert.ToString(Session["UserRole"]);         //Get User Role
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Company
            var mapdevice = dbEnt.spGetDevice(CurrentUserId, UserRole, UserCompanyID).ToList();
            //var json = JsonConvert.SerializeObject(mapdevice);

            return Json(mapdevice, JsonRequestBehavior.AllowGet);
        }

        //**  Get All Month opportunties count  **//
        public JsonResult GetOppMonthWise()
        {
            //Show opportunity in Graph according to their company
            string UserCompanyID = Convert.ToString(Session["UserCompany"]);   //Get User Compan
            var OppMonthwise = dbEnt.spGetOppMonthWise(UserCompanyID);
            

            return Json(OppMonthwise, JsonRequestBehavior.AllowGet);
        }

        
    }
}