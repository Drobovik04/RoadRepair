import api from './axiosInstance';
import type { RepairEventMedia } from '../types/RepairEventMedia';

export const getRepairEventMedia = (id: number) => api.get<RepairEventMedia[]>(`/RepairEventMedia/repairEvent/${id}`);
export const addRepairEventMedia = (data: { repairEventId: number; file: File; description: string | null }) => {
    const formData = new FormData();
    formData.append("RepairEventId", data.repairEventId.toString());
    formData.append("File", data.file);
    if (data.description) {
        formData.append("Description", data.description);
    }
    return api.post('/RepairEventMedia/', formData);
};
export const updateRepairEventMedia = (id: number, data: { repairEventId: number; file: File; description: string | null }) => {
    const formData = new FormData();
    formData.append("RepairEventId", data.repairEventId.toString());
    if (data.file)
        formData.append("File", data.file);
    else {
        formData.append("File", "");
    }
    if (data.description) {
        formData.append("Description", data.description);
    }
    return api.put(`/RepairEventMedia/${id}`, formData);
}
export const deleteRepairEventMedia = (id: number) => api.delete(`/RepairEventMedia/${id}`);
