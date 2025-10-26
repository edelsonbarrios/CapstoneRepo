import { PurchaseOrderReceivedItem } from "../purchase-order-item-models/purchase-order-received-item";

export class PurchaseOrderReceivedRequest {
  purchaseOrderId : number = 0;
  totalAmount : number = 0;
  employeeId: number = 0;
  receiptNumber: string = '';
  purchaseOrderItems : PurchaseOrderReceivedItem[] = [];
}