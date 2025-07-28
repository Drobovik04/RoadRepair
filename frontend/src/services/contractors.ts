import api from './axiosInstance';
import type { Contractor } from '../types/Contractor';

export const getContractors = () => api.get<Contractor[]>(`/Contractor/`);
export const addContractor = (data: { name: string; address: string; email: string | null; contactPhone: string | null, unp: number }) => api.post('/Contractor/', data);
export const updateContractor = (id: number, data: { name: string; address: string; email: string | null; contactPhone: string | null, unp: number }) => api.put(`/Contractor/${id}`, data);
export const deleteContractor = (id: number) => api.delete(`/Contractor/${id}`);
