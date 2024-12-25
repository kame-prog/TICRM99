using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TICRM.Cloud.Adapter.Adaptee;
using TICRM.Cloud.Adapter.Target;
using TICRM.DTOs;
using static TICRM.Cloud.Adapter.Adaptee.IBM;

namespace TICRM.Cloud.Adapter
{
    public class CloudMain
    {

        public string IBMCloudCoverage(DeviceDto deviceDto,IBMCloudViewModel ibmCloud,string type)
        {
            string status = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                IIBM iBM = new Adapter.IBMAdapter(ibmCloud.APIKey, ibmCloud.AuthToken);

                if (type == "all")
                {
                    var get_all_Devices = iBM.GetAllDevices();
                }


                RegisterSingleDevicesInfo info = new RegisterSingleDevicesInfo();
                info.authToken = ibmCloud.DeviceTokken;
                info.deviceId = ibmCloud.DeviceId;
                DeviceInfo deviceInfo = new DeviceInfo();
                deviceInfo.description = deviceDto.Name + " Register Date: " + DateTime.Now;
                deviceInfo.descriptiveLocation = deviceDto.Name;
                deviceInfo.deviceClass = "Temprature";
                deviceInfo.fwVersion = "1.0";
                deviceInfo.hwVersion = "1.1.0";
                deviceInfo.manufacturer = "Techimplement";
                deviceInfo.model = deviceDto.Mac;
                deviceInfo.serialNumber = deviceDto.EMEINumber;

                info.deviceInfo = deviceInfo;

                LocationInfo loc = new LocationInfo();
                loc.longitude = (double)deviceDto.Longitude;
                loc.latitude = (double)deviceDto.Latitude;
                loc.accuracy = 1;
                loc.measuredDateTime = DateTime.Now.ToString("o");
                info.location = loc;

                info.metadata = new { };


                List<DeviceTypeInfo> deviceTypes = new List<DeviceTypeInfo>();
                dynamic getAllDeviceType = iBM.GetAllDeviceTypes();
                Dictionary<String, Object> testdict = new Dictionary<String, Object>();
                testdict = iBM.GetAllDeviceTypes();
                var contain_Key = testdict.ContainsKey("results");
                if (contain_Key == true)
                {
                    var results = testdict.FirstOrDefault(x => x.Key == "results").Value;
                    var data1 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(results);
                    deviceTypes = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<DeviceTypeInfo>>(data1);
                }

                DeviceTypeInfo typeInfo = deviceTypes.FirstOrDefault(x => x.id == ibmCloud.DeviceType);
                if (typeInfo == null)
                {
                    DeviceTypeInfo deviceTypeInfo = new DeviceTypeInfo();
                    deviceTypeInfo.classId = "Device";
                    deviceTypeInfo.deviceInfo = deviceInfo;
                    deviceTypeInfo.id = ibmCloud.DeviceType;
                    deviceTypeInfo.metadata = new { };
                    deviceTypeInfo.description = "Techimplement register this device type";

                    dynamic registerdevicetype = iBM.RegisterDeviceType(deviceTypeInfo);
                }

                if (type == "create")
                {
                    var registerDevice = iBM.RegisterDevice(ibmCloud.DeviceType, info);

                    status = "success";
                }
                else if (type == "update")
                {
                    UpdateDevicesInfo updateDevicesInfo = new UpdateDevicesInfo();
                    updateDevicesInfo.deviceInfo = deviceInfo;

                    dynamic update = iBM.UpdateDeviceInfo(ibmCloud.DeviceType,ibmCloud.DeviceId, updateDevicesInfo);
                    status = "success";
                }
            }
            catch (Exception ex)
            {
                status = "error";
                throw ex;
            }
            return status;
        }


        public List<IBMDevicesInfo> GetAllIBMCloudDevice(List<IBMCloudViewModel> ibmCloud)
        {
            List<IBMDevicesInfo> registerDevicesInfos = new List<IBMDevicesInfo>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var distinctCloudData = ibmCloud.Select(m => new { m.OrganizationId, m.AuthToken, m.APIKey,m.AppId }).Distinct().ToList();

                foreach (var item in distinctCloudData)
                {
                    if (item.OrganizationId != null && item.AuthToken != null && item.APIKey != null)
                    {
                        IIBM iBM = new Adapter.IBMAdapter(item.APIKey,item.AuthToken);

                        Dictionary<String, Object> getAllDevices = new Dictionary<String, Object>();
                        getAllDevices = iBM.GetAllDevices();
                        var contain_Key = getAllDevices.ContainsKey("results");
                        if (contain_Key == true)
                        {
                            List<IBMDevicesInfo> Devices_List = new List<IBMDevicesInfo>();

                            var results = getAllDevices.FirstOrDefault(x => x.Key == "results").Value;
                            var data = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(results);
                            Devices_List = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<IBMDevicesInfo>>(data);

                            foreach (var Additem in Devices_List)
                            {
                                dynamic loc = iBM.GetDeviceLocationInfo(Additem.typeId, Additem.deviceId);
                                var loc_data = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(loc);
                                var loc_result = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<LocationInfo>(loc_data);
                                Additem.location = loc_result;
                                Additem.authToken = item.AuthToken;
                                Additem.APIKey = item.APIKey;
                                Additem.OrganizationId = item.OrganizationId;
                                Additem.AppId = item.AppId;
                            }

                            registerDevicesInfos.AddRange(Devices_List);
                        }

                    }
                }





            }
            catch (Exception ex)
            {
                throw;
            }
            return registerDevicesInfos;
        }





    }
}
