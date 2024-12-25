/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Workorder's scipt file Containing all the scripts workorders use
 */
var url = window.location.pathname.toLowerCase();

//Index Page Script Starts

if (url == "/workorders/index" || url == "/workorders") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/WorkOrders/GetWorkOrdersList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Title" },
                { "mData": "NTE" },
                { "mData": "Description" },
                { "mData": "Status.Name" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                { "mData": "WorkStage.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/Workorders/Edit/' + o.WorkOrderId + '" title="Edit WorkOrders"><i class="fa fa-edit"></i></a> | <a href="#" onclick="WorkOrders_Details_Modal(\'' + o.WorkOrderId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.WorkOrderId + '\')" class="arial" title="Delete WorkOrders"><i class="fa fa-trash"></i></a>';
                    }
                },
            ],
            responsive: true,
            scrollY: false,
            scrollX: true,
            scrollCollapse: true,
            "bDestroy": true,
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
            url: "/WorkOrders/PartialDeleteOnId",
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
//Index Page Script Ends

//Create Page Script Starts
if (url == "/workorders/create") {
    $(document).ready(function () {
        console.log("docReadyCreate");
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
        //console.log("savebutton");
        $('#WorkOrdersCreateForm').submit();
    });
}
//Create Page Script Ends


//Edit Page Script Starts
if (url.includes("/workorders/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#WorkOrdersEditForm').submit();
    });
}
//Edit Page Script Ends

//Delete Modal Script Starts
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}
//Delete Modal Script Ends
