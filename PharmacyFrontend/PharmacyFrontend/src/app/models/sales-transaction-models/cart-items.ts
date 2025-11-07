export interface CartItem {
  productID: number;
  productName: string;
  categoryName: string;
  brandName: string;
  formulationName: string;
  price?: number;
  unitPrice: number;
  unitType: string;
  quantity: number;
}
