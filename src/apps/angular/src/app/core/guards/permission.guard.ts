import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PermissionService } from '../services/permission.service';

/**
 * Route guard that checks if the user has the required permission.
 * Usage in route config:
 * {
 *   path: 'products',
 *   component: ProductsComponent,
 *   canActivate: [permissionGuard],
 *   data: { action: 'View', resource: 'Products' }
 * }
 */
export const permissionGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PermissionService);
  const router = inject(Router);
  
  const action = route.data['action'] as string;
  const resource = route.data['resource'] as string;
  
  if (!action || !resource) {
    console.warn('Permission guard requires action and resource in route data');
    return true; // Allow if no permission specified
  }
  
  if (permissionService.hasPermission(action, resource)) {
    return true;
  }
  
  // Redirect to unauthorized page
  router.navigate(['/unauthorized']);
  return false;
};

/**
 * Route guard that checks if user has ANY of the required permissions.
 * Usage in route config:
 * {
 *   path: 'dashboard',
 *   component: DashboardComponent,
 *   canActivate: [hasAnyPermissionGuard],
 *   data: { 
 *     permissions: [
 *       { action: 'View', resource: 'Dashboard' },
 *       { action: 'View', resource: 'Analytics' }
 *     ]
 *   }
 * }
 */
export const hasAnyPermissionGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PermissionService);
  const router = inject(Router);
  
  const permissions = route.data['permissions'] as Array<{ action: string; resource: string }>;
  
  if (!permissions || permissions.length === 0) {
    console.warn('hasAnyPermission guard requires permissions array in route data');
    return true;
  }
  
  const hasPermission = permissions.some(p => 
    permissionService.hasPermission(p.action, p.resource)
  );
  
  if (hasPermission) {
    return true;
  }
  
  router.navigate(['/unauthorized']);
  return false;
};

/**
 * Route guard that checks if user has ALL of the required permissions.
 * Usage in route config:
 * {
 *   path: 'admin',
 *   component: AdminComponent,
 *   canActivate: [hasAllPermissionsGuard],
 *   data: { 
 *     permissions: [
 *       { action: 'View', resource: 'Users' },
 *       { action: 'View', resource: 'Roles' }
 *     ]
 *   }
 * }
 */
export const hasAllPermissionsGuard: CanActivateFn = (route, state) => {
  const permissionService = inject(PermissionService);
  const router = inject(Router);
  
  const permissions = route.data['permissions'] as Array<{ action: string; resource: string }>;
  
  if (!permissions || permissions.length === 0) {
    console.warn('hasAllPermissions guard requires permissions array in route data');
    return true;
  }
  
  const hasAllPermissions = permissions.every(p => 
    permissionService.hasPermission(p.action, p.resource)
  );
  
  if (hasAllPermissions) {
    return true;
  }
  
  router.navigate(['/unauthorized']);
  return false;
};

