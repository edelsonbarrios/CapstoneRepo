import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';

import { Product } from '../../../models/product-models/product';
import { Employee } from '../../../models/employee-models/employee';
import { Category } from '../../../models/category-models/category';
import { LogInToken } from '../../shared/token';
import { GetService } from '../../../services/GetService/get.service';
import { PostService } from '../../../services/PostService/post.service';

import { ViewSettleComponent } from '../view-settle/view-settle.component';
import { PrintReceiptComponent } from '../print-receipt/print-receipt.component';

@Component({
  selector: 'app-cashier-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ViewSettleComponent, PrintReceiptComponent],
  templateUrl: './cashier-dashboard.component.html',
  styleUrls: ['./cashier-dashboard.component.scss'],
})
export class CashierDashboardComponent implements OnInit {
  // --------------------------
  // UI States
  // --------------------------
  viewSettle?: Product | null;
  printReceipt?: Product | null;
  loggedInEmployee: Employee = new Employee();

  // Products with temporary UI states
  products: (Product & {
    selected?: boolean;
    quantity?: number;
    batchNumber?: string;
    tempUnitType?: 'Box' | 'Sheet' | 'Piece';
    sheetPrice?: number;
    piecePrice?: number;
    sheetsPerBox?: number;
    piecesPerSheet?: number;
  })[] = [];

  // Cart items (aligned with backend)
  selectedProducts: {
    productID: number;
    productName: string;
    unitType: 'Box' | 'Sheet' | 'Piece';
    quantity: number;
    unitPrice: number;
  }[] = [];

  // Filters
  searchTerm: string = '';
  categories: Category[] = [];
  selectedCategory: string = '';

  constructor(private getService: GetService, private postService: PostService) {}

  // --------------------------
  // Lifecycle
  // --------------------------
  ngOnInit(): void {
    this.getLoggedInUser();
    this.getAllProducts();
    this.getAllCategories();
  }

  // --------------------------
  // Data Fetching
  // --------------------------
  getAllCategories() {
    this.getService.getAllCategoriesAsync().subscribe({
      next: (res) => (this.categories = res),
      error: (err) => console.error(err.error),
    });
  }

  getAllProducts() {
    this.getService.getAllAvailableInventoriesAsync().subscribe({
      next: (res) => {
        this.products = res.map((p: any) => ({
          ...p,
          tempUnitType: 'Box',
          sheetPrice: p.sheetPrice,
          piecePrice: p.piecePrice,
          sheetsPerBox: p.sheetsPerBox,
          piecesPerSheet: p.piecesPerSheet,
        }));
      },
      error: (err) => console.error(err.error),
    });
  }

  getLoggedInUser() {
    const token = localStorage.getItem(LogInToken);
    if (!token) return;

    try {
      const decoded: any = jwtDecode(token);
      const employeeId = parseInt(decoded.EmployeeId);
      if (!employeeId) return console.warn('Employee ID missing from token.');

      this.getService.GetEmployeeById(employeeId).subscribe({
        next: (res) => (this.loggedInEmployee = res),
        error: () => console.warn('Failed to fetch logged-in user.'),
      });
    } catch (err) {
      console.warn('Failed to decode token:', err);
    }
  }

  // --------------------------
  // Filtering Logic
  // --------------------------
  get filteredProducts() {
    const term = this.searchTerm?.toLowerCase()?.trim();
    return this.products.filter((p) => {
      if (this.selectedCategory && p.categoryName !== this.selectedCategory) return false;
      if (!term) return true;
      return (
        (p.productName || '').toLowerCase().includes(term) ||
        (p.productID + '').includes(term) ||
        (p.brandName || '').toLowerCase().includes(term)
      );
    });
  }

  // --------------------------
  // Pricing Helpers
  // --------------------------
  getProductUnitPriceDisplay(product: any): number {
    const unit = product.tempUnitType || 'Box';
    return this.resolveUnitPriceFromProduct(product, unit);
  }

  resolveUnitPriceFromProduct(product: any, unit: 'Box' | 'Sheet' | 'Piece'): number {
    const boxPrice = product.boxPrice ?? 0;
    const sheetPrice =
      product.sheetPrice ??
      (product.sheetsPerBox ? boxPrice / product.sheetsPerBox : undefined);
    const piecePrice =
      product.piecePrice ??
      (product.piecesPerSheet
        ? (sheetPrice ?? boxPrice / (product.sheetsPerBox ?? 1)) / product.piecesPerSheet
        : undefined);

    switch (unit) {
      case 'Box': return boxPrice;
      case 'Sheet': return sheetPrice ?? boxPrice;
      case 'Piece': return piecePrice ?? sheetPrice ?? boxPrice;
      default: return boxPrice;
    }
  }

  // --------------------------
  // Cart Management
  // --------------------------
  addSelectedProducts() {
    const toAdd = this.products.filter((p) => p.selected);

    toAdd.forEach((p) => {
      const unitType = p.tempUnitType || 'Box';
      const price = this.resolveUnitPriceFromProduct(p, unitType);
      const qtyToAdd = p.quantity && p.quantity > 0 ? p.quantity : 1;

      const existing = this.selectedProducts.find(
        (sp) => sp.productID === p.productID && sp.unitType === unitType
      );

      if (existing) {
        existing.quantity += qtyToAdd;
      } else {
        this.selectedProducts.push({
          productID: p.productID,
          productName: p.productName,
          unitType,
          quantity: qtyToAdd,
          unitPrice: price,
        });
      }

      // Reset UI states
      p.selected = false;
      p.quantity = 0;
      p.tempUnitType = undefined;
    });
  }

  onCartItemChange(item: any) {
    if (!item.quantity || item.quantity < 1) item.quantity = 1;
    if (!item.unitPrice || item.unitPrice < 0) item.unitPrice = 0;

    const product = this.products.find((p) => p.productID === item.productID);
    if (product && (!item.unitPrice || item.unitPrice === 0)) {
      item.unitPrice = this.resolveUnitPriceFromProduct(product, item.unitType);
    }
  }

  clearSelection() {
    this.selectedProducts = [];
  }

  // --------------------------
  // Computation Helpers
  // --------------------------
  getTotalItems(): number {
    return this.selectedProducts.reduce((acc, p) => acc + (p.quantity || 0), 0);
  }

  getTotalPrice(): number {
    return this.selectedProducts.reduce(
      (acc, p) => acc + p.unitPrice * (p.quantity || 0),
      0
    );
  }

  getGrossPrice(): number {
    return +(this.getTotalPrice() * 1.12);
  }

  // --------------------------
  // Payment & Transaction
  // --------------------------
  handlePayment(event: any) {
    console.log('Payment confirmed:', event);

    const employeeId = event.employeeID;
    if (!employeeId || employeeId <= 0) {
      console.error('No valid employeeID from payment event');
      alert('No logged-in employee found. Please login again.');
      return;
    }

    const invoiceNumber = this.generateInvoiceNumber();

    const items = this.selectedProducts.map((sp) => ({
      ProductId: sp.productID,
      UnitType: sp.unitType,
      Quantity: sp.quantity,
      UnitPrice: sp.unitPrice,
      Subtotal: +(sp.quantity * sp.unitPrice),
    }));

    const totalAmount = items.reduce((acc, it) => acc + it.Subtotal, 0);
    const discountAmount = event?.discountAmount ?? 0;
    const vatAmount = event?.vatAmount ?? 0;

    const salesTransactionPayload = {
      invoiceNumber,
      employeeId,
      discountId: event.discountId ?? null,
      discountAmount,
      vatId: event.vatId ?? null,
      vatAmount,
      paymentMethod: event.paymentMethod,
      amountPaid: event.amountPaid,
      changeAmount: event.changeAmount,
      items: event.items.map((i: any) => ({
        productId: i.productId ?? i.productID,
        unitType: i.unitType ?? i.UnitType,
        quantity: i.quantity,
        unitPrice: i.unitPrice ?? i.UnitPrice,
        subtotal: i.subtotal ?? i.quantity * (i.unitPrice ?? i.UnitPrice),
      })),
      totalAmount,
      netAmount: totalAmount - discountAmount + vatAmount,
    };

    console.log('Constructed payload:', salesTransactionPayload);

    this.postService.createSalesTransactionAsync(event).subscribe({
      next: () => {
        alert('Transaction successful.');
        this.openPrintReceiptForm();
      },
      error: (err) => {
        console.error('Error creating transaction:', err);
        alert('Failed to create transaction.');
      },
    });

    this.clearSelection();
    this.closeViewSettle();
  }

  // --------------------------
  // Modal Controls
  // --------------------------
  openViewSettleForm() {
    this.viewSettle = new Product();
  }

  closeViewSettle() {
    this.viewSettle = null;
  }

  openPrintReceiptForm() {
    this.printReceipt = new Product();
  }

  // --------------------------
  // Utility
  // --------------------------
  private generateInvoiceNumber(): string {
    const now = new Date();
    const ts =
      now.getFullYear().toString().slice(2) +
      ('0' + (now.getMonth() + 1)).slice(-2) +
      ('0' + now.getDate()).slice(-2) +
      ('0' + now.getHours()).slice(-2) +
      ('0' + now.getMinutes()).slice(-2) +
      ('0' + now.getSeconds()).slice(-2);
    const random = Math.floor(Math.random() * 9000) + 1000;
    return `INV-${ts}-${random}`;
  }
}
