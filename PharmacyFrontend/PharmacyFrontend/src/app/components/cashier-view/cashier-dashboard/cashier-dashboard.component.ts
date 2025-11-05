import { Component, OnDestroy, OnInit } from '@angular/core';
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
import { Subscription } from 'rxjs';
import { ResetService } from '../../../services/ResetService/reset.service';
import { Inventory } from '../../../models/inventory-module-models/inventory';
import { ViewInventoryComponent } from '../view-inventory/view-inventory.component';

@Component({
  selector: 'app-cashier-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ViewSettleComponent,
    PrintReceiptComponent,
    ViewInventoryComponent,
  ],
  templateUrl: './cashier-dashboard.component.html',
  styleUrls: ['./cashier-dashboard.component.scss'],
})
export class CashierDashboardComponent implements OnInit {
  viewSettle?: Product | null;
  printReceipt?: Product | null;
  loggedInEmployee: Employee = new Employee();
  viewInventory: any;
  finishedTransac: boolean = false;
  receipt: any;
  newInvoice: string = '';
  timeNow: Date = new Date();
  timer : any;
  errorMessage: string = '';
  successMessage: string = '';
  showError: boolean = false;
  showSuccess: boolean = false;

  products: (Product & {
    selected?: boolean;
    quantity?: number;
    batchNumber?: string;
    tempUnitType?: 'Piece' | 'Sheet' | 'Box';
    sheetPrice?: number;
    piecePrice?: number;
    sheetsPerBox?: number;
    piecesPerSheet?: number;
    boxQuantity: number;
    sheetQuantity: number;
    pieceQuantity: number;
    unitMeasurement: string;
    product: Product;
    piecePerBox: number;
    reorderLevel: number;
  })[] = [];

  selectedProducts: {
    productID: number;
    productName: string;
    unitType: 'Box' | 'Sheet' | 'Piece';
    quantity: number;
    unitPrice: number;
    unitMeasurement: string;
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
    this.generateNewInvoiceNumber();

    this.timer = setInterval(() => {
      this.timeNow = new Date();
    }, 1000);
  }

  resetComponent(): void {
    this.getService.getAllInventoriesAsync().subscribe((response) => {
      this.products = response.message;
    });
  }

  generateNewInvoiceNumber() {
    this.postService.generateNewInvoiceNumberAsync().subscribe({
      next: (res) => {
        console.log('✅ Raw API Response:', res);
        console.log('✅ Response type:', typeof res);

        // Handle different response formats
        if (typeof res === 'string') {
          this.newInvoice = res;
        } else if (res && (res as any).message) {
          this.newInvoice = (res as any).message;
        } else if (res && (res as any).value) {
          this.newInvoice = (res as any).value;
        } else {
          this.newInvoice = String(res);
        }

        console.log('✅ Final newInvoice:', this.newInvoice);
        this.newInvoice = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  closeTransactionMessage() {
    this.finishedTransac = false;
    this.openPrintReceiptForm();
  }

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
          tempUnitType: 'Piece',
          sheetPrice: p.sheetPrice,
          piecePrice: p.piecePrice,
          sheetsPerBox: p.sheetsPerBox,
          piecesPerSheet: p.piecesPerSheet,
          boxQuantity: p.boxQuantity,
          sheetQuantity: p.sheetQuantity,
          pieceQuantity: p.pieceQuantity,
          unitMeasurement: p.product?.unitMeasurement,
          product: p.product,
          piecePerBox: p.piecePerBox,
          reorderLevel: p.reorderLevel,
        }));
      },
      error: (err) => console.error(err.error),
    });
  }

  getAvailableUnitMeasurements(product: any): string[] {
    if (!product) return [];
    if (product.unitMeasurement == 'Multi-Level') {
      return ['Piece', 'Sheet', 'Box'];
    } else if (product.unitMeasurement == 'Dual-Level') {
      return ['Piece', 'Box'];
    } else {
      return ['Piece'];
    }
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

  closeReceiptForm() {
    this.receipt = null;
  }

  getProductUnitPriceDisplay(product: any): number {
    const unit = product.tempUnitType || 'Box';
    return this.resolveUnitPriceFromProduct(product, unit);
  }

  resolveUnitPriceFromProduct(
    product: any,
    unit: 'Box' | 'Sheet' | 'Piece'
  ): number {
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
    this.clearMessages(); // Clear any existing messages
    
    const toAdd = this.products.filter((p) => p.selected);

    if (toAdd.length === 0) {
      this.showErrorMessage('Please select at least one product to add.');
      return;
    }

    toAdd.forEach((p) => {
      const unitType = p.tempUnitType || 'Piece';
      const price = this.resolveUnitPriceFromProduct(p, unitType);
      const qtyToAdd = p.quantity && p.quantity > 0 ? p.quantity : 1;

      // Get max available quantity
      const maxQty = this.getMaxQuantityForProduct(p, unitType);
      
      // Check if product with same ID and unit type already exists in cart
      const existing = this.selectedProducts.find(
        (sp) => sp.productID === p.productID && sp.unitType === unitType
      );

      const totalQtyInCart = (existing?.quantity || 0) + qtyToAdd;

      // Validate stock availability
      if (totalQtyInCart > maxQty) {
        this.showErrorMessage(
          `Insufficient stock for '${p.productName}'. ` +
          `Available: ${maxQty} ${unitType}(s), ` +
          `Attempting to add: ${totalQtyInCart} ${unitType}(s)`
        );
        p.selected = false;
        return;
      }

      if (existing) {
        // Add to existing quantity
        existing.quantity += qtyToAdd;
      } else {
        // Add as new item
        this.selectedProducts.push({
          productID: p.productID,
          productName: p.productName,
          unitType,
          quantity: qtyToAdd,
          unitPrice: price,
          unitMeasurement: p.unitMeasurement,
        });
      }

      // Reset product selection state
      p.selected = false;
      p.quantity = 0;
      p.tempUnitType = 'Piece';
    });

    if (this.selectedProducts.length > 0) {
      this.showSuccessMessage('Products added to cart successfully!');
    }
  }

  // Helper to get max quantity for a specific unit type
  getMaxQuantityForProduct(product: any, unitType: string): number {
    switch (unitType) {
      case 'Box':
        return product.boxQuantity || 0;
      case 'Sheet':
        return product.sheetQuantity || 0;
      case 'Piece':
        return product.pieceQuantity || 0;
      default:
        return 0;
    }
  }

  onCartItemChange(item: any) {
    if (!item.quantity || item.quantity < 1) item.quantity = 1;
    if (!item.unitPrice || item.unitPrice < 0) item.unitPrice = 0;

    const product = this.products.find((p) => p.productID === item.productID);
    if (product) {
      if (!item.unitPrice || item.unitPrice === 0) {
        item.unitPrice = this.resolveUnitPriceFromProduct(product, item.unitType);
      }

      // Validate quantity against available stock
      const maxQty = this.getMaxQuantityForProduct(product, item.unitType);
      if (item.quantity > maxQty) {
        this.showErrorMessage(
          `Insufficient stock for '${product.productName}'. ` +
          `Available: ${maxQty} ${item.unitType}(s)`
        );
        item.quantity = maxQty;
      }
    }
  }

  onCartItemUnitTypeChange(item: any) {
    const product = this.products.find((p) => p.productID === item.productID);
    if (!product) return;

    const resolved = this.resolveUnitPriceFromProduct(product, item.unitType);
    item.unitPrice = resolved;

    const max = this.getMaxQuantity(item);
    if (max !== Infinity && item.quantity > max) {
      item.quantity = max;
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
    return +(this.getTotalPrice() * 1.12);
  }

  getMaxQuantity(item: any): number {
    const product = this.products.find((p) => p.productID === item.productID);
    if (!product) return Infinity;

    switch (item.unitType) {
      case 'Box':
        return product.boxQuantity || 0;
      case 'Sheet':
        return product.sheetQuantity || 0;
      case 'Piece':
        return product.pieceQuantity || 0;
      default:
        return Infinity;
    }
  }

  handlePayment(event: any) {
    const employeeId = event.employeeID;
    if (!employeeId || employeeId <= 0) {
      this.showErrorMessage('No logged-in employee found. Please login again.');
      return;
    }

    const invoiceNumber = this.newInvoice;

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

    this.postService.createSalesTransactionAsync(event).subscribe({
      next: (response) => {
        this.showSuccessMessage('Transaction completed successfully!');
        this.receipt = event;
        this.getAllProducts();
        this.generateNewInvoiceNumber();
        this.clearSelection();
        this.closeViewSettle();
        
        // Show success overlay
        this.finishedTransac = true;
      },
      error: (err) => {
        console.error('❌ Transaction Error:', err);
        
        // Extract error message from backend
        let errorMsg = 'An error occurred while processing the transaction.';
        
        if (err.error && err.error.message) {
          errorMsg = err.error.message;
        } else if (err.error && typeof err.error === 'string') {
          errorMsg = err.error;
        } else if (err.message) {
          errorMsg = err.message;
        }
        
        this.showErrorMessage(errorMsg);
      },
    });
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    this.showError = true;
    this.showSuccess = false;
    
    // Auto-hide after 7 seconds
    setTimeout(() => {
      this.clearMessages();
    }, 7000);
    
    // Scroll to top to show the message
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
    this.showSuccess = true;
    this.showError = false;
    
    // Auto-hide after 3 seconds
    setTimeout(() => {
      this.clearMessages();
    }, 3000);
  }

   clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
    this.showError = false;
    this.showSuccess = false;
  }
  
  openViewSettleForm() {
    this.viewSettle = new Product();
  }

  closeViewSettle() {
    this.viewSettle = null;
    this.getAllProducts();
    this.clearSelection();
  }

  openPrintReceiptForm() {
    this.printReceipt = new Product();
  }

  openViewInventoryForm(inv: any) {
    this.viewInventory = inv;
  }
  closeViewInventoryForm() {
    this.viewInventory = null;
  }
}