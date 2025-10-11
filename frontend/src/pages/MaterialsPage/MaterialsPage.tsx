import {
  Button,
  Table,
  Space,
  Modal,
  Form,
  Input,
  message,
  Popconfirm,
  notification,
} from "antd";
import { useEffect, useMemo, useState } from "react";
import type { Material } from "../../types/Material";
import MaterialForm from "./MaterialForm";
import {
  getMaterials,
  addMaterial,
  updateMaterial,
  deleteMaterial,
} from "../../services/materials";
import { useSelector } from "react-redux";
import type { RootState } from "../../store";
import { filterByQuery } from "../../utilities/textSearch";

const MaterialsPage = () => {
  const [materials, setMaterials] = useState<Material[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingMaterial, setEditingMaterial] = useState<Material | null>(null);
  const [search, setSearch] = useState("");

  const loadMaterials = () => {
    getMaterials()
      .then((res) => setMaterials(res.data))
      .catch(() => message.error("Не удалось загрузить материалы"));
  };

  useEffect(() => {
    loadMaterials();
  }, []);

  const filtered = useMemo(
    () =>
      filterByQuery(materials, search, [
        (m) => m.name,
        (m) => m.typeOfMeasureShortName,
      ]),
    [materials, search]
  );

  const handleAdd = () => {
    setEditingMaterial(null);
    setFormVisible(true);
  };

  const handleEdit = (record: Material) => {
    setEditingMaterial(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteMaterial(id);
      message.success("Удалено");
      loadMaterials();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingMaterial) {
        await updateMaterial(editingMaterial.id, data);
        message.success("Материал обновлен");
      } else {
        await addMaterial(data);
        message.success("Материал добавлен");
      }
      setFormVisible(false);
      loadMaterials();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
        <Button type="primary" onClick={handleAdd}>Добавить материал</Button>
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
            sorter: (a, b) => a.name.localeCompare(b.name),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Ед. изм.",
            dataIndex: "typeOfMeasureShortName",
            sorter: (a, b) =>
              a.typeOfMeasureShortName.localeCompare(b.typeOfMeasureShortName),
            sortDirections: ["descend", "ascend"],
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
      <MaterialForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingMaterial || undefined}
      />
    </div>
  );
};

export default MaterialsPage;
