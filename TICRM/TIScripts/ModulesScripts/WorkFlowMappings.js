/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Workflow Mappings scipt file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//Index Page Script Starts
if (url == "/workflowmappings/index" || url == "/workflowmappings") {
    var oTable = $('#m_table_1').DataTable({
        "bServerSide": true,
        "sAjaxSource": "/WorkFlowMappings/GetWorkflowMappingList",
        "sServerMethod": "POST",
        "aoColumns": [
            { "mData": "WorkFlow.Name" },
            { "mData": "SourceType" },
            { "mData": "Action" },
            { "mData": "IsDone" },
            { "mData": "CreatedDate" },
            { "mData": "CreatedBy" },
            { "mData": "UpdatedDate" },
            { "mData": "UpdatedBy" },
            {
                "mData": function (o) {
                    return '<a href="#" onclick="WorkFlowMappings_Details_Modal(\'' + o.WorkFlowMappingId + '\')" title="View Details"><i class="fa fa-eye"></i></a> | <a href="#" onclick="LoadModalForDelete(\'' + o.WorkFlowMappingId + '\')" class="arial" title="Delete Workflow Mapping"><i class="fa fa-trash"></i></a>';
                }
            },
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

  
    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch

        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
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
if (url == "/workflowmappings/create") {
    $('#savebtn').on('click', function () {
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#ServiceCallsCreateForm').submit();
    });

    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/WorkFlowMappings/Index">WorkFlow Mappings</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Create", "WorkFlowMappings")">Create</a></li>');
        $('#SourceValue').val('');
        $('#DivSourceValue').hide();
    });

    var Sourcedatatypes = [];

    $("#SourceType").change(function () {
        LoadInputField();
    });

    $("#Action").change(function () {

        var ActionValue = $('#Action').val();

        if (ActionValue == "" || ActionValue == "Please Select") {
            alert("Please Select Any Action");
        }
        else if (ActionValue == "Create") {

            $('#SourceValue').val('');
            $('#DivSourceValue').hide();
        }
        else if (ActionValue == "Update") {
            $('#SourceValue').val('');
            $('#DivSourceValue').show();
            LoadInputField();

        }

    });

    var LoadInputField = function () {

        if ($('#SourceType').val() == "" || $('#SourceType').val() == "Please Select") { return false; }
        Sourcedatatypes = [];
        if ($('#Action').val() == "Create") {



            var obj = { type: $('#SourceType').val() }
            $.ajax({
                type: "GET",
                url: "/WorkFlowMappings/GetWorkTypeValue",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    console.log(response);

                    Sourcedatatypes = response.DataTypes;


                    var Columnsdata = response.Columns;

                    $('#AppendDivInputFields').html('');
                    var inputField = '';
                    for (var i = 0; i < Columnsdata.length; i++) {

                        var column = Sourcedatatypes.filter(x => x.ColumnName === Columnsdata[i].Text);
                        if (column[0].DataType == "Guid") {
                            loadNewDDList(Columnsdata[i].Text);
                        }
                        else {
                            inputField += '<div class="form-group"><label class="control-label col-md-2" for="' + Columnsdata[i].Text + '">' + Columnsdata[i].Text + '</label>';
                            inputField += '<div class="col-md-10">';
                            inputField += '<input class="form-control text-box single-line" id="' + Columnsdata[i].Text + '" name="' + Columnsdata[i].Text + '" type="text" value="">';
                            inputField += '<span class="field-validation-valid text-danger" data-valmsg-for="' + Columnsdata[i].Text + '" data-valmsg-replace="true"></span></div></div>';
                        }

                    }
                    $('#AppendDivInputFields').append(inputField);

                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }
        else if ($('#Action').val() == "Update") {
            loadSourceValueDD();
            var obj = { type: $('#SourceType').val() }
            $.ajax({
                type: "GET",
                url: "/WorkFlowMappings/GetWorkTypeValue",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    console.log(response);

                    Sourcedatatypes = response.DataTypes;

                    var Columnsdata = response.Columns;

                    $('#AppendDivInputFields').html('');
                    $('#SourceValue').val('');
                    $('#DivSourceValue').show();

                    var inputField = '';
                    for (var i = 0; i < Columnsdata.length; i++) {

                        var column = Sourcedatatypes.filter(x => x.ColumnName === Columnsdata[i].Text);
                        if (column[0].DataType == "Guid") {
                            loadNewDDList(Columnsdata[i].Text);
                        }
                        else {
                            inputField += '<div class="form-group"><label class="control-label col-md-2" for="' + Columnsdata[i].Text + '">' + Columnsdata[i].Text + '</label>';
                            inputField += '<div class="col-md-10">';
                            inputField += '<input class="form-control text-box single-line" id="' + Columnsdata[i].Text + '" name="' + Columnsdata[i].Text + '" type="text" value="">';
                            inputField += '<span class="field-validation-valid text-danger" data-valmsg-for="' + Columnsdata[i].Text + '" data-valmsg-replace="true"></span></div></div>';
                        }
                    }

                    $('#AppendDivInputFields').append(inputField);

                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }
        else {
            alert("Please Select Any Action");
        }

    }

    var loadNewDDList = function (ColumnName) {
        var obj = { type: $('#SourceType').val(), column: ColumnName }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetDropDownOfSourceValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var options1 = '<div class="form-group"><label class="control-label col-md-2" for="' + ColumnName + '">' + ColumnName + '</label>';
                options1 += '<div class="col-md-10">';
                options1 += '<select class="form-control" id="' + ColumnName + '" name="' + ColumnName + '"></select>';
                options1 += '<span class="field-validation-valid text-danger" data-valmsg-for="' + ColumnName + '" data-valmsg-replace="true"></span></div></div>';

                $('#AppendDivInputFields').append(options1);

                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {

                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $("#" + ColumnName).append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    var loadSourceValueDD = function () {

        var sourceType = $('#SourceType').val();

        var columnName = "";
        if (sourceType == "Lead" || sourceType == "Account") {
            columnName = "Name";
        }

        var obj = { type: sourceType, column: columnName }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetDropDownOfSourceValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#SourceValue').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#SourceValue').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }


    $('#SourceValue').change(function () {

        var obj = { type: $('#SourceType').val(), Selected: $('#SourceValue').val() }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetObjectOnId",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                console.log(response);

                for (var i = 0; i < Sourcedatatypes.length; i++) {
                    $('#' + Sourcedatatypes[i].ColumnName).val(response[Sourcedatatypes[i].ColumnName]);
                }

            },
            failure: function () {
                alert("Failed!");
            }
        });
    });

    $('#btnSave').on('click', function () {
        var fields = {};
        $("form").find(":input").each(function () {
            // The selector will match buttons; if you want to filter
            // them out, check `this.tagName` and `this.type`; see
            // below
            fields[this.name] = $(this).val();
        });

        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#SourceData').val(JSON.stringify(fields));
        $('#CreateMappingForm').submit();
    });

}
//Create Page Script Ends

//Edit Page Script Starts
if (url.includes("/workflowreports/edit")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/WorkFlowMappings/Index">WorkFlowMappings</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Edit", "WorkFlowMappings", new { id = Model.WorkFlowMappingId })">Edit @Model.WorkFlow.Name</a></li>');

        GetSourceDatatype($('#SourceType').val(), 'Name');
        LoadInputField();


        if ('@Model.Action' == 'Update') {

            documentreadySourceValue('@Model.SourceValue');
            $('#DivSourceValue').show();
        }
        else {
            var data = JSON.parse($('#SourceData').val());
            for (var i = 0; i < Sourcedatatypes.length; i++) {
                var value = Sourcedatatypes[i].ColumnName;
                $('#' + Sourcedatatypes[i].ColumnName).val(data[Sourcedatatypes[i].ColumnName]);
            }
            $('#SourceData').val('');
            $('#DivSourceValue').hide();
        }


    });

    var Sourcedatatypes = [];

    var GetSourceDatatype = function (value, selectedValue) {

        var obj = { type: value }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetWorkTypeValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var Columnsdata = response.Columns;
                $('#SourceColumn').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < Columnsdata.length; i++) {
                    if (Columnsdata[i].Text == selectedValue) {
                        options += '<option selected="selected" value="' + Columnsdata[i].Value + '">' + Columnsdata[i].Text + '</option>';
                    }
                    else {
                        options += '<option value="' + Columnsdata[i].Value + '">' + Columnsdata[i].Text + '</option>';
                    }
                }
                $('#SourceColumn').append(options);
                Sourcedatatypes = response.DataTypes;
            },
            failure: function () {
                alert("Failed!");
            }
        });

    }

    $("#SourceType").change(function () {
        LoadInputField();
    });

    $("#Action").change(function () {

        var ActionValue = $('#Action').val();

        if (ActionValue == "" || ActionValue == "Please Select") {
            alert("Please Select Any Action");
        }
        else if (ActionValue == "Create") {

            $('#SourceValue').val('');
            $('#DivSourceValue').hide();
        }
        else if (ActionValue == "Update") {
            $('#SourceValue').val('');
            $('#DivSourceValue').show();
            LoadInputField();

        }

    });

    var LoadInputField = function () {

        if ($('#SourceType').val() == "" || $('#SourceType').val() == "Please Select") { return false; }
        Sourcedatatypes = [];
        if ($('#Action').val() == "Create") {

            var obj = { type: $('#SourceType').val() }
            $.ajax({
                type: "GET",
                url: "/WorkFlowMappings/GetWorkTypeValue",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    console.log(response);

                    Sourcedatatypes = response.DataTypes;


                    var Columnsdata = response.Columns;

                    $('#AppendDivInputFields').html('');
                    var inputField = '';
                    for (var i = 0; i < Columnsdata.length; i++) {

                        var column = Sourcedatatypes.filter(x => x.ColumnName === Columnsdata[i].Text);
                        if (column[0].DataType == "Guid") {
                            loadNewDDList(Columnsdata[i].Text);
                        }
                        else {
                            inputField += '<div class="form-group"><label class="control-label col-md-2" for="' + Columnsdata[i].Text + '">' + Columnsdata[i].Text + '</label>';
                            inputField += '<div class="col-md-10">';
                            inputField += '<input class="form-control text-box single-line" id="' + Columnsdata[i].Text + '" name="' + Columnsdata[i].Text + '" type="text" value="">';
                            inputField += '<span class="field-validation-valid text-danger" data-valmsg-for="' + Columnsdata[i].Text + '" data-valmsg-replace="true"></span></div></div>';
                        }

                    }
                    $('#AppendDivInputFields').append(inputField);

                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }
        else if ($('#Action').val() == "Update") {
            loadSourceValueDD();
            var obj = { type: $('#SourceType').val() }
            $.ajax({
                type: "GET",
                url: "/WorkFlowMappings/GetWorkTypeValue",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    console.log(response);

                    Sourcedatatypes = response.DataTypes;

                    var Columnsdata = response.Columns;

                    $('#AppendDivInputFields').html('');
                    $('#SourceValue').val('');
                    $('#DivSourceValue').show();

                    var inputField = '';
                    for (var i = 0; i < Columnsdata.length; i++) {

                        var column = Sourcedatatypes.filter(x => x.ColumnName === Columnsdata[i].Text);
                        if (column[0].DataType == "Guid") {
                            loadNewDDList(Columnsdata[i].Text);
                        }
                        else {
                            inputField += '<div class="form-group"><label class="control-label col-md-2" for="' + Columnsdata[i].Text + '">' + Columnsdata[i].Text + '</label>';
                            inputField += '<div class="col-md-10">';
                            inputField += '<input class="form-control text-box single-line" id="' + Columnsdata[i].Text + '" name="' + Columnsdata[i].Text + '" type="text" value="">';
                            inputField += '<span class="field-validation-valid text-danger" data-valmsg-for="' + Columnsdata[i].Text + '" data-valmsg-replace="true"></span></div></div>';
                        }
                    }

                    $('#AppendDivInputFields').append(inputField);

                },
                failure: function () {
                    alert("Failed!");
                }
            });
        }
        else {
            alert("Please Select Any Action");
        }

    }

    var loadNewDDList = function (ColumnName) {
        var obj = { type: $('#SourceType').val(), column: ColumnName }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetDropDownOfSourceValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var options1 = '<div class="form-group"><label class="control-label col-md-2" for="' + ColumnName + '">' + ColumnName + '</label>';
                options1 += '<div class="col-md-10">';
                options1 += '<select class="form-control" id="' + ColumnName + '" name="' + ColumnName + '"></select>';
                options1 += '<span class="field-validation-valid text-danger" data-valmsg-for="' + ColumnName + '" data-valmsg-replace="true"></span></div></div>';

                $('#AppendDivInputFields').append(options1);

                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {

                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $("#" + ColumnName).append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    var loadSourceValueDD = function () {

        var sourceType = $('#SourceType').val();

        var columnName = "";
        if (sourceType == "Lead" || sourceType == "Account") {
            columnName = "Name";
        }

        var obj = { type: sourceType, column: columnName }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetDropDownOfSourceValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#SourceValue').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {
                    options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                }
                $('#SourceValue').append(options);
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    $('#SourceValue').change(function () {
        loadSourceValues();
    });

    var loadSourceValues = function () {
        var obj = { type: $('#SourceType').val(), Selected: $('#SourceValue').val() }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetObjectOnId",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                for (var i = 0; i < Sourcedatatypes.length; i++) {
                    var value = Sourcedatatypes[i].ColumnName;
                    $('#' + Sourcedatatypes[i].ColumnName).val(response[Sourcedatatypes[i].ColumnName]);
                }

            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

    $('#btnSave').on('click', function () {
        var fields = {};
        $("form").find(":input").each(function () {
            fields[this.name] = $(this).val();
        });
        $('#SourceData').val(JSON.stringify(fields));
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        })
        $('#EditWorkFlowMappingForm').submit();
    });

    var documentreadySourceValue = function (Selected) {
        var obj = { type: $('#SourceType').val(), column: 'Name' }
        $.ajax({
            type: "GET",
            url: "/WorkFlowMappings/GetDropDownOfSourceValue",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                $('#SourceValue').html('');
                var options = '';
                options += '<option value="Select">Select</option>';
                for (var i = 0; i < response.length; i++) {

                    if (response[i].Value == Selected) {
                        options += '<option selected="selected" value="' + response[i].Value + '">' + response[i].Text + '</option>';
                    }
                    else {
                        options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                    }
                }

                $('#SourceValue').append(options);

                loadSourceValues();
            },
            failure: function () {
                alert("Failed!");
            }
        });
    }

}
//Edit Page Script Ends

//Details Page Script Starts
if (url.includes("/workflowreports/details")) {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/WorkFlowMappings/Index">ActivityTemplates</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "WorkFlowMappings", new { id = Model.WorkFlowMappingId })">@Model.WorkFlow.Name</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url.includes("/workflowreports/delete")) {
    $(document).ready(function () {
        //  $('#example1').dataTable();
        $('#searchNavigationList').append('<li><a href="/WorkFlowMappings/Index">WorkFlow Mappings</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "WorkFlowMappings", new { id = Model.WorkFlowMappingId })">@Model.WorkFlow.Name</a></li>');
    });
}
//Delete Page Script Ends

//Delete Call in Modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}
