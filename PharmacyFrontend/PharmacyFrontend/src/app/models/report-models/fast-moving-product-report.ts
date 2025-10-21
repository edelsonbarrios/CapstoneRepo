import { FastMovingProduct } from "./fast-moving-product";

export class FastMovingProductReport {
  totalProductsSold : number = 0;
  totalQuantitySold: number = 0;
  totalRevenue: number = 0;
  products: FastMovingProduct[] = [];
}