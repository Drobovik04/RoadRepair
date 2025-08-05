import { Form, Input, Modal, Select, Button, message } from "antd";
import { useEffect, useState } from "react";
import type { TypeOfRole } from "../../types/TypeOfRole";
import type { User } from "../../types/User";
import { getAllRoles } from "../../services/auth";
import type { Organization } from "../../types/Organization";
import { getOrganizations } from "../../services/organizations";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const AdminUserForm = ({ open, onClose, onSubmit, initialValues }: Props) => {
  const [form] = Form.useForm();
  const [roles, setRoles] = useState<TypeOfRole[]>([]);
  const [organizations, setOrganizations] = useState<Organization[]>([]);
  const [editingUser, setEditingUser] = useState<User | null>(null);

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue(initialValues);
      setEditingUser(initialValues);
    } else {
      form.resetFields();
      setEditingUser(null);
    }
  }, [initialValues]);

  useEffect(() => {
    getAllRoles().then((res) => setRoles(res.data));
    getOrganizations().then((res) => setOrganizations(res.data));
  }, []);

  const handleModalClose = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={
        initialValues ? "Редактировать пользователя" : "Добавить пользователя"
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
        <Form.Item name="userName" label="Логин" rules={[{ required: true }]}>
          <Input />
        </Form.Item>

        <Form.Item name="phoneNumber" label="Номер телефона">
          <Input />
        </Form.Item>

        <Form.Item name="lastName" label="Фамилия">
          <Input />
        </Form.Item>

        <Form.Item name="firstName" label="Имя">
          <Input />
        </Form.Item>

        <Form.Item name="middleName" label="Отчество">
          <Input />
        </Form.Item>

        <Form.Item name="email" label="Адрес электронной почты">
          <Input />
        </Form.Item>

        <Form.Item name="roleId" label="Роль" rules={[{ required: true }]}>
          <Select placeholder="Выберите роль">
            {roles.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          name="organizationId"
          label="Организация"
          rules={[{ required: true }]}
        >
          <Select placeholder="Выберите организацию">
            {organizations.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default AdminUserForm;
