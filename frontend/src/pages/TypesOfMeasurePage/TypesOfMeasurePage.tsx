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
import {
  getTypesOfMeasure,
  addTypeOfMeasure,
  updateTypeOfMeasure,
  deleteTypeOfMeasure,
} from "../../services/typesOfMeasure";
import TypeOfMeasureForm from "./TypeOfMeasureForm";
import type { TypeOfMeasure } from "../../types/TypeOfMeasure";

const TypesOfMeasurePage = () => {
  const [typesOfMeasure, setTypesOfMeasre] = useState<TypeOfMeasure[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingTypeOfMeasure, setEditingTypeOfMeasure] =
    useState<TypeOfMeasure | null>(null);

  const loadTypesOfMeasure = () => {
    getTypesOfMeasure()
      .then((res) => setTypesOfMeasre(res.data))
      .catch(() => message.error("Не удалось загрузить единицы измерения"));
  };

  useEffect(() => {
    loadTypesOfMeasure();
  }, []);

  const handleAdd = () => {
    setEditingTypeOfMeasure(null);
    setFormVisible(true);
  };

  const handleEdit = (record: TypeOfMeasure) => {
    setEditingTypeOfMeasure(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteTypeOfMeasure(id);
      message.success("Удалено");
      loadTypesOfMeasure();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingTypeOfMeasure) {
        await updateTypeOfMeasure(editingTypeOfMeasure.id, data);
        message.success("Единица измерения обновлена");
      } else {
        await addTypeOfMeasure(data);
        message.success("Единица измерения добавлена");
      }
      setFormVisible(false);
      loadTypesOfMeasure();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <Button type="primary" onClick={handleAdd} style={{ marginBottom: 16 }}>
        Добавить единицу измерения
      </Button>
      <Table
        rowKey="id"
        dataSource={typesOfMeasure}
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
      <TypeOfMeasureForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingTypeOfMeasure || undefined}
      />
    </div>
  );
};

export default TypesOfMeasurePage;
