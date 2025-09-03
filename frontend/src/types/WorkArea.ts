export interface WorkArea {
  id: number;
  name: string;
  description: string | null;
  createdAt: Date | null;
  updatedAt: Date | null;
  responsibleId: number | null;
  lastName: string | null;
  firstName: string | null;
  middleName: string | null;
}