/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Leads script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/leads/index" || url == "/leads") {
    $(document).ready(function () {
        $.fn.DataTable.ext.errMode = 'none';
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Leads/GetLeadsList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "PhoneNumber" },
                { "mData": "Email" },
                { "mData": "Description" },
                { "mData": "Address.Street1" },
                { "mData": "Industry.Name" },
                { "mData": "LeadSource.Name" },
                { "mData": "LeadType.Name" },
                { "mData": "Status.Name" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/Leads/Edit/' + o.LeadId + '" title="Edit Lead"><i class="fa fa-edit"></i></a> | <a href="#" onclick="Leads_Details_Modal(\'' + o.LeadId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.LeadId + '\')" class="arial" title="Delete Lead"><i class="fa fa-trash"></i></a>';
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
            url: "/Leads/PartialDeleteOnId",
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

    var Autopopulating = function () {
        debugger;
        var serverUrl = Xrm.Page.context.getClientUrl();

        var queryUrl = "http://dummy.restapiexample.com/api/v1/employees";
        var req = new XMLHttpRequest();
        req.open("GET", queryUrl, false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.send();
        var results = JSON.parse(req.responseText).d.results;
        if (results != null && results.length > 0) {
            console.log(results);
        }
    }

    var Autopopulating = function () {
        // Define the data to create new account
        var accountData =
        {
            "name": "Arun Potti Inc.", // Single Line of Text
            "creditonhold": false, // Two Option Set
            "description": "This is the description of the sample account", // Multiple Lines of Text
            "revenue": 10000000, // Currency
            "industrycode": 1 // 1 - Accounting // OptionSet
        }

        // Create account record
        Xrm.WebApi.createRecord("account", accountData).then(
            function success(result) {
                // Show Account GUID
                Xrm.Utility.alertDialog("Account created with ID: " + result.id, null);
            },
            function (error) {
                // Show Error
                Xrm.Utility.alertDialog("Error :" + error.message, null);
            }
        );

        var serverUrl = Xrm.Page.context.getClientUrl();
        var queryUrl = "http://dummy.restapiexample.com/api/v1/employees";
        var req = new XMLHttpRequest();
        req.open("GET", queryUrl, false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.send();
    }

    function CreateAccountRecord() {

    }
}
//index Page Script Ends

//Edit Page Script Starts
if (url.includes("/leads/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#LeadEditForm').submit();
    });

    $('#PhoneNumber').maxlength({
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

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Leads/PartialDetailsOnId",
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
if (url.includes("/leads/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/Leads/Index">Leads</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "Leads", new { id = Model.LeadId })">Details @Model.Name</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/leads/delete")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/Leads/Index">Leads</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "Leads", new { id = Model.LeadId })">Delete @Model.Name</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/leads/create")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#LeadCreateForm').submit();
    });

    $('#PhoneNumber').maxlength({
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