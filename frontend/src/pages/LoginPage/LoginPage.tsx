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
    } catch (err: any) {
      const isBlocked = err?.status == 403;
      if (isBlocked) {
        navigate("/blocked");
      } else {
        //message.error("Неверный логин или пароль");
      }
    }
  };

  return (
    <Form onFinish={onFinish} style={{ maxWidth: 300, margin: "50px auto" }}>
      <Form.Item name="emailOrUserName" messageVariables={{ label: "Логин или email" }} rules={[{ required: true }]}>
        <Input placeholder="Логин или email" />
      </Form.Item>
      <Form.Item name="password" messageVariables={{ label: "Пароль" }} rules={[{ required: true }]}>
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
