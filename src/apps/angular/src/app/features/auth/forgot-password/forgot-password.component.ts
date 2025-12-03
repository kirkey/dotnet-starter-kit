import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NotificationService } from '@core/services/notification.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="forgot-password-container">
      @if (!emailSent()) {
        <h2 class="title">Forgot Password?</h2>
        <p class="subtitle">
          Enter your email address and we'll send you instructions to reset your password.
        </p>
        
        <form [formGroup]="forgotForm" (ngSubmit)="onSubmit()" class="forgot-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Email</mat-label>
            <input matInput 
                   type="email" 
                   formControlName="email" 
                   placeholder="Enter your email"
                   autocomplete="email">
            <mat-icon matPrefix>email</mat-icon>
            @if (forgotForm.get('email')?.hasError('required') && forgotForm.get('email')?.touched) {
              <mat-error>Email is required</mat-error>
            }
            @if (forgotForm.get('email')?.hasError('email') && forgotForm.get('email')?.touched) {
              <mat-error>Please enter a valid email</mat-error>
            }
          </mat-form-field>

          <button mat-raised-button 
                  color="primary" 
                  type="submit" 
                  class="submit-btn"
                  [disabled]="isLoading() || forgotForm.invalid">
            @if (isLoading()) {
              <mat-spinner diameter="20"></mat-spinner>
              <span>Sending...</span>
            } @else {
              <mat-icon>send</mat-icon>
              <span>Send Reset Link</span>
            }
          </button>
        </form>
      } @else {
        <div class="success-state">
          <mat-icon class="success-icon">mark_email_read</mat-icon>
          <h2 class="title">Check Your Email</h2>
          <p class="subtitle">
            We've sent password reset instructions to<br>
            <strong>{{ submittedEmail() }}</strong>
          </p>
          <p class="info-text">
            Didn't receive the email? Check your spam folder or
            <button mat-button color="primary" (click)="resetForm()">try again</button>
          </p>
        </div>
      }

      <div class="back-to-login">
        <a routerLink="/auth/login" class="back-link">
          <mat-icon>arrow_back</mat-icon>
          Back to Sign In
        </a>
      </div>
    </div>
  `,
  styles: [`
    .forgot-password-container {
      text-align: center;
    }

    .title {
      font-size: 24px;
      font-weight: 600;
      margin: 0 0 8px;
      color: var(--text-primary);
    }

    .subtitle {
      font-size: 14px;
      color: var(--text-secondary);
      margin: 0 0 24px;
      line-height: 1.5;
    }

    .forgot-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .full-width {
      width: 100%;
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

    .success-state {
      padding: 24px 0;
    }

    .success-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: var(--primary-color);
      margin-bottom: 16px;
    }

    .info-text {
      font-size: 13px;
      color: var(--text-secondary);
      margin: 16px 0 0;
    }

    .info-text button {
      vertical-align: baseline;
      padding: 0 4px;
      min-width: auto;
    }

    .back-to-login {
      margin-top: 32px;
      padding-top: 24px;
      border-top: 1px solid var(--border-color);
    }

    .back-link {
      display: inline-flex;
      align-items: center;
      gap: 8px;
      color: var(--text-secondary);
      text-decoration: none;
      font-size: 14px;
      transition: color 0.2s;
    }

    .back-link:hover {
      color: var(--primary-color);
    }

    .back-link mat-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
    }
  `]
})
export class ForgotPasswordComponent {
  private fb = inject(FormBuilder);
  private notification = inject(NotificationService);

  isLoading = signal(false);
  emailSent = signal(false);
  submittedEmail = signal('');

  forgotForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  async onSubmit(): Promise<void> {
    if (this.forgotForm.invalid) {
      this.forgotForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    
    try {
      // Simulate API call - replace with actual service
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      this.submittedEmail.set(this.forgotForm.value.email);
      this.emailSent.set(true);
    } catch (error: any) {
      this.notification.error(error?.message || 'Failed to send reset email. Please try again.');
    } finally {
      this.isLoading.set(false);
    }
  }

  resetForm(): void {
    this.emailSent.set(false);
    this.submittedEmail.set('');
    this.forgotForm.reset();
  }
}
