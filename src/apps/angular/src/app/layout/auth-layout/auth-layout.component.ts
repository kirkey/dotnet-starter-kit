import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule],
  template: `
    <div class="auth-layout">
      <div class="auth-background">
        <div class="auth-overlay"></div>
      </div>
      
      <div class="auth-content">
        <div class="auth-logo">
          <img src="assets/images/logo.svg" alt="Logo" class="logo-img" />
          <h1 class="app-name">FSH Starter Kit</h1>
        </div>
        
        <mat-card class="auth-card">
          <router-outlet></router-outlet>
        </mat-card>
        
        <p class="auth-footer">
          &copy; {{ currentYear }} Full Stack Hero. All rights reserved.
        </p>
      </div>
    </div>
  `,
  styles: [`
    .auth-layout {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      position: relative;
    }

    .auth-background {
      position: absolute;
      inset: 0;
      background: linear-gradient(135deg, #594ae2 0%, #7c3aed 50%, #a855f7 100%);
      z-index: 0;
    }

    .auth-overlay {
      position: absolute;
      inset: 0;
      background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="grain" width="100" height="100" patternUnits="userSpaceOnUse"><circle cx="25" cy="25" r="1" fill="white" opacity="0.1"/><circle cx="75" cy="75" r="1" fill="white" opacity="0.1"/><circle cx="50" cy="10" r="0.5" fill="white" opacity="0.1"/><circle cx="10" cy="60" r="0.5" fill="white" opacity="0.1"/></pattern></defs><rect fill="url(%23grain)" width="100" height="100"/></svg>');
    }

    .auth-content {
      position: relative;
      z-index: 1;
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 24px;
      width: 100%;
      max-width: 450px;
    }

    .auth-logo {
      display: flex;
      flex-direction: column;
      align-items: center;
      margin-bottom: 24px;
    }

    .logo-img {
      width: 80px;
      height: 80px;
      margin-bottom: 16px;
    }

    .app-name {
      color: white;
      font-size: 28px;
      font-weight: 600;
      margin: 0;
      text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .auth-card {
      width: 100%;
      padding: 32px;
      border-radius: 16px;
      box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
    }

    .auth-footer {
      color: rgba(255, 255, 255, 0.8);
      font-size: 12px;
      margin-top: 24px;
    }

    @media (max-width: 480px) {
      .auth-content {
        padding: 16px;
      }

      .auth-card {
        padding: 24px;
      }

      .app-name {
        font-size: 24px;
      }
    }
  `]
})
export class AuthLayoutComponent {
  currentYear = new Date().getFullYear();
}
