import { Component, OnInit } from '@angular/core';
import { GetService } from '../../../services/GetService/get.service';
import { Employee } from '../../../models/employee-models/employee';
import { PostService } from '../../../services/PostService/post.service';
import { Product } from '../../../models/product-models/product';
import { CreateSalesItemDto } from '../../../models/sales-transaction-models/create-sales-item';
import { FormsModule } from '@angular/forms';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-cashier-transaction',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './cashier-transaction.component.html',
  styleUrl: './cashier-transaction.component.scss',
})
export class CashierTransactionComponent implements OnInit {
  loggedInEmployee: Employee = new Employee();
  products : Product[] = [];
  selectedItems : CreateSalesItemDto[] = [];

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
  }

}
