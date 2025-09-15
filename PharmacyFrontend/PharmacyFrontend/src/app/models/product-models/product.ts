export class Product {
  productID: number = 0;
  productName: string = '';
  categoryId: number = 0;
  brandId: number = 0;
  genericId: number = 0;
  formulationId: number = 0;
  classificationId: number = 0;
  typeId: number = 0;
  supplierId: number = 0;
  price: number = 0;
  isActive: boolean = true;
  dateCreated: Date = new Date();


  totalStock: number = 0;
  expiredStock: number = 0;
  nearExpiryStock: number = 0;  
  isCriticalStock: boolean = false;
  hasExpiredStock: boolean = false;


  categoryName: string = '';
  brandName: string = '';
  genericName: string = '';
  formulationName: string = '';
  classificationName: string = '';
  typeName: string = '';
  supplierName: string = '';

  selected?: boolean;
}
