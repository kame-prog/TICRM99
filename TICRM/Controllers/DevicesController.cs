using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.CRMFilters;
using TICRM.DTOs;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static TICRM.Cloud.Adapter.Adaptee.IBM;

namespace TICRM.Controllers
{
    /************Devices Controller************
   Class [DevicesController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Devices like, 
   ||             navigating to the pages, getting associated modules for specific Device]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
   ||                  
    ********************************************/
    
    public class DevicesController : BaseController
    {
        //Mqtt 
        MqttClient client;
        string clientId;
        MqttClient clients;
        string clientIds;
        private DeviceManager dm = new DeviceManager();
        private DeviceSensorGraphManager dsgm = new DeviceSensorGraphManager();
        private GraphManager gm = new GraphManager();
        private DisconnectionManager dc = new DisconnectionManager();
        private CosumptionManager pc = new CosumptionManager();
        private CostsManager cm = new CostsManager();
        // GET: Devices
        public ActionResult Index()
        {
            try { 

                ViewBag.AccountId = dm.Accounts;
                ViewBag.CustomerAssetId = dm.GetAllCustomerAssets();
                ViewBag.StatusId = new SelectList(dm.Status, "StatusId", "Name");
                ViewBag.AssignedTeam = new SelectList(dm.Teams, "TeamId", "Name");
                ViewBag.AssignedUser = new SelectList(dm.Users, "UserId", "Name");
           
                return View(dm.GetDevices());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the channel counts.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetCounts()
        {
            try { 

            return Json(dsgm.GetChannelCounts(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }
        /// <summary>
        /// Device sensor graph view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult DeviceSensorGraph()
        {
            try { 
                ViewBag.AccountId = dm.Accounts;
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the list of device.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetListOfDevice()
        {
            try { 
                ViewBag.AccountId = dm.Accounts;
                return Json(dm.GetDevices(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Getting devices on the basis of customer asset Id
        /// </summary>
        /// <param name="AssetId"></param>
        /// <returns></returns>
        public JsonResult GetListOfDevicesonAssetId(Guid AssetId)
        {
            try
            {
                return Json(dm.GetDevicesonAssetId(AssetId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Ibms the cloud synchronized.
        /// </summary>
        /// <param name="CloudId">The cloud identifier.</param>
        /// <param name="AccountId">The account identifier.</param>
        /// <param name="CustomerAssetId">The customer asset identifier.</param>
        /// <param name="AssignedTeam">The assigned team.</param>
        /// <param name="AssignedUser">The assigned user.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult IBMCloudSynchronized(string CloudId,Guid AccountId, Guid CustomerAssetId, Guid AssignedTeam, Guid AssignedUser)
        {
            try { 
                var status = dm.GetIBMCloudList(CloudId, AccountId, CustomerAssetId, AssignedTeam, AssignedUser);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Synchronize with ibm.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult SynchronizedWithIBM()
        {
            try { 
                List<IBMDevicesInfo> status = dm.IBMCloudList();
                ViewBag.AccountId = new SelectList(dm.Accounts, "AccountId", "Name");
                ViewBag.CustomerAssetId = new SelectList(dm.GetAllCustomerAssets(), "CustomerAssetId", "Title");
                ViewBag.AssignedTeam = new SelectList(dm.Teams, "TeamId", "Name");
                ViewBag.AssignedUser = new SelectList(dm.Users, "UserId", "Name");
                return PartialView("_PartialIBMCloud", status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the device in ibm.
        /// </summary>
        /// <param name="dvc">The DVC.</param>
        /// <param name="ibm">The ibm.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult CreateDevice_in_IBM_INCA(DeviceDto dvc, IBMCloudViewModel ibm)
        {
            try { 
                DeviceDto data = dm.CreateNewCloudDevice(dvc,ibm);
                AccountDto accountDto = dm.Accounts.FirstOrDefault(x => x.AccountId == data.AccountId);
                CustomerAssetDto customerAsset = dm.GetAllCustomerAssets().FirstOrDefault(x => x.CustomerAssetId == data.CustomerAssetId);
                TeamDto team = dm.Teams.FirstOrDefault(x => x.TeamId == data.AssignedTeam);
                UserDto user = dm.Users.FirstOrDefault(x => x.UserId == data.AssignedUser);
                StatusDto status = dm.Status.FirstOrDefault(x => x.StatusId == data.StatusId);

                var obj = new
                {
                    assignUser = user != null ? user.Name : "",
                    assignTeam = team != null ? team.Name : "",
                    CustomerAsset = customerAsset != null ? customerAsset.Title : "",
                    Status = status != null ? status.Name : "",
                    AccountName = accountDto != null ? accountDto.Name : "",
                    DeviceId = data.DeviceId,
                    Name = data.Name,
                    EMEINumber = data.EMEINumber,
                    RegistrationDate = data.RegistrationDate.Value.ToString(),
                    Latitude = data.Latitude,
                    Longitude = data.Longitude,
                    AccountId = data.AccountId,
                    Mac = data.Mac
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Detail view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DeviceDto device = dm.GetDevice(id);
                if (device == null)
                {
                    return HttpNotFound();
                }
                return View(device);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial details view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                DeviceDto device = dm.GetDevice(id);
                return PartialView("_PartialDeviceDetails", device);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial delete view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                DeviceDto device = dm.GetDevice(id);
                return PartialView("_PartialDeviceDelete", device);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Create view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                DeviceViewModel model = new DeviceViewModel();
                model.StatusDropdown= new SelectList(dm.Status, "StatusId", "Name");
                model.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name");
                model.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name");
                model.AccountsDropdown = new SelectList(dm.Accounts, "AccountId", "Name");


                model.CustomerAssetDropdown = new SelectList(new List<CustomerAssetDto>(), "CustomerAssetId", "Title");
                model.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name");

                model.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text");

                model.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text");
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified ibm cloud.
        /// </summary>
        /// <param name="IBMCloud">The ibm cloud.</param>
        /// <param name="deviceDto">The device dto.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [DeviceActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IBMCloudViewModel IBMCloud,DeviceDto deviceDto, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (deviceDto.IsGateway == true)
                    {
                        deviceDto.IsGateway = false;
                    }
                    else
                        deviceDto.IsGateway = true;
                    IBMCloud.DeviceId = deviceDto.Name;
                    string CurrentUserId = User.Identity.GetUserId();
                    dm.SaveDevice(deviceDto, CurrentUserId, IBMCloud);

                    System.Diagnostics.Debug.WriteLine("Created");
                    if (loc != "False")
                    {
                        Guid temp = new Guid(loc);
                        return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                    }
                    else
                        return RedirectToAction("Index");

                }

                DeviceViewModel deviceViewModel = new DeviceViewModel();

                deviceViewModel.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name", deviceDto.StatusId);
                deviceViewModel.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name", deviceDto.AssignedTeam);
                deviceViewModel.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name", deviceDto.AssignedUser);
                deviceViewModel.AccountsDropdown = new SelectList(dm.Accounts, "AccountId", "Name", deviceDto.AccountId);
                deviceViewModel.CustomerAssetDropdown= new SelectList(dm.CustomerAssetsOnAccountId((Guid)deviceDto.AccountId), "CustomerAssetId", "Title", deviceDto.CustomerAssetId);
                deviceViewModel.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name", deviceDto.GatewayReference);


                deviceViewModel.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", deviceDto.CloudServices);

                deviceViewModel.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", deviceDto.Maintenance);

               
                deviceViewModel.Name = deviceDto.Name;
                deviceViewModel.Mac = deviceDto.Mac;
                deviceViewModel.EMEINumber = deviceDto.EMEINumber;
                deviceViewModel.RegistrationDate = deviceDto.RegistrationDate;
                deviceViewModel.Latitude = deviceDto.Latitude;
                deviceViewModel.Longitude = deviceDto.Longitude;
                deviceViewModel.IBMCloud = IBMCloud;
                if (loc != "False")
                {
                    Guid temp = new Guid(loc);
                    return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                }
                else
                    return View(deviceViewModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edit view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DeviceDto device = dm.GetDevice(id);
                if (device == null)
                {
                    return HttpNotFound();
                }

                DeviceViewModel deviceViewModel = new DeviceViewModel();
                deviceViewModel.DeviceId = device.DeviceId;
                deviceViewModel.Name = device.Name;
                deviceViewModel.Mac = device.Mac;
                deviceViewModel.EMEINumber = device.EMEINumber;
                deviceViewModel.RegistrationDate = device.RegistrationDate;
                deviceViewModel.Latitude = device.Latitude;
                deviceViewModel.Longitude = device.Longitude;
                if (device.CloudData == null)
                {
                    deviceViewModel.IBMCloud = new IBMCloudViewModel();
                }
                else
                {
                    deviceViewModel.IBMCloud = Newtonsoft.Json.JsonConvert.DeserializeObject<IBMCloudViewModel>(device.CloudData);
                }


                deviceViewModel.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name", device.StatusId);
                deviceViewModel.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name", device.AssignedTeam);
                deviceViewModel.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name", device.AssignedUser);
                deviceViewModel.AccountsDropdown = new SelectList(dm.Accounts, "AccountId", "Name", device.AccountId);
                deviceViewModel.CustomerAssetDropdown = new SelectList(dm.CustomerAssetsOnAccountId((Guid)device.AccountId), "CustomerAssetId", "Title", device.CustomerAssetId);
                deviceViewModel.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name", device.GatewayReference);


                deviceViewModel.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", device.CloudServices);

                deviceViewModel.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", device.Maintenance);


                return View(deviceViewModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified device dto.
        /// </summary>
        /// <param name="deviceDto">The device dto.</param>
        /// <param name="IBMCloud">The ibm cloud.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceDto deviceDto, IBMCloudViewModel IBMCloud)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dm.SaveDevice(deviceDto, null, IBMCloud, true);
                    return RedirectToAction("Index");
                }
               
                DeviceViewModel deviceViewModel = new DeviceViewModel();

                deviceViewModel.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name", deviceDto.StatusId);
                deviceViewModel.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name", deviceDto.AssignedTeam);
                deviceViewModel.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name", deviceDto.AssignedUser);
                deviceViewModel.AccountsDropdown = new SelectList(dm.Accounts, "AccountId", "Name", deviceDto.AccountId);
                deviceViewModel.CustomerAssetDropdown = new SelectList(dm.CustomerAssetsOnAccountId((Guid)deviceDto.AccountId), "CustomerAssetId", "Title", deviceDto.CustomerAssetId);
                deviceViewModel.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name", deviceDto.GatewayReference);


                deviceViewModel.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", deviceDto.CloudServices);

                deviceViewModel.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", deviceDto.Maintenance);



                deviceViewModel.Name = deviceDto.Name;
                deviceViewModel.Mac = deviceDto.Mac;
                deviceViewModel.EMEINumber = deviceDto.EMEINumber;
                deviceViewModel.RegistrationDate = deviceDto.RegistrationDate;
                deviceViewModel.Latitude = deviceDto.Latitude;
                deviceViewModel.Longitude = deviceDto.Longitude;
                deviceViewModel.IBMCloud = IBMCloud;
                return View(deviceViewModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DeviceDto device = dm.GetDevice(id);
                if (device == null)
                {
                    return HttpNotFound();
                }
                return View(device);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                DeviceDto device = dm.GetDevice(id);
                dm.SaveDevice(device, null, null, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
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
        /// <summary>
        /// Device Details view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Device()
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
        /// Updates device serivce date.
        /// </summary>
        /// <param name="mac">The mac.</param>
        /// <param name="date">The date.</param>
        /// <exception cref="System.Exception"></exception>
        [HttpGet]
        public void UpdateDeviceSerivceDate(string mac, string date)
        {
            try
            {
                dm.UpdateDeviceServiceDate(mac, date); // pass mac and date to device manager
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Gets the devices dropdown list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetDevicesDropdownList()
        {
            try
            {
                List<DeviceDto> list = dm.GetDevices(); // get list of Devices from Device Manager
                return Json(list, JsonRequestBehavior.AllowGet); // return json List
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets all devices long lat.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAlldevicesLongLat()
        {
            try
            {
                List<DeviceDto> list = dm.GetDevices(); // get list of Devices from Device Manager
                                                        //var LongLat = list.GroupBy(i => new { i.Longitude, i.Latitude }).Select(g => g.First()).ToList();
                return Json(list, JsonRequestBehavior.AllowGet); // return json List
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// get a device the long lat.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetsingledevicesLongLat(string accountId)
        {
            try
            {
                List<DeviceDto> list = dm.GetDevices(new Guid(accountId)); // get list of Devices from Device Manager
                                                                           //var LongLat = list.GroupBy(i => new { i.Longitude, i.Latitude }).Select(g => g.First()).ToList();
                return Json(list, JsonRequestBehavior.AllowGet); // return json Lis
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets all disconnection list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAllDisconnectionList()
        {
            try
            {
                var list = dc.GetDisconnections();
                var data = list.GroupBy(x => x.Date)
                    .Select(x => new
                    {
                        Value = x.Count(),
                        date = (DateTime)x.Key

                    }).ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        public JsonResult GetAccountDisconnectionList(Guid AccountId)
        {
            try
            {
                var list = dc.GetAccountDisconnections(AccountId);
                var data = list.GroupBy(x => x.Date)
                    .Select(x => new
                    {
                        Value = x.Count(),
                        date = (DateTime)x.Key

                    }).ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Gets all consumption.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAllConsumption()
        {
            try
            {
                var list = pc.GetDisconnections();

                var data = list.GroupBy(x => x.Date)
                    .Select(x => new
                    {
                        Value = x.Count(),
                        date = (DateTime)x.Key
                    }).ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets all costs.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAllCosts()
        {
            try
            {
                var list = cm.GetCosts();
                var data = list.GroupBy(x => x.Date)
                    .Select(x => new
                    {
                        Value = x.Count(),
                        date = (DateTime)x.Key
                    }).ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the sensor drop down list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetSensorDropDownList()
        {
            try
            {
                List<SensorDto> list = dsgm.GetSensorList(); // get sensor list from device sensor graph manager
                return Json(list, JsonRequestBehavior.AllowGet); // return json list
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the graph drop down list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetGraphDropDownList()
        {
            try
            {
                List<GraphDto> list = gm.GetGraphList(); // get graph List from graph manager
                return Json(list, JsonRequestBehavior.AllowGet); // return json list
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the customer assets for account Id.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetCustomerAssetsForDD(Guid accountId)
        {
            try
            {
                SelectList data = new SelectList(dm.CustomerAssetsOnAccountId(accountId), "CustomerAssetId", "Title");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Submits the device sensor graph.
        /// </summary>
        /// <param name="DeviceId">The device identifier.</param>
        /// <param name="SensorId">The sensor identifier.</param>
        /// <param name="GraphId">The graph identifier.</param>
        /// <param name="Data">The data.</param>
        /// <param name="Channel">The channel.</param>
        /// <param name="Network">The network.</param>
        /// <param name="Level">The level.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string SubmitDeviceSensorGraph(Guid DeviceId, Guid SensorId, Guid GraphId, String Data, String Channel, String Network, int? Level)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = dsgm.SubmitDeviceSensorGraph(DeviceId, SensorId, GraphId, CurrentUserId, Data, Channel, Network, Level); // save data in device sensor graph manager

                if (condition == true) //check condition if is true return success otherwise error
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
        /// Gets the device sensor graph list.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string GetDeviceSensorGraphList()
        {
            try
            {
                return dsgm.GetDeviceSensorGraphList(); // get list device sensor graph and return in response
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the graph of mac data.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string GetGraphOfMACData(string MacAddress)
        {
            try 
            { 
                return dsgm.DeviceSensorGraphOfMAC(MacAddress); // get device sensor graph on mac address and return in response
             }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the graph of asset data.
        /// </summary>
        /// <param name="AssetId">The asset identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string GetGraphOfAssetData(Guid AssetId)
        {
            try {
                return dsgm.DeviceSensorGraphOfAsset(AssetId); // get device sensor graph on mac address and return in response
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Get the device sensor graph on id.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DSGonDeviceId(Guid deviceId)
        {
            try {
                return dsgm.GetDSGListOn_DeviceId(deviceId); // get device sensor graph on deviceId and return json in response
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Updates the device sensor graph.
        /// </summary>
        /// <param name="DeviceSensorGraphId">The device sensor graph identifier.</param>
        /// <param name="DeviceId">The device identifier.</param>
        /// <param name="SensorId">The sensor identifier.</param>
        /// <param name="GraphId">The graph identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string UpdateDeviceSensorGraph(Guid DeviceSensorGraphId, Guid DeviceId, Guid SensorId, Guid GraphId)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = dsgm.UpdateDeviceSensorGraph(DeviceSensorGraphId, DeviceId, SensorId, GraphId, CurrentUserId); // update device sensor graph in dsgManager
                if (condition == true) //check condition if is true return success otherwise return error
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
        /// Edits the device sensor graph.
        /// </summary>
        /// <param name="DeviceSensorGraphId">The device sensor graph identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult EditDeviceSensorGraph(Guid DeviceSensorGraphId)
        {
            try
            {
                DeviceSensorGraphDto data = new DeviceSensorGraphDto(); // create an object of a class
                data = dsgm.GetDeviceSensorGraphOnId(DeviceSensorGraphId); // get data from device sensor graph from DeviceSensorGraphManager
                return Json(data, JsonRequestBehavior.AllowGet); // return Json in Response
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Sends the message to MQTT.
        /// </summary>
        /// <param name="deviceStatus">if set to <c>true</c> [device status].</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult SendMessageToMqtt(bool deviceStatus)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(deviceStatus);
                string status = "error";

                //MQTT
                string BrokerAddress = "broker.hivemq.com";
                client = new MqttClient(BrokerAddress, 1883, false, MqttSslProtocols.None, null, null);
                clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                System.Diagnostics.Debug.WriteLine("Connected");
                String Topic = "ServerGateway";
                String state = deviceStatus.ToString();
                state = "R-A-" + state;
                client.Publish(Topic, Encoding.UTF8.GetBytes(state), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

                
                return Content(status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Sends the message to MQTTS.
        /// </summary>
        /// <param name="silderValue">The silder value.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SendMessageToMqtts(int silderValue)
        {
            string status = "error";

            try
            {
                System.Diagnostics.Debug.WriteLine(silderValue);


                //MQTT
                string BrokerAddresss = "broker.hivemq.com";
                clients = new MqttClient(BrokerAddresss, 1883, false, MqttSslProtocols.None, null, null);
                clientIds = Guid.NewGuid().ToString();
                clients.Connect(clientIds);
                System.Diagnostics.Debug.WriteLine("Connected");
                String Topic = "ServerGateway";
                String value = silderValue.ToString();
                value = "D-A-" + value;
                clients.Publish(Topic, Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Content(status);
        }

        /// <summary>
        /// Create from the account.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Mac">The mac.</param>
        /// <param name="EMEI">The emei.</param>
        /// <param name="Reg">The reg.</param>
        /// <param name="Lat">The lat.</param>
        /// <param name="Long">The long.</param>
        /// <param name="Asset">The asset.</param>
        /// <param name="User">The user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Main">The main.</param>
        /// <param name="Cloud">The cloud.</param>
        /// <param name="Status">The status.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult CreatefromAccount(String Name, String Mac, String EMEI, String Reg, String Lat, String Long, String Asset, String User, String Team, String Main, String Cloud, String Status, Guid AccID  )
        {
            try
            {
                DeviceDto dvdt = new DeviceDto();
                dvdt.AccountId = AccID;
                dvdt.Name = Name;
                dvdt.Mac = Mac;
                dvdt.EMEINumber = EMEI;
                dvdt.RegistrationDate = Convert.ToDateTime(Reg);
                dvdt.Latitude = Convert.ToDecimal(Lat);
                dvdt.Longitude = Convert.ToDecimal(Long);
                dvdt.CustomerAssetId = Guid.Parse(Asset);
                dvdt.AssignedUser = Guid.Parse(User);
                dvdt.AssignedTeam = Guid.Parse(Team);
                dvdt.Maintenance = Main;
                dvdt.CloudServices = Cloud;
                dvdt.StatusId = Guid.Parse(Status);
                dm.SaveDevice(dvdt,null);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        ///  create the devices in bulk.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Mac">The mac.</param>
        /// <param name="EMEI">The emei.</param>
        /// <param name="Asset">The asset.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult BulkCreate(String Name, String Mac, String EMEI, String Asset, Guid AccID)
        {
            try{
                DeviceDto dvdt = new DeviceDto();
                dvdt.AccountId = AccID;
                dvdt.Name = Name;
                dvdt.Mac = Mac;
                dvdt.EMEINumber = EMEI;
                dvdt.CustomerAssetId = Guid.Parse(Asset);
                dm.SaveDevice(dvdt,null);
                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes device sensor graph on id.
        /// </summary>
        /// <param name="DeviceSensorGraphId">The device sensor graph identifier.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        public string DeleteDeviceSensorGraph(Guid DeviceSensorGraphId)
        {
            try
            {
                string CurrentUserId = User.Identity.GetUserId();       // get current userid
                bool status = dsgm.DeleteDeviceSensorGraph(DeviceSensorGraphId); // delete DeviceSensorGraph in DeviceSensorGraphManager and get true if successfully deleted otherwise it return false

                if (status == true)//check condition if is true return success otherwise error
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
        /// Return a string with all activities for DataTable to render
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns></returns>
        public string GetDevicesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            var obj = dm.GetDevicesList(sEcho, iDisplayStart, iDisplayLength, sSearch);


            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Name).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Mac).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Mac).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.EMEINumber).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.EMEINumber).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Account.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Account.Name).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CustomerAsset.Title).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CustomerAsset.Title).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AssignedTeam).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AssignedTeam).ToList();
                    }
                    break;
                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.RegistrationDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.RegistrationDate).ToList();
                    }
                    break;

                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AssignedUser).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AssignedUser).ToList();
                    }
                    break;
            }

            int totalRecord = dm.GetTotalCount();

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("{");
            sb.Append("\"sEcho\": ");
            sb.Append(sEcho);
            sb.Append(",");
            sb.Append("\"iTotalRecords\": ");
            sb.Append(totalRecord);
            sb.Append(",");
            sb.Append("\"iTotalDisplayRecords\": ");
            sb.Append(totalRecord);
            sb.Append(",");
            sb.Append("\"aaData\": ");
            sb.Append(JsonConvert.SerializeObject(obj));
            sb.Append("}");
            return sb.ToString();
        }

    }

}   
