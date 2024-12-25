using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TICRM.DTOs
{
    public class TreeViewModel
    {
        public virtual List<LocationDto> Locations { get; set; }
        public virtual List<CustomerAssetDto> CustomerAssets { get; set; }
        public virtual List<DeviceDto> Devices { get; set; }
    }
}
