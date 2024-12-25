/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Event Monitors script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/eventmonitors/index" || url == "/eventmonitors") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/EventMonitors/GetEventMonitorList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "Type" },
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
if (url.includes("/eventmonitors/edit")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EventMonitors/Index">Event Monitors</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Edit", "EventMonitors", new { id = Model.EventMonitorId })">Edit @Model.Name</a></li>');
    });
}
//Edit Page Script Ends

//Details Page Script Starts
if (url == "/eventmonitors/details") {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EventMonitors/Index">Event Monitors</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "EventMonitors", new { id = Model.EventMonitorId })">@Model.Name</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url == "/eventmonitors/delete") {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/EventMonitors/Index">Event Monitors</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "EventMonitors", new { id = Model.EventMonitorId })">@Model.Name</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/eventmonitors/create")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EventMonitors/Index">Event Monitors</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Create", "EventMonitors")">Create</a></li>');
    });
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}