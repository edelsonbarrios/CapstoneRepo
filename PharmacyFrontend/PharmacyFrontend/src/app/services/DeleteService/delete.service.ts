import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeleteService {
    apiUrl = `${BaseApiUrl}`;
    constructor(private httpClient: HttpClient) { }

  // Delete backup file
  deleteBackupAsync(fileName: string): Observable<any> {
    return this.httpClient.delete<any>(`${this.apiUrl}/Delete/backups/${fileName}`);
  }
}
