export class CreateInventory {
    productID: number = 0;
    batchNumber: string = '';
    manufacturingDate: Date = new Date();
    expirationDate: Date = new Date();
    quantityOnHand: number = 0;
    reorderLevel: number = 0;
    costPrice: number = 0;
    unitCost: number = 0;
    sellingPrice: number = 0;
}