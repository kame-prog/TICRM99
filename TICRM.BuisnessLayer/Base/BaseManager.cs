using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TICRM.DAL;
using TICRM.DTOs;
using TICRM.Mapper;

namespace TICRM.BuisnessLayer.Base
{
    public class BaseManager
    {


        protected CRMEntities dbEnt = new CRMEntities();

        protected TIMapper objMapper = new TIMapper();

        //Create the Log object for exceptin logging in the TEXT file.
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseManager));



        #region Properties
        //public List<StatusDto> Status { get; set; }
        //public List<TeamDto> Teams { get; set; }
        //public List<UserDto> Users { get; set; }
        //public List<AddressDto> Addresses { get; set; }

        //public List<AccountDto> Accounts { get; set; }
        //public List<DeviceDto> devices { get; set; }
        //public List<LocationDto> locations { get; set; }
        //public List<WorkFlowDTO> workflows { get; set; }

        //public List<ChannelDto> Channels { get; set; }
        //public List<Data_DurationDto> DataDuration { get; set; }
        //public List<NetworkDto> Networks { get; set; }
        //public List<CountryDto> countries { get; set; }
        public List<ActionDTO> actions { get; set; }
        //public List<PriorityDTO> priorities { get; set; }

    #endregion

    public BaseManager()
        {
            //Status = GetStatuses();
            //Teams = GetTeams();
            //Users = GetUsers();
            //Addresses = GetAddresses();
            //Accounts = GetAccounts();
            //devices = Getdevices();
            //locations = Getlocation();
            //workflows = Getworkflow();
            //Channels = GetChannels();
            //DataDuration = GetData_Durations();
            //Networks = GetNetworks();
            //countries = GetCountries();
            //actions = GetAction();
            //priorities = GetPriorities();
        }

        //Get Account Size
        public List<sp_AccSizeDropDown_Get_Result> AccSizeDropDown()
        {
            try
            {
                var AccSize = dbEnt.sp_AccSizeDropDown_Get().ToList();
                return AccSize;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       //Get Account Type
        public List<sp_AccTypeDropDown_Get_Result> AccTypeDropDown()
        {
            try
            {
                var AccSize = dbEnt.sp_AccTypeDropDown_Get().ToList();
                return AccSize;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get Industries
        public List<sp_IndustryDropDown_Get_Result> IndustryDropDown()
        {
            try
            {
                var AccSize = dbEnt.sp_IndustryDropDown_Get().ToList();
                return AccSize;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get Currency
        public List<sp_CurrencyDropDown_Get_Result> CurrencyDropDown()
        {
            try
            {
                var Currency = dbEnt.sp_CurrencyDropDown_Get().ToList();
                return Currency;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get Urgency
        public List<sp_UrgencyDropDown_Get_Result> UrgencyDropDown()
        {
            try
            {
                var Urgency = dbEnt.sp_UrgencyDropDown_Get().ToList();
                return Urgency;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Get Customer Asset Types
        public List<sp_CusAssTypeDropDown_Get_Result> CusAssTypeDropDown()
        {
            try
            {
                var CusAssType = dbEnt.sp_CusAssTypeDropDown_Get().ToList();
                return CusAssType;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get Opportunity Stages
        public List<sp_OppStageDropDown_Get_Result> OppStageDropDown()
        {
            try
            {
                var OppStage = dbEnt.sp_OppStageDropDown_Get().ToList();
                return OppStage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get Probabilities
        public List<sp_PobabilityDropDown_Get_Result> ProbabilityDropDown()
        {
            try
            {
                var Probability = dbEnt.sp_PobabilityDropDown_Get().ToList();
                return Probability;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get WorkStages
        public List<sp_WorkStageDropDown_Get_Result> WorkStageDropDown()
        {
            try
            {
                var WorkStage = dbEnt.sp_WorkStageDropDown_Get().ToList();
                return WorkStage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Get Lead Sources
        public List<sp_LeadSourDropDown_Get_Result> LeadSourDropDown()
        {
            try
            {
                var LeadSource = dbEnt.sp_LeadSourDropDown_Get().ToList();
                return LeadSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Get Lead Types
        public List<sp_LeadTypeDropDown_Get_Result> LeadTypeDropDown()
        {
            try
            {
                var LeadType = dbEnt.sp_LeadTypeDropDown_Get().ToList();
                return LeadType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Get Location Types
        public List<sp_LocaTypeDropDown_Get_Result> LocaTypeDropDown()
        {
            try
            {
                var LocationType = dbEnt.sp_LocaTypeDropDown_Get().ToList();
                return LocationType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Get Sensor
        public List<sp_SensorDropDown_Get_Result> SensorDropDown()
        {
            try
            {
                var Sensor = dbEnt.sp_SensorDropDown_Get().ToList();
                return Sensor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Properties's methods
        /// <summary>
        /// Get All Priority 
        /// </summary>
        /// <returns></returns>
        public List<sp_PriorityDropDown_Get_Result> PriorityDropDown()
        {
            try
            {
                var Priority = dbEnt.sp_PriorityDropDown_Get().ToList();
                return Priority;
                //LoggerManager.Info("Entered to Get All Priority From Base Manager");
                //List<PriorityDTO> priorityDTOs = new List<PriorityDTO>();
                //foreach (var item in dbEnt.Priorities.CollectionNotNull())
                //{
                //    priorityDTOs.Add(objMapper.GetPriorityDTO(item));
                //}
                //LoggerManager.Info("Returned Collection of All Priority From Base Manager");
                //return priorityDTOs;
            }
            catch (Exception ex)
            {

                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get All Action 
        /// </summary>
        /// <returns></returns>
        public List<sp_ActionDropDown_Get_Result> ActionDropDown()
        {
            try
            {
                var Action = dbEnt.sp_ActionDropDown_Get().ToList();
                return Action;
                //LoggerManager.Info("Entered to Get All Actions From Base Manager");
                //List<ActionDTO> actionDTOs = new List<ActionDTO>();
                //foreach (var item in dbEnt.Actions.CollectionNotNull())
                //{
                //    actionDTOs.Add(objMapper.GetActionDTO(item));
                //}
                //LoggerManager.Info("Returned Collection of All Actions From Base Manager");
                //return actionDTOs;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get All Statuses Domain Transfer Objects
        /// </summary>
        /// <returns></returns>
        public List<sp_StatusDropDown_Get_Result> StatusDropDown()
        {
            try
            {
                var Status = dbEnt.sp_StatusDropDown_Get().ToList();
                return Status;
                //LoggerManager.Info("Entered to Get All statuses From Base Manager");
                //List<StatusDto> statusDtos = new List<StatusDto>(); 
                //foreach (var item in dbEnt.Status.CollectionNotNull())
                //{
                //    statusDtos.Add(objMapper.GetStatusDTO(item));
                //}
                //LoggerManager.Info("Returned Collection of All statuses From Base Manager");
                //return statusDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }
        // Get Country
        public List<sp_CountryDropDown_Get_Result> CountryDropDown()
        {
            try
            {
                var Country = dbEnt.sp_CountryDropDown_Get().ToList();
                return Country;
                //LoggerManager.Info("Entered to Get All Countries From Base Manager");
                //List<CountryDto> countryDtos = new List<CountryDto>();
                //foreach (var item in dbEnt.Countries.CollectionNotNull())
                //{
                //    countryDtos.Add(objMapper.GetCountryDto(item));
                //}
                //LoggerManager.Info("Returned Collection of All Countries From Base Manager");
                //return countryDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }
        public List<sp_ChannelDropDown_Get_Result> ChannelDropDown()
        {
            try
            {
                var channel = dbEnt.sp_ChannelDropDown_Get().ToList();
                return channel;
                //LoggerManager.Info("Entered to Get All Channels From Base Manager");
                //List<ChannelDto> channelDtos = new List<ChannelDto>();
                //foreach (var item in dbEnt.Channels.CollectionNotNull())
                //{
                //    channelDtos.Add(objMapper.GetChannelDto(item));
                //}
                //LoggerManager.Info("Returned Collection of All Channels From Base Manager");
                //return channelDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }
        //Get Network
        public List<sp_NetworkDropDown_Get_Result> NetworkDropDown()
        {
            try
            {
                LoggerManager.Info("Entered to Get All Networks From Base Manager");
                var Network = dbEnt.sp_NetworkDropDown_Get().ToList();
                return Network;
                //LoggerManager.Info("Entered to Get All Networks From Base Manager");
                //List<NetworkDto> networkDtos = new List<NetworkDto>();
                //foreach (var item in dbEnt.Networks.CollectionNotNull())
                //{
                //    networkDtos.Add(objMapper.GetNetworkDto(item));
                //}
                //LoggerManager.Info("Returned Collection of All Networks From Base Manager");
                //return networkDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }
        public List<sp_DataDuraDropDown_Get_Result> DataDurDropDown()
        {
            try
            {
                var DataDuration= dbEnt.sp_DataDuraDropDown_Get().ToList(); 
                return DataDuration;
                //LoggerManager.Info("Entered to Get All Data Durations From Base Manager");
                //List<Data_DurationDto> data_DurationDto = new List<Data_DurationDto>();
                //foreach (var item in dbEnt.Data_Duration.CollectionNotNull())
                //{
                //    data_DurationDto.Add(objMapper.GetData_DurationDto(item));
                //}
                //LoggerManager.Info("Returned Collection of All Data Durations From Base Manager");
                //return data_DurationDto;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }

        public StatusDto GetStatus( string name)
        {
            try
            {
                LoggerManager.Info("Entered to Get All statuses From Base Manager");

                StatusDto statusDtos = new StatusDto();
               
                    statusDtos = objMapper.GetStatusDTO(dbEnt.Status.Where(x => x.Name == name).First());

                LoggerManager.Info("Returned Collection of All statuses From Base Manager");
                return statusDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }

        public CaseStatusDto GetCaseStatus(string name)
        {
            try
            {
                LoggerManager.Info("Entered to Get All statuses From Base Manager");

                CaseStatusDto statusDtos = new CaseStatusDto();

                statusDtos = objMapper.GetCaseStatusDto(dbEnt.CaseStatus.Where(x => x.Name == name).First());

                LoggerManager.Info("Returned Collection of All statuses From Base Manager");
                return statusDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get Team Dto
        /// </summary>
        /// <returns></returns>
        public List<sp_TeamDropDown_Get_Result> TeamDropDown()
        {
            try
            {
                var Teams=dbEnt.sp_TeamDropDown_Get().ToList();
                return Teams;
                //LoggerManager.Info("Entered to Get All Teams From Base Manager");
                //List<TeamDto> teamDtos = new List<TeamDto>();
                //foreach (var item in dbEnt.Teams.CollectionNotNull())
                //{
                //    teamDtos.Add(objMapper.GetTeamDTO(item));
                //}
                //LoggerManager.Info("Returned Collection of All Teams From Base Manager");
                //return teamDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get Users Dtos
        /// </summary>
        /// <returns></returns>
        public List<sp_AssUserDropDown_Get_Result> UserDropDown()
        {
            try
            {
                var Users=dbEnt.sp_AssUserDropDown_Get().ToList();
                return Users;
                //LoggerManager.Info("Enter to get All Users From Base Manager");
                //var userDtos = new List<UserDto>();
                //foreach (var item in dbEnt.Users.CollectionNotNull())
                //{
                //    userDtos.Add(objMapper.GetUserDTO(item));
                //}
                //LoggerManager.Info("Returned Collection of All Users From Base Manager");
                //return userDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }

       

        /// <summary>
        /// get addresses Dtos
        /// </summary>
        /// <returns></returns>
        public List<sp_AddressDropDown_Get_Result> AddresseDropDown(string CompanyId)
        {
            try
            {
                var Address= dbEnt.sp_AddressDropDown_Get(CompanyId).ToList();
                return Address;

                //var addressDtos = new List<AddressDto>();

                //foreach (var item in dbEnt.Addresses.Where(x=>x.IsDeleted != true).CollectionNotNull())
                //{
                //    addressDtos.Add(objMapper.GetAddressDTO(item));
                //}
                //return addressDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the workflow.
        /// </summary>
        /// <returns>List&lt;AccountDto&gt;.</returns>
        public List<sp_WorkFlowDropDown_Get_Result> WorkFlowDropDown(string CompanyID)
        {
            try
            {
                var WorkFlow = dbEnt.sp_WorkFlowDropDown_Get(CompanyID).ToList();
                return WorkFlow;
                //var workflowDtos = new List<WorkFlowDTO>();
                //foreach (var item in dbEnt.WorkFlows.CollectionNotNull())
                //{
                //    workflowDtos.Add(objMapper.WorkFlows(item));
                //}
                //return workflowDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }
        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <returns>List&lt;deviceDto&gt;.</returns>
        public List<sp_DeviceDropDown_Get_Result> DeviceDropDown(string CompanyID)
        {
            try
            {
                var Device = dbEnt.sp_DeviceDropDown_Get(CompanyID).ToList();
                return Device;
                //var deviceDtos = new List<DeviceDto>();
                //foreach (var item in dbEnt.Devices.CollectionNotNull())
                //{
                //    deviceDtos.Add(objMapper.GetDeviceDTO(item));
                //}
                //return deviceDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }
        /// <summary>
        /// Gets the Getlocation.
        /// </summary>
        /// <returns>List&lt;locationDto&gt;.</returns>
        public List<sp_LocationDropDown_Get_Result> LocationDropDown(string CompanyID)
        {
            try
            {
                var Location = dbEnt.sp_LocationDropDown_Get(CompanyID).ToList();
                return Location;
                //var locationDtos = new List<LocationDto>();
                //foreach (var item in dbEnt.Locations.CollectionNotNull())
                //{
                //    locationDtos.Add(objMapper.GetLocationDTO(item));
                //}
                //return locationDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the accounts.
        /// </summary>
        /// <returns>List&lt;AccountDto&gt;.</returns>
        public List<sp_AccountDropDown_Get_Result> AccountDropDown(string CompanyID)
        {
            try
            {
                var Accounts=dbEnt.sp_AccountDropDown_Get(CompanyID).ToList();
                return Accounts;
                //var accountDtos = new List<AccountDto>();
                //foreach (var item in dbEnt.Accounts.CollectionNotNull())
                //{
                //    accountDtos.Add(objMapper.GetAccountDTO(item));
                //}
                //return accountDtos;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }



        /// <summary>
        /// Customers the assets on account identifier.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>List&lt;CustomerAssetDto&gt;.</returns>
        public List<CustomerAssetDto> CustomerAssetsOnAccountId(Guid accountId)
        {
            try
            {
                var customerAssetsDTO = new List<CustomerAssetDto>();
                List<CustomerAsset> customerAssets = dbEnt.CustomerAssets.Where(x => x.AccountId == accountId && x.IsDeleted != true).ToList();
                foreach (CustomerAsset item in customerAssets.CollectionNotNull())
                {
                    customerAssetsDTO.Add(objMapper.GetCustomerAssetDTO(item));
                }
                return customerAssetsDTO;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }
        /// <summary>
        /// Gets all customer assets.
        /// </summary>
        /// <returns>List&lt;CustomerAssetDto&gt;.</returns>
        public List<CustomerAssetDto> GetAllCustomerAssets()
     {
            try
            {
                var customerAssetsDTO = new List<CustomerAssetDto>();
                List<CustomerAsset> customerAssets = dbEnt.CustomerAssets.Where(x=>x.IsDeleted != true).ToList();
                
                foreach (CustomerAsset item in customerAssets.CollectionNotNull())
                {
                    customerAssetsDTO.Add(objMapper.GetCustomerAssetDTO(item));
                }
                return customerAssetsDTO;
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }



        #endregion


        /// <summary>
        /// Inserts the event monitor.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Type">The type.</param>
        /// <param name="color">The color.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Path">The path.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        #region Event Monitor and Event Log            
        public void InsertEventMonitor(string Name, string Type, string color, string Message, string Path, string CurrentUserId)
        {
            try
            {
                LoggerManager.Error(Message);
                EventMonitor eventMonitor = new EventMonitor();
                eventMonitor.EventMonitorId = Guid.NewGuid();
                eventMonitor.Name = Name;
                eventMonitor.Type = Type;
                eventMonitor.Path = Path;
                eventMonitor.Message = Message;
                eventMonitor.Status = true;
                eventMonitor.Color = color;
                eventMonitor.CreatedDate = DateTime.Now;
                eventMonitor.CreatedBy = CurrentUserId;
                dbEnt.EventMonitors.Add(eventMonitor);

                dbEnt.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }
        /// <summary>
        /// Inserts the event log.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Type">The type.</param>
        /// <param name="color">The color.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Path">The path.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        public void InsertEventLog(string Name, string Type, string color, string Message, string Path, string CurrentUserId)
        {
            try
            {
                LoggerManager.Info(Message);
                EventLog eventLog = new EventLog();
                eventLog.EventLogId = Guid.NewGuid();
                eventLog.Name = Name;
                eventLog.Type = Type;
                eventLog.Path = Path;
                eventLog.Message = Message;
                eventLog.Status = true;
                eventLog.Color = color;
                eventLog.CreatedDate = DateTime.Now;
                eventLog.CreatedBy = CurrentUserId;
                dbEnt.EventLogs.Add(eventLog);
                dbEnt.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts the event notification.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Type">The type.</param>
        /// <param name="color">The color.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Path">The path.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        public void InsertEventNotification(string Name, string Type, string color, string Message, string Path, string CurrentUserId)
        {
            try
            {
                LoggerManager.Info(Message);
                EventNotification eventNotification = new EventNotification();
                eventNotification.EventNotificationId = Guid.NewGuid();
                eventNotification.Name = Name;
                eventNotification.Type = Type;
                eventNotification.Message = Message;
                eventNotification.Status = true;
                eventNotification.Color = color;
                eventNotification.CreatedDate = DateTime.Now;
                eventNotification.CreatedBy = CurrentUserId;
                dbEnt.EventNotifications.Add(eventNotification);
                dbEnt.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex.Message, ex);
                throw;
            }
        }



        #endregion


        //public void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        dbEnt.Dispose();
        //    }
        //    //base.Dispose(disposing);
        //}
    }
}
