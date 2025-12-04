import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { JournalEntry, AccountingPeriod } from '@core/models/accounting.model';

@Component({
  selector: 'app-reverse-journal-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>
      <mat-icon class="reverse-icon">undo</mat-icon>
      Reverse Journal Entry
    </h2>

    <mat-dialog-content>
      <div class="entry-info">
        <div class="info-item">
          <span class="label">Entry Number:</span>
          <span class="value">{{ entry.entryNumber }}</span>
        </div>
        <div class="info-item">
          <span class="label">Original Date:</span>
          <span class="value">{{ entry.entryDate | date:'shortDate' }}</span>
        </div>
        <div class="info-item">
          <span class="label">Description:</span>
          <span class="value">{{ entry.description }}</span>
        </div>
        <div class="info-item">
          <span class="label">Total Amount:</span>
          <span class="value">{{ entry.totalDebits | currency }}</span>
        </div>
      </div>

      <div class="warning-message">
        <mat-icon>warning</mat-icon>
        <span>Reversing this entry will create a new journal entry with opposite debits and credits.</span>
      </div>

      <form [formGroup]="form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Reversal Date</mat-label>
          <input matInput [matDatepicker]="datePicker" formControlName="reversalDate">
          <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
          <mat-datepicker #datePicker></mat-datepicker>
          @if (form.get('reversalDate')?.hasError('required')) {
            <mat-error>Reversal date is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Accounting Period</mat-label>
          <mat-select formControlName="periodId">
            @for (period of periods(); track period.id) {
              <mat-option [value]="period.id">{{ period.periodName }}</mat-option>
            }
          </mat-select>
          @if (form.get('periodId')?.hasError('required')) {
            <mat-error>Period is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Reason for Reversal</mat-label>
          <textarea matInput formControlName="reason" rows="3"
                    placeholder="Please provide a reason for the reversal"></textarea>
          @if (form.get('reason')?.hasError('required')) {
            <mat-error>Reason is required</mat-error>
          }
        </mat-form-field>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="warn"
              [disabled]="form.invalid || processing()"
              (click)="submit()">
        @if (processing()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          Reverse Entry
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 450px;
    }

    h2 {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .reverse-icon {
      color: #7b1fa2;
    }

    .entry-info {
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
      margin-bottom: 24px;
    }

    .warning-message mat-icon {
      flex-shrink: 0;
    }

    .full-width {
      width: 100%;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class ReverseJournalEntryDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ReverseJournalEntryDialogComponent>);
  private data = inject<{ entry: JournalEntry }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;
  processing = signal(false);
  periods = signal<AccountingPeriod[]>([]);

  get entry(): JournalEntry {
    return this.data.entry;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadPeriods();
  }

  initForm(): void {
    this.form = this.fb.group({
      reversalDate: [new Date(), Validators.required],
      periodId: ['', Validators.required],
      reason: ['', Validators.required]
    });
  }

  loadPeriods(): void {
    this.accountingService.getOpenPeriods().subscribe({
      next: (periods) => {
        this.periods.set(periods);
        // Set current period as default
        const currentPeriod = periods.find(p => p.isCurrent);
        if (currentPeriod) {
          this.form.patchValue({ periodId: currentPeriod.id });
        }
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.processing.set(true);
    const formValue = this.form.value;

    this.accountingService.reverseJournalEntry({
      id: this.entry.id,
      reversalDate: formValue.reversalDate,
      description: formValue.reason
    }).subscribe({
      next: () => {
        this.snackBar.open('Journal entry reversed successfully', 'Close', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error reversing entry:', error);
        this.snackBar.open('Failed to reverse entry', 'Close', { duration: 3000 });
        this.processing.set(false);
      }
    });
  }
}
