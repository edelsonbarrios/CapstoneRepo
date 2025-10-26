export class PurchaseOrderItem {
  id: number = 0;
  productId: number = 0;
  productName: string = '';
  boxQuantityOrdered: number = 0;
  boxQuantityReceived: number = 0;
  sheetQuantityOrdered: number = 0;
  sheetQuantityReceived: number = 0;
  pieceQuantityOrdered: number = 0;
  pieceQuantityReceived: number = 0;
  boxPrice: number = 0;
  sheetPrice: number = 0;
  piecePrice: number = 0;
  totalCost: number = 0;
  batchNumber: string = '';
  manufacturingDate : Date = new Date();
  expirationDate: Date = new Date();
  unitMeasurement: string = '';

  category: string = '';
  type: string = '';
  classification: string = '';

  mfgDateError?: string;
  expDateError?: string;
  poUnitType: string = '';
  quantityOrdered: number = 0;
  quantityReceived: number = 0;
}