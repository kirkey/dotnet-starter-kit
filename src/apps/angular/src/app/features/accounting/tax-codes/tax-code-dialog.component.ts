import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AccountingService } from '@core/services/accounting.service';
import { TaxCode, ChartOfAccount, AccountType } from '@core/models/accounting.model';

@Component({
  selector: 'app-tax-code-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCheckboxModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.taxCode ? 'Edit Tax Code' : 'Add Tax Code' }}</h2>

    <mat-dialog-content>
      <form [formGroup]="form">
        <div class="form-row">
          <mat-form-field appearance="outline">
            <mat-label>Code</mat-label>
            <input matInput formControlName="taxCode" required>
            <mat-hint>Short code (e.g., GST, VAT, ST)</mat-hint>
            @if (form.get('taxCode')?.hasError('required')) {
              <mat-error>Code is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Rate (%)</mat-label>
            <input matInput type="number" formControlName="rate" required min="0" max="100" step="0.01">
            <span matSuffix>%</span>
            @if (form.get('rate')?.hasError('required')) {
              <mat-error>Rate is required</mat-error>
            }
            @if (form.get('rate')?.hasError('min') || form.get('rate')?.hasError('max')) {
              <mat-error>Rate must be between 0 and 100</mat-error>
            }
          </mat-form-field>
        </div>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Name</mat-label>
          <input matInput formControlName="taxName" required>
          @if (form.get('taxName')?.hasError('required')) {
            <mat-error>Name is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Tax Type</mat-label>
          <mat-select formControlName="taxType" required>
            <mat-option value="Sales">Sales Tax</mat-option>
            <mat-option value="Purchase">Purchase Tax</mat-option>
            <mat-option value="VAT">VAT</mat-option>
            <mat-option value="GST">GST</mat-option>
            <mat-option value="Withholding">Withholding Tax</mat-option>
            <mat-option value="Excise">Excise Tax</mat-option>
            <mat-option value="Other">Other</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Tax Account</mat-label>
          <mat-select formControlName="taxAccountId">
            <mat-option [value]="null">None</mat-option>
            @for (account of liabilityAccounts(); track account.id) {
              <mat-option [value]="account.id">
                {{ account.accountCode }} - {{ account.accountName }}
              </mat-option>
            }
          </mat-select>
          <mat-hint>GL account for tax liability</mat-hint>
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Description</mat-label>
          <textarea matInput formControlName="description" rows="2"></textarea>
        </mat-form-field>

        <mat-checkbox formControlName="isDefault">Default</mat-checkbox>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="cancel()">Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.valid">
        @if (saving) {
          <mat-spinner diameter="20"></mat-spinner>
        } @else {
          {{ data.taxCode ? 'Update' : 'Create' }}
        }
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 400px;
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
export class TaxCodeDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountingService = inject(AccountingService);
  private dialogRef = inject(MatDialogRef<TaxCodeDialogComponent>);
  private snackBar = inject(MatSnackBar);
  public data: { taxCode?: TaxCode } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;
  saving = false;
  liabilityAccounts = signal<ChartOfAccount[]>([]);

  ngOnInit(): void {
    this.initForm();
    this.loadAccounts();
  }

  private initForm(): void {
    const taxCode = this.data.taxCode;
    this.form = this.fb.group({
      taxCode: [taxCode?.taxCode || '', Validators.required],
      taxName: [taxCode?.taxName || '', Validators.required],
      rate: [taxCode?.rate || 0, [Validators.required, Validators.min(0), Validators.max(100)]],
      taxType: [taxCode?.taxType || 'Sales', Validators.required],
      taxAccountId: [taxCode?.taxAccountId || null],
      description: [taxCode?.description || ''],
      isDefault: [taxCode?.isDefault ?? false]
    });
  }

  private loadAccounts(): void {
    this.accountingService.getChartOfAccounts({ 
      pageNumber: 1,
      pageSize: 1000
    }).subscribe({
      next: (result) => this.liabilityAccounts.set(result.items.filter(a => a.accountType === AccountType.Liability)),
      error: (error) => console.error('Error loading accounts:', error)
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const taxCodeData = this.form.value;

    const operation = this.data.taxCode
      ? this.accountingService.updateTaxCode(this.data.taxCode.id, taxCodeData)
      : this.accountingService.createTaxCode(taxCodeData);

    operation.subscribe({
      next: () => {
        this.snackBar.open(
          `Tax code ${this.data.taxCode ? 'updated' : 'created'} successfully`,
          'Close',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Error saving tax code:', error);
        this.snackBar.open('Failed to save tax code', 'Close', { duration: 3000 });
        this.saving = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
