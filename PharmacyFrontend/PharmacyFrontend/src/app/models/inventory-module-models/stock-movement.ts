export class StockMovement {
  stockMovementID: number = 0;
  inventoryID : number = 0;
  productName: string = '';
  batchNumber: string = '';
  movementType: string = '';
  boxQuantityChanged: number = 0;
  sheetQuantityChanged: number = 0;
  pieceQuantityChanged: number = 0;
  boxQuantityBefore: number = 0;
  sheetQuantityBefore: number = 0;
  pieceQuantityBefore: number = 0;
  boxQuantityAfter: number = 0;
  sheetQuantityAfter : number = 0;
  pieceQuantityAfter: number = 0;
  referenceNumber : string = '';
  notes: string = '';
  movementDate: string = '';
  employeeID: number = 0;
  employeeName: string = '';
}