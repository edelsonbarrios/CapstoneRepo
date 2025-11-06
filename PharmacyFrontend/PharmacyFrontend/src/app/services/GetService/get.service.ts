import { Injectable, Type } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient, HttpParams } from '@angular/common/http';
import { filter, last, Observable, ObservedValueOf } from 'rxjs';
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
import { SalesReport } from '../../models/report-models/sales-report';
import { InventoryReport } from '../../models/report-models/inventory-report';
import { ProductSalesSummary } from '../../models/sales-transaction-models/product-sales-summary';
import { FastMovingProductReport } from '../../models/report-models/fast-moving-product-report';
@Injectable({
  providedIn: 'root',
})
export class GetService {
  apiUrl = `${BaseApiUrl}`;
  constructor(private httpClient: HttpClient) {}

  //Employees Service
  GetAllEmployees(): Observable<Employee[]> {
    return this.httpClient.get<Employee[]>(`${this.apiUrl}/Get/EmployeeAll`);
  }

  GetEmployeeById(employeeId: number): Observable<Employee> {
    return this.httpClient.get<Employee>(
      `${this.apiUrl}/Get/Employee?id=${employeeId}`
    );
  }

  GetEmployeesByLastName(lastName: string): Observable<Employee[]> {
    return this.httpClient.get<Employee[]>(
      `${this.apiUrl}/Get/EmployeeLastName?lastName=${lastName}`
    );
  }

  GetDisplayEmployees(): Observable<Employee[]> {
    return this.httpClient.get<Employee[]>(
      `${this.apiUrl}/Get/DisplayEmployees`
    );
  }
  //Employee Service End

  //Accounts Service
  GetAllAccounts(): Observable<Account[]> {
    return this.httpClient.get<Account[]>(`${this.apiUrl}/Get/AccountAll`);
  }

  SearchAccountByUserName(userName: string): Observable<Account> {
    return this.httpClient.get<Account>(
      `${this.apiUrl}/Get/SearchAccount?userName=${userName}`
    );
  }

  getAccountByEmployeeIdAsync(employeeId: number) : Observable<Account> {
    return this.httpClient.get<Account>(`${this.apiUrl}/Get/AccountEmployeeId?employeeId=${employeeId}`);
  }
  //Account Service End

  //VAT Service
  getAllVatAsync(): Observable<VAT[]> {
    return this.httpClient.get<VAT[]>(`${this.apiUrl}/Get/VATAll`);
  }
  //VAT Service End

  //Discount Service
  getAllDiscountAsync(): Observable<Discount[]> {
    return this.httpClient.get<Discount[]>(`${this.apiUrl}/Get/DiscountAll`);
  }
  //Discount Service End
  //Employee History Service
  getAllEmployeeHistoriesAsync(): Observable<EmployeeHistory[]> {
    return this.httpClient.get<EmployeeHistory[]>(
      `${this.apiUrl}/Get/EmployeeHistoryAll`
    );
  }

  getDisplayEmployeeHistoriesAsync(): Observable<EmployeeHistory[]> {
    return this.httpClient.get<EmployeeHistory[]>(
      `${this.apiUrl}/Get/EmployeeHistoryDisplay`
    );
  }
  //Employee History Service End

  //Category Service
  getAllCategoriesAsync(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(`${this.apiUrl}/Get/CategoryAll`);
  }
  getAllInactiveCategoriesAsync() : Observable<Category[]> {
    return this.httpClient.get<Category[]>(`${this.apiUrl}/Get/InactiveCategories`);
  }
  //CategoryService End

  //Brand Service
  getAllBrandsAsync(): Observable<Brand[]> {
    return this.httpClient.get<Brand[]>(`${this.apiUrl}/Get/BrandAll`);
  }
  //Brand Service End

  //Generic Service
  getAllGenericsAsync(): Observable<Generic[]> {
    return this.httpClient.get<Generic[]>(`${this.apiUrl}/Get/GenericAll`);
  }
  //Generic Service End

  //Type Service
  getAllTypesAsync(): Observable<TypeModel[]> {
    return this.httpClient.get<TypeModel[]>(`${this.apiUrl}/Get/TypeAll`);
  }
  //Type Service End

  //Formulation Service
  getAllFormulationsAsync(): Observable<Formulation[]> {
    return this.httpClient.get<Formulation[]>(
      `${this.apiUrl}/Get/FormulationAll`
    );
  }
  //FormulationS Service End

  //Classification Service
  getAllClassificationAsync(): Observable<Classification[]> {
    return this.httpClient.get<Classification[]>(
      `${this.apiUrl}/Get/ClassificationAll`
    );
  }
  //Classification Service End

  //Supplier Service
  getAllSuppliersAsync(): Observable<Supplier[]> {
    return this.httpClient.get<Supplier[]>(`${this.apiUrl}/Get/SupplierAll`);
  }
  getSupplierByIdAsync(id: number): Observable<Supplier> {
    return this.httpClient.get<Supplier>(`${this.apiUrl}/Get/SupplierId`);
  }
  //Supplier Service End

  //Product Service
  getAllProductsAsync(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${this.apiUrl}/Get/ProductAll`);
  }
  getProductByIdAsync(id: number) : Observable<Product> {
    return this.httpClient.get<Product>(`${this.apiUrl}/Get/Product?id=${id}`);
  }
  getAllInactiveProductsAsync() : Observable<Product[]>{
    return this.httpClient.get<Product[]>(`${this.apiUrl}/Get/InactiveProducts`);
  }
  //Product Service End

  //Purchase Order Service
  getAllPendingPurchaseOrdersAsync(): Observable<PurchaseOrder[]> {
    return this.httpClient.get<PurchaseOrder[]>(
      `${this.apiUrl}/Get/PurchaseOrderPendings`
    );
  }
  //Purchase Order Service End

  //Inventory Service
  getAllInventoriesAsync(): Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/InventoryAll`);
  }
  getAllAvailableInventoriesAsync(): Observable<Inventory[]> {
    return this.httpClient.get<Inventory[]>(
      `${this.apiUrl}/Get/InventoryAvailable`
    );
  }
  getInventoryDashboardValues(): Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/InventoryDashboard`);
  }
  //Inventory Service End

  //Stock Movement Service
  getAllStockMovementsAsync(): Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/StockMovementAll`);
  }
  //Stock Movement Service End

  //Transaction Service
  getAllSalesTransactionsAsync(): Observable<SalesTransaction[]> {
    return this.httpClient.get<SalesTransaction[]>(
      `${this.apiUrl}/Get/TransactionAll`
    );
  }
  getSalesTransactionByCashier(id: number) : Observable<SalesTransaction[]> {
    return this.httpClient.get<SalesTransaction[]>(`${this.apiUrl}/Get/TransactionByEmployeeId?employeeId=${id}`);
  }
  getDailySalesSummaryAsync(): Observable<DailySalesSummary> {
    return this.httpClient.get<DailySalesSummary>(
      `${this.apiUrl}/Get/daily-sales/summary`
    );
  }
  getDailySalesSummaryByFilter(
    startDate: string,
    endDate: string,
    employeeId: number,
    paymentMethod: string
  ): Observable<DailySalesSummary> {
    let params = new HttpParams();

    // Always include dates (they're required by the backend)
    params = params.append('startDate', startDate);
    params = params.append('endDate', endDate);

    // Only add optional parameters if they have values
    if (employeeId && employeeId > 0) {
      params = params.append('employeeId', employeeId.toString());
    }

    if (paymentMethod && paymentMethod !== '' && paymentMethod !== 'all') {
      params = params.append('paymentMethod', paymentMethod);
    }

    console.log('API Request params:', params.toString());

    return this.httpClient.get<DailySalesSummary>(
      `${this.apiUrl}/Get/daily-sales/summary`,
      { params }
    );
  }

  getSummarySalesForTodayAsync(): Observable<DailySalesSummary> {
    return this.httpClient.get<DailySalesSummary>(
      `${this.apiUrl}/Get/DailySalesSummary`
    );
  }

  getProductSalesSummaryAsync(
    startDate?: Date | string,
    endDate?: Date | string
  ) {
    let params = new HttpParams();
    if (startDate) {
      const dateStr =
        startDate instanceof Date ? startDate.toISOString() : startDate;
      params = params.append('startDate', dateStr);
    }
    if (endDate) {
      const dateStr = endDate instanceof Date ? endDate.toISOString() : endDate;
      params = params.append('endDate', dateStr);
    }
    return this.httpClient.get<ProductSalesSummary[]>(
      `${this.apiUrl}/Get/daily-sales/products`,
      { params }
    );
  }
  //Transaction Service End

  //Sales Report Service
  getSalesReport(
    startDate?: Date | string,
    endDate?: Date | string,
    employeeId?: number,
    paymentMethod?: string
  ): Observable<SalesReport> {
    let params = new HttpParams();

    if (startDate) {
      const dateStr =
        startDate instanceof Date ? startDate.toISOString() : startDate;
      params = params.append('startDate', dateStr);
    }

    if (endDate) {
      const dateStr = endDate instanceof Date ? endDate.toISOString() : endDate;
      params = params.append('endDate', dateStr);
    }

    if (employeeId !== undefined && employeeId !== null) {
      params = params.append('employeeId', employeeId.toString());
    }

    if (paymentMethod) {
      params = params.append('paymentMethod', paymentMethod);
    }

    const url = `${this.apiUrl}/Get/reports/sales`;

    return this.httpClient.get<SalesReport>(url, { params });
  }

  exportSalesReport(
    startDate?: Date | string,
    endDate?: Date | string,
    employeeId?: number,
    paymentMethod?: string
  ): Observable<Blob> {
    let params = new HttpParams();

    if (startDate) {
      const dateStr =
        startDate instanceof Date ? startDate.toISOString() : startDate;
      params = params.append('startDate', dateStr);
    }

    if (endDate) {
      const dateStr = endDate instanceof Date ? endDate.toISOString() : endDate;
      params = params.append('endDate', dateStr);
    }

    if (employeeId !== undefined && employeeId !== null) {
      params = params.append('employeeId', employeeId.toString());
    }

    if (paymentMethod && paymentMethod !== 'All Methods') {
      params = params.append('paymentMethod', paymentMethod);
    }

    const url = `${this.apiUrl}/Get/reports/sales/export`;
    return this.httpClient.get(url, { params, responseType: 'blob' });
  }
  //Sales Report Service End

  //Inventory Report Service
  getInventoryReportsAsync(
    startDate?: Date | string,
    endDate?: Date | string,
    filterType?: string
  ): Observable<InventoryReport> {
    let params = new HttpParams();

    if (startDate) {
      const dateStr =
        startDate instanceof Date ? startDate.toISOString() : startDate;
      params = params.append('startDate', dateStr);
    }

    if (endDate) {
      const dateStr = endDate instanceof Date ? endDate.toISOString() : endDate;
      params = params.append('endDate', dateStr);
    }
    if (filterType) {
      params = params.set('filterType', filterType);
    }

    return this.httpClient.get<InventoryReport>(
      `${this.apiUrl}/Get/reports/inventory`,
      { params }
    );
  }

  exportInventoryReport(
    startDate?: Date | string,
    endDate?: Date | string,
    filterType?: string
  ): Observable<Blob> {
    let params = new HttpParams();

    if (startDate) {
      const dateStr =
        startDate instanceof Date ? startDate.toISOString() : startDate;
      params = params.append('startDate', dateStr);
    }

    if (endDate) {
      const dateStr = endDate instanceof Date ? endDate.toISOString() : endDate;
      params = params.append('endDate', dateStr);
    }

    if (filterType) {
      params = params.append('filterType', filterType);
    }

    const url = `${this.apiUrl}/Get/reports/inventory/export`;
    return this.httpClient.get(url, { params, responseType: 'blob' });
  }

  //Inventory Report Service End

  //Fast Moving Product Report Service
  getFastMovingProductReportAsync(
    startDate?: Date | string,
    endDate?: Date | string,
    topN?: number
  ): Observable<FastMovingProductReport> {
    let params = new HttpParams();

    // Only add parameters if they have valid values
    if (startDate) {
      let dateStr: string;
      if (startDate instanceof Date) {
        dateStr = startDate.toISOString();
      } else if (startDate.trim() !== '') {
        // Convert YYYY-MM-DD to ISO string
        const date = new Date(startDate);
        dateStr = date.toISOString();
      } else {
        dateStr = ''; // Empty string, don't add to params
      }

      if (dateStr) {
        params = params.append('startDate', dateStr);
      }
    }

    if (endDate) {
      let dateStr: string;
      if (endDate instanceof Date) {
        // Set to end of day
        const date = new Date(endDate);
        date.setHours(23, 59, 59, 999);
        dateStr = date.toISOString();
      } else if (endDate.trim() !== '') {
        // Convert YYYY-MM-DD to end of day ISO string
        const date = new Date(endDate);
        date.setHours(23, 59, 59, 999);
        dateStr = date.toISOString();
      } else {
        dateStr = ''; // Empty string, don't add to params
      }

      if (dateStr) {
        params = params.append('endDate', dateStr);
      }
    }

    if (topN !== undefined && topN !== null) {
      params = params.append('topN', topN.toString());
    }

    const url = `${this.apiUrl}/Get/reports/fast-moving-products`;
    console.log('Request URL:', url);
    console.log('Params:', params.toString());

    return this.httpClient.get<FastMovingProductReport>(url, { params });
  }

  exportFastMovingProductsReport(
    startDate?: Date | string,
    endDate?: Date | string,
    topN?: number
  ): Observable<Blob> {
    let params = new HttpParams();

    if (startDate) {
      const dateStr = startDate instanceof Date 
        ? startDate.toISOString() 
        : new Date(startDate).toISOString();
      params = params.append('startDate', dateStr);
    }

    if (endDate) {
      const date = endDate instanceof Date 
        ? endDate 
        : new Date(endDate);
      date.setHours(23, 59, 59, 999);
      params = params.append('endDate', date.toISOString());
    }

    if (topN !== undefined && topN !== null) {
      params = params.append('topN', topN.toString());
    }

    const url = `${this.apiUrl}/Get/reports/fast-moving-products/export`;
    return this.httpClient.get(url, { params, responseType: 'blob' });
  }
  //Fast Moving Product Report Service End

  //Admin Dashboard Services
  getTodaySalesAmountAsync() : Observable<number> {
    return this.httpClient.get<number>(`${this.apiUrl}/Get/TodaySalesAmount`);
  }
  getInventoryLowStockCountAsync() : Observable<number> {
    return this.httpClient.get<number>(`${this.apiUrl}/Get/InventoryLowStock`);
  }
  getInventoryOutOfStockCountAsync() : Observable<number> {
    return this.httpClient.get<number>(`${this.apiUrl}/Get/InventoryOutOfStock`)
  }
  getTopSellingProductsMonthly() : Observable<ProductSalesSummary[]>{
    return this.httpClient.get<ProductSalesSummary[]>(`${this.apiUrl}/Get/TopSellingMonthly`);
  }
  getInventoryNearExpiry() : Observable<Inventory[]>{
    return this.httpClient.get<Inventory[]>(`${this.apiUrl}/Get/InventoryNearExpiry`);
  }
  //Admin Dashboard Services End/

  //Database-BackUp Services

  // Get all backup files
  getBackupListAsync(): Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/backups/list`);
  }

  // Get backup settings
  getBackupSettingsAsync(): Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/Get/backups/settings`);
  }

  // Download backup file
  downloadBackupAsync(fileName: string): Observable<Blob> {
    return this.httpClient.get(`${this.apiUrl}/Get/backups/download/${fileName}`, {
      responseType: 'blob'
    });
  }
  //Database-BackUp Services End

  //StockHistory Services

  getStockHistory(
    startDate?: string,
    endDate?: string,
    transactionType?: string,
    searchTerm?: string
  ): Observable<any> {
    let params = new HttpParams();

    if (startDate) {
      params = params.set('startDate', startDate);
    }
    if (endDate) {
      params = params.set('endDate', endDate);
    }
    if (transactionType) {
      params = params.set('transactionType', transactionType);
    }
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.httpClient.get<any>(`${this.apiUrl}/GET/StockHistory`, { params });
  }
  //StockHistory Services end
}
