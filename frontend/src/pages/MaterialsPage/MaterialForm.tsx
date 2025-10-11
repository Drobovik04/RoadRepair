import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import {
  addTypeOfMeasure,
  getTypesOfMeasure,
  getTypeOfMeasure,
} from "../../services/typesOfMeasure";
import type { TypeOfMeasure } from "../../types/TypeOfMeasure";
import type { Material } from "../../types/Material";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const MaterialForm = ({ open, onClose, onSubmit, initialValues }: Props) => {
  const [form] = Form.useForm();
  const [types, setTypes] = useState<TypeOfMeasure[]>([]);
  const [addingTypeOfMeasure, setAddingTypeOfMeasure] = useState(false);
  const [editingMaterial, setEditingMaterial] = useState<Material | null>(null);
  const [newTypeOfMeasureName, setNewTypeOfMeasureName] = useState("");
  const [newTypeOfMeasureShortName, setNewTypeOfMeasureShortName] =
    useState("");

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingMaterial(initialValues);
    } else {
      form.resetFields();
      setEditingMaterial(null);
    }
  }, [initialValues]);

  useEffect(() => {
    getTypesOfMeasure().then((res) => setTypes(res.data));
  }, []);

  const handleAddTypeOfMeasure = async () => {
    try {
      const res = await addTypeOfMeasure({
        name: newTypeOfMeasureName,
        shortName: newTypeOfMeasureShortName,
      });
      const newType = await getTypeOfMeasure(res.data.id);
      setTypes([...types, { ...newType.data, id: res.data.id }]);
      setNewTypeOfMeasureName("");
      setNewTypeOfMeasureShortName("");
      setAddingTypeOfMeasure(false);
      message.success("Добавлена новая единица измерения");
      // Устанавливаем новую единицу измерения в форме
      form.setFieldsValue({
        typeOfMeasureId: res.data.id,
      });
    } catch {
      message.error("Ошибка при добавлении новой единицы измерения");
    }
  };

  const handleModalClose = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={initialValues ? "Редактировать материал" : "Добавить материал"}
      onCancel={handleModalClose}
      onOk={() => form.submit()}
      okText={initialValues ? "Сохранить" : "Добавить"}
      cancelText="Отмена"
      destroyOnHidden
    >
      <Form
        form={form}
        initialValues={initialValues}
        onFinish={onSubmit}
        layout="vertical"
        preserve={false}
      >
        <Form.Item
          name="name"
          label="Название материала"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          name="typeOfMeasureId"
          label="Единица измерения"
          rules={[{ required: true }]}
        >
          <Select
            placeholder="Выберите или добавьте"
            showSearch
            optionFilterProp="children"
            filterOption={(input, option) =>
              String(option?.children)
                .toLowerCase()
                .includes(input.toLowerCase())
            }
            dropdownRender={(menu) => (
              <>
                {menu}
                <div style={{ display: "flex", padding: 8 }}>
                  <Input
                    value={newTypeOfMeasureName}
                    onChange={(e) => setNewTypeOfMeasureName(e.target.value)}
                    placeholder="Новая единица"
                    style={{ marginRight: 8 }}
                  />
                  <Input
                    value={newTypeOfMeasureShortName}
                    onChange={(e) =>
                      setNewTypeOfMeasureShortName(e.target.value)
                    }
                    placeholder="Сокращение новой единицы"
                    style={{ marginRight: 8 }}
                  />
                  <Button onClick={handleAddTypeOfMeasure} type="link">
                    Добавить
                  </Button>
                </div>
              </>
            )}
          >
            {types.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.name} - {u.shortName}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default MaterialForm;
