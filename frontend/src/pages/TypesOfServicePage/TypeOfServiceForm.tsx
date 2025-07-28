import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import type { TypeOfService } from "../../types/TypeOfService";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const TypeOfServiceForm = ({
  open,
  onClose,
  onSubmit,
  initialValues,
}: Props) => {
  const [form] = Form.useForm();
  const [editingTypeOfService, setEditingTypeOfService] =
    useState<TypeOfService | null>(null);

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingTypeOfService(initialValues);
    } else {
      form.resetFields();
      setEditingTypeOfService(null);
    }
  }, [initialValues]);

  const handleModalClose = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={initialValues ? "Редактировать тип услуги" : "Добавить тип услуги"}
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
          label="Наименовение типа услуги"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default TypeOfServiceForm;
