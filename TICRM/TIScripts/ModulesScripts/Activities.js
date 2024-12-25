/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Activities script file Containing all the scripts it use
 */
//index Page Script Starts
var url = window.location.pathname.toLowerCase();


if (url == "/activities/index" || url == "/activities") {
    $(document).ready(function () {
        var oTable = $('#m_table_1').DataTable({
            "bServerSide": true,
            "sAjaxSource": "/Activities/GetActivitesList",
            "sServerMethod": "POST",
            "aoColumns": [
                { "mData": "Name" },
                { "mData": "Description" },
                { "mData": "RelatedTo" },
                { "mData": "RelatedToName" },
                {
                    "mData": "Status.Name"
                },
                { "mData": "CreatedDate" },
                {
                    "mData": function (o) {
                        return o.Team.Name;
                    }
                },
                {
                    "mData": function (o) {
                        return o.User.Name;
                    }
                },
                {
                    "mData": function (o) {
                        return '<a href="/Activities/Edit/' + o.ActivityId + '" title="Edit Activity"><i class="fa fa-edit"></i></a> | <a href="#" onclick="LoadModalForDetails(\'' + o.ActivityId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.ActivityId + '\')" class="arial" title="Delete Activity"><i class="fa fa-trash"></i></a>';
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
            url: "/Activities/PartialDetailsOnId",
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
            url: "/Activities/PartialDeleteOnId",
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
if (url.includes("/activities/edit")) {
    console.log("editpage");
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ActivitiesEditForm').submit();
    });

    var LoadDropDownRelatedToID = function (Selectedvalue) {
        debugger;
        var obj = { value: $('#RelatedTo').val(), selectedvalue: Selectedvalue}

        $.ajax({
            type: "GET",
            url: "/Activities/GetRelatedToValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#RelatedToID').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {


                    if (response[i].Selected == true) {
                        options += '<option selected="selected" value="' + response[i].Value + '">' + response[i].Text + '</option>';
                    }
                    else {
                        options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                    }

                }
                $('#RelatedToID').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });

    }

}
//Edit Page Script Ends

//Create Page Script Starts
if (url.includes("/activities/create")) {
    $(document).ready(function () {
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
        $('#ActivitiesCreateForm').submit();
    });


}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();

}

$("#RelatedTo").change(function () {
    var obj = { value: $('#RelatedTo').val() }
   
    $.ajax({
        type: "GET",
        url: "/Activities/GetRelatedToData",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#RelatedToID').html('');
            var options = '';
            //options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#RelatedToID').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
});