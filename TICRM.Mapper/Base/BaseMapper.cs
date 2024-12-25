using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.Mapper.Base
{
    /// <summary>
    /// Base Class having Common Domain Transfer objects 
    /// </summary>
    public class BaseMapper
    {
        protected CRMEntities dbEnt = new CRMEntities();

        /// <summary>
        /// Priority DTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PriorityDTO GetPriorityDTO(Priority priority)
        {
            try
            {
                if (priority == null) return null;

                var priorityDTO = new PriorityDTO();
                priorityDTO.ID = priority.ID;
                priorityDTO.Name = priority.Name;
                priorityDTO.Value = priority.Value;
                return priorityDTO;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Action DTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public ActionDTO GetActionDTO(TICRM.DAL.Action action)
        {
            try
            {
                if (action == null) return null;

                var actionDTO = new ActionDTO();
                actionDTO.ID = action.ID;
                actionDTO.Name = action.Name;

                return actionDTO;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// Status DTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public StatusDto GetStatusDTO(Status e)
        {
            try
            {
                if (e == null) return null;

                var d = new StatusDto();
                d.StatusId = e.StatusId;
                d.Name = e.Name;
                d.Type = e.Type;

                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CountryDto GetCountryDto(Country country)
        {
            try
            {
                if (country == null) return null;

                var c = new CountryDto();
                c.ID = country.ID;
                c.Country_Name = country.Country_Name;
                return c;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ChannelDto GetChannelDto(Channel channel)
        {
            try
            {
                if (channel == null) return null;
                var obj = new ChannelDto();
                obj.Channel_Id = channel.Channel_Id;
                obj.Channel = channel.Channel_Name;
                return obj;
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public NetworkDto GetNetworkDto(Network network)
        {
            try
            {
                if (network == null) return null;
                var obj = new NetworkDto();
                obj.Network_Id = network.Network_Id;
                obj.Network = network.Network_Name;
                return obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Data_DurationDto GetData_DurationDto(Data_Duration data_Duration)
        {
            try
            {
                if (data_Duration == null) return null;
                var obj = new Data_DurationDto();
                obj.Duration_Id = data_Duration.Duration_Id;
                obj.Data = data_Duration.Time ;
                return obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Status GetStatus(StatusDto e)
        {
            try
            {
                if (e == null) return null;

                var d = new Status();
                d.StatusId = e.StatusId;
                d.Name = e.Name;
                d.Type = e.Type;

                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Get Team Dto 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public TeamDto GetTeamDTO(Team e)
        {
            try
            {
                if (e == null) return null;
                var d = new TeamDto();
                d.TeamId = e.TeamId;
                d.Name = e.Name;
                d.Description = e.Description;
                d.StatusId = e.StatusId;
                d.CreatedBy = e.CreatedBy;
                d.CreatedDate = e.CreatedDate;
                d.UpdatedBy = e.UpdatedBy;
                d.UpdatedDate = e.UpdatedDate;
                d.Email = e.Email;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Team GetTeam(TeamDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new Team();
                d.TeamId = e.TeamId;
                d.Name = e.Name;
                d.Description = e.Description;
                d.StatusId = e.StatusId;
                d.CreatedBy = e.CreatedBy;
                d.CreatedDate = e.CreatedDate;
                d.UpdatedBy = e.UpdatedBy;
                d.UpdatedDate = e.UpdatedDate;
                d.Email = e.Email;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Get User DTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public UserDto GetUserDTO(User e)
        {
            try
            {
                if (e == null) return null;
                var d = new UserDto();
                d.UserId = e.UserId;
                d.Name = e.Name;
                d.Email = e.Email;
                d.Phone = e.Phone;
                d.StatusId = e.StatusId;
                d.CreatedBy = e.CreatedBy;
                d.CreatedDate = e.CreatedDate;
                d.UpdatedBy = e.UpdatedBy;
                d.UpdatedDate = e.UpdatedDate;
                d.IsAssigned = e.IsAssigned;
                d.AssignedItem = e.AssignedItem;
                d.AssignedItemId = e.AssignedItemId;
                d.Location = e.Location;
                d.AssignedItemTime = e.AssignedItemTime;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User GetUser(UserDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new User();
                d.UserId = e.UserId;
                d.Name = e.Name;
                d.Email = e.Email;
                d.Phone = e.Phone;
                d.StatusId = e.StatusId;
                d.CreatedBy = e.CreatedBy;
                d.CreatedDate = e.CreatedDate;
                d.UpdatedBy = e.UpdatedBy;
                d.UpdatedDate = e.UpdatedDate;
                d.IsAssigned = e.IsAssigned;
                d.AssignedItem = e.AssignedItem;
                d.AssignedItemId = e.AssignedItemId;
                d.Location = e.Location;
                d.AssignedItemTime = e.AssignedItemTime;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        #region WorkFlow
        public WorkFlowDTO TakeWorkFlowDTO(WorkFlow e)
        {
            try
            {
                if (e == null) return null;
                var d = new WorkFlowDTO();
                d.WorkFlowId = e.WorkFlowId;
                d.Name = e.Name;
                d.TargetOn = e.TargetOn;
                d.Description = e.Description;
                d.Priority = e.Priority;
                d.CreatedBy = e.CreatedBy;
                d.CreatedDate = e.CreatedDate;
                d.UpdatedBy = e.UpdatedBy;
                d.UpdatedDate = e.UpdatedDate;

                //d.TriggerCondition = e.TriggerCondition;
                //d.TriggerIn = e.TriggerIn;
                //d.TriggerOut = e.TriggerOut;
                //d.WorkFlowStatus = e.WorkFlowStatus;
                //d.AppliedTo = e.AppliedTo;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public WorkFlow TakeWorkFlows(WorkFlowDTO e)
        {
            try
            {
                if (e == null) return null;
                var d = new WorkFlow();
                d.WorkFlowId = e.WorkFlowId;
                d.Name = e.Name;
                d.TargetOn = e.TargetOn;
                d.Description = e.Description;
                d.Priority = e.Priority;
                d.CreatedBy = e.CreatedBy;
                d.CreatedDate = e.CreatedDate;
                d.UpdatedBy = e.UpdatedBy;
                d.UpdatedDate = e.UpdatedDate;

                //d.TriggerCondition = e.TriggerCondition;
                //d.TriggerIn = e.TriggerIn;
                //d.TriggerOut = e.TriggerOut;
                //d.WorkFlowStatus = e.WorkFlowStatus;
                //d.AppliedTo = e.AppliedTo;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion

        //public string GetAccountSizeOnId(Guid? guid)
        //{
        //    try
        //    {
        //       string AccountSize = dbEnt.AccountSizes.FirstOrDefault(x => x.AccountSizeId == guid) == null ? "" : dbEnt.AccountSizes.FirstOrDefault(x => x.AccountSizeId == guid).Name;
        //        return AccountSize;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        /// <summary>
        /// Get account size dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public AccountSizeDto GetAccountSizeDTO(AccountSize e)
        {
            try
            {
                if (e == null) return null;
                var d = new AccountSizeDto();
                d.AccountSizeId = e.AccountSizeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public AccountSize GetAccountSize(AccountSizeDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new AccountSize();
                d.AccountSizeId = e.AccountSizeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// get account type dto 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public AccountTypeDto GetAccountTypeDTO(AccountType e)
        {
            try
            {
                if (e == null) return null;
                var d = new AccountTypeDto();
                d.AccountTypeId = e.AccountTypeId;
                d.Name = e.Name;
                d.Font_Class = e.Font_Class;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public AccountType GetAccountType(AccountTypeDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new AccountType();
                d.AccountTypeId = e.AccountTypeId;
                d.Name = e.Name;
                d.Font_Class = e.Font_Class;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Get address dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public AddressDto GetAddressDTO(Address e)
        {
            try
            {
                if (e == null) return null;
                var d = new AddressDto();
                d.AddressId = e.AddressId;
                d.Street1 = e.Street1;
                d.Street2 = e.Street2;
                d.City = e.City;
                d.State = e.State;
                d.Zip = e.Zip;
                d.Country = e.Country;
                d.IsDeleted = e.IsDeleted;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Address GetAddress(AddressDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new Address();
                d.AddressId = e.AddressId;
                d.Street1 = e.Street1;
                d.Street2 = e.Street2;
                d.City = e.City;
                d.State = e.State;
                d.Zip = e.Zip;
                d.Country = e.Country;
                d.IsDeleted = e.IsDeleted;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// Get portability 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PobabilityDto GetPobabilityDTO(Pobability e)
        {
            try
            {
                if (e == null) return null;
                var d = new PobabilityDto();
                d.ProbabilityId = e.ProbabilityId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Pobability GetPobability(PobabilityDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new Pobability();
                d.ProbabilityId = e.ProbabilityId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Get opportunity stage
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public OpportunityStageDto GetOpportunityStageDTO(OpportunityStage e)
        {
            try
            {
                if (e == null) return null;
                var d = new OpportunityStageDto();
                d.OpportunityStageId = e.OpportunityStageId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OpportunityStage GetOpportunityStage(OpportunityStageDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new OpportunityStage();
                d.OpportunityStageId = e.OpportunityStageId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// Opportunity stage Dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public CurrencyDto GetCurrencyDTO(Currency e)
        {
            try
            {
                if (e == null) return null;
                var d = new CurrencyDto();
                d.CurrencyId = e.CurrencyId;
                d.Name = e.Name;
                d.Value = e.Value;
                d.Sign = e.Sign;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Currency GetCurrency(CurrencyDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new Currency();
                d.CurrencyId = e.CurrencyId;
                d.Name = e.Name;
                d.Value = e.Value;
                d.Sign = e.Sign;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///  //IndustryDto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public IndustryDto GetIndustryDTO(Industry e)
        {
            try
            {
                if (e == null) return null;
                var d = new IndustryDto();
                d.IndustryId = e.IndustryId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Industry GetIndustry(IndustryDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new Industry();
                d.IndustryId = e.IndustryId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///    //LeadSourceDto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public LeadSourceDto GetLeadSourceDTO(LeadSource e)
        {
            try
            {
                if (e == null) return null;
                var d = new LeadSourceDto();
                d.LeadSourceId = e.LeadSourceId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeadSource GetLeadSource(LeadSourceDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new LeadSource();
                d.LeadSourceId = e.LeadSourceId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///  //LeadTypeDto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public LeadTypeDto GetLeadTypeDTO(LeadType e)
        {
            try
            {
                if (e == null) return null;
                var d = new LeadTypeDto();
                d.LeadTypeId = e.LeadTypeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public LeadType GetLeadType(LeadTypeDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new LeadType();
                d.LeadTypeId = e.LeadTypeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CustomerAssetTypeDto GetCustomerAssetTypeDTO(CustomerAssetType e)
        {
            try
            {
                if (e == null) return null;
                var d = new CustomerAssetTypeDto();
                d.CustomerAssetTypeId = e.CustomerAssetTypeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CustomerAssetType GetCustomerAssetType(CustomerAssetTypeDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new CustomerAssetType();
                d.CustomerAssetTypeId = e.CustomerAssetTypeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Urgency Dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public UrgencyDto GetUrgencyDTO(Urgency e)
        {
            try
            {
                if (e == null) return null;
                var d = new UrgencyDto();
                d.UrgencyId = e.UrgencyId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Urgency GetUrgency(UrgencyDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new Urgency();
                d.UrgencyId = e.UrgencyId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Location Type Dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public LocationTypeDto GetLocationTypeDTO(LocationType e)
        {
            try
            {
                if (e == null) return null;
                var d = new LocationTypeDto();
                d.LocationTypeId = e.LocationTypeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LocationType GetLocationType(LocationTypeDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new LocationType();
                d.LocationTypeId = e.LocationTypeId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Reading Type Dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public ReadingTypeDto GetReadingTypeDTO(ReadingType e)
        {
            try
            {
                if (e == null) return null;
                var d = new ReadingTypeDto();
                d.ReadingTypeId = e.ReadingTypeId;
                d.Name = e.Name;
                d.IsDeleted = e.IsDeleted;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ReadingType GetReadingType(ReadingTypeDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new ReadingType();
                d.ReadingTypeId = e.ReadingTypeId;
                d.Name = e.Name;
                d.IsDeleted = e.IsDeleted;
                dbEnt.SaveChanges();
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Reading Unit Dto
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public ReadingUnitDto GetReadingUnitDTO(ReadingUnit e)
        {
            try
            {
                if (e == null) return null;
                var d = new ReadingUnitDto();
                d.ReadingUnitId = e.ReadingUnitId;
                d.Name = e.Name;
                d.IsDeleted = e.IsDeleted;
                d.Type = e.Type;
                d.ReadingType = GetReadingTypeDTO(e.ReadingType);
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ReadingUnit GetReadingUnit(ReadingUnitDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new ReadingUnit();
                d.ReadingUnitId = e.ReadingUnitId;
                d.Name = e.Name;
                d.IsDeleted = e.IsDeleted;
                d.Type = e.Type;
                d.ReadingType = GetReadingType(e.ReadingType);
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///  get work stage DTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public WorkStageDto GetWorkStageDTO(WorkStage e)
        {
            try
            {
                if (e == null) return null;
                var d = new WorkStageDto();
                d.WorkStageId = e.WorkStageId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public WorkStage GetWorkStage(WorkStageDto e)
        {
            try
            {
                if (e == null) return null;
                var d = new WorkStage();
                d.WorkStageId = e.WorkStageId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        ///  //GetSkillLevelDTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public SkillLevelDto GetSkillLevelDTO(SkillLevel e)
        {
            try
            {
                if (e == null) return null;
                var d = new SkillLevelDto();
                d.SkillLevelId = e.SkillLevelId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// //GetSkillDTO
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public SkillDto GetSkillDTO(Skill e)
        {
            try
            {
                if (e == null) return null;
                var d = new SkillDto();
                d.SkillId = e.SkillId;
                d.Name = e.Name;
                return d;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetRelatedId(Guid? guid,string relatedTo)
        {
            try
            {
                if (relatedTo == "Account")
                {
                    return dbEnt.Accounts.FirstOrDefault(x => x.AccountId == guid) == null ? "" : dbEnt.Accounts.FirstOrDefault(x => x.AccountId == guid).Name;
                }
                else if (relatedTo == "Opportunity")
                {
                    return dbEnt.Opportunities.FirstOrDefault(x => x.OpportunityId == guid) == null ? "" : dbEnt.Opportunities.FirstOrDefault(x => x.OpportunityId == guid).Title;

                }
                else if (relatedTo == "Leads")
                {
                    return dbEnt.Leads.FirstOrDefault(x => x.LeadId == guid) == null ? "" : dbEnt.Leads.FirstOrDefault(x => x.LeadId == guid).Name;

                }
                else if (relatedTo == "Device")
                {
                    return dbEnt.Devices.FirstOrDefault(x => x.DeviceId == guid) == null ? "" : dbEnt.Devices.FirstOrDefault(x => x.DeviceId == guid).Name;

                }
                else if (relatedTo == "Cases")
                {
                    return dbEnt.Cases.FirstOrDefault(x => x.CaseId == guid) == null ? "" : dbEnt.Cases.FirstOrDefault(x => x.CaseId == guid).CaseTitle;

                }
                //else if (relatedTo == "Contacts")
                //{
                //    return dbEnt.Contacts.FirstOrDefault(x => x.Id == guid) == null ? "" : dbEnt.Contacts.FirstOrDefault(x => x.Id == guid).Name;

                //}
                return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetDeviceOnId(Guid? guid)
        {
            try
            {
               string DeviceName = dbEnt.Devices.FirstOrDefault(x => x.DeviceId == guid) == null ? "" : dbEnt.Devices.FirstOrDefault(x => x.DeviceId == guid).Name;
                return DeviceName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string GetAccountOnId(Guid? guid)
        {
            try
            {
                string AccountName = dbEnt.Accounts.FirstOrDefault(x => x.AccountId == guid) == null ? "" : dbEnt.Accounts.FirstOrDefault(x => x.AccountId == guid).Name;
                return AccountName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string GetPriority(int? value)
        {
            try
            {
                string Threshold = dbEnt.Priorities.FirstOrDefault(x => x.Value == value) == null ? "" : dbEnt.Priorities.FirstOrDefault(x => x.Value == value).Name;
                return Threshold;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetGraphOnId(Guid? guid)
        {
            try
            {
                string GraphName = dbEnt.Graphs.FirstOrDefault(x => x.GraphId == guid) == null ? "" : dbEnt.Graphs.FirstOrDefault(x => x.GraphId == guid).GraphName;
                return GraphName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetSensorOnId(Guid? guid)
        {
            try
            {
                string SensorName = dbEnt.Sensors.FirstOrDefault(x => x.SensorId == guid) == null ? "" : dbEnt.Sensors.FirstOrDefault(x => x.SensorId == guid).SensorName;
                return SensorName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string GetCurrencyOnId(Guid? guid)
        {
            try
            {
                string Currency = dbEnt.Currencies.FirstOrDefault(x => x.CurrencyId == guid) == null ? "" : dbEnt.Currencies.FirstOrDefault(x => x.CurrencyId == guid).Name;
                return Currency;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        



    }
}
