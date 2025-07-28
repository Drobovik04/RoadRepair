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

interface Props {
  organizationId: number;
}

const MaterialsPage = () => {
  const [materials, setMaterials] = useState<Material[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingMaterial, setEditingMaterial] = useState<Material | null>(null);
  const organizationId = useSelector(
    (state: RootState) => state.auth.organizationId
  );

  const loadMaterials = () => {
    if (!organizationId) return;
    getMaterials(organizationId!)
      .then((res) => setMaterials(res.data))
      .catch(() => message.error("Не удалось загрузить материалы"));
  };

  useEffect(() => {
    loadMaterials();
  }, [organizationId]);

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
      const data = { ...values, organizationId };
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
      <Button type="primary" onClick={handleAdd} style={{ marginBottom: 16 }}>
        Добавить материал
      </Button>
      <Table
        rowKey="id"
        dataSource={materials}
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
            dataIndex: "typeOfMeasureName",
            sorter: (a, b) =>
              a.typeOfMeasureName.localeCompare(b.typeOfMeasureName),
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
        organizationId={organizationId!}
      />
    </div>
  );
};

export default MaterialsPage;
