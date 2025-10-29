import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CurrencyPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { ProductSalesSummary } from '../../../models/sales-transaction-models/product-sales-summary';
import { Product } from '../../../models/product-models/product';
import { Inventory } from '../../../models/inventory-module-models/inventory';
import { GetService } from '../../../services/GetService/get.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [RouterLink, NgFor, NgIf, CurrencyPipe, DatePipe],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent implements OnInit {
  todaySalesAmount: number = 0;
  lowStockCount: number = 0;
  outOfStockCount: number = 0;
  topSellingMedicines: ProductSalesSummary[] = [];
  paginatedTopMeds: ProductSalesSummary[] = [];
  dateToday: Date = new Date();

  // Expiring Soon data
  expiringProducts: Inventory[] = [];
  paginatedExpiringProducts: Inventory[] = [];

  // Pagination properties for Top Selling Medicines
  topMedsCurrentPage: number = 1;
  topMedsItemsPerPage: number = 2;
  topMedsTotalPages: number = 0;

  // Pagination properties for Expiring Soon
  expiryCurrentPage: number = 1;
  expiryItemsPerPage: number = 3;
  expiryTotalPages: number = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getTodaySalesAmount();
    this.getLowStockCount();
    this.getOutOfStockCount();
    this.getNearExpiryInventory();
    this.getTopSellingProductsMonthly();
  }

  getTodaySalesAmount() {
    this.getService.getTodaySalesAmountAsync().subscribe({
      next: (res) => (this.todaySalesAmount = res),
      error: (err) => console.log(`${err.error}`),
    });
  }

  getLowStockCount() {
    this.getService.getInventoryLowStockCountAsync().subscribe({
      next: (res) => (this.lowStockCount = res),
      error: (err) => console.log(`${err.error}`),
    });
  }

  getOutOfStockCount() {
    this.getService.getInventoryOutOfStockCountAsync().subscribe({
      next: res => this.outOfStockCount = res,
      error: err => console.log(`${err.error}`)
    });
  }

  getNearExpiryInventory() {
    this.getService.getInventoryNearExpiry().subscribe({
      next: res => {
        this.expiringProducts = res;
        this.updateExpiryPagination();
      },
      error: err => console.log(`${err.error}`)
    });
  }

  getTopSellingProductsMonthly() {
    this.getService.getTopSellingProductsMonthly().subscribe({
      next: res => {
        this.topSellingMedicines = res;
        console.log(`${JSON.stringify(res)}`);
        this.updateTopMedsPagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updateTopMedsPagination() {
    this.topMedsTotalPages = Math.ceil(
      this.topSellingMedicines.length / this.topMedsItemsPerPage
    );
    if (this.topMedsTotalPages === 0) this.topMedsTotalPages = 1;
    const startIndex = (this.topMedsCurrentPage - 1) * this.topMedsItemsPerPage;
    const endIndex = startIndex + this.topMedsItemsPerPage;
    this.paginatedTopMeds = this.topSellingMedicines.slice(
      startIndex,
      endIndex
    );
  }

  goToTopMedsPage(page: number) {
    if (page >= 1 && page <= this.topMedsTotalPages) {
      this.topMedsCurrentPage = page;
      this.updateTopMedsPagination();
    }
  }

  // Expiring Soon Pagination
  updateExpiryPagination() {
    this.expiryTotalPages = Math.ceil(
      this.expiringProducts.length / this.expiryItemsPerPage
    );
    if (this.expiryTotalPages === 0) this.expiryTotalPages = 1;
    const startIndex = (this.expiryCurrentPage - 1) * this.expiryItemsPerPage;
    const endIndex = startIndex + this.expiryItemsPerPage;
    this.paginatedExpiringProducts = this.expiringProducts.slice(
      startIndex,
      endIndex
    );
  }

  goToExpiryPage(page: number) {
    if (page >= 1 && page <= this.expiryTotalPages) {
      this.expiryCurrentPage = page;
      this.updateExpiryPagination();
    }
  }
}
