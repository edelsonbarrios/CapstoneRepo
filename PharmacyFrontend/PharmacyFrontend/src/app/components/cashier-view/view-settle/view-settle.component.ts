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
  @Input() newInvoice : string = '';
  vats: VAT[] = [];
  discounts: Discount[] = [];
  loggedInEmployee: Employee = new Employee();
  selectedVat: VAT | null = null;
  selectedDiscount: Discount | null = null;
  currentPage: number = 1;
  totalPages: number = 0;
  itemsPerPage: number = 2;
  paginateItems: CartItem[] = [];

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

  updatePagination() {
    this.totalPages = Math.ceil(this.cartItems.length / this.itemsPerPage);
    if(this.totalPages == 0) this.totalPages = 1;
    const startIndex = (this.currentPage -1) * this.itemsPerPage;
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
        const defaultVat = res.find(v => v.vatName == 'Standard');
        this.selectedVat = defaultVat || null;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  calculateTotals() {
    const vatAmount = this.subtotal * ((this.selectedVat?.vatRate || 0) / 100);
    const discountAmount =
      this.subtotal * ((this.selectedDiscount?.discountRate || 0) / 100);
    return this.subtotal + vatAmount - discountAmount;
  }

  onPaymentChange() {
    const total = this.calculateTotals();
    this.change = this.payment - total;
  }


  confirmPayment() {
    const eventPayload = {
      invoiceNumber: this.newInvoice, // or however you generate it
      paymentMethod: 'Cash', // or user selection
      amountPaid: this.payment,
      changeAmount: this.change,
      discountAmount:
        this.subtotal * ((this.selectedDiscount?.discountRate || 0) / 100),
      vatAmount: this.subtotal * ((this.selectedVat?.vatRate || 0) / 100),
      discountId: 1, // use actual discount ID from DB if available
      employeeID: this.loggedInEmployee.id, // pass logged-in user ID
      vatId: this.selectedVat?.vatid, // optional if you don’t track by table
      items: this.cartItems.map((i) => ({
        productId: i.productID,
        quantity: i.quantity,
        unitType: i.unitType,
        unitPrice: i.unitPrice,
        salesTransactionId: 0, // backend should ignore or set this
      })),
      totalAmount: this.calculateTotals(),
    };

    this.paymentConfirmed.emit(eventPayload);
    console.log(eventPayload);
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
