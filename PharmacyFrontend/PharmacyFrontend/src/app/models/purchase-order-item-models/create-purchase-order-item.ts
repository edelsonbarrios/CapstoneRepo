export class CreatePurchaseOrderItem {
  productId : number = 0;
  boxQuantityOrdered : number = 0;
  sheetQuantityOrdered : number = 0;
  pieceQuantityOrdered : number = 0;
  boxPrice : number = 0;
  sheetPrice : number = 0;
  piecePrice : number = 0;
  batchNumber : string = '';
  totalCost: number = 0;
  manufacturingDate : string = '';
  expirationDate : string = '';
}