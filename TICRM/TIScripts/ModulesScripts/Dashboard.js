
var open;
var closed;
var won;
function getperm(adminRole, salesRole, techRole) {

    if (adminRole == "true") {
        adminDashboard();
    }
    else if (salesRole == "true") {
        salesDashboard();
    }
    else if (techRole == "true") {
        techDashboard();
    }

    if (adminRole == "true" || salesRole == "true") {
        adminSalesDashboard();
    }

}


//Dashoard division based on role

function toggleDataSeries(e) {
    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
        e.dataSeries.visible = false;
    } else {
        e.dataSeries.visible = true;
    }

}

function adminDashboard() {
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetTechCount",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            document.getElementById("workordercounts").textContent = response.Workorders;
            document.getElementById("alertcounts").textContent = response.Alerts;
        },
        failure: function () {
            alert("Failed!");
        }
    });

    //Cost Charts
    var CostsDataPoints = [];
    $.ajax({
        type: "GET",
        url: "/Devices/GetAllCosts",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { label: resolvedate(response[i].date), y: response[i].Value, color: "orange" }
                CostsDataPoints.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });

    var Costschart = new CanvasJS.Chart("CostsChart", {
        theme: "light2",
        animationEnabled: true,
        backgroundColor: "transparent",
        title: {
            // text: "Game of Thrones Viewers of the First Airing on HBO"
        },
        axisY: {
            includeZero: false,
            title: "Cost",
            suffix: "$"
        },
        toolTip: {
            shared: "true"
        },
        legend: {
            cursor: "pointer",
            itemclick: toggleDataSeries
        },
        data: [
            {
                type: "spline",
                lineColor: "orange",
                showInLegend: true,
                legendMarkerColor: "orange",
                yValueFormatString: "##.00$",
                name: "Consumptions",
                dataPoints: CostsDataPoints
            }
        ]
    });
    Costschart.render();

    //Device Map configuration
    var devicemarkers = [];

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

    //fetch device markers for all devices

    $.ajax({
        type: "GET",
        url: "/Devices/GetAlldevicesLongLat",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            console.log("responseDb");
            console.log(response);
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

    deviceMapObj.addMarkers(devicemarkers, []);

    //fetch device markers for all devices


    //Fetch devices by account
    $('#accountmap').on('change', function () {

        var accountId = $('#accountmap option:selected').val();
        if (accountId !== "Select") {
            devicemarkers = [];
            $.ajax({
                type: "GET",
                data: { accountId: accountId },
                url: "/Devices/GetsingledevicesLongLat",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    for (var i = 0; i < response.length; i++) {
                        var obj = { latLng: [response[i].Latitude, response[i].Longitude], name: response[i].Name, style: { r: 5 } }
                        devicemarkers.push(obj);
                    }
                },
                failure: function () {
                    alert("Failed!");
                }
            });
            deviceMapObj.addMarkers(devicemarkers, []);
        }

    });
    //Fetch devices by account

    //populating list of accounts in device map drop down
    $.ajax({
        type: "GET",
        url: "/Master/GetAccounts",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#accountmap').html('');
            var options = '';
            options += '<option value="Select">Select by account</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#accountmap').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
}

function adminSalesDashboard () {
    $.ajax({
        type: "GET",
        url: "/Opportunities/GetOppCounts",
        dataType: "json",
        async: false,
        success: function (response) {
            open = response.Open;
            closed = response.Lost;
            won = response.Lostwon;
        },
        failure: function () {
            alert("Failed!");
        }
    });


    var chart = new CanvasJS.Chart("salesChart", {
        //exportEnabled: true,
        animationEnabled: true,
        backgroundColor: "transparent",
        title: {
            //text: "Sales Chart"
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "bottom",
            horizontalAlign: "center",
            itemclick: explodePie
        },
        data: [{
            type: "pie",
            showInLegend: true,
            toolTipContent: "{name}: <strong>{y}%</strong>",
            indexLabel: "{name} - {y}%",
            dataPoints: [
                { y: open, name: "Open", exploded: true },
                { y: closed, name: "Closed as Lost" },
                { y: won, name: "Closed as Won" }
            ]
        }]
    });
    chart.render();


    //var chart2 = new CanvasJS.Chart("salesChart2", {
    //    //exportEnabled: true,
    //    animationEnabled: true,
    //    backgroundColor: "transparent",
    //    title: {
    //        //text: "Sales Chart"
    //    },
    //    legend: {
    //        cursor: "pointer",
    //        verticalAlign: "center",
    //        horizontalAlign: "right",
    //        itemclick: explodePie
    //    },
    //    data: [{
    //        type: "pie",
    //        showInLegend: true,
    //        toolTipContent: "{name}: <strong>{y}%</strong>",
    //        indexLabel: "{name} - {y}%",
    //        dataPoints: [
    //            { y: open, name: "Open", exploded: true },
    //            { y: closed, name: "Closed as Lost" },
    //            { y: won, name: "Closed as Won" }
    //        ]
    //    }]
    //});
    //chart2.render();
}

function salesDashboard() {
    //fetch Opportunity markers for all devices
    var oppmarkers = [];

    var OpportunitiesMapObj = new jvm.Map({
        container: $('#OpportunitiesMap'),
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
        markers: oppmarkers,
        onMarkerClick: function (event, index) {

            LoadOpportunityOnId(oppmarkers[index].id);
        }
    });

    $.ajax({
        type: "GET",
        url: "/Opportunities/GetAllOpportunitiesLongLat",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { latLng: [response[i].Latitude, response[i].Longitude], name: response[i].Name, id: response[i].OpportunityId, style: { r: 5 } }
                oppmarkers.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });

    OpportunitiesMapObj.addMarkers(oppmarkers, []);
    //fetch Opportunity markers for all devices


    //fetch Accounts markers for all devices

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

    //fetch Account markers for all devices

    var boiler, engine, gen, proj, tours;
    $.ajax({
        type: "GET",
        url: "/CustomerAssets/GetAssetTypes",
        dataType: "json",
        async: false,
        success: function (response) {

            boiler = response.Boiler;
            engine = response.Engine;
            gen = response.Generator;
            proj = response.Projector;
            tours = response.Tours;

        },
        failure: function () {
            alert("Failed!");
        }
    });

    var Assetschart = new CanvasJS.Chart("Assetschart", {
        //exportEnabled: true,
        animationEnabled: true,
        backgroundColor: "transparent",
        title: {
            //text: "Sales Chart"
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "center",
            horizontalAlign: "right",
            itemclick: explodePie
        },
        data: [{
            type: "pie",
            showInLegend: true,
            toolTipContent: "{name}: <strong>{y}%</strong>",
            indexLabel: "{name} - {y}%",
            dataPoints: [
                { y: boiler, name: "boiler", color: "green" },
                { y: engine, name: "Engin" },
                { y: gen, name: "Generator", color: "yellow" },
                { y: proj, name: "Projector", color: "blue" },
                { y: tours, name: "Tours", color: "purple" }
            ]
        }]
    });
    Assetschart.render();
    //Assets chart End
}

function techDashboard() {
    var disconnectiondatapoints = [];
    $.ajax({
        type: "GET",
        url: "/Devices/GetAllDisconnectionList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            for (var i = 0; i < response.length; i++) {
                var obj = { label: resolvedate(response[i].date), y: response[i].Value, color: "orange" }

                disconnectiondatapoints.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });

    var Disconnectionchart = new CanvasJS.Chart("Disconnectionchart", {
        height: 270,
        theme: "light2",
        backgroundColor: "transparent",
        animationEnabled: true,
        title: {
        },
        axisY: {
            includeZero: false,
            title: "Disconnections",
            suffix: "--"
        },
        toolTip: {
            shared: "true"
        },
        legend: {
            cursor: "pointer",
            itemclick: toggleDataSeries
        },
        data: [
            {
                type: "spline",
                lineColor: "orange",
                showInLegend: true,
                legendMarkerColor: "orange",
                yValueFormatString: "##",
                name: "Disconnectivity",
                dataPoints: disconnectiondatapoints
            }
        ]
    });
    Disconnectionchart.render();

    //Power consumption
    var consumptiondatapoints = [];
    $.ajax({
        type: "GET",
        url: "/Devices/GetAllConsumption",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { label: resolvedate(response[i].date), y: response[i].Value, color: "orange" }
                consumptiondatapoints.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });

    var Powerconsumptionchart = new CanvasJS.Chart("powerconsumptionchart", {
        height: 270,
        theme: "light2",
        backgroundColor: "transparent",
        animationEnabled: true,
        title: {
            // text: ""
        },
        axisY: {
            includeZero: false,
            title: "Power Consumption",
            suffix: "--"
        },
        toolTip: {
            shared: "true"
        },
        legend: {
            cursor: "pointer",
            itemclick: toggleDataSeries
        },
        data: [
            {
                type: "spline",
                lineColor: "red",
                showInLegend: true,
                legendMarkerColor: "red",
                yValueFormatString: "##.00$",
                name: "Power Consumption",
                dataPoints: consumptiondatapoints
            }
        ]
    });
    Powerconsumptionchart.render();
    var firmwareDatapointsU = [];
    var firmwareDatapointsP = [];
    $.ajax({
        type: "GET",
        url: "/Firmwares/GetFirmwaresPending",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { label: resolvedate(response[i].date), y: response[i].Value }
                firmwareDatapointsP.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });
    $.ajax({
        type: "GET",
        url: "/Firmwares/GetFirmwaresUpdated",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { label: resolvedate(response[i].date), y: response[i].Value }
                firmwareDatapointsU.push(obj);
            }
        },
        failure: function () {
            alert("Failed!");
        }
    });

    var firmwareChart = new CanvasJS.Chart("gatewayChart", {
        animationEnabled: true,
        backgroundColor: "transparent",
        title: {

        },
        axisX: {

        },
        axisY: {
            valueFormatString: "",
            gridColor: "#B6B1A8",
            tickColor: "#B6B1A8"
        },
        toolTip: {
            shared: true,
            content: toolTipContent
        },
        data: [{
            type: "stackedColumn",
            showInLegend: true,
            color: "#696661",
            name: "Pending",
            dataPoints: firmwareDatapointsU
        },
        {
            type: "stackedColumn",
            showInLegend: true,
            name: "Updated",
            color: "#EDCA93",
            dataPoints: firmwareDatapointsP
        }
        ]
    });
    firmwareChart.render();

    function toolTipContent(e) {
        var str = "";
        var total = 0;
        var str2, str3;
        for (var i = 0; i < e.entries.length; i++) {
            var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\"> " + e.entries[i].dataSeries.name + "</span>: $<strong>" + e.entries[i].dataPoint.y + "</strong>bn<br/>";
            total = e.entries[i].dataPoint.y + total;
            str = str.concat(str1);
        }
        str2 = "<span style = \"color:DodgerBlue;\"><strong>" + (e.entries[0].dataPoint.x).getFullYear() + "</strong></span><br/>";
        total = Math.round(total * 100) / 100;
        str3 = "<span style = \"color:Tomato\">Total:</span><strong> $" + total + "</strong>bn<br/>";
        return (str2.concat(str)).concat(str3);
    }

    var mqtt, http, lorawan, cellular, ethernet, wifi;

    $.ajax({
        type: "GET",
        url: "/Devices/GetCounts",
        dataType: "json",
        async: false,
        success: function (response) {
            mqtt = response.MQTT;
            http = response.HTTP;
            lorawan = response.LORAWAN;
            cellular = response.CELLULAR;
            ethernet = response.ETHERNET;
            wifi = response.WIFI;


        },
        failure: function () {
            alert("Failed!");
        }
    });

    //$.ajax({
    //    type: "GET",
    //    url: "/WorkOrders/GetWorkorderCount",
    //    dataType: "json",
    //    async: false,
    //    success: function (response) {


    //    },
    //    failure: function () {
    //        alert("Failed!");
    //    }
    //});

    var channelschart = new CanvasJS.Chart("channelschart", {
        animationEnabled: true,
        theme: "light2", // "light1", "light2", "dark1", "dark2"
        backgroundColor: "transparent",
        title: {
            //text: "Top Oil Reserves"
        },
        axisY: {
            title: "Channels"
        },
        data: [{
            type: "column",
            showInLegend: true,
            legendMarkerColor: "grey",
            legendText: "channels",
            dataPoints: [
                { y: wifi, label: "Wi-Fi" },
                { y: cellular, label: "Cellular" },
                { y: http, label: "HTTP" },
                { y: mqtt, label: "MQTT" },
                { y: ethernet, label: "Ethernet" },
                { y: lorawan, label: "LoraWAN" },
            ]
        }]
    });
    channelschart.render();
}

function explodePie(e) {
    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
    } else {
        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
    }
    e.chart.render();
}
        /*Maps End*/
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


$(function () {

    $('#jstree1').on('changed.jstree', function (e, data) {
        var objNode = data.instance.get_node(data.selected);

        if (objNode == false) { return false; }


    }).jstree({
        "core": {
            "multiple": true,
            "check_callback": true,
            'themes': {
                "responsive": true,
                'variant': 'larg',
                'stripes': false,
                'dots': false,
                'icons': false
            }
        },
        "types": {
            "default": {

            },
            "file": {
                "icon": "fa fa-microchip icon-state-warning icon-lg"

            }
        },
        "plugins": ["dnd", "state", "types", "sort"]
    });

    $('#consumptionTree').on('changed.jstree', function (e, data) {
        var objNode = data.instance.get_node(data.selected);


        if (objNode == false) { return false; }


    }).jstree({
        "core": {
            "multiple": true,
            "check_callback": true,
            'themes': {
                "responsive": true,
                'variant': 'larg',
                'stripes': false,
                'dots': false,
                'icons': false
            }
        },
        "types": {
            "default": {

            },
            "file": {
                "icon": "fa fa-microchip icon-state-warning icon-lg"

            }
        },
        "plugins": ["dnd", "state", "types", "sort"]
    });

});

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


    //var str = data.split(",");

    ////Organization ID
    //var org = str[0].toString();
    //org = org.match(":(.*)");
    //var orgID = org[1].toString();
    //orgID = orgID.substr(1, orgID.length - 2);
    ////Application Name
    //var app = str[1].toString();
    //app = app.match(":(.*)");
    //app = app[1].toString();
    //var appname = app.substr(1, app.length - 2);
    ////Device Type
    //var dev = str[4].toString();
    //dev = dev.match(":(.*)");
    //dev = dev[1].toString();
    //var deviceType = dev.substr(1, dev.length - 2);

    ////Device ID substr.length-1
    //var devid = str[5].toString();
    //devid = devid.match(":(.*)");
    //devid = devid[1].toString();
    //var deviceID = devid.substr(1, devid.length - 2);

    //url = "https://www." + orgID + "." + appname + ".ibmcloud.com/dashboard/devices/drilldown/" + deviceType + ":" + deviceID;
    GetGraphOfMAC(mac);


}
function GetGraphOfMAC(MacAddress) {


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
function ascii(a) { return String.fromCharCode(a); }

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
jQuery.support.cors = true;

var host;
var defaultKey = "f8be18e9-3ab7-4874-9520-b385c841d82f";
var profile = "car";

// create a routing client to fetch real routes, elevation.true is only supported for vehicle bike or foot
var ghRouting = new GraphHopper.Routing({ key: defaultKey, host: host, vehicle: profile, elevation: false });


var routingMap = createMaps('routing-map');
setupRoutingAPI(routingMap, ghRouting);

$('#MapRightSliderModal').on('shown.bs.modal', function () {

    setTimeout(function () {
        routingMap.invalidateSize();
    }, 10);
});

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
//Map tracking Start PART 2

var iconObject = L.icon({
    iconUrl: '/Content/Metronic/vendors/graphhopper/img/marker-icon.png',
    shadowSize: [50, 64],
    shadowAnchor: [4, 62],
    iconAnchor: [12, 40]
});

function setupRoutingAPI(map, ghRouting) {

    map.setView([52.521235, 13.3992], 12);

    var instructionsDiv = $("#instructions");

    map.on('click', function (e) {
        if (ghRouting.points.length > 1) {
            ghRouting.clearPoints();
            routingLayer.clearLayers();
        }
        //Lahore
        e.latlng.lat = 31.520;
        e.latlng.lng = 74.3587;

        L.marker([31.20, 74.3587], { icon: iconObject }).addTo(routingLayer);

        //Karachi
        e.latlng.lat = 24.8607;
        e.latlng.lng = 67.0011;
        L.marker(e.latlng, { icon: iconObject }).addTo(routingLayer);

        // ghRouting.addPoint(new GHInput(e.latlng.lat, e.latlng.lng));
        ghRouting.addPoint(new GHInput(31.5204, 74.3587));
        ghRouting.addPoint(new GHInput(24.8607, 67.0011));

        if (ghRouting.points.length > 1) {
            // ******************
            //  Calculate route!
            // ******************
            ghRouting.doRequest()
                .then(function (json) {
                    var path = json.paths[0];
                    routingLayer.addData({
                        "type": "Feature",
                        "geometry": path.points
                    });
                    var outHtml = "Distance in meter:" + path.distance;
                    outHtml += "<br/>Times in seconds:" + path.time / 1000;
                    outHtml += "<br/><a href='" + ghRouting.getGraphHopperMapsLink() + "'>GraphHopper Maps</a>";
                    $("#routing-response").html(outHtml);

                    if (path.bbox) {
                        var minLon = path.bbox[0];
                        var minLat = path.bbox[1];
                        var maxLon = path.bbox[2];
                        var maxLat = path.bbox[3];
                        var tmpB = new L.LatLngBounds(new L.LatLng(minLat, minLon), new L.LatLng(maxLat, maxLon));
                        map.fitBounds(tmpB);
                    }

                    instructionsDiv.empty();
                    if (path.instructions) {
                        var allPoints = path.points.coordinates;
                        var listUL = $("<ol>");
                        instructionsDiv.append(listUL);
                        for (var idx in path.instructions) {
                            var instr = path.instructions[idx];

                            // use 'interval' to find the geometry (list of points) until the next instruction
                            var instruction_points = allPoints.slice(instr.interval[0], instr.interval[1]);

                            // use 'sign' to display e.g. equally named images

                            $("<li>" + instr.text + " <small>(" + ghRouting.getTurnText(instr.sign) + ")</small>" +
                                " for " + instr.distance + "m and " + Math.round(instr.time / 1000) + "sec" +
                                ", geometry points:" + instruction_points.length + "</li>").appendTo(listUL);
                        }
                    }

                })
                .catch(function (err) {
                    var str = "An error occured: " + err.message;
                    $("#routing-response").text(str);
                });
        }
    });

    //start

    //End

    var instructionsHeader = $("#instructions-header");
    instructionsHeader.click(function () {
        instructionsDiv.toggle();
    });

    var routingLayer = L.geoJson().addTo(map);
    routingLayer.options = {
        style: { color: "#00cc33", "weight": 5, "opacity": 0.6 }
    };
}
