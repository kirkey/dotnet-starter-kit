import { Routes } from '@angular/router';

export const MULTITENANCY_ROUTES: Routes = [
  {
    path: 'tenants',
    loadComponent: () => import('./tenants/tenants.component').then(m => m.TenantsComponent)
  },
  {
    path: '',
    redirectTo: 'tenants',
    pathMatch: 'full'
  }
];
