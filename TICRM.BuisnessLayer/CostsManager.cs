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
    ||  Class [CostsManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [|This class serves as a bridge between the front end and the database. 
    ||             The class provides list of costs in general and another list specifically
    ||             for the accounts]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class CostsManager : BaseManager
    {
        /// <summary>
        /// Gets the account cost by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>List&lt;CostDto&gt;.</returns>
        public List<CostDto> GetAccountCostById(Guid id)
        {
            try
            {
                InsertEventLog("GetAccountCostById", EventType.Log, EventColor.yellow, "to get list of Costs on basis of account Id", "TICRM.BuisnessLayer.CostsManager.GetAccountCostById", "");

                List<CostDto> costdto = new List<CostDto>();
                List<Cost> costs = dbEnt.Costs.Where(a => a.AccountId == id).ToList();
                foreach (Cost item in costs.CollectionNotNull())
                {
                    costdto.Add(objMapper.GetCostDto(item)); // add in a list object

                }
                return costdto;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccountCostById", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CostsManager.GetAccountCostById", "");
                throw;
            }
        }
        /// <summary>
        /// Gets the costs.
        /// </summary>
        /// <returns>List&lt;CostDto&gt;.</returns>
        public List<CostDto> GetCosts()
        {
            try
            {
                InsertEventLog("GetCosts", EventType.Log, EventColor.yellow, "to get list of Costs ", "TICRM.BuisnessLayer.CostsManager.GetCosts", "");

                List<CostDto> costdto = new List<CostDto>();
                List<Cost> costs = dbEnt.Costs.ToList();
                foreach (Cost item in costs.CollectionNotNull())
                {
                    costdto.Add(objMapper.GetCostDto(item)); // add in a list object

                }
                return costdto;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCosts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CostsManager.GetCosts", "");
                throw;
            }

        }
    }
}
