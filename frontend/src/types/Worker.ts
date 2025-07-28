export interface Worker {
  id: number;
  organizationId: number;
  lastName: string;
  middleName: string | null;
  firstName: string;
  hiredAt: Date;
  firedAt: Date | null;
  positionId: number | null;
  positionName: string | null;
}
