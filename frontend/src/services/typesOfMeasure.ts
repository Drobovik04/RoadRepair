import api from './axiosInstance';
import type { TypeOfMeasure } from '../types/TypeOfMeasure';

export const getTypesOfMeasure = () => api.get<TypeOfMeasure[]>('/TypeOfMeasure');
export const getTypeOfMeasure = (typeId: number) => api.get<TypeOfMeasure>(`/TypeOfMeasure/${typeId}`);
export const addTypeOfMeasure = (data: { name: string, shortName: string }) => api.post('/TypeOfMeasure', data);
export const updateTypeOfMeasure = (id: number, data: { name: string, shortName: string }) => api.put(`/TypeOfMeasure/${id}`, data);
export const deleteTypeOfMeasure = (id: number) => api.delete(`/TypeOfMeasure/${id}`);
