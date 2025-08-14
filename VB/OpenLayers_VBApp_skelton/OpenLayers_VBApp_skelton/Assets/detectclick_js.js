const __info = document.getElementById('info');
var __currentfeature = null;
const __displayFeatureInfo = function (feature) {
    if (typeof feature !== 'undefined'
        && feature.getProperties().label != null
        && feature !== __currentfeature) {

        const pixel = __map.getPixelFromCoordinate(feature.getGeometry().getCoordinates());

        __info.style.left = pixel[0] + 10 + 'px';
        __info.style.top = pixel[1] + 10 + 'px';
        __info.style.visibility = 'visible';
        __info.innerHTML = feature.getProperties().label;
        __currentfeature = feature;
    } else {
        __info.style.visibility = 'hidden';
        __currentfeature = undefined;
    };
};
const __moveFeatureInfo = function () {
    if (typeof __currentfeature !== 'undefined'
        && __info.style.visibility == 'visible') {
        const pixel = __map.getPixelFromCoordinate(__currentfeature.getGeometry().getCoordinates());

        __info.style.left = pixel[0] + 10 + 'px';
        __info.style.top = pixel[1] + 10 + 'px';
    };
};
function entryMapEvent() {
    __map.on('click', function (e) {
        var message = null;
        var feature = __map.forEachFeatureAtPixel(e.pixel,
            function (feature) {
                return feature;
            });
        __displayFeatureInfo(feature, e.pixel);
        if (feature) {
            var lonlat = feature.getGeometry().getCoordinates();
            if (typeof lonlat[0] === 'object') {
                lonlat = lonlat[0];
            };
            var uid = feature.getId();
            var child_uid = '';
            if (feature.getProperties() != null) {
                if (feature.getProperties().parentid != null) {
                    child_uid = uid;
                    uid = feature.getProperties().parentid;
                };
            };
            message = {
                "Type": "EventMessage",
                "Command": "FeatureClicked",
                "Parameters": [
                    {
                        "Name": "Uid",
                        "Value": uid
                    },
                    {
                        "Name": "ChildUid",
                        "Value": child_uid
                    },
                    {
                        "Name": "Longitude",
                        "Value": String(lonlat[0])
                    },
                    {
                        "Name": "Latitude",
                        "Value": String(lonlat[1])
                    }
                ]
            };
        } else {
            const lonlat = ol.proj.transform(e.coordinate, 'EPSG:3857', 'EPSG:4326');
            message = {
                "Type": "EventMessage",
                "Command": "MapClicked",
                "Parameters": [
                    {
                        "Name": "Longitude",
                        "Value": String(lonlat[0])
                    },
                    {
                        "Name": "Latitude",
                        "Value": String(lonlat[1])
                    }
                ]
            };
        };

        window.chrome.webview.postMessage(JSON.stringify([message]));
    });

    if (__dragMoveFeatureInteraction !== null) {
        __dragMoveFeatureInteraction.on('translateend', function (e) {
            var features = [];
            e.features.forEach(function (b) {
                var coord = b.getGeometry().getCoordinates();
                var uid = b.getId();
                var child_uid = '';
                if (b.getProperties() != null && b.getProperties().parentid != null) {
                    child_uid = uid;
                    uid = b.getProperties().parentid;
                };
                features.push(
                    {
                        "uid": uid,
                        "child_uid": child_uid, 
                        "coordinates": ol.proj.transform(coord, 'EPSG:3857', 'EPSG:4326')
                    }
                );
            });
            if (features.length > 0) {
                var messages = [];
                for (var i = 0; i < features.length; i++) {
                    messages.push(
                        {
                            "Type": "EventMessage",
                            "Command": "FeatureDragged",
                            "Parameters": [
                                {
                                    "Name": "Uid",
                                    "Value": features[i].uid
                                },
                                {
                                    "Name": "ChildUid",
                                    "Value": features[i].child_uid
                                },
                                {
                                    "Name": "Longitude",
                                    "Value": String(features[i].coordinates[0])
                                },
                                {
                                    "Name": "Latitude",
                                    "Value": String(features[i].coordinates[1])
                                }
                            ]
                        }
                    );
                };
                window.chrome.webview.postMessage(JSON.stringify(messages));
            };
        });
    };
};
