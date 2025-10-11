import '@ant-design/v5-patch-for-react-19';
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { Provider } from "react-redux";
import { store } from "./store/index.ts";
import { ConfigProvider } from "antd";
import ruRU from 'antd/locale/ru_RU';
import dayjs from 'dayjs';
import 'dayjs/locale/ru';

dayjs.locale('ru');

const validateMessages = {
  required: "'${label}' обязательно",
  types: {
    email: "'${label}' должен быть корректным email",
    number: "'${label}' должен быть числом",
    url: "'${label}' должен быть корректным URL",
  },
  number: {
    min: "'${label}' не может быть меньше ${min}",
    max: "'${label}' не может быть больше ${max}",
    range: "'${label}' должен быть между ${min} и ${max}",
  },
  string: {
    min: "'${label}' должен содержать минимум ${min} символов",
    max: "'${label}' должен содержать максимум ${max} символов",
    range: "'${label}' должен содержать от ${min} до ${max} символов",
  },
};

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Provider store={store}>
        <ConfigProvider locale={ruRU} form={{ validateMessages }}>
          <App />
        </ConfigProvider>
    </Provider>
  </StrictMode>
);
