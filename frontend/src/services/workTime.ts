import api from './axiosInstance';
import type { WorkTime } from '../types/WorkTime';

export const getAllWorkTimesByWorkAreaWorkerId = (workAreaWorkerId: number) => api.get<WorkTime[]>(`/WorkTime/workAreaWorker/${workAreaWorkerId}`);
export const addWorkTime = (data: { dayOfWork: Date; hours: number; workAreaWorkerId: number; }) => api.post('/WorkTime/', data);
export const updateWorkTime = (id: number, data: { workAreaId: number; workerId: number; }) => api.put(`/WorkTime/${id}`, data);
export const saveAllWorkTimes = (entries: any[]) => api.post(`/WorkTime/bulkWorkTime`, entries);
export const deleteWorkTimeByWorkAreaWorkerId = (workAreaWorkerId: number) => api.delete(`/WorkTime/workAreaWorker/${workAreaWorkerId}`);
export const deleteWorkTime = (id: number) => api.delete(`/WorkTime/${id}`);
