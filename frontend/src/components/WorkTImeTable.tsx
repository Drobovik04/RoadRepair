import React, {
  forwardRef,
  useEffect,
  useImperativeHandle,
  useState,
} from "react";
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
import type { WorkAreaWorker } from "../types/WorkAreaWorker";
import type { WorkTime } from "../types/WorkTime";
import {
  getWorkAreaWorkers,
  addWorkAreaWorker,
  deleteWorkAreaWorker,
} from "../services/workAreaWorker";
import {
  getAllWorkTimesByWorkAreaWorkerId,
  addWorkTime,
  deleteWorkTime,
  deleteWorkTimeByWorkAreaWorkerId,
  saveAllWorkTimes,
} from "../services/workTime";
import { getWorkers } from "../services/workers";
import { getRepairEventsByWorkAreaId } from "../services/repairEvents";

dayjs.extend(isSameOrBefore);

const { Title } = Typography;

export interface WorkTimeTableRef {
  reload: () => void;
}

interface Props {
  workAreaId: number;
}

const WorkTimeTable = forwardRef<WorkTimeTableRef, Props>(
  ({ workAreaId }, ref) => {
    const [workersList, setWorkersList] = useState<WorkAreaWorker[]>([]);
    const [allWorkers, setAllWorkers] = useState<Worker[]>([]);
    const [workTimes, setWorkTimes] = useState<WorkTime[]>([]);
    const [dateList, setDateList] = useState<string[]>([]);
    const [saving, setSaving] = useState(false);

    const [isAddModalVisible, setIsAddModalVisible] = useState(false);
    const [selectedWorkerId, setSelectedWorkerId] = useState<number | null>(
      null
    );

    useImperativeHandle(ref, () => ({
      reload: async () => {
        try {
          const [respWorkers, respAll, respEvents] = await Promise.all([
            await getWorkAreaWorkers(workAreaId),
            await getWorkers(),
            await getRepairEventsByWorkAreaId(workAreaId),
          ]);

          const newWorkers = respWorkers.data;
          const events = respEvents.data;

          let startDate: string | null = null;
          let endDate: string | null = null;

          events.forEach((ev) => {
            const start = dayjs(ev.startedAt);
            const end = dayjs(ev.endedAt);
            if (!startDate || start.isBefore(startDate))
              startDate = start.format("YYYY-MM-DD");
            if (!endDate || end.isAfter(endDate))
              endDate = end.format("YYYY-MM-DD");
          });

          if (!startDate || !endDate) {
            setDateList([]);
            return;
          }

          const dates: string[] = [];
          let current = dayjs(startDate).clone();
          while (current.isSameOrBefore(endDate)) {
            dates.push(current.format("YYYY-MM-DD"));
            current = current.add(1, "day");
          }
          setDateList(dates);

          const allWorkTimesResults = await Promise.all(
            newWorkers.map((x) => getAllWorkTimesByWorkAreaWorkerId(x.id))
          );

          let allWorkTimes = allWorkTimesResults.flatMap((r) => r.data);

          const validDateSet = new Set(dates);
          const outOfRange = allWorkTimes.filter(
            (wt) => !validDateSet.has(dayjs(wt.dayOfWork).format("YYYY-MM-DD"))
          );

          if (outOfRange.length > 0) {
            // 4. Удаляем лишние часы из БД
            await Promise.all(outOfRange.map((wt) => deleteWorkTime(wt.id)));
            // 5. Удаляем их из локального состояния
            allWorkTimes = allWorkTimes.filter((wt) =>
              validDateSet.has(dayjs(wt.dayOfWork).format("YYYY-MM-DD"))
            );
          }
        } catch {
          message.error("Ошибка загрузки данных");
        }
      },
    }));

    // Генерация диапазона дат
    // useEffect(() => {
    //   const dates: string[] = [];
    //   let current = dayjs(startDate);
    //   const end = dayjs(endDate);
    //   while (current.isSameOrBefore(end)) {
    //     dates.push(current.format("YYYY-MM-DD"));
    //     current = current.add(1, "day");
    //   }
    //   setDateList(dates);
    // }, [startDate, endDate]);

    useEffect(() => {
      let isCancelled = false;

      async function load() {
        try {
          const [respWorkers, respAll, respEvents] = await Promise.all([
            await getWorkAreaWorkers(workAreaId),
            await getWorkers(),
            await getRepairEventsByWorkAreaId(workAreaId),
          ]);

          const newWorkers = respWorkers.data;
          const events = respEvents.data;

          let startDate: string | null = null;
          let endDate: string | null = null;

          events.forEach((ev) => {
            const start = dayjs(ev.startedAt);
            const end = dayjs(ev.endedAt);
            if (!startDate || start.isBefore(startDate))
              startDate = start.format("YYYY-MM-DD");
            if (!endDate || end.isAfter(endDate))
              endDate = end.format("YYYY-MM-DD");
          });

          if (!startDate || !endDate) {
            setDateList([]);
            return;
          }

          const dates: string[] = [];
          let current = dayjs(startDate).clone();
          while (current.isSameOrBefore(endDate)) {
            dates.push(current.format("YYYY-MM-DD"));
            current = current.add(1, "day");
          }
          setDateList(dates);

          const allWorkTimesResults = await Promise.all(
            newWorkers.map((x) => getAllWorkTimesByWorkAreaWorkerId(x.id))
          );

          let allWorkTimes = allWorkTimesResults.flatMap((r) => r.data);

          const validDateSet = new Set(dates);
          const outOfRange = allWorkTimes.filter(
            (wt) => !validDateSet.has(dayjs(wt.dayOfWork).format("YYYY-MM-DD"))
          );

          if (outOfRange.length > 0) {
            // 4. Удаляем лишние часы из БД
            await Promise.all(outOfRange.map((wt) => deleteWorkTime(wt.id)));
            // 5. Удаляем их из локального состояния
            allWorkTimes = allWorkTimes.filter((wt) =>
              validDateSet.has(dayjs(wt.dayOfWork).format("YYYY-MM-DD"))
            );
          }

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
    }, [workAreaId]);

    // Обработка изменений
    const handleCellChange = (
      workerRow: WorkAreaWorker,
      date: string,
      val: number | null
    ) => {
      setWorkTimes((prev) => {
        const idx = prev.findIndex(
          (w) =>
            w.workAreaWorkerId === workerRow.id &&
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
              workAreaWorkerId: workerRow.id,
            },
          ];
        }
        return prev;
      });
    };

    interface TableRow {
      key: number;
      id: number;
      row: WorkAreaWorker;
      fullName: string;
      totalHours: number;
      [date: string]: any; // <-- это даст доступ к row[date]
    }

    // Подготовка данных для таблицы
    const dataSource: TableRow[] = workersList.map((row) => {
      const worker = allWorkers.find((x) => x.id === row.workerId);
      const fullName = worker
        ? [worker.lastName, worker.firstName, worker.middleName]
            .filter(Boolean)
            .join(" ")
        : "—";

      const hoursByDate = dateList.map((date) => {
        const wt = workTimes.find(
          (w) =>
            w.workAreaWorkerId === row.id &&
            dayjs(w.dayOfWork).isSame(date, "day")
        );
        return wt?.hours ?? 0;
      });

      const totalHours = hoursByDate.reduce((sum, h) => sum + h, 0);

      return {
        key: row.id,
        id: row.id,
        row,
        fullName: fullName || "—",
        totalHours,
        ...Object.fromEntries(
          dateList.map((date, idx) => [date, hoursByDate[idx]])
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
              title="Удалить сотрудника из заявки на ремонт?"
              onConfirm={async () => {
                await deleteWorkTimeByWorkAreaWorkerId(rec.row.id);
                await deleteWorkAreaWorker(rec.row.id);
                setWorkersList((prev) =>
                  prev.filter((r) => r.id !== rec.row.id)
                );
                setWorkTimes((prev) =>
                  prev.filter((w) => w.workAreaWorkerId !== rec.row.id)
                );
                message.success("Удален");
              }}
            >
              <Button danger icon={<DeleteOutlined />}></Button>
            </Popconfirm>
          </Space>
        ),
      },
      {
        title: "Итого часов",
        dataIndex: "totalHours",
        width: 120,
        fixed: "left",
        render: (val) => <strong>{val}</strong>,
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
        const resp = await addWorkAreaWorker({
          workAreaId,
          workerId: selectedWorkerId,
        });

        const newWorkersResp = await getWorkAreaWorkers(workAreaId);
        const newWorkers = newWorkersResp.data;

        const allTimes = await Promise.all(
          newWorkers.map((w) =>
            getAllWorkTimesByWorkAreaWorkerId(w.id).then((res) => res.data)
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
          summary={() => {
            const totalAll = dataSource.reduce(
              (sum, row) => sum + row.totalHours,
              0
            );
            return (
              <Table.Summary.Row>
                <Table.Summary.Cell index={0}>
                  <strong>Итого по всем</strong>
                </Table.Summary.Cell>
                <Table.Summary.Cell index={1}>
                  <strong>{totalAll}</strong>
                </Table.Summary.Cell>
                {dateList.map((date, idx) => {
                  const colTotal = dataSource.reduce(
                    (sum, row) => sum + (row[date] || 0),
                    0
                  );
                  return (
                    <Table.Summary.Cell key={date} index={idx + 2}>
                      <strong>{colTotal}</strong>
                    </Table.Summary.Cell>
                  );
                })}
              </Table.Summary.Row>
            );
          }}
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
  }
);

export default WorkTimeTable;
