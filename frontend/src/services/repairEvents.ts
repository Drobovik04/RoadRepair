import api from './axiosInstance';
import type { RepairEvent } from '../types/RepairEvent';

export const getRepairEvents = (id: number) => api.get<RepairEvent[]>(`/RepairEvent/workArea/${id}`);
export const addRepairEvent = (data: { repairZoneId: number; startedAt: Date; endedAt: Date | null; typeOfRepairId: number }) => api.post('/RepairEvent/', data);
export const updateRepairEvent = (id: number, data: { repairZoneId: number; startedAt: Date; endedAt: Date | null; typeOfRepairId: number }) => api.put(`/RepairEvent/${id}`, data);
export const deleteRepairEvent = (id: number) => api.delete(`/RepairEvent/${id}`);
