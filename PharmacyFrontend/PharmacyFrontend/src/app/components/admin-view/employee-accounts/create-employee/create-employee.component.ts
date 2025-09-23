import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Employee } from '../../../../models/employee-models/employee';
import { PostService } from '../../../../services/PostService/post.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '../../../../../../node_modules/@angular/common';
import { Account } from '../../../../models/account';
import { CreateAccountComponent } from '../create-account/create-account.component';
import { NgFor } from "../../../../../../node_modules/@angular/common";
import { LogInToken } from '../../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { GetService } from '../../../../services/GetService/get.service';
import { CreateEmployeeRequest } from '../../../../models/employee-models/createEmployeeRequest';

@Component({
  selector: 'app-create-employee',
  standalone: true,
  imports: [FormsModule, NgIf, CreateAccountComponent, NgFor],
  templateUrl: './create-employee.component.html',
  styleUrl: './create-employee.component.scss',
})
export class CreateEmployeeComponent implements OnInit{
  @Input() newEmployee: Employee | null = new Employee();
  @Output() closeForm = new EventEmitter<number>();
  newAccount?: Account | null;
  newEmployeeId: number = 0;
  genders : string[] = ['Male', 'Female'];
  rolePositions : string[] = ['Administrator', 'Inventory Manager', 'Cashier'];
  loggedInEmployeeId : number = 0;
  isUserLoading: boolean = true;

  // Error properties
  employeeCodeError: string = '';
  lastNameError: string = '';
  firstNameError: string = '';
  middleNameError: string = '';
  genderError: string = '';
  ageError: string = '';
  birthDateError: string = '';
  phoneNumberError: string = '';
  civilStatusError: string = '';
  rolePositionError: string = '';
  emailError: string = '';
  addressError: string = '';

  constructor(
    private postService: PostService,
    private getService: GetService
  ) {}

  ngOnInit(): void {
      this.getLoggedInEmployee();
  }

  createNewEmployee() {
    if (!this.newEmployee) return;

    // Clear previous errors
    this.employeeCodeError = '';
    this.lastNameError = '';
    this.firstNameError = '';
    this.middleNameError = '';
    this.genderError = '';
    this.ageError = '';
    this.birthDateError = '';
    this.phoneNumberError = '';
    this.civilStatusError = '';
    this.rolePositionError = '';
    this.emailError = '';
    this.addressError = '';

    let hasErrors = false;

    // Validate required fields
    if (!this.newEmployee.employeeCode || this.newEmployee.employeeCode.trim() === '') {
      this.employeeCodeError = 'Employee code is required';
      hasErrors = true;
    }

    if (!this.newEmployee.lastName || this.newEmployee.lastName.trim() === '') {
      this.lastNameError = 'Last name is required';
      hasErrors = true;
    }

    if (!this.newEmployee.firstName || this.newEmployee.firstName.trim() === '') {
      this.firstNameError = 'First name is required';
      hasErrors = true;
    }

    if (!this.newEmployee.middleName || this.newEmployee.middleName.trim() === '') {
      this.middleNameError = 'Middle name is required';
      hasErrors = true;
    }

    if (!this.newEmployee.gender || this.newEmployee.gender.trim() === '') {
      this.genderError = 'Gender is required';
      hasErrors = true;
    }

    if (!this.newEmployee.address || this.newEmployee.address.trim() === '') {
      this.addressError = 'Address is required';
      hasErrors = true;
    }

    // If there are validation errors, don't proceed to create-account
    if (hasErrors) {
      return;
    }

    // Create the DTO object to send to the server
    const requestPayload: CreateEmployeeRequest = {
      employeeCode: this.newEmployee.employeeCode,
      lastName: this.newEmployee.lastName,
      firstName: this.newEmployee.firstName,
      middleName: this.newEmployee.middleName,
      age: this.newEmployee.age,
      birthDate: new Date(this.newEmployee.birthDate).toISOString(),
      civilStatus: this.newEmployee.civilStatus,
      gender: this.newEmployee.gender,
      email: this.newEmployee.email,
      phoneNumber: this.newEmployee.phoneNumber,
      address: this.newEmployee.address,
      rolePosition: this.newEmployee.rolePosition,
      createdBy: this.loggedInEmployeeId,
      createdAt: new Date(),
      isActive: true,
      archivedBy: this.newEmployee.archivedBy,
      archivedAt: this.newEmployee.archivedAt,
      updatedBy: this.newEmployee.updatedBy,
      updatedAt: this.newEmployee.updatedAt,
    };

    this.postService.createEmployee(requestPayload).subscribe({
      next: res => {
        console.log(`${res.message}`);
        this.newEmployeeId = res.employeeId;
        this.clearFields();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  clearFields() {
    if (!this.newEmployee) {
      this.newEmployee = new Employee();
    }
    this.newEmployee.employeeCode = '';
    this.newEmployee.lastName = '';
    this.newEmployee.firstName = '';
    this.newEmployee.middleName = '';
    this.newEmployee.age = 0;
    this.newEmployee.gender = '';
    this.newEmployee.phoneNumber = '';
    this.newEmployee.civilStatus = '';
  }

  closeCreateEmployeeForm() {
    // Clear all errors when closing form
    this.employeeCodeError = '';
    this.lastNameError = '';
    this.firstNameError = '';
    this.middleNameError = '';
    this.genderError = '';
    this.ageError = '';
    this.birthDateError = '';
    this.phoneNumberError = '';
    this.civilStatusError = '';
    this.rolePositionError = '';
    this.emailError = '';
    this.addressError = '';
    this.closeForm.emit();
  }

  closeCreateAccountForm() {
    this.newEmployeeId = 0;
    this.closeForm.emit();
  }

  getLoggedInEmployee() {
    var token = localStorage.getItem(LogInToken);
    if(!token) {
      console.log('Token does not exist.');
      return;
    }

    var employee : any = jwtDecode(token);
    console.log("Decoded token:", employee);
    var employeeId = parseInt(employee.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: res => {
        this.loggedInEmployeeId = res.id;
         console.log("Logged in employee ID:", this.loggedInEmployeeId);
        this.isUserLoading = false;
      },
      error: err => {
        console.log(`${err.error}`);
        this.isUserLoading = false;
      }
    });
  }
}