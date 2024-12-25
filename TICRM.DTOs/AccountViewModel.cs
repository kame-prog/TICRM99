using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TICRM.DTOs
{
    public class UserRegistrationVM
    {
        public UserRegisterDto UserRegister { get; set; }
        public CompanyDto Company { get; set; }
    }
    public class AccountViewModel
    {
        public UserDto loggedInUser { get; set; }
        public AccountDto account { get; set; }
        public FirmwareDto firmware { get; set; }
        public WorkFlowDTO workFlow { get; set; }
        public DeviceSensorGraphDto deviceSensorGraph { get; set; }
        public List<OpportunityDto> accountOppertunities { get; set; }
        public List<LocationDto> accountLocations { get; set; }
        public List<DeviceDto> accountDevices { get; set; }
        public List<AccountDto> DashboardAccounts { get; set; }
        public List<DeviceDto> DashboardDevices { get; set; }
        public List<AccountDto> ConsumptionAccounts { get; set; }
        public List<DeviceDto> ConsumptionDevices { get; set; }
        public List<CustomerAssetDto> accountAssetes { get; set; }
        public List<ActivityDTO> accountActivity { get; set; }
        public List<WorkOrderDto> accountWorkOrder { get; set; }
        public List<ContactDto> accountContact { get; set; }
        public List<WorkFlowReportDTO> accountWorkflow { get; set; }
        public List<WorkFlowDTO> allworkflowsforaccount { get; set; }
        public List<WorkFlowReportDTO> workflowReportAdmin { get; set; }
        public List<CaseDto> accountCases { get; set; }
        public SelectList CustomerAssetDropdown { get; set; }
        public SelectList MaintenanceDropdown { get; set; }
        public SelectList CloudServicesDropdown { get; set; }
        public SelectList LocationTypeDropdown { get; set; }
        public SelectList CustomerAssetTypeDropdown { get; set; }
        public SelectList LocationDropdown { get; set; }
        public SelectList WorkOrderStageDropdown { get; set; }
        public SelectList ActivityTypeDropdown { get; set; }
        public SelectList OpportunityStageDropdown { get; set; }
        public SelectList ProbabilityDropdown { get; set; }
        public SelectList CurrencyDropdown { get; set; }
        public SelectList ContactsDropdown { get; set; }
        public SelectList CaseResolutionDropdown { get; set; }
        public SelectList CaseStatusDropdown { get; set; }
        public SelectList CaseTypeDropdown { get; set; }
        public SelectList RelatedToDropdown { get; set; }
        public SelectList RelatedToIdDropdown { get; set; }
        public SelectList ResulutionTypeDropdown { get; set; }


    }

    public class DeviceChannelDonutChartVM 
    {
        public DeviceChannelDonutChartVM() 
        {
            lstPercentage = new List<double>();
            lstLabels = new List<string>();
        }

        public List<double> lstPercentage { get; set; }
        public List<string> lstLabels { get; set; }
    }
    public class DeviceNetworkProgressbar
    {
        public DeviceNetworkProgressbar()
        {
            lst_DeviceCount = new List<double>();
            lst_DevicePercentage = new List<double>();
        }
        public List<double> lst_DeviceCount { get; set; }
        public List<double> lst_DevicePercentage { get; set; }
    }
}
