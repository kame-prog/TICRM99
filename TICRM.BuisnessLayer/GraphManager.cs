using System;
using System.Collections.Generic;
using System.Linq;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
||  Class [GraphManager] 
||
||  Author:  [Undefined]
||
||  Purpose:  [This class serves as a bridge between the front end and the database. 
||             Getting List of Graphs 
||
||  Inherits From:  [BaseManager]
||
||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
****************************************************************************************/
    public class GraphManager : BaseManager
    {
        /// <summary>
        /// Gets the graph list.
        /// </summary>
        /// <returns>List&lt;GraphDto&gt;.</returns>
        public List<GraphDto> GetGraphList()
        {
            try
            {
                InsertEventLog("GetGraphList", EventType.Log, EventColor.yellow, "Get List Of Graph","TICRM.BusinessLayer.GraphManager.GetGraphList", "");
                List<GraphDto> graphDtos = new List<GraphDto>();
                List<Graph> graph = dbEnt.Graphs.ToList();
                foreach (Graph item in graph.CollectionNotNull())
                {
                    graphDtos.Add(objMapper.GetGraphDto(item));
                }
                return graphDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetGraphList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.GraphManager.GetGraphList", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

    }
}
