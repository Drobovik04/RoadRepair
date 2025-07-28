export interface ContractorService {
  id: number;
  typeOfServiceId: number;
  typeOfServiceName: string;
  contractorId: number;
  contractorName: string;
  price: number;
  description: string | null;
  dateOfService: Date;
  workAreaId: number;
}
