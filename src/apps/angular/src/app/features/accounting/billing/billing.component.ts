import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiClient } from '@app/api';

@Component({
  selector: 'app-billing',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="page-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <div class="header-row">
              <span>Billing</span>
              <button mat-raised-button color="primary">
                <mat-icon>add</mat-icon>
                Add
              </button>
            </div>
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          @if (loading()) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          } @else {
            <div class="table-container">
              <p>Feature coming soon: Billing management</p>
              <p class="hint">This component will integrate with the Billing API endpoint</p>
            </div>
          }
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 1.5rem;
    }

    .header-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }

    .loading-container {
      display: flex;
      justify-content: center;
      padding: 3rem;
    }

    .table-container {
      padding: 2rem;
      text-align: center;
    }

    .hint {
      color: #666;
      font-size: 0.9rem;
      margin-top: 1rem;
    }
  `]
})
export class BillingComponent implements OnInit {
  private apiClient = inject(ApiClient);
  loading = signal(false);

  ngOnInit(): void {
    // TODO: Implement data loading using apiClient
    console.log('Billing component initialized');
  }
}
