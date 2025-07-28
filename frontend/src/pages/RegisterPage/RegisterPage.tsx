import { Form, Input, Button, message, Select } from "antd";
import { register } from "../../services/auth";
import { Link, useNavigate } from "react-router-dom";
import type { Organization } from "../../types/Organization";
import { useState, useEffect } from "react";
import { getOrganizations } from "../../services/organizations";

const RegisterPage = () => {
  const [orgs, setOrgs] = useState<Organization[]>([]);

  const navigate = useNavigate();

  useEffect(() => {
    getOrganizations().then((res) => setOrgs(res.data));
  }, []);
  const onFinish = async (values: any) => {
    try {
      await register(values);
      message.success("Регистрация успешна, войдите в систему");
      navigate("/login");
    } catch {
      message.error("Ошибка регистрации");
    }
  };

  return (
    <Form onFinish={onFinish} style={{ maxWidth: 300, margin: "50px auto" }}>
      <Form.Item name="userName" rules={[{ required: true }]}>
        <Input placeholder="Логин" />
      </Form.Item>
      <Form.Item name="email" rules={[{ required: true }]}>
        <Input type="email" placeholder="Адрес электронной почты" />
      </Form.Item>
      <Form.Item name="lastName" rules={[{ required: true }]}>
        <Input placeholder="Фамилия" />
      </Form.Item>
      <Form.Item name="firstName" rules={[{ required: true }]}>
        <Input placeholder="Имя" />
      </Form.Item>
      <Form.Item name="middleName" rules={[{ required: false }]}>
        <Input placeholder="Отчество" />
      </Form.Item>
      <Form.Item name="password" rules={[{ required: true }]}>
        <Input.Password placeholder="Пароль" />
      </Form.Item>
      <Form.Item
        name="organizationId"
        label="Организация"
        rules={[{ required: true }]}
      >
        <Select
          placeholder="Выберите организацию"
          dropdownRender={(menu) => <>{menu}</>}
        >
          {orgs.map((u) => (
            <Select.Option key={u.id} value={u.id}>
              {u.name}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Button htmlType="submit" type="primary" block>
        Зарегистрироваться
      </Button>
      <div style={{ marginTop: 10 }}>
        Уже есть аккаунт? <Link to="/login">Вход</Link>
      </div>
    </Form>
  );
};

export default RegisterPage;
