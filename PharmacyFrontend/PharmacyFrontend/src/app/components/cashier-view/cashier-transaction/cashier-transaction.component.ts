// cashier-transaction.component.ts
import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';
import { PostService } from '../../../services/PostService/post.service';
import { Product } from '../../../models/product-models/product';
import { CreateSalesItemDto } from '../../../models/sales-transaction-models/create-sales-item';
import { FormsModule } from '@angular/forms';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import {
  CommonModule,
  CurrencyPipe,
  DatePipe,
  NgFor,
  NgIf,
} from '@angular/common';
import { ViewSalesComponent } from '../view-sales/view-sales.component';
import { SalesTransaction } from '../../../models/sales-transaction-models/sales-transaction';
import { DailySalesSummary } from '../../../models/sales-transaction-models/daily-sales-summary';
import { ProductSalesSummary } from '../../../models/sales-transaction-models/product-sales-summary';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-cashier-transaction',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    ViewSalesComponent,
    CommonModule,
    DatePipe,
    CurrencyPipe,
  ],
  templateUrl: './cashier-transaction.component.html',
  styleUrl: './cashier-transaction.component.scss',
})
export class CashierTransactionComponent implements OnInit {
  loggedInEmployee: Employee = new Employee();
  products: Product[] = [];
  selectedItems: CreateSalesItemDto[] = [];
  viewSales?: SalesTransaction | null;
  transactions: SalesTransaction[] = [];
  dailySalesSummary: DailySalesSummary = new DailySalesSummary();
  productSalesSummary: ProductSalesSummary[] = [];
  enrichedProducts: any[] = [];
  startDate: string = '';
  endDate: string = '';
  selectedPaymentMethod: string = '';

  // Pagination for products table
  paginatedProducts: any[] = [];
  currentProductPage: number = 1;
  itemsPerProductPage: number = 7;
  totalProductPages: number = 0;

  // Pagination for sales table
  paginatedTransactions: SalesTransaction[] = [];
  currentSalesPage: number = 1;
  itemsPerSalesPage: number = 6;
  totalSalesPages: number = 0;

  // Breakdown modal
  showBreakdownModal: boolean = false;

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    // Set default dates to today
    const today = new Date();
    this.startDate = this.formatDate(today);
    this.endDate = this.formatDate(today);

    this.getLoggedInEmployee();
    this.getSummarySalesForToday();
    this.getProductSalesSummary();
  }

  // Helper method to format date as YYYY-MM-DD
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  getAllSalesTransactions() {
    this.getService.getSalesTransactionByCashier(this.loggedInEmployee.id).subscribe({
      next: (res) => {
        this.transactions = res;
        this.updateSalesPagination();
        this.filterTransactionsLocally();
      },
      error: (err) => {
        console.log(`${err.error}`);
        console.log(`${this.loggedInEmployee.id}`);
      },
    });
  }

  getProductSalesSummary() {
    if (!this.startDate || !this.endDate) {
      console.log('Dates not set, skipping product sales summary');
      return;
    }

    this.getService
      .getProductSalesSummaryAsync(this.startDate, this.endDate)
      .subscribe({
        next: (res) => {
          this.productSalesSummary = res;
          console.log('Product Sales Summary received:', res);
          // Backend now returns the breakdown directly, so just use it
          this.enrichedProducts = res;
          this.updateProductPagination();
        },
        error: (err) => {
          console.log(`Product sales summary error: ${err.error}`);
        },
      });
  }

  getUnitTypeClass(unitMeasurement: string): string {
    switch (unitMeasurement) {
      case 'Multi-Level':
        return 'unit-multi';
      case 'Dual-Level':
        return 'unit-dual';
      case 'Single-Level':
        return 'unit-single';
      default:
        return 'unit-default';
    }
  }

  getDailySalesSummary() {
    this.getService.getDailySalesSummaryAsync().subscribe({
      next: (res) => {
        this.dailySalesSummary = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  getDailySalesByFilter() {
    // Validate that both dates are set
    if (!this.startDate || !this.endDate) {
      console.warn('Both start and end dates are required');
      return;
    }

    console.log('Filtering with:', {
      startDate: this.startDate,
      endDate: this.endDate,
      paymentMethod: this.selectedPaymentMethod,
    });

    // Update summary cards
    this.getService
      .getDailySalesSummaryByFilter(
        this.startDate,
        this.endDate,
        0,
        this.selectedPaymentMethod
      )
      .subscribe({
        next: (res) => {
          console.log('Summary response:', res);
          this.dailySalesSummary = res;
        },
        error: (err) => {
          console.error('Summary error:', err);
        },
      });

    // Update product sales table
    this.getProductSalesSummary();

    // Filter transactions locally (or call backend if you have an endpoint)
    this.filterTransactionsLocally();
  }

  // Filter transactions locally based on date and payment method
  filterTransactionsLocally() {
    this.getService.getAllSalesTransactionsAsync().subscribe({
      next: (allTransactions) => {
        let filtered = allTransactions;

        // Filter by date range
        if (this.startDate && this.endDate) {
          filtered = filtered.filter((transaction) => {
            const transactionDateStr = new Date(transaction.transactionDate)
              .toISOString()
              .split('T')[0];
            return transactionDateStr >= this.startDate && transactionDateStr <= this.endDate;
          });
        }

        // Filter by payment method
        if (
          this.selectedPaymentMethod &&
          this.selectedPaymentMethod !== '' &&
          this.selectedPaymentMethod !== 'all'
        ) {
          filtered = filtered.filter(
            (transaction) =>
              transaction.paymentMethod.toLowerCase() ===
              this.selectedPaymentMethod.toLowerCase()
          );
        }

        this.transactions = filtered;
        this.currentSalesPage = 1; // Reset to first page
        this.updateSalesPagination();
      },
      error: (err) => {
        console.error('Error filtering transactions:', err);
      },
    });
  }

  getSummarySalesForToday() {
    this.getService.getSummarySalesForTodayAsync().subscribe({
      next: (res) => {
        this.dailySalesSummary = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  updateProductPagination() {
    // Use enrichedProducts if available, otherwise fall back to productSalesSummary
    const dataSource = this.enrichedProducts.length > 0 
      ? this.enrichedProducts 
      : this.productSalesSummary;
      
    this.totalProductPages = Math.ceil(
      dataSource.length / this.itemsPerProductPage
    );
    if (this.totalProductPages === 0) this.totalProductPages = 1;
    
    const startIndex = (this.currentProductPage - 1) * this.itemsPerProductPage;
    const endIndex = startIndex + this.itemsPerProductPage;
    this.paginatedProducts = dataSource.slice(startIndex, endIndex);
  }

  goToProductPage(page: number) {
    if (page >= 1 && page <= this.totalProductPages) {
      this.currentProductPage = page;
      this.updateProductPagination();
    }
  }

  updateSalesPagination() {
    this.totalSalesPages = Math.ceil(
      this.transactions.length / this.itemsPerSalesPage
    );
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

  openViewSalesForm(view: SalesTransaction) {
    this.viewSales = view;
  }

  closeViewSalesForm() {
    this.viewSales = null;
  }

  getLoggedInEmployee() {
    const token = localStorage.getItem(LogInToken);
    if (!token) {
      console.log(`Token does not exist.`);
      return;
    }

    const employee: any = jwtDecode(token);
    const employeeId = parseInt(employee.EmployeeId);

    this.getService.GetEmployeeById(employeeId).subscribe({
      next: (employee) => {
        this.loggedInEmployee = employee;
        this.getAllSalesTransactions();
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  // Breakdown modal methods
  showItemsSoldBreakdown(): void {
    this.showBreakdownModal = true;
  }

  closeBreakdownModal(): void {
    this.showBreakdownModal = false;
  }

  getTotalItemsSold(): number {
    if (!this.enrichedProducts || this.enrichedProducts.length === 0) {
      return 0;
    }

    return this.enrichedProducts.reduce((total, product) => {
      const boxes = product.boxSold || 0;
      const sheets = product.sheetSold || 0;
      const pieces = product.pieceSold || 0;
      
      // Each unit (box, sheet, or piece) counts as 1 item
      return total + boxes + sheets + pieces;
    }, 0);
  }
}