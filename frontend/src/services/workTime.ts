import api from './axiosInstance';
import type { WorkTime } from '../types/WorkTime';

export const getAllWorkTimesByRepairEventWorkerId = (repairEventWorkerId: number) => api.get<WorkTime[]>(`/WorkTime/repairEventWorker/${repairEventWorkerId}`);
export const addWorkTime = (data: { dayOfWork: Date; hours: number; repairEventWorkerId: number; }) => api.post('/WorkTime/', data);
export const updateWorkTime = (id: number, data: { repairEventId: number; workerId: number; }) => api.put(`/WorkTime/${id}`, data);
export const saveAllWorkTimes = (entries: any[]) => api.post(`/WorkTime/bulkWorkTime`, entries);
export const deleteWorkTimeByRepairEventWorkerId = (repairEventWorkerId: number) => api.delete(`/WorkTime/repairEventWorker/${repairEventWorkerId}`);
export const deleteWorkTime = (id: number) => api.delete(`/WorkTime/${id}`);
