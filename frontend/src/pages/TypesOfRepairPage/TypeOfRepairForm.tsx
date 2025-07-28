import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import type { TypeOfRepair } from "../../types/TypeOfRepair";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const TypeOfRepairForm = ({
  open,
  onClose,
  onSubmit,
  initialValues,
}: Props) => {
  const [form] = Form.useForm();
  const [editingTypeOfRepair, setEditingTypeOfRepair] =
    useState<TypeOfRepair | null>(null);

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingTypeOfRepair(initialValues);
    } else {
      form.resetFields();
      setEditingTypeOfRepair(null);
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
        initialValues ? "Редактировать тип ремонта" : "Добавить тип ремонта"
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
          label="Наименовение типа ремонта"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default TypeOfRepairForm;
