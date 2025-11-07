import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
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
import { PurchaseOrderItem } from '../../../../models/purchase-order-item-models/purchase-order-item';
import { Product } from '../../../../models/product-models/product';
import { ViewPurchaseOrderItemComponent } from '../view-purchase-order-item/view-purchase-order-item.component';

@Component({
  selector: 'app-view-purchase-order-details',
  standalone: true,
  imports: [NgIf, DatePipe, FormsModule, CurrencyPipe, NgFor, ViewPurchaseOrderItemComponent],
  templateUrl: './view-purchase-order-details.component.html',
  styleUrl: './view-purchase-order-details.component.scss',
})
export class ViewPurchaseOrderDetailsComponent implements OnInit, OnChanges {
  @Input() purchaseOrder: PurchaseOrder = new PurchaseOrder();
  @Output() closeForm = new EventEmitter<void>();
  employee: Employee = new Employee();
  receiptNumber: string = '';
  paginatedItems: PurchaseOrderItem[] = [];
  markReceivedError: string = '';
  transactionReceived: boolean = false;
  viewPurchaseOrderItem ?: PurchaseOrderItem | null;
  purchaseOrders: PurchaseOrder[] = [];

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 2;
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

  getAllPendingPurchaseOrders() {
    this.getService.getAllPendingPurchaseOrdersAsync().subscribe({
      next: res =>  {
        this.purchaseOrders = res;
      },
      error: err => {
        console.log(err.error);
      }
    });
  }

  updatePagination() {
    if (!this.purchaseOrder.purchaseOrderItems) {
      this.paginatedItems = [];
      this.totalPages = 1;
      return;
    }

    this.totalPages = Math.ceil(
      this.purchaseOrder.purchaseOrderItems.length / this.itemsPerPage
    );
    if (this.totalPages === 0) this.totalPages = 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedItems = this.purchaseOrder.purchaseOrderItems.slice(
      startIndex,
      endIndex
    );
  }

  openViewPurchaseOrderItem(poItem: PurchaseOrderItem) {
    this.viewPurchaseOrderItem = poItem;
  }

  closeViewPurchaseOrderItem() {
    this.viewPurchaseOrderItem = null;
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

  updateItemTotal(item: PurchaseOrderItem) {
    const quantityReceived = item.quantityReceived || 0;
    const damagedPiecesCount = item.damagedPiecesCount || 0;

    // Calculate base total based on unit type
    let baseTotal = 0;
    if (item.poUnitType === 'Box') {
      item.boxQuantityReceived = quantityReceived;
      baseTotal = quantityReceived * item.boxPrice;
    } else {
      item.pieceQuantityReceived = quantityReceived;
      baseTotal = quantityReceived * item.piecePrice;
    }

    // Deduct damaged pieces (damaged pieces × piece price)
    const damagedAmount = damagedPiecesCount * item.piecePrice;

    // Calculate final total
    item.totalCost = baseTotal - damagedAmount;

    // Ensure totalCost doesn't go negative
    if (item.totalCost < 0) {
      item.totalCost = 0;
    }

    console.log(`Updated item: ${JSON.stringify(item)}`);
    this.computeTotalAmount();
  }

  computeTotalAmount() {
    this.purchaseOrder.totalAmount =
      this.purchaseOrder.purchaseOrderItems.reduce(
        (sum, item) => sum + (item.totalCost || 0),
        0
      );
  }

  getDisplayUnitPrice(req: PurchaseOrderItem): number {
    if (req.poUnitType === 'Box') {
      return req.boxPrice;
    } else {
      return req.piecePrice;
    }
  }

  closeViewPOForm() {
    this.closeForm.emit();
  }

  markAsReceived() {
    // Clear previous errors
    this.purchaseOrder.purchaseOrderItems.forEach((item) => {
      item.mfgDateError = '';
      item.expDateError = '';
    });

    let isValid = true;

    // Validate all items
    this.purchaseOrder.purchaseOrderItems.forEach((item) => {
      this.validateMfgDate(item);
      this.validateExpDate(item);

      if (item.mfgDateError || item.expDateError) {
        isValid = false;
      }
    });

    if (!isValid) {
      this.markReceivedError = 'Please correct the invalid dates before proceeding.';
      return;
    }

    // Validate received quantities
    for (let item of this.purchaseOrder.purchaseOrderItems) {
      if (item.boxQuantityReceived > item.boxQuantityOrdered) {
        this.markReceivedError = `Received quantity of ${item.productName} is greater than ordered quantity`;
        return;
      }
      if (item.pieceQuantityReceived > item.pieceQuantityOrdered) {
        this.markReceivedError = `Received quantity of ${item.productName} is greater than ordered quantity`;
        return;
      }
    }

    // Validate receipt number
    if (!this.receiptNumber || this.receiptNumber.trim() === '') {
      this.markReceivedError = 'Please enter a receipt number';
      return;
    }

    const requestPayload: PurchaseOrderReceivedRequest = {
      purchaseOrderId: this.purchaseOrder.id,
      totalAmount: this.purchaseOrder.totalAmount,
      employeeId: this.employee.id,
      receiptNumber: this.receiptNumber,
      purchaseOrderItems: this.purchaseOrder.purchaseOrderItems.map((item) => ({
        id: item.id,
        productID: item.productId,
        boxQuantityOrdered: item.boxQuantityOrdered,
        boxQuantityReceived: item.boxQuantityReceived || 0,
        sheetQuantityOrdered: item.sheetQuantityOrdered,
        sheetQuantityReceived: item.sheetQuantityReceived || 0,
        pieceQuantityOrdered: item.pieceQuantityOrdered,
        pieceQuantityReceived: item.pieceQuantityReceived || 0,
        damagedPiecesCount: item.damagedPiecesCount || 0,
        expiredPiecesCount: item.expiredPiecesCount || 0,
        defectPiecesCount: item.defectPiecesCount || 0,
        quantityReceived: item.quantityReceived || 0,
        batchNumber: item.batchNumber,
        manufacturingDate: new Date(item.manufacturingDate).toISOString(),
        expirationDate: new Date(item.expirationDate).toISOString(),
      })),
    };

    this.updateService
      .markPurchaseOrderAsReceivedAsync(requestPayload)
      .subscribe({
        next: (res) => {
          this.transactionReceived = true;
          this.getAllPendingPurchaseOrders();
        },
        error: (err) => {
          this.markReceivedError = err.error || 'An error occurred while processing the request';
          console.log('Error payload:', requestPayload);

        },
      });
  }

  closeErrorMessage() {
    this.markReceivedError = '';
  }

  closeReceiedMessage() {
    this.transactionReceived = false;
    this.closeForm.emit();
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

  saveItemChanges(updatedItem: PurchaseOrderItem) {
    // Find the item in the purchaseOrder and update it
    if(updatedItem.poUnitType == 'Box') {
      updatedItem.boxQuantityReceived = updatedItem.quantityReceived;
    } else if (updatedItem.poUnitType === 'Pieces') {
    updatedItem.pieceQuantityReceived = updatedItem.quantityReceived;
    }
    const index = this.purchaseOrder.purchaseOrderItems.findIndex(
      i => i.id === updatedItem.id
    );

    if (index !== -1) {
      this.purchaseOrder.purchaseOrderItems[index] = updatedItem;
      this.computeTotalAmount();
      this.updatePagination();
    }

    this.closeViewPurchaseOrderItem();
  }
}
