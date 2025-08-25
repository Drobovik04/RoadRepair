import {
  Button,
  Table,
  Space,
  Modal,
  Form,
  Input,
  message,
  Popconfirm,
} from "antd";
import { useEffect, useState } from "react";
import type { Worker } from "../../types/Worker";
import {
  getWorkers,
  addWorker,
  updateWorker,
  deleteWorker,
} from "../../services/workers";
import WorkerForm from "./WorkerForm";
import { useSelector } from "react-redux";
import type { RootState } from "../../store";
import dayjs from "dayjs";

const WorkersPage = () => {
  const [workers, setWorkers] = useState<Worker[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingWorker, setEditingWorker] = useState<Worker | null>(null);

  const loadWorkers = () => {
    getWorkers()
      .then((res) => {
        const processed = res.data.map((item) => {
          let temp = {
            ...item,
            hiredAt: new Date(item.hiredAt),
          };
          if (temp.firedAt != null) {
            temp = { ...temp, firedAt: new Date(temp.firedAt) };
          }

          return temp;
        });
        setWorkers(processed);
        //setWorkers(res.data);
      })
      .catch(() => message.error("Не удалось загрузить работников"));
  };

  useEffect(() => {
    loadWorkers();
  }, []);

  const handleAdd = () => {
    setEditingWorker(null);
    setFormVisible(true);
  };

  const handleEdit = (record: Worker) => {
    setEditingWorker(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteWorker(id);
      message.success("Удалено");
      loadWorkers();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      data.hiredAt = data.hiredAt?.format("YYYY-MM-DD");
      data.firedAt = data.firedAt?.format("YYYY-MM-DD");
      if (editingWorker) {
        await updateWorker(editingWorker.id, data);
        message.success("Сотрудник обновлен");
      } else {
        await addWorker(data);
        message.success("Сотрудник добавлен");
      }
      setFormVisible(false);
      loadWorkers();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <Button type="primary" onClick={handleAdd} style={{ marginBottom: 16 }}>
        Добавить сотрудника
      </Button>
      <Table
        rowKey="id"
        dataSource={workers}
        bordered
        columns={[
          {
            title: "Фамилия",
            dataIndex: "lastName",
            sorter: (a, b) => a.lastName.localeCompare(b.lastName),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Имя",
            dataIndex: "firstName",
            sorter: (a, b) => a.firstName.localeCompare(b.firstName),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Отчество",
            dataIndex: "middleName",
            sorter: (a, b) => {
              if (a.middleName == null || b.middleName == null) {
                return 1;
              }
              return a.middleName!.localeCompare(b.middleName!);
            },
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Нанят",
            dataIndex: "hiredAt",
            sorter: (a, b) => a.hiredAt.getTime() - b.hiredAt.getTime(),
            sortDirections: ["descend", "ascend"],
            render: (date: Date) => dayjs(date).format("YYYY-MM-DD"),
          },
          {
            title: "Уволен",
            dataIndex: "firedAt",
            sorter: (a, b) => {
              if (a.firedAt == null || b.firedAt == null) {
                return 1;
              }
              return a.firedAt!.getTime() - b.firedAt!.getTime();
            },
            sortDirections: ["descend", "ascend"],
            render: (date: Date) => dayjs(date).format("YYYY-MM-DD"),
          },
          {
            title: "Должность",
            dataIndex: "positionName",
            sorter: (a, b) => {
              if (a.positionName == null || b.positionName == null) {
                return 1;
              }
              return a.positionName!.localeCompare(b.positionName!);
            },
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Действия",
            render: (_, record) => (
              <div style={{ display: "flex", justifyContent: "center" }}>
                <Button onClick={() => handleEdit(record)}>Изменить</Button>
                <Popconfirm
                  title="Удалить?"
                  okText="ОК"
                  cancelText="Отмена"
                  onConfirm={() => handleDelete(record.id)}
                >
                  <Button danger style={{ marginLeft: 8 }}>
                    Удалить
                  </Button>
                </Popconfirm>
              </div>
            ),
          },
        ]}
      />
      <WorkerForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingWorker || undefined}
      />
    </div>
  );
};

export default WorkersPage;
