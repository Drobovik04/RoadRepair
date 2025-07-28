import api from './axiosInstance';
import type { TypeOfRepair } from '../types/TypeOfRepair';

export const getTypesOfRepair = () => api.get<TypeOfRepair[]>(`/TypeOfRepair/`);
export const addTypeOfRepair = (data: { name: string; }) => api.post(`/TypeOfRepair/`, data);
export const updateTypeOfRepair = (id: number, data: { name: string; }) => api.put(`/TypeOfRepair/${id}`, data);
export const deleteTypeOfRepair = (id: number) => api.delete(`/TypeOfRepair/${id}`);
