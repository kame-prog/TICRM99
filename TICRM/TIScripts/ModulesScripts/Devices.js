/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Devices script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/devices/index" || url == "/devices") {
    //General Page Settings
    $(document).ready(function () {
        $.fn.DataTable.ext.errMode = 'none';
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Devices/GetDevicesList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "Mac" },
                { "mData": "EMEINumber" },
                { "mData": "RegistrationDate" },
                { "mData": "Account.Name" },
                { "mData": "CustomerAsset.Title" },
                { "mData": "Status.Name" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/Devices/Edit/' + o.DeviceId + '" title="Edit Devices"><i class="fa fa-edit"></i></a> | <a href="#" onclick="Devices_Details_Modal(\'' + o.DeviceId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.DeviceId + '\')" class="arial" title="Delete Devices"><i class="fa fa-trash"></i></a>';
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

        //For tracking map
        jQuery.support.cors = true;
        var host;
        var defaultKey = "f8be18e9-3ab7-4874-9520-b385c841d82f";
        var profile = "car";

        // create a routing client to fetch real routes, elevation.true is only supported for vehicle bike or foot
        var ghRouting = new GraphHopper.Routing({ key: defaultKey, host: host, vehicle: profile, elevation: false });

        var routingMap = createMaps('routing-map');
        setupRoutingAPI(routingMap, ghRouting);
            //End Map
    });

   

    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch
        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/Devices/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                //$('.modal-content').html('').html(response);
                $('.modal-content-rightside').html('').html(response);
                $("#loader").css("display", "none");
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }

    //Cloud Script
    var CloudSynchronize = function () {
        //$('#m_modal_IBM').modal('show');
        IBMCloudSynchronize();
    }

    var GetIBMCloudSynchronize = function () {

        $('body').addClass('m-page--loading');
        var CloudId = "IBM"
        var AccountId = $('#AccountId option:selected').val();
        var CustomerAssetId = $('#CustomerAssetId').val();
        var AssignedTeam = $('#AssignedTeam').val();
        var AssignedUser = $('#AssignedUser').val();
        $.ajax({
            type: "GET",
            url: "/Devices/IBMCloudSynchronized",
            data: { CloudId: CloudId, AccountId: AccountId, CustomerAssetId: CustomerAssetId, AssignedTeam: AssignedTeam, AssignedUser: AssignedUser },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                window.location.reload();

                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });

    }

    var IBMCloudSynchronize = function () {

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Devices/SynchronizedWithIBM",
            // data: { CloudId: CloudId, AccountId: AccountId, CustomerAssetId: CustomerAssetId, AssignedTeam: AssignedTeam, AssignedUser: AssignedUser },
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

    //Tracking
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

        var instructionsHeader = $("#instructions-header");
        instructionsHeader.click(function () {
            instructionsDiv.toggle();
        });

        var routingLayer = L.geoJson().addTo(map);
        routingLayer.options = {
            style: { color: "#00cc33", "weight": 5, "opacity": 0.6 }
        };
    }

    //Row Highlight and navigations
    var j = 1;
    var getListofDevices = function (mac) {
        $('#m_table_1 tbody tr').filter(function () {
            return $.trim($('td', this).eq(1).text()) == mac;
        }).css("background-color", "#90EE90").animate({ left: '10px' }, "slow").toggle({ fontSize: '2em' }, "slow").toggle({ fontSize: '1em' }, "slow");
    }

    var MACAddressSelected = function (value) {
        localStorage.setItem('selectedDevice', value);
        SaveMacAddress(value);
        window.location.href = "../Devices/device";
    }

    //  var data = event.target.value;
    //localStorage.setItem('selectedDevice', mac);
    //window.location.href = "../Devices/device";
    //}
}
//index Page Script Ends

//Edit Page Script Starts
if (url.includes("/devices/edit")) {
    $(document).ready(function () {
        if ($('#CloudServices').val() == "IBM") {
            $('#Name').attr('disabled', 'disabled');
        }
    });

    var Get_CloudService = function () {

        if ($('#CloudServices').val() == "IBM") {
            $('#m_modal_IBM').modal('show');
        }
    }

    $('#CloudServices').on('change', function () {
        Get_CloudService();
    });

    $('#RegistrationDate').datepicker();

    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        });
        $('#DevicesEditForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Devices/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $('.modal-content').html('').html(response);

                $('#m_modal_Details').modal('show');
                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }

    $('#AccountId').on('change', function () {
        LoadCustomerAssetsDD();
    });

    var LoadCustomerAssetsDD = function () {

        var accountId = $('#AccountId option:selected').val();

        var obj = { accountId: accountId }
        $.ajax({
            type: "GET",
            url: "/Devices/GetCustomerAssetsForDD",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#CustomerAssetId').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#CustomerAssetId').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }
}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/devices/details")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/Devices/Index">Devices</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "Devices",new { id = Model.DeviceId })">@Model.Name</a></li>');
    });
}
//Details Page Script Ends

//DeviceSensorGraph Page Script Starts
if (url == "/devices/devicesensorgraph") {
    $('#SensorIdDropDownList').on('change', function () {
        var SensorId = $("#SensorIdDropDownList option:selected").text();
        $("#SensorIdDropDownList option:selected").text();
        console.log("sensor");
        console.log(SensorId);
        console.log("sensor");
        if (SensorId == "Liquid Level") {
            $("#tankHeight").show();
        }
    });

    $('#m_table_1').DataTable({
        responsive: false,
        scrollY: false,
        scrollX: true,
        scrollCollapse: true,
        //== Pagination settings
        dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
                                        <'row'<'col-sm-12'tr>>
                                        <'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

        buttons: [
            'print',
            //'copyHtml5',
            //'excelHtml5',
            //'csvHtml5',
            'pdfHtml5',
        ]
    });

    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#DevicesCreateForm').submit();
    });

    $(function () {
        GetDeviceDropDownData();
        GetGraphDropDownData();
        GetSensorDropDownData();
        LoadDeviceSensorGraphList();
    });
    var isEditMode = false;

    function GetDeviceDropDownData() {
        $.ajax({
            type: "GET",
            url: "/Devices/GetDevicesDropdownList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#DeviceIdDropDownList').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].DeviceId + '">' + response[i].Name + '</option>';
                }
                $('#DeviceIdDropDownList').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });

    }

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

    function ResetDeviceSensorGraph() {
        $('#DeviceIdDropDownList').val('Select');
        $('#SensorIdDropDownList').val('Select');
        $('#GraphIdDropDownlist').val('Select');
        isEditMode = false;
        $('#submit_DeviceSensorTemplate').removeAttr('disabled');
        $('#submit_DeviceSensorTemplate').html('Submit');
    }

    $('#refresh_DeviceSensorTemplate').on("click", function () {
        ResetDeviceSensorGraph();
    });

    $('#submit_DeviceSensorTemplate').on("click", function () {

        $('#submit_DeviceSensorTemplate').attr('disabled', 'disabled');
        $('#submit_DeviceSensorTemplate').html('Please wait..');

        var DeviceId = $('#DeviceIdDropDownList').val();
        var SensorId = $('#SensorIdDropDownList').val();
        var GraphId = $('#GraphIdDropDownlist').val();
        var Data = $('#data').val();
        var Channel = $('#channel').val();
        var Network = $('#network').val();
        var level = $('#TankLevel').val();
        if (DeviceId == "Select" || DeviceId == null) {
            alert("Please Select a Device.");
            $('#submit_DeviceSensorTemplate').removeAttr('disabled');
            $('#submit_DeviceSensorTemplate').html('Submit');
            return false;
        }
        else if (SensorId == "Select" || SensorId == null) {
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
        if (isEditMode == true) {

            var DeviceSensorGraphId = $('#DeviceSensorGraphId').val();
            var obj = { DeviceSensorGraphId: DeviceSensorGraphId, DeviceId: DeviceId, SensorId: SensorId, GraphId: GraphId };
            $.ajax({
                type: "GET",
                url: "/Devices/UpdateDeviceSensorGraph",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {
                    if (response == "error") {
                        toastr.warning("Device Sensor Graph is not Saved. Please Refresh the page.");
                    }
                    else {
                        toastr.success("Device Sensor Graph is Saved successfully.");
                    }
                    LoadDeviceSensorGraphList();
                    ResetDeviceSensorGraph();
                },
                failure: function () {
                    alert("Failed!");
                }
            });

        }
        else {
            var obj = { DeviceId: DeviceId, SensorId: SensorId, GraphId: GraphId, Data: Data, Channel: Channel, Network: Network, Level: level };
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
                    }
                    LoadDeviceSensorGraphList();
                    ResetDeviceSensorGraph();
                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }

    });

    function LoadDeviceSensorGraphList() {
        $.ajax({
            type: "GET",
            url: "/Devices/GetDeviceSensorGraphList",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                $('#deviceSensorGridtbody').html('');
                var data = JSON.parse(response);
                var options = '';
                options += '<tr>';
                for (var i = 0; i < data.length; i++) {
                    options += '<tr>';
                    options += '<td>' + data[i].DeviceName + '</td>';
                    options += '<td>' + data[i].SensorName + '</td>';
                    options += '<td>' + data[i].GraphName + '</td>';
                    options += '<td>';
                    options += '<a href="#" onClick="EditDeviceSensorGraph(\'' + data[i].DeviceSensorGraphId + '\')" ><i class="fa fa-edit"></i></a>';
                    options += '&nbsp;|&nbsp;<a href="#" onClick="DeleteDeviceSensorGraph(\'' + data[i].DeviceSensorGraphId + '\')" ><i class="fa fa-trash"></i></a>';
                    options += '</td></tr>';
                }
                $('#deviceSensorGridtbody').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    function EditDeviceSensorGraph(value) {

        var obj = { DeviceSensorGraphId: value };
        $.ajax({
            type: "GET",
            url: "/Devices/EditDeviceSensorGraph",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#DeviceSensorGraphId').val(response.DeviceSensorGraphId);
                $('#DeviceIdDropDownList').val(response.DeviceId);
                $('#SensorIdDropDownList').val(response.SensorId);
                $('#GraphIdDropDownlist').val(response.GraphId);
                $('#submit_DeviceSensorTemplate').removeAttr('disabled');
                $('#submit_DeviceSensorTemplate').html('Update');
                isEditMode = true;
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    function DeleteDeviceSensorGraph(value) {
        var confirm = window.confirm('Are you sure you want to delete!!');
        if (confirm) {
            var obj = { DeviceSensorGraphId: value };
            $.ajax({
                type: "GET",
                url: "/Devices/DeleteDeviceSensorGraph",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (response) {
                    if (response == "error") {
                        alert("Device Sensor Graoh not Deleted. Please Refresh the page.");
                    }
                    LoadDeviceSensorGraphList();
                    ResetDeviceSensorGraph();
                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }
    }

    var url = new URL(window.location.href);
    var name = url.searchParams.get("DeviceName");
    if (name != null) {
        $('[id=DeviceIdDropDownList] option').filter(function () {
            return ($(this).text() == name);
        }).prop('selected', true);
    }
}
//DeviceSensorGraph Page Script Ends

//Delete Page Script Starts
if (url.includes("/devices/delete")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/Devices/Index">Devices</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "Devices",new { id = Model.DeviceId })">@Model.Name</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/devices/create")) {

    $("#IsGateway").click(function () {
        var checkbox = document.getElementById("IsGateway");
        var field = document.getElementById("gateway");
        if (checkbox.checked == true) {
            field.style.display = "block";
        }
        else
            field.style.display = "none";
    });

    //Get the url and check for account id
    $(document).ready(function () {

        $('[id=status] option').filter(function () {
            return ($(this).text() == 'Active');
        }).prop('selected', true);
        var url = new URL(window.location.href);
        var name = url.searchParams.get("accountname");
        var id = url.searchParams.get("AccountId");
        getURLLocation();
        if (name != null) {
            $('[id=AccountId] option').filter(function () {
                return ($(this).text() == name);
            }).prop('selected', true);
            LoadCustomerAssetsDDs(id);
        }
        getURLLocation();

    });

    var getURLLocation = function () {

        var url = new URL(window.location.href);
        var id = url.searchParams.get("AccountId");
        if (id == null) {
            document.getElementById("loc").value = "False";
        }
        if (id != null) {
            document.getElementById("loc").value = id;
        }

    }

    $('#Name').on('change', function () {
        if ($('#CloudServices').val() == "IBM") {
            $('#IBMCloud_DeviceId').attr('disabled', 'disabled');
            $('#IBMCloud_DeviceId').val($('#Name').val());
        }

    });

    var Get_CloudService = function () {

        if ($('#CloudServices').val() == "IBM") {
            $('#m_modal_IBM').modal('show');

            if ($('#CloudServices').val() == "IBM") {
                $('#IBMCloud_DeviceId').attr('disabled', 'disabled');
                $('#IBMCloud_DeviceId').val($('#Name').val());
            }

        }

    }

    $('#CloudServices').on('change', function () {
        Get_CloudService();
    });

    $('#RegistrationDate').datepicker();

    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        });
        $('#DevicesCreateForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Devices/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                $('.modal-content').html('').html(response);

                $('#m_modal_Details').modal('show');
                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }

    $('#AccountId').on('change', function () {
        LoadCustomerAssetsDD();
    });

    var LoadCustomerAssetsDD = function () {

        var accountId = $('#AccountId option:selected').val();

        var obj = { accountId: accountId }
        $.ajax({
            type: "GET",
            url: "/Devices/GetCustomerAssetsForDD",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#CustomerAssetId').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#CustomerAssetId').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    var LoadCustomerAssetsDDs = function (accountId) {


        var obj = { accountId: accountId }
        $.ajax({
            type: "GET",
            url: "/Devices/GetCustomerAssetsForDD",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#CustomerAssetId').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#CustomerAssetId').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

}
//Create Page Script Ends
//Create Page Script Starts
if (url.includes("/devices/device")) {
    $(document).ready(function () {
        GetGraphOfMAC();
        $('#HideAndShowLineChart').hide();
        // $('#HideAndShowMap').hide();
        $('#HideAndShowLightOnOff').hide();
        $('#HideAndShowLightOnOff1').hide();
        $('#HideAndShowLightOnOff2').hide();
        $('#sliderHideAndShowOnLight').hide();
        $('#HideAndShowMedical').hide();
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/Devices/Index">Devices</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("device", "Devices")">' + localStorage.getItem('selectedDevice') + '</a></li>');



        //For tracking map
        jQuery.support.cors = true;

        var host;
        var defaultKey = "f8be18e9-3ab7-4874-9520-b385c841d82f";
        var profile = "car";

        // create a routing client to fetch real routes, elevation.true is only supported for vehicle bike or foot
        var ghRouting = new GraphHopper.Routing({ key: defaultKey, host: host, vehicle: profile, elevation: false });


        var routingMap = createMap('routing-map');
        setupRoutingAPI(routingMap, ghRouting);
        //End Map

    });

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

    //function createMap(divId) {
    //    var osmAttr = '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors';

    //    var omniscale = L.tileLayer.wms('https://maps.omniscale.net/v2/swuich-2nd-key-c2867f47/style.default/map', {
    //        layers: 'osm',
    //        attribution: osmAttr + ', &copy; <a href="http://maps.omniscale.com/">Omniscale</a>'
    //    });

    //    var osm = L.tileLayer('https://maps.omniscale.net/v2/swuich-2nd-key-c2867f47/style.default/{z}/{x}/{y}.png', {
    //        attribution: osmAttr
    //    });

    //    var map = L.map(divId, { layers: [omniscale] });
    //    L.control.layers({
    //        "Omniscale": omniscale,
    //        "OpenStreetMap": osm
    //    }).addTo(map);
    //    return map;
    //}

    //Map tracking End


    function GetGraphOfMAC() {
        $('#HideAndShowLightOnOff1').hide();
        $('#HideAndShowLightOnOff2').hide();
        $('#HideAndShowMedical').hide();
        var MacAddress = localStorage.getItem('selectedDevice');

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

                    if (data[i].SensorName == "Location" && data[i].GraphName == "Map") {
                        $('#HideAndShowMap').show();
                        var mapCanvas = document.getElementById('map');

                        var myLatLng = { lat: data[i].Latitude, lng: data[i].Longitude };
                        var mapOptions = {
                            center: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
                            zoom: 15,
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        }
                        var map = new google.maps.Map(mapCanvas, mapOptions);
                        var marker = new google.maps.Marker({
                            position: myLatLng,
                            map: map,
                            title: data[i].Mac
                        });
                        google.maps.event.addDomListener(window, 'load');

                    }
                    if (data[i].SensorName == "Light" && data[i].GraphName == "On/Off") {
                        $('#HideAndShowLightOnOff1').show();
                        $('#HideAndShowLightOnOff2').show();
                    }


                    if (data[i].SensorName == "Heat rate / Fever" && data[i].GraphName == "Medical / Checkups") {

                        $('#HideAndShowMedical').show();
                    }

                }

            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    var dps = []; // dataPoints
    var chart = new CanvasJS.Chart("chartContainer", {
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
    var dataLength = 20; // number of dataPoints visible at any point

    var updateChart = function (Time, Temperature) {
        dps.push({
            x: Time,
            y: Temperature
        });
        if (dps.length > dataLength) {
            dps.shift();
        }
        chart.render();
    };

    $('#mydatepicker').hide();
    $('#datetimepicker1').datepicker();

    $("#commandDropDown").on('change', function () {
        //$("#deviceTitle").text(localStorage.getItem('selectedDevice'));
        if ($("#commandDropDown").val() == "Set Service Due Date") {

            $('#mydatepicker').show();
            $('#dtFrequncy').hide();
            $('#btnSendCommand').text('Save Service Date');
        }
        else {
            $('#mydatepicker').hide();
            $("#datetimepicker1").val("");
            $('#dtFrequncy').show();
            $('#btnSendCommand').text('Send Command');
        }
    });

    $("#deviceTitle").text(localStorage.getItem('selectedDevice'));


    $('#toggle_event_editing button').click(function () {

        var IsDeviceOn = false;

        if ($(this).hasClass('locked_active') || $(this).hasClass('unlocked_inactive')) {
            /* code to do when unlocking */
            $('#switch_status').html('Switched on.');
            $('#BulbOnOffId').removeAttr('style');
            $('#BulbOnOffId').attr('style', "font-size: 400px;color: hsla(50, 100%, 50%, 1);display:block;");
            $('#sliderHideAndShowOnLight').show();
            IsDeviceOn = true;
        } else {
            /* code to do when locking */
            $('#switch_status').html('Switched off.');
            $('#BulbOnOffId').removeAttr('style');
            $('#BulbOnOffId').attr('style', "font-size: 400px;color: hsla(50, 0%, 50%, 1);display:block;");
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
        $('#BulbOnOffId').attr('style', "font-size: 400px;color: hsla(50, " + this.value + "%, 50%, 1);display:block;");
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

}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}