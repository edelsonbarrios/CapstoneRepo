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

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    this.getAllSalesTransactions();
    this.getDailySalesSummary();
  }

  getAllSalesTransactions() {
    this.getService.getAllSalesTransactionsAsync().subscribe({
      next: res => {
        this.transactions = res;
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

  openViewSalesForm(){
    this.viewSales = new Product();
  }

}
