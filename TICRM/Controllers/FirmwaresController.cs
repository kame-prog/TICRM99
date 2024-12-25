
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TICRM.DTOs;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using TICRM.BuisnessLayer;
using Newtonsoft.Json;

namespace TICRM.Controllers
{
    /************Firmwares Controller************
    Class [FirmwaresController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with Firmwares like, 
    ||             navigating to the pages, getting associated modules for specific Firmware]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||  Changes Made:   [20/08/2020     Added ServerSide Processing for Jquery DataTables     Sikandar Mustafa]
    ||                  
     ********************************************/

    public class FirmwaresController : BaseController
    {
        FirmwaresManager fManager = new FirmwaresManager();

        string targetPath = @"C:\inetpub\wwwroot";

        MqttClient client;

        string clientId;

        public class Counts
        {
            public int Total;
            public int Pending;
        }

        /// <summary>
        /// Get the firmwares and return on index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the firmwares list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetFirmwaresList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<FirmwareDto> obj = fManager.GetFirmwaresList(sEcho, iDisplayStart, iDisplayLength, sSearch);

            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.version).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.description).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Date).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Date).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.server).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.server).ToList();
                    }
                    break;


            }
            int totalRecord = fManager.GetTotalCount();

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("{");
            sb.Append("\"sEcho\": ");
            sb.Append(sEcho);
            sb.Append(",");
            sb.Append("\"iTotalRecords\": ");
            sb.Append(totalRecord);
            sb.Append(",");
            sb.Append("\"iTotalDisplayRecords\": ");
            sb.Append(totalRecord);
            sb.Append(",");
            sb.Append("\"aaData\": ");
            sb.Append(JsonConvert.SerializeObject(obj));
            sb.Append("}");
            return sb.ToString();
        }

        // GET: Firmwares/Create
        /// <summary>
        /// Create view.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create(string MacAddress = null)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        //[Bind(Include = "Id,Version,Description, server")] 
        /// <summary>
        /// Creates the specified firmware.
        /// </summary>
        /// <param name="firmware">The firmware.</param>
        /// <param name="file">The file.</param>
        /// <param name="mac">The mac.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
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
                    firmware.Date = DateTime.Now;
                    firmware.File = file.FileName;
                    bool condition = fManager.SaveFirmware(firmware);

                    PublishMqtt(file, mac, firmware);
                    return RedirectToAction("Index");
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Publish messages to MQTT broker.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="mac">The mac.</param>
        /// <param name="firmware">The firmware.</param>
        /// <exception cref="System.Exception"></exception>
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
                                   Path.GetFileName(file.FileName));
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
                throw new Exception(ex.Message + ex.InnerException);
            }

        }


        /// <summary>
        /// Gets the firmwares pending.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetFirmwaresPending()
        {
            try
            {
                List<FirmwareDto> firmawares = fManager.GetFirmwares().Where(a => a.Status == "Pending").ToList();
                var data = firmawares.GroupBy(x => x.Date)
                    .Select(x => new
                    {
                        Value = x.Count(),
                        date = (DateTime)x.Key

                    }).ToArray();


                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the firmwares updated.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetFirmwaresUpdated()
        {
            try
            {
                List<FirmwareDto> firmawares = fManager.GetFirmwares().Where(a => a.Status == "Updated").ToList();

                var data = firmawares.GroupBy(x => x.Date)
                    .Select(x => new
                    {
                        Value = x.Count(),
                        date = (DateTime)x.Key

                    }).ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    }
}
