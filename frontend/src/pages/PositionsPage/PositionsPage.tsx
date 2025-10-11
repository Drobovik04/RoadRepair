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
import { useEffect, useMemo, useState } from "react";
import type { Position } from "../../types/Position";
import {
  getPositions,
  addPosition,
  updatePosition,
  deletePosition,
} from "../../services/positions";
import PositionForm from "./PositionForm";
import { filterByQuery } from "../../utilities/textSearch";

const PositionsPage = () => {
  const [positions, setPositions] = useState<Position[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingPosition, setEditingPosition] = useState<Position | null>(null);
  const [search, setSearch] = useState("");

  const loadPositions = () => {
    getPositions()
      .then((res) => setPositions(res.data))
      .catch(() => message.error("Не удалось загрузить должности"));
  };

  useEffect(() => {
    loadPositions();
  }, []);

  const filtered = useMemo(
    () => filterByQuery(positions, search, [(p) => p.name]),
    [positions, search]
  );

  const handleAdd = () => {
    setEditingPosition(null);
    setFormVisible(true);
  };

  const handleEdit = (record: Position) => {
    setEditingPosition(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deletePosition(id);
      message.success("Удалено");
      loadPositions();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingPosition) {
        await updatePosition(editingPosition.id, data);
        message.success("Должность обновлена");
      } else {
        await addPosition(data);
        message.success("Должность добавлена");
      }
      setFormVisible(false);
      loadPositions();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
        <Button type="primary" onClick={handleAdd}>Добавить должность</Button>
        <Input.Search
          allowClear
          placeholder="Поиск..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          style={{ maxWidth: 320 }}
        />
      </div>
      <Table
        rowKey="id"
        dataSource={filtered}
        bordered
        columns={[
          {
            title: "Название",
            dataIndex: "name",
            showSorterTooltip: { target: "full-header" },
            sorter: (a, b) => a.name.localeCompare(b.name),
          },
          {
            title: "Действия",
            width: "20%",
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
      <PositionForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingPosition || undefined}
      />
    </div>
  );
};

export default PositionsPage;
