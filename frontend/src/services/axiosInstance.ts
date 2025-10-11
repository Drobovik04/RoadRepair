import axios from 'axios';
import { notification } from 'antd';

const api = axios.create({
  baseURL: 'http://localhost:56398/api',
  withCredentials: true,
});

let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

// Автообновление токена при инициализации, если кука с refresh существует
const refreshToken = async () => {
  try {
    const res = await api.post('/Auth/refresh');
    const newToken = res.data.token;
    localStorage.setItem('token', newToken);
    api.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
  } catch (err) {
    localStorage.removeItem('token');
    window.location.href = '/login';
  }
};

// if (!localStorage.getItem('token')) {
//   refreshToken();
// } else {
//   api.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
// }


api.interceptors.response.use(
  res => res,
  async err => {
    const original = err.config as any;
    
    // Обработка ошибок авторизации
    const isLoginRequest = typeof original?.url === 'string' && original.url.toLowerCase().includes('/auth/login');
    const skipAuthRefresh = original?.skipAuthRefresh === true;
    if (err.response?.status === 401 && !original._retry && !isLoginRequest && !skipAuthRefresh) {
      if (isRefreshing) {
        return new Promise(function (resolve, reject) {
          failedQueue.push({ resolve, reject });
        })
          .then(token => {
            original.headers['Authorization'] = 'Bearer ' + token;
            return api(original);
          })
          .catch(err => Promise.reject(err));
      }

      original._retry = true;
      isRefreshing = true;

      try {
        const res = await api.post('/auth/refresh');
        const newToken = res.data.token;
        localStorage.setItem('token', newToken);
        api.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
        processQueue(null, newToken);
        return api(original);
      } catch (refreshErr) {
        processQueue(refreshErr, null);
        localStorage.removeItem('token');
        window.location.href = '/login';
        return Promise.reject(refreshErr);
      } finally {
        isRefreshing = false;
      }
    }

    // Показываем уведомление об ошибке
    if (err.response?.data) {
      const errorData = err.response.data;
      let errorMessage = 'Произошла ошибка';
      
      // Извлекаем сообщение об ошибке из ответа сервера
      if (errorData.detail) {
        errorMessage = errorData.detail;
      } else if (errorData.title) {
        errorMessage = errorData.title;
      } else if (typeof errorData === 'string') {
        errorMessage = errorData;
      } else if (errorData.errors) {
        errorMessage = errorData.errors;
      }

      notification.error({
        message: 'Ошибка',
        description: errorMessage,
        placement: 'bottomRight',
      });
    } else if (err.request) {
      // Ошибка сети
      notification.error({
        message: 'Ошибка сети',
        description: 'Не удалось подключиться к серверу',
        placement: 'bottomRight',
      });
    } else {
      // Другие ошибки
      notification.error({
        message: 'Ошибка',
        description: 'Произошла непредвиденная ошибка',
        placement: 'bottomRight',
      });
    }

    return Promise.reject(err);
  }
);

export default api;