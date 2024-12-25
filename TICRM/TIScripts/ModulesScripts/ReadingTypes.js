/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * ReadingTypes script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/readingtypes/index" || url == "/readingtypes") {
    $(document).ready(function () {
      $('#m_table_1').DataTable({
          responsive: true,
            scrollY: false,
            scrollX: true,
            scrollCollapse: true,
            //== Pagination settings
            dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
                                    <'row'<'col-sm-12'tr>>
                                    <'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

            buttons: [
                'print',
                'pdfHtml5',
            ]
        });
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch
        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/ReadingTypes/PartialDetailsOnId",
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
            url: "/ReadingTypes/PartialDeleteOnId",
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
if (url.includes("/readingtypes/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ReadingTypesEditForm').submit();
    });

    //NOTE: don't rename it LoadModalForDetails and its Get URL in Ajax
    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/ReadingTypes/PartialDetailsOnId",
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
if (url.includes("/readingtypes/Details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/ReadingTypes">Reading Types</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "ReadingTypes", new { id = Model.ReadingTypeId })">Details @Model.Name</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/readingtypes/delete")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/ReadingTypes">Reading Types</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "ReadingTypes", new { id = Model.ReadingTypeId })">Delete @Model.Name</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url == "/readingtypes/create") {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ReadingTypesCreateForm').submit();
    });

}
//Create Page Script Ends

//Delete from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}