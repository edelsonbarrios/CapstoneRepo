import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { last, Observable, ObservedValueOf } from 'rxjs';
import { Employee } from '../../models/employee-models/employee';
import { Account } from '../../models/account';
import { EmployeeHistory } from '../../models/employee-history-models/employee-history';
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

  //Employee History Service 
  getAllEmployeeHistoriesAsync() : Observable<EmployeeHistory[]> {
    return this.httpClient.get<EmployeeHistory[]>(`${this.apiUrl}/Get/EmployeeHistoryAll`);
  }
  
  getDisplayEmployeeHistoriesAsync() : Observable<EmployeeHistory[]>{
    return this.httpClient.get<EmployeeHistory[]>(`${this.apiUrl}/Get/EmployeeHistoryDisplay`);
  }
  //Employee History Service End
}
