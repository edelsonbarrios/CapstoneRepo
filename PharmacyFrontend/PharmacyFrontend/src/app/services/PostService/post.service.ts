import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap,  } from 'rxjs';
import { LogInToken } from '../../components/shared/token';
import { Employee } from '../../models/employee-models/employee';
import { Account } from '../../models/account';
import { CreateEmployeeRequest } from '../../models/employee-models/createEmployeeRequest';
import { VAT } from '../../models/vat-models/vat';
import { CreateVatRequest } from '../../models/vat-models/createVatRequest';
import { CreateDiscountRequest } from '../../models/discount-models/create-discount';

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
}
