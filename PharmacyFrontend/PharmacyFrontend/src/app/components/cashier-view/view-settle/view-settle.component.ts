import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartItem } from '../../../models/sales-transaction-models/cart-items';

@Component({
  selector: 'app-view-settle',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './view-settle.component.html',
  styleUrls: ['./view-settle.component.scss']
})
export class ViewSettleComponent {
  @Input() cartItems: CartItem[] = [];
  @Input() subtotal: number = 0;
  @Input() vat: number = 12;
  @Input() discount: number = 0;

  @Output() paymentConfirmed = new EventEmitter<any>();
  @Output() modalClosed = new EventEmitter<void>();

  payment: number = 0;
  change: number = 0;

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
      payment: this.payment,
      change: this.change,
      subtotal: this.subtotal,
      vat: this.vat,
      discount: this.discount,
      items: this.cartItems
    };
    this.paymentConfirmed.emit(eventPayload);
  }

  closeModal() {
    this.modalClosed.emit();
  }
}
