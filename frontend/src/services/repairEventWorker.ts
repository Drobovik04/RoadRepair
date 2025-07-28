import api from './axiosInstance';
import type { RepairEventWorker } from '../types/RepairEventWorker';

export const getRepairEventWorkers = (repairEventId: number) => api.get<RepairEventWorker[]>(`/RepairEventWorker/repairEvent/${repairEventId}`);
export const addRepairEventWorker = (data: { repairEventId: number; workerId: number; }) => api.post('/RepairEventWorker/', data);
export const updateRepairEventWorker = (id: number, data: { repairEventId: number; workerId: number; }) => api.put(`/RepairEventWorker/${id}`, data);
export const deleteRepairEventWorker = (id: number) => api.delete(`/RepairEventWorker/${id}`);
