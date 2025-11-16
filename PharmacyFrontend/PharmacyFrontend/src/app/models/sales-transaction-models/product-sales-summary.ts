export class ProductSalesSummary {
  productId : number = 0;
  productName: string = '';
  quantitySold: number = 0;
  availableStock: number = 0;
  totalSales: number = 0;
  unitMeasurement?: string;
  boxSold?: number;
  sheetSold?: number;
  pieceSold?: number;
}