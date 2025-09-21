import { CreateSalesItemDto } from "./create-sales-item";

export class CreateSalesTransactionDto {
  invoiceNumber: string = '';
  paymentMethod: string = '';
  amountPaid: number = 0;
  changeAmount: number = 0;
  discountAmount: number = 0;
  vatAmount: number = 0;
  discountId: number = 0;
  employeeID: number = 0;
  vatId?: number;
  items: CreateSalesItemDto[] = [];
}
