import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { NgControl } from '@angular/forms';
import { Employee } from '../../../models/employee-models/employee';
import { GetService } from '../../../services/GetService/get.service';
import { Product } from '../../../models/product-models/product';

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
  product: Product[];
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
  cashier: Employee = new Employee();
  products: Product[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  constructor(private getService: GetService) {}

  ngOnInit(): void {
    this.receiptData?.items.forEach((i) => {
      this.getProductById(i.productId);
    });

    if (this.receiptData && this.receiptData.items) {
      this.updatePagination();
    }

    this.getCashier();
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

  getCashier(): Employee {
    if (this.receiptData!.employeeID == 0) return new Employee();
    this.getService.GetEmployeeById(this.receiptData!.employeeID).subscribe({
      next: (res) => {
        this.cashier = res;
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
    return this.cashier;
  }

  getProduct(product: number) {
    if (product == 0) return;
    this.getProductById(product);
    this.receiptData.product = this.products;
  }

  getProductById(id: number) {
    this.getService.getProductByIdAsync(id).subscribe({
      next: (res) => {
        this.products.push(res);
      },
      error: (err) => {
        console.log(`${err.error}`);
      },
    });
  }

  getProductName(id: number): string | null {
    if (id == 0 || id == null) return '';
    var product = this.products.find((p) => p.productID == id);
    return product ? product.productName : '';
  }

  close() {
    this.closeForm.emit();
  }
}
