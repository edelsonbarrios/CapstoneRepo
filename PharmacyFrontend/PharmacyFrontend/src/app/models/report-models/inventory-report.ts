import { InventoryReportSummary } from "./inventory-report-summary";
import { Inventory } from "../inventory-module-models/inventory";
export class InventoryReport {
  summary: InventoryReportSummary = new InventoryReportSummary();
  inventoryItems: Inventory[] = [];
}