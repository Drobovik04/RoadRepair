import axios from 'axios';

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
    const original = err.config;
    if (err.response?.status === 401 && !original._retry) {
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
    return Promise.reject(err);
  }
);

export default api;