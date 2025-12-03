import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '@core/services/auth.service';
import { NotificationService } from '@core/services/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="login-container">
      <h2 class="login-title">Welcome Back</h2>
      <p class="login-subtitle">Sign in to continue to your account</p>
      
      <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="login-form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Email</mat-label>
          <input matInput 
                 type="email" 
                 formControlName="email" 
                 placeholder="Enter your email"
                 autocomplete="email">
          <mat-icon matPrefix>email</mat-icon>
          @if (loginForm.get('email')?.hasError('required') && loginForm.get('email')?.touched) {
            <mat-error>Email is required</mat-error>
          }
          @if (loginForm.get('email')?.hasError('email') && loginForm.get('email')?.touched) {
            <mat-error>Please enter a valid email</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Password</mat-label>
          <input matInput 
                 [type]="hidePassword() ? 'password' : 'text'" 
                 formControlName="password" 
                 placeholder="Enter your password"
                 autocomplete="current-password">
          <mat-icon matPrefix>lock</mat-icon>
          <button mat-icon-button matSuffix (click)="togglePassword()" type="button">
            <mat-icon>{{ hidePassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
          </button>
          @if (loginForm.get('password')?.hasError('required') && loginForm.get('password')?.touched) {
            <mat-error>Password is required</mat-error>
          }
          @if (loginForm.get('password')?.hasError('minlength') && loginForm.get('password')?.touched) {
            <mat-error>Password must be at least 6 characters</mat-error>
          }
        </mat-form-field>

        <div class="form-extras">
          <mat-checkbox formControlName="rememberMe" color="primary">
            Remember me
          </mat-checkbox>
          <a routerLink="/auth/forgot-password" class="forgot-link">
            Forgot password?
          </a>
        </div>

        <button mat-raised-button 
                color="primary" 
                type="submit" 
                class="submit-btn"
                [disabled]="isLoading() || loginForm.invalid">
          @if (isLoading()) {
            <mat-spinner diameter="20"></mat-spinner>
            <span>Signing in...</span>
          } @else {
            <mat-icon>login</mat-icon>
            <span>Sign In</span>
          }
        </button>
      </form>

      <div class="divider">
        <span>or</span>
      </div>

      <p class="register-prompt">
        Don't have an account?
        <a routerLink="/auth/register" class="register-link">Sign up</a>
      </p>

      <div class="demo-credentials">
        <p class="demo-title">Demo Credentials:</p>
        <p class="demo-info">Email: admin&#64;root.com</p>
        <p class="demo-info">Password: 123Pa$$word!</p>
        <button mat-stroked-button color="accent" (click)="fillDemoCredentials()" type="button">
          Use Demo Account
        </button>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
      text-align: center;
    }

    .login-title {
      font-size: 24px;
      font-weight: 600;
      margin: 0 0 8px;
      color: var(--text-primary);
    }

    .login-subtitle {
      font-size: 14px;
      color: var(--text-secondary);
      margin: 0 0 24px;
    }

    .login-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    .form-extras {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin: -8px 0 8px;
    }

    .forgot-link {
      font-size: 13px;
      color: var(--primary-color);
      text-decoration: none;
    }

    .forgot-link:hover {
      text-decoration: underline;
    }

    .submit-btn {
      height: 48px;
      font-size: 16px;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
    }

    .submit-btn mat-spinner {
      display: inline-block;
    }

    .divider {
      display: flex;
      align-items: center;
      margin: 24px 0;
    }

    .divider::before,
    .divider::after {
      content: '';
      flex: 1;
      height: 1px;
      background: var(--border-color);
    }

    .divider span {
      padding: 0 16px;
      color: var(--text-secondary);
      font-size: 13px;
    }

    .register-prompt {
      font-size: 14px;
      color: var(--text-secondary);
      margin: 0;
    }

    .register-link {
      color: var(--primary-color);
      text-decoration: none;
      font-weight: 500;
    }

    .register-link:hover {
      text-decoration: underline;
    }

    .demo-credentials {
      margin-top: 24px;
      padding: 16px;
      background: rgba(var(--primary-color-rgb, 89, 74, 226), 0.05);
      border-radius: 8px;
      border: 1px dashed var(--primary-color);
    }

    .demo-title {
      font-size: 13px;
      font-weight: 500;
      color: var(--text-primary);
      margin: 0 0 8px;
    }

    .demo-info {
      font-size: 12px;
      color: var(--text-secondary);
      margin: 4px 0;
      font-family: monospace;
    }

    .demo-credentials button {
      margin-top: 12px;
    }
  `]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private notification = inject(NotificationService);

  hidePassword = signal(true);
  isLoading = signal(false);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rememberMe: [false]
  });

  togglePassword(): void {
    this.hidePassword.update(v => !v);
  }

  fillDemoCredentials(): void {
    this.loginForm.patchValue({
      email: 'admin@root.com',
      password: '123Pa$$word!'
    });
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    
    try {
      const { email, password } = this.loginForm.value;
      await this.authService.login({ email, password }).toPromise();
      this.notification.success('Welcome back!');
      this.router.navigate(['/']);
    } catch (error: any) {
      this.notification.error(error?.message || 'Login failed. Please try again.');
    } finally {
      this.isLoading.set(false);
    }
  }
}
