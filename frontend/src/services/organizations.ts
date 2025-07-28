import api from './axiosInstance';
import type { Organization } from '../types/Organization';

export const getOrganizations = () => api.get<Organization[]>(`/Organization`);