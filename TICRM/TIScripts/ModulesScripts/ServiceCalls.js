/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * ServiceCalls script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/servicecalls/index" || url == "/servicecalls") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/ServiceCalls/GetServiceCallList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Title" },
                { "mData": "Detail" },
                { "mData": "Description" },
                { "mData": "Status.Name" },
                { "mData": "Team.Name" },
                { "mData": "Urgency.Name" },
                { "mData": "User.Name" },
                { "mData": "WorkStage.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/ServiceCalls/Edit/' + o.ServiceCallId + '" title="Edit Servjce Call"><i class="fa fa-edit"></i></a> | <a href="#" onclick="ServiceCalls_Details_Modal(\'' + o.ServiceCallId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.ServiceCallId + '\')" class="arial" title="Delete Servjce Call"><i class="fa fa-trash"></i></a>';
                    }
                },
            ],
            responsive: true,
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
            url: "/ServiceCalls/PartialDeleteOnId",
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
if (url.includes("/servicecalls/edit")) {

    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ServiceCallsEditForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/ServiceCalls/PartialDetailsOnId",
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

}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/servicecalls/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/ServiceCalls">Service Calls</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "ServiceCalls", new { id = Model.ServiceCallId })">Details @Model.Title</a></li>');

    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/servicecalls/delete")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/ServiceCalls/Index">Service Calls</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "ServiceCalls", new { id = Model.ServiceCallId })">Delete @Model.Title</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url == "/servicecalls/create") {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ServiceCallsCreateForm').submit();
    });
}
//Create Page Script Ends

//Deleting item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}