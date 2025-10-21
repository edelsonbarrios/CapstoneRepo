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
  // Filters
  startDate: string = '';
  endDate: string = '';
  selectedEmployeeId: number | null = null;
  selectedPaymentMethod: string = '';
  cashiers: Employee[] = [];
  selectedCashier: Employee = new Employee();

  // Data
  salesReport: SalesReport | null = null;
  paginatedTransactions: any[] = [];
  loading: boolean = false;
  error: string | null = null;
  exporting: boolean = false;

  // Dropdowns
  paymentMethods: string[] = ['Cash', 'Online'];

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 4;
  totalPages: number = 0;

  // Modal
  showExportModal: boolean = false;

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
        this.currentPage = 1;
        this.setupPagination();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error;
      }
    });
  }

  setupPagination(): void {
    if (!this.salesReport) return;
    const transactions = this.salesReport.transactions || [];
    this.totalPages = Math.ceil(transactions.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    this.updatePaginatedData();
  }

  updatePaginatedData(): void {
    if (!this.salesReport) return;
    const transactions = this.salesReport.transactions || [];
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedTransactions = transactions.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedData();
    }
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadSalesReport();
    this.updatePaginatedData();
  }

  showExportConfirmation(): void {
    this.showExportModal = true;
  }

  confirmExport(): void {
    this.showExportModal = false;
    this.exportToExcel();
  }

  cancelExport(): void {
    this.showExportModal = false;
  }

  exportToExcel(): void {
    if (this.exporting) return;

    this.exporting = true;

    const start = this.startDate ? new Date(this.startDate) : undefined;
    const end = this.endDate ? new Date(this.endDate) : undefined;
    const empId = this.selectedEmployeeId || undefined;
    const payment = this.selectedPaymentMethod || undefined;

    this.getService.exportSalesReport(start, end, empId, payment).subscribe({
      next: (blob) => {
        // Create download link
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        // Generate filename
        const fileName = `SalesReport_${this.startDate}_${this.endDate}.xlsx`;
        link.download = fileName;
        
        // Trigger download
        link.click();
        
        // Cleanup
        window.URL.revokeObjectURL(url);
        this.exporting = false;
        
        console.log('Sales report exported successfully');
      },
      error: (err) => {
        console.error('Error exporting sales report:', err);
        alert('Failed to export report. Please try again.');
        this.exporting = false;
      }
    });
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
        const cashiers = res.filter(e => e.rolePosition === 'Cashier');
        this.cashiers = cashiers;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}