import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authService = inject(AuthService);
  
  const token = authService.getToken();
  const tenant = authService.getTenant();
  
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        tenant: tenant
      }
    });
  } else if (tenant) {
    req = req.clone({
      setHeaders: {
        tenant: tenant
      }
    });
  }
  
  return next(req);
};
