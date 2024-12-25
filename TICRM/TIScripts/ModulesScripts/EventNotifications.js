/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Event Notifications script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/eventnotifications/index" || url == "/eventnotifications") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/EventNotifications/GetEventNotificationList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "Message" },
                { "mData": "Color" },
                { "mData": "IPAddress" },
                { "mData": "CreatedDate" },
                { "mData": "CreatedBy" },
            ],
            responsive: true,
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
}
//index Page Script Ends


//Edit Page Script Starts
if (url.includes("/eventnotifications/edit")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EventNotifications/Index">EventNotifications</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Edit", "EventNotifications", new { id = Model.EventNotificationId })">Edit @Model.Name</a></li>');
    });
}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/eventnotifications/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EventNotifications/Index">EventNotifications</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "EventNotifications", new { id = Model.EventNotificationId })">@Model.Name</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/eventnotifications/delete")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/EventNotifications/Index">EventNotifications</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "EventLogs", new { id = Model.EventNotificationId })">@Model.Name</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/eventnotifications/create")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EventNotifications/Index">EventNotifications</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Create", "EventNotifications")">Create</a></li>');
    });
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}