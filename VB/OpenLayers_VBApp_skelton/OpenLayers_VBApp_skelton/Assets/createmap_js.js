var __map = null;
var __view = null;
var __baseLayer = null;

function loadMap() {
    if (typeof constantsInitialBaseLayer !== 'undefined') {
        __baseLayer = getBaseLayer(constantsInitialBaseLayer);
    } else {
        __baseLayer = getBaseLayer(null);
    };

    __view = new ol.View({
        projection: "EPSG:3857",
        center: ol.proj.transform([138.336744, 38.748977], "EPSG:4326", "EPSG:3857"),
        minZoom: 0,
        maxZoom: 18,
        zoom: 4.5
    });

    __map = new ol.Map({
        target: 'map',
        renderer: [
            'canvas',
            'dom'
        ],
        layers: [
            __baseLayer
        ],
        view: __view
    });
};

function getBaseLayer(layerid) {
    if (layerid === null || layerid === 1) {
        // 国土地理院
        return new ol.layer.Tile({
            source: new ol.source.XYZ({
                url: 'https://maps.gsi.go.jp/xyz/std/{z}/{x}/{y}.png',
                attributions: "国土地理院(https://maps.gsi.go.jp/development/ichiran.html)",
                projection: "EPSG:3857"
            })
        });

    } else if (layerid === 2) {
        // OpenStreetMap
        return new ol.layer.Tile({
            source: new ol.source.OSM()
        });

    } else if (layerid === 3) {
        // 国土地理院航空写真
        return new ol.layer.Tile({
            source: new ol.source.XYZ({
                url: 'https://maps.gsi.go.jp/xyz/seamlessphoto/{z}/{x}/{y}.jpg',
                attributions: "国土地理院(https://maps.gsi.go.jp/development/ichiran.html) / GRUS画像((c) Axelspace)",
                projection: "EPSG:3857"
            })
        });
    } else {
        return null;
    };
};

function changeBaseLayer(layerid) {
    var tempBaseLayer = getBaseLayer(layerid);

    if (tempBaseLayer !== null && __map !== null) {
        if (__baseLayer !== null) {
            __map.removeLayer(__baseLayer);
        };
        __baseLayer = tempBaseLayer;
        __map.addLayer(__baseLayer);
        __baseLayer.setZIndex(0);
    };
};