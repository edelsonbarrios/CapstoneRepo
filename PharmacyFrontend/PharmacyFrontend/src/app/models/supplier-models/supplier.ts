import { Product } from "../product-models/product";

export class Supplier {
  id : number = 0;
  supplierName : string = '';
  contactNumber : string = '';
  contactPerson: string = '';
  contactPersonNumber: string = '';
  email : string = '';
  address : string = '';
  isActive : boolean = true;
  products : Product[] = [];
}