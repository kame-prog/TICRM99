using System;
using System.Web.Mvc;

namespace TICRM.Controllers
{
    /************CloudConfiguration Controller************
    Class [CloudConfigurationController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with CloudConfiguration]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
     ********************************************/
    public class CloudConfigurationController : BaseController
    {
        private static string orgId = "";
        private static string appId = "";
        private static string apiKey = "";
        private static string authToken = "";


        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Configures it to cloud.
        /// </summary>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="Password">The password.</param>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <param name="APIKey">The API key.</param>
        /// <param name="AuthToken">The authentication token.</param>
        /// <param name="DeviceType">Type of the device.</param>
        /// <param name="DeviceId">The device identifier.</param>
        /// <returns>JsonResult.</returns>
        public JsonResult ConfigureItToCloud(string UserName, string Password, string OrganizationId, string APIKey, string AuthToken, string DeviceType, string DeviceId)
        {
            try
            {


            }
            catch (Exception)
            {
                // ignore
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// return View for Ibms cloud browse.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult IBMCloudBrowse()
        {
            return View();
        }

        /// <summary>
        /// Submits the ibm cloud browse.
        /// </summary>
        /// <param name="Option">The option.</param>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <param name="APIKey">The API key.</param>
        /// <param name="AuthToken">The authentication token.</param>
        /// <param name="DeviceType">Type of the device.</param>
        /// <param name="DeviceId">The device identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult SubmitIBMCloudBrowse(string Option, string OrganizationId, string APIKey, string AuthToken, string DeviceType, string DeviceId)
        {
            if (Option == "GetAllDevices")
            {

            }
            else if (Option == "RegisterDevice")
            {

            }
            else if (Option == "RegisterMultipleDevices")
            {

            }
            return View();
        }
    }
}