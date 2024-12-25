/**
 * Code By Muhammad Usman
 * 24/7/2020
 * Dashboard script file Containing all the scripts it use
 */




$(document).ready(function () {

    //**************************************//
    //  This Week, Month, Year Sales jQuery //
    //**************************************//

    //All Year Sales jQuery
    $.ajax({
          
        url: "/Dashboard/GetAllYearSale",
        method: "POST",
        success: function (data) {
            data = JSON.parse(data);
            $("#SalesSum").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
    $('#AllYearSale').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetAllYearSale",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#SalesSum").text(data);
                $("#DropDownName").html("All Year <i class='las la-angle-down ms-1'></i>" );
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Week Sales jQuery
    $('#ThisWeek').click(function (event) {
        $.ajax({
            
            url: "/Dashboard/GetThisWeekSales",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#SalesSum").text(data);
                $("#DropDownName").html("This Week <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Month Sales jQuery
    $('#ThisMonth').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisMonthSales",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#SalesSum").text(data);
                $("#DropDownName").html("This Month <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Year Sales jQuery
    $('#ThisYear').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisYearSales",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#SalesSum").text(data);
                $("#DropDownName").html("This Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })


    //**************************************//
    //  This Week, Month, Year Cost jQuery  //
    //**************************************//

    //All Year Opportunity jQuery
    $.ajax({
        url: "/Dashboard/GetAllYearOpp",
        method: "POST",
        success: function (data) {
            data = JSON.parse(data);
            $("#OppCount").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
    $('#AllYearOpp').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetAllYearOpp",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#OppCount").text(data);
                $("#OppDropDown").html("All Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Week Opportunity jQuery
    $('#WeekOpp').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisWeekOpp",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#OppCount").text(data);
                
                $("#OppDropDown").html("This Week <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Month Opportunity jQuery
    $('#MonthOpp').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisMonthOpp",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#OppCount").text(data);
                $("#OppDropDown").html("This Month <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Year Opportunity jQuery
    $('#YearOpp').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisYearOpp",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#OppCount").text(data);
                $("#OppDropDown").html("This Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })

    //********************************************//
    //  This Week, Month, Year Work-Order jQuery  //
    //********************************************//

    //All Year Work-Order jQuery
    $.ajax({
        url: "/Dashboard/GetAllYearWorkOrder",
        method: "POST",
        success: function (data) {
            data = JSON.parse(data);
            $("#WorkOrdercount").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
    $('#AllYearWorkOrder').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetAllYearWorkOrder",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#WorkOrdercount").text(data);
                $("#WorkOrderDropDown").html("All Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Week Work-Order jQuery
    $('#WeekWorkOrder').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisWeekWorkOrder",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#WorkOrdercount").text(data);
                $("#WorkOrderDropDown").html("This Week <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Month Work-Order jQuery
    $('#MonthWorkOrder').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisMonthWorkOrder",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#WorkOrdercount").text(data);
                $("#WorkOrderDropDown").html("This Month <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Year Work-Order jQuery
    $('#YearWorkOrder').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisYearWorkOrder",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#WorkOrdercount").text(data);
                $("#WorkOrderDropDown").html("This Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })



    //**************************************//
    //  This Week, Month, Year Lead jQuery  //
    //**************************************//

    //All Year Lead jQuery
    $.ajax({
        url: "/Dashboard/GetAllYearLead",
        method: "POST",
        success: function (data) {
            data = JSON.parse(data);
            $("#Leadcount").text(data);
            
        },
        error: function (err) {
            console.log(err);
        }
    })
    $('#AllYearLead').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetAllYearLead",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#Leadcount").text(data);
                $("#LeadDropDown").html("All Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Week Lead jQuery
    $('#WeekLead').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisWeekLead",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#Leadcount").text(data);
                $("#LeadDropDown").html("This Week <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Month Lead jQuery
    $('#MonthLead').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisMonthLead",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#Leadcount").text(data);
                $("#LeadDropDown").html("This Month <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Year Lead jQuery
    $('#YearLead').click(function (event) {
        $.ajax({
            url: "/Dashboard/GetThisYearLead",
            method: "POST",
            success: function (data) {
                data = JSON.parse(data);
                $("#Leadcount").text(data);
                $("#LeadDropDown").html("This Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })


       //*********************************************//
      //         Live Device Show in MAP jQuery      //
     //*********************************************//

    $.ajax({
        url: '/Dashboard/GetDevicemap',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            LiveDevicesMap(data);
         //   initMap(data);
        }
    });
    //$.ajax({
    //    url: '/Dashboard/GettrackingDevicesmap',
    //    type: 'GET',
    //    dataType: 'json',
    //    success: function (data) {
    //        // LiveDevicesMap(data);
    //        console.log('map results by me is ' + data);
    //        initMap(data);
    //    }
    //});
    


      //*********************************************//
     //          Cloud Device Count jQuery          //
    //*********************************************//


    /****  Connected Devices with Cloud Count jQuery  ****/
    $.ajax({
        url: '/Dashboard/GetCloudDevice',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            //if (data.includes(NaN)) {

            //}
       
            GetCloudDevice(data.lstCloudDevicePer, data.lstCloudDeviceLable);
            
        }
    });


       //*********************************************//
      //         Live Device info Table jQuery       //
     //*********************************************//
    $.ajax({
        url: '/Dashboard/GetLiveDeviceInfo',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            GetLiveDevice(data);
        }
    });

    //*********************************************//
//            Opportunity Stats  jQuery           //
//*********************************************//
    $.ajax({
        url: '/Dashboard/GetOppMonthWise',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var oppcount = [];
            var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]

            for (var i = 0; i < months.length; i++) {
                for (var j = 0; j < data.length; j++) {
                    if (months[i] == data[j].MonthName) {
                        oppcount[i] = data[j].COUNT;
                        break;
                    }
                    else
                    {
                        oppcount[i] = 0;
                    }
                }
            }
            OppMonthWise(oppcount);
        }
    });

      //*********************************************//
     //  This Week, Month, Year Lead Report jQuery  //
    //*********************************************//

    //All Years Lead Report jQuery
    $.ajax({
        url: '/Dashboard/GetAllYearsLeadRepo',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            assignToEventsColumns(data);
        }
    });
    function assignToEventsColumns(data) {
        var table = $('#LeadRTable').dataTable({
            pageLength: 8,
            "lengthChange": false,
            "searching": false,
            "bAutoWidth": false,
            "aaData": data,
            "bDestroy": true,

            "columns": [{
                "data": "LeadType"
            }, {
                "data": "LeadName"
            }, {
                "data": "LeadSource"
            }]
        })
    }

    $('#AllYearLeadRepo').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetAllYearsLeadRepo',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                assignToEventsColumns(data);
            }
        });
        function assignToEventsColumns(data) {
            var table = $('#LeadRTable').dataTable({
                pageLength: 8,
                "lengthChange": false,
                "searching": false,
                "bAutoWidth": false,
                "aaData": data,
                "bDestroy": true,

                "columns": [{
                    "data": "LeadType"
                }, {
                    "data": "LeadName"
                }, {
                    "data": "LeadSource"
                }]
            })
            $("#LeadRepoDropDown").html("All Years <i class='las la-angle-down ms-1'></i>");
        }
    })

    //This Week Lead Report jQuery
    $('#WeekLeadRepo').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetWeekLeadRepo',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                assignToEventsColumns(data);
            }
        });
        function assignToEventsColumns(data) {
            var table = $('#LeadRTable').dataTable({
                pageLength: 8,
                "lengthChange": false,
                "searching": false,
                "bAutoWidth": false,
                "aaData": data,
                "bDestroy": true,

                "columns": [{
                    "data": "LeadType"
                }, {
                    "data": "LeadName"
                }, {
                    "data": "LeadSource"
                }]
            })
            $("#LeadRepoDropDown").html("This Week <i class='las la-angle-down ms-1'></i>");
        }
    })

    //This Month Lead Report jQuery
    $('#MothLeadRepo').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetMonthLeadRepo',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                assignToEventsColumns(data);
            }
        });
        function assignToEventsColumns(data) {
            var table = $('#LeadRTable').dataTable({
                pageLength: 8,
                "lengthChange": false,
                "searching": false,
                "bAutoWidth": false,
                "aaData": data,
                "bDestroy": true,

                "columns": [{
                    "data": "LeadType"
                }, {
                    "data": "LeadName"
                }, {
                    "data": "LeadSource"
                }]
            })
            $("#LeadRepoDropDown").html("This Month <i class='las la-angle-down ms-1'></i>");
        }
    })

    //This Year Lead Report jQuery
    $('#YearLeadRepo').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetYearLeadRepo',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                assignToEventsColumns(data);
            }
        });
        function assignToEventsColumns(data) {
            var table = $('#LeadRTable').dataTable({
                pageLength: 8,
                "lengthChange": false,
                "searching": false,
                "bAutoWidth": false,
                "aaData": data,
                "bDestroy": true,

                "columns": [{
                    "data": "LeadType"
                }, {
                    "data": "LeadName"
                }, {
                    "data": "LeadSource"
                }]
            })
            $("#LeadRepoDropDown").html("This Year <i class='las la-angle-down ms-1'></i>");
        }
    })


      //*********************************************//
     // Today, This Week, This Year Activity jQuery //
    //*********************************************//

    //Today Activities:
    $.ajax({
        url: '/Dashboard/GetTodayActivities',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var activityInfoText = '';
            for (var i = 0; i < data.length; i++) {
                activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-user-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75' >Activity<span> " + data[i].Name + " </span>having type<span> " + data[i].Type + " </span> Was created by <span > " + data[i].AssignedUser + " </span> on <span id = 'createdDate' > " + data[i].createdDate + " </span></p></div></div></div>";
            }
            console.log(activityInfoText);
            $('.activity').html(activityInfoText);
        },
        error: function (err) {
            console.log(err);
        }
    })
    $('#TodayActivity').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetTodayActivities',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var activityInfoText = '';
                for (var i = 0; i < data.length; i++) {
                    activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-user-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75' >Activity<span> " + data[i].Name + " </span>having type<span> " + data[i].Type + " </span> Was created by <span > " + data[i].AssignedUser + " </span> on <span id = 'createdDate' > " + data[i].createdDate + " </span></p></div></div></div>";
                }
                console.log(activityInfoText);
                $('.activity').html(activityInfoText);
                $("#ActivityDropDown").html("Today <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })

    //Yesterday Activities
    $('#YesterdayActivity').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetYesterdayActivities',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var activityInfoText = '';
                for (var i = 0; i < data.length; i++) {
                    activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-user-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75' >Activity<span> " + data[i].Name + " </span>having type<span> " + data[i].Type + " </span> Was created by <span > " + data[i].AssignedUser + " </span> on <span id = 'createdDate' > " + data[i].createdDate + " </span></p></div></div></div>";
                }
                console.log(activityInfoText);
                $('.activity').html(activityInfoText);
                $("#ActivityDropDown").html("Yesterday <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //This Week Activities
    $('#WeekActivity').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetThisWeekActivities',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var activityInfoText = '';
                for (var i = 0; i < data.length; i++) {
                    activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-user-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75' >Activity<span> " + data[i].Name + " </span>having type<span> " + data[i].Type + " </span> Was created by <span > " + data[i].AssignedUser + " </span> on <span id = 'createdDate' > " + data[i].createdDate + " </span></p></div></div></div>";
                }
                console.log(activityInfoText);
                $('.activity').html(activityInfoText);
                $("#ActivityDropDown").html("This Week <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
    //All Year Activities
    $('#YearActivity').click(function (event) {
        $.ajax({
            url: '/Dashboard/GetAllYearsActivities',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var activityInfoText = '';
                for (var i = 0; i < data.length; i++) {
                    activityInfoText += "<div class='activity-info'><div class='icon-info-activity'><i class='las la-user-clock bg-soft-primary'></i></div><div class='activity-info-text'><div class='d-flex justify-content-between align-items-center'><p class='text-muted mb-0 font-13 w-75' >Activity<span> " + data[i].Name + " </span>having type<span> " + data[i].Type + " </span> Was created by <span > " + data[i].AssignedUser + " </span> on <span id = 'createdDate' > " + data[i].createdDate + " </span></p></div></div></div>";
                }
                console.log(activityInfoText);
                $('.activity').html(activityInfoText);
                $("#ActivityDropDown").html("This Year <i class='las la-angle-down ms-1'></i>");
            },
            error: function (err) {
                console.log(err);
            }
        })
    })

    




});

   //*********************************************//
  //         Live Device Show in MAP Function    //
 //*********************************************//
function LiveDevicesMap(data) {
    var mapMarkers = [];
    for (var x = 0; x < data.length; x++) {
        mapMarkers.push({ name: data[x].Name, coords: [data[x].Latitude, data[x].Longitude] });
        console.log("live device function is called " + data);
    }
    
    var map = new jsVectorMap({
        map: 'world',
        selector: '#device_map',
        zoomOnScroll: false,
        zoomButtons: true,
        selectedMarkers: [0, 2],
        markersSelectable: true,
        markers: mapMarkers,
        markerStyle: {
            initial: { fill: "#5c5cff" },
            selected: { fill: "#ff5da0" }
        }
    });
}

// Add this to your existing code, preferably in the initialization part
//function initMap(data) {
//    var mapMarkers = [];
//    for (var x = 0; x < data.length; x++) {
//        mapMarkers.push({ name: data[x].Name, coords: [data[x].Latitude, data[x].Longitude] });
        
//    }
//    console.log('function is called ' + data);
//    var map = new jsVectorMap({
//        map: 'world',
//        selector: '#device_maptrack',
//        zoomOnScroll: false,
//        zoomButtons: true,
//        selectedMarkers: [0, 2],
//        markersSelectable: true,
//        markers: mapMarkers,
//        markerStyle: {
//            initial: { fill: "#5c5cff" },
//            selected: { fill: "#ff5da0" }
//        }
//    });
   
//}

// Call the initMap function when the document is ready

       //*********************************************//
      //         Live Device info Table Function       //
     //*********************************************//

//On cloud live device, we use this method to open device modal popup on the dashboard.  
//$(document).on("click", ".DeviceGraphbtn", function (e) {
//    e.preventDefault();
//    $('#DeviceGraphModal').modal('show');
//});

function GetLiveDevice(data) {
    var table = $('#LiveDevice').dataTable({
        pageLength: 9,
        "lengthChange": false,
        "searching": false,
        "bAutoWidth": false,
        "aaData": data,
        "bDestroy": true,

        "columns": [
        //    {
        //    "data": "Name"
        //},
            {
            "data": null,
            "render": function (data, type, row) {
                return '<td><a href="#" class="" id="" onclick="GetDeviceType(\'' + row.DeviceId + '\', \'' + row.Name + '\')" data-bs-toggle="modal" data-animation="bounce">' + row.Name + '</a><input type="hidden" id="device_id_Graph"></td>';
            }
        },
            {
            "data": "Mac"
        }, {
            "data": "EMEINumber"
        }]
    })
}


        //*********************************************//
       //            Opportunity Stats  Function      //
      //*********************************************//



function OppMonthWise(oppcount) {
    var oppstats = oppcount;
    var BarChart,
        options = {
            chart: { height: 310, type: "bar", toolbar: { show: !1 + "$" } },
            plotOptions: {
                bar: { horizontal: !1, endingShape: "rounded", columnWidth: "20%" },
            },
            dataLabels: { enabled: !1 },
            stroke: { show: !0, width: 2, colors: ["transparent"] },
            colors: ["#4d79f6"],
            series: [
                { name: "Opportunity", data: oppstats },

            ],
            xaxis: {
                categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                axisBorder: { show: !0, color: "#bec7e0" },
                axisTicks: { show: !0, color: "#bec7e0" },
            },
            legend: { show: !1, position: "top", horizontalAlign: "right" },
            fill: { opacity: 1 },
            grid: {
                row: { colors: ["transparent", "transparent"], opacity: 0.2 },
                borderColor: "#f1f3fa",
                strokeDashArray: 3,
            },
            tooltip: {
                y: {
                    formatter: function (e) {
                        return "" + e;
                    },
                },
            },
        };
    (BarChart = new ApexCharts(
        document.querySelector("#OppStatsGraph"),
        options
    )).render();

}


      //*********************************************//
     //          Cloud Device Count Function        //
    //*********************************************//


function GetCloudDevice(values, labels) {

    var chart,

        options = {
            chart: { height: 205, type: "donut" },
            plotOptions: { pie: { donut: { size: "85%" } } },
            dataLabels: { enabled: !1 },
            stroke: { show: !0, width: 2, colors: ["transparent"] },
            series: values,
            legend: {
                show: !1,
                position: "bottom",
                horizontalAlign: "center",
                verticalAlign: "middle",
                floating: !1,
                fontSize: "14px",
                offsetX: 0,
                offsetY: 5,
            },
            labels: labels,
            colors: ["#669999", "#2a76f4", "#ff00aa", "#80d4ff", "#1a1aff"],
            responsive: [
                {
                    breakpoint: 600,
                    options: {
                        plotOptions: { donut: { customScale: 0.2 } },
                        chart: { height: 200 },
                        legend: { show: !1 },
                    },
                },
            ],
            tooltip: {
                y: {
                    formatter: function (e) {
                        
                    },
                },
            },
        };
    (chart = new ApexCharts(
        document.querySelector("#Clouddevice_graph"),
        options
    )).render();
}