/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Customer Assets script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/customerassets/index" || url == "/customerassets") {
    $(document).ready(function () {
        $.fn.DataTable.ext.errMode = 'none';
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/CustomerAssets/GetCustomerAssetList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Title" },
                { "mData": "Model" },
                { "mData": "Description" },
                { "mData": "Account.Name" },
                { "mData": "Location.Name" },
                { "mData": "Status.Name" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/CustomerAssets/Edit/' + o.CustomerAssetId + '" title="Edit Customer Asset"><i class="fa fa-edit"></i></a> | <a href="#" onclick="CustomerAssets_Details_Modal(\'' + o.CustomerAssetId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.CustomerAssetId + '\')" class="arial" title="Delete Customer Asset"><i class="fa fa-trash"></i></a>';
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
    });

    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch
        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");
        $.ajax({
            type: "GET",
            url: "/CustomerAssets/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                $('.modal-content-rightside').html('').html(response);
                $("#loader").css("display", "none");
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
if (url.includes("/customerassets/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#CustomerAssetsEditForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/CustomerAssets/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $('.modal-content').html('').html(response);

                $('#m_modal_Details').modal('show');
                //mApp.unblockPage();
                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                //mApp.unblockPage();
                $('body').removeClass('m-page--loading');
            }
        });
    }

    $('#AccountId').on('change', function () {
        LoadAccountLocation();
    });

    var LoadAccountLocation = function () {

        var accountId = $('#AccountId option:selected').val();

        var obj = { accountId: accountId }
        $.ajax({
            type: "GET",
            url: "/CustomerAssets/GetLocationOfAccount",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#LocationId').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#LocationId').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }
}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/customerassets/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/CustomerAssets/Index">Customer Assets</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "CustomerAssets", new { id = Model.CustomerAssetId })">Details @Model.Title</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/customerassets/delete")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/CustomerAssets/Index">Customer Assets</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "CustomerAssets", new { id = Model.CustomerAssetId })">Delete @Model.Title</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/customerassets/create")) {
    $(document).ready(function () {
        $('[id=status] option').filter(function () {
            return ($(this).text() == 'Active');
        }).prop('selected', true);
        var url = new URL(window.location.href);
        var name = url.searchParams.get("accountname");
        var id = url.searchParams.get("AccountId");
        getURLLocation();
        if (name != null) {
            debugger;
            $('[id=AccountId] option').filter(function () {
                return ($(this).text() == name);
            }).prop('selected', true);
            LoadAccountLocations(id);
        }
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
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#CustomerAssetsCreateForm').submit();
    });

    $('#AccountId').on('change', function () {
        LoadAccountLocation();
    });

    var LoadAccountLocation = function () {

        var accountId = $('#AccountId option:selected').val();
        var obj = { accountId: accountId }
        $.ajax({
            type: "GET",
            url: "/CustomerAssets/GetLocationOfAccount",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#LocationId').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#LocationId').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    var LoadAccountLocations = function (accountId) {

        var obj = { accountId: accountId }
        $.ajax({
            type: "GET",
            url: "/CustomerAssets/GetLocationOfAccount",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#LocationId').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#LocationId').append(options);
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