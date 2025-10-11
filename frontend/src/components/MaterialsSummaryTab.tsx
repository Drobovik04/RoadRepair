import React, {
  forwardRef,
  useEffect,
  useImperativeHandle,
  useState,
  useMemo,
} from "react";
import { Table, Card, Divider, message, Typography } from "antd";
import { getRepairZones } from "../services/repairZones";
import { getRepairEventsByRepairZoneId } from "../services/repairEvents";
import { getMaterialSpends } from "../services/materialSpends";
import type { RepairZone } from "../types/RepairZone";
import type { RepairEvent } from "../types/RepairEvent";
import type { MaterialSpend } from "../types/MaterialSpend";

const { Title, Text } = Typography;

export interface MaterialsSummaryTabRef {
  reload: () => void;
}

interface Props {
  workAreaId: number;
}

interface TaskMaterial {
  id: string;
  name: string;
  contractor: string;
  volume: number;
  measure: string;
  shortMeasure: string;
  price: number;
  sum: number;
}

interface TaskView {
  key: string;
  taskName: string;
  materials: TaskMaterial[];
  totalPrice: number;
}

interface MaterialUse {
  id: string;
  task: string;
  contractor: string;
  volume: number;
  measure: string;
  shortMeasure: string;
  price: number;
  sum: number;
}

interface MaterialView {
  key: string;
  materialName: string;
  uses: MaterialUse[];
  totalPrice: number;
}

const MaterialsSummaryTab = forwardRef<MaterialsSummaryTabRef, Props>(
  ({ workAreaId }, ref) => {
    const [tasksView, setTasksView] = useState<TaskView[]>([]);
    const [materialsView, setMaterialsView] = useState<MaterialView[]>([]);
    const [loading, setLoading] = useState(false);

    const loadData = async () => {
      setLoading(true);
      try {
        const zones: RepairZone[] = (await getRepairZones(workAreaId)).data;

        const allTasks: TaskView[] = [];
        const allMaterialsMap: Record<string, MaterialView> = {};
        let globalCounter = 0;

        for (const zone of zones) {
          // Для каждой зоны получаем события (виды работ)
          const events: RepairEvent[] = (
            await getRepairEventsByRepairZoneId(zone.id)
          ).data;

          for (const event of events) {
            // Для каждого события получаем траты материалов
            const spends: MaterialSpend[] = (await getMaterialSpends(event.id))
              .data;

            const taskMaterials: TaskMaterial[] = spends.map((s, index) => ({
              id: `task-${++globalCounter}`,
              name: s.materialName,
              contractor: s.contractorName,
              volume: s.volume,
              measure: s.typeOfMeasureName,
              shortMeasure: s.typeOfMeasureShortName,
              price: s.price,
              sum: s.price * s.volume,
            }));

            const taskTotal = taskMaterials.reduce(
              (sum, s) => sum + s.price * s.volume,
              0
            );

            allTasks.push({
              key: `task-${event.id}`,
              taskName: `${zone.name} — ${event.typeOfRepairName}`,
              materials: taskMaterials,
              totalPrice: taskTotal,
            });

            // Группируем для "Материал → задачи"
            for (const s of spends) {
              const matKey = s.materialName;
              if (!allMaterialsMap[matKey]) {
                allMaterialsMap[matKey] = {
                  key: `mat-${matKey}`,
                  materialName: s.materialName,
                  uses: [],
                  totalPrice: 0,
                };
              }

              allMaterialsMap[matKey].uses.push({
                id: `use-${++globalCounter}`,
                task: `${zone.name} — ${event.typeOfRepairName}`,
                contractor: s.contractorName,
                volume: s.volume,
                measure: s.typeOfMeasureName,
                shortMeasure: s.typeOfMeasureShortName,
                price: s.price,
                sum: s.price * s.volume,
              });
              allMaterialsMap[matKey].totalPrice += s.price * s.volume;
            }
          }
        }

        setTasksView(allTasks);
        setMaterialsView(Object.values(allMaterialsMap));
      } catch {
        message.error("Ошибка загрузки итогов материалов");
      } finally {
        setLoading(false);
      }
    };

    useEffect(() => {
      loadData();
    }, [workAreaId]);

    useImperativeHandle(ref, () => ({
      reload: loadData,
    }));

    const columnsTaskMaterials = useMemo(
      () => [
        { title: "Материал", dataIndex: "name" },
        { title: "Поставщик", dataIndex: "contractor" },
        {
          title: "Кол-во",
          render: (_: unknown, r: TaskMaterial) =>
            `${r.volume} ${r.shortMeasure}`,
        },
        { title: "Цена за ед.", dataIndex: "price" },
        { title: "Сумма", dataIndex: "sum" },
      ],
      []
    );

    const columnsMaterialUses = useMemo(
      () => [
        { title: "Задача", dataIndex: "task" },
        { title: "Поставщик", dataIndex: "contractor" },
        {
          title: "Кол-во",
          render: (_: unknown, r: MaterialUse) =>
            `${r.volume} ${r.shortMeasure}`,
        },
        { title: "Цена за ед.", dataIndex: "price" },
        { title: "Сумма", dataIndex: "sum" },
      ],
      []
    );

    return (
      <div>
        <Card title="По задачам" style={{ marginBottom: 24 }}>
          {tasksView.map((task) => (
            <div key={task.key} style={{ marginBottom: 16 }}>
              <Title level={5}>{task.taskName}</Title>
              <Table
                className="customTable"
                size="small"
                loading={loading}
                columns={columnsTaskMaterials}
                dataSource={task.materials}
                pagination={false}
                rowKey={(r) => r.id}
              />
              <div style={{ textAlign: "right", fontWeight: "bold" }}>
                Итого по задаче: {task.totalPrice}
              </div>
              <Divider />
            </div>
          ))}
          <div style={{ textAlign: "right", fontSize: 16, fontWeight: "bold" }}>
            Общая сумма:{" "}
            {useMemo(
              () => tasksView.reduce((sum, t) => sum + t.totalPrice, 0),
              [tasksView]
            )}
          </div>
        </Card>
        <Divider />
        <Card title="По использованию материалов">
          {materialsView.map((mat) => (
            <div key={mat.key} style={{ marginBottom: 16 }}>
              <Title level={5}>{mat.materialName}</Title>
              <Table
                className="customTable"
                size="small"
                loading={loading}
                columns={columnsMaterialUses}
                dataSource={mat.uses}
                pagination={false}
                rowKey={(r) => r.id}
              />
              <div style={{ textAlign: "right", fontWeight: "bold" }}>
                Итого по материалу: {mat.totalPrice}
              </div>
              <Divider />
            </div>
          ))}
          <div style={{ textAlign: "right", fontSize: 16, fontWeight: "bold" }}>
            Общая сумма:{" "}
            {useMemo(
              () => materialsView.reduce((sum, m) => sum + m.totalPrice, 0),
              [materialsView]
            )}
          </div>
        </Card>
      </div>
    );
  }
);

export default React.memo(MaterialsSummaryTab);
