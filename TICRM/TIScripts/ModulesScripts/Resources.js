/**
 * 
 * 24/7/2020 Resources script file Containing all the scripts it use Akhtar Zaman
 * 
 */
var url = window.location.pathname.toLowerCase();
//index Page Script Starts
if (url == "/resources/index" || url == "/resources") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Resources/GetResourcesList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "PhoneHome" },
                { "mData": "Email" },
                { "mData": "Website" },
                { "mData": "PhoneOffice" },
                { "mData": "Description" },
                { "mData": "Status.Name" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                { "mData": "Address1.Street1" },
                { "mData": "Address2.Street1" },
                {
                    "mData": function (o) {
                        return '<a href="/Resources/Edit/' + o.ResourceId + '" title="Edit Activity"><i class="fa fa-edit"></i></a> | <a href="#" onclick="Resources_Details_Modal(\'' + o.ResourceId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.ResourceId + '\')" class="arial" title="Delete Activity"><i class="fa fa-trash"></i></a>';
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

    

    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/Resources/PartialDeleteOnId",
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
}
//index Page Script Ends

//Edit Page Script Starts
if (url.includes("/Resources/Edit") || url.includes("/resources/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ResourcesEditForm').submit();
    });


    //NOTE: don't rename it LoadModalForDetails and its Get URL in Ajax
    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Resources/PartialDetailsOnId",
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
if (url.includes("/Resources/Details") || url.includes("/resources/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/Resources/Index">Resources</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "Resources", new { id = Model.ResourceId })">Details @Model.Name</a></li>');

    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/Resources/Delete") || url.includes("/resources/delete")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/Resources/Index">Resources</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "Resources", new { id = Model.ResourceId })">Delete @Model.Name</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url == "/Resources/Create" || url == "/resources/create") {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ResourcesCreateForm').submit();
    });
}
//Create Page Script Ends

//Delete item in modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}