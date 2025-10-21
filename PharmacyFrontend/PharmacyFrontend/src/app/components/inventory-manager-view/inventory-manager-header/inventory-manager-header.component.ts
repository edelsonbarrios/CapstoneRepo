import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-inventory-manager-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './inventory-manager-header.component.html',
  styleUrl: './inventory-manager-header.component.scss',
})
export class InventoryManagerHeaderComponent implements OnInit {
  employee: Employee = new Employee();

  constructor(
    private router: Router,
    private getService: GetService
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

  getInitials(fullName: string): string {
    if (!fullName) return 'U';
    
    const names = fullName.trim().split(' ');
    if (names.length === 1) {
      return names[0].charAt(0).toUpperCase();
    }
    
    return (names[0].charAt(0) + names[names.length - 1].charAt(0)).toUpperCase();
  }
}