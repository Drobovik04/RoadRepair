export interface RepairEventMedia {
  id: number;
  repairEventId: number;
  filePath: string;
  file: File
  createdAt: Date;
  description: string | null;
  blobLink: string; // костыль
}
