import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./layout/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
        title: 'Dashboard'
      },
      {
        path: 'home',
        loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent),
        title: 'Home'
      },
      {
        path: 'tenants',
        loadChildren: () => import('./features/multitenancy/multitenancy.routes').then(m => m.MULTITENANCY_ROUTES),
        title: 'Tenants'
      },
      {
        path: 'identity',
        loadChildren: () => import('./features/identity/identity.routes').then(m => m.IDENTITY_ROUTES),
        title: 'Identity'
      },
      {
        path: 'catalog',
        children: [
          {
            path: '',
            redirectTo: 'products',
            pathMatch: 'full'
          },
          {
            path: 'products',
            loadComponent: () => import('./features/catalog/products/products.component').then(m => m.ProductsComponent),
            title: 'Products'
          }
        ]
      },
      {
        path: 'todos',
        loadComponent: () => import('./features/todos/todos.component').then(m => m.TodosComponent),
        title: 'Todos'
      },
      {
        path: 'accounting',
        loadChildren: () => import('./features/accounting/accounting.routes').then(m => m.ACCOUNTING_ROUTES),
        title: 'Accounting'
      }
    ]
  },
  {
    path: 'auth',
    loadComponent: () => import('./layout/auth-layout/auth-layout.component').then(m => m.AuthLayoutComponent),
    children: [
      {
        path: 'login',
        loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent),
        title: 'Login'
      },
      {
        path: 'register',
        loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent),
        title: 'Register'
      },
      {
        path: 'forgot-password',
        loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent),
        title: 'Forgot Password'
      },
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '**',
    loadComponent: () => import('./layout/not-found/not-found.component').then(m => m.NotFoundComponent),
    title: 'Page Not Found'
  }
];
