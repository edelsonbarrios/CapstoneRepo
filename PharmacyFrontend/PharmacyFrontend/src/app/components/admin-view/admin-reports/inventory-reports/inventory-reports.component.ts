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

  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 4;
  totalPages: number = 0;
  paginatedInventory: any[] = [];

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
      filterType: this.selectedFilterType,
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
}
