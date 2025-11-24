import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';

import { Product } from '../../../models/product-models/product';
import { Employee } from '../../../models/employee-models/employee';
import { Category } from '../../../models/category-models/category';
import { Discount } from '../../../models/discount-models/discount';
import { LogInToken } from '../../shared/token';
import { GetService } from '../../../services/GetService/get.service';
import { PostService } from '../../../services/PostService/post.service';

import { ViewSettleComponent } from '../view-settle/view-settle.component';
import { PrintReceiptComponent } from '../print-receipt/print-receipt.component';
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
  pendingTransaction: any = null; // NEW: Store transaction before saving to DB
  newInvoice: string = '';
  timeNow: Date = new Date();
  timer: any;
  errorMessage: string = '';

  discounts: Discount[] = [];

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
    discountId: number;
    inventoryID: number;
  })[] = [];

  selectedProducts: {
    inventoryID: number;
    productID: number;
    productName: string;
    categoryName: string;
    brandName: string;
    formulationName: string;
    batchNumber: string;
    unitType: 'Box' | 'Sheet' | 'Piece';
    quantity: number;
    unitPrice: number;
    unitMeasurement: string;
    quantityError?: string;
    selectedDiscount: Discount | undefined;
    itemDiscountAmount: number;
    discountId: number;
    product?: any;
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
    this.getAllDiscounts();
    this.generateNewInvoiceNumber();

    this.timer = setInterval(() => {
      this.timeNow = new Date();
    }, 1000);
  }

  getAllDiscounts() {
    this.getService.getAllDiscountAsync().subscribe({
      next: (res) => {
        this.discounts = res;
      },
      error: (err) => console.error('Error loading discounts:', err),
    });
  }

  resetComponent(): void {
    this.getService.getAllInventoriesAsync().subscribe((response) => {
      this.products = response.message;
    });
  }

  generateNewInvoiceNumber() {
    this.postService.generateNewInvoiceNumberAsync().subscribe({
      next: (res) => {
        if (typeof res === 'string') {
          this.newInvoice = res;
        } else if (res && (res as any).message) {
          this.newInvoice = (res as any).message;
        } else if (res && (res as any).value) {
          this.newInvoice = (res as any).value;
        } else {
          this.newInvoice = String(res);
        }

        this.newInvoice = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  closeTransactionMessage() {
    this.finishedTransac = false;
    this.receipt = null;
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
          categoryName: p.categoryName || p.product?.categoryName || p.product?.category?.categoryName || '',
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
          discountId: p.discountId || 0,
          inventoryID: p.inventoryID,
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

  // UPDATED: This now calls confirmTransaction before closing
  closeReceiptForm() {
    this.confirmTransaction();
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

  /**
   * Check if a discount is currently valid based on dates and active status
   */
  isDiscountValid(discount: Discount): boolean {
    if (!discount || !discount.isActive) {
      return false;
    }

    const now = new Date();

    // Check start date (if exists)
    if (discount.startDate) {
      const startDate = new Date(discount.startDate);
      if (now < startDate) {
        return false; // Discount hasn't started yet
      }
    }

    // Check end date (if exists)
    if (discount.endDate) {
      const endDate = new Date(discount.endDate);
      if (now > endDate) {
        return false; // Discount has expired
      }
    }

    return true;
  }

  addSelectedProducts() {
    const toAdd = this.products.filter((p) => p.selected);

    toAdd.forEach((p) => {
      const unitType = p.tempUnitType || 'Piece';
      const price = this.resolveUnitPriceFromProduct(p, unitType);
      const qtyToAdd = p.quantity && p.quantity > 0 ? p.quantity : 1;

      const existing = this.selectedProducts.find(
        (sp) => sp.inventoryID === p.inventoryID && sp.unitType === unitType
      );

      if (existing) {
        existing.quantity += qtyToAdd;
        this.calculateAndApplyItemDiscount(existing);
      } else {
        // Find discount if inventory has one AND validate it
        let assignedDiscount: Discount | undefined = undefined;
        if (p.discountId > 0) {
          const discount = this.discounts.find(d => d.id === p.discountId);
          // ADDED: Check if discount is valid
          if (discount && this.isDiscountValid(discount)) {
            assignedDiscount = discount;
          } else if (discount && !this.isDiscountValid(discount)) {
            // Optional: Show warning that discount is expired
            console.warn(`Discount "${discount.discountName}" for ${p.productName} is expired or inactive`);
          }
        }

        const newItem = {
          inventoryID: p.inventoryID,
          productID: p.productID,
          productName: p.productName,
          categoryName: p.categoryName || '',
          brandName: p.product?.brandName || '',
          formulationName: p.product?.formulationName || '',
          unitType,
          quantity: qtyToAdd,
          unitPrice: price,
          unitMeasurement: p.unitMeasurement,
          batchNumber: p.batchNumber || '',
          selectedDiscount: assignedDiscount,
          itemDiscountAmount: 0,
          discountId: p.discountId || 0,
          originalDiscountId: p.discountId || 0,
          product: p.product,
          quantityError: undefined,
        };

        if (newItem.selectedDiscount) {
          this.calculateAndApplyItemDiscount(newItem);
        }

        this.selectedProducts.push(newItem);
      }

      p.selected = false;
      p.quantity = 0;
      p.tempUnitType = 'Piece';
    });
  }

  calculateAndApplyItemDiscount(item: any) {
    if (!item.selectedDiscount) {
      item.itemDiscountAmount = 0;
      return;
    }

    const itemSubtotal = item.quantity * item.unitPrice;

    if (item.selectedDiscount.discountType === 'Percentage') {
      item.itemDiscountAmount = itemSubtotal * (item.selectedDiscount.discountAmount / 100);
    } else if (item.selectedDiscount.discountType === 'Fixed') {
      item.itemDiscountAmount = Math.min(item.selectedDiscount.discountAmount, itemSubtotal);
    } else {
      item.itemDiscountAmount = 0;
    }
  }

  getAvailableDiscountsForItem(item: any): Discount[] {
    // Use the stored original inventory discount
    if (!item.originalDiscountId || item.originalDiscountId === 0) {
      return [];
    }

    const discount = this.discounts.find(d => d.id === item.originalDiscountId);

    if (!discount) return [];

    if (!this.isDiscountValid(discount)) return [];

    this.calculateItemDiscountAmount(item);
    this.calculateAndApplyItemDiscount(item);

    return [discount];
  }

  calculateItemDiscountAmount(item: any): number {
    if (!item.selectedDiscount) return 0;

    const itemSubtotal = item.quantity * item.unitPrice;

    if (item.selectedDiscount.discountType === 'Percentage') {
      return itemSubtotal * (item.selectedDiscount.discountAmount / 100);
    } else if (item.selectedDiscount.discountType === 'Fixed') {
      return Math.min(item.selectedDiscount.discountAmount, itemSubtotal);
    }

    return 0;
  }

  onItemDiscountChange(item: any) {
    const discount = this.discounts.find(d => d.id === item.discountId);

    if (!discount) {
      item.selectedDiscount = undefined;
      item.itemDiscountAmount = 0;
    } else {
      item.selectedDiscount = discount;
    }

    this.calculateAndApplyItemDiscount(item);

    // 🚀 FORCE Angular to update the UI
    this.selectedProducts = [...this.selectedProducts];
  }



  // OPTIONAL: Visual indicator methods for product list
  getDiscountStatusClass(product: any): string {
    if (product.discountId === 0) {
      return '';
    }

    const discount = this.discounts.find(d => d.id === product.discountId);
    if (!discount) {
      return 'discount-missing';
    }

    if (!this.isDiscountValid(discount)) {
      return 'discount-expired';
    }

    return 'discount-active';
  }

  getDiscountStatusText(product: any): string {
    if (product.discountId === 0) {
      return '';
    }

    const discount = this.discounts.find(d => d.id === product.discountId);
    if (!discount) {
      return 'Discount Not Found';
    }

    if (!discount.isActive) {
      return 'Discount Inactive';
    }

    const now = new Date();

    if (discount.startDate && now < new Date(discount.startDate)) {
      return 'Starts ' + new Date(discount.startDate).toLocaleDateString();
    }

    if (discount.endDate && now > new Date(discount.endDate)) {
      return 'Expired';
    }

    return 'Discount Available';
  }

  incrementQuantity(item: any) {
    const max = this.getMaxQuantity(item);
    if (max !== Infinity && item.quantity >= max) {
      this.errorMessage = `Maximum available quantity for ${item.unitType} is ${max}`;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
      return;
    }
    item.quantity++;
    this.calculateAndApplyItemDiscount(item);
  }

  decrementQuantity(item: any) {
    if (item.quantity > 1) {
      item.quantity--;
      this.calculateAndApplyItemDiscount(item);
    }
  }

  onCartItemChange(item: any) {
    if (item.quantity && !Number.isInteger(Number(item.quantity))) {
      item.quantityError = 'Whole number only';
      return;
    } else {
      item.quantityError = '';
    }

    if (!item.quantity || item.quantity < 1) item.quantity = 1;
    if (!item.unitPrice || item.unitPrice < 0) item.unitPrice = 0;

    const product = this.products.find((p) => p.inventoryID === item.inventoryID);
    if (product && (!item.unitPrice || item.unitPrice === 0)) {
      item.unitPrice = this.resolveUnitPriceFromProduct(product, item.unitType);
    }

    const max = this.getMaxQuantity(item);
    if (max !== Infinity && item.quantity > max) {
      item.quantity = max;
      this.errorMessage = `Maximum available quantity for ${item.unitType} is ${max}`;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
    }

    this.calculateAndApplyItemDiscount(item);
  }

  onCartItemUnitTypeChange(item: any) {
    const product = this.products.find((p) => p.inventoryID === item.inventoryID);
    if (!product) return;

    const resolved = this.resolveUnitPriceFromProduct(product, item.unitType);
    item.unitPrice = resolved;

    const max = this.getMaxQuantity(item);
    if (max !== Infinity && item.quantity > max) {
      item.quantity = max;
      this.errorMessage = `Maximum available quantity for ${item.unitType} is ${max}`;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
    }

    this.calculateAndApplyItemDiscount(item);
  }

  removeFromCart(index: number) {
    this.selectedProducts.splice(index, 1);
  }

  clearSelection() {
    this.selectedProducts = [];
  }

  getTotalItems(): number {
    return this.selectedProducts.reduce((acc, p) => acc + (p.quantity || 0), 0);
  }

  getTotalPrice(): number {
    return this.selectedProducts.reduce((acc, p) => {
      const itemSubtotal = p.unitPrice * (p.quantity || 0);
      const itemDiscount = p.itemDiscountAmount || 0;
      return acc + (itemSubtotal - itemDiscount);
    }, 0);
  }

  getGrossPrice(): number {
    return +(this.getTotalPrice() * 1.12);
  }

  getMaxQuantity(item: any): number {
    const product = this.products.find((p) => p.inventoryID === item.inventoryID);
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

  // UPDATED: Store pending transaction and show receipt (NO DATABASE SAVE YET)
  handlePayment(event: any) {
    const employeeId = event.employeeID;
    if (!employeeId || employeeId <= 0) {
      alert('No logged-in employee found. Please login again.');
      return;
    }

    // Store the transaction data but DON'T save to database yet
    this.pendingTransaction = event;
    
    // Show the receipt
    this.receipt = event;
    
    // Close the settle form
    this.closeViewSettle();
  }

  // NEW: This method saves the transaction to the database
  confirmTransaction() {
    if (!this.pendingTransaction) return;

    // NOW save to database
    this.postService.createSalesTransactionAsync(this.pendingTransaction).subscribe({
      next: () => {
        this.getAllProducts();
        this.generateNewInvoiceNumber();
        this.clearSelection();
        this.pendingTransaction = null;
        this.receipt = null;
        this.finishedTransac = true;
      },
      error: (err) => {
        console.error('Full error object:', err);

        let errorMsg = 'An unexpected error occurred';

        if (err.error) {
          if (typeof err.error === 'string') {
            try {
              const parsed = JSON.parse(err.error);
              errorMsg = parsed.Message || parsed.message || err.error;
            } catch {
              errorMsg = err.error;
            }
          } else if (err.error.Message) {
            errorMsg = err.error.Message;
          } else if (err.error.message) {
            errorMsg = err.error.message;
          }
        } else if (err.message) {
          errorMsg = err.message;
        }

        this.errorMessage = errorMsg;
        this.receipt = null;
        this.pendingTransaction = null;
      },
    });
  }

  openViewSettleForm() {
    if (this.selectedProducts.length === 0) {
      this.errorMessage = 'Cannot proceed to settlement. Please add items to cart first.';
      return;
    }
    this.viewSettle = new Product();
  }

  closeViewSettle() {
    this.viewSettle = null;
    this.getAllProducts();
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

  closeErrorMessageForm() {
    this.errorMessage = '';
  }
}