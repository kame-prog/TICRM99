/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Workflow Reports scipt file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//Index Page Script Starts
if (url == "/workflowreports/index" || url == "/workflowreports") {
    $(document).ready(function () {
       
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "bDestroy": true,
            "sAjaxSource": "/WorkFlows/GetWorkflowList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "WorkFlow.Name" },
                { "mData": "Action" },
                { "mData": "WorkFlowStatus" },
                { "mData": "WorkFlowActionStatus" },
                { "mData": "AppliedTo" },
                { "mData": "Frequency" },
                { "mData": "Priority" },
                { "mData": "CreatedDate" },
                { "mData": "CreatedBy.Name" },
                {
                    "mData": function (o) {
                        return '<a href="#" onclick="WorkFlowReports_Details_Modal(\'' + o.WorkFlowReportId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.WorkFlowReportId + '\')" class="arial" title="Delete Activity"><i class="fa fa-trash"></i></a>';
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
   
}
//Index Page Script Ends

//Edit Page Script Starts
if (url.includes("/workflowreports/edit")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/WorkFlowReports/Index">WorkFlow Reports</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Edit", "WorkFlowReports", new { id = Model.WorkFlowId })">Edit @Model.Action</a></li>');
    });
}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/workflowreports/details")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/WorkFlowReports/Index">WorkFlowReports</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "WorkFlowReports", new { id = Model.WorkFlowId })">@Model.Action</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/workflowreports/delete")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/WorkFlowReports/Index">WorkFlow Reports</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "WorkFlowReports", new { id = Model.WorkFlowId })">@Model.Action</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url == "/workflowreports/create") {
    $(document).ready(function () {
        var url = new URL(window.location.href);
        var id = url.searchParams.get("AccountId");
        document.getElementById("loc").value = id;
        $('#searchNavigationList').append('<li><a href="/WorkFlowReports/Index">WorkFlow Reports</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Create", "WorkFlowReports")">Create WorkFlowReports</a></li>');
    });
}
//Create Page Script Ends

//Deleting Record in the modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}