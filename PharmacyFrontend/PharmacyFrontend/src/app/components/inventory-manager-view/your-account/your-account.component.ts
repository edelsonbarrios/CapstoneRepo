import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Employee } from '../../../models/employee-models/employee';
import { GetService } from '../../../services/GetService/get.service';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { DatePipe, NgFor } from '@angular/common';

@Component({
  selector: 'app-your-account',
  standalone: true,
  imports: [DatePipe, NgFor],
  templateUrl: './your-account.component.html',
  styleUrl: './your-account.component.scss',
})
export class YourAccountComponent implements OnInit{
  employee: Employee = new Employee();

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
        console.log(employee);
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }
}
