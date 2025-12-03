import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, throwError, of, map } from 'rxjs';
import { environment } from '@env/environment';
import { AuthService } from './auth.service';
import { FshActions, FshResources, Permission, getAllPermissions } from '../models/permission.model';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  
  private _userPermissions = signal<string[]>([]);
  readonly userPermissions = this._userPermissions.asReadonly();
  
  // Cache for permission checks
  private permissionCache = new Map<string, boolean>();

  constructor() {
    // Load permissions when service is created
    this.loadUserPermissions();
  }

  /**
   * Load the current user's permissions from the API
   */
  loadUserPermissions(): void {
    if (!this.authService.isAuthenticated()) {
      this._userPermissions.set([]);
      return;
    }

    this.http.get<string[]>(`${environment.apiUrl}/personal/permissions`)
      .pipe(
        catchError(() => {
          // On error, return empty permissions or decode from JWT
          return of(this.getPermissionsFromToken());
        })
      )
      .subscribe(permissions => {
        this._userPermissions.set(permissions);
        this.permissionCache.clear();
      });
  }

  /**
   * Get permissions from JWT token claims
   */
  private getPermissionsFromToken(): string[] {
    const token = this.authService.getToken();
    if (!token) return [];

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.permission || payload.permissions || [];
    } catch {
      return [];
    }
  }

  /**
   * Check if the user has a specific permission
   */
  hasPermission(action: string, resource: string): boolean {
    const permissionName = `Permissions.${resource}.${action}`;
    
    // Check cache first
    if (this.permissionCache.has(permissionName)) {
      return this.permissionCache.get(permissionName)!;
    }

    const permissions = this._userPermissions();
    const hasPermission = permissions.includes(permissionName);
    
    // Cache the result
    this.permissionCache.set(permissionName, hasPermission);
    
    return hasPermission;
  }

  /**
   * Check if the user has any of the specified permissions
   */
  hasAnyPermission(checks: { action: string; resource: string }[]): boolean {
    return checks.some(check => this.hasPermission(check.action, check.resource));
  }

  /**
   * Check if the user has all of the specified permissions
   */
  hasAllPermissions(checks: { action: string; resource: string }[]): boolean {
    return checks.every(check => this.hasPermission(check.action, check.resource));
  }

  /**
   * Check if user can view a resource
   */
  canView(resource: string): boolean {
    return this.hasPermission(FshActions.View, resource);
  }

  /**
   * Check if user can create a resource
   */
  canCreate(resource: string): boolean {
    return this.hasPermission(FshActions.Create, resource);
  }

  /**
   * Check if user can update a resource
   */
  canUpdate(resource: string): boolean {
    return this.hasPermission(FshActions.Update, resource);
  }

  /**
   * Check if user can delete a resource
   */
  canDelete(resource: string): boolean {
    return this.hasPermission(FshActions.Delete, resource);
  }

  /**
   * Check if user can export a resource
   */
  canExport(resource: string): boolean {
    return this.hasPermission(FshActions.Export, resource);
  }

  /**
   * Clear cached permissions (call on logout)
   */
  clearPermissions(): void {
    this._userPermissions.set([]);
    this.permissionCache.clear();
  }

  /**
   * Get all available permissions (for role management)
   */
  getAllAvailablePermissions(): Permission[] {
    return getAllPermissions();
  }

  /**
   * Get role permissions from API
   */
  getRolePermissions(roleId: string): Observable<{ name: string; permissions: string[] }> {
    return this.http.get<{ name: string; permissions: string[] }>(
      `${environment.apiUrl}/roles/${roleId}/permissions`
    );
  }

  /**
   * Update role permissions
   */
  updateRolePermissions(roleId: string, permissions: string[]): Observable<string> {
    return this.http.put<string>(
      `${environment.apiUrl}/roles/${roleId}/permissions`,
      { roleId, permissions }
    );
  }
}
