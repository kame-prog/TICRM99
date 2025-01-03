var chart,
  options = {
    chart: {
      height: 320,
      type: "area",
      width: "100%",
      stacked: !0,
      toolbar: { show: !1, autoSelected: "zoom" },
    },
    colors: ["#2a77f4", "#a5c2f1"],
    dataLabels: { enabled: !1 },
    stroke: {
      curve: "smooth",
      width: [1.5, 1.5],
      dashArray: [0, 4],
      lineCap: "round",
    },
    grid: { padding: { left: 0, right: 0 }, strokeDashArray: 3 },
    markers: { size: 0, hover: { size: 0 } },
    series: [
      {
        name: "New Visits",
        data: [0, 60, 20, 90, 45, 110, 55, 130, 44, 110, 75, 120],
      },
      {
        name: "Unique Visits",
        data: [0, 45, 10, 75, 35, 94, 40, 115, 30, 105, 65, 110],
      },
    ],
    xaxis: {
      type: "month",
      categories: [
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
      axisBorder: { show: !0 },
      axisTicks: { show: !0 },
    },
    fill: {
      type: "gradient",
      gradient: {
        shadeIntensity: 1,
        opacityFrom: 0.4,
        opacityTo: 0.3,
        stops: [0, 90, 100],
      },
    },
    tooltip: { x: { format: "dd/MM/yy HH:mm" } },
    legend: { position: "top", horizontalAlign: "right" },
  };
(chart = new ApexCharts(document.querySelector("#crm-dash"), options)).render(),
  (options = {
    chart: { height: 205, type: "donut" },
    plotOptions: { pie: { donut: { size: "85%" } } },
    dataLabels: { enabled: !1 },
    stroke: { show: !0, width: 2, colors: ["transparent"] },
    series: [10, 65, 25],
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
    labels: ["Sent", "Opened", "Not Opened"],
    colors: ["#fdb5c8", "#2a76f4", "#67c8ff"],
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
          return e + " %";
        },
      },
    },
  }),
  (chart = new ApexCharts(
    document.querySelector("#email_report"),
    options
  )).render();
