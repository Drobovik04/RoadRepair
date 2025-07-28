import api from './axiosInstance';
import type { ContractorService } from '../types/ContractorService';

export const getContractorServices = (id: number) => api.get<ContractorService[]>(`/ContractorService/workArea/${id}`);
export const addContractorService = (data: { typeOfServiceId: number; contractorId: number; price: number; description: string | null, dateOfService: Date, workAreaId: number }) => api.post('/ContractorService/', data);
export const updateContractorService = (id: number, data: { typeOfServiceId: number; contractorId: number; price: number; description: string | null, dateOfService: Date, workAreaId: number }) => api.put(`/ContractorService/${id}`, data);
export const deleteContractorService = (id: number) => api.delete(`/ContractorService/${id}`);
