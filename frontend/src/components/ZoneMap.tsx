import React, { useEffect, useRef, useState } from "react";
import { MapContainer, TileLayer, useMap } from "react-leaflet";
import L from "leaflet";
import "leaflet-draw";
import "leaflet/dist/leaflet.css";
import "leaflet-draw/dist/leaflet.draw.css";
import { createRoot } from "react-dom/client";
import { Button, Input, message, Modal } from "antd";
import { DeleteOutlined, EditOutlined, Loading3QuartersOutlined, PlusOutlined } from "@ant-design/icons";
import {
  addRepairZone,
  deleteRepairZone,
  getRepairZones,
  updateRepairZone,
} from "../services/repairZones";

interface LayerWithZoneId extends L.Layer {
  options: L.PathOptions & { repairZoneId?: number; name?: string };
}

interface Props {
  workAreaId: number;
  onZonesUpdated: () => void;
}

const MapLogic = ({
  workAreaId,
  drawnItemsRef,
  deletedZoneIds,
}: {
  workAreaId: number;
  drawnItemsRef: React.MutableRefObject<L.FeatureGroup>;
  deletedZoneIds: React.MutableRefObject<number[]>;
}) => {
  const map = useMap();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [zoneName, setZoneName] = useState("");
  const [isEditingNameOnly, setIsEditingNameOnly] = useState(false);
  const [editingZoneLayer, setEditingZoneLayer] = useState<L.Layer | null>(
    null
  );

  const isLoadingRef = useRef(false);
  map.doubleClickZoom.disable();

  useEffect(() => {
    const drawnItems = drawnItemsRef.current;

    if (!map.hasLayer(drawnItems)) {
      map.addLayer(drawnItems);
    }

    const drawControl = new L.Control.Draw({
      draw: {
        polygon: {
          allowIntersection: false,
          shapeOptions: { color: "#ff000099" },
        },
        polyline: false,
        rectangle: false,
        circle: false,
        marker: false,
        circlemarker: false,
      },
      edit: {
        featureGroup: drawnItems,
        remove: true,
      },
    });

    map.addControl(drawControl);

    map.on(L.Draw.Event.CREATED, (e: any) => {
      setEditingZoneLayer(e.layer);
      setZoneName(""); // сброс имени
      setIsModalVisible(true);
      setIsEditingNameOnly(false);
      //drawnItems.addLayer(e.layer);
    });

    map.on(L.Draw.Event.DELETED, (e: any) => {
      e.layers.eachLayer((layer: any) => {
        const zoneId = layer.options.repairZoneId;
        if (zoneId) deletedZoneIds.current.push(zoneId);
      });
    });

    return () => {
      map.removeControl(drawControl);
      if (map.hasLayer(drawnItems)) {
        map.removeLayer(drawnItems);
      }
    };
  }, []);

  useEffect(() => {
    if (!map) return;

    if (!map.hasLayer(drawnItemsRef.current)) {
      map.addLayer(drawnItemsRef.current);
    }
  }, [map]);

  useEffect(() => {
    if (!map) return;

    drawnItemsRef.current.clearLayers();
    deletedZoneIds.current = [];
    loadZones();
  }, [workAreaId]);

  const handleModalOk = () => {
    if (!editingZoneLayer) return;

    const layer = editingZoneLayer as LayerWithZoneId;
    layer.options.name = zoneName;
    layer.options.color = "#3388ff";

    if (!drawnItemsRef.current.hasLayer(layer)) {
      attachLayerEvents(layer);
      drawnItemsRef.current.addLayer(layer);
    }
    setIsModalVisible(false);
    setEditingZoneLayer(null);
    setZoneName("");
  };

  const handleLayerClick = (layer: LayerWithZoneId) => {
    const name = layer.options.name ?? "Без имени";

    // Создаём контейнер под React-компонент
    const popupContainer = document.createElement("div");
    popupContainer.style.minWidth = "200px";
    popupContainer.style.maxWidth = "400px";
    popupContainer.style.wordBreak = "break-word";
    popupContainer.style.whiteSpace = "normal";
    // Привязываем его к pop-up
    layer.bindPopup(popupContainer).openPopup();
    const root = createRoot(popupContainer);
    root.render(
      <div
        style={{
          minWidth: 150,
          maxWidth: 300,
          whiteSpace: "normal",
          wordBreak: "break-word",
        }}
      >
        <strong>{name}</strong>
        <Button
          style={{ marginLeft: 4 }}
          icon={<EditOutlined />}
          onClick={() => {
            setEditingZoneLayer(layer);
            setZoneName(layer.options.name ?? "");
            setIsEditingNameOnly(true);
            setIsModalVisible(true);
            layer.closePopup();
          }}
        ></Button>
      </div>
    );
  };

  const handleLayerDoubleClick = (layer: LayerWithZoneId) => {
    setEditingZoneLayer(layer);
    setZoneName(layer.options.name ?? "");
    setIsModalVisible(true);
    setIsEditingNameOnly(true);
  };

  const attachLayerEvents = (layer: LayerWithZoneId) => {
    layer.on("click", () => handleLayerClick(layer));
    //layer.on("dblclick", () => handleLayerDoubleClick(layer));
  };

  const loadZones = async () => {
    if (isLoadingRef.current) {
      console.log("Пропуск повторного вызова loadZones");
      return;
    }

    isLoadingRef.current = true;
    try {
      drawnItemsRef.current.clearLayers();
      deletedZoneIds.current = [];
      const res = await getRepairZones(workAreaId);
      res.data.forEach((zone) => {
        if (!zone.geometryJson) return;
        const geometry = JSON.parse(zone.geometryJson);
        const layer = L.geoJSON(geometry).getLayers()[0] as LayerWithZoneId;
        layer.options.repairZoneId = zone.id;
        layer.options.name = zone.name;
        layer.options.color = "#3388ff";
        (layer as unknown as L.Path).setStyle?.({ color: "#3388ff" });
        attachLayerEvents(layer);
        drawnItemsRef.current.addLayer(layer);
      });
    } catch {
      message.error("Ошибка при загрузке зон");
    } finally {
      isLoadingRef.current = false;
    }
  };

  return (
    <Modal
      title={
        isEditingNameOnly ? "Редактировать имя зоны" : "Назначить имя зоне"
      }
      open={isModalVisible}
      okText="Ок"
      cancelText="Отмена"
      onOk={handleModalOk}
      onCancel={() => {
        setIsModalVisible(false);
        setEditingZoneLayer(null);
        setZoneName("");
      }}
    >
      <Input
        placeholder="Введите имя зоны"
        value={zoneName}
        onChange={(e) => setZoneName(e.target.value)}
      />
    </Modal>
  );
};

const ZoneMap = ({ workAreaId, onZonesUpdated }: Props) => {
  const drawnItemsRef = useRef<L.FeatureGroup>(new L.FeatureGroup());
  const deletedZoneIds = useRef<number[]>([]);

  const handleSave = async () => {
    const layers = drawnItemsRef.current.getLayers();

    for (const layer of layers) {
      if (layer instanceof L.Polygon || layer instanceof L.Polyline) {
        const geoJson = layer.toGeoJSON();
        const repairZoneId = (layer as LayerWithZoneId).options.repairZoneId;

        const payload = {
          workAreaId,
          geometryJson: JSON.stringify(geoJson.geometry),
          name: (layer as LayerWithZoneId).options.name ?? "",
        };

        try {
          if (repairZoneId) {
            await updateRepairZone(repairZoneId, payload);
          } else {
            await addRepairZone(payload);
          }
        } catch {
          message.error(
            `Ошибка при сохранении зоны (ID: ${repairZoneId ?? "новая"})`
          );
        }
      }
    }

    for (const id of deletedZoneIds.current) {
      try {
        await deleteRepairZone(id);
      } catch {
        message.error(`Ошибка при удалении зоны ID ${id}`);
      }
    }

    deletedZoneIds.current = [];
    message.success("Зоны сохранены");
    onZonesUpdated?.();
  };

  return (
    <div>
      <MapContainer
        center={[54.434953, 25.949795]}
        zoom={12}
        style={{ height: "500px", width: "100%" }}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution="© OpenStreetMap"
        />
        <MapLogic
          workAreaId={workAreaId}
          drawnItemsRef={drawnItemsRef}
          deletedZoneIds={deletedZoneIds}
        />
      </MapContainer>
      <Button onClick={handleSave} type="primary" style={{ marginTop: 12 }}>
        Сохранить зоны
      </Button>
    </div>
  );
};

export default ZoneMap;
