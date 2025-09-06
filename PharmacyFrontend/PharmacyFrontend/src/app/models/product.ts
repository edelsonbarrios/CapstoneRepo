export class Product {
    productId: number = 0;
    dateCreated: Date = new Date();
    totalStock: number = 0;
    expiredStock: number = 0;
    nearExpiry: number = 0;
    isCriticalStock: boolean = false;
    hasExpiredStock: boolean = false;
    categoryName: string = '';
    brandName: string = '';
    genericName: string = '';
    formulationName: string = '';
    classificationName: string = '';
    typeName: string = '';
    supplierName: string = '';
}