import { CreatePurchaseOrderItem } from "../purchase-order-item-models/create-purchase-order-item";

export class CreatePurchaseOrder {
  purchaseOrderNumber : string = '';
  orderDate : string = '';
  deliveryDate : string = '';
  employeeLastName : string = '';
  supplierName: string = '';
  notes : string = '';
  purchaseOrderItems : CreatePurchaseOrderItem[] = [];
}