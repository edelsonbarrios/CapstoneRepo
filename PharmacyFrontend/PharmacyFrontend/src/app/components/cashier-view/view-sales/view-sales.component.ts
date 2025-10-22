import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Product } from '../../../models/product-models/product';
import { CurrencyPipe, DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { SalesTransaction } from '../../../models/sales-transaction-models/sales-transaction';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';
import { SalesItem } from '../../../models/sales-transaction-models/sales-item';

// Item interface for the items table
interface SaleItem {
  product: string;
  unit: string;
  qty: number;
  price: number;
  subtotal: number;
}

@Component({
  selector: 'app-view-sales',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe, DecimalPipe],
  templateUrl: './view-sales.component.html',
  styleUrl: './view-sales.component.scss'
})
export class ViewSalesComponent implements OnInit, OnChanges {
  @Input() viewSales: SalesTransaction = new SalesTransaction();
  @Output() closeForm = new EventEmitter<void>();
  paginatedItems: SalesItem[] = [];
  cashier: Employee = new Employee();
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.updatePagination();
    this.getCashier();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items']) {
      this.currentPage = 1;
      this.updatePagination();
    }
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.viewSales.items.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.viewSales.items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  closeViewSales() {
    this.closeForm.emit();
  }

  getCashier() {
    this.getService.GetEmployeeById(this.viewSales.employeeID).subscribe({
      next: res => {
        this.cashier = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}