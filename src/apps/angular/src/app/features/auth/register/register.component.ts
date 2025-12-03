import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatStepperModule } from '@angular/material/stepper';
import { AuthService } from '@core/services/auth.service';
import { NotificationService } from '@core/services/notification.service';

@Component({
  selector: 'app-register',
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
    MatProgressSpinnerModule,
    MatStepperModule
  ],
  template: `
    <div class="register-container">
      <h2 class="register-title">Create Account</h2>
      <p class="register-subtitle">Join us today and get started</p>
      
      <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" class="register-form">
        <div class="name-row">
          <mat-form-field appearance="outline">
            <mat-label>First Name</mat-label>
            <input matInput 
                   formControlName="firstName" 
                   placeholder="First name"
                   autocomplete="given-name">
            <mat-icon matPrefix>person</mat-icon>
            @if (registerForm.get('firstName')?.hasError('required') && registerForm.get('firstName')?.touched) {
              <mat-error>First name is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Last Name</mat-label>
            <input matInput 
                   formControlName="lastName" 
                   placeholder="Last name"
                   autocomplete="family-name">
            <mat-icon matPrefix>person_outline</mat-icon>
            @if (registerForm.get('lastName')?.hasError('required') && registerForm.get('lastName')?.touched) {
              <mat-error>Last name is required</mat-error>
            }
          </mat-form-field>
        </div>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Email</mat-label>
          <input matInput 
                 type="email" 
                 formControlName="email" 
                 placeholder="Enter your email"
                 autocomplete="email">
          <mat-icon matPrefix>email</mat-icon>
          @if (registerForm.get('email')?.hasError('required') && registerForm.get('email')?.touched) {
            <mat-error>Email is required</mat-error>
          }
          @if (registerForm.get('email')?.hasError('email') && registerForm.get('email')?.touched) {
            <mat-error>Please enter a valid email</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Username</mat-label>
          <input matInput 
                 formControlName="userName" 
                 placeholder="Choose a username"
                 autocomplete="username">
          <mat-icon matPrefix>alternate_email</mat-icon>
          @if (registerForm.get('userName')?.hasError('required') && registerForm.get('userName')?.touched) {
            <mat-error>Username is required</mat-error>
          }
          @if (registerForm.get('userName')?.hasError('minlength') && registerForm.get('userName')?.touched) {
            <mat-error>Username must be at least 3 characters</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Phone Number</mat-label>
          <input matInput 
                 type="tel" 
                 formControlName="phoneNumber" 
                 placeholder="Enter your phone number"
                 autocomplete="tel">
          <mat-icon matPrefix>phone</mat-icon>
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Password</mat-label>
          <input matInput 
                 [type]="hidePassword() ? 'password' : 'text'" 
                 formControlName="password" 
                 placeholder="Create a password"
                 autocomplete="new-password">
          <mat-icon matPrefix>lock</mat-icon>
          <button mat-icon-button matSuffix (click)="togglePassword()" type="button">
            <mat-icon>{{ hidePassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
          </button>
          @if (registerForm.get('password')?.hasError('required') && registerForm.get('password')?.touched) {
            <mat-error>Password is required</mat-error>
          }
          @if (registerForm.get('password')?.hasError('minlength') && registerForm.get('password')?.touched) {
            <mat-error>Password must be at least 8 characters</mat-error>
          }
          @if (registerForm.get('password')?.hasError('pattern') && registerForm.get('password')?.touched) {
            <mat-error>Password must contain uppercase, lowercase, number, and special character</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Confirm Password</mat-label>
          <input matInput 
                 [type]="hideConfirmPassword() ? 'password' : 'text'" 
                 formControlName="confirmPassword" 
                 placeholder="Confirm your password"
                 autocomplete="new-password">
          <mat-icon matPrefix>lock_outline</mat-icon>
          <button mat-icon-button matSuffix (click)="toggleConfirmPassword()" type="button">
            <mat-icon>{{ hideConfirmPassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
          </button>
          @if (registerForm.get('confirmPassword')?.hasError('required') && registerForm.get('confirmPassword')?.touched) {
            <mat-error>Please confirm your password</mat-error>
          }
          @if (registerForm.get('confirmPassword')?.hasError('passwordMismatch') && registerForm.get('confirmPassword')?.touched) {
            <mat-error>Passwords do not match</mat-error>
          }
        </mat-form-field>

        <div class="terms-checkbox">
          <mat-checkbox formControlName="acceptTerms" color="primary">
            I agree to the <a href="/terms" target="_blank">Terms of Service</a> 
            and <a href="/privacy" target="_blank">Privacy Policy</a>
          </mat-checkbox>
          @if (registerForm.get('acceptTerms')?.hasError('requiredTrue') && registerForm.get('acceptTerms')?.touched) {
            <mat-error class="terms-error">You must accept the terms to continue</mat-error>
          }
        </div>

        <button mat-raised-button 
                color="primary" 
                type="submit" 
                class="submit-btn"
                [disabled]="isLoading() || registerForm.invalid">
          @if (isLoading()) {
            <mat-spinner diameter="20"></mat-spinner>
            <span>Creating account...</span>
          } @else {
            <mat-icon>person_add</mat-icon>
            <span>Create Account</span>
          }
        </button>
      </form>

      <div class="divider">
        <span>or</span>
      </div>

      <p class="login-prompt">
        Already have an account?
        <a routerLink="/auth/login" class="login-link">Sign in</a>
      </p>
    </div>
  `,
  styles: [`
    .register-container {
      text-align: center;
    }

    .register-title {
      font-size: 24px;
      font-weight: 600;
      margin: 0 0 8px;
      color: var(--text-primary);
    }

    .register-subtitle {
      font-size: 14px;
      color: var(--text-secondary);
      margin: 0 0 24px;
    }

    .register-form {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .name-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 12px;
    }

    .full-width {
      width: 100%;
    }

    .terms-checkbox {
      text-align: left;
      margin: 8px 0;
    }

    .terms-checkbox a {
      color: var(--primary-color);
      text-decoration: none;
    }

    .terms-checkbox a:hover {
      text-decoration: underline;
    }

    .terms-error {
      font-size: 12px;
      display: block;
      margin-top: 4px;
    }

    .submit-btn {
      height: 48px;
      font-size: 16px;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
      margin-top: 8px;
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

    .login-prompt {
      font-size: 14px;
      color: var(--text-secondary);
      margin: 0;
    }

    .login-link {
      color: var(--primary-color);
      text-decoration: none;
      font-weight: 500;
    }

    .login-link:hover {
      text-decoration: underline;
    }

    @media (max-width: 480px) {
      .name-row {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private notification = inject(NotificationService);

  hidePassword = signal(true);
  hideConfirmPassword = signal(true);
  isLoading = signal(false);

  // Password pattern: at least 1 uppercase, 1 lowercase, 1 number, 1 special char
  private passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/;

  registerForm: FormGroup = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    userName: ['', [Validators.required, Validators.minLength(3)]],
    phoneNumber: [''],
    password: ['', [
      Validators.required, 
      Validators.minLength(8),
      Validators.pattern(this.passwordPattern)
    ]],
    confirmPassword: ['', [Validators.required, this.passwordMatchValidator.bind(this)]],
    acceptTerms: [false, Validators.requiredTrue]
  });

  constructor() {
    // Update confirmPassword validation when password changes
    this.registerForm.get('password')?.valueChanges.subscribe(() => {
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = this.registerForm?.get('password')?.value;
    const confirmPassword = control.value;
    
    if (password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }

  togglePassword(): void {
    this.hidePassword.update(v => !v);
  }

  toggleConfirmPassword(): void {
    this.hideConfirmPassword.update(v => !v);
  }

  async onSubmit(): Promise<void> {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    
    try {
      const { confirmPassword, acceptTerms, ...registerData } = this.registerForm.value;
      await this.authService.register(registerData).toPromise();
      this.notification.success('Account created successfully! Please sign in.');
      this.router.navigate(['/auth/login']);
    } catch (error: any) {
      this.notification.error(error?.message || 'Registration failed. Please try again.');
    } finally {
      this.isLoading.set(false);
    }
  }
}
