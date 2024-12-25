using Microsoft.AspNet.Identity;
//using Stripe;
//using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DAL;
using TICRM.DTOs;
//using Stripe.Infrastructure;
using TwoCheckout;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using EllipticCurve.Utils;
using System.Data.Entity;
using log4net;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;
//using Stripe;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize]
    public class DeviceOrderController : Controller
    {
        private DeviceOrderManager deviceOrder = new DeviceOrderManager();

        // GET: DeviceOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderDevice()
        {
            try
            {
                OrderDeviceDto orderDevice = new OrderDeviceDto();
                orderDevice.CountryDropdown = new SelectList(deviceOrder.CountryDropDown(), "Country_Name", "Country_Name");
                return View(orderDevice);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderDevice(OrderDeviceDto orderDevice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();              // pass current userid
                    string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Pass Company ID
                    var condition = deviceOrder.SaveDeviceOrder(orderDevice, CurrentUserId, UserCompanyID);  //Device order create function
                   
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //TempData["Success"] = "Device ordered successfully";
                        return RedirectToAction("Billing");
                    }
                   
                }
                orderDevice.CountryDropdown = new SelectList(deviceOrder.CountryDropDown(), "Country_Name", "Country_Name");
                return View(orderDevice);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }


        public ActionResult Billing()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View(); 
        }
    }
}
