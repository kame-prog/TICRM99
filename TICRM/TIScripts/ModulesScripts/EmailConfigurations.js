/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * EmailConfigurations script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/emailconfigurations/index" || url == "/emailconfigurations") {
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
            ],
            columnDefs: [
                {
                    targets: 5,
                    render: function (data, type, full, meta) {
                        var Status = {
                            Active: { 'title': 'Active', 'class': ' m-badge--success' },
                            InActive: { 'title': 'InActive', 'class': ' m-badge--danger' },
                        };
                        if (typeof Status[data] === 'undefined') {
                            return data;
                        }
                        return '<span class="m-badge ' + Status[data].class + ' m-badge--wide">' + Status[data].title + '</span>';
                    },
                }
            ],
        });
    });
    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/EmailConfigurations/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                //$('.modal-content').html('').html(response);
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
            url: "/EmailConfigurations/PartialDeleteOnId",
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
if (url.includes("/emailconfigurations/edit")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#EmailConfigurationsEditForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/EmailConfigurations/PartialDetailsOnId",
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
if (url == "/emailtemplates/Details") {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EmailConfigurations/Index">EmailConfigurations</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "EmailConfigurations", new { id = Model.EmailConfigurationId })">@Model.UserName</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url == "/emailtemplates/delete") {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EmailConfigurations/Index">Email Configurations</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "EventLogs", new { id = Model.EmailConfigurationId })">@Model.UserName</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if ((url.includes("/emailtemplates/create")) {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#EmailConfigurationsCreateForm').submit();
    });

    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('body').addClass('m-page--loading');

        $.ajax({
            type: "GET",
            url: "/EmailConfigurations/PartialDetailsOnId",
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
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}