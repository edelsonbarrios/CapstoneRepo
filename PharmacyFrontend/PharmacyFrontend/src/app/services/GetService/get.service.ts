import { Injectable, Type } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { last, Observable, ObservedValueOf } from 'rxjs';
import { Employee } from '../../models/employee-models/employee';
import { Account } from '../../models/account';
import { VAT } from '../../models/vat-models/vat';
import { Discount } from '../../models/discount-models/discount';
import { EmployeeHistory } from '../../models/employee-history-models/employee-history';
import { Category } from '../../models/category-models/category';
import { Brand } from '../../models/brand-models/brand';
import { Generic } from '../../models/generic-models/generic';
import { TypeModel } from '../../models/type-models/type';
import { Formulation } from '../../models/formulation-models/formulation';
import { Classification } from '../../models/classification-models/classification';
import { Supplier } from '../../models/supplier-models/supplier';
import { Product } from '../../models/product-models/product';
import { PurchaseOrder } from '../../models/purchase-order-models/purchase-order';
import { Inventory } from '../../models/inventory-module-models/inventory';
import { SalesTransaction } from '../../models/sales-transaction-models/sales-transaction';
import { DailySalesSummary } from '../../models/sales-transaction-models/daily-sales-summary';
@Injectable({
  providedIn: 'root'
})
export class GetService {
  apiUrl = `${BaseApiUrl}`;
  constructor(
    private httpClient : HttpClient
  ) { } 

  //Employees Service
  GetAllEmployees() : Observable<Employee[]> {
    return this.httpClient.get<Employee[]>(`${this.apiUrl}/Get/EmployeeAll`);
  }

  GetEmployeeById(employeeId : number) : Observable<Employee> {
    return this.httpClient.get<Employee>(`${this.apiUrl}/Get/Employee?id=${employeeId}`);
  }

  GetEmployeesByLastName(lastName: string) : Observable<Employee[]>{
    return this.httpClient.get<Employee[]>(`${this.apiUrl}/Get/EmployeeLastName?lastName=${lastName}`);
  }

  GetDisplayEmployees() : Observable<Employee[]> {
    return this.httpClient.get<Employee[]>(`${this.apiUrl}/Get/DisplayEmployees`);
  }
  //Employee Service End


  //Accounts Service
  GetAllAccounts() : Observable<Account[]> {
    return this.httpClient.get<Account[]>(`${this.apiUrl}/Get/AccountAll`);
  }

  SearchAccountByUserName(userName: string) : Observable<Account>{
    return this.httpClient.get<Account>(`${this.apiUrl}/Get/SearchAccount?userName=${userName}`);
  }
  //Account Service End

  //VAT Service
  getAllVatAsync() : Observable<VAT[]> {
    return this.httpClient.get<VAT[]>(`${this.apiUrl}/Get/VATAll`);
  }
  //VAT Service End

  //Discount Service
  getAllDiscountAsync() : Observable<Discount[]> {
    return this.httpClient.get<Discount[]>(`${this.apiUrl}/Get/DiscountAll`);
  }
  //Discount Service End
  //Employee History Service 
  getAllEmployeeHistoriesAsync() : Observable<EmployeeHistory[]> {
    return this.httpClient.get<EmployeeHistory[]>(`${this.apiUrl}/Get/EmployeeHistoryAll`);
  }
  
  getDisplayEmployeeHistoriesAsync() : Observable<EmployeeHistory[]>{
    return this.httpClient.get<EmployeeHistory[]>(`${this.apiUrl}/Get/EmployeeHistoryDisplay`);
  }
  //Employee History Service End

  //Category Service
  getAllCategoriesAsync() : Observable<Category[]> {
    return this.httpClient.get<Category[]>(`${this.apiUrl}/Get/CategoryAll`);
  }
  //CategoryService End 

  //Brand Service
  getAllBrandsAsync() : Observable<Brand[]> {
    return this.httpClient.get<Brand[]>(`${this.apiUrl}/Get/BrandAll`);
  }
  //Brand Service End

  //Generic Service
  getAllGenericsAsync() : Observable<Generic[]> {
    return this.httpClient.get<Generic[]>(`${this.apiUrl}/Get/GenericAll`);
  }
  //Generic Service End

  //Type Service
  getAllTypesAsync() : Observable<TypeModel[]> {
    return this.httpClient.get<TypeModel[]>(`${this.apiUrl}/Get/TypeAll`);
  }
  //Type Service End

  //Formulation Service
  getAllFormulationsAsync() : Observable<Formulation[]> {
    return this.httpClient.get<Formulation[]>(`${this.apiUrl}/Get/FormulationAll`);
  }
  //FormulationS Service End

  //Classification Service
  getAllClassificationAsync() : Observable<Classification[]> {
    return this.httpClient.get<Classification[]>(`${this.apiUrl}/Get/ClassificationAll`);
  }
  //Classification Service End

  //Supplier Service
  getAllSuppliersAsync() : Observable<Supplier[]> {
    return this.httpClient.get<Supplier[]>(`${this.apiUrl}/Get/SupplierAll`);
  }
  //Supplier Service End

  //Product Service
  getAllProductsAsync() : Observable<Product[]>{
    return this.httpClient.get<Product[]>(`${this.apiUrl}/Get/ProductAll`);
  }
  //Product Service End

  //Purchase Order Service
  getAllPendingPurchaseOrdersAsync() : Observable<PurchaseOrder[]> {
    return this.httpClient.get<PurchaseOrder[]>(`${this.apiUrl}/Get/PurchaseOrderPendings`);
  }
  //Purchase Order Service End

  //Inventory Service
  getAllInventoriesAsync() : Observable<any>{ 
    return this.httpClient.get<any>(`${this.apiUrl}/Get/InventoryAll`);
  }
  getAllAvailableInventoriesAsync() : Observable<Inventory[]> {
    return this.httpClient.get<Inventory[]>(`${this.apiUrl}/Get/InventoryAvailable`);
  }
  getInventoryDashboardValues() : Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/InventoryDashboard`);
  }
  //Inventory Service End

  //Stock Movement Service
  getAllStockMovementsAsync() : Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/StockMovementAll`);
  }
  //Stock Movement Service End

  //Transaction Service
  getAllSalesTransactionsAsync() : Observable<SalesTransaction[]> {
    return this.httpClient.get<SalesTransaction[]>(`${this.apiUrl}/Get/TransactionAll`);
  }
  getDailySalesSummaryAsync() : Observable<DailySalesSummary> {
    return this.httpClient.get<DailySalesSummary>(`${this.apiUrl}/Get/daily-sales/summary`);
  }
  getSummarySalesForTodayAsync() : Observable<DailySalesSummary> {
    return this.httpClient.get<DailySalesSummary>(`${this.apiUrl}/Get/DailySalesSummary`);
  }
  //Transaction Service End 
}
