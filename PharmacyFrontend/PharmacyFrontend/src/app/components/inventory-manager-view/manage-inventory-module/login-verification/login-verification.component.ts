import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { LogInToken } from '../../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { Employee } from '../../../../models/employee-models/employee';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Account } from '../../../../models/account';

@Component({
  selector: 'app-login-verification',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login-verification.component.html',
  styleUrl: './login-verification.component.scss',
})
export class LoginVerificationComponent implements OnInit {
  enteredUserName: string = '';
  enteredPassword: string = '';
  employee: Employee = new Employee();
  @Output() closeForm = new EventEmitter<boolean>();
  account: Account = new Account();
  errorMessage: string = '';

  constructor(private getService: GetService) {}

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
      next: (res) => {
        this.account = res;
        console.log('Account loaded:', this.account);
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  closeVerificationForm() {
    this.closeForm.emit(false);
  }

  verification() {
    console.log(`Account: ${JSON.stringify(this.account)}`);
    console.log(`Entered Username: ${this.enteredUserName}`);
    console.log(`Entered Password: ${this.enteredPassword}`);
    if (this.enteredUserName != this.account.username) {
      this.errorMessage = 'Invalid Username. Please try again';
      this.clearFields();
      return;
    } else if (this.enteredPassword != this.account.password) {
      this.errorMessage = 'Invalid Password. Please try again';
      this.clearFields();
      return;
    }

    this.closeForm.emit(true);
  }

  clearFields() {
    this.enteredUserName = '';
    this.enteredPassword = '';
  }

  closeErrorMessageForm() {
    this.errorMessage = '';
  }
}
