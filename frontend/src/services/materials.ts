import api from './axiosInstance';
import type { Material } from '../types/Material';

export const getMaterials = () => api.get<Material[]>(`/Material`);
export const addMaterial = (data: { name: string; typeOfMeasureId: number; }) => api.post('/Material/', data);
export const updateMaterial = (id: number, data: { name: string; typeOfMeasureId: number; }) => api.put(`/Material/${id}`, data);
export const deleteMaterial = (id: number) => api.delete(`/Material/${id}`);
