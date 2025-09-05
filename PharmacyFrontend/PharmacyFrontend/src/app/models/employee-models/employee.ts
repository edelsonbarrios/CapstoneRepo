export class Employee {
  id: number = 0;
  employeeCode: string = '';
  lastName: string = '';
  firstName: string = '';
  middleName: string = '';
  age: number = 0;
  birthDate: Date = new Date();
  civilStatus: string = '';
  gender: string = '';
  email: string = '';
  address: string = '';
  phoneNumber : string = '';
  rolePosition: string = '';
  fullName: string = '';
  createdBy: number = 0;
  createdAt: Date = new Date();
  isActive: boolean = true;
  archivedBy: number = 0;
  archivedAt: Date = new Date();
  updatedBy: number = 0;
  updatedAt: Date = new Date();
}