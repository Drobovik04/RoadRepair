import api from './axiosInstance';
import type { RepairZone } from '../types/RepairZone';

export const getRepairZones = (id: number) => api.get<RepairZone[]>(`/RepairZone/workArea/${id}`);
export const addRepairZone = (data: { workAreaId: number; geometryJson: string | null; name: string; }) => api.post('/RepairZone/', data);
export const updateRepairZone = (id: number, data: { workAreaId: number; geometryJson: string | null; name:string; }) => api.put(`/RepairZone/${id}`, data);
export const deleteRepairZone = (id: number) => api.delete(`/RepairZone/${id}`);
