import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { User, CreateUserRequest, UpdateUserRequest } from '../models/user.model';
import { UserRole, AssignUserRolesRequest } from '../models/role.model';

export interface ToggleUserStatusRequest {
  userId: string;
  activateUser: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/users`;

  /**
   * Get all users
   */
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  /**
   * Get a specific user by ID
   */
  getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  /**
   * Create a new user (admin)
   */
  createUser(request: CreateUserRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, request);
  }

  /**
   * Register a new user (self-registration)
   */
  registerUser(request: CreateUserRequest): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/self-register`, request);
  }

  /**
   * Update user profile
   */
  updateUser(id: string, request: UpdateUserRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  /**
   * Delete a user
   */
  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  /**
   * Toggle user status (activate/deactivate)
   */
  toggleUserStatus(id: string, activateUser: boolean): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/toggle-status`, { 
      userId: id,
      activateUser 
    });
  }

  /**
   * Get user roles
   */
  getUserRoles(userId: string): Observable<UserRole[]> {
    return this.http.get<UserRole[]>(`${this.apiUrl}/${userId}/roles`);
  }

  /**
   * Assign roles to user
   */
  assignRolesToUser(userId: string, request: AssignUserRolesRequest): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/${userId}/roles`, request);
  }

  /**
   * Get current user's permissions
   */
  getMyPermissions(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/personal/permissions`);
  }

  /**
   * Get current user's profile
   */
  getMyProfile(): Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}/personal/profile`);
  }

  /**
   * Update current user's profile
   */
  updateMyProfile(request: UpdateUserRequest): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/personal/profile`, request);
  }

  /**
   * Change current user's password
   */
  changePassword(currentPassword: string, newPassword: string, confirmNewPassword: string): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/personal/change-password`, {
      password: currentPassword,
      newPassword,
      confirmNewPassword
    });
  }
}
