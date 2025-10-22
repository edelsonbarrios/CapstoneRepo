import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { NgControl } from '@angular/forms';

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
export class PrintReceiptComponent implements OnInit {
  @Input() receiptData!: ReceiptPayload;
  @Output() closeForm = new EventEmitter<void>();
  finishedTransac: boolean = false;
  paginatedItems: ReceiptItem[] = [];
  isPrinting: boolean = false;

  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  ngOnInit(): void {
    if (this.receiptData && this.receiptData.items) {
      this.updatePagination();
    }
  }

  updatePagination() {
    this.totalPages = Math.ceil(
      this.receiptData.items.length / this.itemsPerPage
    );
    if (this.totalPages == 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.receiptData.items.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  today: Date = new Date();

  printReceipt() {
    this.isPrinting = true;
    setTimeout(() => {
      window.print();
      this.isPrinting = false;
    }, 1000);
  }

  close() {
    this.closeForm.emit();
  }
}
