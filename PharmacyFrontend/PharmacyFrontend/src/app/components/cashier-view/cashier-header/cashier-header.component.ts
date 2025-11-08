import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Product } from '../../../models/product-models/product';
import { NgIf, DatePipe } from '@angular/common';
import { ViewCashierProductsComponent } from '../view-cashier-products/view-cashier-products.component';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';

@Component({
  selector: 'app-cashier-header',
  standalone: true,
  imports: [RouterLink, NgIf, ViewCashierProductsComponent, DatePipe],
  templateUrl: './cashier-header.component.html',
  styleUrl: './cashier-header.component.scss',
})
export class CashierHeaderComponent implements OnInit, OnDestroy {
  viewCashierProduct?: Product | null;
  dateTime: Date = new Date();
  timer: any;
  employee: Employee = new Employee();

  constructor(
    private router: Router,
    private getService: GetService
  ) {}

  ngOnInit(): void {
    // Update date/time every second
    this.timer = setInterval(() => {
      this.dateTime = new Date();
    }, 1000);

    // Load employee information
    this.getLoggedInEmployee();
  }

  ngOnDestroy(): void {
    // Clean up timer when component is destroyed
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  openViewCashierProductForm() {
    this.viewCashierProduct = new Product();
  }

  closeCashierProductForm() {
    this.viewCashierProduct = null;
  }

  logOut() {
    localStorage.removeItem(LogInToken);
    this.router.navigate(['/log-in'], { replaceUrl: true });
    this.router.navigateByUrl('/log-in');
  }

  getLoggedInEmployee() {
    const token = localStorage.getItem(LogInToken);
    if (!token) {
      console.log('Token does not exist.');
      return;
    }

    const decodedToken: any = jwtDecode(token);
    const employeeId = parseInt(decodedToken.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
      },
      error: (err) => {
        console.log(`Error: ${err.error}`);
      },
    });
  }
}
