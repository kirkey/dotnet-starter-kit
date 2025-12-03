import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, BehaviorSubject } from 'rxjs';
import { environment } from '@env/environment';
import { TokenRequest, TokenResponse, RefreshTokenRequest, RegisterRequest } from '../models/auth.model';
import { User } from '../models/user.model';

const TOKEN_KEY = 'auth_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const USER_KEY = 'user_data';
const TENANT_KEY = 'tenant_id';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Token endpoint is 'api/token' (not 'api/v1/tokens')
  private readonly tokenUrl = '/api/token';
  private readonly apiUrl = environment.apiUrl;
  
  // Signals for reactive state
  private _isAuthenticated = signal<boolean>(this.hasValidToken());
  private _currentUser = signal<User | null>(this.loadUserFromStorage());
  private _currentTenant = signal<string>(this.loadTenantFromStorage());
  
  // Computed signals
  readonly isAuthenticated = this._isAuthenticated.asReadonly();
  readonly currentUser = this._currentUser.asReadonly();
  readonly currentTenant = this._currentTenant.asReadonly();
  
  readonly userFullName = computed(() => {
    const user = this._currentUser();
    return user ? `${user.firstName} ${user.lastName}` : '';
  });
  
  readonly userInitials = computed(() => {
    const user = this._currentUser();
    if (!user) return '';
    return `${user.firstName?.charAt(0) || ''}${user.lastName?.charAt(0) || ''}`.toUpperCase();
  });

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  login(credentials: TokenRequest, tenant: string = environment.defaultTenant): Observable<TokenResponse> {
    // Set tenant FIRST so the interceptor picks it up
    this.setTenant(tenant);
    
    // deviceType is passed in query parameter (API requirement)
    const deviceType = credentials.deviceType || 'Web';
    const url = `${this.tokenUrl}?deviceType=${deviceType}`;
    
    // Send only email and password in body; tenant will be added by interceptor
    const body = {
      email: credentials.email,
      password: credentials.password
    };
    
    return this.http.post<TokenResponse>(url, body).pipe(
      tap(response => this.handleAuthResponse(response)),
      catchError(error => {
        this.clearAuth();
        return throwError(() => error);
      })
    );
  }

  register(request: RegisterRequest): Observable<string> {
    return this.http.post<string>(`${environment.apiUrl}/users/self-register`, request).pipe(
      catchError(error => throwError(() => error))
    );
  }

  logout(): void {
    this.clearAuth();
    this.router.navigate(['/auth/login']);
  }

  refreshToken(): Observable<TokenResponse> {
    const token = this.getToken();
    const refreshToken = this.getRefreshToken();
    
    if (!token || !refreshToken) {
      return throwError(() => new Error('No tokens available'));
    }

    const request: RefreshTokenRequest = { token, refreshToken };
    
    return this.http.post<TokenResponse>(`${this.tokenUrl}/refresh`, request).pipe(
      tap(response => this.handleAuthResponse(response)),
      catchError(error => {
        this.clearAuth();
        return throwError(() => error);
      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  getTenant(): string {
    return this._currentTenant();
  }

  setTenant(tenant: string): void {
    localStorage.setItem(TENANT_KEY, tenant);
    this._currentTenant.set(tenant);
  }

  setUser(user: User): void {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
    this._currentUser.set(user);
  }

  private handleAuthResponse(response: TokenResponse): void {
    localStorage.setItem(TOKEN_KEY, response.token);
    localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
    this._isAuthenticated.set(true);
    
    // Decode JWT to get user info
    const user = this.decodeToken(response.token);
    if (user) {
      this.setUser(user);
    }
  }

  private clearAuth(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this._isAuthenticated.set(false);
    this._currentUser.set(null);
  }

  private hasValidToken(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const expiry = payload.exp * 1000;
      return Date.now() < expiry;
    } catch {
      return false;
    }
  }

  private loadUserFromStorage(): User | null {
    const userData = localStorage.getItem(USER_KEY);
    return userData ? JSON.parse(userData) : null;
  }

  private loadTenantFromStorage(): string {
    return localStorage.getItem(TENANT_KEY) || environment.defaultTenant;
  }

  private decodeToken(token: string): User | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload.uid || payload.sub,
        userName: payload.name || payload.email,
        firstName: payload.firstName || payload.given_name || '',
        lastName: payload.lastName || payload.family_name || '',
        email: payload.email,
        imageUrl: payload.image_url,
        isActive: true,
        emailConfirmed: payload.email_verified === 'true'
      };
    } catch {
      return null;
    }
  }
}
