using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Calendar Controller************
  Class [CalendarController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with Calendar like, 
  ||             navigating to the pages, getting associated modules for specific Calendar]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ********************************************/
    public class CalendarController : BaseController
    {

        private CalendarManager cm = new CalendarManager();

        /// <summary>
        /// Index viw for specified calender.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index(Guid? id)
        {
            try
            {
                ViewBag.id = id;
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the activity for calendar.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetActivityForCalendar(Guid? id)
        {
            try
            {
                CalendarEventDTO data = new CalendarEventDTO();
                if (id != null)
                {
                    data = cm.GetActivityForCalendar(id);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Main(string[] args)
        {
            try
            {
                UserCredential credential;

                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Calendar API service.
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define parameters of request.
                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                Events events = request.Execute();
                Console.WriteLine("Upcoming events:");
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        string when = eventItem.Start.DateTime.ToString();
                        if (String.IsNullOrEmpty(when))
                        {
                            when = eventItem.Start.Date;
                        }
                        Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                    }
                }
                else
                {
                    Console.WriteLine("No upcoming events found.");
                }

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

    }
}