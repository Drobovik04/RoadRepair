import api from './axiosInstance';
import type { WorkAreaWorker } from '../types/WorkAreaWorker';

export const getWorkAreaWorkers = (workAreaId: number) => api.get<WorkAreaWorker[]>(`/WorkAreaWorker/workArea/${workAreaId}`);
export const addWorkAreaWorker = (data: { workAreaId: number; workerId: number; }) => api.post('/WorkAreaWorker/', data);
export const updateWorkAreaWorker = (id: number, data: { workAreaId: number; workerId: number; }) => api.put(`/WorkAreaWorker/${id}`, data);
export const deleteWorkAreaWorker = (id: number) => api.delete(`/WorkAreaWorker/${id}`);
