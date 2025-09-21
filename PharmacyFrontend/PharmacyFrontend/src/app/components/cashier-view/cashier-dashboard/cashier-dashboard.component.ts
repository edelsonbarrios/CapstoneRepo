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

@Component({
  selector: 'app-cashier-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ViewSettleComponent, PrintReceiptComponent],
  templateUrl: './cashier-dashboard.component.html',
  styleUrls: ['./cashier-dashboard.component.scss']
})
export class CashierDashboardComponent implements OnInit {
  viewSettle?: Product | null;
  printReceipt?: Product | null;
  loggedInEmployee: Employee = new Employee();

  // UI state
  products: (Product & { selected?: boolean; quantity?: number })[] = [];
  selectedProducts: { productID: number; productName: string; price: number; quantity: number }[] = [];
  searchTerm: string = '';
  categories: string[] = ['Medicine', 'Supplements', 'Others'];
  selectedCategory: string = '';

  constructor(private getService: GetService, private postService: PostService) {}

  ngOnInit(): void {
    this.getLoggedInUser();

    // Temporary sample products
    const p1 = new Product(); p1.productID = 1; p1.productName = 'Paracetamol (500mg)'; p1.price = 50; p1.selected = false; p1.totalStock = 100;
    const p2 = new Product(); p2.productID = 2; p2.productName = 'Ibuprofen (200mg)'; p2.price = 70; p2.selected = false; p2.totalStock = 50;
    const p3 = new Product(); p3.productID = 3; p3.productName = 'Vitamin C (500mg)'; p3.price = 30; p3.selected = false; p3.totalStock = 200;
    const p4 = new Product(); p4.productID = 4; p4.productName = 'Cetirizine (10mg)'; p4.price = 40; p4.selected = false; p4.totalStock = 120;
    const p5 = new Product(); p5.productID = 5; p5.productName = 'Alcohol (70% sol.)'; p5.price = 100; p5.selected = false; p5.totalStock = 60;

    this.products = [p1, p2, p3, p4, p5];
  }

  get filteredProducts() {
    const term = this.searchTerm?.toLowerCase()?.trim();
    return this.products.filter(p => {
      if (this.selectedCategory && p.categoryName !== this.selectedCategory) return false;
      if (!term) return true;
      return (p.productName || '').toLowerCase().includes(term) ||
             (p.productID + '').includes(term) ||
             (p.brandName || '').toLowerCase().includes(term);
    });
  }

  addSelectedProducts() {
    const toAdd = this.products.filter(p => p.selected);
    toAdd.forEach(p => {
      const already = this.selectedProducts.find(sp => sp.productID === p.productID);
      if (!already) {
        this.selectedProducts.push({
          productID: p.productID,
          productName: p.productName,
          price: p.price,
          quantity: p.quantity ? p.quantity : 1
        });
      } else {
        already.quantity += p.quantity ? p.quantity : 1;
      }
      p.selected = false;
    });
  }

  onItemChange() {
    this.selectedProducts.forEach(sp => {
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
    return this.selectedProducts.reduce((acc, p) => acc + (p.price * (p.quantity || 0)), 0);
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

    // Example: send to backend
    // this.postService.SaveTransaction(event).subscribe(...);

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
        error: () => console.log(`Logged-in user does not exist.`)
      });
    } catch (err) {
      console.warn('Failed to decode token', err);
    }
  }
}
