import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Medicine } from '../models/medicine';
import { SaleRecord } from '../models/sales-records';

@Injectable({ providedIn: 'root' })
export class PharmacyApiService {
  private baseUrl = '/api';

  constructor(private http: HttpClient) {}

  getMedicines(): Observable<Medicine[]> {
    return this.http.get<Medicine[]>(`${this.baseUrl}/medicines`);
  }

  addMedicine(payload: Omit<Medicine, 'id'>): Observable<Medicine> {
    return this.http.post<Medicine>(`${this.baseUrl}/medicines`, payload);
  }

  getSales(): Observable<SaleRecord[]> {
    return this.http.get<SaleRecord[]>(`${this.baseUrl}/sales`);
  }

  addSale(payload: { medicineId: number; soldQuantity: number }): Observable<SaleRecord> {
    return this.http.post<SaleRecord>(`${this.baseUrl}/sales`, payload);
  }
}