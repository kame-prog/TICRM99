var ctx1 = document.getElementById("lineChart").getContext("2d"),
    myChart = new Chart(ctx1, {
        type: "line",
        data: {
            labels: [
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec",
            ],
            datasets: [
                {
                    label: "Monthly Report",
                    data: [2, 1, 3, 6, 2, 1, 2, 6, 5, 4, 2, 1],
                    backgroundColor: ["rgba(11, 81, 183, 0.1)"],
                    borderColor: ["rgba(11, 81, 183, 1)"],
                    borderWidth: 2,
                    borderDash: [3],
                    borderJoinStyle: "round",
                    borderCapStyle: "round",
                    pointBorderColor: "rgba(11, 81, 183, 1)",
                    pointRadius: 3,
                    pointBorderWidth: 1,
                    tension: 0.3,
                },
                {
                    label: "Monthly Report",
                    data: [8, 2, 5, 1, 8, 4, 6, 3, 0, 7, 3, 6],
                    backgroundColor: ["rgba(28, 202, 184, 0.1)"],
                    borderColor: ["rgba(28, 202, 184, 1)"],
                    borderWidth: 2,
                    borderDash: [0],
                    borderJoinStyle: "round",
                    borderCapStyle: "round",
                    pointBorderColor: "rgba(28, 202, 184, 1)",
                    pointRadius: 3,
                    pointBorderWidth: 1,
                    tension: 0.3,
                },
            ],
        },
        options: {
            maintainAspectRatio: !1,
            plugins: { legend: { labels: { color: "#7c8ea7" } } },
            scales: {
                y: {
                    beginAtZero: !0,
                    ticks: {
                        callback: function (r, o, e) {
                            return "$" + r;
                        },
                        color: "#7c8ea7",
                    },
                    grid: {
                        drawBorder: "border",
                        color: "rgba(132, 145, 183, 0.15)",
                        borderDash: [3],
                        borderColor: "rgba(132, 145, 183, 0.15)",
                    },
                    beginAtZero: !0,
                },
                x: {
                    ticks: { color: "#7c8ea7" },
                    grid: {
                        display: !1,
                        color: "rgba(132, 145, 183, 0.09)",
                        borderDash: [3],
                        borderColor: "rgba(132, 145, 183, 0.09)",
                    },
                },
            },
        },
    });



//    ctx2 = document.getElementById("bar").getContext("2d"),
//    ctx3 =
//        ((myChart = new Chart(ctx2, {
//            type: "bar",
//            data: {
//                labels: [
//                    "Jan",
//                    "Feb",
//                    "Mar",
//                    "Apr",
//                    "May",
//                    "Jun",
//                    "Jul",
//                    "Aug",
//                    "Sep",
//                    "Oct",
//                    "Nov",
//                    "Dec",
//                ],
//                datasets: [
//                    {
//                        label: "Monthly Report",
//                        data: [12, 19, 13, 9, 12, 11, 12, 19, 13, 9, 12, 11],
//                        borderRadius: 100,
//                        borderSkipped: !1,
//                        backgroundColor: "#367de4",
//                        borderColor: "#367de4",
//                        borderWidth: 1,
//                        indexAxis: "x",
//                        barThickness: 15,
//                        grouped: !0,
//                        maxBarThickness: 9,
//                        barPercentage: 50,
//                    },
//                    {
//                        label: "Monthly Report",
//                        data: [8, 12, 15, 11, 8, 14, 16, 13, 10, 7, 19, 16],
//                        borderRadius: 100,
//                        borderSkipped: !1,
//                        backgroundColor: "#1ccab8",
//                        borderColor: "#1ccab8",
//                        borderWidth: 1,
//                        indexAxis: "x",
//                        barThickness: 15,
//                        grouped: !0,
//                        maxBarThickness: 9,
//                    },
//                ],
//            },
//            options: {
//                maintainAspectRatio: !1,
//                responsive: !0,
//                plugins: {
//                    legend: { position: "top", labels: { color: "#7c8ea7" } },
//                    title: { display: !1, text: "Chart.js Bar Chart" },
//                },
//                scales: {
//                    y: {
//                        beginAtZero: !0,
//                        ticks: {
//                            callback: function (r, o, e) {
//                                return "$" + r;
//                            },
//                            color: "#7c8ea7",
//                        },
//                        grid: {
//                            drawBorder: "border",
//                            color: "rgba(132, 145, 183, 0.15)",
//                            borderDash: [3],
//                            borderColor: "rgba(132, 145, 183, 0.15)",
//                        },
//                        beginAtZero: !0,
//                    },
//                    x: {
//                        ticks: { color: "#7c8ea7" },
//                        grid: {
//                            display: !1,
//                            color: "rgba(132, 145, 183, 0.09)",
//                            borderDash: [3],
//                            borderColor: "rgba(132, 145, 183, 0.09)",
//                        },
//                    },
//                },
//            },
//        })),
//            document.getElementById("doughnut").getContext("2d")),
//    ctx4 =
//        ((myChart = new Chart(ctx3, {
//            type: "doughnut",
//            data: {
//                labels: ["Desktops", "Laptop", "Tablets", "Mobiles"],
//                datasets: [
//                    {
//                        data: [80, 50, 100, 121],
//                        backgroundColor: ["#4d79f6", "#ff5da0", "#e0e7fd", "#4ac7ec"],
//                        cutout: 100,
//                        radius: 100,
//                        borderColor: "transparent",
//                        borderRadius: 0,
//                        hoverBackgroundColor: ["#4d79f6", "#ff5da0", "#e0e7fd", "#4ac7ec"],
//                    },
//                ],
//            },
//            options: {
//                maintainAspectRatio: !1,
//                plugins: { legend: { labels: { color: "#7c8ea7" } } },
//            },
//        })),
//            document.getElementById("polarArea").getContext("2d")),
//    ctx5 =
//        ((myChart = new Chart(ctx4, {
//            type: "polarArea",
//            data: {
//                labels: ["Desktops", "Laptop", "Tablets", "Mobiles"],
//                datasets: [
//                    {
//                        data: [80, 50, 100, 121],
//                        backgroundColor: ["#4d79f6", "#ff5da0", "#e0e7fd", "#4ac7ec"],
//                        borderColor: "transparent",
//                        hoverBackgroundColor: ["#4d79f6", "#ff5da0", "#e0e7fd", "#4ac7ec"],
//                    },
//                ],
//            },
//            options: {
//                maintainAspectRatio: !1,
//                plugins: { legend: { labels: { color: "#7c8ea7" } } },
//            },
//        })),
//            document.getElementById("pie").getContext("2d")),
//    ctx6 =
//        ((myChart = new Chart(ctx5, {
//            type: "pie",
//            data: {
//                labels: ["Desktops", "Laptop", "Tablets", "Mobiles"],
//                datasets: [
//                    {
//                        data: [80, 50, 100, 121],
//                        backgroundColor: ["#4d79f6", "#ff5da0", "#e0e7fd", "#4ac7ec"],
//                        cutout: 0,
//                        radius: 100,
//                        borderColor: "transparent",
//                        borderRadius: 0,
//                        hoverBackgroundColor: ["#4d79f6", "#ff5da0", "#e0e7fd", "#4ac7ec"],
//                    },
//                ],
//            },
//            options: {
//                maintainAspectRatio: !1,
//                plugins: { legend: { labels: { color: "#7c8ea7" } } },
//            },
//        })),
//            document.getElementById("radar").getContext("2d"));
//myChart = new Chart(ctx6, {
//    type: "radar",
//    data: {
//        labels: [
//            "Jan",
//            "Feb",
//            "Mar",
//            "Apr",
//            "May",
//            "Jun",
//            "Jul",
//            "Aug",
//            "Sep",
//            "Oct",
//            "Nov",
//            "Dec",
//        ],
//        datasets: [
//            {
//                label: "Monthly Report",
//                data: [12, 19, 13, 9, 12, 11, 12, 19, 13, 9, 12, 11],
//                backgroundColor: ["rgba(11, 81, 183, 0.1)"],
//                borderColor: ["rgba(11, 81, 183, 1)"],
//                borderWidth: 2,
//                borderDash: [3],
//                borderJoinStyle: "round",
//                borderCapStyle: "round",
//                pointBorderColor: "rgba(11, 81, 183, 1)",
//                pointRadius: 3,
//                pointBorderWidth: 1,
//                tension: 0.3,
//                fill: !0,
//                hitRadius: 5,
//            },
//            {
//                label: "Monthly Report",
//                data: [8, 12, 15, 11, 8, 14, 16, 13, 10, 7, 19, 16],
//                backgroundColor: ["rgba(28, 202, 184, 0.1)"],
//                borderColor: ["rgba(28, 202, 184, 1)"],
//                borderWidth: 2,
//                borderDash: [0],
//                borderJoinStyle: "round",
//                borderCapStyle: "round",
//                pointBorderColor: "rgba(28, 202, 184, 1)",
//                pointRadius: 3,
//                pointBorderWidth: 1,
//                tension: 0.3,
//            },
//        ],
//    },
//    options: {
//        maintainAspectRatio: !1,
//        scales: {
//            r: {
//                angleLines: {
//                    display: !0,
//                    color: "rgba(132, 145, 183, 0.15)",
//                    borderDash: [2],
//                },
//            },
//        },
//        plugins: { legend: { labels: { color: "#7c8ea7" } } },
//    },
//});
