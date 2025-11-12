import React, { useEffect, useState } from "react";
import { Card, Form, Input, Button, Space, message } from "antd";
import {
  checkLoginAndGetInfoAboutUser,
  updateMyProfile,
} from "../../services/auth";
import { changeUserPassword } from "../../services/user";
import { useDispatch } from "react-redux";
import { setInfo } from "../../store/authSlice";

const ProfilePage: React.FC = () => {
  const [form] = Form.useForm();
  const [passwordForm] = Form.useForm();
  const [loading, setLoading] = useState<boolean>(false);
  const [passwordLoading, setPasswordLoading] = useState<boolean>(false);
  const [userId, setUserId] = useState<number | null>(null);
  const dispatch = useDispatch();

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      try {
        const res = await checkLoginAndGetInfoAboutUser();
        const data = res.data as any;
        setUserId(data.userId ?? null);
        form.setFieldsValue({
          userName: data.userName ?? "",
          email: data.email ?? "",
          phoneNumber: data.phoneNumber ?? "",
          lastName: data.lastName ?? "",
          middleName: data.middleName ?? "",
          firstName: data.firstName ?? "",
        });
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [form]);

  const onFinish = async (values: any) => {
    setLoading(true);
    try {
      await updateMyProfile(values);
      const refreshed = await checkLoginAndGetInfoAboutUser();
      dispatch(setInfo(JSON.stringify(refreshed.data)));
      message.success("Данные профиля обновлены");
      setUserId(refreshed.data.userId ?? null);
    } catch (error) {
      message.error("Не удалось обновить профиль");
    } finally {
      setLoading(false);
    }
  };

  const onPasswordFinish = async (values: any) => {
    setPasswordLoading(true);
    try {
      if (!userId) {
        message.error("Не удалось определить пользователя");
        return;
      }
      await changeUserPassword(userId, {
        newPassword: values.newPassword,
      });
      message.success("Пароль обновлен");
      passwordForm.resetFields();
    } catch (error) {
      message.error("Не удалось изменить пароль");
    } finally {
      setPasswordLoading(false);
    }
  };

  return (
    <Space direction="vertical" size="large" style={{ width: "100%" }}>
      <Card title="Профиль" loading={loading}>
        <Form form={form} layout="vertical" onFinish={onFinish}>
          <Space direction="vertical" style={{ width: "100%" }}>
            <Form.Item
              label="Логин"
              name="userName"
              rules={[{ required: true, message: "Укажите логин" }]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Email"
              name="email"
              rules={[
                { required: true, message: "Укажите адрес электронной почты" },
              ]}
            >
              <Input type="email" />
            </Form.Item>
            <Form.Item label="Телефон" name="phoneNumber">
              <Input />
            </Form.Item>
            <Form.Item
              label="Фамилия"
              name="lastName"
              rules={[{ required: true, message: "Укажите фамилию" }]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Имя"
              name="firstName"
              rules={[{ required: true, message: "Укажите имя" }]}
            >
              <Input />
            </Form.Item>
            <Form.Item label="Отчество" name="middleName">
              <Input />
            </Form.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit" loading={loading}>
                Сохранить
              </Button>
            </Form.Item>
          </Space>
        </Form>
      </Card>

      <Card title="Изменение пароля">
        <Form form={passwordForm} layout="vertical" onFinish={onPasswordFinish}>
          <Space direction="vertical" style={{ width: "100%" }}>
            <Form.Item
              label="Новый пароль"
              name="newPassword"
              rules={[{ required: true, message: "Введите новый пароль" }]}
            >
              <Input.Password />
            </Form.Item>
            <Form.Item
              label="Подтверждение пароля"
              name="confirmPassword"
              dependencies={["newPassword"]}
              rules={[
                { required: true, message: "Повторите новый пароль" },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue("newPassword") === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(new Error("Пароли не совпадают"));
                  },
                }),
              ]}
            >
              <Input.Password />
            </Form.Item>
            <Form.Item>
              <Button
                type="primary"
                htmlType="submit"
                loading={passwordLoading}
              >
                Изменить пароль
              </Button>
            </Form.Item>
          </Space>
        </Form>
      </Card>
    </Space>
  );
};

export default ProfilePage;
