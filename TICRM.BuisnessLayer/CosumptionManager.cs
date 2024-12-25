using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [CosumptionManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [|This class serves as a bridge between the front end and the database. 
    ||             Getting list for all the consumptions, Getting consuptions details for
                   a specific account, Getting consumption for each date
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class CosumptionManager : BaseManager
    {
        #region Variables
        AccountViewModel avm = new AccountViewModel();
        DeviceManager dm = new DeviceManager();
        AccountManager am = new AccountManager();
        DeviceDto device = new DeviceDto();
        AccountDto account = new AccountDto();
        List<DeviceDto> dto = new List<DeviceDto>();
        List<AccountDto> acc = new List<AccountDto>();
        #endregion        

        /// <summary>
        /// Gets the consumption details for accounts.
        /// </summary>
        /// <returns>AccountViewModel.</returns>
        public AccountViewModel GetConsumptionDetails()
        {
            try
            {
                InsertEventLog("GetConsumptionDetails", EventType.Log, EventColor.yellow, "to get lConsumption details ", "TICRM.BuisnessLayer.CosumptionManager.GetConsumptionDetails", "");

                List<Consumption> discon = dbEnt.Consumptions.ToList();

                foreach (var item in discon.CollectionNotNull())
                {

                    device = dm.GetDevice(item.DeviceId);
                    account = am.GetAccount(item.AccountId);
                    dto.Add(objMapper.GetDeviceDTO(item.Device));
                    acc.Add(objMapper.GetAccountDTO(item.Account));
                }

                avm.ConsumptionAccounts = acc;
                avm.ConsumptionDevices = dto;
                return avm;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetConsumptionDetails", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CosumptionManager.GetConsumptionDetails", "");
                throw;
            }

        }
        /// <summary>
        /// Gets the consumptions.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int getConsumptions()
        {
            List<Consumption> consumption = dbEnt.Consumptions.ToList();
            int total = 0;
            foreach(Consumption item in consumption.CollectionNotNull())
            {
                Match match = Regex.Match(item.Unit, @"(\d+)");
                int unit = int.Parse(match.Groups[1].Value);
                total += unit;
            }
            return total;
        }

        /// <summary>
        /// Gets the disconnections.
        /// </summary>
        /// <returns>List&lt;ConsumptionDTO&gt;.</returns>
        public List<ConsumptionDTO> GetDisconnections()
        {
            try
            {
                InsertEventLog("GetDisconnections", EventType.Log, EventColor.yellow, "to get list disconections  to measure cinsumption", "TICRM.BuisnessLayer.CosumptionManager.GetDisconnections", "");
                List<ConsumptionDTO> consumtionDto = new List<ConsumptionDTO>();
                List<Consumption> consumption = dbEnt.Consumptions.ToList();
                foreach (Consumption item in consumption.CollectionNotNull())
                {
                    consumtionDto.Add(objMapper.GetConsumptionDTO(item)); // add in a list object

                }
                return consumtionDto;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDisconnections", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CosumptionManager.GetDisconnections", "");
                throw;
            }

        }
    }
}
