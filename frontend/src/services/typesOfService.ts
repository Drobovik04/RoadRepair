import api from './axiosInstance';
import type { TypeOfService } from '../types/TypeOfService';

export const getTypesOfService = () => api.get<TypeOfService[]>(`/TypeOfService/`);
export const addTypeOfService = (data: { name: string; }) => api.post(`/TypeOfService/`, data);
export const updateTypeOfService = (id: number, data: { name: string; }) => api.put(`/TypeOfService/${id}`, data);
export const deleteTypeOfService = (id: number) => api.delete(`/TypeOfService/${id}`);
