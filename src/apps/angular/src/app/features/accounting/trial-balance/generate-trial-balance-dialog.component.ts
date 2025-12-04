import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { AccountingPeriod } from '@core/models/accounting.model';

@Component({
  selector: 'app-generate-trial-balance-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>
      <mat-icon>calculate</mat-icon>
      Generate Trial Balance
    </h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Accounting Period</mat-label>
          <mat-select formControlName="periodId" (selectionChange)="onPeriodChange()">
            @for (period of periods(); track period.id) {
              <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
            }
          </mat-select>
          @if (form.get('periodId')?.hasError('required')) {
            <mat-error>Period is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>As Of Date</mat-label>
          <input matInput [matDatepicker]="datePicker" formControlName="asOfDate">
          <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
          <mat-datepicker #datePicker></mat-datepicker>
          @if (form.get('asOfDate')?.hasError('required')) {
            <mat-error>As of date is required</mat-error>
          }
        </mat-form-field>

        <div class="options-section">
          <h4>Options</h4>
          <mat-checkbox formControlName="includeZeroBalances">
            Include accounts with zero balance
          </mat-checkbox>
          <mat-checkbox formControlName="includeInactiveAccounts">
            Include inactive accounts
          </mat-checkbox>
          <mat-checkbox formControlName="showSubAccounts">
            Show sub-accounts
          </mat-checkbox>
        </div>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary"
              [disabled]="form.invalid || generating()"
              (click)="generate()">
        @if (generating()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          Generate
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

    mat-dialog-content {
      min-width: 400px;
    }

    .full-width {
      width: 100%;
    }

    .options-section {
      margin: 16px 0;
      padding: 16px;
      background-color: #f5f5f5;
      border-radius: 8px;
    }

    .options-section h4 {
      margin: 0 0 12px;
    }

    .options-section mat-checkbox {
      display: block;
      margin-bottom: 8px;
    }

    .options-section mat-checkbox:last-child {
      margin-bottom: 0;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class GenerateTrialBalanceDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<GenerateTrialBalanceDialogComponent>);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;
  generating = signal(false);
  periods = signal<AccountingPeriod[]>([]);

  ngOnInit(): void {
    this.initForm();
    this.loadPeriods();
  }

  initForm(): void {
    this.form = this.fb.group({
      periodId: ['', Validators.required],
      asOfDate: [new Date(), Validators.required],
      includeZeroBalances: [false],
      includeInactiveAccounts: [false],
      showSubAccounts: [true]
    });
  }

  loadPeriods(): void {
    this.accountingService.getAccountingPeriods({ pageNumber: 1, pageSize: 100 }).subscribe({
      next: (result) => {
        this.periods.set(result.items);
        // Set current period as default
        const currentPeriod = result.items.find(p => p.isCurrent);
        if (currentPeriod) {
          this.form.patchValue({
            periodId: currentPeriod.id
          });
        }
      }
    });
  }

  onPeriodChange(): void {
    const periodId = this.form.get('periodId')?.value;
    const period = this.periods().find(p => p.id === periodId);
    if (period) {
      this.form.patchValue({
        asOfDate: new Date(period.endDate)
      });
    }
  }

  generate(): void {
    if (this.form.invalid) return;

    this.generating.set(true);
    const formValue = this.form.value;

    this.accountingService.generateTrialBalance({
      periodId: formValue.periodId,
      asOfDate: formValue.asOfDate,
      includeZeroBalances: formValue.includeZeroBalances
    }).subscribe({
      next: () => {
        this.snackBar.open('Trial balance generated successfully', 'Close', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error generating trial balance:', error);
        this.snackBar.open('Failed to generate trial balance', 'Close', { duration: 3000 });
        this.generating.set(false);
      }
    });
  }
}
