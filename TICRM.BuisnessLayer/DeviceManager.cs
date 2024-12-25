using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;
using static TICRM.Cloud.Adapter.Adaptee.IBM;
using static TICRM.DTOs.DeviceViewModel;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [DeviceManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Crud operations are being performed here, Getting device detils
    ||             on asset Id, mac, location. Updating devices services date, Gettig ibm 
    ||             cloud list, getting gateway devices
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class DeviceManager : BaseManager
    {
        public DeviceManager()
        {

        }

        /// <summary>
        /// Updates the device service date.
        /// </summary>
        /// <param name="mac">The mac.</param>
        /// <param name="date">The date.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UpdateDeviceServiceDate(string mac, string date)
        {
            try
            {
                InsertEventLog("UpdateDeviceServiceDate", EventType.Log, EventColor.yellow, "Successfully Enter", "TICRM.BuisnessLayer.DeviceManager.UpdateDeviceServiceDate", "");

                Device device = dbEnt.Devices.Where(a => a.Mac == mac).FirstOrDefault();
                if (device != null)
                {
                    InsertEventLog("UpdateDeviceServiceDate", EventType.Log, EventColor.yellow, "device is not null", "TICRM.BuisnessLayer.DeviceManager.UpdateDeviceServiceDate", "");
                    dbEnt.Entry(device).State = EntityState.Modified;
                    //var dt = DateTime.Parse(date);
                    DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //update service date 
                    device.ServiceDate = dt;
                    device.ServiceDateFlag = false;
                    dbEnt.SaveChanges();
                }
                else
                {
                    InsertEventLog("UpdateDeviceServiceDate", EventType.Log, EventColor.yellow, "device is null on mac and date", "TICRM.BuisnessLayer.DeviceManager.UpdateDeviceServiceDate", "");
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("UpdateDeviceServiceDate", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.UpdateDeviceServiceDate", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return true;
        }

        /// <summary>
        /// Gets the gateway device count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetGatewayDeviceCount()
        {
            try
            {
                InsertEventLog("GetGatewayDeviceCount", EventType.Log, EventColor.yellow, "Entered To get Device Count For Gateways", "TICRM.BuisnessLayer.DeviceManager.GetGatewayDeviceCount", "");

                return dbEnt.Devices.Where(a => a.IsGateway == true).Count();
                
            }
            catch(Exception ex)
            {
                InsertEventMonitor("GetGatewayDeviceCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetGatewayDeviceCount", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DeviceDto.</returns>
        public DeviceDto GetDevice(Guid? guid)
        {
            try
            {
                var devices = dbEnt.Devices.Find(guid);
                return objMapper.GetDeviceDTO(devices);

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevice on deviceid", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetDevice", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the ibm cloud list.
        /// </summary>
        /// <param name="CloudId">The cloud identifier.</param>
        /// <param name="AccountId">The account identifier.</param>
        /// <param name="CustomerAssetId">The customer asset identifier.</param>
        /// <param name="AssignedTeam">The assigned team.</param>
        /// <param name="AssignedUser">The assigned user.</param>
        /// <returns>System.String.</returns>
        public string GetIBMCloudList(string CloudId, Guid AccountId, Guid CustomerAssetId, Guid AssignedTeam, Guid AssignedUser)
        {
            string status = "";
            List<IBMDevicesInfo> registerDevicesInfos = new List<IBMDevicesInfo>();
            List<IBMCloudViewModel> ibm_List = new List<IBMCloudViewModel>();
            try
            {
                List<Device> devices = dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true).ToList();
                TICRM.Cloud.Adapter.CloudMain cloudMain = new Cloud.Adapter.CloudMain();

                foreach (Device item in devices.CollectionNotNull())
                {
                    if (item.CloudData != null && item.CloudData != "")
                    {
                        IBMCloudViewModel IBMCloudView = new IBMCloudViewModel();

                        IBMCloudViewModel qIBMCloudView = Newtonsoft.Json.JsonConvert.DeserializeObject<IBMCloudViewModel>(item.CloudData) as IBMCloudViewModel;
                        ibm_List.Add(qIBMCloudView);
                    }
                }

                registerDevicesInfos = cloudMain.GetAllIBMCloudDevice(ibm_List);


                foreach (IBMDevicesInfo item in registerDevicesInfos.CollectionNotNull())
                {
                    //if (item.APIKey == null && item.AuthToken == null){continue;}

                    IBMCloudViewModel condition = ibm_List.FirstOrDefault(x => x.DeviceId == item.deviceId && x.DeviceType == item.typeId);

                    Device get_Device = devices.FirstOrDefault(x => x.Name == item.deviceId);


                    if (get_Device == null && condition == null)
                    {

                        Device d = new Device();
                        d.AccountId = AccountId;
                        d.CustomerAssetId = CustomerAssetId;
                        d.AssignedTeam = AssignedTeam;
                        d.AssignedUser = AssignedUser;
                        d.CloudData = "";
                        d.CloudServices = "";
                        d.CreatedDate = DateTime.Now;
                        //d.DeciveConfigurations = "";
                        d.DeviceId = Guid.NewGuid();
                        //d.DeviceSensors = "";
                        d.EMEINumber = item.deviceInfo.serialNumber;
                        d.IsDeleted = false;
                        d.Latitude = (decimal)item.location.latitude;
                        d.Longitude = (decimal)item.location.longitude;
                        d.Mac = item.deviceInfo.model;
                        d.Maintenance = DeviceMaintenance.None;
                        d.Name = item.deviceId;
                        d.RegistrationDate = Convert.ToDateTime(item.registration.date);
                        d.StatusId = new Guid("192f959f-2dfa-4d41-8464-dd482325dc6c");

                        //IBMCloudViewModel ibm = new IBMCloudViewModel();
                        ////ibm.OrganizationId = item.
                        ////ibm.APIKey = item.
                        ////ibm.AppId =item.
                        //ibm.AuthToken = item.authToken;
                        //ibm.DeviceId = item.deviceId;
                        ////ibm.DeviceTokken = item
                        //ibm.DeviceType = item.typeId;

                        dbEnt.Devices.Add(d);
                        dbEnt.SaveChanges();

                    }
                }


            }
            catch (Exception ex)
            {   
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return status;
        }


        /// <summary>
        /// Ibms the cloud list.
        /// </summary>
        /// <returns>List&lt;IBMDevicesInfo&gt;.</returns>
        public List<IBMDevicesInfo> IBMCloudList()
        {
            //string status = "";
            List<IBMDevicesInfo> registerDevicesInfos = new List<IBMDevicesInfo>();
            List<IBMCloudViewModel> ibm_List = new List<IBMCloudViewModel>();
            List<IBMDevicesInfo> return_list = new List<IBMDevicesInfo>();

            try
            {
                List<Device> devices = dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true).ToList();
                TICRM.Cloud.Adapter.CloudMain cloudMain = new Cloud.Adapter.CloudMain();

                foreach (Device item in devices.CollectionNotNull())
                {
                    if (item.CloudData != null && item.CloudData != "")
                    {
                        IBMCloudViewModel IBMCloudView = new IBMCloudViewModel();

                        IBMCloudViewModel qIBMCloudView = Newtonsoft.Json.JsonConvert.DeserializeObject<IBMCloudViewModel>(item.CloudData) as IBMCloudViewModel;
                        ibm_List.Add(qIBMCloudView);
                    }
                }

                registerDevicesInfos = cloudMain.GetAllIBMCloudDevice(ibm_List);

                foreach (IBMDevicesInfo item in registerDevicesInfos.CollectionNotNull())
                {
                    //if (item.APIKey == null && item.AuthToken == null){continue;}

                    IBMCloudViewModel condition = ibm_List.FirstOrDefault(x => x.DeviceId == item.deviceId && x.DeviceType == item.typeId);

                    Device get_Device = devices.FirstOrDefault(x => x.Name == item.deviceId); // here device id is not a int its is string and it come from ibm

                    if (get_Device == null && condition == null)
                    {
                        return_list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return return_list;
        }

        /// <summary>
        /// Get device list 
        /// </summary>
        /// <returns></returns>
        /// 
        public List<DeviceDto> GetDevices(string CurrentUserId, string UserRole, string UserCompanyID)
        {
            try
            {
                InsertEventLog("GetDevices", EventType.Log, EventColor.yellow, "To get Device Dto list ", "TICRM.BuisnessLayer.DeviceManager.GetDevices", "");
                List<DeviceDto> deviceDtos = new List<DeviceDto>();
                List<IBMCloudViewModel> ibmList = new List<IBMCloudViewModel>();

                //List<Device> devices = dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true).ToList();
                List<Device> devices = dbEnt.sp_Devices_Get(CurrentUserId, UserRole, UserCompanyID).ToList();
                foreach (Device item in devices.CollectionNotNull())
                {
                    deviceDtos.Add(objMapper.GetDeviceDTO(item));
                }
                return deviceDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevices", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetDevices", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the deviceson asset identifier.
        /// </summary>
        /// <param name="AssetId">The asset identifier.</param>
        /// <returns>List&lt;DeviceDto&gt;.</returns>
        public List<DeviceDto> GetDevicesonAssetId(Guid AssetId)
        {
            try
            {
                InsertEventLog("GetDevicesonAssetId", EventType.Log, EventColor.yellow, "To get Device Dto list on AssetId ", "TICRM.BuisnessLayer.DeviceManager.GetDevicesonAssetId", "");
                List<DeviceDto> deviceDtos = new List<DeviceDto>();
                List<IBMCloudViewModel> ibmList = new List<IBMCloudViewModel>();

                List<Device> devices = dbEnt.Devices.Where(a => a.CustomerAssetId == AssetId).ToList();
                foreach (Device item in devices)
                {
                    deviceDtos.Add(objMapper.GetDeviceDTO(item));
                }
                return deviceDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevicesonAssetId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetDevicesonAssetId", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Devices the count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int DeviceCount()
        {
            try
            {
                InsertEventLog("DeviceCount", EventType.Log, EventColor.yellow, "to get total no of devices count", "TICRM.BuisnessLayer.DeviceManager.DeviceCount", "");
;
                return dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true).Count();
               
            }
            catch (Exception ex)
            {
                InsertEventMonitor("DeviceCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.DeviceCount", "");
                throw;
            }
            
        }

        /// <summary>
        /// Gets the device mac.
        /// </summary>
        /// <param name="Mac">The mac.</param>
        /// <returns>IQueryable.</returns>
        public IQueryable getDeviceMac(String Mac)
        {
            IQueryable Device  = dbEnt.Devices.Where(a => a.Mac == Mac);
            return Device;
        }

        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>List&lt;DeviceDto&gt;.</returns>
        public List<DeviceDto> GetDevices(Guid? accountId)
        {
            try
            {
                InsertEventLog("GetDevices", EventType.Log, EventColor.yellow, "To get Device Dto list on accountid=" + accountId + " ", "TICRM.BuisnessLayer.DeviceManager.GetDevices", "");
                List<DeviceDto> deviceDtos = new List<DeviceDto>();
                List<Device> devices = dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true && a.AccountId == accountId).ToList();
                foreach (Device item in devices.CollectionNotNull())
                {
                    deviceDtos.Add(objMapper.GetDeviceDTO(item));
                }
                return deviceDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevices on Accountid", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetDevices", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the devices on assets identifier.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>List&lt;DeviceDto&gt;.</returns>
        public List<DeviceDto> GetDevicesOnAssetsId(Guid assetId)
        {
            try
            {
                InsertEventLog("GetDevices", EventType.Log, EventColor.yellow, "To get Device Dto list on assetId=" + assetId + " ", "TICRM.BuisnessLayer.DeviceManager.GetDevicesOnAssetsId", "");
                List<DeviceDto> deviceDtos = new List<DeviceDto>();
                List<Device> devices = dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true && a.CustomerAssetId == assetId).ToList();
                foreach (Device item in devices.CollectionNotNull())
                {
                    deviceDtos.Add(objMapper.GetDeviceDTO(item));
                }
                return deviceDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevices on Accountid", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetDevicesOnAssetsId", "");
                throw;
            }

        }

        /// <summary>
        /// Gets the gateway devices.
        /// </summary>
        /// <returns>List&lt;DeviceDto&gt;.</returns>
        public List<DeviceDto> GetGatewayDevices()
        {
            try
            {
                InsertEventLog("GetGatewayDevices", EventType.Log, EventColor.yellow, "To get Device Dto list for gateway"  + " ", "TICRM.BuisnessLayer.DeviceManager.GetGatewayDevices", "");
                List<DeviceDto> deviceDtos = new List<DeviceDto>();
                List<Device> devices = dbEnt.Devices.Include(d => d.Status).Include(d => d.Team).Include(d => d.User).Where(a => a.IsDeleted != true && a.IsGateway == true).ToList();
                foreach (Device item in devices.CollectionNotNull())
                {
                    deviceDtos.Add(objMapper.GetDeviceDTO(item));
                }
                return deviceDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevices on Accountid", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.GetDevicesOnAssetsId", "");
                throw;
            }
        }

        /// <summary>
        /// save and edit DeviceDto 
        /// </summary>
        /// <param name="dvc"></param>
        /// <returns></returns>
        public bool SaveDevice(DeviceDto dvc, string CurrentUserId,string UserCompanyID, IBMCloudViewModel ibm = null,  bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveDevice", EventType.Log, EventColor.yellow, "Successfully Enter ", "TICRM.BuisnessLayer.DeviceManager.SaveDevice", "");
                Device device;                
                if (isEditMode)
                {
                    InsertEventLog("SaveDevice", EventType.Log, EventColor.yellow, "Enter in Edit Mode To Save Edit Device on id ", "TICRM.BuisnessLayer.DeviceManager.SaveDevice", "");
                    device = objMapper.GetDevice(dvc);
                    Device ObjDevice = dbEnt.Devices.FirstOrDefault(x => x.DeviceId == device.DeviceId);
                    if (ObjDevice!=null)     //here we check data is present in DB or not
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveDevice", EventType.Log, EventColor.yellow, "Enter in Delete Mode To Delete Device ", "TICRM.BuisnessLayer.DeviceManager.SaveDevice", "");
                            Device devicefordelete = dbEnt.Devices.FirstOrDefault(x => x.DeviceId == device.DeviceId);
                            devicefordelete.IsDeleted = true;
                        }
                        else
                        {
                            if (dvc.CloudServices == CloudServiceForDD.IBM)
                            {
                                TICRM.Cloud.Adapter.CloudMain cloudMain = new Cloud.Adapter.CloudMain();
                                string result = cloudMain.IBMCloudCoverage(dvc, ibm, "update");
                                if (result == "error") { return false; }
                            }
                            ObjDevice.Name = dvc.Name;
                            ObjDevice.Mac = dvc.Mac;
                            ObjDevice.EMEINumber = dvc.EMEINumber;
                            ObjDevice.RegistrationDate = dvc.RegistrationDate;
                            ObjDevice.Latitude = dvc.Latitude;
                            ObjDevice.Longitude = dvc.Longitude;
                            ObjDevice.StatusId = dvc.StatusId;
                            ObjDevice.AccountId = dvc.AccountId;
                            ObjDevice.CustomerAssetId = dvc.CustomerAssetId;
                            ObjDevice.AssignedUser = dvc.AssignedUser;
                            ObjDevice.AssignedTeam = dvc.AssignedTeam;
                            ObjDevice.Maintenance = dvc.Maintenance;
                            ObjDevice.ServiceDate = dvc.ServiceDate;
                            ObjDevice.Maintenance = dvc.Maintenance;
                            ObjDevice.UpdatedDate = DateTime.Now;
                            ObjDevice.UpdatedBy = CurrentUserId;
                            ObjDevice.CloudServices = dvc.CloudServices;
                            ObjDevice.CloudData = Newtonsoft.Json.JsonConvert.SerializeObject(ibm);
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveDevice", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + ObjDevice.DeviceId, "TICRM.BuisnessLayer.DeviceManager.SaveDevice", "");
                        return false; // return false if no any condition found for edit and delete
                    }
                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }
                }
                else
                {
                    InsertEventLog("SaveDevice", EventType.Log, EventColor.yellow, "Create new device ", "TICRM.BuisnessLayer.DeviceManager.SaveDevice", "");
                    if (dvc.CloudServices == CloudServiceForDD.IBM)
                    {
                        TICRM.Cloud.Adapter.CloudMain cloudMain = new Cloud.Adapter.CloudMain();
                        string result = cloudMain.IBMCloudCoverage(dvc, ibm, "create");
                        if (result == "error") { return false; }
                    }
                    dvc.CloudData = Newtonsoft.Json.JsonConvert.SerializeObject(ibm);
                    device = objMapper.GetDevice(dvc);
                    device.DeviceId = Guid.NewGuid();
                    device.Company= Guid.Parse(UserCompanyID);
                    device.CreatedBy= CurrentUserId;
                    device.CreatedDate = DateTime.Now;
                    dbEnt.Devices.Add(device);
                    HttpContext.Current.Session["DeiveObj"] = device;
                    if (dbEnt.SaveChanges() > 0)  // check if database save changes is done return true
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveDevice", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.SaveDevice", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;
        }

        /// <summary>
        /// Creates the new cloud device.
        /// </summary>
        /// <param name="dvc">The DVC.</param>
        /// <param name="item">The item.</param>
        /// <returns>DeviceDto.</returns>
        public DeviceDto CreateNewCloudDevice(DeviceDto dvc,IBMCloudViewModel item = null)
        {
            try
            {
                InsertEventLog("CreateNewCloudDevice", EventType.Log, EventColor.yellow, "Successfully Enter ", "TICRM.BuisnessLayer.DeviceManager.CreateNewCloudDevice", "");

                Device d = new Device();
                d.DeviceId = Guid.NewGuid();
                d.AccountId = dvc.AccountId;
                d.CustomerAssetId = dvc.CustomerAssetId;
                d.AssignedTeam = dvc.AssignedTeam;
                d.AssignedUser = dvc.AssignedUser;
                d.CloudData = "";
                d.CloudServices = "";
                d.CreatedDate = DateTime.Now;
                //d.DeciveConfigurations = "";
                //d.DeviceSensors = "";
                d.EMEINumber = dvc.EMEINumber;
                d.IsDeleted = false;
                d.Latitude = dvc.Latitude;
                d.Longitude = dvc.Longitude;
                d.Mac = dvc.Mac;
                d.Maintenance = DeviceMaintenance.None;
                d.Name = dvc.Name;
                d.RegistrationDate = dvc.RegistrationDate;
                d.StatusId = new Guid("192f959f-2dfa-4d41-8464-dd482325dc6c");
                d.CloudServices = "IBM";
                d.CloudData = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                dbEnt.Devices.Add(d);
                if (dbEnt.SaveChanges() > 0)
                {
                    return objMapper.GetDeviceDTO(d);
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("CreateNewCloudDevice", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceManager.CreateNewCloudDevice", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return null;
        }

        /// <summary>
        ///   Return a string of data for datatables to render on front end
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns>String of data</returns>
        public List<DeviceDto> GetDevicesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetDevicesList", EventType.Log, EventColor.yellow, "Get List of devices Based on Datatable Query", "TICRM.BuisnessLayer.DeviceManager.GetDevicesList", "");

                var devices = new List<Device>();
                var deviceDto = new List<DeviceDto>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                int totalRecord = this.GetTotalCount();

                if (!string.IsNullOrEmpty(sSearch))
                {
                    devices = dbEnt.Devices.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.Mac.ToLower().Contains(sSearch)
                    || a.EMEINumber.ToLower().Contains(sSearch)
                    || a.RegistrationDate.ToString().ToLower().Contains(sSearch)
                    || a.Account.Name.ToString().ToLower().Contains(sSearch)
                    || a.CustomerAsset.Title.ToString().ToLower().Contains(sSearch)
                    || a.Status.Name.ToString().ToLower().Contains(sSearch)
                    || a.Team.Name.ToString().ToLower().Contains(sSearch)
                    || a.User.Name.ToString().ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    devices = dbEnt.Devices.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Device item in devices.CollectionNotNull())
                {
                    deviceDto.Add(objMapper.GetDeviceDTO(item)); // add in a list object
                }

                return deviceDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDevicesList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DevicesManager.GetDevicesList", "");

                throw ex;
            }

        }

        /// <summary>
        /// Count all activites
        /// </summary>
        /// <returns>No of total activites</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Devices", "TICRM.BuisnessLayer.DevciesManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Devices.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DevicesManager.GetTotalCount", "");
                throw ex;
            }
        }


        public CloudDeviceDonutVM Get_CloudDevicesCount(string UserCompanyID)
        {
            CloudDeviceDonutVM objclouddevice = new CloudDeviceDonutVM();

            var clouddevice = dbEnt.sp_CloudDevices_Get(UserCompanyID).ToList();

            double Swuich = Convert.ToDouble(clouddevice.First().Swuich);
            double IBM = Convert.ToDouble(clouddevice.First().IBM);
            double Google = Convert.ToDouble(clouddevice.First().Google);
            double Amazon = Convert.ToDouble(clouddevice.First().Amazon);
            double Microsoft = Convert.ToDouble(clouddevice.First().Microsoft);

            double total = Swuich + IBM + Google + Amazon + Microsoft;

            double Swuichper = (Swuich / total) * 100;
            double IBMper = (IBM / total) * 100;
            double Googleper = (Google / total) * 100;
            double Amazonper = (Amazon / total) * 100;
            double Microsoftper = (Microsoft / total) * 100;

            objclouddevice.lstCloudDeviceLable.Add("Swuich: " + Swuich);
            objclouddevice.lstCloudDeviceLable.Add("IBM: " + IBM);
            objclouddevice.lstCloudDeviceLable.Add("Google: " + Google);
            objclouddevice.lstCloudDeviceLable.Add("Amazon: " + Amazon);
            objclouddevice.lstCloudDeviceLable.Add("Microsoft: " + Microsoft);


            objclouddevice.lstCloudDevicePer.Add(Swuichper);
            objclouddevice.lstCloudDevicePer.Add(IBMper);
            objclouddevice.lstCloudDevicePer.Add(Googleper);
            objclouddevice.lstCloudDevicePer.Add(Amazonper);
            objclouddevice.lstCloudDevicePer.Add(Microsoftper);

            
            return objclouddevice;
        }

    }
}
