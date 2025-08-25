export interface User {
  id: number;
  userName: string;
  phoneNumber: string | null;
  lastName: string;
  middleName: string | null;
  firstName: string;
  email: string;
  isBlocked: boolean;
  roleId: number;
  roleName: string | null;
  createdAt: string;
}