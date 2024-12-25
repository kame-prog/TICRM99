using System;
using System.Linq;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Admin Controller************
  Class [AdminController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with Admin ]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ********************************************/

    public class AdminController : BaseController
    {

        OpportunityManager om = new OpportunityManager();
        AccountManager am = new AccountManager();
        WorkOrderManager wm = new WorkOrderManager();
        WorkFlowManager wfm = new WorkFlowManager();
        WorkFlowReportManager wfrm = new WorkFlowReportManager();
        FirmwaresManager fm = new FirmwaresManager();
        DeviceManager dm = new DeviceManager();
        DisconnectionManager dis = new DisconnectionManager();
        CosumptionManager cm = new CosumptionManager();
        AccountViewModel avm = new AccountViewModel();

        /// <summary>
        /// Getting accounts detials on admin.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult AccountDetailforAdmin(Guid accountId)
        {
            try
            {
                //var data = am.GetAccount();
                AccountViewModel accWithDetail = am.GetAccountAndDetails(accountId);
                var c = new AccDetails
                {
                    devices = accWithDetail.accountDevices.Count(),
                    customerAssets = accWithDetail.accountAssetes.Count(),
                    workflow = accWithDetail.accountWorkflow.Count(),
                    workorders = accWithDetail.accountWorkOrder.Count(),
                    open = om.GetOpportunitiesCountAccount(accountId).Open,
                    lost = om.GetOpportunitiesCountAccount(accountId).Lost,
                    won = om.GetOpportunitiesCountAccount(accountId).Lostwon

                };
                return Json(c, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Admin index page 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                avm.workflowReportAdmin = wfrm.GetWorkFlowReports();
                ViewBag.Team = om.Teams.Count();
                ViewBag.Users = om.Users.Count();
                ViewBag.Accounts = am.Accounts.Count();
                ViewBag.Workorders = wm.WorkorderCount();
                ViewBag.Workflows = wfm.GetWorkFlows().Count();
                ViewBag.WorkflowReports = wfrm.GetWorkFlowReports().Count();
                ViewBag.Firmwares = fm.GetFirmwares().Count();
                ViewBag.Gateways = dm.GetGatewayDeviceCount();
                ViewBag.Disconnections = dis.GetDisconnections().Count();
                ViewBag.Consumptions = cm.getConsumptions();
                return View(avm);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public class AccDetails
        {
            public int open { get; set; }
            public int lost { get; set; }
            public int won { get; set; }
            public int customerAssets { get; set; }
            public int devices { get; set; }
            public int workflow { get; set; }
            public int workorders { get; set; }

        }
    }
}