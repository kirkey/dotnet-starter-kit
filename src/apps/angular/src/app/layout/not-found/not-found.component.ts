import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, MatIconModule],
  template: `
    <div class="not-found-container">
      <div class="not-found-content">
        <div class="error-code">404</div>
        <h1 class="error-title">Page Not Found</h1>
        <p class="error-message">
          Oops! The page you're looking for doesn't exist or has been moved.
        </p>
        
        <div class="error-actions">
          <a mat-raised-button color="primary" routerLink="/">
            <mat-icon>home</mat-icon>
            Go to Home
          </a>
          <button mat-stroked-button (click)="goBack()">
            <mat-icon>arrow_back</mat-icon>
            Go Back
          </button>
        </div>
      </div>
      
      <div class="error-illustration">
        <svg viewBox="0 0 200 200" xmlns="http://www.w3.org/2000/svg">
          <circle cx="100" cy="100" r="80" fill="none" stroke="currentColor" stroke-width="2" opacity="0.2"/>
          <circle cx="100" cy="100" r="60" fill="none" stroke="currentColor" stroke-width="2" opacity="0.3"/>
          <circle cx="100" cy="100" r="40" fill="none" stroke="currentColor" stroke-width="2" opacity="0.4"/>
          <text x="100" y="115" text-anchor="middle" font-size="40" font-weight="bold" fill="currentColor">?</text>
        </svg>
      </div>
    </div>
  `,
  styles: [`
    .not-found-container {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 24px;
      text-align: center;
      background: linear-gradient(180deg, var(--background-color) 0%, var(--surface-color) 100%);
    }

    .not-found-content {
      max-width: 500px;
    }

    .error-code {
      font-size: 120px;
      font-weight: 800;
      line-height: 1;
      background: linear-gradient(135deg, var(--primary-color) 0%, var(--accent-color) 100%);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      background-clip: text;
      margin-bottom: 16px;
    }

    .error-title {
      font-size: 32px;
      font-weight: 600;
      margin: 0 0 16px;
      color: var(--text-primary);
    }

    .error-message {
      font-size: 16px;
      color: var(--text-secondary);
      margin: 0 0 32px;
      line-height: 1.6;
    }

    .error-actions {
      display: flex;
      gap: 16px;
      justify-content: center;
      flex-wrap: wrap;
    }

    .error-actions button,
    .error-actions a {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .error-illustration {
      margin-top: 48px;
      width: 200px;
      height: 200px;
      color: var(--primary-color);
      opacity: 0.5;
      animation: float 3s ease-in-out infinite;
    }

    @keyframes float {
      0%, 100% {
        transform: translateY(0);
      }
      50% {
        transform: translateY(-20px);
      }
    }

    @media (max-width: 480px) {
      .error-code {
        font-size: 80px;
      }

      .error-title {
        font-size: 24px;
      }

      .error-actions {
        flex-direction: column;
      }
    }
  `]
})
export class NotFoundComponent {
  goBack(): void {
    window.history.back();
  }
}
