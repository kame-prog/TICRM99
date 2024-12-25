/**
 * Code By Akhtar Zaman
 * 15/9/2020
 * Cases script file Containing all the scripts it use
 */
//index Page Script Starts
var url = window.location.pathname.toLowerCase();

if (url == "/cases/index" || url == "/cases") {
    $(document).ready(function () {

        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Cases/GetCasesList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "CaseTitle" },
                { "mData": "Description" },
                { "mData": "RelatedTo" },
                { "mData": "Team.Name" },
                { "mData": "User.Name" },
                { "mData": "CaseTypeDto.Name" },
                { "mData": "CaseStatusDto.Name" },
                { "mData": "ContactDto.Name" },
                {
                    "mData": function (o) {
                        return '<a href="/Cases/Edit/' + o.CaseId + '" title="Edit Case"><i class="fa fa-edit"></i></a> | <a href="#" onclick="LoadModalForDetails(\'' + o.CaseId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.CaseId + '\')" class="arial" title="Delete Activity"><i class="fa fa-trash"></i></a>';

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
        debugger;
        $.ajax({
            type: "GET",
            url: "/Cases/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            
            success: function (response) {
                debugger;
                //$('.modal-content').html('').html(response);
                $('.modal-content-rightside').html('').html(response);
                $("#loader").css("display", "none");

            },

            failure: function () {
                alert("Failed!");
                $("#loader").css("display", "none");
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
            url: "/Cases/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $('.modal-content-rightside').html('').html(response);
                $("#loader").css("display", "none");
            },
            failure: function () {
                alert("Failed!");
                $("#loader").css("display", "none");
            }
        });
    }
  
}
//index Page Script Ends


//Edit Page Script Starts
if (url.includes("/accounts/edit")) {

}
//Edit Page Script Ends


//Create Page Script Starts
if (url.includes("/cases/create")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#CaseCreateForm').submit();
    });

    $("#RelatedTo").change(function () {
        var obj = { value: $('#RelatedTo').val() }

        $.ajax({
            type: "GET",
            url: "/Activities/GetRelatedToData",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#RelatedToId').html('');
                var options = '';
                console.log("Ajax");
                console.log(response);
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                console.log(options);
                $('#RelatedToId').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    });

    
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}


