// models/sales-transaction.model.ts
import { SalesItem } from "./sales-item";

export class SalesTransaction {
  id: number = 0;
  transactionDate: string = '';
  invoiceNumber: string = '';
  totalAmount: number = 0;
  discountAmount: number = 0;
  vatAmount: number = 0;
  netAmount: number = 0;
  paymentMethod: string = '';
  amountPaid: number = 0;
  changeAmount: number = 0;
  discountId: number = 0;
  employeeID: number = 0;
  vatId?: number;

  // Navigation/display properties
  discountName: string = '';
  employeeName: string = '';
  vatRate?: number;

  items: SalesItem[] = [];
}
