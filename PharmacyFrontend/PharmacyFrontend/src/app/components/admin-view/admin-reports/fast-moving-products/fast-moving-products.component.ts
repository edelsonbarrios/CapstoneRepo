import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetService } from '../../../../services/GetService/get.service';
import { FastMovingProduct } from '../../../../models/report-models/fast-moving-product';
import { FastMovingProductReport } from '../../../../models/report-models/fast-moving-product-report';

@Component({
  selector: 'app-fast-moving-products',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './fast-moving-products.component.html',
  styleUrl: './fast-moving-products.component.scss'
})
export class FastMovingProductsComponent implements OnInit {
  startDate: string = '';
  endDate: string = '';
  selectedTopN: string = '';
  fastMovingReport: FastMovingProductReport | null = null;

  loading: boolean = false;
  error: string = '';
  exporting: boolean = false;

  // Pagination properties
  paginatedProducts: FastMovingProduct[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalPages: number = 0;

  // Modal
  showExportModal: boolean = false;
  showValidationModal: boolean = false;
  validationMessage: string = '';

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getFastMovingReport();
  }

  // Helper method to format date as YYYY-MM-DD
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  getFastMovingReport() {
    this.loading = true;
    this.error = '';

    const topN = this.selectedTopN ? parseInt(this.selectedTopN) : 20;

    const start = this.startDate && this.startDate.trim() !== '' ? this.startDate : undefined;
    const end = this.endDate && this.endDate.trim() !== '' ? this.endDate : undefined;

    console.log('Calling API with:', { start, end, topN });

    this.getService.getFastMovingProductReportAsync(
      start,
      end,
      topN
    ).subscribe({
      next: (res) => {
        this.fastMovingReport = res;
        this.loading = false;
        this.setupPagination();
      },
      error: (err) => {
        console.error('API Error:', err);
        this.error = err.error?.message || err.message || 'Failed to load report';
        this.loading = false;
      }
    });
  }

  setupPagination() {
    const products = this.fastMovingReport?.products || [];
    this.totalPages = Math.ceil(products.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    this.currentPage = 1; // Reset to first page
    this.updatePaginatedData();
  }

  updatePaginatedData() {
    const products = this.fastMovingReport?.products || [];
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedProducts = products.slice(start, end);
  }

  onFilterChange() {
    this.currentPage = 1;
    this.getFastMovingReport();
  }

  resetFilters() {
    const today = new Date();
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(today.getDate() - 30);
    
    this.startDate = this.formatDate(thirtyDaysAgo);
    this.endDate = this.formatDate(today);
    this.selectedTopN = '';
    this.onFilterChange();
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedData();
    }
  }

  showExportConfirmation(): void {
    // Validate that both start and end dates are provided
    if (!this.startDate || !this.endDate) {
      this.validationMessage = 'Please select both Start Date and End Date before exporting.';
      this.showValidationModal = true;
      return;
    }

    // Validate that start date is not after end date
    if (new Date(this.startDate) > new Date(this.endDate)) {
      this.validationMessage = 'Start Date cannot be later than End Date.';
      this.showValidationModal = true;
      return;
    }
    this.showExportModal = true;
  }

  closeValidationModal(): void {
    this.showValidationModal = false;
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
    const topN = this.selectedTopN ? parseInt(this.selectedTopN) : undefined;

    this.getService.exportFastMovingProductsReport(start, end, topN).subscribe({
      next: (blob) => {
        // Create download link
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        // Generate filename
        const topNSuffix = this.selectedTopN ? `_Top${this.selectedTopN}` : '';
        const dateRange = this.startDate && this.endDate ? `_${this.startDate}_${this.endDate}` : '_AllTime';
        const fileName = `FastMovingProducts${dateRange}${topNSuffix}.xlsx`;
        link.download = fileName;
        
        // Trigger download
        link.click();
        
        // Cleanup
        window.URL.revokeObjectURL(url);
        this.exporting = false;
        
        console.log('Fast moving products report exported successfully');
      },
      error: (err) => {
        console.error('Error exporting fast moving products report:', err);
        alert('Failed to export report. Please try again.');
        this.exporting = false;
      }
    });
  }

  // Helper methods for styling
  getStockClass(stock: number): string {
    if (stock === 0) return 'out-of-stock';
    if (stock < 10) return 'low-stock';
    if (stock < 50) return 'moderate-stock';
    return 'adequate-stock';
  }

  getStockStatusClass(status: string | undefined): string {
    if (!status) return '';
    switch (status.toLowerCase()) {
      case 'out of stock':
        return 'status-out';
      case 'low stock':
        return 'status-low';
      case 'moderate':
        return 'status-moderate';
      case 'adequate':
        return 'status-adequate';
      default:
        return '';
    }
  }

  getTrendClass(trend: string): string {
    switch (trend.toLowerCase()) {
      case 'increasing':
        return 'trend-increasing';
      case 'stable':
        return 'trend-stable';
      case 'decreasing':
        return 'trend-decreasing';
      default:
        return '';
    }
  }
}