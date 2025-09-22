import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Product } from '../../../models/product-models/product';
import { Employee } from '../../../models/employee-models/employee';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';

import { GetService } from '../../../services/GetService/get.service';
import { PostService } from '../../../services/PostService/post.service';
import { ViewSettleComponent } from '../view-settle/view-settle.component';
import { PrintReceiptComponent } from '../print-receipt/print-receipt.component';
import { Category } from '../../../models/category-models/category';

@Component({
  selector: 'app-cashier-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ViewSettleComponent,
    PrintReceiptComponent,
  ],
  templateUrl: './cashier-dashboard.component.html',
  styleUrls: ['./cashier-dashboard.component.scss'],
})
export class CashierDashboardComponent implements OnInit {
  viewSettle?: Product | null;
  printReceipt?: Product | null;
  loggedInEmployee: Employee = new Employee();

  // UI state
  products: (Product & { selected?: boolean; quantity?: number })[] = [];
  selectedProducts: {
    productID: number;
    productName: string;
    price: number;
    quantity: number;
  }[] = [];
  searchTerm: string = '';
  categories: Category[] = [];
  selectedCategory: string = '';

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {
    this.getLoggedInUser();
    this.getAllProducts();
    this.getAllCategories();
  }

  getAllCategories() {
    this.getService.getAllCategoriesAsync().subscribe({
      next: res => {
        this.categories = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }

  get filteredProducts() {
    const term = this.searchTerm?.toLowerCase()?.trim();
    return this.products.filter((p) => {
      if (this.selectedCategory && p.categoryName !== this.selectedCategory)
        return false;
      if (!term) return true;
      return (
        (p.productName || '').toLowerCase().includes(term) ||
        (p.productID + '').includes(term) ||
        (p.brandName || '').toLowerCase().includes(term)
      );
    });
  }

  addSelectedProducts() {
    const toAdd = this.products.filter((p) => p.selected);
    toAdd.forEach((p) => {
      const already = this.selectedProducts.find(
        (sp) => sp.productID === p.productID
      );
      if (!already) {
        this.selectedProducts.push({
          productID: p.productID,
          productName: p.productName,
          price: p.price,
          quantity: p.quantity ? p.quantity : 1,
        });
      } else {
        already.quantity += p.quantity ? p.quantity : 1;
      }
      p.selected = false;
    });
  }

  onItemChange() {
    this.selectedProducts.forEach((sp) => {
      if (!sp.quantity || sp.quantity < 1) sp.quantity = 1;
      if (!sp.price || sp.price < 0) sp.price = 0;
    });
  }

  clearSelection() {
    this.selectedProducts = [];
  }

  getTotalItems(): number {
    return this.selectedProducts.reduce((acc, p) => acc + (p.quantity || 0), 0);
  }

  getTotalPrice(): number {
    return this.selectedProducts.reduce(
      (acc, p) => acc + p.price * (p.quantity || 0),
      0
    );
  }

  getGrossPrice(): number {
    const total = this.getTotalPrice();
    return +(total * 1.12);
  }

  openViewSettleForm() {
    this.viewSettle = new Product();
  }

  closeViewSettle() {
    this.viewSettle = null;
  }

  handlePayment(event: any) {
    console.log('Payment confirmed:', event);

    this.postService.createSalesTransactionAsync(event).subscribe({
      next: (res) => {
        alert('Transaction successful.');
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });

    this.clearSelection();
    this.closeViewSettle();
  }

  openPrintReceiptForm() {
    this.printReceipt = new Product();
  }

  getLoggedInUser() {
    const token = localStorage.getItem(LogInToken);
    if (!token) return;

    try {
      const employee: any = jwtDecode(token);
      const employeeId = parseInt(employee.EmployeeId);
      if (!employeeId) console.log(`Employee Id does not exist.`);

      this.getService.GetEmployeeById(employeeId).subscribe({
        next: (res) => (this.loggedInEmployee = res),
        error: () => console.log(`Logged-in user does not exist.`),
      });
    } catch (err) {
      console.warn('Failed to decode token', err);
    }
  }

  getAllProducts() {
    this.getService.getAllProductsAsync().subscribe({
      next: res => {
        this.products = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
}
