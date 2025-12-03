import { Routes } from '@angular/router';

export const TODOS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./todos.component').then(m => m.TodosComponent)
  }
];
