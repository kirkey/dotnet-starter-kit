import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Role, CreateOrUpdateRoleRequest, RolePermissions, UpdatePermissionsRequest } from '../models/role.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/roles`;

  /**
   * Get all roles
   */
  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.apiUrl);
  }

  /**
   * Get a specific role by ID
   */
  getRole(id: string): Observable<Role> {
    return this.http.get<Role>(`${this.apiUrl}/${id}`);
  }

  /**
   * Create or update a role
   */
  createOrUpdateRole(request: CreateOrUpdateRoleRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, request);
  }

  /**
   * Delete a role
   */
  deleteRole(id: string): Observable<string> {
    return this.http.delete<string>(`${this.apiUrl}/${id}`);
  }

  /**
   * Get role permissions
   */
  getRolePermissions(roleId: string): Observable<RolePermissions> {
    return this.http.get<RolePermissions>(`${this.apiUrl}/${roleId}/permissions`);
  }

  /**
   * Update role permissions
   */
  updateRolePermissions(roleId: string, request: UpdatePermissionsRequest): Observable<string> {
    return this.http.put<string>(`${this.apiUrl}/${roleId}/permissions`, request);
  }
}
