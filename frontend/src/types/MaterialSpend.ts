export interface MaterialSpend {
  id: number;
  materialId: number;
  materialName: string;
  typeOfMeasureId: number;
  typeOfMeasureName: string;
  price: number;
  volume: number;
  repairEventId: number;
  contractorId: number;
  contractorName: string;
}
