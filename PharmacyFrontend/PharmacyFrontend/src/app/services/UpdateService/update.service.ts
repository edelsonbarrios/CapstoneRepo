import { Injectable } from '@angular/core';
import { BaseApiUrl } from '../../shared/apiUrl';

@Injectable({
  providedIn: 'root'
})
export class UpdateService {
  apiUrl = `${BaseApiUrl}`;
  constructor() { }
}
