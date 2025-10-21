import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';

interface ReceiptItem {
  productId: number;
  quantity: number;
  unitType: string;
  unitPrice: number;
}

interface ReceiptPayload {
  invoiceNumber: string;
  paymentMethod: string;
  amountPaid: number;
  changeAmount: number;
  discountAmount: number;
  vatAmount: number;
  discountId: number;
  employeeID: number;
  vatId?: number;
  totalAmount: number;
  items: ReceiptItem[];
}

@Component({
  selector: 'app-print-receipt',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, DatePipe],
  templateUrl: './print-receipt.component.html',
  styleUrls: ['./print-receipt.component.scss'],
})
export class PrintReceiptComponent {
  @Input() receiptData!: ReceiptPayload;
  @Output() closeForm = new EventEmitter<void>();
  finishedTransac : boolean = false;

  today: Date = new Date();

  printReceipt() {
    window.print();
  }

  close() {
    this.closeForm.emit();
  }
}
