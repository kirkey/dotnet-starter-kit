import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Tenant, CreateTenantRequest, UpgradeSubscriptionRequest } from '../models/tenant.model';

@Injectable({
  providedIn: 'root'
})
export class TenantService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/tenants`;

  /**
   * Get all tenants
   */
  getTenants(): Observable<Tenant[]> {
    return this.http.get<Tenant[]>(this.apiUrl);
  }

  /**
   * Get a specific tenant by ID
   */
  getTenant(id: string): Observable<Tenant> {
    return this.http.get<Tenant>(`${this.apiUrl}/${id}`);
  }

  /**
   * Create a new tenant
   */
  createTenant(request: CreateTenantRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, request);
  }

  /**
   * Activate a tenant
   */
  activateTenant(id: string): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/${id}/activate`, {});
  }

  /**
   * Deactivate a tenant
   */
  deactivateTenant(id: string): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/${id}/deactivate`, {});
  }

  /**
   * Upgrade tenant subscription
   */
  upgradeSubscription(request: UpgradeSubscriptionRequest): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/upgrade`, request);
  }
}
