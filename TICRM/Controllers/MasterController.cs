using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Master Controller************
Class [MasterController] 
||  Author:  [Undefined]
||
||  Purpose:  [The class serves all the functionlities which are shared among 
                all the controllers and views]
||
||  Inherits From:  [Controller]
||
||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
||                  
********************************************/
    
    public class MasterController : BaseController
    {
        private AccountManager am = new AccountManager();
        private DeviceManager deviceManager = new DeviceManager();
        private OpportunityManager om = new OpportunityManager();
        private OpportunityDto opportunity = new OpportunityDto();
        private DeviceDto deviceDto = new DeviceDto();
        private LocationManager lm = new LocationManager();
        private CustomerAssetManager cam = new CustomerAssetManager();
        private LocationManager locationManager = new LocationManager();
        private WorkOrderManager wom = new WorkOrderManager();
        private WorkOrderDto workOrder = new WorkOrderDto();
        private ActivityDTO activity = new ActivityDTO();


        
        /// <summary>
        /// Master index file
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                ViewBag.AccountId = new SelectList(om.Accounts, "AccountId", "Name");
                ViewBag.CurrencyId = new SelectList(om.Currencies, "CurrencyId", "Name", opportunity.CurrencyId);
                //ViewBag.AssignedUser = new SelecctList(om.Users, "UserId", "Name", opportunity.AssignedUser);
                ViewBag.Maintenance = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", deviceDto.Maintenance);
                ViewBag.CloudServices = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", deviceDto.CloudServices);
                ViewBag.CustomerAssetTypeId = new SelectList(cam.CustomerAssetTypes, "CustomerAssetTypeId", "Name");
                ViewBag.Type = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", activity.Type);

                System.Diagnostics.Debug.WriteLine("olamba");

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the cloud services and return to browser via ajax in json format.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetCloud()
        {
            try
            {
                SelectList data = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", deviceDto.CloudServices);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the teams.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetTeams()
        {
            try
            {
                SelectList data = new SelectList(om.Teams, "TeamId", "Name", opportunity.AssignedTeam);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the maintanance.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetMaintanance()
        {
            try
            {
                SelectList data = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", deviceDto.Maintenance);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetCurrency()
        {
            try
            {
                SelectList data = new SelectList(om.Currencies, "CurrencyId", "Name", opportunity.CurrencyId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetUsers()
        {
            try
            {
                SelectList data = new SelectList(om.Users, "UserId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the accounts.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetAccounts()
        {
            try
            {
                SelectList data = new SelectList(om.Accounts, "AccountId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetAddresses()
        {
            try
            {
                SelectList data = new SelectList(lm.Addresses, "AddressId", "Street1");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the work stage.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetWorkStage()
        {
            try
            {
                SelectList data = new SelectList(wom.WorkStages, "WorkStageId", "Name", workOrder.WorkOrderStageId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the type of the location.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetLocationType()
        {
            try
            {
                SelectList data = new SelectList(lm.LocationTypes, "LocationTypeId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the location identifier.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetLocationID()
        {
            try
            {
                SelectList data = new SelectList(new List<LocationDto>(), "LocationId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the probability identifier.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetProbabilityId()
        {
            try
            {
                SelectList data = new SelectList(om.Probabilities, "ProbabilityId", "Name", opportunity.ProbabilityId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the oppurtunity stage identifier.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetOppurtunityStageId()
        {
            try
            {
                SelectList data = new SelectList(om.OpportunityStages, "OpportunityStageId", "Name", opportunity.OpportunityStageId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the status identifier.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetStatusId()
        {
            try
            {
                SelectList data = new SelectList(om.Status, "StatusId", "Name", opportunity.StatusId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the size of the account.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetAccountSize()
        {
            try
            {
                SelectList data = new SelectList(am.AccountSizes, "AccountSizeId", "Name");
                //string json = JsonConvert.SerializeObject(data);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the type of the account.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetAccountType()
        {
            try
            {
                SelectList data = new SelectList(am.AccountTypes, "AccountTypeId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the account industry.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetAccountIndustry()
        {
            try
            {
                SelectList data = new SelectList(am.Industries, "IndustryId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
    }
}