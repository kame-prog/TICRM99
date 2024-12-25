$(document).ready(function () {
    $('#emailForm').hide();
    $('#deviceForm').hide();
    $('#PublishandSubscribeScreen').hide();

    var devicemarkers = [];
    $.ajax({
        type: "GET",
        url: "/Devices/GetAlldevicesLongLat",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = {
                    latLng: [response[i].Latitude, response[i].Longitude], name: response[i].Name,
                    mac: response[i].Mac, EMEI: response[i].EMEI, lat: response[i].Latitude, long: response[i].Longitude,
                    cloud: response[i].CloudServices, data: response[i].CloudData, id: response[i].deviceID, style: { r: 5 }
                }
                devicemarkers.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });
    //Device Map configuration

    var deviceMapObj = new jvm.Map({
        container: $('#DevicesMap'),
        map: 'world_mill_en',
        backgroundColor: "transparent",
        regionStyle: {
            initial: {
                fill: '#7fceff',
                "fill-opacity": 1,
                stroke: 'none',
                "stroke-width": 0,
                "stroke-opacity": 1
            },
            hover: {
                "fill-opacity": 0.8,
                cursor: 'pointer'
            },
            selected: {
                fill: 'yellow'
            },
            selectedHover: {
            }
        },
        markers: devicemarkers,
        onMarkerClick: function (event, index) {

            MapRightSlider(devicemarkers[index].EMEI, devicemarkers[index].lat,
                devicemarkers[index].long, devicemarkers[index].mac, devicemarkers[index].name, devicemarkers[index].cloud, devicemarkers[index].data, devicemarkers[index].id);
        }

    });

    deviceMapObj.addMarkers(devicemarkers, []);

    var accountmarkers = [];
    $.ajax({
        type: "GET",
        url: "/Accounts/GetAllAccountsLongLat",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { latLng: [response[i].Latitude, response[i].Longitude], name: response[i].Name, id: response[i].AccountId, style: { r: 5 } }
                accountmarkers.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });
    var AccountsMapObj = new jvm.Map({
        container: $('#AccountsMap'),
        map: 'world_mill_en',
        backgroundColor: "transparent",
        regionStyle: {
            initial: {
                fill: '#7fceff',
                "fill-opacity": 1,
                stroke: 'none',
                "stroke-width": 0,
                "stroke-opacity": 1
            },
            hover: {
                "fill-opacity": 0.8,
                cursor: 'pointer'
            },
            selected: {
                fill: 'yellow'
            },
            selectedHover: {
            }
        },
        markers: accountmarkers,
        onMarkerClick: function (event, index) {

            accSidebar(accountmarkers[index].id, accountmarkers[index].name);
        }
    });

    AccountsMapObj.addMarkers(accountmarkers, []);
    $('#sliderHideAndShowOnLight').hide();


    var options = {
        timeout: 1550,
        //Gets Called if the connection has sucessfully been established
        onSuccess: function () {
            console.log("Connected to Mqtt");
            //client.subscribe("DeviceToServerm", { qos: 2 });
            //client.subscribe(Topic, { qos: integer });
        },
        //Gets Called if the connection could not be established
        onFailure: function (message) {
            //alert("Connection failed: " + message.errorMessage);
        }
    };


    Adminclient.connect(options);
});

var MqttConfigurations = function () {
    $('#MqttConfigurations').modal('show');
}

function resolvedate(DateValue) {
    var DateVal = new Date(parseInt(DateValue.substr(6)));
    var dd = DateVal.getDate();

    var mm = DateVal.getMonth();
    var yyyy = DateVal.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    date = mm + '/' + dd + '/' + yyyy;
    return date;
}

var MapRightSlider = function (EMEI, lat, long, mac, name, cloud, data, id) {

    $('#MapRightSliderModal').modal('show');
    $('#btnsCloud').hide();
    $('#btnsAmazon').hide();
    $('#btnsINCA').hide();
    $('#EmeiVal').html(EMEI);
    $('#LatVal').html(lat);
    $('#LongVal').html(long);
    $('#MacVal').html(mac);
    $('#Dname').html(name);
    $('#Did').html(id);
    document.getElementById('macaddress').value = mac;
    if (cloud == "IBM") {
        $('#btnsCloud').show();
        $('#btnsINCA').hide();
        $('#btnsAmazon').hide();

    }
    else if (cloud == "Amazon") {
        $('#btnsCloud').hide();
        $('#btnsAmazon').show();
        $('#btnsINCA').hide();
    }
    else {
        $('#btnsCloud').hide();
        $('#btnsINCA').show();
        $('#btnsAmazon').hide();

    }

    GetGraphOfMAC(mac);


}
function GetGraphOfMAC(MacAddress) {

    //var MacAddress = localStorage.getItem('selectedDevice');
    $('#HideAndShowLineChart').hide();
    $('#HideAndShowMap').hide();
    $('#HideAndShowLightOnOff').hide();
    $('#HideAndShowLightOnOff1').hide();
    $('#HideAndShowLightOnOff2').hide();
    $('#HideAndShowMedical').hide();
    $('#HideAndShowTank').hide();
    var obj = { MacAddress: MacAddress };
    $.ajax({
        type: "GET",
        url: "/Devices/GetGraphOfMACData",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            var data = JSON.parse(response);
            for (var i = 0; i < data.length; i++) {
                if (data[i].SensorName == "Temperature" && data[i].GraphName == "Line") {
                    $('#HideAndShowLineChart').show();
                }
                else if (data[i].SensorName == "Liquid Level" && data[i].GraphName == "Tank") {
                    $('#HideAndShowTank').show();
                }
                else if (data[i].SensorName == "Location" && data[i].GraphName == "Map") {
                    $('#HideAndShowMap').show();
                }
                else if (data[i].SensorName == "Light" && data[i].GraphName == "On/Off") {
                    $('#HideAndShowLightOnOff').show();
                    $('#HideAndShowLightOnOff1').show();
                    $('#HideAndShowLightOnOff2').show();
                    ResetBlobSliderValue();
                }
                else if (data[i].SensorName == "Heat rate / Fever" && data[i].GraphName == "Medical / Checkups") {

                    $('#HideAndShowMedical').show();
                }
                for (var i = 0; i < data.length; i++) {
                    $("#Wsensors").append(new Option(data[i].SensorName, data[i].SensorName));
                }
            }
        },
        failure: function () {
            alert("Failed!");
            $('body').removeClass('m-page--loading');
        }
    });

}

var PostDataFW = function () {
    var ver = document.getElementById("FWver").value;
    var des = document.getElementById("FWdes").value;
    var ser = document.getElementById("FWser").value;
    var file = document.getElementById("file").value;
    var devname = document.getElementById("Dname").textContent;
    var obj = { DevName: devname, Version: ver, Desc: des, Serv: ser, File: file };
    // debugger;
    $.ajax({
        type: "POST",
        url: "/Firmwares/FWDevice",
        data: obj,
        dataType: "text",
        success: function (response) {
            if (response == "error") {
                ("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }

        },
        failure: function () {
            alert("Failed!");
        }
    })
}

var PostData = function () {
    var name = document.getElementById("Wname").value;
    var sensors = document.getElementById("Wsensors").value;
    var threshold = document.getElementById("WThreshold").value;
    var threshold1 = document.getElementById("WThreshold1").value;
    var action = document.getElementById("Waction").value;
    var priority = document.getElementById("Wpriority").value;
    var desc = document.getElementById("Wdescription").value;
    var accID = document.getElementById("accID").textContent;
    var devname = document.getElementById("Dname").textContent;
    var macadd = document.getElementById("macaddress").value;
    var cloud = document.getElementById("WCloud").value;
    if (priority == "High") {
        priority = 100;
    }
    else if (priority == "Medium") {
        priority = 66;
    }
    else if (priority == "Low") {
        priority = 33;
    }
    var thres = threshold + "," + threshold1;
    var obj = { Name: name, Sensor: sensors, Thresh: thres, Action: action, Priority: priority, desc: desc, AccountId: accID, DevName: devname, mac: macadd, Cloud: cloud };
    $.ajax({
        type: "POST",
        url: "/WorkFlows/workflowDevice",
        data: obj,
        dataType: "text",
        success: function (response) {
            if (response == "error") {
                ("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }

        },
        failure: function () {
            alert("Failed!");
        }
    })
    $('#workflow').hide();
}

var dps = []; // dataPoints
var chart = new CanvasJS.Chart("chartContainer", {
    height: 120,
    width: 280,
    title: {
        text: "Live Temperature"
    },
    axisY: {
        includeZero: false
    },
    data: [{
        type: "line",
        dataPoints: dps
    }]
});

var xVal = 0;
var yVal = 100;
var updateInterval = 1000;
var dataLength = 10; // number of dataPoints visible at any point
var count = 0;
var updateChart = function (Time, Temperature) {
    dps.push({
        x: Time,
        y: Temperature
    });
    count++;
    if (dps.length > dataLength) {
        dps.shift();
        count = 0;
    }
    //if (count === 5) {
    //}
    document.getElementById("temperatureValue").textContent = Temperature + ascii(176) + "C";

    chart.render();
};

$("#deviceTitle").text(localStorage.getItem('selectedDevice'));

function ascii(a) {
    return String.fromCharCode(a);
}

$('#toggle_event_editing button').click(function () {

    var IsDeviceOn = false;

    if ($(this).hasClass('locked_active') || $(this).hasClass('unlocked_inactive')) {
        /* code to do when unlocking */
        $('#switch_status').html('Switched on.');
        $('#BulbOnOffId').removeAttr('style');
        $('#BulbOnOffId').attr('style', "font-size: 200px;color: hsla(50, 100%, 50%, 1);display:block;");
        $('#sliderHideAndShowOnLight').show();
        IsDeviceOn = true;
    } else {
        /* code to do when locking */
        $('#switch_status').html('Switched off.');
        $('#BulbOnOffId').removeAttr('style');
        $('#BulbOnOffId').attr('style', "font-size: 200px;color: hsla(50, 0%, 50%, 1);display:block;");
        $('#sliderHideAndShowOnLight').hide();
        IsDeviceOn = false;
    }

    /* reverse locking status */
    $('#toggle_event_editing button').eq(0).toggleClass('locked_inactive locked_active btn-default btn-info');
    $('#toggle_event_editing button').eq(1).toggleClass('unlocked_inactive unlocked_active btn-info btn-default');

    var obj = { deviceStatus: IsDeviceOn };
    $.ajax({
        type: "POST",
        url: "/Devices/SendMessageToMqtt",
        data: obj,
        dataType: "text",
        success: function (response) {
            if (response == "error") {
                ("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }

        },
        failure: function () {
            alert("Failed!");
        }
    });

});


var slider = document.getElementById("myRange");
var output = document.getElementById("myRangeValue");
var slidevalue = slider.value;
output.innerHTML = slider.value;

slider.oninput = function () {
    $('#BulbOnOffId').attr('style', "font-size: 200px;color: hsla(50, " + this.value + "%, 50%, 1);display:block;");
    output.innerHTML = this.value;
    slidevalue = this.value;
    var obj = { silderValue: slidevalue };
    $.ajax({
        type: "POST",
        url: "/Devices/SendMessageToMqtts",
        data: obj,
        success: function (response) {
            if (response == "error") {
                ("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

var ResetBlobSliderValue = function () {
    $('#myRange').val('100');
    $('#myRangeValue').html('100');
}

$(function () {
    GetGraphDropDownData();
    GetSensorDropDownData();
});

function GetGraphDropDownData() {
    $.ajax({
        type: "GET",
        url: "/Devices/GetGraphDropDownList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#GraphIdDropDownlist').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].GraphId + '">' + response[i].GraphName + '</option>';
            }
            $('#GraphIdDropDownlist').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

function GetSensorDropDownData() {
    $.ajax({
        type: "GET",
        url: "/Devices/GetSensorDropDownList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#SensorIdDropDownList').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].SensorId + '">' + response[i].SensorName + '</option>';
            }
            $('#SensorIdDropDownList').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

$('#submit_DeviceSensorTemplate').on("click", function () {


    $('#submit_DeviceSensorTemplate').attr('disabled', 'disabled');
    $('#submit_DeviceSensorTemplate').html('Please wait..');

    var DeviceId = document.getElementById("Did").textContent;
    var SensorId = $('#SensorIdDropDownList').val();
    var GraphId = $('#GraphIdDropDownlist').val();

    if (SensorId == "Select" || SensorId == null) {
        alert("Please Select a Device Sensor.");
        $('#submit_DeviceSensorTemplate').removeAttr('disabled');
        $('#submit_DeviceSensorTemplate').html('Submit');
        return false;
    }
    else if (GraphId == null || GraphId == "Select") {
        alert("pleae select a Device Graph.");
        $('#submit_DeviceSensorTemplate').removeAttr('disabled');
        $('#submit_DeviceSensorTemplate').html('Submit');
        return false;
    }

    var obj = { DeviceId: DeviceId, SensorId: SensorId, GraphId: GraphId };
    $.ajax({
        type: "GET",
        url: "/Devices/SubmitDeviceSensorGraph",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            if (response == "error") {
                toastr.warning("Device Sensor Graph is not Saved. Please Refresh the page.");
            }
            else {
                toastr.success("Device Sensor Graph is saved successfully.");
                $("#config").hide();

            }

        },
        failure: function () {
            alert("Failed!");
        }
    });





});

//For tracking map
//jQuery.support.cors = true;

//var host;
//var defaultKey = "f8be18e9-3ab7-4874-9520-b385c841d82f";
//var profile = "car";

//// create a routing client to fetch real routes, elevation.true is only supported for vehicle bike or foot
//var ghRouting = new GraphHopper.Routing({ key: defaultKey, host: host, vehicle: profile, elevation: false });


//var routingMap = createMap('routing-map');
//setupRoutingAPI(routingMap, ghRouting);

$('#MapRightSliderModal').on('shown.bs.modal', function () {

    setTimeout(function () {
        routingMap.invalidateSize();
    }, 10);
});





//Map tracking End

/*MQTT Script*/
var globalint = 0;
var globalArray = new Array();
var globalDevicesArray = new Array();
var subscriptions = new Array();
var count = 0;
var count1 = 0;
var globalSelectedDevice;
var Adminclient = new Messaging.Client("192.168.22.79", 8080, "Swuich" + parseInt(Math.random() * 100, 10));
var messages = new Array();


//Gets  called if the websocket/mqtt connection gets disconnected for any reason
Adminclient.onConnectionLost = function (responseObject) {
    //Depending on your scenario you could implement a reconnect logic here
    alert("connection lost: " + responseObject.errorMessage);
};

//Gets called whenever you receive a message for your subscriptions
Adminclient.onMessageArrived = function (message) {
    //alert("i am here");
    var url = window.location.pathname;
    var array = message.payloadString.split(',');
    globalArray.push([array[0], array[1], array[2], array[3]]);
    messages.push(message.payload);
    count++;
    var options = '';
    options += '<div class="m-widget2__item m-widget2__item--primary">';
    options += '<div class="m-widget2__checkbox"></div>';
    options += '<div class="m-widget2__desc"><span class="m-widget2__text">';
    options += message.payloadString;
    options += '</span></br ><span class="m-widget2__user-name"><a href="#" class="m-widget2__link">';
    options += message.destinationName;

    options += '</a></span ></div ></div >';
    $('#receivedMessagesArea').prepend(options);

};

var publishToMqtt = function () {
    var payload = document.getElementById('PublishMessage').value;
    var Qos = document.getElementById('PublishQos').value;
    var Topic = document.getElementById('PublishTopic').value;

    if (payload === "" || Qos === "" || Topic === "") {
        alert("Enter All values to publish");
    } else {

        var integer = parseInt(Qos);

        var message = new Messaging.Message(payload);
        message.destinationName = Topic;
        message.qos = integer;
        client.send(message);
        console.log("Message published");

        document.getElementById('PublishMessage').value = "";
        document.getElementById('PublishQos').value = "";
        document.getElementById('PublishTopic').value = "";
    }
}

function subscribetoMQTT() {
   
    var Qos = document.getElementById('SubscribeQos').value;
    var Topic = document.getElementById('SubscribeTopic').value;

    if (Qos === "" || Topic === "") {
        alert("Enter All values to publish");
    } else {
        //Connect Options

        if (subscriptions.includes(Topic)) {
            alert(Topic + " : Already Subscribed");
            return;
        }


        var integer = parseInt(Qos);

        Adminclient.subscribe(Topic, { qos: integer });

        console.log("Sucscribed with " + Topic);


        subscriptions.push(Topic);

        if (subscriptions.length == 1) {
            document.getElementById("SubscribedTopicsList").innerHTML = "";
        }

        var myString = "";
        myString = myString + "<div class=\"m-widget2\">";
        myString = myString + "<div class=\"m-widget2__item m-widget2__item--primary\">\n";
        myString = myString + "<div class=\"m-widget2__checkbox\">\n";
        myString = myString + "\n";
        myString = myString + "</div>\n";
        myString = myString + "<div class=\"m-widget2__desc\">\n";
        myString = myString + "                                        <span class=\"m-widget2__text\">\n";
        myString = myString + Topic;
        myString = myString + "                                        </span>\n";
        myString = myString + "                                        <br>\n";
        myString = myString + "                                        <span class=\"m-widget2__user-name\">\n";
        myString = myString + "                                            <a href=\"#\" class=\"m-widget2__link\">\n";
        myString = myString + "                                                Topic for recieving messages from Gateway Devices\n";
        myString = myString + "                                            </a>\n";
        myString = myString + "                                        </span>\n";
        myString = myString + "                                    </div>\n";
        myString = myString + "                                </div>\n";
        myString = myString + "                            </div>\n";

        $('#SubscribedTopicsList').prepend(myString);

        toggleMqttScreens();

        document.getElementById('SubscribeQos').value = "";
        document.getElementById('SubscribeTopic').value = "";
    }
}

var hidden = "ConfigurationScreen";
function toggleMqttScreens() {

    if (hidden === "ConfigurationScreen") {
        $('#MessageScreen').hide();
        $('#PublishandSubscribeScreen').show();
        hidden = "MessageScreen";

        document.getElementById("InteractButton").innerHTML = "";
        document.getElementById("InteractButton").innerHTML = "Back";
        

    } else {
        $('#PublishandSubscribeScreen').hide();
        $('#MessageScreen').show();
        
        hidden = "ConfigurationScreen";

        document.getElementById("InteractButton").innerHTML = "";
        document.getElementById("InteractButton").innerHTML = "Interact";
    }

}


/*End mqtt*/
