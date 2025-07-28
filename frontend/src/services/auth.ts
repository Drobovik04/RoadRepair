import api from './axiosInstance';

export interface RegisterDto {
  userName: string;
  email: string;
  lastName: string;
  firstName: string;
  middleName: string | null;
  password: string;
  organizationId: number;
}

export interface AuthDto {
    emailOrUserName: string;
    password: string;
}

export const login = (data: AuthDto) => api.post('/Auth/login', data);
export const register = (data: RegisterDto) => api.post('/Auth/register', data);
export const checkLoginAndGetInfoAboutUser = () => api.get('/Auth/getInfoAboutUser');
export const logout = () => api.post('/Auth/logout');