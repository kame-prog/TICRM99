using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [DisconnectionManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting discpnnections for a specific account and against each date
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class DisconnectionManager : BaseManager
    {
        #region Variables
        AccountViewModel avm = new AccountViewModel();
        DeviceManager dm = new DeviceManager();
        AccountManager am = new AccountManager();
        DeviceDto device = new DeviceDto();
        AccountDto account = new AccountDto();
        List<DeviceDto> dto = new List<DeviceDto>();
        List<AccountDto> acc = new List<AccountDto>();
        List<DeviceDto> dev = new List<DeviceDto>();
        List<AccountDto> accc = new List<AccountDto>();
        #endregion

        /// <summary>
        /// Gets the disconnections.
        /// </summary>
        /// <returns>List&lt;DisconnectionDto&gt;.</returns>
        public List<DisconnectionDto> GetDisconnections()
        {
            try
            {
                InsertEventLog("GetDisconnections", EventType.Log, EventColor.yellow, "to get list of Disconnections ", "TICRM.BuisnessLayer.DisconnectionManager.GetDisconnections", "");
                List<DisconnectionDto> dis = new List<DisconnectionDto>();
                List<Disconnection> discon = dbEnt.Disconnections.ToList();

                foreach (Disconnection item in discon.CollectionNotNull())
                {
                    dis.Add(objMapper.GetDisconnectionDto(item)); // add in a list object
                }
                return dis;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDisconnections", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DisconnectionManager.GetDisconnections", "");
                throw;
            }
            

        }

        public List<DisconnectionDto> GetAccountDisconnections(Guid AccountId)
        {
            try
            {
                InsertEventLog("GetAccountDisconnections", EventType.Log, EventColor.yellow, "to Get list of Disconnections for an account", "TICRM.BuisnessLayer.DisconnectionManager.GetAccountDisconnections", "");
                List<DisconnectionDto> dis = new List<DisconnectionDto>();
                List<Disconnection> discon = dbEnt.Disconnections.Where(x => x.AccountId == AccountId).ToList();

                foreach (Disconnection item in discon.CollectionNotNull())
                {
                    dis.Add(objMapper.GetDisconnectionDto(item)); // add in a list object
                }
                return dis;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountDisconnections", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DisconnectionManager.GetAccountDisconnections", "");
                throw;
            }


        }

        /// <summary>
        /// Gets the disconnections for account.
        /// </summary>
        /// <returns>AccountViewModel.</returns>
        public AccountViewModel GetDisconnectionsAVM()
        {
            try
            {
                InsertEventLog("GetDisconnectionsAVM", EventType.Log, EventColor.yellow, "to get list of Disconnections ", "TICRM.BuisnessLayer.DisconnectionManager.GetDisconnectionsAVM", "");
                List<Disconnection> discon = dbEnt.Disconnections.ToList();
                List<Consumption> cons = dbEnt.Consumptions.ToList();

                IEnumerable<Disconnection> s = dbEnt.Disconnections.ToList();


                foreach (var item in discon.CollectionNotNull())
                {
                    device = dm.GetDevice(item.DeviceId);
                    account = am.GetAccount(item.AccountId);
                    dto.Add(objMapper.GetDeviceDTO(item.Device));
                    acc.Add(objMapper.GetAccountDTO(item.Account));
                }
                foreach (var item in cons.CollectionNotNull())
                {
                    device = dm.GetDevice(item.DeviceId);
                    account = am.GetAccount(item.AccountId);
                    dev.Add(objMapper.GetDeviceDTO(item.Device));
                    accc.Add(objMapper.GetAccountDTO(item.Account));
                }

                avm.DashboardAccounts = acc;
                avm.DashboardDevices = dto;
                avm.ConsumptionAccounts = accc;
                avm.ConsumptionDevices = dev;
                return avm;
            }
            catch(Exception ex)
            {
                InsertEventMonitor("GetDisconnectionsAVM", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DisconnectionManager.GetDisconnectionsAVM", "");
                throw;
            }
            
        }

    }
}
