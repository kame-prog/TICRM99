var checkbox = function () {
    var checkbox = document.getElementById("checkbox");
    var field = document.getElementById("crm");
    if (checkbox.checked == true) {
        field.style.display = "";
    }
    else
        field.style.display = "none";
}

function Upload() {
    //Reference the FileUpload element.
    $("#global").show();

    var fileUpload = document.getElementById("fileUpload");
    //Validate whether File is valid Excel file.
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$/;
    if (regex.test(fileUpload.value.toLowerCase())) {
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            //For Browsers other than IE.
            if (reader.readAsBinaryString) {
                reader.onload = function (e) {
                    ProcessExcel(e.target.result);
                };
                reader.readAsBinaryString(fileUpload.files[0]);
            } else {
                //For IE Browser.
                reader.onload = function (e) {
                    var data = "";
                    var bytes = new Uint8Array(e.target.result);
                    for (var i = 0; i < bytes.byteLength; i++) {
                        data += String.fromCharCode(bytes[i]);
                    }
                    ProcessExcel(data);
                };
                reader.readAsArrayBuffer(fileUpload.files[0]);
            }
        } else {
            alert("This browser does not support HTML5.");
        }
    } else {
        alert("Please upload a valid Excel file.");
    }
};
function ProcessExcel(data) {
    //Read the Excel File data.
    var workbook = XLSX.read(data, {
        type: 'binary'
    });
    //Fetch the name of First Sheet.
    var firstSheet = workbook.SheetNames[0];
    //Read all rows from First Sheet into an JSON array.
    var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
    //Create a HTML Table element.

    //Add the data rows from Excel file.

    var options = '';

    for (var i = 0; i < excelRows.length; i++) {
        options += '<tr id="row' + excelRows[i].Id + '">';
        options += '<td ><div class="form-group""><input name="DeviceSensorGraphId" id="deviceName' + excelRows[i].Id + '" class="form - control" /></div ></td>';
        options += '<td >' + excelRows[i].Mac + '</td>';
        options += '<td >' + excelRows[i].EMEI + '</td>';
        options += '<td><div class="form-group"><select onClick="cellSeleccted(\'' + excelRows[i].Id + '\')" name="account" id="accountDevices\'' + excelRows[i].Id + '\'" class="form-control"></select></div ></td>';
        options += '<td><div class="form-group"><select name="asset" id="assetDevices" class="form-control"></select></div ></td>';
        options += '<td>';
        options += '<button type="button" onClick="publishdata(\'' + excelRows[i].Id + '\')" class="btn btn-xs btn-info">Add</button>';
        options += '&nbsp;<button type="button" onClick="dropData(\'' + excelRows[i].Id + '\')" class="btn btn-xs btn-danger">Drop</button>';
        options += '</td></tr>';
    }
    $('#deviceSensorGridtbody').append(options);
    for (var j = 0; j < excelRows.length; j++) {
        var id = "accountDevices'" + excelRows[j].Id + "'";
        var dom = document.getElementById(id);
        $('#acc option').clone().appendTo(dom);

    }
    length = excelRows.length;
};

function cellSeleccted(row) {
    var id = "accountDevices'" + row + "'";
    var dom = document.getElementById(id).value;
    if (id != "") {
        LoadCustomerAssetsDD(dom, row);
    }
}

function dropData(row) {
    var rowD = "row" + row;
    document.getElementById("m_table_1").deleteRow(rowD);
}

function publishdata(row) {
    var id = "row" + row;
    var dom = document.getElementById(id);
    var acc = "accountDevices'" + row + "'";

    var accid = document.getElementById(acc);
    var nameD = "deviceName" + row;
    var name = document.getElementById(nameD).value;
    var mac = document.getElementById(id).cells.item(1).innerHTML;
    var emei = document.getElementById(id).cells.item(2).innerHTML;
    var accD = document.getElementById(acc).value;
    //var accD = document.getElementById("accountDevices'1'").value;
    var assD = document.getElementById("assetDevices").value;


    var obj = { Name: name, Mac: mac, EMEI: emei, Asset: assD, AccID: accD };
    $.ajax({
        type: "POST",
        url: "/Devices/BulkCreate",
        data: obj,
        dataType: "text",
        success: function (response) {
            if (response == "error") {
                ("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }

        },
        failure: function () {
            alert("Failed!");
        }
    })
    document.getElementById("m_table_1").deleteRow(id);


}
var LoadCustomerAssetsDD = function (id, row) {

    var accountId = id;
    var assId = "assetDevices" + row;
    var obj = { accountId: accountId }
    $.ajax({
        type: "GET",
        url: "/BulkDevices/GetCustomerAssetsForDD",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            //document.getElementById(assId).appendChild(options);
            $('#assetDevices').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}
$('#AccountId').on('change', function () {

    LoadCustomerAssetsDDs();
    for (var i = 1; i <= length; i++) {

        var accs = "accountDevices'" + i + "'";
        document.getElementById(accs).disabled = true;

    }
});

var LoadCustomerAssetsDDs = function () {

    var accountId = $('#AccountId option:selected').val();

    var obj = { accountId: accountId }
    $.ajax({
        type: "GET",
        url: "/Devices/GetCustomerAssetsForDD",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#CustomerAssetId').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#CustomerAssetId').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

$('#accountG').on('change', function () {

    LoadCustomerAssetsDDG();

});


var LoadCustomerAssetsDDG = function () {

    var accountId = $('#accountG').val();

    var obj = { accountId: accountId }
    $.ajax({
        type: "GET",
        url: "/Devices/GetCustomerAssetsForDD",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#CustomerAssetId').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#CustomerAssetId').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

var submit = function () {
    var checkbox = document.getElementById("checkbox");

    if (checkbox.checked == true) {
        GlobalUpload();

    }
    else
        Upload();
}
var GlobalUpload = function () {
    var gName = document.getElementById("gName").value;
    var gAcc = document.getElementById("accountG").value;
    var gAsset = document.getElementById("CustomerAssetId").value;
    var gFile = document.getElementById("fileUpload").value;

    var formdata = new FormData(); //FormData object
    var fileInput = document.getElementById('fileUpload');
    //Iterating through each files selected in fileInput
    for (i = 0; i < fileInput.files.length; i++) {
        //Appending each file to FormData object
        formdata.append(fileInput.files[i].name, fileInput.files[i]);
    }
    //Creating an XMLHttpRequest and sending
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/BulkDevices/Upload');
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            alert(xhr.responseText);
        }
    }


    var obj = { Name: gName, Account: gAcc, Asset: gAsset };
    $.ajax({
        type: "POST",
        url: "/BulkDevices/BulkCreateGlobal",
        data: obj,
        success: function (response) {
            if (response == "error") {
                ("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }

        },
        failure: function () {
            alert("Failed!");
        }
    })
}

var length;
$(document).ready(function () {
    $("#global").hide();
});

$('#m_table_1').DataTable({
    responsive: false,
    scrollY: false,
    scrollX: true,
    scrollCollapse: true,
    dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
                                                        <'row'<'col-sm-12'tr>>
                                                        <'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,
    buttons: [
        'print',
        'pdfHtml5',
    ]
});
