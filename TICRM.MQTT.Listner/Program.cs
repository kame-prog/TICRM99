using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Timers;
//using Microsoft.Xrm.Tooling.Connector;
//using Microsoft.Xrm.Sdk;

namespace TICRM.MQTT.Listner
{
    internal class Program
    {
        public static TechImplementCRMEntities db = new TechImplementCRMEntities();

        private static int cnt = 0;
        private static List<SensorData> lstsensor = new List<SensorData>();
        private static Timer timer = new Timer();
        /// <summary>
        /// Main Execution of the Listner
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            try
            {

                MqttClient client = Connect("broker.hivemq.com");
                Device devicecheck = db.Devices.Where(a => a.IsGateway == true).FirstOrDefault();

                Console.WriteLine("Connectd Successfully to broker.hivemq.com ");
                Subscribe(client);
                Console.WriteLine("Subscribed Successfully to the Topic TiTempSensor");
                Console.WriteLine("Now Waiting for connected devices data");
                timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                timer.Interval = 1000; //number in milisecinds
                timer.Enabled = true;
            }

            catch (Exception ex)
            {
                // will log this exception
                Console.WriteLine("Main :" + ex.Message);
            }
        }
        public static void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            List<User> users = db.Users.Where(x => x.IsAssigned == true && x.StatusId.ToString() == "192f959f-2dfa-4d41-8464-dd482325dc6c" && x.AssignedItem == "Cases").ToList();
            foreach (var item in users)
            {
                DateTime s = Convert.ToDateTime(item.AssignedItemTime);
                if (DateTime.Now.Minute > s.Minute && DateTime.Now.Minute - s.Minute > 0 || DateTime.Now.Minute < s.Minute && s.Minute - DateTime.Now.Minute > 0)
                {
                    User u = db.Users.Where(x => x.IsAssigned == false).FirstOrDefault();
                    TeamUser t = db.TeamUsers.Where(x => x.UserId == item.UserId).FirstOrDefault();
                    //Assign new user to the Case
                    Case cases = db.Cases.Where(x => x.AssignedUser == item.UserId && x.RelatedTo == "Device").First();
                    cases.AssignedUser = u.UserId;
                    //change the previous user
                    item.IsAssigned = false;
                    item.AssignedItem = null;
                    item.AssignedItemId = null;
                    item.AssignedItemTime = null;
                    item.StatusId = Guid.Parse("fb6bab54-3e26-4270-a875-34bc7f72afd8");
                    //change new user
                    u.AssignedItem = "Cases";
                    u.AssignedItemId = cases.CaseId;
                    u.IsAssigned = true;
                    u.AssignedItemTime = DateTime.Now;
                    u.StatusId = Guid.Parse("192f959f-2dfa-4d41-8464-dd482325dc6c");
                    if (db.SaveChanges() > 0)
                    {
                        Console.WriteLine("Olamba");
                        SendEmail(cases, t.Team, item, u);
                    }
                }
            }

        }

        public static void SendEmail(Case c, Team t, User prevUser, User newUser)
        {

            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = db.EmailConfigurations.FirstOrDefault(x => x.Email.ToString() == "swuichcrm@gmail.com").Email; //Sender Email Address  
            string password = "Swuich@123"; //Sender Password  
            string emailToAddress = newUser.Email; //Receiver Email Address  
            string emailToAddresses = prevUser.Email; //Receiver Email Address  
            string emailToAddressTeam = t.Email; //Receiver Email Address  
            string subject = "Case Assignment";
            string Body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            Body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            Body += "</HEAD><BODY><DIV>";
            Body += "<p>A new case has been assigned to you</p><br/>";
            Body += "<h1>"+ c.CaseTitle + "</h1><br/>";
            Body += "<p> Respond to the case in given time else it will be automatucally awarded to someone else</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            Body += "</DIV></BODY></HTML>";

            string BodyPrev = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            BodyPrev += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            BodyPrev += "</HEAD><BODY><DIV>";
            BodyPrev += "<p>The Case has been removed</p><br/>";
            BodyPrev += "<h1>" + c.CaseTitle + "</h1><br/>";
            BodyPrev += "<p> Respond to the cases in given time else it will be automatucally awarded to someone else</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            BodyPrev += "</DIV></BODY></HTML>";

            string BodyTeam = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            BodyTeam += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            BodyTeam += "</HEAD><BODY><DIV>";
            BodyTeam += "<p>The Case has been removed from " + prevUser.Email + " and has been awarded to " + newUser.Email + "</p><br/>";
            BodyTeam += "<h1>" + c.CaseTitle + "</h1><br/>";
            BodyTeam += "<p> Respond to the cases in given time else it will be automatucally awarded to someone else</p><footer class=\"footer\"><img src=\"D:\\TI_Projects\\Dev\\TFS Project\\TICRM New\\swuichNew\\Project\\TICRM\\TICRM\\Content\\Images\\TI_Logo.png\" style=\"width:100px;\"/></footer>";
            BodyTeam += "</DIV></BODY></HTML>";

            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                    Console.WriteLine("Email has been sent Successfully to " + emailToAddress);
                }
            }

            using (MailMessage mails = new MailMessage())
            {
                mails.From = new MailAddress(emailFromAddress);
                mails.To.Add(emailToAddresses);
                mails.Subject = subject;
                mails.Body = BodyPrev;
                mails.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mails);
                    Console.WriteLine("Email has been sent Successfully to " + emailToAddresses);
                }
            }

            using (MailMessage mailss = new MailMessage())
            {
                mailss.From = new MailAddress(emailFromAddress);
                mailss.To.Add(emailToAddressTeam);
                mailss.Subject = subject;
                mailss.Body = BodyTeam;
                mailss.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mailss);
                    Console.WriteLine("Email has been sent Successfully to " + emailToAddressTeam);
                }
            }
        }

        /// <summary>
        /// Connect with Broker
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static MqttClient Connect(string host)
        {
            MqttClient client = new MqttClient(host);
            try
            {
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connect :" + ex.Message);
            }
            return client;
        }


        /// <summary>
        /// Subscribe messages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="topic"></param>
        public static void Subscribe(MqttClient client)
        {
            try
            {
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                //List<Device> d = db.Devices.Where(a => a.IsGateway == true &&  a.Mac != null).ToList();
                //Console.WriteLine(d.Count());
                //List<string> sub = new List<string>();

                //foreach (var item in d)
                //{
                //    sub.Add(item.Mac);
                //}
                //// register to message received
                //foreach(var item in sub)
                //{
                //    client.Subscribe(new string[] { item }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                //    Console.WriteLine("Subscribe :" + item);

                //}
                client.Subscribe(new string[] { "GatewayServer" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Subscribe :" + ex.Message);
            }
        }

        /// <summary>
        /// Message received 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                cnt++;
                if (e.Topic == "Olamba")
                {
                    Console.WriteLine("Olamba");
                }
                if (e.Topic == "swuich")
                {
                    Console.WriteLine("swuich");
                }
                if (e.Topic == "Olamba")
                {
                    Console.WriteLine("Olamba");
                }
                Console.WriteLine(e.Topic + " : " + Encoding.UTF8.GetString(e.Message));
                string[] msg = Encoding.UTF8.GetString(e.Message).Split(',');
                int temp = Convert.ToInt32(msg[0]);
                string mac = msg[1];
                string loc = msg[2] + "," + msg[3];
                Device d = db.Devices.FirstOrDefault(a => a.Mac == mac);    //Get the device against the mac in message
                DeviceSensorGraph dsg = db.DeviceSensorGraphs.FirstOrDefault(a => a.DeviceId == d.DeviceId); // get the sensor graph for that device
                String duration = dsg.Data;
                String[] s = duration.Split(' ');
                int num = Convert.ToInt32(s[0]);
                string day = s[1];
                DateTime lastmess = Convert.ToDateTime(d.LastMessage);

                if (day == "Hour" || day == "Hours")
                {
                    if (DateTime.Now.Hour - lastmess.Hour > num)
                    {
                        Disconnection dis = new Disconnection();
                        dis.AccountId = d.AccountId;
                        dis.DeviceId = d.DeviceId;
                        dis.Date = Convert.ToDateTime(DateTime.Now);
                        db.Disconnections.Add(dis);
                        db.SaveChanges();
                    }
                }
                else if (day == "Days" || day == "Day")
                {
                    if (DateTime.Now.Day - lastmess.Day > num)
                    {
                        Disconnection dis = new Disconnection();
                        dis.AccountId = d.AccountId;
                        dis.DeviceId = d.DeviceId;
                        dis.Date = Convert.ToDateTime(DateTime.Now);
                        db.Disconnections.Add(dis);
                        db.SaveChanges();
                    }
                }
                d.LastMessage = DateTime.Now;
                //IQueryable<DeviceSensorGraph> dsg = db.DeviceSensorGraphs.Where(a => a.DeviceId == d.DeviceId);
                //device.FirstOrDefault().LastMessage = DateTime.Now;
                db.SaveChanges();
                Tracking tracking = new Tracking();
                tracking.Id = Guid.NewGuid();
                tracking.Devicemac = msg[1];
                tracking.Devicelocation = msg[2] + "," + msg[3];
                tracking.Datatime = DateTime.Now;
                db.Trackings.Add(tracking);
                db.SaveChanges();

                SaveDeviceData(temp.ToString(), mac);

                //workflow reports and dynamics code

                //List<WorkFlow> workFlowsCloud = db.WorkFlows.Where(x => x.TriggerCondition == "Temperature" && x.DeviceMac == mac && x.Cloud == "DYNAMICS CRM").ToList();
                //foreach (var item in workFlowsCloud)
                //{
                //    string[] threshold = item.Threshold.Split(',');
                //    int less = int.Parse(threshold[0]);
                //    int greater = int.Parse(threshold[1]);
                //    if (temp < less || temp > greater)
                //    {
                //        try
                //        {
                //            var connectionString = "Url=https://orgaa7af2c2.crm4.dynamics.com; Domain=techimplement; Username=akhtar@techimplement.com;Password=Tech@1234;AuthType=Office365";
                //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //TLS1.2 is required for v9 and above
                //            CrmServiceClient con = new CrmServiceClient(connectionString);
                //            var service = (IOrganizationService)con.OrganizationWebProxyClient != null ?
                //                                (IOrganizationService)con.OrganizationWebProxyClient : (IOrganizationService)con.OrganizationServiceProxy;
                //            //Accou acc = db.Accounts.Where(x => x.AccountId.ToString() == item.AccountId);
                //            //acc.
                //            List<Account> acc = db.Accounts.Where(x => x.AccountId.ToString() == item.AccountId).ToList();
                //            foreach(var items in acc)
                //            {
                //                try
                //                {
                //                    accName = items.Name;
                //                    Entity workorder = new Entity("new_swuichentity");
                //                    workorder.LogicalName = "new_swuichentity";
                //                    workorder["new_account"] = accName;
                //                    workorder["new_action"] = "Create Workorder";
                //                    workorder["new_sensorreading"] = temp.ToString();
                //                    workorder["new_device"] = item.DeviceName.ToString();
                //                    workorder["new_name"] = item.Name;
                //                    workorder["new_accountindustry"] = items.Industry.ToString();
                //                    workorder["new_accountlocation"] = items.Address.ToString();
                //                    workorder["new_devicelocation"] = loc.ToString();
                //                    workorder["new_thresholdvalue"] = item.Threshold.ToString();

                //                    Guid leadId = service.Create(workorder);
                //                    Console.WriteLine("New contact id: {0}.", leadId.ToString());
                //                }
                //                catch(Exception ex)
                //                {
                //                    Console.WriteLine(ex.Message);
                //                }

                //            }

                //            //Entity lead = new Entity("lead");
                //            //lead["jobtitle"] = "Device";
                //            //lead["firstname"] = "MXCHIP";
                //            //lead["lastname"] = "Temperature";
                //            //Guid leadId = service.Create(lead);
                //            //Console.WriteLine("New contact id: {0}.", leadId.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }


                //}

                List<WorkFlow> workFlows = db.WorkFlows.Where(x => x.TriggerCondition == "Temperature" && x.DeviceMac == mac).ToList();
                foreach (var item in workFlows)
                {
                    string[] threshold = item.Threshold.Split(',');
                    int less = int.Parse(threshold[0]);
                    int greater = int.Parse(threshold[1]);
                    if (temp < less || temp > greater)
                    {
                        WorkFlowReport wfr = new WorkFlowReport();
                        wfr.WorkFlowReportId = Guid.NewGuid();
                        wfr.WorkFlowId = item.WorkFlowId;
                        wfr.DeviceName = item.DeviceName;
                        wfr.Action = item.Action;
                        wfr.AccountId = item.AccountId;
                        wfr.CreatedDate = DateTime.Now;
                        wfr.WorkFlowStatus = "Success";

                        db.WorkFlowReports.Add(wfr);
                        if (db.SaveChanges() > 0)
                        {

                            Console.WriteLine("Stored");

                        }
                        if (item.Action == "Case")
                        {
                            string DeviceName = db.Devices.Where(x => x.DeviceId == item.RelatedToId).FirstOrDefault().Name.ToString();
                            string DeviceMac = db.Devices.Where(x => x.DeviceId == item.RelatedToId).FirstOrDefault().Mac.ToString();

                            Case cases = new Case();
                            Guid id = Guid.NewGuid();
                            cases.CaseId = id;
                            cases.CaseTitle = item.Name;
                            cases.RelatedToId = (Guid)item.RelatedToId;
                            cases.Reading = temp.ToString();
                            cases.CaseTypeId = Guid.Parse("91a2b393-cbd9-4d22-8a5f-ebf5a62c9b65");
                            cases.ContactId = 6;
                            cases.RelatedTo = "Device";
                            cases.Description = "The device " + DeviceName + " with Mac Address " + DeviceMac + " is not working expectedly";
                            //cases.SensorId = Guid.Parse(item.TriggerCondition);
                            //cases.CreatedBy = item.CreatedBy;
                            cases.CreatedDate = DateTime.Now;

                            db.Cases.Add(cases);
                            if (db.SaveChanges() > 0)
                            {
                                string smtpAddress = "smtp.gmail.com";
                                int portNumber = 587;
                                bool enableSSL = true;
                                string emailFromAddress = "imakhtrkhan@gmail.com"; //Sender Email Address  
                                string password = "Olamba@123"; //Sender Password  
                                string emailToAddress = "akhtar2014@namal.edu.pk"; //Receiver Email Address  
                                string subject = "WorkFlow Report";
                                string body = "The Device sensor " + item.TriggerCondition + " for " + item.DeviceName + " is not working as expected and case has been created for it";
                                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                using (MailMessage mail = new MailMessage())
                                {
                                    mail.From = new MailAddress(emailFromAddress);
                                    mail.To.Add(emailToAddress);
                                    mail.Subject = subject;
                                    mail.Body = body;
                                    mail.IsBodyHtml = true;
                                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                                    {
                                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                                        smtp.EnableSsl = enableSSL;
                                        smtp.Send(mail);
                                        Console.WriteLine("Email has been sent Successfully to " + emailToAddress);
                                    }
                                }
                            }

                        }
                    }
                }

                //if (temp > 50)
                //{
                //    lstsensor.Add(GetDeviceData(msg[1], msg[0]));
                //}
                //if (cnt >= 15)
                //{
                //    cnt = 0;
                //    InsertDeviceData(lstsensor);
                //    lstsensor.Clear();
                //    lstsensor = new List<SensorData>();
                //    Console.Clear();
                //    Console.WriteLine("Clear Screen \n  Sensor Data Bulk inserted into DataBase Successfully!");

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Publish Received: " + ex.Message);
            }

        }


        /// <summary>
        /// Saves the device data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <param name="Mac">The mac.</param>
        public static void SaveDeviceData(String Data, String Mac)
        {
            Device d = db.Devices.FirstOrDefault(a => a.Mac == Mac);    //Get the device against the mac in message
            List<string> data = new List<string>();
            if (data.Count() == 5)
            {
                data.RemoveAt(0);
                data.Add(Data);
            }
            else
                data.Add(Data);
            d.Data = data.ToString();
        }

        /// <summary>
        /// Publis Send Value to the Client - Devices
        /// </summary>
        /// <param name="client"></param>
        /// <param name="title"></param>
        /// <param name="value"></param>
        public static void Publish(MqttClient client, string title, string value)
        {
            try
            {
                string strValue = Convert.ToString(value);
                client.Publish(title, Encoding.UTF8.GetBytes(strValue));
            }
            catch (Exception ex)
            {

                Console.WriteLine("Publish :" + ex.Message);
            }

        }

        /// <summary>
        /// Add sensor data to the device 
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="data"></param>
        public static SensorData GetDeviceData(string mac, string data)
        {
            try
            {
                IQueryable<Device> device = db.Devices.Where(a => a.Mac == mac);
                if (device != null)
                {
                    IQueryable<DeviceSensor> sensor = db.DeviceSensors.Where(a => a.DeviceId == device.FirstOrDefault().DeviceId);
                    if (device.FirstOrDefault().ServiceDate != null)
                    {
                        DateTime deviceserviceDate = Convert.ToDateTime(device.FirstOrDefault().ServiceDate);
                        int result = DateTime.Compare(deviceserviceDate.Date, DateTime.Now.Date);
                        if (device.FirstOrDefault().ServiceDateFlag == false && result == 0)
                        {
                            Device createWorkOrder = device.FirstOrDefault();
                            string MacAddress = device.FirstOrDefault().Mac;
                            Guid? AssignedUser = device.FirstOrDefault().AssignedUser;
                            Guid? AssignedTeam = device.FirstOrDefault().AssignedTeam;

                            bool status = GenerateWorkOrder(MacAddress, AssignedUser, AssignedTeam);
                            status = GenerateOpportunity(MacAddress, AssignedUser, AssignedTeam);
                            if (status == true)
                            {
                                device.FirstOrDefault().ServiceDateFlag = true;
                            }

                        }
                    }

                    if (sensor != null)
                    {
                        SensorData sensorData = new SensorData();

                        // sensorData.DeviceSensor = sensor;
                        sensorData.DeviceSensorId = sensor.FirstOrDefault().DeviceSensorId;
                        sensorData.SensorValue = Double.Parse(data);
                        sensorData.RecordDate = DateTime.Now;
                        using (System.IO.StreamWriter file =
          new System.IO.StreamWriter(@"C:\Users\Muhammad.zaman\Desktop\Blink\New.txt", true))
                        {
                            file.WriteLine(data);
                        }
                        return sensorData;

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Insert Device Data :" + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Inserts the device data.
        /// </summary>
        /// <param name="lst">The LST.</param>
        public static void InsertDeviceData(List<SensorData> lst)
        {
            try
            {
                db.SensorDatas.AddRange(lst);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Insert Device Data :" + ex.Message);
            }
        }

        /// <summary>
        /// Generates the work order.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <param name="AssignedUser">The assigned user.</param>
        /// <param name="AssignedTeam">The assigned team.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GenerateWorkOrder(string MacAddress, Guid? AssignedUser, Guid? AssignedTeam)
        {
            WorkOrder wo = new WorkOrder();
            try
            {
                Device devicecheck = db.Devices.Where(a => a.Mac == MacAddress).FirstOrDefault();
                if (devicecheck.ServiceDateFlag == false)
                {
                    wo.WorkOrderId = Guid.NewGuid();
                    wo.Title = MacAddress;
                    //Random random = new Random();
                    //int month = random.Next(1, 13);
                    wo.NTE = Convert.ToDecimal(1542.23);
                    wo.WorkOrderStageId = new Guid("fd9313c9-65c9-44b7-b34f-f67fd7a8f90c"); // to insert new WorkOrder stage
                    wo.Description = "Device Need To be Serviced.";
                    wo.IsDeleted = false;
                    wo.StatusId = new Guid("192f959f-2dfa-4d41-8464-dd482325dc6c"); // insert a active entry in database that have a value "192f959f-2dfa-4d41-8464-dd482325dc6c"
                    wo.AssignedUser = AssignedUser;
                    wo.AssignedTeam = AssignedTeam;
                    wo.CreatedDate = DateTime.Now;
                    db.WorkOrders.Add(wo);
                    if (db.SaveChanges() > 0)
                    {
                        //devicecheck.ServiceDateFlag = true;
                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// Generates the opportunity.
        /// </summary>
        /// <param name="MacAddress">The mac address.</param>
        /// <param name="AssignedUser">The assigned user.</param>
        /// <param name="AssignedTeam">The assigned team.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GenerateOpportunity(string MacAddress, Guid? AssignedUser, Guid? AssignedTeam)
        {
            Opportunity opp = new Opportunity();
            try
            {
                Device devicecheck = db.Devices.Where(a => a.Mac == MacAddress).FirstOrDefault();
                if (devicecheck.ServiceDateFlag == false)
                {
                    opp.OpportunityId = Guid.NewGuid();
                    opp.Title = MacAddress;
                    opp.Description = "Device Need To be Serviced.";
                    opp.IsDeleted = false;
                    opp.StatusId = new Guid("192f959f-2dfa-4d41-8464-dd482325dc6c"); // insert a active entry in database that have a value "192f959f-2dfa-4d41-8464-dd482325dc6c"
                    opp.AssignedUser = AssignedUser;
                    opp.AssignedTeam = AssignedTeam;
                    opp.CreatedDate = DateTime.Now;
                    db.Opportunities.Add(opp);
                    if (db.SaveChanges() > 0)
                    {
                        devicecheck.ServiceDateFlag = true;
                        if (db.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

    }
}
