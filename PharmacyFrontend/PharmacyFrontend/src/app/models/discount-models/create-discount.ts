export class CreateDiscountRequest {
  discountName: string = '';
  discountType: string = '';
  discountAmount : number = 0;
  startDate ?: string = '';
  endDate ?: string = ''
}
