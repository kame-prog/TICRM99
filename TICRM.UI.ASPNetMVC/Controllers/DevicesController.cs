using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
//using TICRM.CRMFilters;
using TICRM.DAL;
using TICRM.DTOs;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;
using log4net;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Resources;
using TICRM.UI.ASPNetMVC.Helpers;


namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class DevicesController : Controller
    {

        MqttClient client;
        string clientId;
        //MqttClient clients;
        //string clientIds;
        private DeviceManager dm = new DeviceManager();
        private GraphManager gm = new GraphManager();
        private DeviceSensorGraphManager dsgm = new DeviceSensorGraphManager();
        CustomerAssetManager cam = new CustomerAssetManager();
        protected CRMEntities dbEnt = new CRMEntities();
        private decimal latitude ;
        private decimal longitude ;
        private string deviceidsingle;
        

        // GET: Devices
        public ActionResult Index(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    //id --> Account ID,,,,When we come from account page to device page then this id use
                    List<DeviceDto> device = dm.GetDevices(id);

                    //Return the list of devicest that's are related to the Account ID.
                    return View(device);
                }
                else
                {
                    //Get Current User ID
                    string CurrentUserId = User.Identity.GetUserId();

                    //Get current User Role and convert it to a string
                    string UserRole = Convert.ToString(Session["UserRole"]);

                    //Get User Company ID and convert it to a string
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                    //Retrive the all devices using GetDevices() method.
                    List<DeviceDto> device = dm.GetDevices(CurrentUserId,UserRole, UserCompanyID);
                    //Return value on the view in the form of device object.
                    return View(device);
                }
               
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
        
        

        [HttpGet]
        [Route("GettrackingDevicesmap")]
        public JsonResult GettrackingDevicesmap(string deviceid)
        {
            string clientId = Guid.NewGuid().ToString();
            var client = new MqttClient("broker.hivemq.com");
            deviceidsingle = deviceid;
            client.Connect(clientId);
            //string latitude = "", longitude = "";
            while (client.IsConnected)
            {
                // Subscribe to the topic to receive messages

                // client.Subscribe(new string[] { "Device/Pub/ygHP16.hIq" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                // client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                JsonResult result = GetDeviceSensorGraphByDeviceId(deviceid);
                string MqttPublishtopic="";
                if (result.Data != null && result.Data is DeviceSensorGraph devicedb)
                {
                    MqttPublishtopic = devicedb.MqttPublishtopic;
                }
                
                if(!string.IsNullOrEmpty(MqttPublishtopic))
                {
                    client.Subscribe(new string[] { MqttPublishtopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    string jsonLocations = " ";

                    client.MqttMsgPublishReceived += (sender, e) =>
                    {
                        string receivedMessage = Encoding.UTF8.GetString(e.Message);
                        string[] parts = receivedMessage.Split(',', ':');

                        // Find the index of latitude and longitude
                        int latIndex = Array.IndexOf(parts, "Satellites Located - Latitude") + 1;
                        int lonIndex = Array.IndexOf(parts, " Longitude") + 1;

                        // Extract latitude and longitude
                        string rawLatitude = parts[latIndex].Trim();
                        string rawLongitude = parts[lonIndex].Trim();

                        // Use decimal.TryParse to convert the raw strings into decimal
                        bool isLatitudeValid = decimal.TryParse(rawLatitude, out latitude);
                        bool isLongitudeValid = decimal.TryParse(rawLongitude, out longitude);
                        System.Diagnostics.Debug.WriteLine("Received message latitude-e : " + latitude);
                        System.Diagnostics.Debug.WriteLine("Received message longitude-e : " + longitude + deviceid);
                        //GetLatestDeviceCoordinates(latitude, longitude);
                        GetLocationMethods(deviceid);



                    };

                    JsonResult results = GetsingleDevice(deviceid);
                    // Check if the JsonResult has a valid data object
                    if (results != null && results.Data is DeviceDto device)
                    {

                        if (latitude == null || latitude == 0)
                        {

                            latitude = device.Latitude ?? 0;
                        }

                        if (longitude == null || longitude == 0)
                        {
                            longitude = device.Longitude ?? 0;
                        }
                    }
                    var deviceDtos = new DeviceDto
                    {

                        Name = "Device",
                        Latitude = latitude,
                        Longitude = longitude
                    };

                    // Serialize the JSON object
                    //string jsonLocation2 = JsonConvert.SerializeObject(deviceDtos);

                    return Json(deviceDtos, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
                }

            }


            var deviceDto = new DeviceDto
            {
                
                Name = "Device",
                Latitude = latitude,
                Longitude = longitude
            };
            

            // Serialize the JSON object
            string jsonLocation = JsonConvert.SerializeObject(deviceDto);

            return Json(jsonLocation, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("GetDeviceSensorGraphByDeviceId")]
        public JsonResult GetDeviceSensorGraphByDeviceId(string deviceid)
        {
            if (deviceid != null)
            {
                var deviceIdGuid = new Guid(deviceid);  // Convert string to GUID
                var deviceSensorGraphs = dbEnt.DeviceSensorGraphs  // Access the DeviceSensorGraph table
                    .Where(dsg => dsg.DeviceId == deviceIdGuid)  // Filter by deviceid
                    .ToList();  // Get all records matching the deviceid

                if (deviceSensorGraphs.Any())  // If there's at least one matching record
                {
                    // Prepare the data to be returned as JSON
                    var result = deviceSensorGraphs.FirstOrDefault();

                    return Json(result, JsonRequestBehavior.AllowGet);  // Return the data as JSON
                }
                else
                {
                    return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);  // Return a message if no data is found
                }
            }

            return Json(new { message = "Invalid deviceid" }, JsonRequestBehavior.AllowGet);  // If deviceid is null or invalid
        }
        [HttpGet]
        [Route("GetLocationMethods")]
        public void GetLocationMethods(string deviceid)
        {
            if(deviceid !=null)
            {
                var deviceIdGuid = new Guid(deviceid);
                var device = dbEnt.Devices.SingleOrDefault(d => d.DeviceId == deviceIdGuid);
                // var device = dbEnt.Devices.SingleOrDefault(d => d.DeviceId == deviceid);

                if (device != null)
                {
                    // Update the latitude and longitude
                    device.Latitude = latitude;
                    device.Longitude = longitude;

                    // Save changes to the database
                    dbEnt.SaveChanges();
                }
                //longitude = longitude;
                //latitude = latitude;
                //var location = new
                //{
                //    Name = "Devices",
                //    Latitude = latitude,
                //    Longitude = longitude
                //};
               // System.Diagnostics.Debug.WriteLine("Received message longitude-e : " + latitude + "-" + longitude);
              //  return Json(location, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpGet]
        [Route("GetsingleDevice")]
        public JsonResult GetsingleDevice(string deviceid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceid))
                {
                    return Json(new { error = "Device ID cannot be null or empty." }, JsonRequestBehavior.AllowGet);
                }

                Guid deviceIdGuid;
                if (!Guid.TryParse(deviceid, out deviceIdGuid))
                {
                    return Json(new { error = "Invalid device ID format." }, JsonRequestBehavior.AllowGet);
                }

                var device = dbEnt.Devices.SingleOrDefault(d => d.DeviceId == deviceIdGuid);

                if (device == null)
                {
                    return Json(new { error = "Device not found." }, JsonRequestBehavior.AllowGet);
                }

                var deviceDto = new DeviceDto
                {
                    
                    Name = device.Name,
                    Latitude = device.Latitude,
                    Longitude = device.Longitude
                };

                return Json(deviceDto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Route("GetLatestDeviceCoordinates")]
        public JsonResult GetLatestDeviceCoordinates(string latitude, string longitude)
        {
           // var location = new { Name = "Devices", Latitude = latitude, Longitude = longitude };
            var locations = new
            {
                Name = "Devices",
                Latitude = latitude,
                Longitude = longitude
            };

            // Serialize the JSON object
            string jsonLocation = JsonConvert.SerializeObject(locations);

            return Json(jsonLocation, JsonRequestBehavior.AllowGet);

        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle the incoming message here
            string message = Encoding.UTF8.GetString(e.Message);
            // Do whatever you need to do with the received message
            System.Diagnostics.Debug.WriteLine("Received message: " + message);
        }
        public ActionResult Create()
        {
            try
            {
                //Get the User Company ID and convert it into the String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                //Populate the values to the dropdowns.
                DeviceDto model = new DeviceDto();
                model.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                model.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                model.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                model.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                model.CustomerAssetDropdown = new SelectList(cam.GetAllCustomerAssets(), "CustomerAssetId", "Title");
                model.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name");

                model.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = CloudServiceForDD.Swuich, Value = CloudServiceForDD.Swuich},
                    new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
                    new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
                    new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
                    new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text");

                model.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
                    new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
                    new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text");

                //Return value to the view
                return View(model);
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
        /// Creates the specified ibm cloud.
        /// </summary>
        //[DeviceActionFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IBMCloudViewModel IBMCloud, DeviceDto deviceDto)
        {
            try
            {
                //Get current company ID and convert it into the String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                // Check if the model is valid.
                if (ModelState.IsValid)
                {
                    // Check if the device already exists
                    var AlreadyExists = dbEnt.Devices.Any(x => x.IsDeleted != true && x.Mac == deviceDto.Mac || x.EMEINumber == deviceDto.EMEINumber);
                    if (AlreadyExists)
                    {
                        //Set exist messge in the ViewBag
                        ViewBag.exist = ExistMessage.DeviceExist;

                        //Create object of the DeviceDto
                        DeviceDto DVmodel = new DeviceDto();

                        // Populate dropdowns with the relevant value
                        DVmodel.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                        DVmodel.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                        DVmodel.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                        DVmodel.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                        DVmodel.CustomerAssetDropdown = new SelectList(cam.GetAllCustomerAssets(), "CustomerAssetId", "Title");
                        DVmodel.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name");

                        DVmodel.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = CloudServiceForDD.Swuich, Value = CloudServiceForDD.Swuich},
                    new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
                    new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
                    new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
                    new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text");

                        DVmodel.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
                    new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
                    new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text");

                        //Return value on view
                        return View(DVmodel);
                    }
                    else
                    {
                        deviceDto.IsGateway = true;
                        //IBMCloud.DeviceId = deviceDto.Name;

                        //Get current userid
                        string CurrentUserId = User.Identity.GetUserId();
                        //string UserCompanyID = Convert.ToString(Session["UserCompany"]);    //Pass Company ID

                        //Save the device using SaveDevice() method.
                        var condition = dm.SaveDevice(deviceDto, CurrentUserId,UserCompanyID, IBMCloud);

                        //System.Diagnostics.Debug.WriteLine("Created");     
                        if (!condition)
                        {
                            //If condition is null return the Modelstate error.
                            ModelState.AddModelError("", WarningMessage.DataNotSaved);
                        }
                        else
                        {
                            //set success message in the TempData.
                            //TempData["Success"] = "Device submitted successfully";
                            TempData["Success"] = SuccessMessage.DeviceSuccess;

                            // Redirect to the Index action method
                            return RedirectToAction("Index");
                        }
                    }
                }
                //Populate dropdown if the model state is not valid.
                //DeviceViewModel deviceViewModel = new DeviceViewModel();
                DeviceDto model = new DeviceDto();
                model.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                model.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                model.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                model.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                model.CustomerAssetDropdown = new SelectList(cam.GetAllCustomerAssets(), "CustomerAssetId", "Title");
                model.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name");

                model.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = CloudServiceForDD.Swuich, Value = CloudServiceForDD.Swuich},
                    new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
                    new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
                    new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
                    new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text");

                model.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
                    new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
                    new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text");

                //set Warning message in the TempData.
                TempData["Warning"] = WarningMessage.EnterField;

                //Return value to the view in form of contact object
                return View(model);
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
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the device  based on the provided id
                DeviceDto device = dm.GetDevice(id);
                if (device == null)
                {
                    // If the device is null, return a "Not Found" status
                    return HttpNotFound();
                }
                //Bind dropdown values with edit form
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
                //Get User company ID and convert it into the string.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);  

                // Populate the dropdowns for the edit view
                deviceViewModel.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name", device.StatusId);
                deviceViewModel.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name", device.AssignedTeam);
                deviceViewModel.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name", device.AssignedUser);
                deviceViewModel.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name", device.AccountId);
                deviceViewModel.CustomerAssetDropdown = new SelectList(cam.GetAllCustomerAssets(), "CustomerAssetId", "Title", device.CustomerAssetId);
                deviceViewModel.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name", device.GatewayReference);

                deviceViewModel.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = CloudServiceForDD.Swuich, Value = CloudServiceForDD.Swuich},
                    new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
                    new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
                    new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
                    new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", device.CloudServices);

                deviceViewModel.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
                    new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
                    new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", device.Maintenance);

                ViewBag.Latitude = device.Latitude;
                ViewBag.Longitude = device.Longitude;

                // Return the view with the populated object
                return View(deviceViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceDto deviceDto, IBMCloudViewModel IBMCloud)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the current user ID.
                    string CurrentUserId = User.Identity.GetUserId();

                    //Edit the device using SaveDevice() method.
                    var condition = dm.SaveDevice(deviceDto, CurrentUserId, null, IBMCloud, true,false);
                    if (!condition)
                    {
                        //If this false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Set a success message in tempdata
                        TempData["Success"] = UpdateMessage.DeviceUpdate;

                        //Redirect to the index action method
                        return RedirectToAction("Index");
                    }

                }
                //Get current  User Company id
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);  
                
                //Create deviceviewmodel object
                DeviceViewModel deviceViewModel = new DeviceViewModel();

                //Populate values to the corresponding dropdown.
                deviceViewModel.StatusDropdown = new SelectList(dm.StatusDropDown(), "StatusId", "Name");
                deviceViewModel.AssignedTeamDropdown = new SelectList(dm.TeamDropDown(), "TeamId", "Name");
                deviceViewModel.AssignedUserDropdown = new SelectList(dm.UserDropDown(), "UserId", "Name");
                deviceViewModel.AccountsDropdown = new SelectList(dm.AccountDropDown(UserCompanyID), "AccountId", "Name");
                deviceViewModel.CustomerAssetDropdown = new SelectList(cam.GetAllCustomerAssets(), "CustomerAssetId", "Title");
                deviceViewModel.GatewayReferenceDropdown = new SelectList(dm.GetGatewayDevices(), "DeviceId", "Name");

                deviceViewModel.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = CloudServiceForDD.Swuich, Value = CloudServiceForDD.Swuich},
                    new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
                    new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
                    new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
                    new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text");

                deviceViewModel.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
                    new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
                    new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
                    new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text");

                //Set Warning message in the TempData
                TempData["Warning"] = WarningMessage.EnterField;


                //Return value to the view with the populated object.
                return View(deviceViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }


        public ActionResult DeviceDetail(Guid? id)
        {
            try
            {
                if (id == null) 
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the device detail  based on the provided id
                DeviceDto device = dm.GetDevice(id);

                if (device == null)    
                {
                    // If the Contact object is null, return a "Not Found" status
                    return HttpNotFound();
                }
                // Return the ContactDetail view with the Contact object
                return View(device);
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
                // Retrieve the device to be deleted based on the provided id
                DeviceDto device = dm.GetDevice(id);

                //Delete the device using SaveDevice() method.
                dm.SaveDevice(device, null,null, null,true, true);

                //Redirect to the Index Action method.
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

        public ActionResult DeviceSensorGraphList()
        {
            try
            {
                //Get Current User ID
                string CurrentUserId = User.Identity.GetUserId();

                //Get current User Role and convert it to a string
                string UserRole = Convert.ToString(Session["UserRole"]);

                //Get User Company ID and convert it to a string
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                //Retrive DeviceSensorGraph using GetDeviceSensorGraphs_List() method.
                List<DeviceSensorGraphDto> deviceSensorGraphDtos = dsgm.GetDeviceSensorGraphs_List(CurrentUserId, UserRole, UserCompanyID);

                //Return value on the view.
                return View(deviceSensorGraphDtos);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult CreateDeviceSensorGraph()
        {
            try
            {
                //Get the User Company ID and convert it into the Stri
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                //Create the object of DeviceSensorGraphDto
                DeviceSensorGraphDto deviceSensorGraph = new DeviceSensorGraphDto();

                //Populate the values to the dropdowns.
                deviceSensorGraph.ChannelsDropDown = new SelectList(dm.ChannelDropDown(), "Channel_Name", "Channel_Name");
                deviceSensorGraph.DataDropDown = new SelectList(dm.DataDurDropDown(), "Time", "Time");
                deviceSensorGraph.NetworksDropDown = new SelectList(dm.NetworkDropDown(), "Network_Name", "Network_Name");
                deviceSensorGraph.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                deviceSensorGraph.GraphsDropDown = new SelectList(gm.GetGraphList(), "GraphId", "GraphName");
                deviceSensorGraph.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");


                //Return value to the view 
                return View(deviceSensorGraph);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeviceSensorGraph(DeviceSensorGraphDto deviceSensorGraph)
        {
            try
            {
                //Get current company ID and convert it into the String.
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                // Check if the model is valid.
                if (ModelState.IsValid)
                {
                    // Check if the device sensor graph already exists
                    var AlreadyExists = dbEnt.DeviceSensorGraphs.Any(x => x.IsDeleted != true && x.DeviceId == deviceSensorGraph.DeviceId);
                    if (AlreadyExists)
                    {
                        //Set exist messge in the ViewBag
                        ViewBag.exist = ExistMessage.DeviceSensorGraphExist;

                        // Populate dropdowns with the relevant value
                        deviceSensorGraph.ChannelsDropDown = new SelectList(dm.ChannelDropDown(), "Channel_Name", "Channel_Name");
                        deviceSensorGraph.DataDropDown = new SelectList(dm.DataDurDropDown(), "Time", "Time");
                        deviceSensorGraph.NetworksDropDown = new SelectList(dm.NetworkDropDown(), "Network_Name", "Network_Name");
                        deviceSensorGraph.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                        deviceSensorGraph.GraphsDropDown = new SelectList(gm.GetGraphList(), "GraphId", "GraphName");
                        deviceSensorGraph.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");
                        //deviceSensorGraph.MqttPublishtopic = new SelectList(dsgm.GetSensorList(), "MqttPublishtopic", "MqttPublishtopic");
                        //Return value on view
                        return View(deviceSensorGraph);
                    }
                    else
                    {
                        // Get current userid
                        string CurrentUserId = User.Identity.GetUserId();

                        //Save the device sensor graph using SaveDeviceSensorGraph() method.
                        var condition = dsgm.SaveDeviceSensorGraph(deviceSensorGraph, CurrentUserId, UserCompanyID, false, false);
                        if (!condition)
                        {
                            //If condition is null return the Modelstate error.
                            ModelState.AddModelError("", WarningMessage.DataNotSaved);
                        }
                        else
                        {
                            //set success message in the TempData.
                            TempData["Success"] = SuccessMessage.DeviceSensorSubmit;

                            // Redirect to the Index action method
                            return RedirectToAction("DeviceSensorGraphList");
                        }
                    }

                }
                //Populate dropdown if the model state is not valid.
                deviceSensorGraph.ChannelsDropDown = new SelectList(dm.ChannelDropDown(), "Channel_Name", "Channel_Name");
                deviceSensorGraph.DataDropDown = new SelectList(dm.DataDurDropDown(), "Time", "Time");
                deviceSensorGraph.NetworksDropDown = new SelectList(dm.NetworkDropDown(), "Network_Name", "Network_Name");
                deviceSensorGraph.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                deviceSensorGraph.GraphsDropDown = new SelectList(gm.GetGraphList(), "GraphId", "GraphName");
                deviceSensorGraph.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");

                //set Warning message in the TempData.
                TempData["Warning"] = WarningMessage.EnterField;

                //Return value to the view
                return View(deviceSensorGraph);

            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult EditDeviceSensorGraph(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    //Get User Company
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); 

                    // Retrieve the deviceSensorGraph  based on the provided id
                    DeviceSensorGraphDto deviceSensorGraph = dsgm.GetDeviceSensorGraphOnId(id);

                    // Populate the dropdowns for the edit view
                    deviceSensorGraph.ChannelsDropDown = new SelectList(dm.ChannelDropDown(), "Channel_Name", "Channel_Name");
                    deviceSensorGraph.DataDropDown = new SelectList(dm.DataDurDropDown(), "Time", "Time");
                    deviceSensorGraph.NetworksDropDown = new SelectList(dm.NetworkDropDown(), "Network_Name", "Network_Name");
                    deviceSensorGraph.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                    deviceSensorGraph.GraphsDropDown = new SelectList(gm.GetGraphList(), "GraphId", "GraphName");
                    deviceSensorGraph.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");

                    // Return the view with the populated object
                    return View(deviceSensorGraph);
                }
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDeviceSensorGraph(DeviceSensorGraphDto deviceSensorGraph)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get current userid
                    string CurrentUserId = User.Identity.GetUserId();

                    //Edit the device sensor graph using SaveDeviceSensorGraph() method.
                    var condition = dsgm.SaveDeviceSensorGraph(deviceSensorGraph, CurrentUserId,null, true, false);
                    if (!condition)
                    {
                        //If is false / not true then return the model state error
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //Set a success message in tempdata
                        TempData["Success"] = UpdateMessage.DeviceSensorUpdate;
                        //Redirect to the index action method
                        return RedirectToAction("DeviceSensorGraphList");
                    }

                }
                //Get current  User Company id
                string UserCompanyID = Convert.ToString(Session["UserCompany"]);

                //Populate values to the corresponding dropdown.
                deviceSensorGraph.ChannelsDropDown = new SelectList(dm.ChannelDropDown(), "Channel_Name", "Channel_Name");
                deviceSensorGraph.DataDropDown = new SelectList(dm.DataDurDropDown(), "Time", "Time");
                deviceSensorGraph.NetworksDropDown = new SelectList(dm.NetworkDropDown(), "Network_Name", "Network_Name");
                deviceSensorGraph.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                deviceSensorGraph.GraphsDropDown = new SelectList(gm.GetGraphList(), "GraphId", "GraphName");
                deviceSensorGraph.SensorsDropDown = new SelectList(dsgm.GetSensorList(), "SensorId", "SensorName");
                
                //Set Warning message in the TempData
                TempData["Warning"] = WarningMessage.EnterField;


                //Return value to the view with the populated object.
                return View(deviceSensorGraph);

            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult DeviceSensorGraphDetail(Guid? id)
        {
            try
            {
                if (id == null) 
                {
                    // If the id parameter is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // Retrieve the device sensor graph detail  based on the provided id
                DeviceSensorGraphDto deviceSensorGraph = dsgm.GetDeviceSensorGraphOnId(id);

                if (deviceSensorGraph == null)    
                {
                    // If the Contact object is null, return a "Not Found" status
                    return HttpNotFound();
                }
                // Return the ContactDetail view with the Contact object
                return View(deviceSensorGraph);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult DeleteDeviceSensorGraph(Guid id)
        {
            try
            {
                // Retrieve the device sensor graph to be deleted based on the provided id
                DeviceSensorGraphDto deviceSensorGraph = dsgm.GetDeviceSensorGraphOnId(id);

                //Get current user id
                string CurrentUserId = User.Identity.GetUserId();

                //Delete the device sensor graph using SaveDeviceSensorGraph() method.
                dsgm.SaveDeviceSensorGraph(deviceSensorGraph, CurrentUserId,null, true, true);

                //Redirect to the Index Action method.
                return RedirectToAction("DeviceSensorGraphList");

            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }

        }


        //public ActionResult SaveLatLng(double lat, double lng)
        //{
        //    // Do something with the received data
        //    return Json(new { success = true });
        //}


        //Send data from Website to HIVE MQTT Server
      
        [HttpPost]
        public ActionResult SendMessageToMqtt(string deviceStatus, string deviceID)
        {
            try
            {
                //deviceStatus = true;
                System.Diagnostics.Debug.WriteLine(deviceStatus);
                string status = "error";

                //MQTT
                string BrokerAddress = "broker.hivemq.com";
                client = new MqttClient(BrokerAddress, 1883, false, MqttSslProtocols.None, null, null);
                //clientId = Guid.NewGuid().ToString();
                clientId = deviceID;
                client.Connect(clientId);
                System.Diagnostics.Debug.WriteLine("Connected");
                //String Topic = "ServerGateway";
                String Topic = "/swuich/Device001/updateState";
                String state = deviceStatus;
                
                //state = "R-A-" + state;                    

                client.Publish(Topic, Encoding.UTF8.GetBytes(state), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

                return Content(status);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult ReceiveMessageToMqtt(string deviceID)
        {
            try
            {
                // Create a new MQTT client with a unique client ID
                string clientId = Guid.NewGuid().ToString();
                var client = new MqttClient("broker.hivemq.com");
                client.Connect(deviceID);
                // Subscribe to the "ServerGateway" topic
                client.Subscribe(new string[] { "ServerGateway" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                // Define the message arrived event handler
                string latestMessage = "";
                client.MqttMsgPublishReceived += (sender, e) =>
                {
                    latestMessage = Encoding.UTF8.GetString(e.Message);
                };
                // Wait for messages to arrive
                Thread.Sleep(5000);
                // Disconnect MQTT client
                client.Disconnect();
                ViewBag.LatestMessage = latestMessage;
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
            
        }


    }
}