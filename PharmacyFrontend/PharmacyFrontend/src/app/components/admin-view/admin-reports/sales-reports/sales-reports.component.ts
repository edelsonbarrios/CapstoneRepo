import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, CurrencyPipe, DatePipe } from '@angular/common';
import { GetService } from '../../../../services/GetService/get.service';
import { SalesReport } from '../../../../models/report-models/sales-report';
import { Employee } from '../../../../models/employee-models/employee';

@Component({
  selector: 'app-sales-reports',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, CurrencyPipe, DatePipe],
  templateUrl: './sales-reports.component.html',
  styleUrl: './sales-reports.component.scss'
})
export class SalesReportsComponent implements OnInit {
  // Filter controls
  startDate: string = '';
  endDate: string = '';
  selectedEmployeeId: number | null = null;
  selectedPaymentMethod: string = '';
  cashiers: Employee[] = [];
  selectedCashier: Employee = new Employee();

  // Report data
  salesReport: SalesReport | null = null;
  loading: boolean = false;
  error: string | null = null;

  // Dropdown options
  employees: any[] = [];
  paymentMethods: string[] = ['Cash', 'Credit Card', 'Debit Card', 'GCash'];

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.loadSalesReport();
    this.getAllCashiers();
  }

  loadSalesReport(): void {
    this.loading = true;
    this.error = null;

    const start = this.startDate ? new Date(this.startDate) : undefined;
    const end = this.endDate ? new Date(this.endDate) : undefined;
    const empId = this.selectedEmployeeId || undefined;
    const payment = this.selectedPaymentMethod || undefined;

    this.getService.getSalesReport(start, end, empId, payment).subscribe({
      next: (report) => {  
        this.salesReport = report;
        this.loading = false;
      },
      error: (err) => {        
        console.log(`${err.error}`);
        this.loading = false;
      }
    });
  }

  onFilterChange(): void {
    this.loadSalesReport();
  }

  clearFilters(): void {
    this.startDate = '';
    this.endDate = '';
    this.selectedEmployeeId = null;
    this.selectedPaymentMethod = '';
    this.loadSalesReport();
  }

  exportToExcel(): void {
    console.log('Exporting to Excel...', this.salesReport);
  }

  resetFilterValues() {
    this.startDate = '';
    this.endDate = '';
    this.selectedCashier = new Employee();
    this.selectedPaymentMethod = '';
    this.loadSalesReport();
  }

  getAllCashiers() {
    this.getService.GetAllEmployees().subscribe({
      next: res => {
        const cashiers = res.filter(e => e.rolePosition == 'Cashier');
        this.cashiers = cashiers;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}