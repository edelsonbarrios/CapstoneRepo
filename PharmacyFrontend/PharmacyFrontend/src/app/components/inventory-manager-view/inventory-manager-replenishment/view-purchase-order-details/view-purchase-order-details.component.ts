import { Component, EventEmitter, Input, OnInit, Output, OnChanges, SimpleChanges } from '@angular/core';
import { PurchaseOrder } from '../../../../models/purchase-order-models/purchase-order';
import {
  CurrencyPipe,
  DatePipe,
  NgFor,
  NgIf,
} from '../../../../../../node_modules/@angular/common';
import { FormsModule } from '@angular/forms';
import { UpdateService } from '../../../../services/UpdateService/update.service';
import { PurchaseOrderReceivedRequest } from '../../../../models/purchase-order-models/purchase-order-received-request';
import { Employee } from '../../../../models/employee-models/employee';
import { jwtDecode } from 'jwt-decode';
import { LogInToken } from '../../../shared/token';
import { GetService } from '../../../../services/GetService/get.service';

@Component({
  selector: 'app-view-purchase-order-details',
  standalone: true,
  imports: [NgIf, DatePipe, FormsModule, CurrencyPipe, NgFor],
  templateUrl: './view-purchase-order-details.component.html',
  styleUrl: './view-purchase-order-details.component.scss',
})
export class ViewPurchaseOrderDetailsComponent implements OnInit, OnChanges {
  @Input() purchaseOrder: PurchaseOrder = new PurchaseOrder();
  @Output() closeForm = new EventEmitter<void>();
  employee: Employee = new Employee();
  receiptNumber: string = '';
  paginatedItems: any[] = [];
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 3;
  totalPages: number = 0;

  constructor(
    private updateService: UpdateService,
    private getService: GetService 
  ) {}

  ngOnInit(): void {
    this.getLoggedInEmployee();
    this.updatePagination();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['purchaseOrder']) {
      this.currentPage = 1;
      this.updatePagination();
    }
  }

  updatePagination() {
    if (!this.purchaseOrder.purchaseOrderItems) {
      this.paginatedItems = [];
      this.totalPages = 1;
      return;
    }
    
    this.totalPages = Math.ceil(this.purchaseOrder.purchaseOrderItems.length / this.itemsPerPage);
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.purchaseOrder.purchaseOrderItems.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  validateMfgDate(item: any) {
    if (!item.manufacturingDate) {
      item.mfgDateError = '';
      return;
    }

    const mfgDate = new Date(item.manufacturingDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    const YearsAgo = new Date(today);
    YearsAgo.setFullYear(today.getFullYear() - 5);

    if (mfgDate > today) {
      item.mfgDateError = 'Invalid Date';
    } else if (mfgDate < YearsAgo) {
      item.mfgDateError = 'Invalid Date';
    } else {
      item.mfgDateError = '';
    }
  }

  validateExpDate(item: any) {
    if (!item.expirationDate) {
      item.expDateError = '';
      return;
    }

    const expDate = new Date(item.expirationDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (expDate <= today) {
      item.expDateError = 'Invalid Date';
    } else {
      item.expDateError = '';
    }
  }

  updateItemTotal(item: any) {
    const boxTotal = (item.boxQuantityReceived || 0) * (item.boxPrice || 0);
    const sheetTotal =
      (item.sheetQuantityReceived || 0) * (item.sheetPrice || 0);
    const pieceTotal =
      (item.pieceQuantityReceived || 0) * (item.piecePrice || 0);

    item.totalCost = boxTotal + sheetTotal + pieceTotal;
    this.computeTotalAmount();
  }

  computeTotalAmount() {
    this.purchaseOrder.totalAmount =
      this.purchaseOrder.purchaseOrderItems.reduce(
        (sum, item) => sum + (item.totalCost || 0),
        0
      );
  }

  closeViewPOForm() {
    this.closeForm.emit();
  }

  markAsReceived() {
    // Clear previous errors
    this.purchaseOrder.purchaseOrderItems.forEach(item => {
      item.mfgDateError = '';
      item.expDateError = '';
    });

    let isValid = true;

    // Validate all items
    this.purchaseOrder.purchaseOrderItems.forEach(item => {
      this.validateMfgDate(item);
      this.validateExpDate(item);

      if (item.mfgDateError || item.expDateError) {
        isValid = false;
      }
    });

    if (!isValid) {
      return;
    }

    const requestPayload: PurchaseOrderReceivedRequest = {
      purchaseOrderId: this.purchaseOrder.id,
      totalAmount: this.purchaseOrder.totalAmount,
      employeeId: this.employee.id,
      purchaseOrderItems: this.purchaseOrder.purchaseOrderItems.map((item) => ({
        id: item.id,
        productID: item.productId,
        boxQuantityOrdered: item.boxQuantityOrdered,
        boxQuantityReceived: item.boxQuantityReceived,
        sheetQuantityOrdered: item.sheetQuantityOrdered,
        sheetQuantityReceived: item.sheetQuantityReceived,
        pieceQuantityOrdered: item.pieceQuantityOrdered,
        pieceQuantityReceived: item.pieceQuantityReceived,
        batchNumber: item.batchNumber,
        manufacturingDate: new Date(item.manufacturingDate).toISOString(),
        expirationDate: new Date(item.expirationDate).toISOString(),
      })),
    };
    this.updateService
      .markPurchaseOrderAsReceivedAsync(requestPayload)
      .subscribe({
        next: (res) => {
          alert(`Purchase Order has now been received.`);
          this.closeForm.emit();
        },
        error: (err) => {
          alert(`${err.error}`);
          console.log(requestPayload);
        },
      });
  }

  getLoggedInEmployee() {
      const token = localStorage.getItem(LogInToken);
      if (!token) {
        console.log(`Token does not exist.`);
        return;
      }
  
      const employee: any = jwtDecode(token);
      const employeeId = parseInt(employee.EmployeeId);
  
      this.getService.GetEmployeeById(employeeId).subscribe({
        next: (employee) => {
          this.employee = employee;
        },
        error: (err) => {
          console.log(`${err.error}`);
        },
      });
    }
}