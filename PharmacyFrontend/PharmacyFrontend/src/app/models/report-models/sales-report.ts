import { SalesTransaction } from "../sales-transaction-models/sales-transaction";
import { SalesReportSummary } from "./sales-report-summary";

export class SalesReport {
  summary: SalesReportSummary = new SalesReportSummary();
  transactions: SalesTransaction[] = [];
}