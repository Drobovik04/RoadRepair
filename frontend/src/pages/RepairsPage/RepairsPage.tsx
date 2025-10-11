// src/pages/RepairsPage.tsx
import { useEffect, useRef, useState, useMemo, useCallback } from "react";
import {
  Button,
  Form,
  Input,
  List,
  Modal,
  Select,
  Space,
  Tabs,
  message,
} from "antd";
import { PlusOutlined } from "@ant-design/icons";
import ZoneWorksTab from "../../components/ZoneWorksTab";
import type { ZoneWorksTabRef } from "../../components/ZoneWorksTab";
import ZoneMap from "../../components/ZoneMap";
import "antd/dist/reset.css";
import type { WorkArea } from "../../types/WorkArea";
import {
  addWorkArea,
  updateWorkArea,
  deleteWorkArea,
  getWorkAreas,
} from "../../services/repairs";
import RepairForm from "./RepairForm";
import ContractorServicesPage from "../ContractorServicePage/ContractorServicePage";
import WorkTimeTable, {
  type WorkTimeTableRef,
} from "../../components/WorkTImeTable";
import MaterialsSummaryTab, {
  type MaterialsSummaryTabRef,
} from "../../components/MaterialsSummaryTab";
import { filterByQuery } from "../../utilities/textSearch";

const { TabPane } = Tabs;

const RepairsPage = () => {
  const [workAreas, setWorkAreas] = useState<WorkArea[]>([]);
  const [selectedWorkArea, setSelectedWorkArea] = useState<WorkArea | null>(
    null
  );
  const [formVisible, setFormVisible] = useState(false);
  const [editingWorkArea, setEditingWorkArea] = useState<WorkArea | null>(null);
  const [isDeleteVisible, setDeleteVisible] = useState(false);
  const [deletingWorkArea, setDeletingWorkArea] = useState<WorkArea | null>(
    null
  );
  const [sortBy, setSortBy] = useState<"createdAt" | "updatedAt">("createdAt");
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");
  const [search, setSearch] = useState("");

  const zoneWorksRef = useRef<ZoneWorksTabRef>(null);
  const materialSummaryRef = useRef<MaterialsSummaryTabRef>(null);
  const workTimeTableRef = useRef<WorkTimeTableRef>(null);

  const loadWorkAreas = async () => {
    getWorkAreas()
      .then((res) => {
        setWorkAreas(res.data);

        //костыль для обновления формы в случае открытия ее для редактирования после изменения сразу же
        if (selectedWorkArea) {
          const updated = res.data.find((x) => x.id === selectedWorkArea.id);
          if (updated) setSelectedWorkArea(updated);
        }
      })
      .catch(() => message.error("Не удалось загрузить ремонты"));
  };

  useEffect(() => {
    loadWorkAreas();
  }, []);

  const filteredWorkAreas = useMemo(
    () => filterByQuery(workAreas, search, [(w) => w.name]),
    [workAreas, search]
  );

  // Мемоизированная сортировка после фильтрации
  const sortedWorkAreas = useMemo(() => {
    return [...filteredWorkAreas].sort((a, b) => {
      const dateA = a[sortBy];
      const dateB = b[sortBy];

      // Обработка null значений - помещаем их в конец
      if (!dateA && !dateB) return 0;
      if (!dateA) return 1;
      if (!dateB) return -1;

      const comparison = new Date(dateA).getTime() - new Date(dateB).getTime();
      return sortOrder === "asc" ? comparison : -comparison;
    });
  }, [filteredWorkAreas, sortBy, sortOrder]);

  // Мемоизированная функция форматирования даты
  const formatDate = useCallback((date: Date | null) => {
    if (!date) return "Не указано";
    return new Date(date).toLocaleDateString("ru-RU", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      // hour: "2-digit",
      // minute: "2-digit",
    });
  }, []);

  const handleAdd = useCallback(() => {
    setEditingWorkArea(null);
    setFormVisible(true);
  }, []);

  const handleEdit = useCallback((record: WorkArea) => {
    setEditingWorkArea(record);
    setFormVisible(true);
  }, []);

  const handleSelectWorkArea = useCallback((workArea: WorkArea) => {
    setSelectedWorkArea(workArea);
  }, []);

  const handleDelete = async (id: number) => {
    try {
      await deleteWorkArea(id);
      message.success("Удалено");
      setDeleteVisible(false);
      setDeletingWorkArea(null);
      loadWorkAreas();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  // const createRepair = async (values: any) => {
  //   try {
  //     // заменить на POST-запрос
  //     const newRepair: Repair = {
  //       id: Date.now(),
  //       ...values,
  //     };
  //     setRepairs((prev) => [...prev, newRepair]);
  //     message.success("Ремонт создан");
  //     setModalVisible(false);
  //     form.resetFields();
  //   } catch {
  //     message.error("Ошибка при создании ремонта");
  //   }
  // };

  // const updateRepair = async (values: any) => {
  //   try {
  //     // заменить на PUT-запрос
  //     setRepairs((prev) =>
  //       prev.map((r) => (r.id === selectedRepair?.id ? { ...r, ...values } : r))
  //     );
  //     message.success("Ремонт обновлён");
  //     setModalVisible(false);
  //     form.resetFields();
  //   } catch {
  //     message.error("Ошибка при обновлении");
  //   }
  // };

  // useEffect(() => {
  //   loadWorkAreas();
  // }, []);

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      data.createdAt = data.createdAt?.format("YYYY-MM-DD");
      data.updatedAt = data.updatedAt?.format("YYYY-MM-DD");
      if (editingWorkArea) {
        await updateWorkArea(editingWorkArea.id, data);
        message.success("Ремонт обновлен");
      } else {
        await addWorkArea(data);
        message.success("Ремонт добавлен");
      }
      setFormVisible(false);
      loadWorkAreas();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>
          Добавить ремонт
        </Button>

        <Button
          disabled={!selectedWorkArea}
          onClick={() => selectedWorkArea && handleEdit(selectedWorkArea)}
        >
          Редактировать выбранный
        </Button>

        <Button
          danger
          disabled={!selectedWorkArea}
          onClick={() => {
            if (selectedWorkArea) {
              setDeletingWorkArea(selectedWorkArea);
              setDeleteVisible(true);
            }
          }}
        >
          Удалить выбранный
        </Button>

        <Space>
          <Input.Search
            allowClear
            placeholder="Поиск по названию..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            style={{ width: 300 }}
          />
          <span>Сортировка:</span>
          <Select
            value={sortBy}
            onChange={setSortBy}
            style={{ width: 200 }}
            options={[
              { value: "createdAt", label: "По дате создания" },
              { value: "updatedAt", label: "По дате обновления" },
            ]}
          />
          <Select
            value={sortOrder}
            onChange={setSortOrder}
            options={[
              { value: "desc", label: "По убыванию" },
              { value: "asc", label: "По возрастанию" },
            ]}
          />
        </Space>
      </Space>

      <List
        bordered
        dataSource={sortedWorkAreas}
        renderItem={(workArea) => (
          <List.Item
            onClick={() => handleSelectWorkArea(workArea)}
            style={{
              backgroundColor:
                selectedWorkArea?.id === workArea.id ? "#e6f7ff" : "white",
              cursor: "pointer",
              borderRadius: "8px",
            }}
          >
            <div style={{ width: "100%" }}>
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                }}
              >
                <div>
                  <strong>{workArea.name}</strong> — (ответственный:{" "}
                  {workArea.responsibleId
                    ? workArea.lastName +
                      " " +
                      workArea.firstName +
                      " " +
                      workArea.middleName
                    : "Не назначен"}
                  )
                </div>
                <div style={{ fontSize: "12px", color: "#666" }}>
                  {sortBy === "createdAt" ? "Создан" : "Обновлен"}:{" "}
                  {formatDate(workArea[sortBy])}
                </div>
              </div>
            </div>
          </List.Item>
        )}
      />

      <RepairForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingWorkArea || undefined}
      />

      <Modal
        open={isDeleteVisible}
        onCancel={() => {
          setDeleteVisible(false);
          setDeletingWorkArea(null);
        }}
        onOk={() => handleDelete(selectedWorkArea!.id)}
        okType="danger"
        okText="Удалить"
        cancelText="Отмена"
        title="Удалить ремонт?"
      >
        <p>
          Вы уверены, что хотите удалить{" "}
          <strong>{deletingWorkArea?.name}</strong>? Это действие необратимо.
        </p>
      </Modal>

      {selectedWorkArea && (
        <Tabs defaultActiveKey="1" style={{ marginTop: 24 }}>
          <TabPane tab="Инфо" key="1">
            <p>{selectedWorkArea.description}</p>
          </TabPane>
          <TabPane tab="Карта" key="2">
            <div className="tab">
              <ZoneMap
                workAreaId={selectedWorkArea.id}
                onZonesUpdated={() => {
                  zoneWorksRef.current?.reload();
                  materialSummaryRef.current?.reload();
                }}
              />
            </div>
          </TabPane>
          <TabPane tab="Зоны и виды работ" key="3">
            <ZoneWorksTab
              workAreaId={selectedWorkArea.id}
              ref={zoneWorksRef}
              onZoneWorksUpdated={() => {
                materialSummaryRef.current?.reload();
                workTimeTableRef.current?.reload();
              }}
            />
          </TabPane>
          <TabPane tab="Услуги" key="4">
            {/* <p>Здесь будут услуги от сторонних организаций</p> */}
            <ContractorServicesPage workAreaId={selectedWorkArea.id} />
          </TabPane>
          <TabPane tab="Материалы" key="5">
            <MaterialsSummaryTab
              workAreaId={selectedWorkArea.id}
              ref={materialSummaryRef}
            />
            {/* <p>Затраченные материалы</p> */}
          </TabPane>
          <TabPane tab="Рабочие" key="6">
            <WorkTimeTable
              workAreaId={selectedWorkArea.id}
              ref={workTimeTableRef}
            />
            {/* <p>Календарь трудозатрат, сотрудники</p> */}
          </TabPane>
        </Tabs>
      )}
    </div>
  );
};

export default RepairsPage;
