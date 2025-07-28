import api from './axiosInstance';
import type { Position } from '../types/Position';

export const getPositions = () => api.get<Position[]>(`/Position/`);
export const addPosition = (data: { name: string; }) => api.post('/Position/', data);
export const updatePosition = (id: number, data: { name: string; }) => api.put(`/Position/${id}`, data);
export const deletePosition = (id: number) => api.delete(`/Position/${id}`);
