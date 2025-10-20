// cashier-transaction.component.ts
import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';
import { PostService } from '../../../services/PostService/post.service';
import { Product } from '../../../models/product-models/product';
import { CreateSalesItemDto } from '../../../models/sales-transaction-models/create-sales-item';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { ViewSalesComponent } from '../view-sales/view-sales.component';
import { SalesTransaction } from '../../../models/sales-transaction-models/sales-transaction';
import { DailySalesSummary } from '../../../models/sales-transaction-models/daily-sales-summary';

@Component({
  selector: 'app-cashier-transaction',
  standalone: true,
  imports: [FormsModule, NgIf, ViewSalesComponent, CommonModule, DatePipe],
  templateUrl: './cashier-transaction.component.html',
  styleUrl: './cashier-transaction.component.scss',
})
export class CashierTransactionComponent implements OnInit {
  loggedInEmployee: Employee = new Employee();
  products : Product[] = [];
  selectedItems : CreateSalesItemDto[] = [];
  viewSales ?: Product | null;
  transactions: SalesTransaction[] = [];
  dailySalesSummary : DailySalesSummary = new DailySalesSummary();

  // Pagination for products table
  paginatedProducts: any[] = [];
  currentProductPage: number = 1;
  itemsPerProductPage: number = 5;
  totalProductPages: number = 0;

  // Pagination for sales table
  paginatedTransactions: SalesTransaction[] = [];
  currentSalesPage: number = 1;
  itemsPerSalesPage: number = 5;
  totalSalesPages: number = 0;

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    this.getAllSalesTransactions();
    this.getSummarySalesForToday();
    this.updateProductPagination();
  }

  getAllSalesTransactions() {
    this.getService.getAllSalesTransactionsAsync().subscribe({
      next: res => {
        this.transactions = res;
        this.updateSalesPagination();
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getDailySalesSummary() {
    this.getService.getDailySalesSummaryAsync().subscribe({
      next: res => {
        this.dailySalesSummary = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  getSummarySalesForToday() {
    this.getService.getSummarySalesForTodayAsync().subscribe({
      next: res => {
        this.dailySalesSummary = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  updateProductPagination() {
    // This will work with dynamic product data when you add it later
    this.totalProductPages = Math.ceil(this.products.length / this.itemsPerProductPage);
    if (this.totalProductPages === 0) this.totalProductPages = 1;
    const startIndex = (this.currentProductPage - 1) * this.itemsPerProductPage;
    const endIndex = startIndex + this.itemsPerProductPage;
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  goToProductPage(page: number) {
    if (page >= 1 && page <= this.totalProductPages) {
      this.currentProductPage = page;
      this.updateProductPagination();
    }
  }

  updateSalesPagination() {
    this.totalSalesPages = Math.ceil(this.transactions.length / this.itemsPerSalesPage);
    if (this.totalSalesPages === 0) this.totalSalesPages = 1;
    const startIndex = (this.currentSalesPage - 1) * this.itemsPerSalesPage;
    const endIndex = startIndex + this.itemsPerSalesPage;
    this.paginatedTransactions = this.transactions.slice(startIndex, endIndex);
  }

  goToSalesPage(page: number) {
    if (page >= 1 && page <= this.totalSalesPages) {
      this.currentSalesPage = page;
      this.updateSalesPagination();
    }
  }

  openViewSalesForm(){
    this.viewSales = new Product();
  }
}