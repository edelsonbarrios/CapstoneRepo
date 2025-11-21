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
  vats: VAT[] = [];
  discounts: Discount[] = [];
  loggedInEmployee: Employee = new Employee();
  selectedVat: VAT | null = null;
  selectedDiscount: Discount | null = null;
  currentPage: number = 1;
  totalPages: number = 0;
  itemsPerPage: number = 2;
  paginateItems: CartItem[] = [];
  paymentMethods: string[] = ['Cash', 'e-Payment'];
  selectedPaymentMethod: string = 'Cash';
  refNum: string = '';
  refNumErrorMessage: string = '';

  @Output() paymentConfirmed = new EventEmitter<any>();
  @Output() modalClosed = new EventEmitter<void>();

  payment: number = 0;
  change: number = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.getLoggedInUser();
    this.getAllVAT(); // Load VAT first to set default
    this.getAllDiscounts();
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
        
        // Set Standard VAT as default
        const defaultVat = res.find((v) => v.vatName === 'Standard');
        
        if (defaultVat) {
          this.selectedVat = defaultVat;
        } else if (res.length > 0) {
          // Fallback: use first VAT if 'Standard' not found
          this.selectedVat = res[0];
          console.warn('Standard VAT not found, using first available VAT');
        } else {
          this.selectedVat = null;
          console.warn('No VAT options available');
        }
        
        // Calculate initial totals with default VAT
        this.onPaymentChange();
      },
      error: (err) => {
        console.log(`${err.error}`);
        this.selectedVat = null;
      },
    });
  }

  /**
   * Calculate discount amount based on discount type
   */
  calculateDiscountAmount(): number {
    if (!this.selectedDiscount) return 0;

    const subtotalWithVat = this.subtotal + this.calculateVatAmount();
    let discountAmount = 0;

    if (this.selectedDiscount.discountType === 'Percentage') {
      // Percentage-based discount
      discountAmount = this.subtotal * ((this.selectedDiscount.discountAmount || 0) / 100);
    } else if (this.selectedDiscount.discountType === 'Fixed') {
      // Fixed amount discount
      discountAmount = this.selectedDiscount.discountAmount || 0;
    }

    // Ensure discount doesn't exceed subtotal + VAT
    return Math.min(discountAmount, subtotalWithVat);
  }

  /**
   * Calculate VAT amount
   */
  calculateVatAmount(): number {
    return this.subtotal * ((this.selectedVat?.vatRate || 0) / 100);
  }

  /**
   * Calculate total amount (Subtotal + VAT - Discount)
   * Ensures total is never negative
   */
  calculateTotals(): number {
    const vatAmount = this.calculateVatAmount();
    const discountAmount = this.calculateDiscountAmount();
    const total = this.subtotal + vatAmount - discountAmount;
    
    // Ensure total is never negative
    return Math.max(total, 0);
  }

  /**
   * Format discount display text
   */
  getDiscountDisplayText(): string {
    if (!this.selectedDiscount) return 'No Discount';

    if (this.selectedDiscount.discountType === 'Percentage') {
      return `${this.selectedDiscount.discountName} (${this.selectedDiscount.discountAmount}%)`;
    } else {
      return `${this.selectedDiscount.discountName} (₱${this.selectedDiscount.discountAmount.toFixed(2)})`;
    }
  }

  onPaymentChange() {
    const total = this.calculateTotals();
    
    // If total is 0 (discount exceeds total), set payment to 1 peso
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

  confirmPayment() {
    // Validate reference number only for e-Payment
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
          this.refNumErrorMessage = err.error.message || 'Invalid reference number';
          console.error('Error validating reference number:', err);
        },
      });
    } else {
      // For cash payment, submit directly
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
      referenceNumber: this.selectedPaymentMethod === 'e-Payment' ? this.refNum : '',
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