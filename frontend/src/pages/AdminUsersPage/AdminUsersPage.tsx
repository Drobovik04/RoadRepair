import React, { useEffect, useState } from "react";
import {
  Table,
  Button,
  Popconfirm,
  message,
  Space,
  Modal,
  Input,
  Select,
  Tag,
} from "antd";
import {
  DeleteOutlined,
  EditOutlined,
  LockOutlined,
  UnlockOutlined,
} from "@ant-design/icons";
import type { ColumnsType } from "antd/es/table";
import {
  getUsers,
  deleteUser,
  toggleBlockUser,
  updateUser,
  changeUserPassword,
} from "../../services/user";
import type { User } from "../../types/User";
import AdminUserForm from "./AdminUserForm";

const AdminUsersPage: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [passwordUser, setPasswordUser] = useState<User | null>(null);
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const loadUsers = () => {
    getUsers()
      .then((x) => setUsers(x.data))
      .catch(() => message.error("Ошибка загрузки пользователей"));
  };

  useEffect(() => {
    loadUsers();
  }, []);

  const handleEdit = (record: User) => {
    setEditingUser(record);
    setFormVisible(true);
  };

  const handleDelete = async (userId: number) => {
    try {
      await deleteUser(userId);
      message.success("Пользователь удалён");
      loadUsers();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleToggleBlock = async (user: User) => {
    try {
      await toggleBlockUser(user.id, { status: !user.isBlocked });
      loadUsers();
      message.success("Статус обновлён");
    } catch {
      message.error("Ошибка");
    }
  };

  const openPasswordModal = (user: User) => {
    setPasswordUser(user);
    setNewPassword("");
    setConfirmPassword("");
    setPasswordVisible(true);
  };

  const handlePasswordSave = async () => {
    if (!passwordUser) return;
    if (!newPassword.trim()) {
      message.warning("Введите новый пароль");
      return;
    }
    if (newPassword !== confirmPassword) {
      message.error("Пароли не совпадают");
      return;
    }
    try {
      await changeUserPassword(passwordUser.id, { newPassword: newPassword });
      message.success("Пароль обновлён");
      setPasswordVisible(false);
    } catch {
      message.error("Ошибка обновления пароля");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingUser) {
        await updateUser(editingUser.id, data);
        message.success("Пользователь обновлен");
      } else {
        //await addUser(data);
        //message.success("Пользователь добавлен");
      }
      setFormVisible(false);
      loadUsers();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  const columns: ColumnsType<User> = [
    {
      title: "Фамилия",
      dataIndex: "lastName",
      key: "lastName",
      sorter: (a, b) => a.lastName.localeCompare(b.lastName),
    },
    {
      title: "Имя",
      dataIndex: "firstName",
      key: "firstName",
      sorter: (a, b) => a.firstName.localeCompare(b.firstName),
    },
    {
      title: "Отчество",
      dataIndex: "middleName",
      key: "middleName",
      sorter: (a, b) => {
        if (a.middleName == null || b.middleName == null) {
          return 1;
        }
        return a.middleName.localeCompare(b.middleName);
      },
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
      sorter: (a, b) => a.email.localeCompare(b.email),
    },
    {
      title: "Роль",
      dataIndex: "roleName",
      key: "role",
      filters: [
        { text: "Админ", value: "admin" },
        { text: "Пользователь", value: "user" },
        { text: "Менеджер", value: "manager" },
      ],
      onFilter: (value, record) => record.roleName === value,
      render: (role) => (
        <Tag color={role === "admin" ? "volcano" : "blue"}>{role}</Tag>
      ),
    },
    {
      title: "Дата регистрации",
      dataIndex: "createdAt",
      key: "createdAt",
      sorter: (a, b) =>
        new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
      render: (date) =>
        new Date(date).toLocaleDateString() +
        " " +
        new Date(date).toLocaleTimeString(),
    },
    {
      title: "Статус",
      dataIndex: "isBlocked",
      key: "status",
      filters: [
        { text: "Активен", value: false },
        { text: "Заблокирован", value: true },
      ],
      onFilter: (value, record) => record.isBlocked === value,
      render: (blocked) =>
        blocked ? (
          <Tag color="red">Заблокирован</Tag>
        ) : (
          <Tag color="green">Активен</Tag>
        ),
    },
    {
      title: "Действия",
      key: "actions",
      render: (_, record) => (
        <Space>
          <Button
            icon={record.isBlocked ? <UnlockOutlined /> : <LockOutlined />}
            onClick={() => handleToggleBlock(record)}
          />
          <Button onClick={() => openPasswordModal(record)}>
            Сменить пароль
          </Button>
          <Button icon={<EditOutlined />} onClick={() => handleEdit(record)} />
          <Popconfirm
            title="Удалить пользователя?"
            okText="ОК"
            cancelText="Отмена"
            onConfirm={() => handleDelete(record.id)}
          >
            <Button icon={<DeleteOutlined />} danger />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <h2>Пользователи</h2>
      <Table rowKey="id" columns={columns} dataSource={users} bordered />
      <AdminUserForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingUser || undefined}
      />
      <Modal
        open={passwordVisible}
        title={
          passwordUser
            ? `Сменить пароль для: ${passwordUser.userName}`
            : "Сменить пароль"
        }
        onCancel={() => setPasswordVisible(false)}
        onOk={handlePasswordSave}
        okText="Сохранить"
        cancelText="Отмена"
        destroyOnClose
        okButtonProps={{
          disabled: !newPassword.trim() || newPassword !== confirmPassword,
        }}
      >
        <Input.Password
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          placeholder="Новый пароль"
        />
        <div style={{ height: 8 }} />
        <Input.Password
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          placeholder="Подтвердите пароль"
        />
      </Modal>
    </div>
  );
};

export default AdminUsersPage;
