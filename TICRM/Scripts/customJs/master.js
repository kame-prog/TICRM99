
//document.write('<script src="~/Content/Metronic/vendors/canvasjs/canvasjs.min.js"></script>');
$(document).ready(function () {
   
    /*AutoComplete Search*/
    Load_All_EACSearchList();
    LoadRecentSearchHistory();
    Load_All_FreeSearch();
    /*this is used to find url is exist in array and render it to SearchFeildId*/
    var recentURL = recentHistoryList.find(x => x.URL === window.location.pathname);
    if (recentURL != null) { $('#SearchFieldId').val(recentURL.Name); }
    /*AutoComplete Search*/
    /*Start Search in Dropdown*/
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });

    $("#m_table_1_length").children("label").children('select').select2('destroy').select2({
        width: '50%'
    });

    $("#accountmap").select2('destroy').select2({
        width: '50%'
    });


    /*Start Search in Dropdown with hyperLink for Accounts*/
    $('#AccountId,#account,#accountopp,#accountwo').attr("multiple", "multiple");
    //$("#AccountId option:selected").prop("selected", false);
    $("#account option:selected").prop("selected", false);
    $("#accountopp option:selected").prop("selected", false);
    $("#accountwo option:selected").prop("selected", false);

    $('#AccountId,#account,#accountopp,#accountwo').select2('destroy').select2({
        width: '100%', tags: true,
        tokenSeparators: [',', ' '], templateSelection: formatAccount, maximumSelectionLength: 1
    });

    function formatAccount(accountId) {
        if (!accountId.id) {
            return accountId.text;
        }

        var baseUrl = "/Accounts/AccountsDetail";
        var $state = $(
            "<a target='_blank' href=" + baseUrl + "/" + accountId.element.value + ">" + accountId.element.text + "</a>"
        );

        return $state;
    };
    /*Search in Dropdown*/
});

var LoadDropdowns = function () {
    $.ajax({
        type: "GET",
        url: "/Master/GetUsers",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#userdevice').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#userdevice').append(options);

            $('#useropp').html('');
            $('#useropp').append(options);

            $('#userwo').html('');

            $('#userwo').append(options);

            $('#accuser').html('');

            $('#accuser').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get teams
    $.ajax({
        type: "GET",
        url: "/Master/GetTeams",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#teamdevice').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#teamdevice').append(options);
            $('#teamwo').html('');
            $('#teamwo').append(options);
            $('#teamopp').html('');
            $('#teamopp').append(options);
            $('#accteam').html('');
            $('#accteam').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get accounts
    $.ajax({
        type: "GET",
        url: "/Master/GetAccounts",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#account').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#account').append(options);
            $('#accountopp').html('');

            $('#accountopp').append(options);
            $('#accountwo').html('');

            $('#accountwo').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get status
    $.ajax({
        type: "GET",
        url: "/Master/GetStatusId",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#statusopp').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#statusopp').append(options);
            $('#statuswo').html('');

            $('#statuswo').append(options);
            $('#statusdevice').html('');

            $('#statusdevice').append(options);

            $('#accstatus').html('');

            $('#accstatus').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

/*Change layout*/
var changeLayout = function (value) {
    //alert(value);
    $.get("/Home/SetLayout?value=" + value, function (data) {
        window.location.reload();

    });
}

/*change layout */
$('#account').on('change', function () {
    var accountId = $('#account option:selected').val();
    LoadCustomerAssetsDD();
});

var LoadCustomerAssetsDD = function () {

    var accountId = $('#account option:selected').val();

    var obj = { accountId: accountId }
    $.ajax({
        type: "GET",
        url: "/Devices/GetCustomerAssetsForDD",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#customerAsset').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#customerAsset').append(options);
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

$("#deviceTop,#deviceTopMobile").click(function () {
    //get cloud
    $.ajax({
        type: "GET",
        url: "/Master/GetCloud",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#clouddevice').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#clouddevice').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    $('#Createdevice').modal('show');

});

$("#accountTop,#accountTopMobile").click(function () {
    //get industry
    $.ajax({
        type: "GET",
        url: "/Master/GetAccountIndustry",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#accind').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#accind').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get account size
    $.ajax({
        type: "GET",
        url: "/Master/GetAccountSize",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#accsize').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#accsize').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get account type
    $.ajax({
        type: "GET",
        url: "/Master/GetAccountType",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#acctype').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#acctype').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    $('#CreateAccount').modal('show');

});

$("#oppTop,#oppTopMobile").click(function () {

    //get currency
    $.ajax({
        type: "GET",
        url: "/Master/GetCurrency",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#currency').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#currency').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get probability
    $.ajax({
        type: "GET",
        url: "/Master/GetProbabilityId",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#probabilty').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#probabilty').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    //get opp stage
    $.ajax({
        type: "GET",
        url: "/Master/GetOppurtunityStageId",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#stage').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#stage').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    $('#CreateModule').modal('show');

});

$("#worderTop,#worderTopMobile").click(function () {
    //get workrder stage
    $.ajax({
        type: "GET",
        url: "/Master/GetWorkStage",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $('#stagewo').html('');
            var options = '';
            options += '<option value="Select">Select</option>';
            for (var i = 0; i < response.length; i++) {
                options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
            }
            $('#stagewo').append(options);

        },
        failure: function () {
            alert("Failed!");
        }
    });
    $('#CreateWorder').modal('show');

});

$(window).on('load', function () {
    $('body').removeClass('m-page--loading');
});

// create Functions

var accountCreate = function () {

    var name = document.getElementById("accName").value;
    var desc = document.getElementById("accDesc").value;
    var phone = document.getElementById("accphone").value;
    var email = document.getElementById("accemail").value;
    var lat = document.getElementById("acclat").value;
    var long = document.getElementById("acclong").value;
    var size = $('#accsize option:selected').val();
    var user = $('#accuser option:selected').val();
    var team = $('#accteam option:selected').val();
    var type = $('#acctype option:selected').val();
    var ind = $('#accind option:selected').val();
    var status = $('#accstatus option:selected').val();

    var obj = {
        Name: name, Desc: desc,
        Phone: phone, Email: email,
        Size: size, User: user,
        Team: team, Type: type,
        Status: status, Ind: ind,
        Latitude: lat, Longitude: long
    };
    $.ajax({
        type: "POST",
        url: "/Accounts/CreateAccountHeader",
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
    $('#CreateAccount').modal('hide');

}

var deviceCreate = function () {

    var name = document.getElementById("DName").value;
    var macAdd = document.getElementById("DMacadd").value;
    var emei = document.getElementById("DEmei").value;
    var reg = document.getElementById("DReg").value;
    var lat = document.getElementById("DLat").value;
    var long = document.getElementById("DLong").value;
    var asset = $('#customerAsset option:selected').val();
    var user = $('#userdevice option:selected').val();
    var team = $('#teamdevice option:selected').val();
    var accID = $('#account option:selected').val();
    var cloud = $('#clouddevice option:selected').val();
    var status = $('#statusdevice option:selected').val();

    var obj = { Name: name, Mac: macAdd, EMEI: emei, Reg: reg, Lat: lat, Long: long, Asset: asset, User: user, Team: team, Cloud: cloud, Status: status, AccID: accID };
    $.ajax({
        type: "POST",
        url: "/Devices/CreatefromAccount",
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
    $('#Createdevice').modal('hide');

}

var oppCreate = function () {
    var accID = $('#accountopp option:selected').val()

    var amount = document.getElementById("OAmount").value;
    var desc = document.getElementById("ODesc").value;
    var title = document.getElementById("OTitle").value;
    var lat = document.getElementById("opplat").value;
    var long = document.getElementById("opplong").value;
    var user = $('#useropp option:selected').val();
    var team = $('#teamopp option:selected').val();
    var status = $('#statusopp option:selected').val();
    var prob = $('#probabily option:selected').val();
    var stage = $('#stage option:selected').val();
    var curr = $('#currency option:selected').val();
    var obj = {
        Amount: amount,
        Desc: desc,
        Title: title,
        OUser: user,
        Team: team,
        Status: status,
        Prob: prob,
        Stage: stage,
        Curr: curr,
        AccID: accID,
        Latitude: lat,
        Longitude: long
    };
    $.ajax({
        type: "POST",
        url: "/Opportunities/CreateOfromAccount",
        data: obj,
        dataType: "text",
        success: function (response) {
            if (response == "error") {
                alert("Failed to update on mqtt.");
            }
            else {
                alert("Failed to update on mqtt.");
            }

        },
        failure: function () {
            alert("Failed!");
        }
    })
    $('#CreateModule').modal('hide');

}

var workorderCreate = function () {
    var accID = $('#accountwo option:selected').val();

    var title = document.getElementById("WOTitle").value;
    var desc = document.getElementById("WODesc").value;
    var nte = document.getElementById("WONTE").value;
    var user = $('#userwo option:selected').val();
    var team = $('#teamwo option:selected').val();
    var status = $('#statuswo option:selected').val();
    var stage = $('#stagewo option:selected').val();
    var obj = { Title: title, Desc: desc, NTE: nte, WOUser: user, Team: team, Status: status, Stage: stage, AccID: accID };
    $.ajax({
        type: "POST",
        url: "/WorkOrders/CreateWOfromAccount",
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
    $('#CreateWorder').modal('hide');

}

//Omni scale map and graphhoper map create
function createMaps(divId) {
   
    var osmAttr = '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors';

    var omniscale = L.tileLayer.wms('https://maps.omniscale.net/v2/swuich-1d90861b/style.default/map', {
        layers: 'osm',
        attribution: osmAttr + ', &copy; <a href="http://maps.omniscale.com/">Omniscale</a>'
    });

    var osm = L.tileLayer('https://maps.omniscale.net/v2/swuich-1d90861b/style.default/{z}/{x}/{y}.png', {
        attribution: osmAttr
    });

    var maps = L.map(divId, { layers: [omniscale] });
    L.control.layers({
        "Omniscale": omniscale,
        "OpenStreetMap": osm
    }).addTo(maps);
    
    return maps;
}
//End create functions

//Auto Complete Search
var SearchDataModule = [];
var SearchDataModulePages = [];
var SearchDataModulePagesAccounts = [];
var SearchData = [];
var FirstInSearch = [];
var FirstInSearcha = [];
var freeSearch = [];

// save MACAddress in Session
function SaveMacAddress(value) {

    var obj = { MacAddress: value }

    $.ajax({
        type: "POST",
        url: "/GlobalSearch/SaveMac",
        data: obj,
        dataType: "text",
        async: false,
        success: function (response) {
        },
        failure: function () {
            alert("Failed!");
        }
    });
}

//f8be18e9-3ab7-4874-9520-b385c841d82f
// its loading a list in object for search
function LoadEACSearchList() {
    $.ajax({
        type: "POST",
        url: "/GlobalSearch/GetEACSearchList",
        //url: "/GlobalSearch/Get_All_EACSearchList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            SearchData = new Array();
            FirstInSearch = new Array();

            var FirstInSearchData = response.FirstInSearch;
            for (var i = 0; i < FirstInSearchData.length; i++) {
                FirstInSearch.push(FirstInSearchData[i]);
            }

            if (response.SearchDataList.length == 0) {
                SearchData = FirstInSearch;

            }
            else {
                var SearchDataList = response.SearchDataList;
                for (var i = 0; i < SearchDataList.length; i++) {
                    SearchData.push(SearchDataList[i]);
                }
            }

            EasyautoCompleteSearch();

        },
        failure: function () {
            alert("Failed!");
        }
    });
}

function Load_All_EACSearchList() {
    $.ajax({
        type: "POST",
        url: "/GlobalSearch/Get_All_EACSearchList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            SearchData = new Array();
            
            var FirstInSearchData = response.FirstInSearch;

            for (var i = 0; i < FirstInSearchData.length; i++) {
                SearchData.push(FirstInSearchData[i]);
            }
            
            EasyautoCompleteSearch();

        },
        failure: function () {
            alert("Failed!");
        }
    });
}

function Load_All_FreeSearch() {
    $.ajax({
        type: "POST",
        url: "/GlobalSearch/GetAllSearchItems",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            freeSearch = new Array();

            var FirstInSearchData = response.FirstInSearch;

            for (var i = 0; i < FirstInSearchData.length; i++) {
                freeSearch.push(FirstInSearchData[i]);
            }
            FreeSearch();

        },
        failure: function () {
            alert("Failed!");
        }
    });
}
/**
 * Search options 
 * */
function FreeSearch() {

    var FreeSearch = {

        data: freeSearch,
        getValue: "Text",
        template: {
            type: "description",
            fields: {
                description: "value"
            }
        },
        
        list: {
            match: {
                enabled: true,
                method: function (element, phrase) {
                    if (element != null) {
                        if (element.indexOf(phrase) >= 0) {
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
            },
            sort: {
                enabled: true
            },
            maxNumberOfElements: 10,
            onClickEvent: function () { },
            onSelectItemEvent: function () {

            },
            onLoadEvent: function () {
                if (($("#SearchFieldId").val().match(/ >/g) || []).length > 0) {
                    var obj = { value: $("#SearchFieldId").val() }
                }

            },
            onChooseEvent: function () {


                addRecentSearch($("#SearchFieldId").getSelectedItemData().FirstURL, $("#LazySearchId").getSelectedItemData().Text);
                var s = $("#SearchFieldId").getSelectedItemData().Text;
                var selectedItemType = $("#LazySearchId").getSelectedItemData().Type;
                var selectedItemValue = $("#LazySearchId").getSelectedItemData().value;
                if (selectedItemType == "URL") {
                    $('body').addClass('m-page--loading');
                    window.location.href = $("#LazySearchId").getSelectedItemData().FirstURL;
                }

                if (selectedItemType == "MACAddress") {
                    localStorage.setItem('selectedDevice', $("#LazySearchId").getSelectedItemData().Result);
                    var mac = $("#LazySearchId").getSelectedItemData().Result;
                    SaveMacAddress(mac);
                    window.location.href = $("#LazySearchId").getSelectedItemData().FirstURL;
                }

                if (selectedItemType == "Modal") {
                    try {
                        if ($("#LazySearchId").getSelectedItemData().value == "Account") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Device") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value== "Lead") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Opportunity") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "CustomerAsset") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Reading") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "ServiceCall") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Resource") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "WorkOrder") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "ReadingType") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "ReadingUnit") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Address") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Location") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "Alert") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "WorkFlow") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "WorkFlowMapping") {
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else if ($("#LazySearchId").getSelectedItemData().value == "WorkFlowReport") {
                          
                            eval($("#LazySearchId").getSelectedItemData().JS_function);
                        }
                        else {

                            toastr.error("Please Refresh the Page and Contact to Support Team.", "Error", {
                                "closeButton": true,
                                "progressBar": true,
                                "positionClass": "toast-top-right"
                            });
                        }
                    } catch (e) {
                        $('body').removeClass('m-page--loading');
                    }
                }

            },
            onKeyEnterEvent: function () {

            },
            onMouseOverEvent: function () { },
            onMouseOutEvent: function () { },
            onHideListEvent: function () { }
        },
        highlightPhrase: true
        
    };

    $("#LazySearchId").easyAutocomplete(FreeSearch);



}


// load Global Search
function EasyautoCompleteSearch() {

    var Searchoptions = {
        data: SearchData,
        getValue: "Text",
        template: {
            type: "description",
            fields: {
                description: "value"
            }
        },
        list: {
            match: {
                enabled: true,
                method: function (element, phrase) {
                    if (element.indexOf(phrase) === 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            sort: {
                enabled: true
            },
            maxNumberOfElements: 52,
            onClickEvent: function () { },
            onSelectItemEvent: function () {

            },
            onLoadEvent: function () {
                if (($("#SearchFieldId").val().match(/ >/g) || []).length > 0) {
                    var obj = { value: $("#SearchFieldId").val() }

                }
                console.log("loaded");
                
            },
            onChooseEvent: function () {

                
                addRecentSearch($("#SearchFieldId").getSelectedItemData().FirstURL, $("#SearchFieldId").getSelectedItemData().Text);
                var s = $("#SearchFieldId").getSelectedItemData().Text;
                var selectedItemType = $("#SearchFieldId").getSelectedItemData().Type;
                 if (selectedItemType == "URL") {
                $('body').addClass('m-page--loading');
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                 }
                 else
                    Load_All_EACSearchListModule(s);
                

                $("#SearchFieldId").focus();
                console.log("olamba2");

            },
            onKeyEnterEvent: function () {

            },
            onMouseOverEvent: function () { },
            onMouseOutEvent: function () { },
            onHideListEvent: function () { }
        },
        highlightPhrase: true


    };
    $("#SearchFieldId").easyAutocomplete(Searchoptions);

    $('#SearchFieldId').keyup(function (e) {
        if (e.keyCode == 8) {
            e.preventDefault();
            var text = $(this).val().split('.');
            text.splice(text.length - 1);
            $(this).val(text.join(' '));
        }
    })
                        
}

/*
 *Code by: Akhtar Zaman 
 * Ajax method to get search list for each module on 
 * OnChose event in search bar
 * 7/7/2020
 **/
function Load_All_EACSearchListModule(Module) {
    $.ajax({
        type: "Post",
        url: "/GlobalSearch/Get_All_EACSearchListModule",
        data: { Module: Module },
        dataType: "json",
        async: false,
        success: function (response) {
            SearchDataModule = new Array();
            var FirstInSearchDataa = response.FirstInSearch;

            for (var i = 0; i < FirstInSearchDataa.length; i++) {
                SearchDataModule.push(FirstInSearchDataa[i]);
            }
            $('#SearchFieldId').val(Module + " > ");
            $("#SearchFieldId").focus();
            

            EasyautoCompleteSearchModule(Module);

        },
        failure: function () {
            alert("Failed!");
        }

    });
}
var itemText;
var itemUrl;
/**
 * Code By: Akhtar Zaman
 * Data: 7/7/2020
 * the function calls off once the data is being fetched from the controller
 * The function nevigates to the url on OnChose event of the module
 * */
function EasyautoCompleteSearchModule(Module) {
    var SearchoptionsModule = {
        data: SearchDataModule,

        getValue: "Text",
        template: {
            type: "description",
            fields: {
                description: "value"
            }
        },
        list: {
            match: {
                enabled: true,
                method: function (element, phrase) {
                    if (element.indexOf(phrase) === 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            sort: {
                enabled: true
            },
            maxNumberOfElements: 52,
            onClickEvent: function () { },
            onSelectItemEvent: function () { },
            onLoadEvent: function () {
                
            },
            onChooseEvent: function () {
                addRecentSearch($("#SearchFieldId").getSelectedItemData().FirstURL, $("#SearchFieldId").getSelectedItemData().Text);
                var s = $("#SearchFieldId").val();
                var selectedItemType = $("#SearchFieldId").getSelectedItemData().Type;

                if (selectedItemType == "URL") {
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                }

                if (selectedItemType == "MACAddress") {
                    localStorage.setItem('selectedDevice', $("#SearchFieldId").getSelectedItemData().Result);
                    var mac = $("#SearchFieldId").getSelectedItemData().Result;
                    SaveMacAddress(mac);
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                }

                if (selectedItemType == "Modal") {
                    try {
                        if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Accounts") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim()== "Devices") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Leads") {
                            console.log("Lead");
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Opportunities") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "CustomerAssets") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Readings") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "ServiceCalls") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Resources") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "WorkOrders") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "ReadingTypes") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "ReadingUnits") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Addresses") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Locations") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "Alerts") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "WorkFlows") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "WorkFlowMappings") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('>')[0].trim() == "WorkFlowReports") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else {

                            toastr.error("Please Refresh the Page and Contact to Support Team.", "Error", {
                                "closeButton": true,
                                "progressBar": true,
                                "positionClass": "toast-top-right"
                            });
                        }
                    } catch (e) {
                        $('body').removeClass('m-page--loading');
                    }
                }

            },
            onKeyEnterEvent: function () {

            },
            onMouseOverEvent: function () { },
            onMouseOutEvent: function () { },
            onHideListEvent: function () { }
        },
        highlightPhrase: true


    };

    $("#SearchFieldId").easyAutocomplete(SearchoptionsModule);

    //$('#SearchFieldId').keyup(function (e) {
    //    if (e.keyCode == 8) {
    //        e.preventDefault();
    //        var text = $(this).val().split('>');
    //        text.splice(text.length - 1);
    //        $(this).val(text.join(' '));
    //        Load_All_EACSearchList();
    //    }
    //})
}

function Load_All_EACSearchListModulePages(Module, Name, Id) {
  
    $.ajax({
        type: "Post",
        url: "/GlobalSearch/Get_All_EACSearchListModulePages",
        data: { Module: Module, Name: Name, Id: Id },
        dataType: "json",
        async: false,
        success: function (response) {
            SearchDataModulePages = new Array();

            var FirstInSearchDataPages = response.FirstInSearch;

            for (var i = 0; i < FirstInSearchDataPages.length; i++) {
                SearchDataModulePages.push(FirstInSearchDataPages[i]);
            }


            EasyautoCompleteSearchModulePages(Module);

        },
        failure: function () {
            alert("Failed!");
        }

    });
}

function EasyautoCompleteSearchModulePages(Module) {
    var SearchoptionsModulePages = {
        data: SearchDataModulePages,

        getValue: "Text",
        template: {
            type: "description",
            fields: {
                description: "value"
            }
        },
        list: {
            match: {
                enabled: true,
                method: function (element, phrase) {
                    if (element.indexOf(phrase) === 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            sort: {
                enabled: true
            },
            maxNumberOfElements: 52,
            onClickEvent: function () { },
            onSelectItemEvent: function () {

            },
            onLoadEvent: function () {
                if (($("#SearchFieldId").val().match(/ >/g) || []).length > 0) {
                    var obj = { value: $("#SearchFieldId").val() }
                }

            },
            onChooseEvent: function () {


                addRecentSearch($("#SearchFieldId").getSelectedItemData().FirstURL, $("#SearchFieldId").getSelectedItemData().Text);
                var s = $("#SearchFieldId").val();
                var selectedItemType = $("#SearchFieldId").getSelectedItemData().Type;
                //var array = message.payloadString.split(',');
                //var array = $("#SearchFieldId").getSelectedItemData().Text.split('>');

                if (selectedItemType == "URL") {
                    $('body').addClass('m-page--loading');
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                }

                if (selectedItemType == "MACAddress") {
                    localStorage.setItem('selectedDevice', $("#SearchFieldId").getSelectedItemData().Result);
                    var mac = $("#SearchFieldId").getSelectedItemData().Result;
                    SaveMacAddress(mac);
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                }
                if (selectedItemType == "Associates") {
                    var associate = $("#SearchFieldId").getSelectedItemData().Text.split('>');
                    var url = $("#SearchFieldId").getSelectedItemData().FirstURL.split('/');
                    GetAccountAssociatesData(url[3], associate[3].trim(), associate[1].trim());

                }

                if (selectedItemType == "Modal") {
                    $('body').addClass('m-page--loading');

                    try {
                        if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Accounts") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Devices") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Leads") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Opportunities") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "CustomerAssets") {

                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Readings") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "ServiceCalls") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Resources") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "WorkOrders") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "ReadingTypes") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "ReadingUnits") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Addresses") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Locations") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "Alerts") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "WorkFlows") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "WorkFlowMappings") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[0] == "WorkFlowReports") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else {

                            toastr.error("Please Refresh the Page and Contact to Support Team.", "Error", {
                                "closeButton": true,
                                "progressBar": true,
                                "positionClass": "toast-top-right"
                            });
                        }
                    } catch (e) {
                        $('body').removeClass('m-page--loading');
                    }
                }

            },
            onKeyEnterEvent: function () {

            },
            onMouseOverEvent: function () { },
            onMouseOutEvent: function () { },
            onHideListEvent: function () { }
        },
        highlightPhrase: true


    };
    $("#SearchFieldId").easyAutocomplete(SearchoptionsModulePages);
    $('#SearchFieldId').keyup(function (e) {
        if (e.keyCode == 8) {
            e.preventDefault();
            var text = $(this).val().split('.');
            text.splice(text.length - 1);
            $(this).val(text.join(' '));
        }
    })
}

function GetAccountAssociatesData(AccountId, Module, Name) {
    $.ajax({
        type: "Post",
        url: "/GlobalSearch/Get_ALL_EACSearchListAccountModule",
        data: { AccountId: AccountId, Module: Module, Name: Name},
        dataType: "json",
        async: false,
        success: function (response) {
      
            SearchDataModulePagesAccounts = new Array();

            var FirstInSearchDataPages = response.FirstInSearch;

            for (var i = 0; i < FirstInSearchDataPages.length; i++) {
                SearchDataModulePagesAccounts.push(FirstInSearchDataPages[i]);
            }

            EasyautoCompleteSearchAccountsAssocaites(AccountId, Module, Name);

        },
        failure: function () {
            alert("Failed!");
        }

    });
}

function EasyautoCompleteSearchAccountsAssocaites(AccountId, Module, Name) {
    var SearchoptionsAccountsAssocaites = {
        data: SearchDataModulePagesAccounts,

        getValue: "Text",
        template: {
            type: "description",
            fields: {
                description: "value"
            }
        },
        list: {
            match: {
                enabled: true,
                method: function (element, phrase) {
                    if (element.indexOf(phrase) === 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            sort: {
                enabled: true
            },
            maxNumberOfElements: 52,
            onClickEvent: function () { },
            onSelectItemEvent: function () {

            },
            onLoadEvent: function () {
                if (($("#SearchFieldId").val().match(/ >/g) || []).length > 0) {
                    var obj = { value: $("#SearchFieldId").val() }
                }

            },
            onChooseEvent: function () {


                addRecentSearch($("#SearchFieldId").getSelectedItemData().FirstURL, $("#SearchFieldId").getSelectedItemData().Text);
                var s = $("#SearchFieldId").val();
                var selectedItemType = $("#SearchFieldId").getSelectedItemData().Type;
                if (selectedItemType == "URL") {
                    $('body').addClass('m-page--loading');
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                }

                if (selectedItemType == "MACAddress") {
                    localStorage.setItem('selectedDevice', $("#SearchFieldId").getSelectedItemData().Result);
                    var mac = $("#SearchFieldId").getSelectedItemData().Result;
                    SaveMacAddress(mac);
                    window.location.href = $("#SearchFieldId").getSelectedItemData().FirstURL;
                }
              

                if (selectedItemType == "Modal") {
                    $('body').addClass('m-page--loading');

                    try {
                        
                        if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[3] == "Devices") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[3] == "Opportunities") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[3] == "CustomerAssets") {

                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        
                        else if ($("#SearchFieldId").getSelectedItemData().Text.split('.')[3] == "Locations") {
                            eval($("#SearchFieldId").getSelectedItemData().JS_function);
                        }
                        
                        else {

                            toastr.error("Please Refresh the Page and Contact to Support Team.", "Error", {
                                "closeButton": true,
                                "progressBar": true,
                                "positionClass": "toast-top-right"
                            });
                        }
                    } catch (e) {
                        $('body').removeClass('m-page--loading');
                    }
                }

            },
            onKeyEnterEvent: function () {

            },
            onMouseOverEvent: function () { },
            onMouseOutEvent: function () { },
            onHideListEvent: function () { }
        },
        highlightPhrase: true


    };

    $("#SearchFieldId").easyAutocomplete(SearchoptionsAccountsAssocaites);

    $('#SearchFieldId').keyup(function (e) {
        if (e.keyCode == 8) {
            e.preventDefault();
            var text = $(this).val().split('.');
            if (text.length == 4) {
                text.splice(text.length - 1);
                $(this).val(text.join('.'));
                EasyautoCompleteSearchModulePages(Module);
            }
            else {
                text.splice(text.length - 4);
                $(this).val(text.join('.'));
                EasyautoCompleteSearchModulePages(Module);
            }
           
        }
    })
}

function ClearSearchSession() {
    SearchoptionsModulePages = null;
    Load_All_EACSearchList();
    $('#SearchFieldId').val('');
}

function ClearSearchSession1() {
    if (document.getElementById("SearchFieldId").value == "Dashboard") {
        $('#SearchFieldId').val('');
    }
}

function ClickOnNavigationLilsk(value) {

    var url = window.location.pathname;
    if ($.inArray(url, SearchData) !== -1) {
        alert();
    }
    var isexist = false;

    for (var i = 0; i < SearchData.length; i++) {
        var data = SearchData[i].Text;
        if (data == value) {
            isexist = true;
            break;
        }
    }
    alert(isexist);
}

var recentHistoryList = [];

var addRecentSearch = function (URL, Name) {

    var d = new Date();
    d.getHours(); // => 9
    d.getMinutes(); // =>  30
    d.getSeconds(); // => 51
    var time = d.getHours + ":" + d.getMinutes;

    if (recentHistoryList == null) {
        recentHistoryList = [];
    }

    recentHistoryList.push({ 'Name': Name, 'URL': URL, SearchDateTime: new Date().toLocaleString() });
    localStorage.setItem("recentHistoryList", JSON.stringify(recentHistoryList));

}

var nvaigateRecentsearch = function (URL, Name) {
    addRecentSearch(URL, Name);
    window.location = URL;
}

var LoadRecentSearchHistory = function () {
    recentHistoryList = JSON.parse(localStorage.getItem("recentHistoryList"));

    if (recentHistoryList == null) {
        recentHistoryList = [];
        recentHistoryList.push({ 'Name': "Dashboard", 'URL': "/", SearchDateTime: new Date().toLocaleString() });
        localStorage.setItem("recentHistoryList", JSON.stringify(recentHistoryList));
    }
    $('#RecentSearchItems').html('');
    var htmlData = '';
    for (var i = recentHistoryList.length - 1; i >= 0; i--) {
        htmlData += '<div class="m-list-timeline__item">';
        htmlData += '    <span class="m-list-timeline__badge"></span>';
        htmlData += '    <span class="m-list-timeline__text m--padding-left-20" onclick="nvaigateRecentsearch(\'' + recentHistoryList[i].URL + '\',\'' + recentHistoryList[i].Name + '\')" style="color:#575962;cursor: pointer;">' + recentHistoryList[i].Name + '</span>';
        htmlData += '    <span class="m-list-timeline__time">' + recentHistoryList[i].SearchDateTime + '</span>';
        htmlData += '</div >';
    }

    $('#RecentSearchItems').append(htmlData);

}

/*MQTT Script*/
var globalint = 0;
var globalArray = new Array();
var globalDevicesArray = new Array();
var count = 0;
var count1 = 0;
var globalSelectedDevice;
var client = new Messaging.Client("192.168.22.79", 8080, "Swuich" + parseInt(Math.random() * 100, 10));
var messages = new Array();

$(document).ready(function () {

    client.connect(options);
});
//Using the HiveMQ public Broker, with a random client Id
//var client = new Messaging.Client("http://110.36.225.74", 8000, "TechImplement_id_" + parseInt(Math.random() * 100, 10));

//Gets  called if the websocket/mqtt connection gets disconnected for any reason
client.onConnectionLost = function (responseObject) {
    //Depending on your scenario you could implement a reconnect logic here
    alert("connection lost: " + responseObject.errorMessage);
};

if (count == 0) {
    $('#counter').html("Place your Thumb on the sensor");
    $('#counterb').html("Place your Thumb on the sensor");

}


//Gets called whenever you receive a message for your subscriptions
client.onMessageArrived = function (message) {
    
    var url = window.location.pathname;
    var array = message.payloadString.split(',');
    globalArray.push([array[0], array[1], array[2], array[3]]);
    messages.push(message.payload);
    count++;
    //if (url.toLowerCase() == "/admin" || url.toLowerCase() == "/admin/index") {
    //    console.log("Admin");
    //    var options = '';
    //    options += '<div class="m-widget2__item m-widget2__item--primary">';
    //    options += '<div class="m-widget2__checkbox"></div>';
    //    options += '<div class="m-widget2__desc"><span class="m-widget2__text">';
    //    options += message.payloadString;
    //    options += '</span></br ><span class="m-widget2__user-name"><a href="#" class="m-widget2__link">';
    //    options += message.destinationName;
        
    //    options += '</a></span ></div ></div >';
    //    $('#receivedMessagesArea').prepend(options);

    //}

    if (globalDevicesArray.indexOf(array[1]) > -1) {
    }
    else {

        globalDevicesArray.push(array[1]);
        if (url == "/Devices/Index" || url == "/Devices") {
            getListofDevices(array[1]);
        }
        else if (url.includes("/Accounts/AccountsDetail")) {
            getListofDevices(array[1]);
        }
        else if (url == "/Devices" || url == "/Devices/Index" || url == "/") {
            $('#connectedDeviceCount').html(globalDevicesArray.length);
        }
        else if (url == "/" || url == "/Dashboard/Index" || url == "/Dashboard") {
            //getListofDevicees(array[1]);
        }
    }
    var DeviceSelected = localStorage.getItem('selectedDevice');
    if (DeviceSelected != null) {
        globalSelectedDevice = DeviceSelected;
    }
    var d = new Date();
    var Time = d.getHours() + '' + d.getMinutes() + d.getSeconds();

    if (url == "/Devices" || url == "/Devices/Index" || url == "/") {
        $('#messages').append('<span>Topic: ' + message.destinationName + '  | ' + array[0] + '</span><br/>');
        //updateMap(array[1], array[2], array[3]);
    }
    if (array[1] == "A4:CF:12:DA:92:10") {
        updateTank(array[2]);
    }

    else if ((url == "/Devices/device" || url == "/devices/device" || url == "/Admin" || url == "/Admin/index" || url.indexOf("/Accounts/AccountsDetail") >= 0) && (DeviceSelected == array[1] || globalSelectedDevice == array[1])) {
        console.log("accounts");
        $('#messages').append('<span>Topic: ' + message.destinationName + '  | ' + array[0] + '</span><br/>');
        if (array[1] == "04:78:63:01:B8:DC") {
            updateChart(new Date(), Math.round(array[0]));
            console.log("MessageArrived");
        }
        if (array[4] > 97) {
            $('#feverValue').html(array[4]);
            $('#counter').html("Completed");
        }
        if (array[5] > 0) {
            $('#heartbeatvalue').html(array[5]);
        }
        if (array[4] > 0 && array[4] < 97) {
            $('#counter').html("Calculating your Temperature...")
        }

    }
    else if ((url == "/Devices/device" || url == "/devices/device") && (DeviceSelected == null || globalSelectedDevice == null)) {
        window.location.href = "../Devices/Index";
    }
};


$("#btnSendCommand").click(function () {

    if ($("#btnSendCommand").text() == "Send Command") {
        if ($("#txtValue").val().length > 0) {

            if (Number.isInteger(parseInt($("#txtValue").val()))) {

                publish($("#txtValue").val(), 'delay', 2);

                alert($("#commandDropDown").val() + ' - Command successfully send with value = ' + $("#txtValue").val());
            }
        }
        else {
            alert($("#commandDropDown").val() + ' - Command successfully send with value = ' + $("#txtValue").val());


        }

    }
    else if ($("#btnSendCommand").text() == "Save Service Date") {
        var date = $('#txtDate').val();
        if (date == "" || date == "undefined") {

            alert("Please enter a device service date");

        }
        else {
            if (validateDate(date)) {
                var myDate = new Date(date);
                var today = new Date();

                if (myDate >= today) {
                    $.ajax({
                        url: "/Devices/UpdateDeviceSerivceDate",
                        type: "get", //send it through get method
                        data: {
                            mac: localStorage.getItem('selectedDevice'),
                            date: date

                        },
                        success: function (response) {
                            alert("Device service date updated successfully!");
                        },
                        error: function (xhr) {
                            //Do Something to handle error

                        }
                    });
                }
                else {

                    alert("Device service date must be in future!");
                }

                // alert(localStorage.getItem('selectedDevice'));

            }
            else {
                alert("Please enter a valid device service date");
            }

        }

    }


});

function validateDate(testdate) {
    var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
    return date_regex.test(testdate);
}

var conectedDeviceCount = function () {

}


//Connect Options
var options = {
    timeout: 1550,
    //Gets Called if the connection has sucessfully been established
    onSuccess: function () {
        client.subscribe('DeviceToServerm', { qos: 2 });
        console.log("Subscribed");
        //client.subscribe('Olamba', { qos: 2 });
    },
    //Gets Called if the connection could not be established
    onFailure: function (message) {
        //alert("Connection failed: " + message.errorMessage);
    }
};

//Creates a new Messaging.Message Object and sends it to the HiveMQ MQTT Broker
var publish = function (payload, topic, qos) {
    //Send your message (also possible to serialize it as JSON or protobuf or just use a string, no limitations)
    var message = new Messaging.Message(payload);
    message.destinationName = topic;
    message.qos = qos;
    client.send(message);
}


/*End mqtt*/

//opportunity model

var LoadOpportunityOnId = function (id) {
    //$('body').addClass('m-page--loading');
    $('#RightSideModal').modal('show');
    $(".m-page--loading").css("display", "block");
    //debugger;
    $.ajax({
        type: "GET",
        url: "/Accounts/GetOppertunityDetailOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            $('.modal-content-rightside').html('').html(response);

            $(".m-page--loading").css("display", "none");
        },
        failure: function () {
            alert("Failed!");
            //$('body').removeClass('m-page--loading');
        }
    });
}


//Acount Right Side Bar
var accSidebar = function (accountId, name) {
    document.getElementById("accountPanelTitle").textContent = name;

    $('#accountDetailRightSliderModal').modal('show');
    $("#accLoader").css("display", "block ");

    var acc = { accountId: accountId };
    var open;
    var closed;
    var won;
    var chart2;
    $.ajax({
        type: "GET",
        url: "/Admin/AccountDetailforAdmin",
        data: acc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            open = response.open;
            closed = response.lost;
            won = response.won;
            document.getElementById("assetsCountData").textContent = response.customerAssets;
            document.getElementById("devicesCountData").textContent = response.devices;
            document.getElementById("workflowCountData").textContent = response.workflow;
            document.getElementById("workorderCountData").textContent = response.workorders;
            chart2 = new CanvasJS.Chart("salesChartAcc", {
                //exportEnabled: true,
                animationEnabled: true,
                backgroundColor: "transparent",
                title: {
                    //text: "Sales Chart"
                },
                legend: {
                    cursor: "pointer",
                    verticalAlign: "bottom",
                    horizontalAlign: "center",
                    itemclick: explodePie
                },
                data: [{
                    type: "pie",
                    showInLegend: true,
                    toolTipContent: "{name}: <strong>{y}%</strong>",
                    indexLabel: "{name} - {y}%",
                    dataPoints: [
                        { y: open, name: "Open", exploded: true },
                        { y: closed, name: "Closed as Lost" },
                        { y: won, name: "Closed as Won" }
                    ]
                }]
            });
            chart2.render();
        },
        failure: function () {
            alert("Failed!");
        }
    });

    function explodePie(e) {
        if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
            e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
        } else {
            e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
        }
        e.chart.render();
    }

    var obj = { accountId: accountId };
    var costDataPoints = [];
    $.ajax({
        type: "GET",
        url: "/Accounts/GetAccountCost",
        data: obj,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var obj = { label: resolvedate(response[i].Date), y: parseInt(response[i].Cost1) }
                costDataPoints.push(obj);
            }
            var Powerconsumptionchart = new CanvasJS.Chart("costchart", {
                theme: "light2",
                animationEnabled: true,
                backgroundColor: "transparent",
                title: {
                    // text: "Game of Thrones Viewers of the First Airing on HBO"
                },
                axisY: {
                    includeZero: false,
                    title: "Cost",
                    suffix: "$"
                },
                toolTip: {
                    shared: "true"
                },
                legend: {
                    cursor: "pointer",
                    itemclick: toggleDataSeries
                },
                data: [
                    {
                        type: "spline",
                        lineColor: "orange",
                        showInLegend: true,
                        legendMarkerColor: "orange",
                        yValueFormatString: "##.00$",
                        name: "Consumptions",
                        dataPoints: costDataPoints
                    }
                ]
            });
            Powerconsumptionchart.render();
            $("#accLoader").css("display", "none");

        },
        failure: function () {
            alert("Failed!");
        }
    });

    
    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        } else {
            e.dataSeries.visible = true;
        }

    }
}

/**
 * Code by AKhtar Zaman
 * 12/7/2020
 * The below set of methods load the modules detial data on the module selection
 * in search fiedls and show it in the right side bar
 * @param {any} id
 */

var Accounts_Details_Modal = function (id) {
    $('body').addClass('m-page--loading');
    $('.modal-content-rightside').html('').html('');

    $.ajax({
        type: "GET",
        url: "/Accounts/AccountDetailsPartial",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {

            $('.modal-content').html('').html(response);
            $('#m_table_Assets').DataTable({
                columnDefs: [
                    { "width": "10px", "targets": 0 },
                    { "width": "40px", "targets": 1 },
                    { "width": "100px", "targets": 2 },
                    { "width": "70px", "targets": 3 },
                    { "width": "70px", "targets": 4 },
                    { "width": "70px", "targets": 5 },
                    { "width": "70px", "targets": 6 },
                    { "width": "70px", "targets": 7 },
                    { "width": "70px", "targets": 8 }
                ]
            });

            $('#m_table_Location').DataTable({ scrollX: true });
            $('#m_table_Device').DataTable({ scrollX: true });

            $('#m_table_Opportunity').DataTable({ scrollX: true });

            $('#m_modal_Details').modal('show');

            $('body').removeClass('m-page--loading');
        },
        failure: function () {
            alert("Failed!");
            $('body').removeClass('m-page--loading');
        }
    });
}

var Devices_Details_Modal = function (id) {
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");
    $.ajax({
        type: "GET",
        url: "/Devices/PartialDetailsOnId",
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

var Leads_Details_Modal = function (id) {
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    //$(".m-page--loading").css("display", "block");
    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Leads/PartialDetailsOnId",
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

var Opportunities_Details_Modal = function (id) {
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");


    $.ajax({
        type: "GET",
        url: "/Opportunities/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {

            $('.modal-content-rightside').html('').html(response);
            $("#loader").css("display", "none");

        },
        failure: function () {
            alert("Failed!");
            //mApp.unblockPage();
            $('body').removeClass('m-page--loading');
        }
    });
}

var CustomerAssets_Details_Modal = function (id) {
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");
    $.ajax({
        type: "GET",
        url: "/CustomerAssets/PartialDetailsOnId",
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

var Readings_Details_Modal = function (id) {
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");
    $.ajax({
        type: "GET",
        url: "/Readings/PartialDetailsOnId",
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

var ServiceCalls_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/ServiceCalls/PartialDetailsOnId",
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

var Resources_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Resources/PartialDetailsOnId",
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

var WorkOrders_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/WorkOrders/PartialDetailsOnId",
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

var ReadingTypes_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/ReadingTypes/PartialDetailsOnId",
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

var ReadingUnits_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/ReadingUnits/PartialDetailsOnId",
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

var Addresses_Details_Modal = function (id) {
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");


    $.ajax({
        type: "GET",
        url: "/Addresses/PartialDetailsOnId",
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

var Locations_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Locations/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {

            $('.modal-content-rightside').html('').html(response);
            $("#loader").css("display", "none");

        },
        failure: function () {
            alert("Failed!");
            //mApp.unblockPage();
            $('body').removeClass('m-page--loading');
        }
    });
}

var Alerts_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Alerts/PartialDetailsOnId",
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

var WorkFlows_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/WorkFlows/PartialDetailsOnId",
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

var WorkFlowMappings_Details_Modal = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#m_modal_Details').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/WorkFlowMappings/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            $("#m_modal_Details").children('.modal-dialog.modal-rightside-dialog.modal-RightSide-slideout').children('.modal-content').html('').html(response);
            $("#loader").css("display", "none");

            //$('body').removeClass('m-page--loading');
            //$('.modal-content-rightside').html('').html(response);
            //$('#RightSideModal').modal('show');
            //$('body').removeClass('m-page--loading');
        },
        failure: function () {
            alert("Failed!");
            $('body').removeClass('m-page--loading');
        }
    });
}

var WorkFlowReports_Details_Modal = function (id) {
 
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#m_modal_Details').modal('show');
    $('.modal-content-rightside').html('').html('');

    $(".m-page--loading").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/WorkFlowMappings/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            $('.modal-content').html('').html(response);
            $(".m-page--loading").css("display", "none");

        },
        failure: function () {
            alert("Failed!");
            $('body').removeClass('m-page--loading');
        }
    });
}

var CaseCreate = function (RelatedTo, RelatedToId, CaseTitle, CaseType, CaseDescription, CaseContact, CaseTeam, CaseUser, CaseOrigin) {
    var obj = { CaseTitle: CaseTitle, Description: CaseDescription, AssignedUser: CaseUser, AssignedTeam: CaseTeam, CaseType: CaseType, Contact: CaseContact, Relatedto: RelatedTo, RelatedToId: RelatedToId };
    

    $.ajax({
        type: "POST",
        url: "/Cases/CreateCase",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');

        },
        failure: function () {
            alert("Failed!");
        }
    })
    if (RelatedTo == "Opportunities") {
        OpportunitiesDetailsModal(RelatedToId);
    }
    else if (RelatedTo == "Contacts") {
        LoadModalForDetailsContacts_Case(RelatedToId);
    }
    else if (RelatedTo == "Account") {
        
    }

}

var LoadModalForDetailsContacts = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Contact/PartialDetailsOnId",
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

var LoadModalForDetailsContacts_Case = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Contact/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            //$('.modal-content').html('').html(response);
            $('#RightSideModal').modal('show');

            $('.modal-content-rightside').html('').html(response);
            $("#loader").css("display", "none");
        },
        failure: function () {
            alert("Failed!");
            $('body').removeClass('m-page--loading');
        }
    });
}

var OpportunitiesDetailsModal = function (id) {
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");


    $.ajax({
        type: "GET",
        url: "/Opportunities/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (response) {
            $('#RightSideModal').modal('show');

            $('.modal-content-rightside').html('').html(response);
            $("#loader").css("display", "none");

        },
        failure: function () {
            alert("Failed!");
            //mApp.unblockPage();
            $('body').removeClass('m-page--loading');
        }
    });
}

var LoadModalForDetailsGeneral = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    //$('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    console.log("Olamba");
    $("#loader").css("display", "block");
   
    $.ajax({
        type: "GET",
        url: "/Cases/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",

        success: function (response) {
            //$('.modal-content').html('').html(response);
        $('#RightSideModal').modal('show');

            $('.modal-content-rightside').html('').html(response);
            $("#loader").css("display", "none");

        },

        failure: function () {
            alert("Failed!");
            $("#loader").css("display", "none");
        }

    });
}

var LoadModalForDetails_Case = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Cases/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",

        success: function (response) {
            console.log("responsecase");
            console.log(response);
           
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

var LoadModalForDetails_CaseUser = function (id, userid) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");
   
    $.ajax({
        type: "GET",
        url: "/Cases/PartialDetailsOnIdUser",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",

        success: function (response) {
            console.log("responsecase");
            console.log(response);
         
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

var AcceptCase = function (userId, caseId) {
    
    var obj = { CaseId: caseId, UserId: userId};

    $.ajax({
        type: "POST",
        url: "/Users/AcceptCase",
        data: obj,
        dataType: "text",
        success: function (response) {

        },
        failure: function () {
            alert("Failed!");
        }
    })

}

var RejectCase = function (userId, caseId) {
    var obj = { CaseId: caseId, UserId: userId };

    $.ajax({
        type: "POST",
        url: "/Users/RejectCase",
        data: obj,
        dataType: "text",
        success: function (response) {

        },
        failure: function () {
            alert("Failed!");
        }
    })
}

var ResolveCase = function (id) {
    $('.details').html('').html('');
    $('#ResolveCase').css("display", "block");
    $('#caseNotesButtons').css({
        'cssText': 'display: block !important'
    });
    $('#caseButtons').css({
        'cssText': 'display: none !important'
    });
    $('#caseActivityButtons').css({
        'cssText': 'display: none !important'
    });
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}

var ResolveCaseUser = function () {
    $('.details').html('').html('');
    $('#ResolveCase').css("display", "block");
    $('#caseButtons').css({
        'cssText': 'display: none !important'
    });
    $('#caseResolveButtons').css({
        'cssText': 'display: none !important'
    });
    $('#ResolveButtons').css({
        'cssText': 'display: block !important'
    });
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}

var ResolveCaseFromUser = function (userId, caseId) {
    var resolution = document.getElementById("CaseResolution").value;
    var totalTime = document.getElementById("CaseTotalTime").value;
    var billableTime = document.getElementById("CaseBillableTime").value;
    var desc = document.getElementById("CaseRemarks").value;
    var resolutionType = $('#ResolutionType option:selected').val();
    var obj = { CaseId: caseId, Resolution: resolution, ResolutionType: resolutionType, TotalTime: totalTime, BillableTime: billableTime, Remarks: desc, UserId: userId };

    $.ajax({
        type: "POST",
        url: "/Cases/ResolveCaseUser",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');
            LoadModalForDetailsCase(caseId);

        },
        failure: function () {
            alert("Failed!");
        }
    })

}

var ReActivateCase = function (id) {
    var obj = { CaseId: id};

    $.ajax({
        type: "POST",
        url: "/Cases/ReActivateCase",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');
            LoadModalForDetailsCase(id);
        },
        failure: function () {
            alert("Failed!");
        }
    })
}

var CaseResolution = function () {
    var caseId = document.getElementById("caseId").textContent;
    var resolution = document.getElementById("CaseResolution").value;
    var totalTime = document.getElementById("CaseTotalTime").value;
    var billableTime = document.getElementById("CaseBillableTime").value;
    var desc = document.getElementById("CaseRemarks").value;
    var resolutionType = $('#ResolutionType option:selected').val();
    var obj = { CaseId: caseId, Resolution: resolution, ResolutionType: resolutionType, TotalTime: totalTime, BillableTime: billableTime, Remarks: desc };

    $.ajax({
        type: "POST",
        url: "/Cases/ResolveCase",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');
            LoadModalForDetailsCase(caseId);

        },
        failure: function () {
            alert("Failed!");
        }
    })

}

var ActivityCreate = function () {
    var caseId = document.getElementById("caseId").textContent;
    var name = document.getElementById("ActivityNameValueofcases").value;
    var desc = document.getElementById("ActivityDescriptionCase").value;
    var user = $('#CaseUserActivity option:selected').val();
    var team = $('#CaseTeamActivity option:selected').val();
    var status = $('#CaseStatusActivity option:selected').val();
    var type = $('#ActivityTypeCase option:selected').val();
   
    var obj = { Name: name, Desc: desc, AUser: user, Team: team, Status: status, Type: type, Relatedto: "Cases", RelatedToId: caseId };


    $.ajax({
        type: "POST",
        url: "/Activities/CreateAfromCases",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');

            LoadModalForDetailsCase(caseId);

        },
        failure: function () {
            alert("Failed!");
        }
    })


}

var CreateNote = function () {
    var caseId = document.getElementById("caseId").textContent;
    var note = document.getElementById("noteContent").value;
    var obj = { Note: note, Relatedto: "Cases", RelatedToId: caseId };
    $.ajax({
        type: "POST",
        url: "/Cases/CreateNote",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');

            LoadModalForDetailsCase(caseId);

        },
        failure: function () {
            alert("Failed!");
        }
    })

}

var LoadModalForDetailsCase = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    //$('#RightSideModal').modal('show');
    console.log("Olamba");
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");
    $.ajax({
        type: "GET",
        url: "/Cases/PartialDetailsOnId",
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
            $("#loader").css("display", "none");
        }

    });
}

var CreateCaseActivity = function ()    {
    $('.details').html('').html('');
    $('#createActivity').css("display", "block");
    $('#caseActivityButtons').css({
        'cssText': 'display: block !important'
    });
    $('#caseButtons').css({
        'cssText': 'display: none !important'
    });
    $('#caseNotesButtons').css({
        'cssText': 'display: none !important'
    });
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}

var ScheduleCaseForm = function () {
    $('.details').html('').html('');
    $('#scheduleCase').css("display", "block");
    $('#caseScheduleButtons').css({
        'cssText': 'display: block !important'
    });
    $('#caseButtons').css({
        'cssText': 'display: none !important'
    });
    $('#caseNotesButtons').css({
        'cssText': 'display: none !important'
    });
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}

var CreateCaseNote = function () {
    $('#noteDetails').css("display", "none");
    $('#createNote').css("display", "block");
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
   
}

//Contact Cases

var CreateContactCase = function () {
    $('#DetialsContact').css("display", "none");
    $('#createCaseContact').css("display", "block");
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}
var CancelCaseContact = function () {
    $('#DetialsContact').css("display", "block");
    $('#createCaseContact').css("display", "none");
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}

var CaseCreateContact = function (RelatedToId) {
    var CaseTitle = $('#CaseTitleContact').val();
    var CaseDescription = $('#CaseDescriptionContact').val();
    var CaseOrigin = $('#CaseOriginContact').val();
    var CaseType = $('#CaseTypeContact option:selected').val();
    var CaseUser = $('#AssignedUserCaseContact option:selected').val();
    var CaseTeam = $('#AssignedTeamCaseContact option:selected').val();
    CaseCreate("Contacts", RelatedToId, CaseTitle, CaseType, CaseDescription, "", CaseTeam, CaseUser, CaseOrigin);


}

var ScheduleCase = function () {
    var CaseId = document.getElementById("caseId").textContent;
    var CaseDate = $('#scheduleDate').val();
    var CaseTime = $('#scheduleTime').val();
    var CaseTeam = $('#CaseTeamSchedule option:selected').val();
    var CaseUser = $('#CaseUserSchedule option:selected').val();

    var obj = { CaseId: CaseId, CaseDate: CaseDate, CaseTime: CaseTime, CaseTeam: CaseTeam, CaseUser: CaseUser};
   

    $.ajax({
        type: "POST",
        url: "/Cases/ScheduleCase",
        data: obj,
        dataType: "text",
        success: function (response) {
            $('.modal-body').html('').html('');

        },
        failure: function () {
            alert("Failed!");
        }
    })

}

var LoadCase = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");
  
    $.ajax({
        type: "GET",
        url: "/Cases/PartialDetailsOnId",
        data: { id: id },
        contentType: "application/json; charset=utf-8",
        dataType: "text",

        success: function (response) {
         
            //$('.modal-content').html('').html(response);
            $('#RightSideModal').modal('show');
            $('.modal-content-rightside').html('').html(response);
            $("#loader").css("display", "none");

        },

        failure: function () {
            alert("Failed!");
            $("#loader").css("display", "none");
        }

    });
}

//Opportunity Cases

var CreateOppCase = function () {
    $('#DetialsOpp').css("display", "none");
    $('#createCase').css("display", "block");
    $('#OpportunityButton').css("display", "none !important");
    $('#CreateCaseButtonsOpportunity').css("display", "block");
    $('#CreateCaseButtonsOpportunity').css({
        'cssText': 'display: block !important'
    });
    $('#OpportunityButton').css({
        'cssText': 'display: none !important'
    });
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
  
}

var CancelCase = function () {
    $('#DetialsOpp').css("display", "block");
    $('#createCase').css("display", "none");
    $('#OpportunityButton').css("display", "block");
    $('#CreateCaseButtonsOpportunity').css("display", "none");
    $('select').each(function () {
        $(this).select2({
            dropdownParent: $(this).parent(), width: '100%'
        });
    });
}

var CaseCreateOpportunity = function (RelatedToId) {
    var CaseTitle = $('#CaseTitleOpportunity').val();
    var CaseDescription = $('#CaseDescriptionOpportunity').val();
    var CaseType = $('#CaseTypeOpportunity option:selected').val();
    var CaseContact = $('#ContactCase option:selected').val();
    var CaseUser = $('#AssignedUserCase option:selected').val();
    var CaseTeam = $('#AssignedTeamCase option:selected').val();
    CaseCreate("Oppertunities", RelatedToId, CaseTitle, CaseType, CaseDescription, CaseContact, CaseTeam, CaseUser, "");

}

//ContactDetailmodel
var LoadModalForDetails_Contact = function (id) {
    // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
    // in master page use in EasyautoCompleteSearch
    $('#RightSideModal').modal('show');
    $('.modal-content-rightside').html('').html('');

    $("#loader").css("display", "block");

    $.ajax({
        type: "GET",
        url: "/Contact/PartialDetailsOnId",
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