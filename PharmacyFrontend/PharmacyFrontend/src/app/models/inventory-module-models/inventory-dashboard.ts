import { InventoryReportsComponent } from "../../components/admin-view/admin-reports/inventory-reports/inventory-reports.component";
import { Inventory } from "./inventory";

export class InventoryDashboard {
  totalActiveProducts : number = 0;
  totalInventoryItems: number = 0;
  criticalStockCount : number = 0;
  nearExpiryCount: number = 0;
  expiredStockCount: number = 0;
  criticalStockItems: Inventory[] = [];
  nearExpiryItems: Inventory[] = [];
  expiredStockItems: Inventory[] = [];
}