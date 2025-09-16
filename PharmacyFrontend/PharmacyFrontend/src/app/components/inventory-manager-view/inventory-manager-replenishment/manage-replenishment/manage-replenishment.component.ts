import { Component, OnInit } from '@angular/core';
import { PurchaseOrder } from '../../../../models/purchase-order-models/purchase-order';
import { GetService } from '../../../../services/GetService/get.service';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { ViewPurchaseOrderDetailsComponent } from '../view-purchase-order-details/view-purchase-order-details.component';

@Component({
  selector: 'app-manage-replenishment',
  standalone: true,
  imports: [
    FormsModule, 
    NgFor, 
    DatePipe,
    ViewPurchaseOrderDetailsComponent,
    NgIf
  ],
  templateUrl: './manage-replenishment.component.html',
  styleUrl: './manage-replenishment.component.scss'
})
export class ManageReplenishmentComponent implements OnInit{
  purchaseOrders : PurchaseOrder[] = [];
  purchaseOrder ?: PurchaseOrder | null;
  
  constructor(
    private getService : GetService
  ) {}

  ngOnInit(): void {
    this.getAllPendingPurchaseOrders();
  }

  getAllPendingPurchaseOrders() {
    this.getService.getAllPendingPurchaseOrdersAsync().subscribe({
      next: res => {
        this.purchaseOrders = res;
      },
      error: err => {
        console.log(`${err.error}`);
      }
    });
  }
  
  viewPurchaseOrderDetails(purchaseOrder: PurchaseOrder) {
    this.purchaseOrder = purchaseOrder;
  }

  closeViewPurchaseOrderForm() {
    this.purchaseOrder = null;
  }
}
