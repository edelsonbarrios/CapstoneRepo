import { CreatePurchaseOrderItem } from "../purchase-order-item-models/create-purchase-order-item";
import { PurchaseOrderItem } from "../purchase-order-item-models/purchase-order-item";

export class PurchaseOrder {
  id : number = 0;
  purchaseOrderNumber: string = '';
  orderDate : Date = new Date();
  deliveryDate: Date = new Date();
  supplierName : string = '';
  totalAmount : number = 0;
  status : string = '';
  notes: string = '';
  receiptNumber: string = '';
  purchaseOrderItems: PurchaseOrderItem[] = [];
}