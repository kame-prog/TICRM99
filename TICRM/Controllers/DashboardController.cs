using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using System.Web.Security;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Dashboard Controller************
  Class [DashboardController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with Dashboard]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
  ||                  
   ********************************************/
    
    public class DashboardController : BaseController
    {
        private DeviceManager deviceManager = new DeviceManager();
        private WorkOrderManager wo = new WorkOrderManager();
        private AlertManager am = new AlertManager();
        private DisconnectionManager dc = new DisconnectionManager();

        /// <summary>
        /// Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try { 
               

                AccountViewModel av = dc.GetDisconnectionsAVM();

                return View(av);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the tech count.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetTechCount()
        {
            try { 
               //String  x = JsonConvert.SerializeObject(dc.GetDisconnections());
                var count = new Counts
                {
                    Workorders = wo.WorkorderCount(),
                    Alerts = am.GetAlertCounts()

                };
                //return null;
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }
        /// <summary>
        /// Gets all services devices.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAllServicesDevices()
        {
            try
            {
                return Json(deviceManager.DeviceCount(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
        }
    }

        public class Counts
        {
            public int Workorders;
            public int Alerts;
        }
    }
}