import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DengueAlert } from '../models/dengue-alert.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  // ajuste aqui se a API estiver em outro host/porta (no Docker, porta 8080)
  private readonly baseUrl =
    (window as any).__env?.API_BASE_URL || 'http://localhost:8080/api/dengue';

  constructor(private readonly http: HttpClient) {}

  // GET /api/dengue  (returns all)
  getAllAlerts(): Observable<DengueAlert[]> {
    return this.http.get<DengueAlert[]>(`${this.baseUrl}/all`);
  }

  // GET /api/dengue/sync  (synchronize remote -> db)
  syncAlerts(): Observable<{ message: string; total?: number; data?: any }> {
    return this.http.get<{ message: string; total?: number; data?: any }>(
      `${this.baseUrl}/sync`
    );
  }

  // GET /api/dengue/{week}/{year}
  getAlertByWeekAndYear(week: number, year: number) {
    return this.http.get<DengueAlert>(`${this.baseUrl}/${week}/${year}`);
  }
}
