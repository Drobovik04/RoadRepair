import api from './axiosInstance';
import type { Worker } from '../types/Worker';

export const getWorkers = () => api.get<Worker[]>(`/Worker/`);
export const addWorker = (data: { lastName: string; middleName: string | null; firstName: string, hiredAt: Date, positionId: number }) => api.post('/Worker/', data);
export const updateWorker = (id: number, data: { lastName: string; middleName: string | null; firstName: string, hiredAt: Date, positionId: number }) => api.put(`/Worker/${id}`, data);
export const deleteWorker = (id: number) => api.delete(`/Worker/${id}`);
