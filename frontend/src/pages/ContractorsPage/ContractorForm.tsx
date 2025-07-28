import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import type { Contractor } from "../../types/Contractor";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const ContractorForm = ({ open, onClose, onSubmit, initialValues }: Props) => {
  const [form] = Form.useForm();
  const [editingContractor, setEditingContractor] = useState<Contractor | null>(
    null
  );

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingContractor(initialValues);
    } else {
      form.resetFields();
      setEditingContractor(null);
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
        initialValues ? "Редактировать контрагента" : "Добавить контрагента"
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
          label="Наименовение контрагента"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>
        <Form.Item name="address" label="Адрес" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="email" label="Адрес электронной почты">
          <Input />
        </Form.Item>
        <Form.Item name="contactPhone" label="Контактный телефон">
          <Input />
        </Form.Item>
        <Form.Item name="unp" label="УНП" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default ContractorForm;
