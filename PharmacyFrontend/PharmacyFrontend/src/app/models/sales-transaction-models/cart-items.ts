import { Product } from "../product-models/product";

export interface CartItem {
  productID: number;
  productName: string;
  product ?: Product;
  price?: number;
  unitPrice: number;
  unitType: string;
  quantity: number;
  categoryName: string;
  brandName: string;
  formulationName: string;
}
