import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { ArchiveEmployeeRequest } from '../../models/employee-models/archiveEmployeeRequest';
import { Observable } from 'rxjs';
import { Employee } from '../../models/employee-models/employee';
import { Discount } from '../../models/discount-models/discount';
import { PurchaseOrderReceivedRequest } from '../../models/purchase-order-models/purchase-order-received-request';
import { Brand } from '../../models/brand-models/brand';
import { Generic } from '../../models/generic-models/generic';
import { Formulation } from '../../models/formulation-models/formulation';
import { Classification } from '../../models/classification-models/classification';
import { TypeModel } from '../../models/type-models/type';

@Injectable({
  providedIn: 'root'
})
export class UpdateService {
  apiUrl = `${BaseApiUrl}`;
  constructor(
    private httpClient: HttpClient
  ) { }

  //Employee Service
  ArchiveEmployeeAsync(request : ArchiveEmployeeRequest) : Observable<{message: string}> {
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/ArchiveEmployee`, request);
  }

  UpdateEmployeeAsync(request: Employee) : Observable<{message: string}> {
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/Employee`, request);
  }
  //Employee Service End

 //Discount Service
  archiveDiscountAsync(discountId: number) : Observable<{message: string}> {
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/ArchiveDiscount?discountId=${discountId}`, {});
  }
 //Discount Service End

 //VAT Service
  archiveVatAsync(vatId: number) : Observable<{message : string}> {
    return this.httpClient.put<{message : string}>(`${this.apiUrl}/Put/ArchiveVat?vatId=${vatId}`, {});
  }
 //VAT Service End

 //Supplier Service
  archiveSupplierAsync(supplierId : number) : Observable<{message: string}> {
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/ArchiveSupplier?supplierId=${supplierId}`, {});
  }
 //Supplier Service End

 //Purchase Order Service
  markPurchaseOrderAsReceivedAsync(purchaseOrder : PurchaseOrderReceivedRequest) : Observable<boolean> {
    return this.httpClient.put<boolean>(`${this.apiUrl}/Put/MarkAsReceived`, purchaseOrder);
  } 
 //Purchase Order Service End

 //Brand Service
  updateBrandAsync(id: number, brand: Brand) : Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Brand?id=${id}`, brand);
  }
 //Brand Service End

 //Generic Service
  updateGenericAsync(id: number, generic : Generic) : Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Generic?id=${id}`, generic);
  }
 //Generic Service End

 //Formulation Service
  updateFormulationAsync(id: number, formulation: Formulation) : Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Formulation?id=${id}`, formulation);
  }
 //Formulation Service End

 //Classification Service
  updateClassificationAsync(id: number, classification: Classification) : Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Classification?id=${id}`, classification);
  }
 //Classification Service End

 //Type Service
  updateTypeAsync(id: number, type: TypeModel) : Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Type?id=${id}`, type);
  }
 //Type Service End 
}
