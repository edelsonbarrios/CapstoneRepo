export class Inventory {
    inventoryID: number = 0;
    productID: number = 0;
    productName: string = '';
    batchNumber: string = '';
    manufacturingDate: Date = new Date();
    expirationDate: Date = new Date();
    quantityOnHand: number = 0;
    reorderLevel: number = 0;
    costPrice: number = 0;
    unitCost: number = 0;
    sellingPrice: number = 0;
    dateAdded: Date = new Date();
    isActive: boolean = false;
    isExpired: boolean = false;
    isNearExpiry: boolean = false;
    isCriticalStock: boolean = false;
}