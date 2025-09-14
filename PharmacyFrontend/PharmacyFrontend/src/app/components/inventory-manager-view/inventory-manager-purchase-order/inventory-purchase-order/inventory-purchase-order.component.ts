import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PurchaseOrder } from '../../../../models/purchase-order-models/purchase-order';
import { GetService } from '../../../../services/GetService/get.service';
import { PostService } from '../../../../services/PostService/post.service';
import { PurchaseOrderProductsComponent } from '../purchase-order-products/purchase-order-products.component';
import { PurchaseOrderItem } from '../../../../models/purchase-order-item-models/purchase-order-item';
import { CurrencyPipe } from '@angular/common';
import { Product } from '../../../../models/product-models/product';
@Component({
  selector: 'app-inventory-purchase-order',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    PurchaseOrderProductsComponent,
    CurrencyPipe,
    NgFor,
  ],
  templateUrl: './inventory-purchase-order.component.html',
  styleUrl: './inventory-purchase-order.component.scss',
})
export class InventoryPurchaseOrderComponent implements OnInit {
  purchaseOrders: PurchaseOrder = new PurchaseOrder();
  openProductForm: boolean = false;

  constructor(
    private getService: GetService,
    private postService: PostService
  ) {}

  ngOnInit(): void {}

  openProductsForm() {
    this.openProductForm = true;
  }

  closeProductsForm() {
    this.openProductForm = false;
  }

  onProductsSelected(products: Product[]) {
    const items: PurchaseOrderItem[] = products.map((p) => ({
      id: 0,
      productId: p.productId,
      productName: p.productName,
      quantityOrdered: 1, // default, user can edit later
      quantityReceived: 0,
      unitCost: p.price,
      totalCost: p.price, // qty(1) * price
      batchNumber: '',
      manufacturingDate: new Date(),
      expirationDate: new Date(),
    }));

    this.purchaseOrders.purchaseOrderItems.push(...items);
    this.computeTotalAmount();
    this.openProductForm = false;
  }

  computeTotalAmount() {
    this.purchaseOrders.totalAmount =
      this.purchaseOrders.purchaseOrderItems.reduce(
        (sum, item) => sum + item.totalCost,
        0
      );
  }

  updateItemTotal(item: PurchaseOrderItem) {
    item.totalCost = item.quantityOrdered * item.unitCost;
    this.computeTotalAmount();
  }

  removeItem(index: number) {
    this.purchaseOrders.purchaseOrderItems.splice(index, 1);
    this.computeTotalAmount();
  }
}
