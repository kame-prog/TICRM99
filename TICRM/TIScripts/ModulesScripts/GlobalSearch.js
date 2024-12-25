/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Global Search script file Containing all the scripts it use
 */

//index Page Script Starts

$(function () {
    LoadGlobalSearchList();
});

function LoadGlobalSearchList() {
    $('#m_datatable_latest_orders1').html('');

    $('.m_datatable1').mDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: '../GlobalSearch/GetGlobalSearchList'
                }
            },
            pageSize: 10,
            saveState: {
                cookie: false,
                webstorage: true
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        },

        layout: {
            theme: 'default',
            class: '',
            scroll: true,
            //height: 380,
            footer: false
        },

        sortable: true,

        filterable: false,

        pagination: true,

        columns: [{
            field: "Name",
            title: "Name",
            sortable: 'asc',
            filterable: false,
            //width: 150
        },
        {
            field: "URL",
            title: "URL",
        },
        {
            field: "Type",
            title: "Type"
        },
        {
            field: "Actions",
            width: 50,
            title: "Actions",
            sortable: false,
            overflow: 'visible',
            template: function (row, index, datatable) {
                var dropup = (datatable.getPageSize() - index) <= 4 ? 'dropup' : '';
                return '\
                                                        <a href="#" onClick="EditGlobalSearch(\'' + row.GlobalSearchId + '\')" title="Edit Global Search"><i class="fa fa-edit"></i></a> | <a href="#" onClick="DeleteGlobalSearh(\'' + row.GlobalSearchId + '\')" title="Delete Global Search" ><i class="fa fa-trash"></i></a>\
                                                    ';
            }
        }
        ]
    });


}

var isEditMode = false;

function ResetGlobalSearch() {
    $('#GlobalSearchId').val('');
    $('#Name').val('');
    $('#URL').val('');
    isEditMode = false;
    $('#SubmitGlobalSearchId').removeAttr('disabled');
    $('#SubmitGlobalSearchId').html('Submit');
}

$('#refresh_GlobalSearch').on("click", function () {
    ResetGlobalSearch();
});

$('#SubmitGlobalSearchId').on("click", function () {

    var GlobalSearchId = $('#GlobalSearchId').val();

    var Name = $('#Name').val();
    var URL = $('#URL').val();

    var patt1 = /(\/)\w+/g;
    var result = URL.match(patt1);
    if (!result) {
        var path2 = /(\/)/g;
        var result1 = URL.match(path2);
        if (!result1) {
            alert("Please Insert URL start with Charector '/' ");
            return false;
        }
    }

    $('#SubmitGlobalSearchId').attr('disabled', 'disabled');
    $('#SubmitGlobalSearchId').html('Please wait..');

    if (isEditMode == true) {
        var obj = { GlobalSearchId: GlobalSearchId, Name: Name, URL: URL };

        $.ajax({
            type: "GET",
            url: "/GlobalSearch/UpdateGlobalSearch",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                if (response == "error") {
                    alert("Global/Cammand Search Data not Saved. Please Refresh the page.");
                }
                window.location.reload();
                ResetGlobalSearch();
            },
            failure: function () {
                alert("Failed!");
            }
        });

    }
    else {
        var obj = { Name: Name, URL: URL };

        $.ajax({
            type: "GET",
            url: "/GlobalSearch/SubmitGlobalSearch",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                if (response == "error") {
                    alert("Global/Cammand Search Data not Saved. Please Refresh the page.");
                }
                window.location.reload();
                ResetGlobalSearch();
            },
            failure: function () {
                alert("Failed!");
            }
        });

    }

});

function EditGlobalSearch(value) {

    var obj = { GlobalSearchId: value };
    $.ajax({
        type: "GET",
        url: "/GlobalSearch/EditGlobalSearch",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            $('#GlobalSearchId').val(response.GlobalSearchId);
            $('#Name').val(response.Name);
            $('#URL').val(response.URL);

            $('#SubmitGlobalSearchId').removeAttr('disabled');
            $('#SubmitGlobalSearchId').html('Update');
            isEditMode = true;
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

function DeleteGlobalSearh(value) {
    var confirm = window.confirm('Are you sure you want to delete!!');
    if (confirm) {
        var obj = { GlobalSearchId: value };
        $.ajax({
            type: "GET",
            url: "/GlobalSearch/DeleteGlobalSearch",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                if (response == "error") {
                    alert("Global/Cammand Search not Deleted. Please Refresh the page.");
                }
                window.location.reload();
                ResetGlobalSearch();
                ClearSearchSession();
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }
}