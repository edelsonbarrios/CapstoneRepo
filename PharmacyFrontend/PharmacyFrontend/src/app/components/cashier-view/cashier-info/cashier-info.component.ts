import { Component, OnInit } from '@angular/core';
import { NgFor, NgIf, CommonModule } from '@angular/common';
import { GetService } from '../../../services/GetService/get.service';
import { ProductSalesSummary } from '../../../models/sales-transaction-models/product-sales-summary';
import { FastMovingProduct } from '../../../models/report-models/fast-moving-product';
import { FastMovingProductReport } from '../../../models/report-models/fast-moving-product-report';
import { InventoryDashboard } from '../../../models/inventory-module-models/inventory-dashboard';
import { Inventory } from '../../../models/inventory-module-models/inventory';

// Extended interface for top selling medicines with unit breakdown
interface TopSellingMedWithBreakdown extends ProductSalesSummary {
  unitType: 'Multi-Level' | 'Dual-Level' | 'Single-Level';
  quantitySoldBreakdown: {
    box?: number;
    sheet?: number;
    piece: number;
  };
  totalQty: number;
  boxSold?: number;
  sheetSold?: number;
  pieceSold?: number;
  unitMeasurement?: string;
}

@Component({
  selector: 'app-cashier-info',
  standalone: true,
  imports: [NgFor, NgIf, CommonModule],
  templateUrl: './cashier-info.component.html',
  styleUrl: './cashier-info.component.scss'
})
export class CashierInfoComponent implements OnInit {
  // Top Selling Products
  topSellingMedicines: TopSellingMedWithBreakdown[] = [];
  paginatedTopMeds: TopSellingMedWithBreakdown[] = [];
  
  topMedsCurrentPage: number = 1;
  topMedsItemsPerPage: number = 5;
  topMedsTotalPages: number = 0;

  // Fast Moving Products
  fastMovingProducts: FastMovingProduct[] = [];
  paginatedFastMoving: FastMovingProduct[] = [];
  fastMovingReport: FastMovingProductReport | null = null;
  
  fastMovingCurrentPage: number = 1;
  fastMovingItemsPerPage: number = 10;
  fastMovingTotalPages: number = 0;
  
  fastMovingLoading: boolean = false;
  fastMovingError: string = '';

  // Critical Stock
  criticalStockItems: Inventory[] = [];
  paginatedCriticalStock: Inventory[] = [];
  inventoryDashboard: InventoryDashboard | null = null;
  
  criticalCurrentPage: number = 1;
  criticalItemsPerPage: number = 3;
  criticalTotalPages: number = 0;
  
  criticalLoading: boolean = false;
  criticalError: string = '';

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getTopSellingProductsMonthly();
    this.getFastMovingProducts();
    this.getCriticalStockItems();
  }

  getTopSellingProductsMonthly() {
    this.getService.getTopSellingProductsMonthly().subscribe({
      next: res => {
        console.log('Backend response:', res);
        
        this.topSellingMedicines = res.map(product => {
          const mapped: TopSellingMedWithBreakdown = {
            ...product,
            unitType: (product.unitMeasurement || 'Single-Level') as 'Multi-Level' | 'Dual-Level' | 'Single-Level',
            quantitySoldBreakdown: {
              box: product.boxSold ?? undefined,
              sheet: product.sheetSold ?? undefined,
              piece: product.pieceSold ?? product.quantitySold ?? 0
            },
            totalQty: product.quantitySold || 0,
            boxSold: product.boxSold,
            sheetSold: product.sheetSold,
            pieceSold: product.pieceSold,
            unitMeasurement: product.unitMeasurement
          };
          
          console.log('Mapped product:', mapped);
          return mapped;
        });
        
        this.updateTopMedsPagination();
      },
      error: err => console.log(`${err.error}`)
    });
  }

  getFastMovingProducts() {
    this.fastMovingLoading = true;
    this.fastMovingError = '';

    this.getService.getFastMovingProductReportAsync(
      undefined,
      undefined,
      20
    ).subscribe({
      next: (res) => {
        this.fastMovingReport = res;
        this.fastMovingProducts = res.products || [];
        this.fastMovingLoading = false;
        this.updateFastMovingPagination();
        console.log('Fast moving products loaded:', this.fastMovingProducts);
      },
      error: (err) => {
        console.error('Fast moving API Error:', err);
        this.fastMovingError = err.error?.message || err.message || 'Failed to load fast moving products';
        this.fastMovingLoading = false;
      }
    });
  }

  updateTopMedsPagination() {
    this.topMedsTotalPages = Math.ceil(this.topSellingMedicines.length / this.topMedsItemsPerPage);
    if (this.topMedsTotalPages === 0) this.topMedsTotalPages = 1;
    const startIndex = (this.topMedsCurrentPage - 1) * this.topMedsItemsPerPage;
    const endIndex = startIndex + this.topMedsItemsPerPage;
    this.paginatedTopMeds = this.topSellingMedicines.slice(startIndex, endIndex);
  }

  updateFastMovingPagination() {
    this.fastMovingTotalPages = Math.ceil(this.fastMovingProducts.length / this.fastMovingItemsPerPage);
    if (this.fastMovingTotalPages === 0) this.fastMovingTotalPages = 1;
    const startIndex = (this.fastMovingCurrentPage - 1) * this.fastMovingItemsPerPage;
    const endIndex = startIndex + this.fastMovingItemsPerPage;
    this.paginatedFastMoving = this.fastMovingProducts.slice(startIndex, endIndex);
  }

  goToTopMedsPage(page: number) {
    if (page >= 1 && page <= this.topMedsTotalPages) {
      this.topMedsCurrentPage = page;
      this.updateTopMedsPagination();
    }
  }

  goToFastMovingPage(page: number) {
    if (page >= 1 && page <= this.fastMovingTotalPages) {
      this.fastMovingCurrentPage = page;
      this.updateFastMovingPagination();
    }
  }

  getCriticalStockItems() {
    this.criticalLoading = true;
    this.criticalError = '';

    this.getService.getInventoryDashboardValues().subscribe({
      next: (res) => {
        this.inventoryDashboard = res;
        this.criticalStockItems = res.criticalStockItems || [];
        this.criticalLoading = false;
        this.updateCriticalPagination();
        console.log('Critical stock items loaded:', this.criticalStockItems);
      },
      error: (err) => {
        console.error('Critical stock API Error:', err);
        this.criticalError = err.error?.message || err.message || 'Failed to load critical stock items';
        this.criticalLoading = false;
      }
    });
  }

  updateCriticalPagination() {
    this.criticalTotalPages = Math.ceil(this.criticalStockItems.length / this.criticalItemsPerPage);
    if (this.criticalTotalPages === 0) this.criticalTotalPages = 1;
    const startIndex = (this.criticalCurrentPage - 1) * this.criticalItemsPerPage;
    const endIndex = startIndex + this.criticalItemsPerPage;
    this.paginatedCriticalStock = this.criticalStockItems.slice(startIndex, endIndex);
  }

  goToCriticalPage(page: number) {
    if (page >= 1 && page <= this.criticalTotalPages) {
      this.criticalCurrentPage = page;
      this.updateCriticalPagination();
    }
  }
}