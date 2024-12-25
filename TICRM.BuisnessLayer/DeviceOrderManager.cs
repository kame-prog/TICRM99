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
    public class DeviceOrderManager :BaseManager
    {
        public bool SaveDeviceOrder(OrderDeviceDto orderDeviceDto, string CurrentUserId, string UserCompanyID)
        {
			try
			{

                InsertEventLog("SaveDeviceOrder", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.DeviceOrderManager.SaveDeviceOrder", "");
                OrderDevice orderdevice;
                orderdevice = objMapper.GetOrderDevice(orderDeviceDto); 
                orderdevice.OrderDate = DateTime.Now;
                orderdevice.OrderBy = CurrentUserId;
                orderdevice.Order_id = Guid.NewGuid();
                orderdevice.Company = Guid.Parse(UserCompanyID);
                orderdevice.OrderStatus = "Pending";
                dbEnt.OrderDevices.Add(orderdevice);
                if (dbEnt.SaveChanges()>0)
                {
                    InsertEventLog("SaveDeviceOrder", EventType.Log, EventColor.yellow, "Save Order", "TICRM.BusinessLayer.DeviceOrderManager.SaveDeviceOrder", "");
                    return true;
                }
                return false;
            }
			catch (Exception ex)
			{
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
    }
}
