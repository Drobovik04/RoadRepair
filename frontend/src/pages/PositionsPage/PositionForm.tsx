import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import type { Position } from "../../types/Position";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const PositionForm = ({ open, onClose, onSubmit, initialValues }: Props) => {
  const [form] = Form.useForm();
  const [editingPosition, setEditingPosition] = useState<Position | null>(null);

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingPosition(initialValues);
    } else {
      form.resetFields();
      setEditingPosition(null);
    }
  }, [initialValues]);

  const handleModalClose = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={initialValues ? "Редактировать должность" : "Добавить должность"}
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
          label="Наименовение должности"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default PositionForm;
