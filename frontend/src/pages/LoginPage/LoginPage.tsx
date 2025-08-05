import { Form, Input, Button, message } from "antd";
import { login } from "../../services/auth";
import { useNavigate, Link } from "react-router-dom";

const LoginPage = () => {
  const navigate = useNavigate();

  const onFinish = async (values: any) => {
    try {
      const { data } = await login(values);
      localStorage.setItem("token", data.token);
      navigate("/");
    } catch {
      message.error("Ошибка авторизации");
      navigate("/blocked");
    }
  };

  return (
    <Form onFinish={onFinish} style={{ maxWidth: 300, margin: "50px auto" }}>
      <Form.Item name="emailOrUserName" rules={[{ required: true }]}>
        <Input placeholder="Логин" />
      </Form.Item>
      <Form.Item name="password" rules={[{ required: true }]}>
        <Input.Password placeholder="Пароль" />
      </Form.Item>
      <Button htmlType="submit" type="primary" block>
        Войти
      </Button>
      <div style={{ marginTop: 10 }}>
        Нет аккаунта? <Link to="/register">Регистрация</Link>
      </div>
    </Form>
  );
};

export default LoginPage;
