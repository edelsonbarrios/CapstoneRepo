import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { LogInToken } from '../../shared/token';
import { GetService } from '../../../services/GetService/get.service';
import { jwtDecode }from 'jwt-decode';
import { Employee } from '../../../models/employee-models/employee';

@Component({
  selector: 'app-admin-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.scss'
})
export class AdminHeaderComponent implements OnInit{
  employee : Employee = new Employee();

  constructor(
    private router : Router,
    private getService : GetService
  ) {}

  ngOnInit(): void {
      this.getLoggedInEmployee();
  }

  logOut() {
    localStorage.removeItem(LogInToken);
    this.router.navigateByUrl('/log-in');
  }

  getLoggedInEmployee() {
    const token = localStorage.getItem(LogInToken);
    if(!token){
      console.log(`Token does not exist.`);
      return;
    }

    const employee : any = jwtDecode(token);
    const employeeId = parseInt(employee.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: employee => {
        this.employee = employee;
        console.log(employee);
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

}
