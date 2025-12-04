import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
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
import { AccountingPeriod, PeriodStatus } from '@core/models/accounting.model';

@Component({
  selector: 'app-accounting-period-dialog',
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
      {{ isEditMode ? 'Edit Accounting Period' : 'Create Accounting Period' }}
    </h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Period Name</mat-label>
          <input matInput formControlName="periodName" placeholder="e.g., January 2024">
          @if (form.get('periodName')?.hasError('required')) {
            <mat-error>Period name is required</mat-error>
          }
        </mat-form-field>

        <div class="form-row">
          <mat-form-field appearance="outline" class="half-width">
            <mat-label>Fiscal Year</mat-label>
            <mat-select formControlName="fiscalYear">
              @for (year of fiscalYears; track year) {
                <mat-option [value]="year">{{ year }}</mat-option>
              }
            </mat-select>
            @if (form.get('fiscalYear')?.hasError('required')) {
              <mat-error>Fiscal year is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="half-width">
            <mat-label>Period Number</mat-label>
            <mat-select formControlName="periodNumber">
              @for (num of periodNumbers; track num) {
                <mat-option [value]="num">{{ num }}</mat-option>
              }
            </mat-select>
            @if (form.get('periodNumber')?.hasError('required')) {
              <mat-error>Period number is required</mat-error>
            }
          </mat-form-field>
        </div>

        <div class="form-row">
          <mat-form-field appearance="outline" class="half-width">
            <mat-label>Start Date</mat-label>
            <input matInput [matDatepicker]="startPicker" formControlName="startDate">
            <mat-datepicker-toggle matIconSuffix [for]="startPicker"></mat-datepicker-toggle>
            <mat-datepicker #startPicker></mat-datepicker>
            @if (form.get('startDate')?.hasError('required')) {
              <mat-error>Start date is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="half-width">
            <mat-label>End Date</mat-label>
            <input matInput [matDatepicker]="endPicker" formControlName="endDate">
            <mat-datepicker-toggle matIconSuffix [for]="endPicker"></mat-datepicker-toggle>
            <mat-datepicker #endPicker></mat-datepicker>
            @if (form.get('endDate')?.hasError('required')) {
              <mat-error>End date is required</mat-error>
            }
          </mat-form-field>
        </div>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Status</mat-label>
          <mat-select formControlName="status">
            @for (status of statuses; track status) {
              <mat-option [value]="status">{{ status }}</mat-option>
            }
          </mat-select>
        </mat-form-field>

        <div class="checkbox-section">
          <mat-checkbox formControlName="isCurrent">Set as Current Period</mat-checkbox>
          <mat-checkbox formControlName="isAdjustmentPeriod">Adjustment Period</mat-checkbox>
        </div>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary"
              [disabled]="form.invalid || saving()"
              (click)="save()">
        @if (saving()) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ isEditMode ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 450px;
    }

    .form-row {
      display: flex;
      gap: 16px;
    }

    .half-width {
      flex: 1;
    }

    .full-width {
      width: 100%;
    }

    .checkbox-section {
      display: flex;
      flex-direction: column;
      gap: 12px;
      margin: 16px 0;
    }

    mat-dialog-actions {
      padding: 16px 24px;
    }
  `]
})
export class AccountingPeriodDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<AccountingPeriodDialogComponent>);
  private data = inject<{ period?: AccountingPeriod }>(MAT_DIALOG_DATA);
  private accountingService = inject(AccountingService);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;
  saving = signal(false);
  statuses = Object.values(PeriodStatus);

  fiscalYears: number[] = [];
  periodNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]; // 13 for adjustment period

  get isEditMode(): boolean {
    return !!this.data?.period;
  }

  ngOnInit(): void {
    this.generateFiscalYears();
    this.initForm();
  }

  generateFiscalYears(): void {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 5; i <= currentYear + 2; i++) {
      this.fiscalYears.push(i);
    }
  }

  initForm(): void {
    const period = this.data?.period;
    const currentYear = new Date().getFullYear();

    this.form = this.fb.group({
      periodName: [period?.periodName || '', Validators.required],
      fiscalYear: [period?.fiscalYear || currentYear, Validators.required],
      periodNumber: [period?.periodNumber || 1, Validators.required],
      startDate: [period?.startDate ? new Date(period.startDate) : new Date(), Validators.required],
      endDate: [period?.endDate ? new Date(period.endDate) : new Date(), Validators.required],
      status: [period?.status || PeriodStatus.Open],
      isCurrent: [period?.isCurrent || false],
      isAdjustmentPeriod: [period?.isAdjustmentPeriod || false]
    });

    // Auto-generate name when fiscal year or period number changes
    this.form.get('fiscalYear')?.valueChanges.subscribe(() => this.updateName());
    this.form.get('periodNumber')?.valueChanges.subscribe(() => this.updateName());
  }

  updateName(): void {
    const year = this.form.get('fiscalYear')?.value;
    const periodNum = this.form.get('periodNumber')?.value;
    if (year && periodNum) {
      const monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
                          'July', 'August', 'September', 'October', 'November', 'December', 'Adjustment'];
      const name = `${monthNames[periodNum - 1]} ${year}`;
      this.form.patchValue({ periodName: name });
    }
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving.set(true);
    const formValue = this.form.value;

    if (this.isEditMode) {
      // Update existing period
      // TODO: Implement update API
      this.snackBar.open('Period updated successfully', 'Close', { duration: 3000 });
      this.dialogRef.close(true);
    } else {
      this.accountingService.createAccountingPeriod({
        periodName: formValue.periodName,
        fiscalYear: formValue.fiscalYear,
        startDate: formValue.startDate,
        endDate: formValue.endDate,
        isAdjustmentPeriod: formValue.isAdjustmentPeriod
      }).subscribe({
        next: () => {
          this.snackBar.open('Period created successfully', 'Close', { duration: 3000 });
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error creating period:', error);
          this.snackBar.open('Failed to create period', 'Close', { duration: 3000 });
          this.saving.set(false);
        }
      });
    }
  }
}
