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
import type { TypeOfService } from "../../types/TypeOfService";
import {
  getTypesOfService,
  addTypeOfService,
  updateTypeOfService,
  deleteTypeOfService,
} from "../../services/typesOfService";
import TypeOfServiceForm from "./TypeOfServiceForm";
import { filterByQuery } from "../../utilities/textSearch";

const TypesOfServicePage = () => {
  const [typesOfService, setTypesOfService] = useState<TypeOfService[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingTypeOfService, setEditingTypeOfService] =
    useState<TypeOfService | null>(null);
  const [search, setSearch] = useState("");

  const loadTypesOfService = () => {
    getTypesOfService()
      .then((res) => setTypesOfService(res.data))
      .catch(() => message.error("Не удалось загрузить типы услуг"));
  };

  useEffect(() => {
    loadTypesOfService();
  }, []);

  const filtered = useMemo(
    () => filterByQuery(typesOfService, search, [(t) => t.name]),
    [typesOfService, search]
  );

  const handleAdd = () => {
    setEditingTypeOfService(null);
    setFormVisible(true);
  };

  const handleEdit = (record: TypeOfService) => {
    setEditingTypeOfService(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteTypeOfService(id);
      message.success("Удалено");
      loadTypesOfService();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingTypeOfService) {
        await updateTypeOfService(editingTypeOfService.id, data);
        message.success("Тип услуги обновлен");
      } else {
        await addTypeOfService(data);
        message.success("Тип услуги добавлен");
      }
      setFormVisible(false);
      loadTypesOfService();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
        <Button type="primary" onClick={handleAdd}>Добавить тип услуги</Button>
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
      <TypeOfServiceForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingTypeOfService || undefined}
      />
    </div>
  );
};

export default TypesOfServicePage;
