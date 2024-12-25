using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TICRM.DTOs
{
    public class IBMCloudViewModel
    {
        public string OrganizationId { get; set; }
        public string AppId { get; set; }
        public string APIKey { get; set; }
        public string AuthToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string DeviceTokken { get; set; }
    }
}
