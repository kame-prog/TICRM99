/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Opportunities script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/opportunities/index" || url == "/opportunities") {
    $(document).ready(function () {
        $.fn.DataTable.ext.errMode = 'none';
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Opportunities/GetopportunitiesList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Title" },
                { "mData": "Amount" },
                { "mData": "Description" },
                { "mData": "Currency.Name" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                { "mData": "OpportunityStage.Name" },
                { "mData": "Pobability.Name" },
                { "mData": "Status.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/Opportunities/Edit/' + o.OpportunityId + '" title="Edit Opportunity"><i class="fa fa-edit"></i></a> | <a href="#" onclick="Opportunities_Details_Modal(\'' + o.OpportunityId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.OpportunityId + '\')" class="arial" title="Delete Opportunity"><i class="fa fa-trash"></i></a>';
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
            url: "/Opportunities/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                $("#m_modal_Delete").children('.modal-dialog.modal-rightside-dialog.modal-RightSide-slideout').children('.modal-content').html('').html(response);

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
if (url.includes("/opportunities/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#OpportunityEditForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch
        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/Opportunities/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                console.log(response);

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
if (url.includes("/opportunities/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/Opportunities/Index">Opportunities</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "Opportunities", new { id = Model.OpportunityId })">Details @Model.Title</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/opportunities/delete")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/Opportunities/Index">Opportunities</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "Opportunities", new { id = Model.OpportunityId })">Delete @Model.Title</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url.includes("/opportunities/create")) {
    $(document).ready(function () {
        var url = new URL(window.location.href);
        var name = url.searchParams.get("accountname");
        $('[id=AccountId] option').filter(function () {
            return ($(this).text() == name); 
        }).prop('selected', true);
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
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#OpportunityCreateForm').submit();
    });
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}