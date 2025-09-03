import api from './axiosInstance';
import type { WorkArea } from '../types/WorkArea';

export const getWorkAreas = () => api.get<WorkArea[]>(`/WorkArea/`);
export const addWorkArea = (data: { name: string; description: string | null; createdAt: Date | null, updatedAt: Date | null, responsibleId: number }) => api.post('/WorkArea/', data);
export const updateWorkArea = (id: number, data: { name: string; description: string | null; createdAt: Date | null, updatedAt: Date | null, responsibleId: number }) => api.put(`/WorkArea/${id}`, data);
export const deleteWorkArea = (id: number) => api.delete(`/WorkArea/${id}`);
