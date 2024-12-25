var iconObject = L.icon({
    iconUrl: '/Content/Metronic/vendors/graphhopper/img/marker-icon.png',
    shadowSize: [50, 64],
    shadowAnchor: [4, 62],
    iconAnchor: [12, 40]
});


$(document).ready(function(e) {
    jQuery.support.cors = true;


    var host;

    //
    // Sign-up for free and get your own key: https://graphhopper.com/#directions-api
    //
    var defaultKey = "f8be18e9-3ab7-4874-9520-b385c841d82f";
    var profile = "car";

    // create a routing client to fetch real routes, elevation.true is only supported for vehicle bike or foot
    var ghRouting = new GraphHopper.Routing({ key: defaultKey, host: host, vehicle: profile, elevation: false });


    var routingMap = createMap('routing-map');
    setupRoutingAPI(routingMap, ghRouting);


});

function setupRoutingAPI(map, ghRouting) {

    map.setView([52.521235, 13.3992], 12);

    var instructionsDiv = $("#instructions");

    map.on('click', function(e) {
        if (ghRouting.points.length > 1) {
            ghRouting.clearPoints();
            routingLayer.clearLayers();
        }
        //Lahore
        e.latlng.lat = 31.520;
        e.latlng.lng = 74.3587;

        L.marker([31.20, 74.3587], { icon: iconObject }).addTo(routingLayer);

        //Karachi
        e.latlng.lat = 24.8607;
        e.latlng.lng = 67.0011;
        L.marker(e.latlng, { icon: iconObject }).addTo(routingLayer);

        // ghRouting.addPoint(new GHInput(e.latlng.lat, e.latlng.lng));
        ghRouting.addPoint(new GHInput(31.5204, 74.3587));
        ghRouting.addPoint(new GHInput(24.8607, 67.0011));

        if (ghRouting.points.length > 1) {
            // ******************
            //  Calculate route! 
            // ******************
            ghRouting.doRequest()
                .then(function(json) {
                    var path = json.paths[0];
                    routingLayer.addData({
                        "type": "Feature",
                        "geometry": path.points
                    });
                    var outHtml = "Distance in meter:" + path.distance;
                    outHtml += "<br/>Times in seconds:" + path.time / 1000;
                    outHtml += "<br/><a href='" + ghRouting.getGraphHopperMapsLink() + "'>GraphHopper Maps</a>";
                    $("#routing-response").html(outHtml);

                    if (path.bbox) {
                        var minLon = path.bbox[0];
                        var minLat = path.bbox[1];
                        var maxLon = path.bbox[2];
                        var maxLat = path.bbox[3];
                        var tmpB = new L.LatLngBounds(new L.LatLng(minLat, minLon), new L.LatLng(maxLat, maxLon));
                        map.fitBounds(tmpB);
                    }

                    instructionsDiv.empty();
                    if (path.instructions) {
                        var allPoints = path.points.coordinates;
                        var listUL = $("<ol>");
                        instructionsDiv.append(listUL);
                        for (var idx in path.instructions) {
                            var instr = path.instructions[idx];

                            // use 'interval' to find the geometry (list of points) until the next instruction
                            var instruction_points = allPoints.slice(instr.interval[0], instr.interval[1]);

                            // use 'sign' to display e.g. equally named images

                            $("<li>" + instr.text + " <small>(" + ghRouting.getTurnText(instr.sign) + ")</small>" +
                                " for " + instr.distance + "m and " + Math.round(instr.time / 1000) + "sec" +
                                ", geometry points:" + instruction_points.length + "</li>").appendTo(listUL);
                        }
                    }

                })
                .catch(function(err) {
                    var str = "An error occured: " + err.message;
                    $("#routing-response").text(str);
                });
        }
    });

    //start

    //End

    var instructionsHeader = $("#instructions-header");
    instructionsHeader.click(function() {
        instructionsDiv.toggle();
    });

    var routingLayer = L.geoJson().addTo(map);
    routingLayer.options = {
        style: { color: "#00cc33", "weight": 5, "opacity": 0.6 }
    };
}

function createMap(divId) {
    var osmAttr = '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors';

    var omniscale = L.tileLayer.wms('https://maps.omniscale.net/v1/ghexamples-3646a190/tile', {
        layers: 'osm',
        attribution: osmAttr + ', &copy; <a href="http://maps.omniscale.com/">Omniscale</a>'
    });

    var osm = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: osmAttr
    });

    var map = L.map(divId, { layers: [omniscale] });
    L.control.layers({
        "Omniscale": omniscale,
        "OpenStreetMap": osm
    }).addTo(map);
    return map;
}