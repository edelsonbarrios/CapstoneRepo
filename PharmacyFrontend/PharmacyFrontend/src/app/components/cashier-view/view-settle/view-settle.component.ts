import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartItem } from '../../../models/sales-transaction-models/cart-items';
import { LogInToken } from '../../shared/token';
import { jwtDecode } from 'jwt-decode';
import { Employee } from '../../../models/employee-models/employee';
import { GetService } from '../../../services/GetService/get.service';

@Component({
  selector: 'app-view-settle',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './view-settle.component.html',
  styleUrls: ['./view-settle.component.scss'],
})
export class ViewSettleComponent implements OnInit{
  @Input() cartItems: CartItem[] = [];
  @Input() subtotal: number = 0;
  @Input() vat: number = 12;
  @Input() discount: number = 0;
  loggedInEmployee: Employee = new Employee();

  @Output() paymentConfirmed = new EventEmitter<any>();
  @Output() modalClosed = new EventEmitter<void>();

  payment: number = 0;
  change: number = 0;

  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getLoggedInUser();
  }

  calculateTotals() {
    const vatAmount = this.subtotal * (this.vat / 100);
    const discountAmount = this.subtotal * (this.discount / 100);
    return this.subtotal + vatAmount - discountAmount;
  }

  onPaymentChange() {
    const total = this.calculateTotals();
    this.change = this.payment - total;
  }

  confirmPayment() {
    const eventPayload = {
      invoiceNumber: 'INV-' + new Date().getTime(), // or however you generate it
      paymentMethod: 'Cash', // or user selection
      amountPaid: this.payment,
      changeAmount: this.change,
      discountAmount: this.subtotal * (this.discount / 100),
      vatAmount: this.subtotal * (this.vat / 100),
      discountId: 1, // use actual discount ID from DB if available
      employeeID: this.loggedInEmployee.id, // pass logged-in user ID
      vatId: null, // optional if you don’t track by table
      items: this.cartItems.map((i) => ({
        productId: i.productID,
        quantity: i.quantity,
        unitPrice: i.price,
        salesTransactionId: 0, // backend should ignore or set this
      })),
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
