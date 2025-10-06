import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
export class ViewPurchaseOrderDetailsComponent implements OnInit {
  @Input() purchaseOrder: PurchaseOrder = new PurchaseOrder();
  @Output() closeForm = new EventEmitter<void>();
  employee: Employee = new Employee();

  constructor(
    private updateService: UpdateService,
    private getService : GetService 
  ) {}

  ngOnInit(): void {
    this.getLoggedInEmployee();
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
          console.log(`${err.error}`);
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
