import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { ArchiveEmployeeRequest } from '../../models/employee-models/archiveEmployeeRequest';
import { Observable } from 'rxjs';

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
  //Employee Service End
}
