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

  constructor(private updateService: UpdateService) {}

  ngOnInit(): void {}

  updateItemTotal(item: any) {
    const qty = item.quantityReceived || 0;
    item.totalCost = qty * item.unitCost;
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
      purchaseOrderItems: this.purchaseOrder.purchaseOrderItems.map((item) => ({
        id: item.id,
        productId: item.productId,
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
        },
      });
  }
}
