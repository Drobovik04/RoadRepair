import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import type { TypeOfMeasure } from "../../types/TypeOfMeasure";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const TypeOfMeasureForm = ({
  open,
  onClose,
  onSubmit,
  initialValues,
}: Props) => {
  const [form] = Form.useForm();
  const [editingTypeOfMeasure, setEditingTypeOfMeasure] =
    useState<TypeOfMeasure | null>(null);

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingTypeOfMeasure(initialValues);
    } else {
      form.resetFields();
      setEditingTypeOfMeasure(null);
    }
  }, [initialValues]);

  const handleModalClose = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={
        initialValues
          ? "Редактировать единицу измерения"
          : "Добавить единицу измерения"
      }
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
          label="Наименовение единицы измерения"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default TypeOfMeasureForm;
