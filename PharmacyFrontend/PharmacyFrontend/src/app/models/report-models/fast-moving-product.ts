export class FastMovingProduct {
  rank: number = 0;
  productId: number = 0;
  productName: string = '';
  brand: string = '';
  categoryName: string = '';
  generic: string = '';
  classification: string = '';
  unitType: string = '';
  boxSold?: number;
  sheetSold?: number;
  pieceSold: number = 0;
  transactionCount: number = 0;
  revenue: number = 0;
  currentStock: number = 0;
  stockStatus: string = '';
  trend: string = '';
}