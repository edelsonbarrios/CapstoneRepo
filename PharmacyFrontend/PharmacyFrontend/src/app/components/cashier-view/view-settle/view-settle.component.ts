import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartItem } from '../../../models/sales-transaction-models/cart-items';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { Employee } from '../../../models/employee-models/employee';
import { GetService } from '../../../services/GetService/get.service';
import { VAT } from '../../../models/vat-models/vat';
import { Discount } from '../../../models/discount-models/discount';
import { Product } from '../../../models/product-models/product';

@Component({
  selector: 'app-view-settle',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './view-settle.component.html',
  styleUrls: ['./view-settle.component.scss'],
})
export class ViewSettleComponent implements OnInit {
  @Input() cartItems: CartItem[] = [];
  @Input() subtotal: number = 0;
  @Input() newInvoice: string = '';
  @Input() products: any[] = []; // Add this to receive products from parent

  vats: VAT[] = [];
  @Input() discounts: Discount[] = [];
  loggedInEmployee: Employee = new Employee();
  selectedVat: VAT | null = null;
  selectedDiscount: Discount | null = null;
  currentPage: number = 1;
  totalPages: number = 0;
  itemsPerPage: number = 5;
  paginateItems: CartItem[] = [];
  paymentMethods: string[] = ['Cash', 'e-Payment'];
  selectedPaymentMethod: string = 'Cash';
  refNum: string = '';
  refNumErrorMessage: string = '';
  errorMessage: string = '';

  @Output() paymentConfirmed = new EventEmitter<any>();
  @Output() modalClosed = new EventEmitter<void>();

  payment: number = 0;
  change: number = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getLoggedInUser();
    this.getAllDiscounts();
    this.getAllVAT();
    this.updatePagination();
  }

  validateReferenceNumber() {
    this.getService.referenceNumberValidationAsync(this.refNum).subscribe({
      next: (res) => {
        this.refNumErrorMessage = res.message;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.cartItems.length / this.itemsPerPage);
    if (this.totalPages == 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginateItems = this.cartItems.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  getAllDiscounts() {
    this.getService.getAllDiscountAsync().subscribe({
      next: (res) => {
        this.discounts = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  getAllVAT() {
    this.getService.getAllVatAsync().subscribe({
      next: (res) => {
        this.vats = res;
        const defaultVat = res.find((v) => v.vatName === 'Standard');
        this.selectedVat = defaultVat || null;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  // NEW: Get available unit measurements for a product
  getAvailableUnitMeasurements(item: any): string[] {
    if (!item) return [];
    const unitMeasurement =
      item.unitMeasurement || item.product?.unitMeasurement;

    if (unitMeasurement === 'Multi-Level') {
      return ['Piece', 'Sheet', 'Box'];
    } else if (unitMeasurement === 'Dual-Level') {
      return ['Piece', 'Box'];
    } else {
      return ['Piece'];
    }
  }

  // NEW: Get max quantity for validation
  getMaxQuantity(item: any): number {
    const product = this.products.find(
      (p: any) => p.productID === item.productID
    );
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

  // NEW: Resolve unit price from product
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

  // NEW: Handle unit type change
  onCartItemUnitTypeChange(item: any) {
    const product = this.products.find(
      (p: any) => p.productID === item.productID
    );
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

    // Recalculate subtotal
    this.recalculateSubtotal();
    this.onPaymentChange();
  }

  // NEW: Handle quantity change
  onCartItemChange(item: any) {
    if (item.quantity && !Number.isInteger(Number(item.quantity))) {
      item.quantityError = 'Whole number only';
      return;
    } else {
      item.quantityError = '';
    }

    if (!item.quantity || item.quantity < 1) item.quantity = 1;

    const max = this.getMaxQuantity(item);
    if (max !== Infinity && item.quantity > max) {
      item.quantity = max;
      this.errorMessage = `Maximum available quantity for ${item.unitType} is ${max}`;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
    }

    // Recalculate subtotal
    this.recalculateSubtotal();
    this.onPaymentChange();
  }

  // NEW: Increment quantity
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
    this.recalculateSubtotal();
    this.onPaymentChange();
  }

  // NEW: Decrement quantity
  decrementQuantity(item: any) {
    if (item.quantity > 1) {
      item.quantity--;
      this.recalculateSubtotal();
      this.onPaymentChange();
    }
  }

  // NEW: Recalculate subtotal when items change
  recalculateSubtotal() {
    this.subtotal = this.cartItems.reduce(
      (acc, item) => acc + item.unitPrice * (item.quantity || 0),
      0
    );
  }

  calculateDiscountAmount(): number {
    if (!this.selectedDiscount) return 0;

    const subtotalWithVat = this.subtotal + this.calculateVatAmount();
    let discountAmount = 0;

    if (this.selectedDiscount.discountType === 'Percentage') {
      discountAmount =
        this.subtotal * ((this.selectedDiscount.discountAmount || 0) / 100);
    } else if (this.selectedDiscount.discountType === 'Fixed') {
      discountAmount = this.selectedDiscount.discountAmount || 0;
    }

    return Math.min(discountAmount, subtotalWithVat);
  }

  calculateVatAmount(): number {
    return this.subtotal * ((this.selectedVat?.vatRate || 0) / 100);
  }

  calculateTotals(): number {
    const vatAmount = this.calculateVatAmount();
    const discountAmount = this.calculateDiscountAmount();
    const total = this.subtotal + vatAmount - discountAmount;

    return Math.max(total, 0);
  }

  getDiscountDisplayText(): string {
    if (!this.selectedDiscount) return 'No Discount';

    if (this.selectedDiscount.discountType === 'Percentage') {
      return `${this.selectedDiscount.discountName} (${this.selectedDiscount.discountAmount}%)`;
    } else if (this.selectedDiscount.discountType === 'Fixed') {
      return `${this.selectedDiscount.discountName} (₱${
        this.selectedDiscount.discountAmount?.toFixed(2) || '0.00'
      })`;
    }

    return this.selectedDiscount.discountName;
  }

  onPaymentChange() {
    const total = this.calculateTotals();

    if (total === 0) {
      this.payment = 1;
      this.change = 0;
    } else {
      this.change = this.payment - total;
    }
  }

  closeRefNumErrorMessage() {
    this.refNumErrorMessage = '';
  }

  closeErrorMessage() {
    this.errorMessage = '';
  }

  confirmPayment() {
    if (this.selectedPaymentMethod === 'e-Payment') {
      if (!this.refNum || this.refNum.trim() === '') {
        this.refNumErrorMessage = 'Reference number is required for e-Payment';
        return;
      }

      this.getService.referenceNumberValidationAsync(this.refNum).subscribe({
        next: (res) => {
          if (res.message === 'Reference number already used') {
            this.refNumErrorMessage = res.message;
            return;
          }
          this.submitPayment();
        },
        error: (err) => {
          this.refNumErrorMessage =
            err.error.message || 'Invalid reference number';
          console.error('Error validating reference number:', err);
        },
      });
    } else {
      this.submitPayment();
    }
  }

  private submitPayment() {
    const eventPayload = {
      invoiceNumber: this.newInvoice,
      paymentMethod: this.selectedPaymentMethod,
      amountPaid: this.payment,
      changeAmount: this.change,
      discountAmount: this.calculateDiscountAmount(),
      vatAmount: this.calculateVatAmount(),
      referenceNumber:
        this.selectedPaymentMethod === 'e-Payment' ? this.refNum : '',
      discountId: this.selectedDiscount?.id || 0,
      employeeID: this.loggedInEmployee.id,
      vatId: this.selectedVat?.vatid || null,
      items: this.cartItems.map((i) => ({
        productId: i.productID,
        quantity: i.quantity,
        unitType: i.unitType,
        unitPrice: i.unitPrice,
        salesTransactionId: 0,
        product: i.product,
      })),
      totalAmount: this.calculateTotals(),
    };

    this.paymentConfirmed.emit(eventPayload);
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

  closeModal() {
    this.modalClosed.emit();
  }
}
