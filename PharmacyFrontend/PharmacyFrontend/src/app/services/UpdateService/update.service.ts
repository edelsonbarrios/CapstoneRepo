import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient, HttpParams } from '@angular/common/http';
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
import { AssignProductSupplierRequest } from '../../models/supplier-models/assign-product-supplier';
import { Supplier } from '../../models/supplier-models/supplier';
import { UpdateProductRequest } from '../../models/product-models/update-product-request';
import { Category } from '../../models/category-models/category';
import { VAT } from '../../models/vat-models/vat';
import { UpdateEmployeeRequest } from '../../models/employee-models/updateEmployeeRequest';
import { UpdateEmployeeAccountRequest } from '../../models/update-employee-account-request';

@Injectable({
  providedIn: 'root',
})
export class UpdateService {
  apiUrl = `${BaseApiUrl}`;
  constructor(private httpClient: HttpClient) {}

  //Employee Service
  ArchiveEmployeeAsync(
    request: ArchiveEmployeeRequest
  ): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveEmployee`,
      request
    );
  }

  UpdateEmployeeAsync(request: UpdateEmployeeRequest): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/Employee`,
      request
    );
  }
  //Employee Service End

  //Account Service
  resetAccountPasswordAsync(accountId: number) : Observable<{message: string}> {
    let params = new HttpParams();
    if(accountId){
      params = params.set('accountId', accountId);
    }
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/ResetAccountPassword`,{}, {params});
  }
  updateEmployeeAccountAsync(request: UpdateEmployeeAccountRequest) : Observable<{message: string}> {
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/UpdateEmployeeAccount`, request);
  }
  //Account Service End

  //Discount Service
  archiveDiscountAsync(discountId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveDiscount?discountId=${discountId}`,
      {}
    );
  }

  updateDiscountAsync(id: number, request: Discount): Observable<string> {
    return this.httpClient.put(`${this.apiUrl}/Put/Discount?id=${id}`, request, {
      responseType: 'text',
    });
  }
  //Discount Service End

  //VAT Service
  archiveVatAsync(vatId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveVat?vatId=${vatId}`,
      {}
    );
  }
  updateVatAsync(id: number, request: VAT): Observable<string> {
    return this.httpClient.put(`${this.apiUrl}/Put/VAT?id=${id}`, request, {
      responseType: 'text',
    });
  }

  //VAT Service End

  //Supplier Service
  archiveSupplierAsync(supplierId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveSupplier?supplierId=${supplierId}`,
      {}
    );
  }

  assignProductsToSupplierAsync(
    request: AssignProductSupplierRequest
  ): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/AssignProducts`,
      request
    );
  }
  updateSupplierAsync(id: number, supplier: Supplier): Observable<any> {
    return this.httpClient.put<any>(
      `${this.apiUrl}/Put/Supplier?id=${id}`,
      supplier
    );
  }
  //Supplier Service End

  //Purchase Order Service
  markPurchaseOrderAsReceivedAsync(
    purchaseOrder: PurchaseOrderReceivedRequest
  ): Observable<boolean> {
    return this.httpClient.put<boolean>(
      `${this.apiUrl}/Put/MarkAsReceived`,
      purchaseOrder
    );
  }
  archivePurchaseOrderByIdAsync(id: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchivePurchaseOrder?id=${id}`,
      {}
    );
  }
  //Purchase Order Service End

  //Brand Service
  updateBrandAsync(id: number, brand: Brand): Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Brand?id=${id}`, brand);
  }
  archiveBrandByIdAsync(brandId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveBrand?brandId=${brandId}`,
      {}
    );
  }
  //Brand Service End

  //Generic Service
  updateGenericAsync(id: number, generic: Generic): Observable<any> {
    return this.httpClient.put<any>(
      `${this.apiUrl}/Put/Generic?id=${id}`,
      generic
    );
  }
  archiveGenericByIdAsync(genericId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveGeneric?genericId=${genericId}`,
      {}
    );
  }
  //Generic Service End

  //Formulation Service
  updateFormulationAsync(
    id: number,
    formulation: Formulation
  ): Observable<any> {
    return this.httpClient.put<any>(
      `${this.apiUrl}/Put/Formulation?id=${id}`,
      formulation
    );
  }
  archiveFormulationByIdAsync(
    formulationId: number
  ): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveFormulation?formulationId=${formulationId}`,
      {}
    );
  }
  //Formulation Service End

  //Classification Service
  updateClassificationAsync(
    id: number,
    classification: Classification
  ): Observable<any> {
    return this.httpClient.put<any>(
      `${this.apiUrl}/Put/Classification?id=${id}`,
      classification
    );
  }
  archiveClassificationByIdAsync(
    classificationId: number
  ): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveClassification?classificationId=${classificationId}`,
      {}
    );
  }
  //Classification Service End

  //Type Service
  updateTypeAsync(id: number, type: TypeModel): Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/Type?id=${id}`, type);
  }
  archiveTypeModelByIdAsync(typeId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveType?typeId=${typeId}`,
      {}
    );
  }
  //Type Service End

  //Product Service
  archiveProductByIdAsync(productId: number): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveProduct?productId=${productId}`,
      {}
    );
  }
  updateProductAsync(
    productId: number,
    request: UpdateProductRequest
  ): Observable<any> {
    return this.httpClient.put<any>(
      `${this.apiUrl}/Put/Product?id=${productId}`,
      request
    );
  }
  //Product Service End

  //Category Service
  updateCategoryAsync(
    categoryId: number,
    request: Category
  ): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/Category?id=${categoryId}`,
      request
    );
  }
  archiveCategoryByIdAsync(
    categoryId: number
  ): Observable<{ message: string }> {
    return this.httpClient.put<{ message: string }>(
      `${this.apiUrl}/Put/ArchiveCategory?categoryId=${categoryId}`,
      {}
    );
  }
  //Category Service End
  // Inventory Service
  archiveInventoryByIdAsync(inventoryId: number) : Observable<{message: string}> {
    return this.httpClient.put<{message: string}>(`${this.apiUrl}/Put/ArchiveInventory?inventoryId=${inventoryId}`, {});
  }
  // Inventory Service End

  //Databasebakup service
  // update.service.ts
  updateBackupSettingsAsync(settings: any): Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}/Put/backups/settings`, settings);
  }
  //Databasebakup service end
}
