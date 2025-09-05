// src/app/models/create-employee-request.ts
export class CreateEmployeeRequest {
  employeeCode: string = '';
  lastName: string = '';
  firstName: string = '';
  middleName: string = '';
  age: number = 0;
  birthDate: string = '';
  civilStatus: string = '';
  gender: string = '';
  email: string = '';
  address: string = '';
  phoneNumber: string = '';
  rolePosition: string = '';
  createdBy: number = 0;
  createdAt: Date = new Date();
  isActive: boolean = true;
  archivedBy?: number; 
  archivedAt?: Date; 
  updatedBy?: number; 
  updatedAt?: Date; 
}