export interface RepairEvent {
  id: number;
  repairZoneId: number;
  startedAt: Date;
  endedAt: Date | null;
  typeOfRepairId: number;
  typeOfRepairName: string;
}