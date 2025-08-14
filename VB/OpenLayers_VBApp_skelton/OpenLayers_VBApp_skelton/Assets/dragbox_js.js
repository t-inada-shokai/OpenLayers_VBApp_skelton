var __rectangleSelectionInteraction = null;

//インタラクション（相互作用）の指定
//範囲指定の指定
function setSelectionRectangleInteraction(modeOn) {
    if (modeOn === null || modeOn === false) {
        // mode -> off
        if (__rectangleSelectionInteraction !== null) {
            __map.removeInteraction(__rectangleSelectionInteraction);
            __rectangleSelectionInteraction = null;
        };
    } else {
        // mode -> on
        if (__rectangleSelectionInteraction === null) {
            __rectangleSelectionInteraction = new ol.interaction.DragBox({
                condition: ol.events.condition.always,
            });
            __map.addInteraction(__rectangleSelectionInteraction);
            __rectangleSelectionInteraction.on('boxend', function (e) {
                var extent = ol.proj.transformExtent(__rectangleSelectionInteraction.getGeometry().getExtent(), 'EPSG:3857', 'EPSG:4326');

                var messages = [];
                message = {
                    "Type": "EventMessage",
                    "Command": "RectangleSelected",
                    "Parameters": [
                        {
                            "Name": "Extent",
                            "Value": String(extent[0]) + "," + String(extent[1]) + "," + String(extent[2]) + "," + String(extent[3])
                        }
                    ]
                };
                messages.push(message);

                window.chrome.webview.postMessage(JSON.stringify(messages));
            });
        };
    };
};
