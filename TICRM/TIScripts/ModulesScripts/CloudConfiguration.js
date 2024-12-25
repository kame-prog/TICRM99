/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * CloudConfiguration script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/cloudconfiguration/index" || url == "/cloudconfiguration") {
    $('#savebtn').on('click', function () {

        $('body').addClass('m-page--loading');

        var UserName = $('#UserName').val();
        var Password = $('#Password').val();
        var OrganizationId = $('#OrganizationId').val();
        var APIKey = $('#APIKey').val();
        var AuthToken = $('#AuthToken').val();
        var DeviceType = $('#DeviceType').val();
        var DeviceId = $('#DeviceId').val();

        $.ajax({
            type: "GET",
            url: "/CloudConfiguration/ConfigureItToCloud",
            data: { UserName: UserName, Password: Password, OrganizationId: OrganizationId, APIKey: APIKey, AuthToken: AuthToken, DeviceType: DeviceType, DeviceId: DeviceId },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });

    });
}
//index Page Script Ends


//Edit Page Script Starts
if (url == "/cloudconfiguration/ibmcloudbrowse") {
    $('#savebtn').on('click', function () {

        $('body').addClass('m-page--loading');

        var UserName = $('#UserName').val();
        var Password = $('#Password').val();
        var OrganizationId = $('#OrganizationId').val();
        var APIKey = $('#APIKey').val();
        var AuthToken = $('#AuthToken').val();
        var DeviceType = $('#DeviceType').val();
        var DeviceId = $('#DeviceId').val();

        $.ajax({
            type: "GET",
            url: "/CloudConfiguration/ConfigureItToCloud",
            data: { UserName: UserName, Password: Password, OrganizationId: OrganizationId, APIKey: APIKey, AuthToken: AuthToken, DeviceType: DeviceType, DeviceId: DeviceId },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {

                $('body').removeClass('m-page--loading');
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });

    });
}
//Edit Page Script Ends
