$(document).ready(function () {

    $("#EventStartDate").datepicker({
        dateFormat: 'yy-mm-dd',//check change
        changeMonth: true,
        changeYear: true
    });
    $("#EventEndDate").datepicker({
        dateFormat: 'yy-mm-dd',//check change
        changeMonth: true,
        changeYear: true
    });

    GetActivitydata()

});

function insertEmailTag(value) {
    if (value) {
        $("#tags").prepend('<span class="emailAdd" tabindex="1">' + value + '</span>');
    }

}

var GetActivitydata = function () {
    var data = "@ViewBag.id";
    if (data == "") {
        return false;
    }
    var obj = { id: data }

    $.ajax({
        type: "GET",
        url: "/Calendar/GetActivityForCalendar",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            $('#EventName').val(response.Summary);
            $('#EventDescription').val(response.Description);
            for (var i = 0; i < response.Attendees.length; i++) {
                insertEmailTag(response.Attendees[i].Email);
            }

        },
        failure: function () {
            alert("Failed!");
        }
    });

}

var EventList = [];
var EventListItem = [];
var FullCalendarList = [];

$('#EventEndTime').change(function () {

    var time = $('#EventEndTime').val();
    // regular expression to match required time format
    re = /^(\d{1,2}):(\d{2})([ap]m)?$/;
    if (time != '') {
        if (regs = time.match(re)) {
            if (regs[3]) {
                // 12-hour value between 1 and 12
                if (regs[1] < 1 || regs[1] > 12) {
                    alert("Invalid value for hours: " + regs[1]);
                    $('#EventEndTime').focus();
                    return false;
                }
            } else {
                // 24-hour value between 0 and 23
                if (regs[1] > 23) {
                    alert("Invalid value for hours: " + regs[1]);
                    $('#EventEndTime').focus();
                    return false;
                }
            }
            // minute value between 0 and 59
            if (regs[2] > 59) {
                alert("Invalid value for minutes: " + regs[2]);
                $('#EventEndTime').focus();
                return false;
            }
        } else {
            alert("Invalid time format: " + time);
            $('#EventEndTime').focus();
            return false;
        }
    }

});

$('#EventStartTime').change(function () {

    var time = $('#EventStartTime').val();
    // regular expression to match required time format
    re = /^(\d{1,2}):(\d{2})([ap]m)?$/;
    if (time != '') {
        if (regs = time.match(re)) {
            if (regs[3]) {
                // 12-hour value between 1 and 12
                if (regs[1] < 1 || regs[1] > 12) {
                    alert("Invalid value for hours: " + regs[1]);
                    $('#EventStartTime').focus();
                    return false;
                }
            } else {
                // 24-hour value between 0 and 23
                if (regs[1] > 23) {
                    alert("Invalid value for hours: " + regs[1]);
                    $('#EventStartTime').focus();
                    return false;
                }
            }
            // minute value between 0 and 59
            if (regs[2] > 59) {
                alert("Invalid value for minutes: " + regs[2]);
                $('#EventStartTime').focus();
                return false;
            }
        } else {
            alert("Invalid time format: " + time);
            $('#EventStartTime').focus();
            return false;
        }
    }

});

function addnewevent() {

    GetInputTag();

    var data = {
        "summary": $('#EventName').val(),
        "kind": "calendar#calendar",
        "description": $('#EventDescription').val(),
        "start": {
            "dateTime": new Date($('#EventStartDate').val() + "T" + $('#EventStartTime').val()),
            "timeZone": "Asia/Karachi"
        },
        "end": {

            "dateTime": new Date($('#EventEndDate').val() + "T" + $('#EventEndTime').val()),
            "timeZone": "Asia/Karachi"
        },
        "attendees": arrayoftag,
        "reminders": {
            "useDefault": false,
            "overrides": [
                {
                    "method": "email", "minutes": 24 * 60
                },
                { "method": "popup", "minutes": 10 }
            ]
        }
    };
    gapi.client.calendar.events.insert({
        'calendarId': 'primary',
        'resource': data
    }).then(function (response) {
        var events = response.result.items;
        window.location.reload();
    });
}

var date = new Date();
var d = date.getDate(),
    m = date.getMonth(),
    y = date.getFullYear();


var LoadFullCalendar = function (data) {
    $('#calendarDIV').html('');
    $('#calendarDIV').html('<div id="calendar"></div>');
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },
        //Random default events
        events: data,

        eventClick: function (e) {
            return window.open(e.url, "gcalevent", "width=700,height=600"), !1
        },
        loading: function (e) { },

        eventRender: function (event, element, view) {

            element.find(".closeon").on('click', function () {
                $('#calendar').fullCalendar('removeEvents', event._id);
            });
        }

    });

}


// Client ID and API key from the Developer Console
var CLIENT_ID = '201742374044-e4rti1noa0dg67ssec127jej0g3tvjou.apps.googleusercontent.com';
var API_KEY = 'AIzaSyCRmNmIqEudOnSj2rMsiHyhWB6zGxZPO54';


// Array of API discovery doc URLs for APIs used by the quickstart
var DISCOVERY_DOCS = ["https://www.googleapis.com/discovery/v1/apis/calendar/v3/rest"];

// Authorization scopes required by the API; multiple scopes can be
// included, separated by spaces.
var SCOPES = "https://www.googleapis.com/auth/calendar";

var authorizeButton = document.getElementById('authorize_button');
var signoutButton = document.getElementById('signout_button');
var addEvent = document.getElementById('add-new-event');


/**
 *  On load, called to load the auth2 library and API client library.
 */
function handleClientLoad() {
    gapi.load('client:auth2', initClient);
}

/**
 *  Initializes the API client library and sets up sign-in state
 *  listeners.
 */
function initClient() {
    gapi.client.init({
        apiKey: API_KEY,
        clientId: CLIENT_ID,
        discoveryDocs: DISCOVERY_DOCS,
        scope: SCOPES
    }).then(function () {
        // Listen for sign-in state changes.
        gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);

        // Handle the initial sign-in state.
        updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());
        authorizeButton.onclick = handleAuthClick;
        signoutButton.onclick = handleSignoutClick;
        addEvent.onclick = addnewevent;

    });
}

/**
 *  Called when the signed in status changes, to update the UI
 *  appropriately. After a sign-in, the API is called.
 */
function updateSigninStatus(isSignedIn) {
    if (isSignedIn) {
        authorizeButton.style.display = 'none';
        signoutButton.style.display = 'block';
        listUpcomingEvents();

    } else {
        authorizeButton.style.display = 'block';
        signoutButton.style.display = 'none';
    }
}

/**
 *  Sign in the user upon button click.
 */
function handleAuthClick(event) {
    gapi.auth2.getAuthInstance().signIn();
}

/**
 *  Sign out the user upon button click.
 */
function handleSignoutClick(event) {
    gapi.auth2.getAuthInstance().signOut();
}

/**
 * Print the summary and start datetime/date of the next ten events in
 * the authorized user's calendar. If no events are found an
 * appropriate message is printed.
 */
function listUpcomingEvents() {
    gapi.client.calendar.events.list({
        'calendarId': 'primary',
        'timeMin': (new Date()).toISOString(),
        'showDeleted': false,
        'singleEvents': true,
        'maxResults': 10,
        'orderBy': 'startTime'
    }).then(function (response) {
        var events = response.result.items;

        if (events.length > 0) {
            FullCalendarList = [];
            for (i = 0; i < events.length; i++) {
                var event = events[i];
                EventList.push(event); // its for future use its has a getting list.

                var data = {
                    title: event.summary,
                    start: event.start.dateTime,
                    end: event.end.dateTime,
                    url: event.htmlLink
                }
                FullCalendarList.push(data);
            }

            LoadFullCalendar(FullCalendarList);

        } else {
            alert('No upcoming events found.');
            LoadFullCalendar(FullCalendarList);

        }
    });
}

    //function AddGoogleEvents() {
    //    updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());
    //    var event = {
    //        "summary": "TechImplement 2018",
    //        "location": "800 Howard St., San Francisco, CA 94103",
    //        "kind": "calendar#calendar",
    //        "description": "A chance to hear more about Google\'s developer products.",
    //        "start": {
    //            "dateTime": new Date(2018, 10, 18, 17, 00, 00),
    //            "timeZone": "Asia/Karachi"
    //        },
    //        "end": {
    //            "dateTime": new Date(2018, 10, 18, 18, 00, 00),
    //            //"dateTime": "2018-11-11T10:00:00-07:00",
    //            "timeZone": "Asia/Karachi"
    //        },
    //        //"recurrence": [
    //        //    "RRULE:FREQ=DAILY;COUNT=2"
    //        //],
    //        "attendees": [
    //            { "email": "aqil@techimplement.com" },
    //            { "email": "pmo@techimplement.com" }
    //        ],
    //        "reminders": {
    //            "useDefault": false,
    //            "overrides": [
    //                {
    //                    "method": "email", "minutes": 24 * 60
    //                },
    //                { "method": "popup", "minutes": 10 }
    //            ]
    //        }
    //    };
    //    gapi.client.calendar.events.insert({
    //        'calendarId': 'primary',
    //        'resource': event
    //    }).then(function (response) {
    //        var events = response.result.items;
    //        appendPre(response);
    //        appendPre(events);
    //        $('#content').html('');
    //        listUpcomingEvents();
    //    });
    //}
    //function appendPre(message) {
    //    var pre = document.getElementById('content');
    //    var textContent = document.createTextNode(message + '\n');
    //    pre.appendChild(textContent);
    //}
