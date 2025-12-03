import { Routes } from '@angular/router';

export const IDENTITY_ROUTES: Routes = [
  {
    path: 'users',
    loadComponent: () => import('./users/users.component').then(m => m.UsersComponent)
  },
  {
    path: 'users/:id/profile',
    loadComponent: () => import('./users/user-profile/user-profile.component').then(m => m.UserProfileComponent)
  },
  {
    path: 'users/:id/roles',
    loadComponent: () => import('./users/user-roles/user-roles.component').then(m => m.UserRolesComponent)
  },
  {
    path: 'roles',
    loadComponent: () => import('./roles/roles.component').then(m => m.RolesComponent)
  },
  {
    path: 'roles/:id/permissions',
    loadComponent: () => import('./roles/role-permissions/role-permissions.component').then(m => m.RolePermissionsComponent)
  },
  {
    path: '',
    redirectTo: 'users',
    pathMatch: 'full'
  }
];
