import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { Bank, ChartOfAccount, AccountType } from '@core/models/accounting.model';

@Component({
  selector: 'app-bank-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.bank ? 'Edit Bank Account' : 'Add Bank Account' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <div class="form-section">
          <h4>Account Information</h4>
          
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Bank Code</mat-label>
              <input matInput formControlName="bankCode" required>
              <mat-hint>A unique code for this bank account</mat-hint>
              @if (form.get('bankCode')?.hasError('required')) {
                <mat-error>Bank code is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Bank Name</mat-label>
              <input matInput formControlName="bankName" required>
              @if (form.get('bankName')?.hasError('required')) {
                <mat-error>Bank name is required</mat-error>
              }
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Account Type</mat-label>
              <mat-select formControlName="accountType" required>
                <mat-option value="Checking">Checking</mat-option>
                <mat-option value="Savings">Savings</mat-option>
                <mat-option value="MoneyMarket">Money Market</mat-option>
                <mat-option value="CreditCard">Credit Card</mat-option>
                <mat-option value="LineOfCredit">Line of Credit</mat-option>
                <mat-option value="Other">Other</mat-option>
              </mat-select>
              @if (form.get('accountType')?.hasError('required')) {
                <mat-error>Account type is required</mat-error>
              }
            </mat-form-field>
          </div>

          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Account Number</mat-label>
              <input matInput formControlName="accountNumber" required>
              @if (form.get('accountNumber')?.hasError('required')) {
                <mat-error>Account number is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Routing Number</mat-label>
              <input matInput formControlName="routingNumber">
            </mat-form-field>
          </div>
        </div>

        <div class="form-section">
          <h4>GL Account Mapping</h4>
          
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>GL Account</mat-label>
            <mat-select formControlName="glAccountId">
              @for (account of cashAccounts(); track account.id) {
                <mat-option [value]="account.id">
                  {{ account.accountCode }} - {{ account.accountName }}
                </mat-option>
              }
            </mat-select>
            <mat-hint>Link to a cash account in the Chart of Accounts</mat-hint>
          </mat-form-field>
        </div>

        <div class="form-section">
          <h4>Contact Information</h4>
          
          <div class="form-row">
            <mat-form-field appearance="outline">
              <mat-label>Contact Name</mat-label>
              <input matInput formControlName="contactName">
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Contact Phone</mat-label>
              <input matInput formControlName="contactPhone">
            </mat-form-field>
          </div>

          <mat-checkbox formControlName="isActive">Active</mat-checkbox>
        </div>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="cancel()">Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.valid">
        @if (saving) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data.bank ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 500px;
    }

    .form-section {
      margin-bottom: 24px;
    }

    .form-section h4 {
      margin: 0 0 16px;
      color: #666;
      font-size: 14px;
      text-transform: uppercase;
    }

    .form-row {
      display: flex;
      gap: 16px;
    }

    .form-row mat-form-field {
      flex: 1;
    }

    .full-width {
      width: 100%;
    }

    mat-form-field {
      margin-bottom: 8px;
    }

    mat-checkbox {
      margin-top: 8px;
    }

    mat-spinner {
      display: inline-block;
    }
  `]
})
export class BankDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<BankDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { bank?: Bank } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;
  saving = false;
  cashAccounts = signal<ChartOfAccount[]>([]);

  ngOnInit(): void {
    this.initForm();
    this.loadCashAccounts();
  }

  private initForm(): void {
    const bank = this.data.bank;
    this.form = this.fb.group({
      bankCode: [bank?.bankCode || '', Validators.required],
      bankName: [bank?.bankName || '', Validators.required],
      accountType: [bank?.accountType || 'Checking', Validators.required],
      accountNumber: [bank?.accountNumber || '', Validators.required],
      routingNumber: [bank?.routingNumber || ''],
      glAccountId: [bank?.glAccountId || ''],
      contactName: [bank?.contactName || ''],
      contactPhone: [bank?.contactPhone || ''],
      isActive: [bank?.isActive ?? true]
    });
  }

  private loadCashAccounts(): void {
    this.accountingService.getChartOfAccounts({ 
      pageNumber: 1,
      pageSize: 1000
    }).subscribe({
      next: (result) => this.cashAccounts.set(result.items.filter(a => a.accountType === AccountType.Asset && a.isBankAccount)),
      error: (error) => console.error('Error loading accounts:', error)
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const bankData = this.form.value;

    const operation = this.data.bank
      ? this.accountingService.updateBank(this.data.bank.id, bankData)
      : this.accountingService.createBank(bankData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Bank account ${this.data.bank ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving bank:', error);
        this.snackBar.open('Failed to save bank account', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
