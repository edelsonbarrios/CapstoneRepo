import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, ObservedValueOf, tap,  } from 'rxjs';
import { LogInToken } from '../../components/shared/token';
import { Employee } from '../../models/employee-models/employee';
import { Account } from '../../models/account';
import { CreateEmployeeRequest } from '../../models/employee-models/createEmployeeRequest';
import { VAT } from '../../models/vat-models/vat';
import { CreateVatRequest } from '../../models/vat-models/createVatRequest';
import { CreateDiscountRequest } from '../../models/discount-models/create-discount';
import { CreateProductRequest } from '../../models/product-models/create-product-request';
import { Supplier } from '../../models/supplier-models/supplier';
import { PurchaseOrder } from '../../models/purchase-order-models/purchase-order';
import { CreatePurchaseOrder } from '../../models/purchase-order-models/create-purchase-order';
import { Brand } from '../../models/brand-models/brand';
import { Generic } from '../../models/generic-models/generic';
import { Formulation } from '../../models/formulation-models/formulation';
import { Classification } from '../../models/classification-models/classification';
import { TypeModel } from '../../models/type-models/type';
import { CreateInventory } from '../../models/inventory-module-models/createInventory';
import { CreateStockMovement } from '../../models/inventory-module-models/createStockMovement';
import { CreateCategory } from '../../models/product-models/createCategory';
import { StockAdjustment } from '../../models/inventory-module-models/stock-adjustment';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  apiUrl = `${BaseApiUrl}`
  constructor(
    private httpClient: HttpClient,
    private router : Router
  ) { }


  Authentication(userName: string, password: string) : Observable<{token: string, message: string}> {
    return this.httpClient.post<{token: string, message: string}>(`${BaseApiUrl}/Post/Login`, {userName, password});
  }

  //Employee Service
  createEmployee(employee ?: CreateEmployeeRequest | null) : Observable<{ message: string, employeeId : number}> {
    return this.httpClient.post<{ message: string, employeeId : number }>(`${this.apiUrl}/Post/Employee`, employee);
  }
  //Employee Service End

  //Account Service
  createAccount(account: Account) : Observable<{message : string}> {
    return this.httpClient.post<{message: string}>(`${this.apiUrl}/Post/Account`, account);
  }
  //Account Service End

  //VAT Service
  createVatAsync(newVat: CreateVatRequest) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/VAT`, newVat);
  }
  //VAT Service End

  //Discount Service
  createDiscountAsync(newDiscount: CreateDiscountRequest) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Discount`, newDiscount);
  }
  //Discount Service end

  //Product Service
  createProductAsync(newProduct: CreateProductRequest) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Product`, newProduct);
  }
  //Product Service End

  //Supplier Service
  createSupplierAsync(newSupplier: Supplier) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Supplier`, newSupplier);
  }
  //Supplier Service End

  //Purchase Order & Purchase Order Item Service End
  createPurchaseOrderAsync(purchaseOrder: CreatePurchaseOrder) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/PurchaseOrder`, purchaseOrder);
  }
  generateNewPurchaseOrderNumberAsync(): Observable<string> {
  return this.httpClient.post(`${this.apiUrl}/Post/NewPONumber`, {}, { responseType: 'text' });
}
  //Purchase Order Service End

  // Brand Service
  createBrandAsync(brand: Brand) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Brand`, brand);
  }
  //Brand Service End

  //Generic Service
  createGenericAsync(generic: Generic) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Generic`, generic);
  }
  //Generic Service End

  //Formulation Service
  createNewFormulationAsync(formulation : Formulation) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Formulation`, formulation);
  }
  //Formulation Service End

  //Classification Service
  createNewClassificationAsync(classification : Classification) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Classification`, classification);
  }
  //Classification Service End

  //Type Service
  createNewTypeAsync(type : TypeModel) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Type`, type);
  }
  //Type Service End

  //Sales Service
  createSalesTransactionAsync(transaction: any) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Transaction`, transaction);
  }
  generateNewInvoiceNumberAsync() : Observable<string> {
    return this.httpClient.post(`${this.apiUrl}/Post/NewInvoiceNumber`, {}, {responseType : 'text'});
  }
  //Sales Service End

  //Inventory Service
  createNewInventoryAsync(request: CreateInventory) : Observable<boolean> {
    return this.httpClient.post<boolean>(`${this.apiUrl}/Post/Inventory`, request);
  }
  //Inventory Service End

  //Stock Movement Service
  createNewStockMovementAsync(request : CreateStockMovement) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/StockMovement`, request);
  }
  //Stock Movement Service End


  //Category Service
  createNewCategoryAsync(request : CreateCategory) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/Category`, request);
  }
  //Category Service End

  //Stock Movement Service
  //Stock Movement Service End
  adjustInventoryStockAsync(request: StockAdjustment) : Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/StockAdjustment`, request);
  }

  //Database-BackUp Service
  // Create manual backup
  createManualBackupAsync(): Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/Post/backups/create`, {});
  }

  // Restore backup
  restoreBackupAsync(fileName: string): Observable<any> {
    const body = {
      fileName: fileName,
      confirmRestore: true
    };
    return this.httpClient.post<any>(`${this.apiUrl}/Post/backups/restore`, body);
  }

  // Update backup settings
  updateBackupSettingsAsync(settings: any): Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Post/backups/settings`, settings);
  }
  //Database-BackUp Service End
}
