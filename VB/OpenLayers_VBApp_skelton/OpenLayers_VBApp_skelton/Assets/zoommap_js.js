//地図の表示状態を取得
var __zoomInfo = {
    "zoom": 0,
    "center": [0, 0],
    "extent": [0, 0, 0, 0]
};

function entryZoomEvent() {
    __map.on('moveend', function () {
        __moveFeatureInfo();
        __zoomInfo.zoom = __view.getZoom();
        __zoomInfo.center = ol.proj.transform(__view.getCenter(), 'EPSG:3857', 'EPSG:4326');
        __zoomInfo.extent = ol.proj.transformExtent(
            __view.calculateExtent(__map.getSize()), 'EPSG:3857', 'EPSG:4326');

        var messages = [];
        message = {
            "Type": "EventMessage",
            "Command": "ZoomInfoChanged",
            "Parameters": [
                {
                    "Name": "Zoom",
                    "Value": __zoomInfo.zoom
                },
                {
                    "Name": "Center",
                    "Value": String(__zoomInfo.center[0])+","+String(__zoomInfo.center[1])
                },
                {
                    "Name": "Extent",
                    "Value": String(__zoomInfo.extent[0])+","+String(__zoomInfo.extent[1])+","+String(__zoomInfo.extent[2])+","+String(__zoomInfo.extent[3])
                }
            ]
        };
        messages.push(message);

        window.chrome.webview.postMessage(JSON.stringify(messages));
    });
};

//要素全体が表示されるようにズーム
function zoomMapAllFeatures() {
    const extent = __geojsonSource.getExtent();
    if (extent[0] !== Infinity && extent[1] !== Infinity && extent[2] !== -Infinity && extent[3] !== -Infinity) {
        __view.fit(extent, {padding: [10, 10, 10, 10]});
    };
};

//初回のマップズーム
function zoomMap() {
    if (typeof constantsInitialBox !== 'undefined') {
        const boxOnMap = ol.proj.transformExtent(constantsInitialBox, 'EPSG:4326', 'EPSG:3857');
        __view.fit(boxOnMap, { padding: [40, 40, 40, 40] });
    } else {
        const point = ol.proj.transform([138.336744, 38.748977], 'EPSG:4326', 'EPSG:3857');
        __view.setCenter(point);
        __view.setZoom(4.5);
    };
};

//ズーム設定
function setZoomInfo(arg) {
    if (arg.center !== undefined && arg.radius !== undefined) {
        var point = ol.proj.transform(arg.center, 'EPSG:4326', 'EPSG:3857');
        var extent = [point[0] - radius, point[1] - radius, point[0] + radius, point[1] + radius];
        __view.fit(extent, { padding: [10, 10, 10, 10] });
    } else if (arg.fence !== undefined) {
        var extent = ol.proj.transformExtent(arg.fence, 'EPSG:4326', 'EPSG:3857');
        __view.fit(extent, { padding: [10, 10, 10, 10] });
    } else if (arg.center !== undefined && arg.zoom !== undefined) {
        var point = ol.proj.transform(arg.center, 'EPSG:4326', 'EPSG:3857');
        __view.setCenter(point);
        __view.setZoom(arg.zoom);
    };
};