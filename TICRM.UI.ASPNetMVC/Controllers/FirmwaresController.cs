using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using TICRM.UI.ASPNetMVC.App_Start;
using TICRM.UI.ASPNetMVC.Helpers;
using TICRM.UI.ASPNetMVC.Resources;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TICRM.UI.ASPNetMVC.Controllers
{
    [SessionExpire]
    [Authorize(Roles = "Admin")]
    public class FirmwaresController : Controller
    {
        FirmwaresManager fManager = new FirmwaresManager();
        private DeviceManager dm = new DeviceManager();
        string targetPath = @"C:\inetpub\wwwroot";

        MqttClient client;

        string clientId;

        public class Counts
        {
            public int Total;
            public int Pending;
        }

        // GET: Firmwares
        public ActionResult Index()
        {
            try
            {
                List<FirmwareDto> firmwares = fManager.GetFirmwares();
                return View(firmwares);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public ActionResult Create(string MacAddress = null)
        {
            try
            {
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                FirmwareDto firmwares = new FirmwareDto();
                //Bind data with dropdown
                firmwares.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                return View(firmwares);
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
        public ActionResult Create(FirmwareDto firmware, HttpPostedFileBase file, string mac)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //id and date
                    byte[] gb = Guid.NewGuid().ToByteArray();
                    int i = BitConverter.ToInt32(gb, 0);
                    firmware.Id = i;
                    firmware.File =firmware.version + "_" + file.FileName;
                    bool condition = fManager.SaveFirmware(firmware, false);
                    PublishMqtt(file, mac, firmware);
                    //In Condition check data submitted in DB successfully or not
                    if (!condition)
                    {
                        ModelState.AddModelError("", WarningMessage.DataNotSaved);
                    }
                    else
                    {
                        //When data submitted show successfully toaster on listing screen 
                        TempData["Success"] = SuccessMessage.FirmwareSubmit;
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                string UserCompanyID = Convert.ToString(Session["UserCompany"]); //Get User Company
                //Bind data with dropdown
                firmware.DevicesDropDown = new SelectList(dm.DeviceDropDown(UserCompanyID), "DeviceId", "Name");
                TempData["Warning"] = WarningMessage.EnterField;
                return View(firmware);
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }

        public void PublishMqtt(HttpPostedFileBase file, string mac, FirmwareDto firmware)
        {
            try
            {
                string BrokerAddress = "broker.hivemq.com";
                client = new MqttClient(BrokerAddress, 1883, false, MqttSslProtocols.None, null, null);
                clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                String Topic = "ServerGateway";
                string path = Path.Combine(targetPath,
                                   Path.GetFileName(firmware.version + "_" + file.FileName));
                file.SaveAs(path);
                ViewBag.Message = "File uploaded successfully";

                //MQTT Publish
                if (mac == "all")
                {
                    string msg = "F-A-" + firmware.version + "-" + firmware.File + "-" + firmware.server;
                    client.Publish(Topic, Encoding.UTF8.GetBytes(msg.ToCharArray()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
                else
                {
                    string msg = "F-" + mac + "-" + firmware.version + "-" + firmware.File + "-" + firmware.server;
                    client.Publish(Topic, Encoding.UTF8.GetBytes(msg.ToCharArray()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

                }

                client.Disconnect();
            }
            catch (Exception ex)
            {
                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                throw;
            }

        }


        public ActionResult Delete(int id)
        {
            try
            {
                //Get firmware from DB
                FirmwareDto firmware = fManager.GetFirmware(id);
                //Delete FormWare data From DB
                bool condition = fManager.SaveFirmware(firmware, true);
                if (condition)
                {
                    var photoName = firmware.File;
                    string path = Path.Combine(targetPath, Path.GetFileName(photoName));       //Get File Path
                    //Delete file from Target folder where file is saved
                    if (System.IO.File.Exists(path))
                    {
                        //Delete file if file exist
                        System.IO.File.Delete(path);
                    }
                    return RedirectToAction("Index");
                }
                TempData["Warning"] = "Data could not be deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                // Log the exception using log4net and LogException Class.
                ExceptionLogging.LogException(ex);
                // Display an error view to the user
                return View("Error");
            }
        }
    }
}