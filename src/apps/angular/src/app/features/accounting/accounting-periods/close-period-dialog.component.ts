import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';

import { AccountingService } from '@core/services/accounting.service';
import { AccountingPeriod } from '@core/models/accounting.model';

@Component({
  selector: 'app-close-period-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDividerModule
  ],
  template: `
    <h2 mat-dialog-title>
      <mat-icon class="warning-icon">lock</mat-icon>
      Close Accounting Period
    </h2>

    <mat-dialog-content>
      <div class="period-info">
        <div class="info-item">
          <span class="label">Period:</span>
          <span class="value">{{ period.periodName }}</span>
        </div>
        <div class="info-item">
          <span class="label">Fiscal Year:</span>
          <span class="value">{{ period.fiscalYear }}</span>
        </div>
        <div class="info-item">
          <span class="label">Date Range:</span>
          <span class="value">
            {{ period.startDate | date:'mediumDate' }} - {{ period.endDate | date:'mediumDate' }}
          </span>
        </div>
      </div>

      <div class="warning-message">
        <mat-icon>warning</mat-icon>
        <span>
          Closing this period will prevent any new transactions from being posted to it.
          Make sure all entries are complete and accurate before proceeding.
        </span>
      </div>

      <mat-divider></mat-divider>

      <div class="checklist-section">
        <h4>Pre-Close Checklist</h4>
        <div class="checklist-items">
          <mat-checkbox [(ngModel)]="checks.allEntriesPosted">
            All journal entries have been posted
          </mat-checkbox>
          <mat-checkbox [(ngModel)]="checks.reconciliationsComplete">
            Bank reconciliations are complete
          </mat-checkbox>
          <mat-checkbox [(ngModel)]="checks.adjustingEntriesMade">
            Adjusting entries have been made
          </mat-checkbox>
          <mat-checkbox [(ngModel)]="checks.trialBalanceBalanced">
            Trial balance is balanced
          </mat-checkbox>
          <mat-checkbox [(ngModel)]="checks.reportsGenerated">
            Financial reports have been generated
          </mat-checkbox>
        </div>
      </div>

      <mat-divider></mat-divider>

      <form [formGroup]="form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Closing Notes</mat-label>
          <textarea matInput formControlName="notes" rows="3"
                    placeholder="Optional notes about the period close"></textarea>
        </mat-form-field>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="warn"
              [disabled]="!allChecksComplete() || closing()"
              (click)="closePeriod()">
        @if (closing()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          Close Period
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    h2 {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .warning-icon {
      color: #f57c00;
    }

    mat-dialog-content {
      min-width: 450px;
    }

    .period-info {
      background-color: #f5f5f5;
      padding: 16px;
      border-radius: 8px;
      margin-bottom: 16px;
    }

    .info-item {
      display: flex;
      justify-content: space-between;
      margin-bottom: 8px;
    }

    .info-item:last-child {
      margin-bottom: 0;
    }

    .label {
      color: #666;
    }

    .value {
      font-weight: 500;
    }

    .warning-message {
      display: flex;
      align-items: flex-start;
      gap: 8px;
      padding: 12px;
      background-color: #fff3e0;
      color: #f57c00;
      border-radius: 4px;
      margin-bottom: 16px;
    }

    .warning-message mat-icon {
      flex-shrink: 0;
    }

    mat-divider {
      margin: 16px 0;
    }

    .checklist-section h4 {
      margin: 0 0 12px;
    }

    .checklist-items {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .full-width {
      width: 100%;
      margin-top: 16px;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class ClosePeriodDialogComponent {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ClosePeriodDialogComponent>);
  private data = inject<{ period: AccountingPeriod }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form: FormGroup;
  closing = signal(false);

  checks = {
    allEntriesPosted: false,
    reconciliationsComplete: false,
    adjustingEntriesMade: false,
    trialBalanceBalanced: false,
    reportsGenerated: false
  };

  get period(): AccountingPeriod {
    return this.data.period;
  }

  constructor() {
    this.form = this.fb.group({
      notes: ['']
    });
  }

  allChecksComplete(): boolean {
    return this.checks.allEntriesPosted &&
           this.checks.reconciliationsComplete &&
           this.checks.adjustingEntriesMade &&
           this.checks.trialBalanceBalanced &&
           this.checks.reportsGenerated;
  }

  closePeriod(): void {
    if (!this.allChecksComplete()) return;

    this.closing.set(true);
    this.accountingService.closePeriod({
      periodId: this.period.id,
      reason: this.form.value.notes
    }).subscribe({
      next: () => {
        this.snackBar.open('Period closed successfully', 'Close', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error closing period:', error);
        this.snackBar.open('Failed to close period', 'Close', { duration: 3000 });
        this.closing.set(false);
      }
    });
  }
}
