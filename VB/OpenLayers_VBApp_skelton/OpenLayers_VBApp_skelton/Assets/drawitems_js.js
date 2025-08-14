// マップ上に点を追加

var __markerStyle = null;
var __JSONStyle = null;
var __JSONStyleFunc = null;
var __featureLayer = null;
var __geojsonSource = null;
var __geojsonLayer = null;
var __dragMoveFeatureInteraction = null;

// 標準のマーカーのスタイルを設定
function setMarkerStyle() {
    if (__markerStyle === null) {
        __markerStyle = {};
        __markerStyle.defaultMarker = new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [0.5, 1],
                anchorXUnits: 'fraction',
                anchorYUnits: 'fraction',
                opacity: 1,
                src: 'marker.png',
                scale: 0.45
            })
        });
    };
};

//Featureを描画するLayerを追加
function setLayer() {
    if (__featureLayer === null) {
        //ベクタレイヤの追加
        __featureLayer = new ol.layer.Vector(
            {
                source: new ol.source.Vector(),
                name: "FeatureLayer"
            }
        );
        __map.addLayer(__featureLayer);
        __featureLayer.setZIndex(20);
    };
};

//外部JSONを読み込み描画するLayerを追加
//外部JSONのスタイルを指定
function setJSONLayer() {
    if (__geojsonLayer === null && __JSONStyle === null && __JSONStyleFunc === null && __geojsonSource === null) {
        const markerScale = 0.4;

        // 外部JSONのスタイル定義
        __JSONStyle = {
            'Point': new ol.style.Style({
                image: new ol.style.Circle({
                    radius: 10,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(52, 152, 219, 1.0)',
                        width: 5
                    }),
                    fill: new ol.style.Fill({
                        color: 'rgba(52, 152, 219, 0.4)'
                    })
                })
            }),
            'LineString': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'rgba(241, 196, 15, 0.6)',
                    width: 20
                })
            }),
            'Polygon': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'rgba(255, 0, 0, 1.0)',
                    width: 2
                }),
                fill: new ol.style.Fill({
                    color: 'rgba(255, 0, 0, 0.4)'
                })
            }),
            // ここからドライブログ専用のスタイル
            // 走行開始地点=Start
            'Point_pathpoint_0': new ol.style.Style({
                image: new ol.style.Icon({
                    src: 'start.png',
					anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    size: [100, 100],
                    scale: markerScale
                })
            }),
            // 走行終了地点=Goal
            'Point_pathpoint_1': new ol.style.Style({
                image: new ol.style.Icon({
                    src: 'goal.png',
					anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    size: [100, 100],
                    scale: markerScale
                })
            }),
            // 警告イベント level=0
            'Point_event_0': new ol.style.Style({
                image: new ol.style.Icon({
    				anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    src: 'cautionYellow.png', 	// 危険（赤）も要注意（黄）に変更
                    size: [91, 87],
                    scale: markerScale
                })
            }),
            // 注意イベント level=1
            'Point_event_1': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    src: 'cautionYellow.png',
                    size: [91, 87],
                    scale: markerScale
                })
            }),
            // 急加速イベント level=2
            'Point_event_2': new ol.style.Style({
                image: new ol.style.Icon({
                    src: 'ico01.png',
				    anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    size: [92, 140],
                    scale: markerScale
                })
            }),
            // 急減速イベント level=3
            'Point_event_3': new ol.style.Style({
                image: new ol.style.Icon({
                    src: 'ico02.png',
    				anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    size: [92, 140],
                    scale: markerScale
                })
            }),
            // 逆走イベント level=4
            'Point_event_4': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    src: 'mapico_Reverse.png',
                    size: [118, 205],
                    scale: markerScale
                })
            }),
            // 圏外走行イベント level=5
            'Point_event_5': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    src: 'mapico_out_area.png',
                    size: [118, 205],
                    scale: markerScale
                })
            }),
            // 緊急自動通報イベント level=6
            'Point_event_6': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1,
                    src: 'mapico_emergency.png',
                    size: [118, 205],
                    scale: markerScale
                })
            }),
            // 走行経路
            'LineString_path_0': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'rgba(0, 0, 0, 0.5)',
                    width: 10
                })
            }),
            // 不良状態 level=0
            'LineString_status_0': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: '#F8868D',
                    width: 5
                })
            }),
            // 普通状態 level=1
            'LineString_status_1': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: '#CBCBCB',
                    width: 5
                })
            }),
            // 良好状態 level=2
            'LineString_status_2': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: '#049F95',
                    width: 5
                })
            }),
            // 速度超過状態 level=10
            'LineString_status_10': new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: '#ff8d02',
                    width: 7
                })
            }),
            // 一時停車状態 level=1
            'Point_pause_1': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    src: 'pause.png',
                    size: [100, 100],
                    scale: markerScale
                })
            }),
            // 駐車状態 level=1
            'Point_ex_parking_1': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    src: 'parking.png',
                    size: [100, 100],
                    scale: markerScale
                })
            }),
            // 基地待機状態 level=1
            'Point_ex_waitbase_1': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    src: 'parking.png',
                    size: [100, 100],
                    scale: markerScale
                })
            }),
            // 基地周辺所在不明 level=1
            'Point_ex_nearbase_1': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    src: 'missing.png',
                    size: [100, 100],
                    scale: markerScale
                })
            }),
            // 所在不明 level=1
            'Point_ex_missing_1': new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 0.5],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 0.8,
                    src: 'missing.png',
                    size: [100, 100],
                    scale: markerScale
                })
            }),
       };
        
        // 外部JSONのスタイル選択関数
        __JSONStyleFunc = function (feature) {
            var key = feature.getGeometry().getType();
            if (feature.getProperties().levelKind != undefined) {
                key += '_' + feature.getProperties().levelKind
                    + '_' + feature.getProperties().levelValue
            };
//            return __JSONStyle[feature.getGeometry().getType()];
            return __JSONStyle[key];
        };

        //GeoJSONレイヤで用いるSourceの追加
        __geojsonSource = new ol.source.Vector(
            {
                url: 'geojson_json.json',
                format: new ol.format.GeoJSON()
            }
        );

        //GeoJSONレイヤの追加
        __geojsonLayer = new ol.layer.Vector(
            {
                source: __geojsonSource,
                style: __JSONStyleFunc,
                name: 'GeoJSONLayer'
            }
        );
        __map.addLayer(__geojsonLayer);
        __geojsonLayer.setZIndex(10);
    };
};

//インタラクション（干渉？）の指定
//ドラッグや範囲指定などの指定
function setDragMoveFeatureInteraction() {
    if (__dragMoveFeatureInteraction === null) {
        __dragMoveFeatureInteraction = new ol.interaction.Translate({
            layers: [ __featureLayer ]
        });
        __map.addInteraction(__dragMoveFeatureInteraction);
    };
};

//Markerを追加する
function insertMarkerOnMap(uid, lon, lat, markerStyleJSON) {
    var f = new ol.Feature(new ol.geom.Point(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857')));
    if (markerStyleJSON) {
        f.setStyle(
            new ol.style.Style(
                JSON.parse(markerStyleJSON)
            )
        );
    } else {
        f.setStyle(__markerStyle.defaultMarker);
    };
    f.setId(uid);
    __featureLayer.getSource().addFeature(f);
};

//uidで指定したFeatureを削除する
function removeMarkerOnMap(uid) {
    var f = __featureLayer.getSource().getFeatureById(uid);
    if (f) {
        __featureLayer.getSource().removeFeature(f);
    };
};

//全てのFeatureを削除する
function removeAllMarkerOnMap() {
    var fs = __featureLayer.getSource().getFeatures();
    fs.forEach((f) => {
        __featureLayer.getSource().removeFeature(f);
    });
};
