import api from './axiosInstance';
import type { User } from '../types/User';

export const getUsers = () => api.get<User[]>(`/Auth/getAllUsers`);
export const toggleBlockUser = (id: number, data: { status: boolean; }) => api.put(`/Auth/toggleBlockUser/${id}`, data);
export const updateUser = (id: number, data: { userName: string; phoneNumber: string | null; lastName: string; middleName: string | null; firstName: string; email: string | null; roleId: number; createdAt: Date; }) => api.put(`/Auth/updateUser/${id}`, data);
export const deleteUser = (id: number) => api.delete(`/Auth/deleteUser/${id}`);
export const changeUserPassword = (id: number, data: { newPassword: string; }) => api.put(`/Auth/updatePassword/${id}`, data);
