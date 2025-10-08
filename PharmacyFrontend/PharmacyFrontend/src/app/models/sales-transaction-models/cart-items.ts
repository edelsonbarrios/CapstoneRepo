export interface CartItem {
  productID: number;
  productName: string;
  price?: number;
  unitPrice: number;
  unitType: string;
  quantity: number;
}
