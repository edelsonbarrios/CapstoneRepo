import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { InventoryDashboard } from '../../../models/inventory-module-models/inventory-dashboard';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { Inventory } from '../../../models/inventory-module-models/inventory';

@Component({
  selector: 'app-inventory-manager-dashboard',
  standalone: true,
  imports: [FormsModule, CommonModule, DatePipe],
  templateUrl: './inventory-manager-dashboard.component.html',
  styleUrl: './inventory-manager-dashboard.component.scss',
})
export class InventoryManagerDashboardComponent implements OnInit {
  inventoryDashboard: InventoryDashboard = new InventoryDashboard();

  // Paginated arrays
  paginatedCriticalItems: Inventory[] = [];
  paginatedNearExpiryItems: any[] = [];
  paginatedExpiredItems: any[] = [];

  // Critical Stock pagination properties
  criticalCurrentPage: number = 1;
  criticalItemsPerPage: number = 3;
  criticalTotalPages: number = 0;

  // Near Expiry pagination properties
  nearExpiryCurrentPage: number = 1;
  nearExpiryItemsPerPage: number = 3;
  nearExpiryTotalPages: number = 0;

  // Expired Stock pagination properties
  expiredCurrentPage: number = 1;
  expiredItemsPerPage: number = 3;
  expiredTotalPages: number = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getInventoryDashboardValues();
  }

  getInventoryDashboardValues() {
    this.getService.getInventoryDashboardValues().subscribe({
      next: (res) => {
        this.inventoryDashboard = res;

        this.criticalCurrentPage = 1;
        this.nearExpiryCurrentPage = 1;
        this.expiredCurrentPage = 1;

        this.updateCriticalPagination();
        this.updateNearExpiryPagination();
        this.updateExpiredPagination();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  // Critical Stock Pagination Methods
  updateCriticalPagination() {
    this.criticalTotalPages = Math.ceil(
      this.inventoryDashboard.criticalStockItems.length /
        this.criticalItemsPerPage
    );
    if (this.criticalTotalPages === 0) this.criticalTotalPages = 1;
    const startIndex =
      (this.criticalCurrentPage - 1) * this.criticalItemsPerPage;
    const endIndex = startIndex + this.criticalItemsPerPage;
    this.paginatedCriticalItems =
      this.inventoryDashboard.criticalStockItems.slice(startIndex, endIndex);
  }

  goToCriticalPage(page: number) {
    if (page >= 1 && page <= this.criticalTotalPages) {
      this.criticalCurrentPage = page;
      this.updateCriticalPagination();
    }
  }

  // Near Expiry Pagination Methods
  updateNearExpiryPagination() {
    this.nearExpiryTotalPages = Math.ceil(
      this.inventoryDashboard.nearExpiryItems.length /
        this.nearExpiryItemsPerPage
    );
    if (this.nearExpiryTotalPages === 0) this.nearExpiryTotalPages = 1;
    const startIndex =
      (this.nearExpiryCurrentPage - 1) * this.nearExpiryItemsPerPage;
    const endIndex = startIndex + this.nearExpiryItemsPerPage;
    this.paginatedNearExpiryItems =
      this.inventoryDashboard.nearExpiryItems.slice(startIndex, endIndex);
  }

  goToNearExpiryPage(page: number) {
    if (page >= 1 && page <= this.nearExpiryTotalPages) {
      this.nearExpiryCurrentPage = page;
      this.updateNearExpiryPagination();
    }
  }

  // Expired Stock Pagination Methods
  updateExpiredPagination() {
    this.expiredTotalPages = Math.ceil(
      this.inventoryDashboard.expiredStockItems.length /
        this.expiredItemsPerPage
    );
    if (this.expiredTotalPages === 0) this.expiredTotalPages = 1;
    const startIndex = (this.expiredCurrentPage - 1) * this.expiredItemsPerPage;
    const endIndex = startIndex + this.expiredItemsPerPage;
    this.paginatedExpiredItems =
      this.inventoryDashboard.expiredStockItems.slice(startIndex, endIndex);
  }

  goToExpiredPage(page: number) {
    if (page >= 1 && page <= this.expiredTotalPages) {
      this.expiredCurrentPage = page;
      this.updateExpiredPagination();
    }
  }
}
