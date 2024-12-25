using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace TICRM.DTOs
{
    public class DeviceViewModel : BaseDropDownDto
    {
        public System.Guid DeviceId { get; set; }
        public string Name { get; set; }
        public string Mac { get; set; }
        public string EMEINumber { get; set; }

        public string CloudServices { get; set; }
        public SelectList CloudServicesDropdown { get; set; }
        public string CloudData { get; set; }

        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> ServiceDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> Latitude { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> Longitude { get; set; }

        public Nullable<System.Guid> AccountId { get; set; }
        public Nullable<System.Guid> CustomerAssetId { get; set; }
        public SelectList CustomerAssetDropdown { get; set; }
        public string Maintenance { get; set; }
        public SelectList MaintenanceDropdown { get; set; }
        public string Gateway { get; set; }
        public Nullable<System.DateTime> LastMessage { get; set; }
        public virtual ICollection<DeciveConfigurationDto> DeciveConfigurations { get; set; }
        public virtual ICollection<DeviceSensorDto> DeviceSensors { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        public virtual StatusDto Status { get; set; }
        public virtual TeamDto Team { get; set; }
        public virtual UserDto User { get; set; }
        public bool IsGateway { get; set; }

        public Nullable<System.Guid> GatewayReference { get; set; }
        public SelectList GatewayReferenceDropdown { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public IBMCloudViewModel IBMCloud { get; set; }

        public class CloudDeviceDonutVM
        {
            public CloudDeviceDonutVM()
            {
                lstCloudDevicePer = new List<double>();
                lstCloudDeviceLable = new List<string>();
            }

            public List<double> lstCloudDevicePer { get; set; }
            public List<string> lstCloudDeviceLable { get; set; }
        }


    }
}
