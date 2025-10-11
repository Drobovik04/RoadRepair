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
import type { TypeOfRepair } from "../../types/TypeOfRepair";
import {
  getTypesOfRepair,
  addTypeOfRepair,
  updateTypeOfRepair,
  deleteTypeOfRepair,
} from "../../services/typesOfRepair";
import TypeOfRepairForm from "./TypeOfRepairForm";
import { filterByQuery } from "../../utilities/textSearch";

const TypesOfRepairPage = () => {
  const [typesOfRepair, setTypesOfRepair] = useState<TypeOfRepair[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingTypeOfRepair, setEditingTypeOfRepair] =
    useState<TypeOfRepair | null>(null);
  const [search, setSearch] = useState("");

  const loadTypesOfRepair = () => {
    getTypesOfRepair()
      .then((res) => setTypesOfRepair(res.data))
      .catch(() => message.error("Не удалось загрузить типы рементов"));
  };

  useEffect(() => {
    loadTypesOfRepair();
  }, []);

  const filtered = useMemo(
    () => filterByQuery(typesOfRepair, search, [(t) => t.name]),
    [typesOfRepair, search]
  );

  const handleAdd = () => {
    setEditingTypeOfRepair(null);
    setFormVisible(true);
  };

  const handleEdit = (record: TypeOfRepair) => {
    setEditingTypeOfRepair(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteTypeOfRepair(id);
      message.success("Удалено");
      loadTypesOfRepair();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingTypeOfRepair) {
        await updateTypeOfRepair(editingTypeOfRepair.id, data);
        message.success("Тип ремонта обновлен");
      } else {
        await addTypeOfRepair(data);
        message.success("Тип ремонта добавлен");
      }
      setFormVisible(false);
      loadTypesOfRepair();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
        <Button type="primary" onClick={handleAdd}>Добавить тип ремонта</Button>
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
      <TypeOfRepairForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingTypeOfRepair || undefined}
      />
    </div>
  );
};

export default TypesOfRepairPage;
