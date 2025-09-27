import { Product } from "../product-models/product";

export class AssignProductSupplierRequest {
  supplierId : number = 0;
  productsToBeAdded : Product[] = [];
}