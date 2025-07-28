import React, { useEffect, useState } from "react";
import {
  Table,
  InputNumber,
  Button,
  message,
  Typography,
  Space,
  Popconfirm,
  Modal,
  Select,
} from "antd";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import type { ColumnsType } from "antd/es/table";
import dayjs from "dayjs";
import isSameOrBefore from "dayjs/plugin/isSameOrBefore";
import type { Worker } from "../types/Worker";
import type { RepairEventWorker } from "../types/RepairEventWorker";
import type { WorkTime } from "../types/WorkTime";
import {
  getRepairEventWorkers,
  addRepairEventWorker,
  deleteRepairEventWorker,
} from "../services/repairEventWorker";
import {
  getAllWorkTimesByRepairEventWorkerId,
  addWorkTime,
  deleteWorkTime,
  deleteWorkTimeByRepairEventWorkerId,
  saveAllWorkTimes,
} from "../services/workTime";
import { getWorkers } from "../services/workers";

dayjs.extend(isSameOrBefore);

const { Title } = Typography;

// Пропсы
interface Props {
  repairEventId: number;
  startDate: string; // ISO
  endDate: string;
}

const WorkTimeTable: React.FC<Props> = ({
  repairEventId,
  startDate,
  endDate,
}) => {
  const [workersList, setWorkersList] = useState<RepairEventWorker[]>([]);
  const [allWorkers, setAllWorkers] = useState<Worker[]>([]);
  const [workTimes, setWorkTimes] = useState<WorkTime[]>([]);
  const [dateList, setDateList] = useState<string[]>([]);
  const [saving, setSaving] = useState(false);

  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [selectedWorkerId, setSelectedWorkerId] = useState<number | null>(null);

  // Генерация диапазона дат
  useEffect(() => {
    const dates: string[] = [];
    let current = dayjs(startDate);
    const end = dayjs(endDate);
    while (current.isSameOrBefore(end)) {
      dates.push(current.format("YYYY-MM-DD"));
      current = current.add(1, "day");
    }
    setDateList(dates);
  }, [startDate, endDate]);

  useEffect(() => {
    let isCancelled = false;

    async function load() {
      try {
        const [respWorkers, respAll] = await Promise.all([
          await getRepairEventWorkers(repairEventId),
          await getWorkers(),
        ]);

        const newWorkers = respWorkers.data;

        const allWorkTimesResults = await Promise.all(
          newWorkers.map((x) => getAllWorkTimesByRepairEventWorkerId(x.id))
        );

        const allWorkTimes = allWorkTimesResults.flatMap((r) => r.data);

        if (!isCancelled) {
          setWorkersList(newWorkers);
          setWorkTimes(allWorkTimes);
          setAllWorkers(respAll.data);
        }
      } catch {
        message.error("Ошибка загрузки данных");
      }
    }
    load();

    return () => {
      isCancelled = true;
    };
  }, [repairEventId]);

  // Обработка изменений
  const handleCellChange = (
    workerRow: RepairEventWorker,
    date: string,
    val: number | null
  ) => {
    setWorkTimes((prev) => {
      const idx = prev.findIndex(
        (w) =>
          w.repairEventWorkerId === workerRow.id &&
          dayjs(w.dayOfWork).isSame(date, "day")
      );
      const newVal = val ?? 0;
      if (idx >= 0) {
        if (newVal === 0) {
          const cp = [...prev];
          cp.splice(idx, 1);
          return cp;
        } else {
          const cp = [...prev];
          cp[idx] = { ...cp[idx], hours: newVal };
          return cp;
        }
      } else if (newVal > 0) {
        return [
          ...prev,
          {
            id: 0,
            dayOfWork: dayjs(date).toDate(),
            hours: newVal,
            repairEventWorkerId: workerRow.id,
          },
        ];
      }
      return prev;
    });
  };

  // Подготовка данных для таблицы
  const dataSource = workersList.map((row) => {
    const worker = allWorkers.find((x) => x.id === row.workerId);
    const fullName = worker
      ? [worker.lastName, worker.firstName, worker.middleName]
          .filter(Boolean)
          .join(" ")
      : "—";

    return {
      key: row.id,
      id: row.id,
      row,
      fullName: fullName || "—",
      ...Object.fromEntries(
        dateList.map((date) => {
          const wt = workTimes.find(
            (w) =>
              w.repairEventWorkerId === row.id &&
              dayjs(w.dayOfWork).isSame(date, "day")
          );
          return [date, wt?.hours ?? 0];
        })
      ),
    };
  });

  // Колонки
  const columns: ColumnsType<any> = [
    {
      title: "Сотрудник",
      dataIndex: "fullName",
      fixed: "left",
      width: 200,
      render: (_, rec) => (
        <Space style={{ display: "flex", justifyContent: "space-between" }}>
          {rec.fullName}
          <Popconfirm
            title="Удалить сотрудника из события?"
            onConfirm={async () => {
              await deleteWorkTimeByRepairEventWorkerId(rec.row.id);
              await deleteRepairEventWorker(rec.row.id);
              setWorkersList((prev) => prev.filter((r) => r.id !== rec.row.id));
              setWorkTimes((prev) =>
                prev.filter((w) => w.repairEventWorkerId !== rec.row.id)
              );
              message.success("Удален");
            }}
          >
            <Button danger icon={<DeleteOutlined />}></Button>
          </Popconfirm>
        </Space>
      ),
    },
    ...dateList.map((date) => ({
      title: dayjs(date, "YYYY-MM-DD").format("DD-MM-YYYY"),
      dataIndex: date,
      width: 100,
      render: (val: number, rec: any) => {
        return (
          <InputNumber
            min={0}
            max={24}
            value={val}
            onChange={(newVal) => {
              handleCellChange(rec.row, date, newVal ?? 0);
            }}
          />
        );
      },
    })),
  ];

  const handleSave = async () => {
    setSaving(true);
    try {
      const transformed = workTimes.map((wt) => ({
        ...wt,
        dayOfWork: dayjs(wt.dayOfWork).format("YYYY-MM-DD"),
      }));

      await saveAllWorkTimes(transformed);

      //await saveAllWorkTimes(workTimes);
      message.success("Сохранено");
    } catch {
      message.error("Ошибка сохранения");
    } finally {
      setSaving(false);
    }
  };

  const availableWorkers = allWorkers.filter(
    (w) => !workersList.some((r) => r.workerId === w.id)
  );

  const handleAddWorker = async () => {
    if (!selectedWorkerId) {
      message.warning("Выберите сотрудника");
      return;
    }

    try {
      const resp = await addRepairEventWorker({
        repairEventId,
        workerId: selectedWorkerId,
      });

      const newWorkersResp = await getRepairEventWorkers(repairEventId);
      const newWorkers = newWorkersResp.data;

      const allTimes = await Promise.all(
        newWorkers.map((w) =>
          getAllWorkTimesByRepairEventWorkerId(w.id).then((res) => res.data)
        )
      );

      setWorkTimes(allTimes.flat());
      setWorkersList(newWorkers);
      setIsAddModalVisible(false);
      setSelectedWorkerId(null);
      message.success("Сотрудник добавлен");
    } catch {
      message.error("Ошибка добавления сотрудника");
    }
  };

  return (
    <div>
      <Title level={3}>Табель учета рабочего времени</Title>
      <Space style={{ marginBottom: 16 }}>
        <Button type="primary" onClick={() => setIsAddModalVisible(true)}>
          Добавить участника
        </Button>
        <Button onClick={handleSave} loading={saving}>
          Сохранить часы
        </Button>
      </Space>

      <Table
        columns={columns}
        dataSource={dataSource}
        scroll={{ x: "max-content" }}
        pagination={false}
        bordered
      />
      <Modal
        title="Добавление сотрудника"
        open={isAddModalVisible}
        onOk={handleAddWorker}
        onCancel={() => {
          setIsAddModalVisible(false);
          setSelectedWorkerId(null);
        }}
        okText="Добавить"
        cancelText="Отмена"
      >
        <Select
          showSearch
          placeholder="Выберите сотрудника"
          style={{ width: "100%" }}
          value={selectedWorkerId ?? undefined}
          filterOption={(input, option) =>
            (option?.children?.toString() as string)
              .toLowerCase()
              .includes(input.toLowerCase())
          }
          onChange={(val) => setSelectedWorkerId(val)}
        >
          {availableWorkers.map((w) => (
            <Select.Option key={w.id} value={w.id}>
              {w.lastName + " " + w.firstName + " " + w.middleName}
            </Select.Option>
          ))}
        </Select>
      </Modal>
    </div>
  );
};

export default WorkTimeTable;
