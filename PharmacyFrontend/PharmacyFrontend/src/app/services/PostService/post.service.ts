import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap,  } from 'rxjs';
import { LogInToken } from '../../components/shared/token';
import { Employee } from '../../models/employee';

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
  createEmployee(employee: Employee) : Observable<{ message: string }> {
    return this.httpClient.post<{ message: string }>(`${this.apiUrl}/Post/Employee`, employee);
  }
  
}
