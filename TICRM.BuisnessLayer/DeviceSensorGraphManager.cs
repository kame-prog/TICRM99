using System;
using System.Collections.Generic;
using System.Linq;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [DeviceSensorGraphManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Crud operations are being performed here, Getting devices sensor graphs
    ||             on asset id, device id and devices sensor graphs itself. Getting graphs
    ||             lists and sensors lists
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class DeviceSensorGraphManager : BaseManager
    {
        public DeviceSensorGraphManager()
        {

        }

        //*      Get device sensor graphs in list form     *//
        public List<DeviceSensorGraphDto> GetDeviceSensorGraphs_List(string CurrentUserId, string UserRole, string UserCompany)
        {
            InsertEventLog("GetDeviceSensorGraphs_List", EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphs_List", "");
            try
            {
                List<DeviceSensorGraphDto> deviceSensorGraphDtos = new List<DeviceSensorGraphDto>();
                List<DeviceSensorGraph> deviceSensorGraphs = dbEnt.sp_DeviceSensors_Get(CurrentUserId, UserRole, UserCompany).ToList();
                foreach (var item in deviceSensorGraphs.CollectionNotNull())
                {
                    deviceSensorGraphDtos.Add(objMapper.GetDeviceSensorGraphDto(item));
                }

                InsertEventLog("GetDeviceSensorGraphs_List", EventType.Log, EventColor.yellow, "Ready to return data in list form ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphs_List", "");
                
                return deviceSensorGraphDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDeviceSensorGraphs_List", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphs_List", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            
        }

        /// <summary>
        /// Gets the device sensor graph list.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDeviceSensorGraphList()
        {
            try
            {
                InsertEventLog("GetDeviceSensorGraphList", EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphList", "");

                // query in database to get desired result
                var query = (from dsg in dbEnt.DeviceSensorGraphs
                             join d in dbEnt.Devices on dsg.DeviceId equals d.DeviceId
                             join g in dbEnt.Graphs on dsg.GraphId equals g.GraphId
                             join s in dbEnt.Sensors on dsg.SensorId equals s.SensorId
                             where dsg.IsDeleted == false
                             select new { dsg.DeviceSensorGraphId, DeviceName = d.Name, g.GraphName, s.SensorName }).ToList();
                string status = Newtonsoft.Json.JsonConvert.SerializeObject(query); // convert query in json Sting
                
                InsertEventLog("GetDeviceSensorGraphList", EventType.Log, EventColor.yellow, "Ready to response Json Status ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphList", "");
                return status; // return in json
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDeviceSensorGraphList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphList", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the DSG list on device identifier.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns>System.String.</returns>
        public string GetDSGListOn_DeviceId(Guid deviceId)
        {
            InsertEventLog("GetDeviceSensorGraphList", EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDSGListOn_DeviceId", "");
            try
            {
                // query in database to get desired result
                var query = (from dsg in dbEnt.DeviceSensorGraphs
                             join d in dbEnt.Devices on dsg.DeviceId equals d.DeviceId
                             join g in dbEnt.Graphs on dsg.GraphId equals g.GraphId
                             join s in dbEnt.Sensors on dsg.SensorId equals s.SensorId
                             where dsg.IsDeleted == false && dsg.DeviceId == deviceId
                             select new { dsg.DeviceSensorGraphId, DeviceName = d.Name, g.GraphName, s.SensorName }).ToList();
                string status = Newtonsoft.Json.JsonConvert.SerializeObject(query); // convert query in json Sting
                InsertEventLog("GetDeviceSensorGraphList", EventType.Log, EventColor.yellow, "Ready to response Json Status ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDSGListOn_DeviceId", "");
                return status; // return in json
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDeviceSensorGraphList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDSGListOn_DeviceId", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the sensor list.
        /// </summary>
        /// <returns>List&lt;SensorDto&gt;.</returns>
        public List<SensorDto> GetSensorList()
        {
            try
            {
                InsertEventLog("GetSensorList", EventType.Log, EventColor.yellow, "to get list of sensor dto ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetSensorList", "");
                List<SensorDto> SensorDtos = new List<SensorDto>(); // create a new object of SensorDtos list
                List<Sensor> Sensors = dbEnt.Sensors.Where(x => x.IsDeleted == null).ToList();  // create a new object of Sensors
                // apply iteration on sensor list
                foreach (Sensor item in Sensors.CollectionNotNull())
                {
                    SensorDtos.Add(objMapper.GetSensorDto(item));
                }
                return SensorDtos;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetSensorList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetSensorList", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Submits the device sensor graph.
        /// </summary>
        /// <param name="DeviceId">The device identifier.</param>
        /// <param name="SensorId">The sensor identifier.</param>
        /// <param name="GraphId">The graph identifier.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="Data">The data.</param>
        /// <param name="Channel">The channel.</param>
        /// <param name="Network">The network.</param>
        /// <param name="Level">The level.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SubmitDeviceSensorGraph(Guid DeviceId, Guid SensorId, Guid GraphId, string CurrentUserId, string Data, 
            string Channel, string Network, int? Level)
        {
            try
            {
                InsertEventLog("SubmitDeviceSensorGraph",  EventType.Log, EventColor.yellow, "create new record of device sensor graph ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.SubmitDeviceSensorGraph", "");
                DeviceSensorGraph dsg = new DeviceSensorGraph(); // create a new Object DeviceSensorGraph
                dsg.DeviceSensorGraphId = Guid.NewGuid(); // add new guid in object
                dsg.DeviceId = DeviceId;
                dsg.SensorId = SensorId;
                dsg.GraphId = GraphId;
                dsg.CreatedOn = DateTime.Now;
                dsg.CreatedBy = CurrentUserId;
                dsg.IsDeleted = false;
                dsg.Data = Data;
                dsg.Channel = Channel;
                dsg.Network = Network;
                dsg.TankLevel = Level;

                //dsg.Threshold = ThreshId;
                dbEnt.DeviceSensorGraphs.Add(dsg);
                dbEnt.SaveChanges(); // save in databse
                return true;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SubmitDeviceSensorGraph", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.SubmitDeviceSensorGraph", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        public bool SaveDeviceSensorGraph(DeviceSensorGraphDto deviceSensorGraphDto, string CurrentUserId, string UserCompanyID, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveDeviceSensorGraph", EventType.Log, EventColor.yellow, "Successfully Enter in DeviceSensorGraph", "TICRM.BusinessLayer.SaveDeviceSensorGraphManager", CurrentUserId);

                DeviceSensorGraph ObjdeviceSensorGraph;
                string MqttPublishtopic;
                MqttPublishtopic = deviceSensorGraphDto.MqttPublishtopic;
                if (isEditMode)
                {
                    ObjdeviceSensorGraph = objMapper.GetDeviceSensorGraph(deviceSensorGraphDto);
                    DeviceSensorGraph Obj = dbEnt.DeviceSensorGraphs.FirstOrDefault(x => x.DeviceSensorGraphId == deviceSensorGraphDto.DeviceSensorGraphId);
                    if (Obj!=null)
                    {
                        if (isDeleteMode)
                        {
                            InsertEventLog("SaveDeviceSensorGraph", EventType.Log, EventColor.yellow, "For Delete. Successfully Enter in SaveDeviceSensorGraph", "TICRM.BusinessLayer.SaveDeviceSensorGraphManager", CurrentUserId);
                            Obj.IsDeleted = true;
                        }
                        else
                        {
                            InsertEventLog("SaveDeviceSensorGraph", EventType.Log, EventColor.yellow, "For Edit. Successfully Enter in SaveDeviceSensorGraph", "TICRM.BusinessLayer.DeviceSensorGraphManager", CurrentUserId);
                            Obj.DeviceSensorGraphId = ObjdeviceSensorGraph.DeviceSensorGraphId;
                            Obj.DeviceId = ObjdeviceSensorGraph.DeviceId;
                            Obj.SensorId = ObjdeviceSensorGraph.SensorId;
                            Obj.GraphId = ObjdeviceSensorGraph.GraphId;
                            Obj.Channel = ObjdeviceSensorGraph.Channel;
                            Obj.Network = ObjdeviceSensorGraph.Network;
                            Obj.Data = ObjdeviceSensorGraph.Data;
                            //Obj.CreatedBy = ObjdeviceSensorGraph.CreatedBy;
                            //Obj.CreatedOn = ObjdeviceSensorGraph.CreatedOn;
                            Obj.UpdatedBy = CurrentUserId;
                            Obj.MqttPublishtopic = ObjdeviceSensorGraph.MqttPublishtopic;
                            Obj.UpdatedOn = DateTime.Now;
                            //ObjdeviceSensorGraph = objMapper.UpdateDeviceSensorGraph(deviceSensorGraphDto);
                            //ObjdeviceSensorGraph.CreatedBy = CurrentUserId;
                        }
                        if (dbEnt.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    //ObjdeviceSensorGraph = objMapper.GetDeviceSensorGraph(deviceSensorGraphDto);
                    //ObjdeviceSensorGraph.MqttPublishtopic = MqttPublishtopic;
                    InsertEventLog("SaveDeviceSensorGraph", EventType.Log, EventColor.yellow, "For Create. Successfully Save DeviceSensorGraph", "TICRM.BusinessLayer.SaveDeviceSensorGraphManager", CurrentUserId);
                    
                    ObjdeviceSensorGraph = objMapper.GetDeviceSensorGraph(deviceSensorGraphDto);
                    ObjdeviceSensorGraph.MqttPublishtopic = MqttPublishtopic;
                    ObjdeviceSensorGraph.DeviceSensorGraphId = Guid.NewGuid();
                    ObjdeviceSensorGraph.CreatedBy = CurrentUserId;
                    ObjdeviceSensorGraph.Company = Guid.Parse(UserCompanyID);
                    ObjdeviceSensorGraph.CreatedOn = DateTime.Now;
                    //ObjdeviceSensorGraph.MqttPublishtopic = DateTime.Now;

                    dbEnt.DeviceSensorGraphs.Add(ObjdeviceSensorGraph);
                }
                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("SaveDeviceSensorGraph", EventType.Log, EventColor.yellow, "data saved successfully in SaveDeviceSensorGraph", "TICRM.BusinessLayer.SaveDeviceSensorGraphManager", CurrentUserId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveDeviceSensorGraph", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.SaveDeviceSensorGraphManager", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            };
            return false;
        }

            /// <summary>
            /// Gets the channel counts.
            /// </summary>
            /// <returns>Counts.</returns>
            public Counts GetChannelCounts()
           {
            try
            {
                InsertEventLog("GetChannelCounts", EventType.Log, EventColor.yellow, "Enter to get channek counts", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetChannelCounts", "");

                var types = new Counts
                {

                    MQTT = dbEnt.DeviceSensorGraphs.Where(a => a.Channel == "MQTT").Count(),
                    HTTP = dbEnt.DeviceSensorGraphs.Where(a => a.Channel == "HTTP").Count(),
                    LORAWAN = dbEnt.DeviceSensorGraphs.Where(a => a.Channel == "LORAWAN").Count(),
                    CELLULAR = dbEnt.DeviceSensorGraphs.Where(a => a.Network == "Cellular").Count(),
                    ETHERNET = dbEnt.DeviceSensorGraphs.Where(a => a.Network == "Ethernet").Count(),
                    WIFI = dbEnt.DeviceSensorGraphs.Where(a => a.Network == "WIFI").Count()
                };
                return types;
            }
            catch(Exception ex)
            {
                InsertEventMonitor("GetChannelCounts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetChannelCounts", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }


        /// <summary>
        /// Updates the device sensor graph.
        /// </summary>
        /// <param name="DeviceSensorGraphId">The device sensor graph identifier.</param>
        /// <param name="DeviceId">The device identifier.</param>
        /// <param name="SensorId">The sensor identifier.</param>
        /// <param name="GraphId">The graph identifier.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UpdateDeviceSensorGraph(Guid DeviceSensorGraphId, Guid DeviceId, Guid SensorId, Guid GraphId, string CurrentUserId)
        {
            try
            {
                InsertEventLog("UpdateDeviceSensorGraph",  EventType.Log, EventColor.yellow, "Enter ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.UpdateDeviceSensorGraph", "");
                DeviceSensorGraph dsg = dbEnt.DeviceSensorGraphs.FirstOrDefault(x => x.DeviceSensorGraphId == DeviceSensorGraphId); // query in database
                if (dsg != null)
                {
                    dsg.DeviceId = DeviceId;
                    dsg.SensorId = SensorId;
                    dsg.GraphId = GraphId;
                    dsg.UpdatedOn = DateTime.Now;
                    dsg.UpdatedBy = CurrentUserId;
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("UpdateDeviceSensorGraph",  EventType.Log, EventColor.yellow, "update successfully ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.UpdateDeviceSensorGraph", "");
                        return true;
                    }
                }

                return false;  //there is no changes is done in db against the object
            }
            catch (Exception ex)

            {
                InsertEventMonitor("UpdateDeviceSensorGraph", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.UpdateDeviceSensorGraph", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Devices the sensor graph of mac.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <returns>System.String.</returns>
        public string DeviceSensorGraphOfMAC(string MacAddress)
        {
            try
            {
                InsertEventLog("DeviceSensorGraphOfMAC",  EventType.Log, EventColor.yellow, "Get Device sensor graph on macaddres ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeviceSensorGraphOfMAC", "");
                // apply query in database to get desired result
                var query = (from dsg in dbEnt.DeviceSensorGraphs
                             join d in dbEnt.Devices on dsg.DeviceId equals d.DeviceId
                             join g in dbEnt.Graphs on dsg.GraphId equals g.GraphId
                             join s in dbEnt.Sensors on dsg.SensorId equals s.SensorId
                             where d.Mac == MacAddress && dsg.IsDeleted == false
                             select new { dsg.DeviceId, d.Mac, DeviceName = d.Name, d.Latitude, d.Longitude, g.GraphName, s.SensorName }).ToList();

                string status = Newtonsoft.Json.JsonConvert.SerializeObject(query);
                InsertEventLog("DeviceSensorGraphOfMAC", EventType.Log, EventColor.yellow, "return data in json ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeviceSensorGraphOfMAC", "");
                return status;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("DeviceSensorGraphOfMAC", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeviceSensorGraphOfMAC", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Devices the sensor graph of asset.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <returns>System.String.</returns>
        public string DeviceSensorGraphOfAsset(Guid MacAddress)
        {
            try
            {
                InsertEventLog("DeviceSensorGraphOfMAC", EventType.Log, EventColor.yellow, "Get Device sensor graph on macaddres ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeviceSensorGraphOfMAC", "");
                // apply query in database to get desired result
                var query = (from dsg in dbEnt.DeviceSensorGraphs
                             join d in dbEnt.Devices on dsg.DeviceId equals d.DeviceId
                             join g in dbEnt.Graphs on dsg.GraphId equals g.GraphId
                             join s in dbEnt.Sensors on dsg.SensorId equals s.SensorId
                             where d.CustomerAssetId == MacAddress && dsg.IsDeleted == false
                             select new { dsg.DeviceId, d.Mac, DeviceName = d.Name, d.Latitude, d.Longitude, g.GraphName, s.SensorName, d.CustomerAssetId }).ToList();

                string status = Newtonsoft.Json.JsonConvert.SerializeObject(query);
                InsertEventLog("DeviceSensorGraphOfMAC", EventType.Log, EventColor.yellow, "return data in json ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeviceSensorGraphOfMAC", "");
                return status;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("DeviceSensorGraphOfMAC", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeviceSensorGraphOfMAC", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Gets the device sensor graph on identifier.
        /// </summary>
        /// <param name="DeviceSensorGraphId">The device sensor graph identifier.</param>
        /// <returns>DeviceSensorGraphDto.</returns>
        public DeviceSensorGraphDto GetDeviceSensorGraphOnId(Guid? DeviceSensorGraphId)
        {
            try
            {
                InsertEventLog("GetDeviceSensorGraphOnId", EventType.Log, EventColor.yellow, "get device sensor graph on id=" + DeviceSensorGraphId + " ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphOnId", "");
                // firstly Get data of device sensor graph on id and then place in DTO class

                //return objMapper.GetDeviceSensorGraphDto(dbEnt.DeviceSensorGraphs.FirstOrDefault(x => x.DeviceSensorGraphId == DeviceSensorGraphId));
                var data = objMapper.GetDeviceSensorGraphDto(dbEnt.DeviceSensorGraphs.Find(DeviceSensorGraphId));
                return data;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDeviceSensorGraphOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.GetDeviceSensorGraphOnId", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        
        /// <summary>
        /// Deletes the device sensor graph.
        /// </summary>
        /// <param name="DeviceSensorGraphId">The device sensor graph identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteDeviceSensorGraph(Guid DeviceSensorGraphId)
        {
            try
            {
                InsertEventLog("DeleteDeviceSensorGraph",  EventType.Log, EventColor.yellow, "Enter", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeleteDeviceSensorGraph", "");
                DeviceSensorGraph dsg = dbEnt.DeviceSensorGraphs.FirstOrDefault(x => x.DeviceSensorGraphId == DeviceSensorGraphId); // get device sensor graph for delete
                if (dsg != null)
                {
                    InsertEventLog("DeleteDeviceSensorGraph", EventType.Log, EventColor.yellow, "delete deveice sensor graph on id is not null ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeleteDeviceSensorGraph", "");
                    dsg.IsDeleted = true;
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("DeleteDeviceSensorGraph", EventType.Log, EventColor.yellow, "Successfully deleted deveice sensor graph on id ", "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeleteDeviceSensorGraph", "");
                        return true;
                    }
                }
                return false; // there is no changes in devicesensorgraph 
            }
            catch (Exception ex)
            {
                InsertEventMonitor("DeleteDeviceSensorGraph", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DeviceSensorGraphManager.DeleteDeviceSensorGraph", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }




        public class Counts
        {
            public int MQTT;
            public int HTTP;
            public int LORAWAN;
            public int WIFI;
            public int CELLULAR;
            public int ETHERNET;

        }
    }
}
