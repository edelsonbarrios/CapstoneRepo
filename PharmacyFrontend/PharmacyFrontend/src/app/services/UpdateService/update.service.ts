import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { ArchiveEmployeeRequest } from '../../models/employee-models/archiveEmployeeRequest';
import { Observable } from 'rxjs';
import { Employee } from '../../models/employee-models/employee';
import { Discount } from '../../models/discount-models/discount';

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
}
