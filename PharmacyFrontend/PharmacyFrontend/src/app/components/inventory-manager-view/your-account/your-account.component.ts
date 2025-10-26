import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Employee } from '../../../models/employee-models/employee';
import { GetService } from '../../../services/GetService/get.service';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Account } from '../../../models/account';
import { UpdateService } from '../../../services/UpdateService/update.service';
import { UpdateEmployeeAccountRequest } from '../../../models/update-employee-account-request';

@Component({
  selector: 'app-your-account',
  standalone: true,
  imports: [DatePipe, NgFor, NgIf, FormsModule],
  templateUrl: './your-account.component.html',
  styleUrl: './your-account.component.scss',
})
export class YourAccountComponent implements OnInit {
  employee: Employee = new Employee();
  account: Account = new Account();
  isEditingCredentials: boolean = false;
  updateErrorMessage: string = '';
  updateSuccessMessage: string = '';
  
  currentPassword: string = '';
  username: string = '';
  password: string = '';
  passwordConfirmation: string = '';

  constructor(
    private getService: GetService,
    private updateService: UpdateService
  ) {}

  ngOnInit(): void {
    this.getLoggedInEmployee();
  }

  getLoggedInEmployee() {
    const token = localStorage.getItem(LogInToken);
    if (!token) {
      console.log(`Token does not exist.`);
      return;
    }

    const employee: any = jwtDecode(token);
    const employeeId = parseInt(employee.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
        this.getEmployeeAccount();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  getEmployeeAccount() {
    this.getService.getAccountByEmployeeIdAsync(this.employee.id).subscribe({
      next: res => {
        this.account = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  toggleEditCredentials() {
    this.isEditingCredentials = !this.isEditingCredentials;
    
    if (this.isEditingCredentials) {
      // Initialize with current values when entering edit mode
      this.currentPassword = '';
      this.username = ''; // Replace with actual username from employee object
      this.password = '';
      this.passwordConfirmation = '';
    }
  }

  saveCredentials() {
    const requestPayload: UpdateEmployeeAccountRequest = {
      currentPassword: this.currentPassword,
      newUserName: this.username,
      newPassword: this.password,
      confirmNewPassword: this.passwordConfirmation,
      accountId: this.account.id
    }
    this.updateService.updateEmployeeAccountAsync(requestPayload).subscribe({
      next: res => {
        this.updateSuccessMessage = res.message;
        this.getLoggedInEmployee();
        this.isEditingCredentials = false;
      },
      error: err => {
        this.updateErrorMessage = err.error.message;
        console.log(`${JSON.stringify(requestPayload)}`);
      }
    });
  }

  cancelEdit() {
    this.isEditingCredentials = false;
    this.currentPassword = '';
    this.username = '';
    this.password = '';
    this.passwordConfirmation = '';
  }

  closeErrorMessage() {
    this.updateErrorMessage = '';
  }

  closeSucceessMessage() {
    this.updateSuccessMessage = '';
  }
}