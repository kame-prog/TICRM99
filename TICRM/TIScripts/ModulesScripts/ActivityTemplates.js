/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * ActivityTemplates script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/activitytemplates/index" || url == "/activitytemplates") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/ActivityTemplates/GetActivitesList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "ActivityTemplateType" },
                { "mData": "PropertyName" },
                { "mData": "PropertyValue" },
                { "mData": "PropertyType" },
                {"mData": "CreatedDate" },
                { "mData": "CreatedBy" },
                { "mData": "UpdatedDate" },
                { "mData": "UpdatedBy" },
                {
                    "mData": function (o) {
                        return '<a href="/ActivityTemplates/Edit/' + o.ActivityTemplateId + '" title="Edit Activity"><i class="fa fa-edit"></i></a> | <a href="#" onclick="LoadModalForDetails(\'' + o.ActivityTemplateId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.ActivityTemplateId + '\')" class="arial" title="Delete Activity"><i class="fa fa-trash"></i></a>';
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

        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/ActivityTemplates/PartialDetailsOnId",
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

    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/ActivityTemplates/PartialDeleteOnId",
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
if (url.includes("/activitytemplates/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ActivityTemplatesEditForm').submit();
    });

}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/activitytemplates/details")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/ActivityTemplates/Index">ActivityTemplates</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "ActivityTemplates", new { id = Model.ActivityTemplateId })">@Model.PropertyType</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/activitytemplates/delete")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/ActivityTemplates/Index">ActivityTemplates</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "ActivityTemplates", new { id = Model.ActivityTemplateId })">@Model.PropertyType</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/activitytemplates/create")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ActivityTemplatesCreateForm').submit();
    });
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}