import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { LogInToken } from '../../shared/token';
import { GetService } from '../../../services/GetService/get.service';
import { jwtDecode } from 'jwt-decode';
import { Employee } from '../../../models/employee-models/employee';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.scss'
})
export class AdminHeaderComponent implements OnInit {
  employee: Employee = new Employee();
  isInventoryDropdownOpen: boolean = false;

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
      next: employee => {
        this.employee = employee;
        console.log(employee);
      },
      error: err => {
        console.log(`${err.error}`);
      }
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

  toggleInventoryDropdown() {
    this.isInventoryDropdownOpen = !this.isInventoryDropdownOpen;
  }

  isInventoryRouteActive(): boolean {
    return this.router.url.includes('/admin-main/admin-product-main') ||
           this.router.url.includes('/admin-main/admin-inventory-module-main') ||
           this.router.url.includes('/admin-main/admin-inventory-purchase-order') ||
           this.router.url.includes('/admin-main/admin-manage-replenishment') ||
           this.router.url.includes('/admin-main/admin-disposable');
  }
}