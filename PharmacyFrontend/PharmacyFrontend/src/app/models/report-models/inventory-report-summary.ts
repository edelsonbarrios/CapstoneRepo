import { Inventory } from "../inventory-module-models/inventory";

export class InventoryReportSummary  {
  totalProducts : number = 0;
  totalValue: number = 0;
  criticalStockCount: number = 0;
  nearExpiryCount: number = 0;
}