using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TICRM.Cloud.Adapter.Adaptee.IBM;

namespace TICRM.Cloud.Adapter.Target
{
    interface IIBM
    {
        dynamic GetAllDevices();
        dynamic UpdateDeviceInfo(string type, string deviceId, UpdateDevicesInfo info);
        dynamic RegisterMultipleDevices(RegisterDevicesInfo[] info);
        dynamic RegisterDevice(string type, RegisterSingleDevicesInfo info);
        dynamic RegisterDeviceType(DeviceTypeInfo info);
        dynamic GetAllDeviceTypes();
        dynamic GetDeviceLocationInfo(string type, string deviceId);
    }
}
