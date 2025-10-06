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
  // products now may contain optional pricing fields used when deciding unit prices
  products: (Product & {
    selected?: boolean;
    quantity?: number;
    tempUnitType?: 'Box' | 'Sheet' | 'Piece';
    sheetPrice?: number;
    piecePrice?: number;
    sheetsPerBox?: number;
    piecesPerSheet?: number;
  })[] = [];

  // Selected products (cart) structure aligned to backend item fields
  selectedProducts: {
    productID: number;
    productName: string;
    unitType: 'Box' | 'Sheet' | 'Piece';
    quantity: number;
    unitPrice: number;
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
      next: (res) => {
        this.categories = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
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

  // Helper to display a product's unit price depending on unit type selected in product list
  getProductUnitPriceDisplay(product: any): number {
    const unit = product.tempUnitType || 'Box';
    return this.resolveUnitPriceFromProduct(product, unit);
  }

  // Resolve unit price given product object and unit type, with fallbacks
  resolveUnitPriceFromProduct(
    product: any,
    unit: 'Box' | 'Sheet' | 'Piece'
  ): number {
    // use named fields if available, otherwise attempt reasonable fallbacks
    const boxPrice = product.boxPrice ?? 0;
    const sheetPrice =
      product.sheetPrice ??
      (product.sheetsPerBox ? boxPrice / product.sheetsPerBox : undefined);
    const piecePrice =
      product.piecePrice ??
      (product.piecesPerSheet
        ? (sheetPrice ?? boxPrice / (product.sheetsPerBox ?? 1)) /
          product.piecesPerSheet
        : undefined);

    switch (unit) {
      case 'Box':
        return boxPrice;
      case 'Sheet':
        return sheetPrice ?? boxPrice;
      case 'Piece':
        return piecePrice ?? sheetPrice ?? boxPrice;
      default:
        return boxPrice;
    }
  }

  addSelectedProducts() {
    const toAdd = this.products.filter((p) => p.selected);
    toAdd.forEach((p) => {
      const unitType = p.tempUnitType || 'Box';
      // find existing cart item
      const already = this.selectedProducts.find(
        (sp) => sp.productID === p.productID && sp.unitType === unitType
      );

      const price = this.resolveUnitPriceFromProduct(p, unitType);
      const qtyToAdd = p.quantity && p.quantity > 0 ? p.quantity : 1;

      if (!already) {
        this.selectedProducts.push({
          productID: p.productID,
          productName: p.productName,
          unitType: unitType,
          quantity: qtyToAdd,
          unitPrice: price,
        });
      } else {
        already.quantity += qtyToAdd;
      }
      // clear selection and temp fields
      p.selected = false;
      p.quantity = 0;
      p.tempUnitType = undefined;
    });
  }

  // Called when cart item changes (qty/unitType/price)
  onCartItemChange(item: any) {
    if (!item.quantity || item.quantity < 1) item.quantity = 1;
    if (!item.unitPrice || item.unitPrice < 0) item.unitPrice = 0;

    // If unit type changed and unitPrice is zero, fill it based on original product prices
    if ((!item.unitPrice || item.unitPrice === 0) && item.unitType) {
      const product = this.products.find((p) => p.productID === item.productID);
      if (product) {
        item.unitPrice = this.resolveUnitPriceFromProduct(
          product,
          item.unitType
        );
      }
    }
  }

  clearSelection() {
    this.selectedProducts = [];
  }

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
    const total = this.getTotalPrice();
    return +(total * 1.12);
  }

  openViewSettleForm() {
    this.viewSettle = new Product();
  }

  closeViewSettle() {
    this.viewSettle = null;
  }

  // When paymentConfirmed is emitted from the settle modal, 'event' should contain payment details
  // e.g. { paymentMethod: 'Cash', paidAmount: 1000, discountId: 1, discountAmount: 50, vatId: 1, vatAmount: 120, other fields... }
  handlePayment(event: any) {
    console.log('Payment confirmed (raw event from modal):', event);

    // Ensure we have a logged-in employee id
    const employeeId = event.employeeID;
    if (!employeeId || employeeId <= 0) {
      console.error('No valid employeeID from payment event');
      alert('No logged-in employee found. Please login again.');
      return;
    }

    // Build SalesTransaction payload expected by backend
    const invoiceNumber = this.generateInvoiceNumber();

    // Map cart to backend sales items
    const items = this.selectedProducts.map((sp) => {
      const subtotal = +(sp.quantity * sp.unitPrice);
      return {
        ProductId: sp.productID,
        UnitType: sp.unitType, // must be "Box"|"Sheet"|"Piece"
        Quantity: sp.quantity,
        UnitPrice: sp.unitPrice,
        Subtotal: subtotal,
      };
    });

    const totalAmount = items.reduce(
      (acc: number, it: any) => acc + it.Subtotal,
      0
    );
    const discountAmount = event?.discountAmount ?? 0;
    const vatAmount = event?.vatAmount ?? 0;
    const discountId = event?.discountId ?? null;
    const vatId = event?.vatId ?? null;

    const salesTransactionPayload = {
      invoiceNumber: invoiceNumber,
      employeeId: employeeId, // lowercase “Id” to match backend property
      discountId: event.discountId ?? null,
      discountAmount: event.discountAmount ?? 0,
      vatId: event.vatId ?? null, // lowercase “vatId”
      vatAmount: event.vatAmount ?? 0,
      paymentMethod: event.paymentMethod, // add this since event includes it
      amountPaid: event.amountPaid,
      changeAmount: event.changeAmount,
      items: event.items.map((i: any) => ({
        productId: i.productId ?? i.productID, // handle both naming styles
        unitType: i.unitType ?? i.UnitType,
        quantity: i.quantity,
        unitPrice: i.unitPrice ?? i.UnitPrice,
        subtotal: i.subtotal ?? i.quantity * (i.unitPrice ?? i.UnitPrice),
      })),
      totalAmount: totalAmount,
      netAmount:
        totalAmount - (event.discountAmount ?? 0) + (event.vatAmount ?? 0),
    };

    console.log(
      'Constructed SalesTransaction payload:',
      salesTransactionPayload
    );

    // Send to backend
    this.postService.createSalesTransactionAsync(event).subscribe({
      next: (res) => {
        alert('Transaction successful.');
        // optionally set print receipt payload, etc.
        this.openPrintReceiptForm();
      },
      error: (err) => {
        console.error('Error creating transaction:', err);
        alert('Failed to create transaction. See console for details.');
      },
    });

    // clear cart and close modal
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
      next: (res) => {
        // initialize tempUnitType default per product
        this.products = res.map((p: any) => ({
          ...p,
          tempUnitType: 'Box',
          // optionally map sheet/piece price and packaging info if present in product payload
          sheetPrice: p.sheetPrice ?? p.sheetPrice,
          piecePrice: p.piecePrice ?? p.piecePrice,
          sheetsPerBox: p.sheetsPerBox ?? p.sheetsPerBox,
          piecesPerSheet: p.piecesPerSheet ?? p.piecesPerSheet,
        }));
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  // Utility: generate an invoice number (simple timestamp + random) — you can replace with server-provided numbering if you have an API
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
