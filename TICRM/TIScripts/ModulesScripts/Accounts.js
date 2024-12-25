/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Accounts script file Containing all the scripts it use
 */
//index Page Script Starts
var url = window.location.pathname.toLowerCase();

if (url == "/accounts/index" || url == "/accounts") {
    $(document).ready(function () {

        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Accounts/GetAccountsList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "Email" },
                { "mData": "Description" },
                { "mData": "AccountType.Name" },
                { "mData": "OppCount" },
                { "mData": "LocationCount" },
                { "mData": "AssetCount" },
                { "mData": "DeviceCount" },
                {
                    "mData": function (o) {
                        return '<a href="/Accounts/Edit/' + o.AccountId + '" title="Edit Activity"><i class="fa fa-edit"></i></a> | <a href="/Accounts/AccountsDetail/' + o.AccountId + '" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.AccountId + '\')" class="arial" title="Delete Activity"><i class="fa fa-trash"></i></a>';
                    }
                },
            ],
            responsive: true,
            'bSortable': true,
            scrollY: false,
            scrollX: true,
            scrollCollapse: true,
            "processing": true,
            "language": {
                //processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Fetching Data...</span> '
                sProcessing: '<div class= "vertical-centered-box" ><div class="content"><img width="100" height="90" version="1.1" src="/Content/Images/swuich final logo.png" /><div class="m-blockui" style="margin-top: 10%;margin-left: -15%;"><span>Fetching Data...</span><span><div class="m-loader m-loader--brand"></div></span></div></div></div>'
            },

            //== Pagination settings
            dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
                                                <'row'<'col-sm-12'tr>>
                                                <'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,
            buttons: [
                'print',
                'pdfHtml5',
            ],
            "initComplete": function () {
                $('#m_table_1_processing').removeClass("card");
            }
        });

    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Accounts/AccountDetailsPartial",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $("#m_modal_Details").children('.modal-dialog.modal-rightside-dialog.modal-RightSide-slideout').children('.modal-content').html('').html(response);

                $('#m_table_Assets').DataTable({
                    columnDefs: [
                        { "width": "10px", "targets": 0 },
                        { "width": "40px", "targets": 1 },
                        { "width": "100px", "targets": 2 },
                        { "width": "70px", "targets": 3 },
                        { "width": "70px", "targets": 4 },
                        { "width": "70px", "targets": 5 },
                        { "width": "70px", "targets": 6 },
                        { "width": "70px", "targets": 7 },
                        { "width": "70px", "targets": 8 }
                    ]


                });


                $('#m_table_Location').DataTable({ scrollX: true });
                $('#m_table_Device').DataTable({ scrollX: true });

                $('#m_table_Opportunity').DataTable({ scrollX: true });

                $('#m_modal_Details').modal('show');

                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }

    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Accounts/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $("#m_modal_Delete").children('.modal-dialog').children('.modal-content').html('').html(response);

                $('#m_modal_Delete').modal('show');
                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }

}
//index Page Script Ends


//Edit Page Script Starts
if (url.includes("/accounts/edit")) {
   
}
//Edit Page Script Ends

//Edit Page Script Starts
if (url.includes("/accounts/accountsdetail")) {
    //alert("han b");
    $(document).ready(function () {
        var accountId = document.getElementById("accID").textContent;
        var obj = { accountId: accountId };
        var costDataPoints = [];
        $.ajax({
            type: "GET",
            url: "/Accounts/GetAccountCost",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                for (var i = 0; i < response.length; i++) {
                    var obj = { label: resolvedate(response[i].Date), y: parseInt(response[i].Cost1) }
                    costDataPoints.push(obj);
                }
            },
            failure: function () {
                alert("Failed!");
            }
        });

        var open = document.getElementById("oppOpen").textContent;
        var closed = document.getElementById("oppLost").textContent;
        var won = document.getElementById("oppLostWon").textContent;

        var chart2 = new CanvasJS.Chart("salesChart2", {
            animationEnabled: true,
            backgroundColor: "transparent",
            title: {
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
        chart2.render();

        function explodePie(e) {
            if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
            } else {
                e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
            }
            e.chart.render();
        }
        var Powerconsumptionchart = new CanvasJS.Chart("costchart", {
            theme: "light2",
            animationEnabled: true,
            backgroundColor: "transparent",
            title: {
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
                    dataPoints: costDataPoints
                }
            ]
        });
        Powerconsumptionchart.render();

        function toggleDataSeries(e) {
            if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                e.dataSeries.visible = false;
            } else {
                e.dataSeries.visible = true;
            }

        }

        $(function () {

            $('#jstree1').on('changed.jstree', function (e, data) {
                var objNode = data.instance.get_node(data.selected);


                if (objNode == false) { return false; }

                if (objNode.li_attr.deviceid != undefined && objNode.li_attr.deviceid != AccountDetialDeviceId) {
                    AccountDetialDeviceId = objNode.li_attr.deviceid;
                    LoadDeviceSensorGraph(objNode.li_attr.deviceid);
                }

                if (objNode.li_attr.macaddress != undefined && objNode.li_attr.deviceid != "" && objNode.li_attr.macaddress != AccountDetailMacAddress) {
                    AccountDetailMacAddress = objNode.li_attr.macaddress;
                    localStorage.setItem('selectedDevice', objNode.li_attr.macaddress);
                    GetGraphOfMAC();
                }
                if (objNode.li_attr.assetid != "undefined" && objNode.li_attr.assetid != "" && objNode.li_attr.assetid != AccountDetialAssetId) {
                    AccountDetialAssetId = objNode.li_attr.assetid;
                    localStorage.setItem('SelectedAsset', objNode.li_attr.assetid);
                }

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


        function GetGraphOfMAC() {

            var MacAddress = localStorage.getItem('selectedDevice');

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
                        if (data[i].SensorName == "Liquid Level" && data[i].GraphName == "Tank") {
                            $('#HideAndShowTank').show();

                        }
                        if (data[i].SensorName == "Location" && data[i].GraphName == "Map") {
                            $('#HideAndShowMap').show();
                        }
                        if (data[i].SensorName == "Light" && data[i].GraphName == "On/Off") {
                            $('#HideAndShowLightOnOff').show();
                            $('#HideAndShowLightOnOff1').show();
                            $('#HideAndShowLightOnOff2').show();
                            ResetBlobSliderValue();
                        }
                        if (data[i].SensorName == "Heat rate / Fever" && data[i].GraphName == "Medical / Checkups") {
                            $('#HideAndShowMedical').show();
                        }

                    }
                    for (var i = 0; i < data.length; i++) {
                        $("#Wsensors").append(new Option(data[i].SensorName, data[i].SensorName));
                    }
                },
                failure: function () {
                    alert("Failed!");
                    $('body').removeClass('m-page--loading');
                }
            });

        }


        var LoadOpportunityOnId = function (id) {
            $('body').addClass('m-page--loading');

            $.ajax({
                type: "GET",
                url: "/Accounts/GetOppertunityDetailOnId",
                data: { id: id },
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {
                    $('.modal-content-rightside').html('').html(response);

                    $('#RightSideModal').modal('show');
                    $('body').removeClass('m-page--loading');
                },
                failure: function () {
                    alert("Failed!");
                    $('body').removeClass('m-page--loading');
                }
            });
        }

        /**
         * Workflow Design
         */
        var LoadWorkflow = function () {
            $('body').addClass('m-page--loading');
            $.ajax({
                type: "GET",
                url: "/Accounts/GetWorkflow",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {
                    $('.modal-content-rightside').html('').html(response);
                    $('#RightSideModal').modal('show');
                    $('body').removeClass('m-page--loading');
                },
                failure: function () {
                    alert("Failed!");
                    $('body').removeClass('m-page--loading');
                }
            });
        }

        var LoadDeviceSensorGraph = function (deviceId) {

            if (deviceId == "undefined") {
                return false;
            }

            var obj = { deviceId: deviceId };
            console.log("DSGonDeviceId");
            $.ajax({
                type: "GET",
                url: "/Devices/DSGonDeviceId",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {

                    var data = JSON.parse(response);
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].SensorName == "Temperature") {

                        }
                        else if (data[i].SensorName == "Location") {

                        }
                        else if (data[i].SensorName == "Light") {

                        }
                    }
                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }


        var ToConfirms = function () {
            var value = document.getElementById("MacVal").innerHTML;
            var name = document.getElementById("Dname").innerHTML;
            var lat = document.getElementById("LatVal").innerHTML;
            var long = document.getElementById("LongVal").innerHTML;

            var location = lat + ',' + long;
            if (confirm('Are you Sure?')) { window.location = '/Firmwares/Create?MacAddress=' + value + '&devicename=' + name + '&location=' + location; }
        }


        var assetRightSlider = function (assetName) {
            var AssetId = localStorage.getItem('SelectedAsset');
            document.getElementById("AssetTitle").textContent = assetName;
            $('#HideAndShowLineChartAsset').hide();
            $('#HideAndShowMapAsset').hide();
            $('#HideAndShowLightOnOffAsset').hide();
            $('#HideAndShowLightOnOff1Asset').hide();
            $('#HideAndShowLightOnOff2Asset').hide();
            $('#HideAndShowMedicalAsset').hide();
            $('#HideAndShowTankAsset').hide();
            var ConnetedDevicesAsset = 0;;
            var obj = { AssetId: AssetId };
            $.ajax({
                type: "GET",
                url: "/Devices/GetGraphOfAssetData",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {
                    var data = JSON.parse(response);
                    for (var i = 0; i < data.length; i++) {

                        if (data[i].SensorName == "Temperature" && data[i].GraphName == "Line") {
                            $('#HideAndShowLineChartAsset').show();
                        }
                        if (data[i].SensorName == "Liquid Level" && data[i].GraphName == "Tank") {
                            $('#HideAndShowTankAsset').show();

                        }
                        if (data[i].SensorName == "Location" && data[i].GraphName == "Map") {
                            $('#HideAndShowMapAsset').show();
                        }
                        if (data[i].SensorName == "Light" && data[i].GraphName == "On/Off") {
                            $('#HideAndShowLightOnOffAsset').show();
                            $('#HideAndShowLightOnOff1Asset').show();
                            $('#HideAndShowLightOnOff2Asset').show();
                            ResetBlobSliderValue();
                        }
                        if (data[i].SensorName == "Heat rate / Fever" && data[i].GraphName == "Medical / Checkups") {
                            $('#HideAndShowMedicalAsset').show();
                        }

                    }
                    for (var i = 0; i < data.length; i++) {
                        $("#Wsensors").append(new Option(data[i].SensorName, data[i].SensorName));
                    }
                },
                failure: function () {
                    alert("Failed!");
                    $('body').removeClass('m-page--loading');
                }
            });
            $.ajax({
                type: "GET",
                url: "/Devices/GetListOfDevicesonAssetId",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {
                    var dataDevices = JSON.parse(response);
                    console.log("Devices");
                    console.log(dataDevices);
                    console.log("Devices");
                    document.getElementById("TotalDevicesAssets").textContent = dataDevices.length;
                    if (ConnectedDevices.length != 0) {
                        for (var i = 0; i < dataDevices.length; i++) {
                            for (var j = 0; j < ConnectedDevices.length; j++) {
                                if (ConnectedDevices[j] == dataDevices[i].Mac) {
                                    ConnetedDevicesAsset++;
                                }
                            }
                        }
                        document.getElementById("ConnectedDevicesAssets").textContent = ConnetedDevicesAsset;
                    }
                    else
                        document.getElementById("ConnectedDevicesAssets").textContent = "0";

                },
                failure: function () {
                    alert("Failed!");
                    $('body').removeClass('m-page--loading');
                }
            });
            $("#MapRightSliderModalAsset").modal("show");

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



        }
        var nevToURL = function () {

            window.location.assign(url);
        }

        var MapRightSliderD = function () {
            $('#MapRightSliderModalD').modal('show');
        }
        var WorfFlowModalShow = function () {
            $('#WorfFlowModal').modal('show');
        }
        var MapRightSliderO = function () {
            $('#MapRightSliderModalO').modal('show');
        }

        var MapRightSliderC = function () {
            $('#MapRightSliderModalC').modal('show');
        }

        var MapRightSliderW = function () {
            $('#MapRightSliderModalW').modal('show');
        }

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

    //Ibm Cloud
    var CloudSynchronize = function () {
        IBMCloudSynchronize();
    }

    var IBMCloudSynchronize = function () {

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Devices/SynchronizedWithIBM",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $('#m_modal_IBMList').modal('show');

                $('#m_portlet_tab_IBMCloudTable').html('').html(response);

                $('#m_table_ibm').DataTable({
                    responsive: true,
                    "bPaginate": true,
                    "bFilter": false,
                    "bInfo": false,
                    "pageLength": 5,
                    "lengthChange": false,
                    "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
                });

                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });

    }


    //Module Creation
    var deviceCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var name = document.getElementById("DeviceName").value;
        var macAdd = document.getElementById("DeviceMacadd").value;
        var emei = document.getElementById("DeviceEmei").value;
        var reg = document.getElementById("DeviceReg").value;
        var lat = document.getElementById("DeviceLat").value;
        var long = document.getElementById("DeviceLong").value;
        var asset = $('#DeviceAsset option:selected').val();
        var user = $('#DeviceUser option:selected').val();
        var team = $('#DeviceTeam option:selected').val();
        var main = $('#DeviceMain option:selected').val();
        var cloud = $('#DeviceCloud option:selected').val();
        var status = $('#DeviceStatus option:selected').val();

        var obj = { Name: name, Mac: macAdd, EMEI: emei, Reg: reg, Lat: lat, Long: long, Asset: asset, User: user, Team: team, Main: main, Cloud: cloud, Status: status, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/Devices/CreatefromAccount",
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
        $('#Createdevice').modal('hide');

    }

    var locationCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var name = document.getElementById("LocationName").value;
        var desc = document.getElementById("LocationDesc").value;
        var lat = document.getElementById("LocationLat").value;
        var long = document.getElementById("LocationLong").value;
        var user = $('#LocationUser option:selected').val();
        var team = $('#LocationTeam option:selected').val();
        var loc = $('#LocationLoc option:selected').val();
        var add = $('#LocationAdd option:selected').val();
        var status = $('#LocationStatus option:selected').val();
        var obj = { Name: name, Desc: desc, Lat: lat, Long: long, User: user, Team: team, Loc: loc, Add: add, Status: status, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/Locations/CreateLfromAccount",
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
        $('#CreateLocation').modal('hide');

    }

    var AssetCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var title = document.getElementById("AssetsTitle").value;
        var desc = document.getElementById("AssetsDesc").value;
        var man = document.getElementById("AssetsMan").value;
        var model = document.getElementById("AssetsModel").value;
        var year = document.getElementById("AssetsYear").value;
        var val = document.getElementById("AssetsVal").value;
        var dep = document.getElementById("AssetsDep").value;
        var sku = document.getElementById("AssetsSKU").value;
        var user = $('#AssetsUser option:selected').val();
        var team = $('#AssetsTeam option:selected').val();
        var loc = $('#AssetsLoc option:selected').val();
        var type = $('#AssetsType option:selected').val();
        var status = $('#AssetsStatus option:selected').val();
        var obj = { Title: title, Desc: desc, User: user, Team: team, Loc: loc, Man: man, Status: status, Model: model, Year: year, Val: val, Dep: dep, SKU: sku, Type: type, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/CustomerAssets/CreateAfromAccount",
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
        $('#CreateAssets').modal('hide');

    }

    var contactCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var name = document.getElementById("ContactName").value;
        var email = document.getElementById("ContactEmail").value;
        var phone = document.getElementById("ContactPhone").value;
        var add = document.getElementById("ContactAdd").value;
        var create = document.getElementById("ContactCrtd").value;
        var update = document.getElementById("ContactUpdate").value;
        var user = $('#ContactUser option:selected').val();
        var team = $('#ContactTeam option:selected').val();
        var status = $('#ContactStatus option:selected').val();
        var obj = { Name: name, Email: email, Phone: phone, Add: add, Create: create, Update: update, CUser: user, Team: team, Status: status, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/Contact/CreateCfromAccount",
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
        $('#CreateContact').modal('hide');

    }

    var oppCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var amount = document.getElementById("OpportunityAmount").value;
        var desc = document.getElementById("OpportunityDesc").value;
        var title = document.getElementById("OpportunityTitle").value;
        var user = $('#OpportunityUser option:selected').val();
        var team = $('#OpportunityTeam option:selected').val();
        var status = $('#OpportunityStatus option:selected').val();
        var prob = $('#OpportunityProbabilty option:selected').val();
        var stage = $('#OpportunityStage option:selected').val();
        var curr = $('#OpportunityCurrency option:selected').val();
        var obj = { Amount: amount, Desc: desc, Title: title, OUser: user, Team: team, Status: status, Prob: prob, Stage: stage, Curr: curr, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/Opportunities/CreateOfromAccount",
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
        $('#CreateModule').modal('hide');

    }

    var workorderCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var title = document.getElementById("WorkOrderTitle").value;
        var desc = document.getElementById("WorkOrderDesc").value;
        var nte = document.getElementById("WorkOrderNTE").value;
        var user = $('#WorkOrderUser option:selected').val();
        var team = $('#WorkOrderTeam option:selected').val();
        var status = $('#WorkOrderStatus option:selected').val();
        var stage = $('#WorkOrderStage option:selected').val();
        var obj = { Title: title, Desc: desc, NTE: nte, WOUser: user, Team: team, Status: status, Stage: stage, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/WorkOrders/CreateWOfromAccount",
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
        $('#CreateWorder').modal('hide');

    }

    var activityCreate = function () {
        var accID = document.getElementById("accID").textContent;

        var name = document.getElementById("ActivityName").value;
        var desc = document.getElementById("ActivityDesc").value;
        var party = document.getElementById("ActivityParty").value;
        var pointer = document.getElementById("ActivityPointer").value;
        var create = document.getElementById("ActivityCrtd").value;
        var cdate = document.getElementById("ActivityCDate").value;
        var update = document.getElementById("ActivityUpdate").value;
        var udate = document.getElementById("ActivityUDate").value;
        var user = $('#ActivityUser option:selected').val();
        var team = $('#ActivityTeam option:selected').val();
        var status = $('#ActivityStatus option:selected').val();
        var type = $('#ActivityType option:selected').val();
        var obj = { Name: name, Desc: desc, Party: party, Pointer: pointer, Create: create, CDate: cdate, Update: update, UDate: udate, AUser: user, Team: team, Status: status, Type: type, AccID: accID };
        $.ajax({
            type: "POST",
            url: "/Activities/CreateAfromAccount",
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
        $('#CreateActivity').modal('hide');

    }

    //Navigations
    var LocationurlD = function () {

        $('#Createdevice').modal('show');

    }

    var LocationurlConf = function () {
        var devName = document.getElementById("Dname").innerHTML;
        window.location = '/Devices/DeviceSensorGraph?DeviceName=' + devName;
    }

    var LocationurlWD = function () {
        var accID = document.getElementById("accID").textContent;
        var accName = document.getElementById("accName").textContent;
        var url = '/WorkFlows/WorkFlowDesigner?AccountId=' + accID + '&accountname=' + accName;
        var w = 800;
        var h = 600;
        var left = Number((screen.width / 2) - (w / 2));
        var tops = Number((screen.height / 2) - (h / 2));

        window.open(url, '', 'titlebar = no,toolbar=no ,location=no, directories=no,  status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + tops + ', left=' + left);
    }

    var LocationurlA = function () {
        $('#CreateAssets').modal('show');
    }

    var LocationurlL = function () {
        $('#CreateLocation').modal('show');
    }

    var LocationurlAc = function () {
        $('#CreateActivity').modal('show');
    }

    var LocationurlO = function () {
        $('#CreateModule').modal('show');
    }

    var LocationurlC = function () {
        $('#CreateContact').modal('show');
    }

    var LocationurlW = function () {
        $('#CreateWorder').modal('show');
    }


}
//Edit Page Script Ends

//Create Page Script Starts
if (url.includes("/accounts/create")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#AccountsCreateForm').submit();
    });

    $('#PhoneOffice').maxlength({
        warningClass: "m-badge m-badge--warning m-badge--rounded m-badge--wide",
        limitReachedClass: "m-badge m-badge--success m-badge--rounded m-badge--wide"
    });

    $('#Name').maxlength({
        threshold: 10,
        warningClass: "m-badge m-badge--danger m-badge--rounded m-badge--wide",
        limitReachedClass: "m-badge m-badge--success m-badge--rounded m-badge--wide",
        separator: ' of ',
        preText: 'You have ',
        postText: ' chars remaining.',
        validate: true
    });
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();

}
