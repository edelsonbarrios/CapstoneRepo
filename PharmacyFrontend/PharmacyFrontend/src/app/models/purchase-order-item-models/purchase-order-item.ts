export class PurchaseOrderItem {
  id: number = 0;
  productId: number = 0;
  productName: string = '';
  quantityOrdered: number = 0;
  quantityReceived: number = 0;
  unitCost: number = 0;
  totalCost: number = 0;
  batchNumber: string = '';
  manufacturingDate : Date = new Date();
  expirationDate: Date = new Date();

  category: string = '';
  type: string = '';
  classification: string = '';
}