var globalDeviceID;
$(document).ready(function () {

    
    //*********************************************//
    //             Sales Tab jQuery                //
    //*********************************************//
   
    $('#step2-tab').on("shown.bs.tab", function () {

        var url_s = window.location.href;
        var params_s = url_s.split("/");
        var id_s = params_s[5];

        //Lead Won Donut Graph jQuery
        $.ajax({
            url: '/Accounts/GetWonLeads/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var Won = Math.round(JSON.parse(data));
                var options = {
                    chart: {
                        height: 170,
                        type: 'radialBar',
                        toolbar: {
                            show: false
                        }
                    },
                    plotOptions: {
                        radialBar: {
                            hollow: {
                                margin: 0,
                                size: "75%",
                                background: 'transparent',
                            },

                            dataLabels: {
                                name: {
                                    offsetY: -5,
                                    fontSize: "13px",
                                },
                                value: {
                                    offsetY: 5,
                                    fontSize: "18px",
                                    show: true
                                }
                            }
                        }
                    },
                    colors: ['#2c74de'],
                    series: [Won],
                    stroke: {
                        lineCap: 'round'
                    },
                    labels: ['Leads Won'],

                }

                var chart = new ApexCharts(
                    document.querySelector("#LeadsWonDonut"),
                    options
                );
                chart.render();
            }
        });

        //Leads from source count

        $.ajax({
            url: '/Accounts/LeadCountFromSource',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $("#Compaign").text(data[0].LeadSourceCount);
                $("#Confrence").text(data[1].LeadSourceCount);
                $("#Email").text(data[2].LeadSourceCount);
                $("#WebSite").text(data[3].LeadSourceCount);
            }
        });

        //Oppertunity Map jQuery
        $.ajax({
            url: '/Accounts/GetOpportunityLoc/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                OpportuntiyMap(data);
            }
        });

        //Opportunity Sale MonthWise jQuery
        $.ajax({

            url: '/Accounts/GetOppSaleMonthWise/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var count = [];
                var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]

                for (var i = 0; i < months.length; i++) {
                    for (var j = 0; j < data.length; j++) {
                        if (months[i] == data[j].MonthName) {
                            count[i] = data[j].Amount;
                            break;
                        }
                        else {
                            count[i] = 0;
                        }
                    }
                }
                OppSalesMonthWise(count);
            }
        });

        //WorkOrder jQuery:
        $.ajax({
            url: '/Accounts/WorkOrderDetail/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var activityInfoText = '';
                for (var i = 0; i < data.length; i++) {
                    if (data[i].WorkOrderStatus == 'Active') {
                        activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-infinity bg-soft-primary'></i></div ><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75'><span>" + data[i].Title + "</span> | <span>" + data[i].AssignedUser + "</span> <br><span>" + data[i].Description + "</span></p><div class='spinner-grow text-white' role='status'><span class='text-success'>" + data[i].WorkOrderStatus + "</span></div></div></div></div>"
                    }
                    else {
                        activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-infinity bg-soft-primary'></i></div ><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75'><span>" + data[i].Title + "</span> | <span>" + data[i].NTE + "</span> <br><span>" + data[i].Description + "</span></p><div class='spinner-grow text-white' role='status'><span class='text-danger'>" + data[i].WorkOrderStatus + "</span></div></div></div></div>"

                    }
                }
                $('.AccWorkorder').html(activityInfoText);
            },
            error: function (err) {
                console.log(err);
            }
        });
        //Opportuntiy Details
        $.ajax({
            url: '/Accounts/GetOpportunity/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                OpportunityDetails(data);
            }
        });


    });

    //*********************************************//
    //             Service Tab jQuery              //
    //*********************************************//
    $('#step3-tab').on("shown.bs.tab", function () {
        var url_s = window.location.href;
        var params_s = url_s.split("/");
        var id_s = params_s[5];


        //Cases Location Show in Map jQuery
        $.ajax({
            url: '/Accounts/GetCasesLoc/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                CasesLocation(data);
            }
        });

        //Activities Against account jQuery
        $.ajax({
            url: '/Accounts/GetAccActivity/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var activityInfoText = '';
                for (var i = 0; i < data.length; i++) {
                    activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-user-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-100' >Activity<span> " + data[i].Name + " </span>having type<span> " + data[i].Type + " </span> Was created by <span > " + data[i].AssignedUser + " </span> on <span id = 'createdDate' > " + data[i].createdDate + " </span></p></div></div></div>";
                }
                $('.Accactivity').html(activityInfoText);
            },
            error: function (err) {
                console.log(err);
            }
        })

        //Contact Table jQuery
        $.ajax({
            url: '/Accounts/GetContact/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                ContactTblFunc(data);
            }
        });

        $('#step3-tab').off(); // ---->to remove the binded event after the initial rendering
    });

    //*********************************************//
    //             Device Tab jQuery               //
    //*********************************************//
    $('#step4-tab').on("shown.bs.tab", function () {
        var url_s = window.location.href;
        var params_s = url_s.split("/");
        var id_s = params_s[5];




        /****  Connected Devices with Network Count jQuery  ****/
        $.ajax({
            url: '/Accounts/GetNetworks/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                NetworkFunction(data.lst_DeviceCount, data.lst_DevicePercentage);
            }
        });

        /****  Connected Devices with Protocol Count jQuery  ****/
        $.ajax({
            url: '/Accounts/GetChannel/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                GetProtocols(data.lstPercentage, data.lstLabels);
            }
        });

        /****  Device Heart Beat Time jQuery  ****/
        $.ajax({
            url: '/Accounts/GetDeviceHeartBeatTime/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var DisConTime = '';
                for (var i = 0; i < data.length; i++) {

                    DisConTime += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75 mt-2'><b><span>" + data[i].Name + "</span></b><br></p><span class='text-primary'>" + (data[i].Time == null ? "Not available" : data[i].Time) + "</span></div></div></div>"

                }
                $('.DeviceDisConnctTime').html(DisConTime);
            },
            error: function (err) {
                console.log(err);
            }
        })

        $('#step4-tab').off(); // to remove the binded event after the initial rendering

    });

    //*********************************************//
    //             Work-Flows Tab jQuery           //
    //*********************************************//
    $('#step5-tab').on("shown.bs.tab", function () {

        var url_s = window.location.href;
        var params_s = url_s.split("/");
        var id_s = params_s[5];

        /****  Work Flows Table jQuery  ****/
        $.ajax({
            url: '/Accounts/GetWorkFlow/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {

                WorkFlowsTbl(data);
            }
        });

        /****  Work Flow Report Table jQuery  ****/
        $.ajax({
            url: '/Accounts/GetWorkFlowReport/' + id_s,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                WorkFlowRepoTbl(data);
            }
        });

        /****  Work Flow Count jQuery  ****/
        $.ajax({
            url: '/Accounts/GetWorkFlowCount/' + id_s,
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#WorkFlowcount").text(data);
            },
            error: function (err) {
                console.log(err);
            }
        });

        /****  Work Flow Status Progress Bar jQuery  ****/
        $.ajax({
            url: '/Accounts/GetWorkFlowStatus/' + id_s,
            method: "POST",
            success: function (data) {
                for (var x = 0; x < data.length; x++) {
                    var succ = data[x].Success;
                    var succ = parseInt(succ);
                    var fails = data[x].Failure;
                    var fails = parseInt(fails);
                    var Pend = data[x].Pening
                    var Pend = parseInt(Pend);
                    var sum = succ + fails + Pend
                    var succper = (succ / sum) * 100
                    succper = succper || 0;
                    var failper = (fails / sum) * 100
                    failper = failper || 0;
                    var Pendper = (Pend / sum) * 100
                    Pendper = Pendper || 0;
                    $("#success").text(Math.round(succper) + "%").css("width", succper + "%");
                    $("#fail").text(Math.round(failper) + "%").css("width", failper + "%");
                    $("#pending").text(Math.round(Pendper) + "%").css("width", Pendper + "%");
                }

            },
            error: function (err) {
                console.log(err);
            }
        });
    });


    /****  Work Flows Table jQuery  ****/
    //$.ajax({
    //    url: '/Accounts/WFCreate/',
    //    type: 'GET',
    //    data: {},
    //    success: function (data) {
    //        console.log("WFCreate")
    //    }
    //});


    //**  Bulb ON/OFF Intensity control bar Hide/Show  when page Reload or load first time  **//

    $('#BulbOffButton').hide();
    $('#BulbOnButton').hide();
    $('#BulbOnImage').hide();
    $('#BulbOffImage').hide();
    $('#sliderHideShow').hide();
    $('#Notype').hide();
    $('#LocationMap').hide();
    $('#TempratureGraph').hide();
    $('#OnOffGraph').hide();
    $('#OnOffspinner').hide();
    $('#FirstLoad').show();

    startPolling();

});

//$(".DeviceGraphbtn").on("click", function (e) {
//    e.preventDefault();
//    $('#DeviceGraphModal').modal('show');
//})
$("#Firmwarebtn").on("click", function (e) {
    e.preventDefault();
    $('#FirmWarModal').modal('show');
})
$("#Workflowbtn").on("click", function (e) {
    e.preventDefault();
    $('#WorkFlowModal').modal('show');
})
$("#Configurationbtn").on("click", function (e) {
    e.preventDefault();
    $('#ConfigModal').modal('show');
})

//**  Bulb ON and OFF Click Function  Start**//
function light(sw) {
    var IsDeviceOn = "OFF";

    if (sw) {
        //debugger
        //$.ajax({
        //    url: '@Url.Action("SendMessageToMqtt", "Device")',
        //    method: "POST",
        //    data: IsDeviceOn,
        //    success: function (response) {
        //        console.log(response);
        //    },
        //    error: function (xhr, status, error) {
        //        console.log(error);
        //    }
        //});
        $('#sliderHideShow').show();
        $('#BulbOffButton').show();
        $('#BulbOnButton').hide();
        $('#BulbOffImage').hide();
        $('#BulbOnImage').show();
        $('#PowerONmsg').show();
        $('#PowerOFFmsg').hide();


        var IsDeviceOn = "ON";
    } else {


        $('#sliderHideShow').hide();
        $('#BulbOffButton').hide();
        $('#BulbOnButton').show();
        $('#BulbOnImage').hide();
        $('#BulbOffImage').show();
        $('#PowerONmsg').hide();
        $('#PowerOFFmsg').show();
        var IsDeviceOn = "OFF";
    }
    //--> Ajax call for send message to on MQTT server for ON and OFF device
    var deviceID = $('#device_id_Graph').val();
    var obj = { deviceStatus: IsDeviceOn, deviceID: deviceID };
    $.ajax({
        type: "post",
        url: "/devices/SendMessageToMqtt",
        data: obj,
        success: function (response) {
            if (response == "error") {
                ("failed to update on mqtt.");
            }
            else {
                alert("failed to update on mqtt.");
            }
        },
        failure: function () {
            alert("failed!");
        }
    });

};
//**  Bulb ON and OFF Click Function  END**//

//**  Change Graph according to device Type Function  Start**//

function GetDeviceType(id, Device_Name) {
    //Get Account id from the URL 
    var url_s = window.location.href;
    var params_s = url_s.split("/");
    var Account_id = params_s[5];

   
    $('#FirstLoad').hide();
    $('#OnOffGraph').hide();
    $('#TempratureGraph').hide();
    $('#Notype').hide();
    $('#LocationMap').hide();
    $('#OnOffspinner').show();
    $('#device_id_Graph').val(id);
    $('#firmware_Device').val(id);
    $('#workFlow_DeviceName').val(Device_Name);
    $('#workFlow_AccountId').val(Account_id);
    $('#deviceSensorGraph_DeviceId').val(id);  //Device id for configuration popup Modal
    $("#Firmwarebtn").attr("disabled", false);
    $("#Workflowbtn").attr("disabled", false);
    $("#Configurationbtn").attr("disabled", false);

    $.ajax({
        url: "/Accounts/GetDeviceType/" + id,
        type: "POST",
        success: function (data) {

            if (data == "Temperature") {
                $('#TempGraphModal').modal('show');

                $("#GraphName").html("Temperature Graph");
                $('#OnOffspinner').hide();
                $('#FirstLoad').hide();
                $('#OnOffGraph').hide();
                $('#Notype').hide();
                $('#LocationMap').hide();
                $('#LiquidLevel').hide();
                $('#TempratureGraph').show();
            }
            else if (data == "Tracking")
            {
               
                $('#TrackingDevicesModal').modal('show');
                
                
                var obj = { deviceID: id };
                debugger;
                globalDeviceID = id;
                $('#TrackingDevicesModal').on('shown.bs.modal', function () {
                    //$.ajax({
                    //    url: '/Devices/GettrackingDevicesmap',
                    //    type: 'GET',
                    //    dataType: 'json',
                    //    success: function (data) {
                            
                    //        $('#device_maptrack').empty();

                    //        console.log('map results by me is ' + data);
                    //        initMapnew(data);
                    //    }
                    //});
                    $.ajax({
                        url: '/Devices/GettrackingDevicesmap',
                        data: obj,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            LiveDevicesMap2(data);
                            //   initMap(data);
                        }
                    });
                });
               
            }
            else if (data == "Light") {

                $('#DeviceGraphModal').modal('show');

                /*** first load fetch device data device is ON or NOT ***/
                /* var deviceID = $('#device_iid').val();*/
        /*        debugger;*/
                var obj = { deviceID: id };
                $.ajax({
                    type: "post",
                    url: "/devices/ReceiveMessageToMqtt",
                    data: obj,
                    success: function (response) {
                        debugger;
                        if (response == "error") {
                            ("failed to update on mqtt.");
                        }
                        else {
                            alert("failed to update on mqtt.");
                        }
                    },
                    failure: function () {
                        alert("failed!");
                    }
                });

                $("#GraphName").html("Device Light ON/OFF");
                $('#OnOffspinner').hide();
                $('#Notype').hide();
                $('#FirstLoad').hide();
                $('#TempratureGraph').hide();
                $('#LocationMap').hide();
                $('#LiquidLevel').hide();
                $('#BulbOnImage').hide();
                $('#sliderHideShow').hide();
                $('#BulbOffButton').hide();
                $('#OnOffGraph').show();
                $('#BulbOffImage').show();
                $('#BulbOnButton').show();

                //Test
                $('.BulbOffButton').hide();
                $('.BulbOnImage').hide();
                $('.BulbOffImage').show();
                $('.BulbOnButton').show();

                //var latestMessage = $('#ONandOFFmessage').val();
                //if (latestMessage == 'true') {
                //    // Show the "Bulb Off" button and image
                //    $('#BulbOnButton').show();
                //    $('#BulbOnImage').show();

                //    $('#BulbOffButton').hide();
                //    $('#BulbOffImage').hide();
                //} else {
                //    // Show the "Bulb On" button and image
                //    $('#BulbOffButton').show();
                //    $('#BulbOffImage').show();

                //    $('#BulbOnButton').hide();
                //    $('#BulbOnImage').hide();
                //}



                //Date and time function
                function fn60sec() {
                    var dt = new Date();
                    var ampm = dt.getHours() >= 12 ? ' PM' : ' AM';
                    hours = dt.getHours() % 12;
                    hours = hours ? hours : 12;
                    var time = hours + ":" + dt.getMinutes() + ":" + dt.getSeconds() + ":" + ampm;
                    var strDate = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                    $("#Date").html(strDate);
                    $("#Time").html(time);
                    var message = dt.getHours() < 12 ? 'Good Morning' : dt.getHours() < 17 ? 'Good Afternoon' : dt.getHours() < 20 ? 'Good Evening' : 'Good Night';
                    $("#Salute").html(message);
                }
                fn60sec();
                setInterval(fn60sec, 1 * 1000);


            }
            else if (data == "Location") {
                $("#GraphName").html("Tracker Location");
                $('#OnOffspinner').hide();
                $('#FirstLoad').hide();
                $('#OnOffGraph').hide();
                $('#Notype').hide();
                $('#TempratureGraph').hide();
                $('#LiquidLevel').hide();
                $('#LocationMap').show();
                DeviceTrackerMap();
            }
            //else if (data == "Liquid Level") {
            //    $("#GraphName").html("Liquid Level");
            //    $('#OnOffspinner').hide();
            //    $('#FirstLoad').hide();
            //    $('#OnOffGraph').hide();
            //    $('#Notype').hide();
            //    $('#TempratureGraph').hide();
            //    $('#LocationMap').hide();
            //    $('#LiquidLevel').show();
            //} 
            else {
                $('#NoGraphModal').modal('show')

                $("#GraphName").html("Device do not have any type");
                $('#OnOffspinner').hide();
                $('#FirstLoad').hide();
                $('#OnOffGraph').hide();
                $('#Notype').show();
                $('#TempratureGraph').hide();
                $('#LocationMap').hide();
                $('#LiquidLevel').hide();
            }
        }
    });
};
//**  Change Graph according to device Type Function  END**//


//*********************************************//
//           Sales Tab Functions               //
//*********************************************//
//function updateMap() {
//    $.ajax({
//        url: '/Devices/GettrackingDevicesmap', // Make AJAX call to get the latest coordinates
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {
//            LiveDevicesMap2(data); // Pass the data to your map function to update the map
//        },
//        error: function (err) {
//            console.error("Error fetching device coordinates:", err);
//        }
//    });
//}




// Function to fetch location data from the controller
function fetchLocationData() {
    var obj = { deviceID: globalDeviceID };
    if (globalDeviceID != null) {
        $.ajax({

            url: '/Devices/GetsingleDevice',// Route to the controller action
            type: 'GET',
            dataType: 'json',
            data: obj,// Expecting a JSON response
            success: function (data) {
                // Call the function to update the map with the fetched data
                LiveDevicesMap2(data);
            },
            error: function () {
                console.error("Error fetching location data.");
            }
        });
    }
    
}

// Function to start polling
function startPolling() {
    debugger;
    // Call fetchLocationData immediately to initialize the map
    fetchLocationData();

    // Set an interval to call fetchLocationData every 30 seconds
    setInterval(fetchLocationData, 5000);
}

// Start polling when the page loads

var myMap = null;

// Function to create or update the map with new coordinates
function LiveDevicesMap2(data) {
    var latitude = parseFloat(data.Latitude);
    var longitude = parseFloat(data.Longitude);

    if (isNaN(latitude) || isNaN(longitude)) {
        console.log("Invalid latitude or longitude values");
        return;
    }

    var marker = { name: data.Name, coords: [latitude, longitude] };

    // Initialize the map if not already created
    if (myMap === null) {
        myMap = new jsVectorMap({
            map: 'world',
            selector: '#device_map',
            zoomOnScroll: false,
            zoomButtons: true,
            markers: [marker], // Set the initial marker
            markerStyle: {
                initial: { fill: "#5c5cff" },
                hover: { fill: "#ff5da0" },
                selected: { fill: "#00ff00" }
            },
            markersSelectable: true
        });

        console.log("Map initialized with markers:", [marker]);
    } else {
        // Clear existing markers and set new markers
        myMap.removeMarkers();
        debugger;
        //myMap.removeAllMarkers();
        //myMap.markers = [];
        myMap.addMarkers([marker]);

        console.log("Markers updated with new coordinates:", [marker]);
    }
}




function initMapnew(data) {
    var mapMarkers = [];
    var map = null;
    var directionsService = new google.maps.DirectionsService();
    var directionsRenderer = new google.maps.DirectionsRenderer();

    function calculateAndDisplayRoute(destination) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                var origin = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);

                directionsService.route(
                    {
                        origin: origin,
                        destination: destination,
                        travelMode: 'DRIVING',
                    },
                    function (response, status) {
                        if (status === 'OK') {
                            directionsRenderer.setDirections(response);
                        } else {
                            window.alert('Directions request failed due to ' + status);
                        }
                    }
                );
            },
            function (error) {
                console.error('Error getting current location:', error.message);
            }
        );
    }

    for (var x = 0; x < data.length; x++) {
        var marker = new google.maps.Marker({
            position: { lat: data[x].Latitude, lng: data[x].Longitude },
            title: data[x].Name,
        });

        mapMarkers.push(marker);

        // Add click event listener to each marker
        marker.addListener('click', function () {
            calculateAndDisplayRoute(this.getPosition());
        });
    }

    var mapOptions = {
        center: { lat: data[0].Latitude, lng: data[0].Longitude },
        zoom: 8,
    };

    map = new google.maps.Map(document.getElementById('map'), mapOptions);

    // Add markers to the map
    for (var i = 0; i < mapMarkers.length; i++) {
        mapMarkers[i].setMap(map);
    }

    // Attach directions renderer to the map
    directionsRenderer.setMap(map);
}

function OpportuntiyMap(data) {
    var mapMarkers = [];
    for (var x = 0; x < data.length; x++) {
        mapMarkers.push({ name: data[x].Title, coords: [data[x].Latitude, data[x].Longitude] });
    }
    var map = new jsVectorMap({
        map: 'world',
        selector: '#map_11',
        zoomOnScroll: false,
        zoomButtons: true,
        selectedMarkers: [0, 2],
        markersSelectable: true,
        markers: mapMarkers,
        markerStyle: {
            initial: { fill: "#5c5cff" },
            selected: { fill: "#ff5da0" }
        }
    });
}
/** Oppertunity Map Function END **/


/**  Opportunity Sale MonthWise function Start  **/
function OppSalesMonthWise(count) {

    var Sales = [];
    Sales = count;
    var BarChart,
        options = {
            chart: { height: 310, type: "bar", toolbar: { show: !1 } },
            plotOptions: {
                bar: { horizontal: !1, endingShape: "rounded", columnWidth: "20%" },
            },
            dataLabels: { enabled: !1 },
            stroke: { show: !0, width: 2, colors: ["transparent"] },
            colors: ["#4d79f6"],
            series: [
                { name: "Sale", data: Sales },

            ],
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: { show: !0, color: "#bec7e0" },
                axisTicks: { show: !0, color: "#bec7e0" },
            },
            legend: { show: !1, position: "top", horizontalAlign: "right" },
            fill: { opacity: 1 },
            grid: {
                row: { colors: ["transparent", "transparent"], opacity: 0.2 },
                borderColor: "#f1f3fa",
                strokeDashArray: 3,
            },
            tooltip: {
                y: {
                    formatter: function (e) {
                        return "$" + e;
                    },
                },
            },
        };
    (BarChart = new ApexCharts(
        document.querySelector("#AccOppStatsGraph"),
        options
    )).render();
}
/**  Opportunity Sale MonthWise function END  **/

/**All Year  Opportunity Sale MonthWise function Start  **/
function AllYearAccOppSales(count) {

    var Sales = [];
    Sales = count;
    var BarChart,
        options = {
            chart: { height: 310, type: "bar", toolbar: { show: !1 } },
            plotOptions: {
                bar: { horizontal: !1, endingShape: "rounded", columnWidth: "20%" },
            },
            dataLabels: { enabled: !1 },
            stroke: { show: !0, width: 2, colors: ["transparent"] },
            colors: ["#4d79f6"],
            series: [
                { name: "Opportunity", data: Sales },

            ],
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: { show: !0, color: "#bec7e0" },
                axisTicks: { show: !0, color: "#bec7e0" },
            },
            legend: { show: !1, position: "top", horizontalAlign: "right" },
            fill: { opacity: 1 },
            grid: {
                row: { colors: ["transparent", "transparent"], opacity: 0.2 },
                borderColor: "#f1f3fa",
                strokeDashArray: 3,
            },
            tooltip: {
                y: {
                    formatter: function (e) {
                        return "$" + e;
                    },
                },
            },
        };
    (BarChart = new ApexCharts(
        document.querySelector("#AccOppStatsGraph"),
        options
    )).render();
    $("#AccSalesDropDown").html("All Years <i class='las la-angle-down ms-1'></i>");
}
/**All Year  Opportunity Sale MonthWise function END  **/

/**    Opportuntiy Details Function Start   **/
function OpportunityDetails(data) {
    var table = $('#opportuntiyTable').dataTable({
        pageLength: 6,
        "searching": false,
        "lengthChange": false,
        "bAutoWidth": false,
        "aaData": data,
        "bDestroy": true,

        "columns": [{
            "data": "Title"
        }, {
            "data": "Amount"
        }, {
            "data": "Description"
        }, {
            "data": "Currency"
        }, {
            "data": "UserName"
        }, {
            "data": "Team"
        }, {
            "data": "OpportunityStages"
        }, {
            "data": "Probablity"
        }]
    })
}
/**    Opportuntiy Details Function END   **/


//*********************************************//
//           Service Tab Functions             //
//*********************************************//

/**    Cases Location Function  Start   **/
function CasesLocation(data) {
    var mapMarkers = [];
    for (var x = 0; x < data.length; x++) {
        mapMarkers.push({ name: data[x].CaseTitle, coords: [data[x].Latitude, data[x].Longitude] });
    }

    var map = new jsVectorMap({
        map: 'world',
        selector: '#map_case',
        zoomOnScroll: false,
        zoomButtons: true,
        selectedMarkers: [0, 2],
        markersSelectable: true,
        markers: mapMarkers,
        markerStyle: {
            initial: { fill: "#5c5cff" },
            selected: { fill: "#ff5da0" }
        }
    });
}
/**    Cases Location Function  END   **/

/**    Contact Table Function  Start   **/
function ContactTblFunc(data) {
    var table = $('#ContactTable').dataTable({
        pageLength: 6,
        "searching": false,
        "lengthChange": false,
        "bAutoWidth": false,
        "aaData": data,
        "bDestroy": true,

        "columns": [{
            "data": "Name"
        }, {
            "data": "Phone"
        }, {
            "data": "Email"
        }, {
            "data": "Address"
        }, {
            "data": "UserName"
        }, {
            "data": "Team"
        }, {
            "data": "Status"
        }]
    })
}
/**    Contact Table Function  END   **/

//*********************************************//
//           Device Tab Functions              //
//*********************************************//

/****  Connected Devices with Network Count Function Start ****/
function NetworkFunction(Count, Percentage) {
    $("#WIFI").text(Count[0] + "  Devices");
    $("#WIFI_Per").css("width", Percentage[0] + "%");

    $("#Ethernet").text(Count[1] + "  Devices");
    $("#Ethernet_Per").css("width", Percentage[1] + "%");

    $("#Cellular").text(Count[2] + "  Devices");
    $("#Cellular_Per").css("width", Percentage[2] + "%");

    $("#Bluetooth").text(Count[3] + "  Devices");
    $("#Bluetooth_Per").css("width", Percentage[3] + "%");
}
/****  Connected Devices with Network Count Function END ****/

/****  Connected Devices with Protocol Count Function Start  ****/
function GetProtocols(values, labels) {
    var chart,
        options = {
        };

    options = {
        chart: { height: 205, type: "donut" },
        plotOptions: { pie: { donut: { size: "85%" } } },
        dataLabels: { enabled: !1 },
        stroke: { show: !0, width: 2, colors: ["transparent"] },
        series: values,
        legend: {
            show: !1,
            position: "bottom",
            horizontalAlign: "center",
            verticalAlign: "middle",
            floating: !1,
            fontSize: "14px",
            offsetX: 0,
            offsetY: 5,
        },
        labels: labels,
        colors: ["#67c8ff", "#2a76f4", "#ff00aa"],
        responsive: [
            {
                breakpoint: 600,
                options: {
                    plotOptions: { donut: { customScale: 0.2 } },
                    chart: { height: 200 },
                    legend: { show: !1 },
                },
            },
        ],
        tooltip: {
            y: {
                formatter: function (e) {
                },
            },
        },
    },
        (chart = new ApexCharts(
            document.querySelector("#Protocol_graph"),
            options
        )).render();
}
/****  Connected Devices with Protocol Count Function END  ****/
/** Device Tracker Map Function Start **/
function DeviceTrackerMap() {
    //var mapMarkers = [];
    //for (var x = 0; x < data.length; x++) {
    //    mapMarkers.push({ name: data[x].Title, coords: [data[x].Latitude, data[x].Longitude] });
    //}
    var map = new jsVectorMap({
        map: 'world',
        selector: '#TrackerMap',
        zoomOnScroll: false,
        zoomButtons: true,
        selectedMarkers: [0, 2],
        markersSelectable: true,
        /*markers: mapMarkers,*/
        markers: [
            { name: "Tracker", coords: [35.09000000, 15.71000000] },
        ],
        markerStyle: {
            initial: { fill: "#5c5cff" },
            selected: { fill: "#ff5da0" }
        },
        labels: {
            markers: {
                render: marker => marker.name
            }
        }
    });
}
/** Device Tracker Map Function END **/

//*********************************************//
//             Work-Flows Tab Functions        //
//*********************************************//

/****  Work Flows Table Function  Start  ****/
function WorkFlowsTbl(data) {
    var table = $('#WorkFlowTable').dataTable({
        pageLength: 7,
        "lengthChange": false,
        "searching": false,
        "bAutoWidth": false,
        "aaData": data,
        "bDestroy": true,

        "columns": [{
            "data": "Name"
        }, {
            "data": "Threshold"
        }, {
            "data": "DeviceName"
        }, {
            "data": "SensorName"
        }, {
            "data": "Description"
        }]
    })
}
/****  Work Flows Table Function  END  ****/

/****  Work Flow Report Table Function Start  ****/
function WorkFlowRepoTbl(data) {
    var table = $('#WorkReportTable').dataTable({
        pageLength: 6,
        "searching": false,
        "lengthChange": false,
        "bAutoWidth": false,
        "aaData": data,
        "bDestroy": true,

        "columns": [{
            "data": "Name"
        }, {
            "data": "Action"
        }, {
            "data": "WorkFlowStatus"
        }, {
            "data": "AppliedTo"
        }, {
            "data": "Frequency"
        }]
    })
}
/****  Work Flow Report Table Function END  ****/




