import { Product } from "../product-models/product";

export class Inventory {
    inventoryID: number = 0;
    productID: number = 0;
    product: Product = new Product();
    productName: string = '';
    categoryName: string='';
    brandName: string ='';
    batchNumber: string = '';
    manufacturingDate: Date = new Date();
    expirationDate: Date = new Date();
    boxQuantity: number = 0;
    sheetQuantity: number = 0;
    pieceQuantity: number = 0;
    sheetsPerBox: number = 0;
    piecesPerSheet: number = 0;
    reorderLevel: number = 0;
    costPrice: number = 0;
    boxPrice: number = 0;
    sheetPrice: number = 0;
    piecePrice: number = 0;
    dateAdded: Date = new Date();
    isActive: boolean = false;
    isExpired: boolean = false;
    isNearExpiry: boolean = false;
    isCriticalStock: boolean = false;
    isMultiUnit : boolean = false;
    totalPiecesAvailable:  number = 0;
    discountId: number = 0;

  // Additional Properties

  selected: boolean = false;
}
