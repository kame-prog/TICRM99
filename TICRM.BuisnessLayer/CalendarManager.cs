using System;
using System.Collections.Generic;
using System.Linq;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /****************************************************************************************
    ||  Class [CalendarManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Calender events are defined for each user against activiteis]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class CalendarManager : BaseManager
    {
        /// <summary>
        /// Gets the activity for calendar.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>CalendarEventDTO.</returns>
        public CalendarEventDTO GetActivityForCalendar(Guid? id)
        {
            try
            {
                InsertEventLog("GetActivityForCalendar", EventType.Log, EventColor.yellow, "to get CalendarEventDTO", "TICRM.BuisnessLayer.CalendarManager.GetActivityForCalendar", "");

                List<EventAttendee> attendees = new List<EventAttendee>();

                Activity query = dbEnt.Activities.FirstOrDefault(x => x.ActivityId == id);

                var data = from tu in dbEnt.TeamUsers
                           join u in dbEnt.Users on tu.TeamUserId equals u.UserId
                           where tu.TeamId == query.AssignedTeam
                           select new { u.Email, u.Name };


                foreach (var item in data.CollectionNotNull())
                {
                    EventAttendee eventAttendee = new EventAttendee();
                    eventAttendee.Email = item.Email;
                    eventAttendee.DisplayName = item.Name;
                    attendees.Add(eventAttendee);
                }

                CalendarEventDTO calendarEventDTO;
                calendarEventDTO = objMapper.GetCalendarEventDTO(query);
                EventAttendee eventUser = new EventAttendee();
                eventUser.Email = calendarEventDTO.User.Email;
                eventUser.DisplayName = calendarEventDTO.User.Name;
                attendees.Add(eventUser);
                calendarEventDTO.Attendees = attendees;

                //calendarEventDTO.Attendance = Newtonsoft.Json.JsonConvert.SerializeObject(attendees);

                return calendarEventDTO;

                //return objMapper.GetCalendarEventDTO(dbEnt.Activities.FirstOrDefault(x => x.ActivityId == id));
            }catch(Exception ex)
            {
                InsertEventMonitor("GetActivityForCalendar", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace + " /n " + ex.InnerException, "TICRM.BuisnessLayer.CalendarManager.GetActivityForCalendar", "");

                throw ex;
            }
        }

    }
}
