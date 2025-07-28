// src/components/ZoneWorksTab.tsx
import React, {
  forwardRef,
  useEffect,
  useImperativeHandle,
  useState,
} from "react";
import { Card, List, message } from "antd";
import WorkTypeList from "./WorkTypeList";
import { getRepairZones } from "../services/repairZones";
import type { RepairZone } from "../types/RepairZone";

export interface ZoneWorksTabRef {
  reload: () => void;
}

interface Props {
  workAreaId: number;
}

const ZoneWorksTab = forwardRef<ZoneWorksTabRef, Props>(
  ({ workAreaId }, ref) => {
    const [zones, setZones] = useState<RepairZone[]>([]);

    const loadZones = async () => {
      try {
        const res = await getRepairZones(workAreaId);
        setZones(res.data);
      } catch {
        message.error("Ошибка загрузки зон");
      }
    };

    useImperativeHandle(ref, () => ({
      reload: loadZones,
    }));

    useEffect(() => {
      loadZones();
    }, [workAreaId]);

    return (
      <List
        dataSource={zones}
        rowKey="id"
        renderItem={(zone) => (
          <List.Item>
            <Card title={`Зона: ${zone.name}`} style={{ width: "100%" }}>
              <WorkTypeList zoneId={zone.id} />
            </Card>
          </List.Item>
        )}
      />
    );
  }
);

export default ZoneWorksTab;
