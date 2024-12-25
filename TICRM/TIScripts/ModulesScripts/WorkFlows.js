/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Workflows scipt file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//Index Page Script Starts
if (url == "/workflows/index" || url == "/workflows") {
    $(document).ready(function () {
        $.fn.DataTable.ext.errMode = 'none';
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "bDestroy": true,
            "sAjaxSource": "/WorkFlows/GetWorkflowList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "TriggerCondition" },
                { "mData": "TriggerIn" },
                { "mData": "TriggerOut" },
                { "mData": "TargetOn" },
                { "mData": "Description" },
                { "mData": "AppliedTo" },
                { "mData": "Priority" },
                { "mData": "Frequency" },
                {
                    "mData": "Team.Name" },
                {
                    "mData": "User.Name" },
                { "mData": "Action" },
                {
                    "mData": function (o) {
                        return '<a href="/WorkFlows/Edit/' + o.WorkFlowId + '" title="Edit Work Flow"><i class="fa fa-edit"></i></a> | <a href="#" onclick="WorkFlows_Details_Modal(\'' + o.WorkFlowId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.WorkFlowId + '\')" class="arial" title="Delete Work Flow"><i class="fa fa-trash"></i></a>';
                    } },
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
            url: "/WorkFlows/PartialDeleteOnId",
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

//Edit Page Script Starts
if (url.includes("/workflows/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#WorkFlowsEditForm').submit();
    });


    //var BootstrapDatepicker = function () {

    //    return {
    //        init: function () {
    //            $("#TriggerIn").datepicker({
    //                //rtl: mUtil.isRTL(),
    //                todayBtn: "linked",
    //                clearBtn: !0,
    //                todayHighlight: !0,
    //                //templates: t,
    //                format: 'dd/mm/yyyy'
    //            }),
    //                $("#TriggerOut").datepicker({
    //                    rtl: mUtil.isRTL(),
    //                    todayBtn: "linked",
    //                    clearBtn: !0,
    //                    todayHighlight: !0,
    //                    //templates: t,
    //                    format: 'dd/mm/yyyy'
    //                })
    //        }
    //    }
    //}();

    jQuery(document).ready(function () {
        BootstrapDatepicker.init();
        $("#TriggerIn").datepicker("setDate", new Date('@Model.TriggerIn'));
        $("#TriggerOut").datepicker("setDate", new Date('@Model.TriggerOut'));
    });


    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/WorkFlows/PartialDetailsOnId",
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
//Create Page Script Starts
if (url == "/workflows/create") {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#WorkFlowsCreateForm').submit();
    });


    $(document).ready(function () {
        getURLLocation();
    });

    var getURLLocation = function () {

        var url = new URL(window.location.href);
        var id = url.searchParams.get("AccountId");
        console.log("1");
        console.log(id);
        console.log("1");
        if (id == null) {
            document.getElementById("loc").value = "False";
        }
        if (id != null) {
            document.getElementById("loc").value = id;
        }
    }

    //var BootstrapDatepicker = function () {
    //    var t;
    //    t = mUtil.isRTL() ? {
    //        leftArrow: '<i class="la la-angle-right"></i>', rightArrow: '<i class="la la-angle-left"></i>'
    //    }
    //        : {
    //            leftArrow: '<i class="la la-angle-left"></i>', rightArrow: '<i class="la la-angle-right"></i>'
    //        }
    //        ;
    //    return {
    //        init: function () {
    //            $("#TriggerIn").datepicker({
    //                rtl: mUtil.isRTL(),
    //                todayBtn: "linked",
    //                clearBtn: !0,
    //                todayHighlight: !0,
    //                //templates: t,
    //                format: 'dd/mm/yyyy'
    //            })
    //                ,
    //                $("#TriggerOut").datepicker({
    //                    rtl: mUtil.isRTL(),
    //                    todayBtn: "linked",
    //                    clearBtn: !0,
    //                    todayHighlight: !0,
    //                    //templates: t,
    //                    format: 'dd/mm/yyyy'
    //                })
    //        }
    //    }
    //}();

    jQuery(document).ready(function () {
        //BootstrapDatepicker.init();

    });
}
//Create Page Script Ends


//Delete Modal Script Starts
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}
//Delete Modal Script Ends
