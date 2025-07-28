import api from './axiosInstance';
import type { MaterialSpend } from '../types/MaterialSpend';

export const getMaterialSpends = (repairEventId: number) => api.get<MaterialSpend[]>(`/MaterialSpend/repairEvent/${repairEventId}`);
export const addMaterialSpend = (data: { materialId: number; price: number; volume: number; repairEventId: number; contractorId: number }) => api.post('/MaterialSpend/', data);
export const updateMaterialSpend = (id: number, data: { materialId: number; price: number; volume: number; repairEventId: number; contractorId: number }) => api.put(`/MaterialSpend/${id}`, data);
export const deleteMaterialSpend = (id: number) => api.delete(`/MaterialSpend/${id}`);
