import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../../services/GetService/get.service';
import { InventoryReport } from '../../../../models/report-models/inventory-report';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-inventory-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './inventory-reports.component.html',
  styleUrl: './inventory-reports.component.scss',
})
export class InventoryReportsComponent implements OnInit {
  startDate: string = '';
  endDate: string = '';
  selectedFilterType: string = '';
  inventoryReport: InventoryReport = new InventoryReport();

  filterTypes: string[] = ['Critical Stock', 'Near Expiry', 'Expired'];

  loading: boolean = false;
  error: string = '';
  exporting: boolean = false;

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;
  paginatedInventory: any[] = [];

  // Modal
  showExportModal: boolean = false;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getInventoryReport();
  }

  getInventoryReport() {
    const requestPayload: any = {
      startDate: this.startDate
        ? new Date(this.startDate).toISOString()
        : undefined,
      endDate: this.endDate ? new Date(this.endDate).toISOString() : undefined,
      filterType: this.selectedFilterType || undefined,
    };

    this.loading = true;
    this.getService
      .getInventoryReportsAsync(
        requestPayload.startDate,
        requestPayload.endDate,
        requestPayload.filterType
      )
      .subscribe({
        next: (res) => {
          this.inventoryReport = res;
          this.loading = false;
          this.setupPagination();
        },
        error: (err) => {
          this.error = err.error;
          this.loading = false;
        },
      });
  }

  setupPagination() {
    const items = this.inventoryReport.inventoryItems || [];
    this.totalPages = Math.ceil(items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    this.updatePaginatedData();
  }

  resetFilters() {
    this.startDate = '';
    this.endDate = '';
    this.selectedFilterType = '';
    this.onFilterChange();
  }

  updatePaginatedData() {
    const items = this.inventoryReport.inventoryItems || [];
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedInventory = items.slice(start, end);
  }

  onFilterChange() {
    this.currentPage = 1;
    this.getInventoryReport();
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedData();
    }
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
    const filterType = this.selectedFilterType || undefined;

    this.getService.exportInventoryReport(start, end, filterType).subscribe({
      next: (blob) => {
        // Create download link
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        // Generate filename
        const filterSuffix = this.selectedFilterType ? `_${this.selectedFilterType}` : '';
        const fileName = `InventoryReport_${this.startDate}_${this.endDate}${filterSuffix}.xlsx`;
        link.download = fileName;
        
        // Trigger download
        link.click();
        
        // Cleanup
        window.URL.revokeObjectURL(url);
        this.exporting = false;
        
        console.log('Inventory report exported successfully');
      },
      error: (err) => {
        console.error('Error exporting inventory report:', err);
        alert('Failed to export report. Please try again.');
        this.exporting = false;
      }
    });
  }

  getInventoryStatus(inv: any): string {
    if (inv.isExpired) {
      return 'Expired';
    } else if (inv.isNearExpiry) {
      return 'Near Expiry';
    } else if (inv.isCriticalStock) {
      return 'Critical Stock';
    } else {
      return 'Normal';
    }
  }
}